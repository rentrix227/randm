using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Aquiris.Ballistic.Game.Services;
using Aquiris.Ballistic.Game.Utility;
using Aquiris.Ballistic.Network;
using Aquiris.Ballistic.Network.Configuration.Steam;
using Aquiris.Ballistic.Network.Gameplay.GameMode;
using Aquiris.Ballistic.Network.Transport.Connection.Events;
using Aquiris.Ballistic.Network.Transport.Gameplay.State;
using Aquiris.Ballistic.Network.Utils;
using Aquiris.Ballistic.Utils;
using Aquiris.Services;
using Aquiris.Services.ItemModel.ConfigItemModel;
using UnityEngine;

namespace Aquiris.Ballistic.Dedicated
{
	// Token: 0x02000059 RID: 89
	public class BallisticDedicatedServer
	{
		// Token: 0x06000083 RID: 131 RVA: 0x0001570C File Offset: 0x0001390C
		public void Start()
		{
			Application.runInBackground = true;
			this._configParser = new ConfigParser("config.txt");
			Debug.Log(this._configParser);
			this._updatePeriod = Mathf.RoundToInt(1000f / (float)this._configParser.UpdateRate);
			Application.targetFrameRate = this._configParser.UpdateRate;
			if (this._configParser.Error)
			{
				Debug.LogError("Unable to parse config.txt");
			}
			else
			{
				EConnectionQuality econnectionQuality;
				switch (this._configParser.DedicatedBroadcast)
				{
				case 0:
					econnectionQuality = EConnectionQuality.P2PBroadcast;
					break;
				case 1:
					econnectionQuality = EConnectionQuality.DedicatedBroadcastOnDemand;
					break;
				case 2:
					econnectionQuality = EConnectionQuality.DedicatedBroadcastAlways;
					break;
				default:
					econnectionQuality = EConnectionQuality.P2PBroadcast;
					break;
				}
				SteamConfig steamConfig = new SteamConfig
				{
					MaxPlayers = (uint)this._configParser.MaxPlayers,
					MaxSpectators = (uint)this._configParser.MaxSpectators,
					MaxPing = this._configParser.MaxPing,
					ServerName = this._configParser.ServerName,
					SteamPort = 8766,
					GamePort = (ushort)this._configParser.ServerPort,
					GameMode = this._configParser.GameMode,
					GameMap = (ulong)this._configParser.GameMap,
					QueryPort = (ushort)this._configParser.QueryPort,
					PrivateMatch = false,
					DedicatedServer = true,
					OfficialServer = !string.IsNullOrEmpty(StatisticsConstants.GetOfficialAPI),
					OfficialApi = StatisticsConstants.GetOfficialAPI,
					Autobalance = (this._configParser.Autobalance == 1),
					BroadcastQuality = econnectionQuality,
					MatchTime = (uint)(this._configParser.MatchTime * 60),
					TeamDeathMatch = (uint)this._configParser.Rounds,
					RoundTime = (uint)this._configParser.RoundTime,
					WarmUpTime = this._configParser.WarmUpTime,
					BannerUrl = this._configParser.BannerUrl,
					ClickUrl = this._configParser.ClickUrl,
					Password = this._configParser.Password,
					Owner = (ulong)this._configParser.Owner
				};
				if (steamConfig.OfficialServer)
				{
					Debug.Log("Starting Ballistic Overkill Official Server " + BallisticVersion.DEDICATED_SERVER_VERSION);
				}
				else
				{
					Debug.Log("Starting Ballistic Overkill Dedicated Server " + BallisticVersion.DEDICATED_SERVER_VERSION);
				}
				this._configGameMap = ServiceProvider.GetService<GameMapModeConfigService>().FindGameMapConfig(steamConfig.GameMap);
				if (this._configGameMap == null)
				{
					Debug.LogError("Invalid game map!" + this._configParser.GameMap);
					return;
				}
				if (!ServiceProvider.GetService<GameMapModeConfigService>().GetAvailableGameModeList(this._configGameMap).Contains(this._configParser.GameMode))
				{
					Debug.LogError("Invalid game mode!" + this._configParser.GameMode);
					return;
				}
				this._host = new BallisticHost(steamConfig);
				BallisticHost host = this._host;
				host.OnHostReady = (Action)Delegate.Combine(host.OnHostReady, new Action(this.OnHostReadyCallback));
				this._host.StartHost();
			}
			this._bodimPipeId = this._configParser.PipeId;
			if (!string.IsNullOrEmpty(this._bodimPipeId))
			{
				new Thread(new ThreadStart(this.PipeListener)).Start();
			}
			ServiceProvider.GetService<EventProxy>();
			BallisticDedicatedServer.IsRunning = true;
			ServiceProvider.GetService<TelemetryService>();
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000029A5 File Offset: 0x00000BA5
		public void OnDestroy()
		{
			this.DestroyServer();
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000029A5 File Offset: 0x00000BA5
		public void OnApplicationQuit()
		{
			this.DestroyServer();
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00015A7C File Offset: 0x00013C7C
		private void UpdateThread()
		{
			double num = TimeUtils.getUTCTimeAsDouble();
			for (;;)
			{
				double utctimeAsDouble = TimeUtils.getUTCTimeAsDouble();
				float num2 = (float)(0.001 * (utctimeAsDouble - num));
				this.DoUpdate(num2);
				double utctimeAsDouble2 = TimeUtils.getUTCTimeAsDouble();
				int num3 = (int)Math.Round(utctimeAsDouble2 - utctimeAsDouble);
				int num4 = this._updatePeriod - num3;
				num = utctimeAsDouble;
				if (num4 > 0)
				{
					Thread.Sleep(num4);
				}
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000029AD File Offset: 0x00000BAD
		public void Update()
		{
			this.DoUpdate(Time.deltaTime);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000029BA File Offset: 0x00000BBA
		private void DoUpdate(float deltaTime)
		{
			if (this._host != null)
			{
				this._host.UpdateGameMode(deltaTime);
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000029D3 File Offset: 0x00000BD3
		private void DestroyServer()
		{
			if (this._host == null)
			{
				return;
			}
			this._host.Close();
			this._host = null;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00015ADC File Offset: 0x00013CDC
		private void StartServer()
		{
			if (this._host == null)
			{
				Debug.LogWarning("Server not created!");
				return;
			}
			this._host.StartGameMode(this._configGameMap, this._configParser.GameMode);
			GameModeHost gameModeHost = this._host.HostManager.GetGameModeHost();
			foreach (KeyValuePair<string, List<long>> keyValuePair in this._configParser.NicknameTags)
			{
				string key = keyValuePair.Key;
				List<long> value = keyValuePair.Value;
				foreach (long num in value)
				{
					gameModeHost.AddNicknameTag(key, num);
				}
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000029F3 File Offset: 0x00000BF3
		private void OnHostReadyCallback()
		{
			this.StartServer();
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000029FB File Offset: 0x00000BFB
		private void Terminate(ServerTerminationReason reason)
		{
			if (this._host.IsTerminating)
			{
				return;
			}
			this._host.IsTerminating = true;
			this._host.UpdateServerTags();
			this._host.Terminate(reason);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00015BD8 File Offset: 0x00013DD8
		private void PipeListener()
		{
			try
			{
				Debug.Log("[BODIM] Opening BODIM pipe...");
				FilePipe filePipe = new FilePipe(this._bodimPipeId);
				Debug.Log("[BODIM] Waiting for SYNC message...");
				string text;
				do
				{
					text = filePipe.Read();
				}
				while (text != null && !text.StartsWith("SYNC"));
				Debug.Log("[BODIM] Received SYNC message!");
				while ((text = filePipe.Read()) != null)
				{
					if (text.StartsWith("TERMINATE"))
					{
						Debug.Log("[BODIM] Received termination message!");
						string[] array = text.Split(new char[] { ' ' });
						ServerTerminationReason serverTerminationReason = ServerTerminationReason.NONE;
						if (array.Length > 1)
						{
							try
							{
								serverTerminationReason = (ServerTerminationReason)Enum.Parse(typeof(ServerTerminationReason), array[1]);
							}
							catch (Exception)
							{
								serverTerminationReason = ServerTerminationReason.NONE;
							}
						}
						this.Terminate(serverTerminationReason);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[BODIM] Error listening to BODIM pipe: " + ex);
			}
		}

		// Token: 0x040002B0 RID: 688
		public static bool IsRunning;

		// Token: 0x040002B1 RID: 689
		private const string _configFile = "config.txt";

		// Token: 0x040002B2 RID: 690
		private ConfigParser _configParser;

		// Token: 0x040002B3 RID: 691
		private GameMapConfig _configGameMap;

		// Token: 0x040002B4 RID: 692
		private BallisticHost _host;

		// Token: 0x040002B5 RID: 693
		private string _bodimPipeId;

		// Token: 0x040002B6 RID: 694
		private int _updatePeriod;
	}
}
