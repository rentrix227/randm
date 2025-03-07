using System;
using System.Linq;
using System.Text;
using Aquiris.Ballistic.Game.Utility;
using Aquiris.Ballistic.Network.Configuration.Steam;
using Aquiris.Ballistic.Network.Connection.Events;
using Aquiris.Ballistic.Network.Connection.Steam;
using Aquiris.Ballistic.Network.Game.Steam;
using Aquiris.Ballistic.Network.Gameplay.GameMode;
using Aquiris.Ballistic.Network.Transmission.Packet;
using Aquiris.Ballistic.Network.Transmission.Packet.Thrift;
using Aquiris.Ballistic.Network.Transport.Connection.Events;
using Aquiris.Ballistic.Network.Transport.Gameplay.State;
using Aquiris.Ballistic.Utils;
using Aquiris.Services;
using Aquiris.Services.ItemModel.ConfigItemModel;
using Steamworks;
using UnityEngine;

namespace Aquiris.Ballistic.Network
{
	// Token: 0x02000344 RID: 836
	internal class BallisticHost
	{
		// Token: 0x060011D8 RID: 4568 RVA: 0x0000E9F7 File Offset: 0x0000CBF7
		internal BallisticHost(SteamConfig config)
		{
			this._config = config;
			this._timeToUpdateData = 0f;
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060011D9 RID: 4569 RVA: 0x0000EA1C File Offset: 0x0000CC1C
		internal ulong CurrentGameMap
		{
			get
			{
				return (ulong)this.HostManager.GetGameModeHost().GetGameConfig().GameMap;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060011DA RID: 4570 RVA: 0x0000EA33 File Offset: 0x0000CC33
		internal EGameMode CurrentGameMode
		{
			get
			{
				return this.HostManager.GetGameModeHost().GetGameConfig().GameMode;
			}
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x00065170 File Offset: 0x00063370
		internal void StartHost()
		{
			this.ConnectionHostManager = new SteamConnectionHostManager(this._config);
			this.ConnectionHostManager.OnHostReady += this.OnHostReadyCallback;
			this.ConnectionHostManager.OnHostFail += this.OnHostFailCallback;
			this.HostManager = new SteamGameHostManager(this.ConnectionHostManager);
			this.HostManager.StartHost();
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x000651D8 File Offset: 0x000633D8
		internal void Close()
		{
			if (this.ConnectionHostManager != null)
			{
				this.ConnectionHostManager.OnHostReady -= this.OnHostReadyCallback;
				this.ConnectionHostManager.OnHostFail -= this.OnHostFailCallback;
			}
			if (this.HostManager == null)
			{
				return;
			}
			this.HostManager.CloseHost();
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x0000EA4A File Offset: 0x0000CC4A
		internal void StartGameMode()
		{
			if (this.HostManager == null)
			{
				return;
			}
			this.HostManager.StartGameMode();
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x00065238 File Offset: 0x00063438
		internal void StartGameMode(GameMapConfig gameMap, EGameMode mode)
		{
			if (this.HostManager != null)
			{
				if (!ServiceProvider.GetService<GameMapModeConfigService>().GetAvaliableGameMapConfigList().Contains(gameMap))
				{
					Debug.LogError("Invalid game map!");
					return;
				}
				if (!ServiceProvider.GetService<GameMapModeConfigService>().GetAvailableGameModeList(gameMap).Contains(mode))
				{
					Debug.LogError("Invalid game mode!");
					return;
				}
				this._config.GameMap = gameMap.MapId;
				this._config.GameMode = mode;
				this.HostManager.UpdateConfig(this._config);
				this.HostManager.StartGameMode();
			}
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x0000EA63 File Offset: 0x0000CC63
		internal CSteamID GetHostId()
		{
			return this.HostManager.GetHostID();
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x0000EA70 File Offset: 0x0000CC70
		internal GameModeHost GetGameModeHost()
		{
			return this.HostManager.GetGameModeHost();
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x000652CC File Offset: 0x000634CC
		public void UpdateGameMode(float deltaTime)
		{
			if (this.HostManager != null)
			{
				this.HostManager.UpdateGameMode(deltaTime);
				this._timeToUpdateData -= deltaTime;
				if (this._timeToUpdateData <= 0f)
				{
					this._timeToUpdateData = 0.5f;
					this.ConnectionHostManager.CurrentConfig.NumPlayers = (uint)this.ConnectionHostManager.ClientCount(EClientMode.PLAYER);
					this.ConnectionHostManager.CurrentConfig.NumSpectators = (uint)this.ConnectionHostManager.ClientCount(EClientMode.SPECTATOR);
					this.UpdateServerTags();
				}
			}
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x00065358 File Offset: 0x00063558
		internal void UpdateServerTags()
		{
			this.build.Length = 0;
			this.build.Append(BytePacker.GetBooleanChar(this.IsTerminating));
			this.build.Append(BytePacker.GetHex64Char(this.ConnectionHostManager.CurrentConfig.NumPlayers));
			this.build.Append(BytePacker.GetHex64Char(this.ConnectionHostManager.CurrentConfig.MaxPlayers));
			this.build.Append(BytePacker.GetHex64Char(this.ConnectionHostManager.CurrentConfig.NumSpectators));
			this.build.Append(BytePacker.GetHex64Char(this.ConnectionHostManager.CurrentConfig.MaxSpectators));
			this.build.Append(',');
			this.build.Append(this.ConnectionHostManager.CurrentConfig.MaxPing);
			this.build.Append(',');
			this.build.Append((!string.IsNullOrEmpty(this.ConnectionHostManager.CurrentConfig.Password)) ? Crypto.ComputeMd5Hash(this.ConnectionHostManager.CurrentConfig.Password) : "null");
			this.build.Append(',');
			if (this.GetGameModeHost() != null && this.GetGameModeHost().GetClientCount() > 0)
			{
				this.build.Append(BytePacker.PackString(from t in this.GetGameModeHost().ListClientCommonMetaData()
					select t.User));
			}
			else
			{
				this.build.Append("null");
			}
			string text = this.build.ToString();
			if (text != this._lastServerTags)
			{
				this._lastServerTags = text;
				SteamGameServer.SetGameTags(text);
			}
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x0000EA7D File Offset: 0x0000CC7D
		private void OnHostReadyCallback(object sender, HostReadyEventArgs e)
		{
			if (this.OnHostReady != null)
			{
				this.OnHostReady();
			}
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x0000EA97 File Offset: 0x0000CC97
		private void OnHostFailCallback(object sender, HostFailEventArgs e)
		{
			if (this.OnHostFail != null)
			{
				this.OnHostFail();
			}
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x00065534 File Offset: 0x00063734
		internal void Terminate(ServerTerminationReason reason)
		{
			ThriftPacket thriftPacket = new ThriftPacket
			{
				TransportType = ETransportType.ReliableOrdered,
				TransportChannel = ETransportChannel.GAME_CONTROL,
				TransportObject = new ServerTerminationEvent
				{
					Reason = reason
				}
			};
			this.ConnectionHostManager.BroadcastPacket(thriftPacket);
			this.HostManager.GetGameModeHost().Terminate();
		}

		// Token: 0x04001690 RID: 5776
		internal Action OnHostReady;

		// Token: 0x04001691 RID: 5777
		internal Action OnHostFail;

		// Token: 0x04001692 RID: 5778
		private readonly SteamConfig _config;

		// Token: 0x04001693 RID: 5779
		internal SteamConnectionHostManager ConnectionHostManager;

		// Token: 0x04001694 RID: 5780
		internal SteamGameHostManager HostManager;

		// Token: 0x04001695 RID: 5781
		private StringBuilder build = new StringBuilder();

		// Token: 0x04001696 RID: 5782
		internal bool IsTerminating;

		// Token: 0x04001697 RID: 5783
		private float _timeToUpdateData;

		// Token: 0x04001698 RID: 5784
		private const float _dataUpdatePeriod = 0.5f;

		// Token: 0x04001699 RID: 5785
		private string _lastServerTags;
	}
}
