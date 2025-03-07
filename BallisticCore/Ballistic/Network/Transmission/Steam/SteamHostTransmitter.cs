using System;
using System.Collections.Generic;
using System.Linq;
using Aquiris.Ballistic.Game.Utility;
using Aquiris.Ballistic.Network.Address.Steam;
using Aquiris.Ballistic.Network.Configuration.Steam;
using Aquiris.Ballistic.Network.Transmission.Mapper.Thrift;
using Aquiris.Ballistic.Network.Transmission.Packet;
using Aquiris.Ballistic.Network.Transmission.Packet.Thrift;
using Aquiris.Ballistic.Utils;
using Aquiris.Services;
using Aquiris.Services.ItemModel.ConfigItemModel;
using Steamworks;
using Thrift.Protocol;
using UnityEngine;

namespace Aquiris.Ballistic.Network.Transmission.Steam
{
	// Token: 0x020003CD RID: 973
	internal class SteamHostTransmitter : BasicHostTransmitter<ThriftPacket, CSteamID, TBase, ETransportChannel, SteamClientInfo, SteamConfig>
	{
		// Token: 0x06001565 RID: 5477 RVA: 0x000107EC File Offset: 0x0000E9EC
		internal SteamHostTransmitter(SteamConfig config)
			: base(config)
		{
			this.m_packetMapper = new ThriftByteArrayPacketMapper();
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x00074050 File Offset: 0x00072250
		~SteamHostTransmitter()
		{
			this.Close();
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x0001080B File Offset: 0x0000EA0B
		internal override void CloseClient(SteamClientInfo p_client)
		{
			SteamGameServerNetworking.CloseP2PSessionWithUser(p_client.Info);
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x00074080 File Offset: 0x00072280
		internal override void Start()
		{
			string text;
			if (base.CurrentConfig.OfficialServer)
			{
				Debug.Log("Official Server Enabled");
				text = BallisticVersion.OFFICIAL_SERVER_VERSION;
			}
			else if (base.CurrentConfig.DedicatedServer)
			{
				text = BallisticVersion.DEDICATED_SERVER_VERSION;
			}
			else
			{
				text = BallisticVersion.CLIENT_VERSION;
			}
			if (GameServer.Init(0U, base.CurrentConfig.SteamPort, base.CurrentConfig.GamePort, base.CurrentConfig.QueryPort, 1, text))
			{
				SteamCallbacks.SteamServersConnected_t.RegisterCallback(new Action<SteamServersConnected_t>(this.OnGameServerConnected));
				SteamCallbacks.SteamServersDisconnected_t.RegisterCallback(new Action<SteamServersDisconnected_t>(this.OnGameServerDisconnected));
				SteamCallbacks.SteamServerConnectFailure_t.RegisterCallback(new Action<SteamServerConnectFailure_t>(this.OnGameServerConnectionFail));
				SteamCallbacks.GSPolicyResponse_t.RegisterCallback(new Action<GSPolicyResponse_t>(this.OnPolicyResponse));
				SteamCallbacks.P2PSessionConnectFail_t.RegisterCallback(new Action<P2PSessionConnectFail_t>(this.OnP2PSessionConnectFail), true);
				SteamCallbacks.P2PSessionRequest_t.RegisterCallback(new Action<P2PSessionRequest_t>(this.OnP2PSessionRequest), true);
				this.m_loginElapsedTime = 0f;
				SteamGameServer.LogOnAnonymous();
				this.m_creatingServerStep = SteamHostTransmitter.EServerStateStep.LOGGING_IN;
				return;
			}
			Debug.LogWarning("GameServer.Init failed");
			this.m_creatingServerStep = SteamHostTransmitter.EServerStateStep.NONE;
			base.DispatchOnHostFail();
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x0007418C File Offset: 0x0007238C
		internal override bool SendPacket(ThriftPacket p_packet, SteamClientInfo p_networkAddress)
		{
			if (this.m_creatingServerStep == SteamHostTransmitter.EServerStateStep.RUNNING)
			{
				byte[] array = this.m_packetMapper.compressPacket(p_packet);
				EP2PSend ep2PSend = 0;
				ETransportType transportType = p_packet.TransportType;
				if (transportType != ETransportType.Reliable)
				{
					if (transportType == ETransportType.ReliableOrdered)
					{
						ep2PSend = 3;
					}
				}
				else
				{
					ep2PSend = 2;
				}
				bool flag = SteamGameServerNetworking.SendP2PPacket(p_networkAddress.Info, array, (uint)array.Length, ep2PSend, 0);
				if (!flag)
				{
					Debug.LogError("P2P packet send host result: " + flag);
				}
				return flag;
			}
			return false;
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x0007420C File Offset: 0x0007240C
		private void VerifyLoginTimeout(float p_deltaTime)
		{
			this.m_loginElapsedTime += p_deltaTime;
			if (!SteamGameServer.BLoggedOn())
			{
				if (this.m_loginElapsedTime > this.LOGIN_MAX_TIME)
				{
					Debug.LogWarning("SteamGameServer.LogOnAnonymous timed out");
					this.Close();
					base.DispatchOnHostFail();
				}
			}
			else
			{
				SteamGameServer.SetDedicatedServer(base.CurrentConfig.DedicatedServer);
				SteamGameServer.SetProduct("ballistic");
				SteamGameServer.SetModDir("ballistic");
				SteamGameServer.SetGameDescription("Ballistic Overkill");
				IEnumerable<GameMapConfig> avaliableGameMapConfigList = ServiceProvider.GetService<GameMapModeConfigService>().GetAvaliableGameMapConfigList();
				SteamGameServer.SetBotPlayerCount(0);
				SteamGameServer.SetKeyValue("md", avaliableGameMapConfigList.Count<GameMapConfig>().ToString());
				int num = 0;
				foreach (GameMapConfig gameMapConfig in avaliableGameMapConfigList)
				{
					SteamGameServer.SetKeyValue("mw" + num, BytePacker.GetHexUint64(gameMapConfig.MapId));
					num++;
				}
				SteamGameServer.EnableHeartbeats(true);
				SteamGameServer.SetHeartbeatInterval(-1);
				this.m_creatingServerStep = SteamHostTransmitter.EServerStateStep.RUNNING;
			}
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x00010819 File Offset: 0x0000EA19
		private bool IsWorkshopMap(GameMapConfig config)
		{
			return config.Workshop;
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x00074338 File Offset: 0x00072538
		private void ReadBuffer()
		{
			uint num = 0U;
			while (SteamGameServerNetworking.IsP2PPacketAvailable(ref num, 0))
			{
				byte[] buffer = ArrayPool<byte>.GetBuffer((int)num);
				CSteamID csteamID;
				if (!SteamGameServerNetworking.ReadP2PPacket(buffer, num, ref num, ref csteamID, 0))
				{
					Debug.LogError("Could not read p2p packet");
					break;
				}
				ThriftPacket thriftPacket = this.m_packetMapper.uncompressPacket(buffer);
				ArrayPool<byte>.FreeBuffer(buffer);
				base.receivePacket(thriftPacket, SteamClientFactory.GetClient(csteamID));
			}
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x000743A0 File Offset: 0x000725A0
		internal override void UpdateTransmitter(float p_deltaTime)
		{
			SteamHostTransmitter.EServerStateStep creatingServerStep = this.m_creatingServerStep;
			if (creatingServerStep != SteamHostTransmitter.EServerStateStep.LOGGING_IN)
			{
				if (creatingServerStep == SteamHostTransmitter.EServerStateStep.RUNNING)
				{
					this.ReadBuffer();
				}
			}
			else
			{
				this.VerifyLoginTimeout(p_deltaTime);
			}
			if (this.m_creatingServerStep != SteamHostTransmitter.EServerStateStep.NONE)
			{
				GameServer.RunCallbacks();
			}
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x00010821 File Offset: 0x0000EA21
		internal override CSteamID GetHostID()
		{
			return SteamGameServer.GetSteamID();
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x000743F0 File Offset: 0x000725F0
		internal override void Close()
		{
			if (this.m_creatingServerStep != SteamHostTransmitter.EServerStateStep.NONE)
			{
				SteamCallbacks.SteamServersConnected_t.UnregisterCallback(new Action<SteamServersConnected_t>(this.OnGameServerConnected));
				SteamCallbacks.SteamServersDisconnected_t.UnregisterCallback(new Action<SteamServersDisconnected_t>(this.OnGameServerDisconnected));
				SteamCallbacks.SteamServerConnectFailure_t.UnregisterCallback(new Action<SteamServerConnectFailure_t>(this.OnGameServerConnectionFail));
				SteamCallbacks.GSPolicyResponse_t.UnregisterCallback(new Action<GSPolicyResponse_t>(this.OnPolicyResponse));
				SteamCallbacks.P2PSessionConnectFail_t.UnregisterCallback(new Action<P2PSessionConnectFail_t>(this.OnP2PSessionConnectFail));
				SteamCallbacks.P2PSessionRequest_t.UnregisterCallback(new Action<P2PSessionRequest_t>(this.OnP2PSessionRequest));
				GameServer.Shutdown();
				SteamClientFactory.FreeAll();
				this.m_creatingServerStep = SteamHostTransmitter.EServerStateStep.NONE;
			}
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x00010828 File Offset: 0x0000EA28
		private void OnGameServerConnected(SteamServersConnected_t pCallback)
		{
			SteamGameServer.SetMaxPlayerCount((int)base.CurrentConfig.MaxPlayers);
			SteamGameServer.SetServerName(base.CurrentConfig.ServerName);
			SteamGameServer.SetPasswordProtected(base.CurrentConfig.PrivateMatch);
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x00074480 File Offset: 0x00072680
		internal override void UpdateConfig(SteamConfig config)
		{
			base.UpdateConfig(config);
			string text = string.Format("{0}/{1}", base.CurrentConfig.GameMap, base.CurrentConfig.GameMode);
			SteamGameServer.SetMapName(text);
			SteamGameServer.SetMaxPlayerCount((int)base.CurrentConfig.MaxPlayers);
			SteamGameServer.SetServerName(base.CurrentConfig.ServerName);
			SteamGameServer.SetPasswordProtected(base.CurrentConfig.PrivateMatch);
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x00002A31 File Offset: 0x00000C31
		private void OnGameServerDisconnected(SteamServersDisconnected_t pCallback)
		{
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x0001085A File Offset: 0x0000EA5A
		private void OnGameServerConnectionFail(SteamServerConnectFailure_t pCallback)
		{
			base.DispatchOnHostFail();
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x00010862 File Offset: 0x0000EA62
		private void OnPolicyResponse(GSPolicyResponse_t pCallback)
		{
			if (SteamGameServer.BSecure())
			{
				Debug.Log("Server is VAC Secure!\n");
			}
			else
			{
				Debug.Log("Server is not VAC Secure!\n");
			}
			base.DispatchOnHostReady();
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x00002A31 File Offset: 0x00000C31
		private void OnP2PSessionConnectFail(P2PSessionConnectFail_t pCallback)
		{
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x0001088D File Offset: 0x0000EA8D
		private void OnP2PSessionRequest(P2PSessionRequest_t pCallback)
		{
			SteamGameServerNetworking.AcceptP2PSessionWithUser(pCallback.m_steamIDRemote);
		}

		// Token: 0x04001893 RID: 6291
		internal const uint IpAddress127_0_0_1 = 2130706433U;

		// Token: 0x04001894 RID: 6292
		private readonly float LOGIN_MAX_TIME = 10f;

		// Token: 0x04001895 RID: 6293
		private ThriftByteArrayPacketMapper m_packetMapper;

		// Token: 0x04001896 RID: 6294
		private SteamHostTransmitter.EServerStateStep m_creatingServerStep;

		// Token: 0x04001897 RID: 6295
		private float m_loginElapsedTime;

		// Token: 0x020003CE RID: 974
		private enum EServerStateStep : byte
		{
			// Token: 0x04001899 RID: 6297
			NONE,
			// Token: 0x0400189A RID: 6298
			LOGGING_IN,
			// Token: 0x0400189B RID: 6299
			STARTED,
			// Token: 0x0400189C RID: 6300
			RUNNING
		}
	}
}
