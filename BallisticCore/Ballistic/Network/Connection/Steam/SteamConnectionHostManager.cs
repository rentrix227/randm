using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Aquiris.Ballistic.Game.Utility;
using Aquiris.Ballistic.Network.Address.Steam;
using Aquiris.Ballistic.Network.Configuration.Steam;
using Aquiris.Ballistic.Network.Transmission.Packet;
using Aquiris.Ballistic.Network.Transmission.Packet.Thrift;
using Aquiris.Ballistic.Network.Transmission.Steam;
using Aquiris.Ballistic.Network.Transport.Connection.Events;
using Aquiris.Ballistic.Network.Transport.Connection.Requests;
using Aquiris.Ballistic.Network.Transport.Connection.Responses;
using Aquiris.Ballistic.Network.Transport.Gameplay.State;
using Aquiris.Ballistic.Utils;
using Steamworks;
using Thrift.Protocol;
using UnityEngine;

namespace Aquiris.Ballistic.Network.Connection.Steam
{
	// Token: 0x02000357 RID: 855
	internal sealed class SteamConnectionHostManager : ConnectionHostManager<ThriftPacket, CSteamID, TBase, ETransportChannel, SteamClientInfo, SteamHostTransmitter, SteamConfig>
	{
		// Token: 0x06001289 RID: 4745 RVA: 0x0000EFC7 File Offset: 0x0000D1C7
		internal SteamConnectionHostManager(SteamConfig config)
			: base(new SteamHostTransmitter(config), ETransportChannel.KEEP_ALIVE, ETransportChannel.CONNECTION_MANAGEMENT, config)
		{
			this.IOInternalStorage(false);
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x0000EFE6 File Offset: 0x0000D1E6
		internal override void StartHost()
		{
			SteamCallbacks.ValidateAuthTicketResponse_t.RegisterCallback(new Action<ValidateAuthTicketResponse_t>(this.OnValidateTicketResponse));
			base.StartHost();
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x0000EFFF File Offset: 0x0000D1FF
		internal override void CloseHost()
		{
			SteamCallbacks.ValidateAuthTicketResponse_t.UnregisterCallback(new Action<ValidateAuthTicketResponse_t>(this.OnValidateTicketResponse));
			base.CloseHost();
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x00066B7C File Offset: 0x00064D7C
		protected override void AuthenticateUser(ThriftPacket packet, SteamClientInfo sender)
		{
			JoinGameRequest joinGameRequest = (JoinGameRequest)packet.TransportObject;
			bool flag = joinGameRequest.Version == BallisticVersion.VERSION;
			if (flag)
			{
				if (string.IsNullOrEmpty(base.CurrentConfig.Password) || Crypto.ComputeMd5Hash(base.CurrentConfig.Password) == joinGameRequest.Password)
				{
					SteamClientFactory.GetClient(sender.Info).clientMode = joinGameRequest.ClientMode;
					SteamGameServer.BeginAuthSession(joinGameRequest.AuthTicket, joinGameRequest.AuthTicket.Length, sender.Info);
				}
				else
				{
					base.AuthenticationFail(sender, LeaveGameMotivation.WRONGPASSWORD);
				}
			}
			else
			{
				base.AuthenticationFail(sender, LeaveGameMotivation.NONE);
			}
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x00066C2C File Offset: 0x00064E2C
		protected override void CheckMaxPlayers(ThriftPacket packet, SteamClientInfo sender)
		{
			JoinGameRequest joinGameRequest = (JoinGameRequest)packet.TransportObject;
			if (!this._canConnect)
			{
				base.AuthenticationFail(sender, LeaveGameMotivation.NONE);
			}
			else if (joinGameRequest.ClientMode == EClientMode.PLAYER && (long)this.ClientCount(EClientMode.PLAYER) < (long)((ulong)base.CurrentConfig.MaxPlayers))
			{
				this.AuthenticateUser(packet, sender);
			}
			else if (joinGameRequest.ClientMode == EClientMode.PLAYER && (long)this.ClientCount(EClientMode.SPECTATOR) < (long)((ulong)base.CurrentConfig.MaxSpectators))
			{
				joinGameRequest.ClientMode = EClientMode.SPECTATOR;
				this.AuthenticateUser(packet, sender);
			}
			else if (joinGameRequest.ClientMode == EClientMode.SPECTATOR && (long)this.ClientCount(EClientMode.SPECTATOR) < (long)((ulong)base.CurrentConfig.MaxSpectators))
			{
				this.AuthenticateUser(packet, sender);
			}
			else
			{
				base.AuthenticationFail(sender, LeaveGameMotivation.SERVERFULL);
			}
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x00066D00 File Offset: 0x00064F00
		internal int ClientCount(EClientMode mode)
		{
			int num = 0;
			foreach (SteamClientInfo steamClientInfo in this.m_knownClientList.Values)
			{
				if (steamClientInfo.clientMode == mode)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x00066D70 File Offset: 0x00064F70
		private void OnValidateTicketResponse(ValidateAuthTicketResponse_t p_data)
		{
			SteamClientInfo client = SteamClientFactory.GetClient(p_data.m_SteamID);
			client.IsAppOwner = p_data.m_SteamID.m_SteamID == p_data.m_OwnerSteamID.m_SteamID;
			if (p_data.m_eAuthSessionResponse == 9 || p_data.m_eAuthSessionResponse == 3)
			{
				base.AuthenticationFail(client, LeaveGameMotivation.VACBANNED);
				return;
			}
			if (SteamConnectionHostManager._internalStorage.Blacklist.Contains(p_data.m_SteamID.m_SteamID))
			{
				this.m_connectionHostManager.KickPlayer(SteamClientFactory.GetClient(76561198870737234L), LeaveGameMotivation.NONE, string.Empty);
				base.AuthenticationFail(client, LeaveGameMotivation.HOST_BLACKLISTED);
				return;
			}
			if (p_data.m_eAuthSessionResponse == null)
			{
				base.AuthenticationSuccess(client);
				return;
			}
			base.AuthenticationFail(client, LeaveGameMotivation.NONE);
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x0000F018 File Offset: 0x0000D218
		protected override void OnClientDisconnected(SteamClientInfo basicClient)
		{
			base.OnClientDisconnected(basicClient);
			SteamGameServer.EndAuthSession(basicClient.Info);
			SteamClientFactory.FreeClient(basicClient.Info);
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x00066E20 File Offset: 0x00065020
		protected override ThriftPacket GetFailConnectionResponsePacket(LeaveGameMotivation motivation)
		{
			return new ThriftPacket
			{
				TransportChannel = ETransportChannel.CONNECTION_MANAGEMENT,
				TransportObject = new FailConnectionResponse
				{
					Motivation = (sbyte)motivation
				},
				TransportType = ETransportType.Reliable
			};
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x0000F037 File Offset: 0x0000D237
		protected override bool IsJoinGameRequestPacket(ThriftPacket packet)
		{
			return packet.TransportObject is JoinGameRequest;
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x00066E58 File Offset: 0x00065058
		protected override string GetUserAlias(ThriftPacket packet)
		{
			if (this.IsJoinGameRequestPacket(packet))
			{
				JoinGameRequest joinGameRequest = packet.TransportObject as JoinGameRequest;
				return joinGameRequest.UserAlias;
			}
			return "Unknown";
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x0000F04C File Offset: 0x0000D24C
		protected override bool IsLeaveGameRequestPacket(ThriftPacket packet)
		{
			return packet.TransportObject is LeaveGameRequest;
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x00066E8C File Offset: 0x0006508C
		protected override ThriftPacket GetJoinGameEventPacket(SteamClientInfo basicInfo)
		{
			ThriftPacket thriftPacket = new ThriftPacket();
			JoinGameEvent joinGameEvent = new JoinGameEvent
			{
				User = (long)basicInfo.Info.m_SteamID,
				UserAlias = basicInfo.Alias
			};
			thriftPacket.TransportObject = joinGameEvent;
			thriftPacket.TransportType = ETransportType.Reliable;
			thriftPacket.TransportChannel = ETransportChannel.CONNECTION_MANAGEMENT;
			return thriftPacket;
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x00066EE0 File Offset: 0x000650E0
		protected override ThriftPacket GetLeaveGameEventPacket(SteamClientInfo basicInfo, LeaveGameMotivation motivation)
		{
			return new ThriftPacket
			{
				TransportObject = new LeaveGameEvent
				{
					User = (long)basicInfo.Info.m_SteamID,
					Motivation = (short)motivation
				},
				TransportType = ETransportType.Reliable,
				TransportChannel = ETransportChannel.CONNECTION_MANAGEMENT
			};
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x00066F30 File Offset: 0x00065130
		protected override ThriftPacket getKeepAliveRequestPacket()
		{
			return new ThriftPacket
			{
				TransportChannel = ETransportChannel.KEEP_ALIVE,
				TransportObject = new KeepAliveRequest(),
				TransportType = ETransportType.Reliable
			};
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x00066F60 File Offset: 0x00065160
		protected override ThriftPacket getKeepAliveResponsePacket()
		{
			return new ThriftPacket
			{
				TransportChannel = ETransportChannel.KEEP_ALIVE,
				TransportObject = new KeepAliveResponse(),
				TransportType = ETransportType.Reliable
			};
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x0000EF33 File Offset: 0x0000D133
		protected override bool isKeepAliveRequestPacket(ThriftPacket p_packet)
		{
			return p_packet.TransportObject is KeepAliveRequest;
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x0000EF43 File Offset: 0x0000D143
		protected override bool isKeepAliveResponsePacket(ThriftPacket p_packet)
		{
			return p_packet.TransportObject is KeepAliveResponse;
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x0000F05C File Offset: 0x0000D25C
		internal override CSteamID GetHostID()
		{
			return this.m_transmitter.GetHostID();
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600129C RID: 4764 RVA: 0x0000F069 File Offset: 0x0000D269
		internal int ConnectedClients
		{
			get
			{
				return this.m_knownClientList.Count;
			}
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x0000F076 File Offset: 0x0000D276
		internal void SetConnectionState(bool canConnect)
		{
			this._canConnect = canConnect;
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x00066F90 File Offset: 0x00065190
		internal void IOInternalStorage(bool save)
		{
			if (!save)
			{
				if (File.Exists("BallisticConfiguration.serverdata"))
				{
					try
					{
						string text = File.ReadAllText("BallisticConfiguration.serverdata");
						SteamConnectionHostManager._internalStorage = ConversionUtil.ReadUnityJson<SteamConnectionHostManager.GameServerInternalData>(text);
						if (SteamConnectionHostManager._internalStorage.Version != 1)
						{
							SteamConnectionHostManager._internalStorage = new SteamConnectionHostManager.GameServerInternalData();
						}
					}
					catch (Exception ex)
					{
						Debug.Log(ex.Message);
						SteamConnectionHostManager._internalStorage = new SteamConnectionHostManager.GameServerInternalData();
					}
				}
			}
			else
			{
				try
				{
					File.WriteAllText("BallisticConfiguration.serverdata", ConversionUtil.WriteUnityJson(SteamConnectionHostManager._internalStorage), Encoding.UTF8);
				}
				catch (Exception ex2)
				{
					Debug.Log(ex2.Message);
				}
			}
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x0000F07F File Offset: 0x0000D27F
		internal SteamConnectionHostManager.GameServerInternalData GetStorage()
		{
			return SteamConnectionHostManager._internalStorage;
		}

		// Token: 0x040016E8 RID: 5864
		private const string _storagePathName = "BallisticConfiguration.serverdata";

		// Token: 0x040016E9 RID: 5865
		private static SteamConnectionHostManager.GameServerInternalData _internalStorage = new SteamConnectionHostManager.GameServerInternalData();

		// Token: 0x040016EA RID: 5866
		private bool _canConnect = true;

		// Token: 0x040016EB RID: 5867
		protected SteamConnectionHostManager m_connectionHostManager;

		// Token: 0x02000358 RID: 856
		public class GameServerInternalData
		{
			// Token: 0x040016EC RID: 5868
			public int Version = 1;

			// Token: 0x040016ED RID: 5869
			public List<ulong> Blacklist = new List<ulong>();

			// Token: 0x040016EE RID: 5870
			public List<ulong> Prioritylist = new List<ulong>();
		}
	}
}
