using System;
using System.Collections.Generic;
using System.Diagnostics;
using Aquiris.Ballistic.Network.Address;
using Aquiris.Ballistic.Network.Address.Steam;
using Aquiris.Ballistic.Network.Connection;
using Aquiris.Ballistic.Network.Connection.Events;
using Aquiris.Ballistic.Network.Connection.Steam;
using Aquiris.Ballistic.Network.Game.Steam;
using Aquiris.Ballistic.Network.Gameplay.GameMode;
using Aquiris.Ballistic.Network.Transmission.Events;
using Aquiris.Ballistic.Network.Transmission.Packet;
using Aquiris.Ballistic.Network.Transmission.Packet.Thrift;
using Aquiris.Ballistic.Network.Transport.Gameplay.Capture.Requests;
using Aquiris.Ballistic.Network.Transport.Gameplay.Chat;
using Aquiris.Ballistic.Network.Transport.Gameplay.Clock.Events;
using Aquiris.Ballistic.Network.Transport.Gameplay.Clock.Requests;
using Aquiris.Ballistic.Network.Transport.Gameplay.Command.Requests;
using Aquiris.Ballistic.Network.Transport.Gameplay.Inventory.Events;
using Aquiris.Ballistic.Network.Transport.Gameplay.Player.Requests;
using Aquiris.Ballistic.Network.Transport.Gameplay.Spawn.Requests;
using Aquiris.Ballistic.Network.Transport.Gameplay.State;
using Aquiris.Ballistic.Network.Transport.Gameplay.Team.Requests;
using Aquiris.Ballistic.Network.Transport.Gameplay.Team.Responses;
using Aquiris.Ballistic.Network.Transport.Gameplay.Vote.Requests;
using Aquiris.Ballistic.Network.Transport.Player;
using Aquiris.Ballistic.Network.Transport.User.Requests;
using Steamworks;
using Thrift.Protocol;
using UnityEngine;

namespace Aquiris.Ballistic.Network
{
	// Token: 0x02000343 RID: 835
	internal class BallisticClient
	{
		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060011AF RID: 4527 RVA: 0x0000E839 File Offset: 0x0000CA39
		// (set) Token: 0x060011B0 RID: 4528 RVA: 0x0000E841 File Offset: 0x0000CA41
		internal SteamGameClientManager SteamGameClientManager { get; private set; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060011B1 RID: 4529 RVA: 0x0000E84A File Offset: 0x0000CA4A
		internal uint Latency
		{
			get
			{
				return (uint)this._networkClockManager.Ping;
			}
		}

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x060011B2 RID: 4530 RVA: 0x000647BC File Offset: 0x000629BC
		// (remove) Token: 0x060011B3 RID: 4531 RVA: 0x000647F4 File Offset: 0x000629F4
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<PacketReceivedEventArgs<ThriftPacket, CSteamID, TBase, ETransportChannel, NetworkAddress<CSteamID>>> OnNetworkPacketReceived;

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x060011B4 RID: 4532 RVA: 0x0006482C File Offset: 0x00062A2C
		// (remove) Token: 0x060011B5 RID: 4533 RVA: 0x00064864 File Offset: 0x00062A64
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action OnConnectionEstablished;

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x060011B6 RID: 4534 RVA: 0x0006489C File Offset: 0x00062A9C
		// (remove) Token: 0x060011B7 RID: 4535 RVA: 0x000648D4 File Offset: 0x00062AD4
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<LeaveGameMotivation> OnConnectionFailed;

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x060011B8 RID: 4536 RVA: 0x0006490C File Offset: 0x00062B0C
		// (remove) Token: 0x060011B9 RID: 4537 RVA: 0x00064944 File Offset: 0x00062B44
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action OnConnectionLost;

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x060011BA RID: 4538 RVA: 0x0006497C File Offset: 0x00062B7C
		// (remove) Token: 0x060011BB RID: 4539 RVA: 0x000649B4 File Offset: 0x00062BB4
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<long> OnUserJoin;

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x060011BC RID: 4540 RVA: 0x000649EC File Offset: 0x00062BEC
		// (remove) Token: 0x060011BD RID: 4541 RVA: 0x00064A24 File Offset: 0x00062C24
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<long> OnUserLeft;

		// Token: 0x060011BE RID: 4542 RVA: 0x0000E858 File Offset: 0x0000CA58
		internal void CreateConnection(CSteamID serverId, EClientMode mode, string password)
		{
			this._hostId = serverId;
			this.CreateSteamGameClientManager(serverId, mode, password);
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x0000E86A File Offset: 0x0000CA6A
		internal void DestroyConnection()
		{
			this.DestroySteamGameClientManager();
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x0000E872 File Offset: 0x0000CA72
		internal void Update()
		{
			if (this.SteamGameClientManager != null)
			{
				this.SteamGameClientManager.UpdateGameMode(Time.deltaTime);
			}
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x0000E88F File Offset: 0x0000CA8F
		internal CSteamID GetHostID()
		{
			return this._hostId;
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0000E897 File Offset: 0x0000CA97
		internal double GetServerTimeStamp()
		{
			return this._networkClockManager.ServerTime;
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060011C3 RID: 4547 RVA: 0x0000E8A4 File Offset: 0x0000CAA4
		internal bool MatchTimestampIsValid
		{
			get
			{
				return this._networkClockManager.HasValidTimeDelta;
			}
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x0000E8B1 File Offset: 0x0000CAB1
		internal GameModeMetaData GetGameModeMetaData()
		{
			if (this.SteamGameClientManager != null && this.SteamGameClientManager.GetGameMode != null)
			{
				return this.SteamGameClientManager.GetGameMode.GameModeMetaData;
			}
			return null;
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x0000E8E0 File Offset: 0x0000CAE0
		internal void Broadcast(TBase request)
		{
			this.Broadcast(request, false);
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x00064A5C File Offset: 0x00062C5C
		internal void Broadcast(TBase request, bool teamOnly)
		{
			ThriftPacket thriftPacket = null;
			if (request is ChatEvent)
			{
				thriftPacket = new ThriftPacket
				{
					TransportChannel = ETransportChannel.GAME_EVENTS,
					TransportObject = request,
					TransportType = ETransportType.Reliable
				};
			}
			if (thriftPacket == null)
			{
				Debug.LogWarning("Can't broadcast thrift packet: " + request);
				return;
			}
			if (teamOnly)
			{
				Dictionary<long, ClientCommonMetaData> clientMetaDataMap = this.GetGameModeMetaData().ClientMetaDataMap;
				sbyte team = clientMetaDataMap[(long)SteamUser.GetSteamID().m_SteamID].Team;
				foreach (ClientCommonMetaData clientCommonMetaData in clientMetaDataMap.Values)
				{
					if ((int)clientCommonMetaData.Team == (int)team && clientCommonMetaData.User != (long)SteamUser.GetSteamID().m_SteamID)
					{
						this._steamConnectionClientManager.SendPacket(thriftPacket, SteamClientFactory.GetClient(new CSteamID((ulong)clientCommonMetaData.User)));
					}
				}
			}
			else
			{
				this._steamConnectionClientManager.BroadcastPacket(thriftPacket);
			}
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x00064B7C File Offset: 0x00062D7C
		internal void Send(TBase request)
		{
			ThriftPacket thriftPacket = null;
			if (request is UserReadyRequest)
			{
				thriftPacket = new ThriftPacket
				{
					TransportChannel = ETransportChannel.PLAYER_DATA,
					TransportObject = request,
					TransportType = ETransportType.ReliableOrdered
				};
			}
			else if (request is PlayerHitRequest)
			{
				thriftPacket = new ThriftPacket
				{
					TransportChannel = ETransportChannel.GAME_EVENTS,
					TransportObject = request,
					TransportType = ETransportType.ReliableOrdered
				};
			}
			else if (request is ReadyToSpawnRequest)
			{
				thriftPacket = new ThriftPacket
				{
					TransportChannel = ETransportChannel.SPAWN_GAME_COMPONENT,
					TransportObject = request,
					TransportType = ETransportType.ReliableOrdered
				};
			}
			else if (request is ChangeWeaponRequest)
			{
				thriftPacket = new ThriftPacket
				{
					TransportChannel = ETransportChannel.PLAYER_DATA,
					TransportObject = request,
					TransportType = ETransportType.ReliableOrdered
				};
			}
			else if (request is SelectTeamRequest)
			{
				thriftPacket = new ThriftPacket
				{
					TransportChannel = ETransportChannel.TEAM_DATA,
					TransportObject = request,
					TransportType = ETransportType.ReliableOrdered
				};
			}
			else if (request is TeamCallResponse)
			{
				thriftPacket = new ThriftPacket
				{
					TransportChannel = ETransportChannel.TEAM_DATA,
					TransportObject = request,
					TransportType = ETransportType.Reliable
				};
			}
			else if (request is RebalanceCallResponse)
			{
				thriftPacket = new ThriftPacket
				{
					TransportChannel = ETransportChannel.TEAM_DATA,
					TransportObject = request,
					TransportType = ETransportType.Reliable
				};
			}
			else if (request is NetworkTimeRequest)
			{
				thriftPacket = new ThriftPacket
				{
					TransportChannel = ETransportChannel.CLOCK_DATA,
					TransportObject = request,
					TransportType = ETransportType.ReliableOrdered
				};
			}
			else if (request is GrenadeLaunchRequest)
			{
				thriftPacket = new ThriftPacket
				{
					TransportChannel = ETransportChannel.PLAYER_DATA,
					TransportObject = request,
					TransportType = ETransportType.ReliableOrdered
				};
			}
			else if (request is CapturePointInteractionRequest)
			{
				thriftPacket = new ThriftPacket
				{
					TransportChannel = ETransportChannel.GAME_EVENTS,
					TransportObject = request,
					TransportType = ETransportType.ReliableOrdered
				};
			}
			else if (request is ResupplyRequest)
			{
				thriftPacket = new ThriftPacket
				{
					TransportChannel = ETransportChannel.GAME_EVENTS,
					TransportObject = request,
					TransportType = ETransportType.ReliableOrdered
				};
			}
			else if (request is WeaponStationRequest)
			{
				thriftPacket = new ThriftPacket
				{
					TransportChannel = ETransportChannel.GAME_EVENTS,
					TransportObject = request,
					TransportType = ETransportType.ReliableOrdered
				};
			}
			else if (request is VoteMapRequest)
			{
				thriftPacket = new ThriftPacket
				{
					TransportChannel = ETransportChannel.VOTE_MAP,
					TransportObject = request,
					TransportType = ETransportType.ReliableOrdered
				};
			}
			else if (request is InventoryResponse)
			{
				thriftPacket = new ThriftPacket
				{
					TransportChannel = ETransportChannel.SPAWN_GAME_COMPONENT,
					TransportObject = request,
					TransportType = ETransportType.ReliableOrdered
				};
			}
			else if (request is PlayerInfoEvent)
			{
				thriftPacket = new ThriftPacket
				{
					TransportChannel = ETransportChannel.PLAYER_DATA,
					TransportObject = request,
					TransportType = ETransportType.Reliable
				};
			}
			else if (request is ServerCommandRequest)
			{
				thriftPacket = new ThriftPacket
				{
					TransportChannel = ETransportChannel.GAME_CONTROL,
					TransportObject = request,
					TransportType = ETransportType.Reliable
				};
			}
			else if (request is VoiceRequest)
			{
				thriftPacket = new ThriftPacket
				{
					TransportChannel = ETransportChannel.PLAYER_DATA,
					TransportObject = request,
					TransportType = ETransportType.Reliable
				};
			}
			if (thriftPacket == null)
			{
				Debug.LogWarning("Can't send thrift packet to the host: " + request);
			}
			else
			{
				this._steamConnectionClientManager.SendPacketToHost(thriftPacket);
			}
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x00064EAC File Offset: 0x000630AC
		internal void SendUdpMessage(byte[] request)
		{
			if (this._playerStatePacket == null)
			{
				this._playerStateEvent = new PlayerStateEvent
				{
					User = (long)SteamUser.GetSteamID().m_SteamID
				};
				this._playerStatePacket = new ThriftPacket
				{
					TransportChannel = ETransportChannel.PLAYER_STATE,
					TransportType = ETransportType.Unreliable,
					TransportObject = this._playerStateEvent
				};
			}
			this._playerStateEvent.PlayerState = request;
			this._steamConnectionClientManager.SendPacketToHost(this._playerStatePacket);
			this._steamConnectionClientManager.BroadcastPacket(this._playerStatePacket);
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x00064F3C File Offset: 0x0006313C
		private void CreateSteamGameClientManager(CSteamID serverId, EClientMode mode, string password)
		{
			this._steamConnectionClientManager = new SteamConnectionClientManager(SteamHostFactory.getHost(serverId));
			this.SteamGameClientManager = new SteamGameClientManager(this._steamConnectionClientManager);
			this.SteamGameClientManager.JoinGame(mode, password);
			this.AddConnectionClientListener();
			this.AddGameModeClientListener();
			this._networkClockManager = new NetworkClockManager(this);
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x0000E8EA File Offset: 0x0000CAEA
		private void DestroySteamGameClientManager()
		{
			this.RemoveGameModeClientListener();
			this.RemoveConnectionClientListener();
			this.SteamGameClientManager.LeaveGame();
			this.SteamGameClientManager = null;
			this._steamConnectionClientManager = null;
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x00064F90 File Offset: 0x00063190
		private void AddConnectionClientListener()
		{
			this._steamConnectionClientManager.onClientConnection += this.OnConnectionClientConnectedReceived;
			this._steamConnectionClientManager.onClientDisconnection += this.OnConnectionClientDisconnectedReceived;
			this._steamConnectionClientManager.onEstablishConnection += this.OnConnectionEstablish;
			this._steamConnectionClientManager.onFailConnection += this.OnConnectionFail;
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x00064FFC File Offset: 0x000631FC
		private void RemoveConnectionClientListener()
		{
			this._steamConnectionClientManager.onClientConnection -= this.OnConnectionClientConnectedReceived;
			this._steamConnectionClientManager.onClientDisconnection -= this.OnConnectionClientDisconnectedReceived;
			this._steamConnectionClientManager.onEstablishConnection -= this.OnConnectionEstablish;
			this._steamConnectionClientManager.onFailConnection -= this.OnConnectionFail;
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x00065068 File Offset: 0x00063268
		private void OnConnectionClientConnectedReceived(object sender, ClientConnectionEventArgs<NetworkAddress<CSteamID>, CSteamID> e)
		{
			SteamClientInfo steamClientInfo = (SteamClientInfo)e.NetworkAddress;
			if (steamClientInfo.isMe())
			{
				this._networkClockManager.Synchronize();
			}
			else
			{
				this.OnUserJoin((long)e.NetworkAddress.Info.m_SteamID);
			}
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x000650BC File Offset: 0x000632BC
		private void OnConnectionClientDisconnectedReceived(object sender, ClientDisconnectionEventArgs<NetworkAddress<CSteamID>, CSteamID> e)
		{
			SteamClientInfo steamClientInfo = (SteamClientInfo)e.NetworkAddress;
			if (steamClientInfo.isMe())
			{
				if (this.OnConnectionLost != null)
				{
					this.OnConnectionLost();
				}
			}
			else if (this.OnUserLeft != null)
			{
				this.OnUserLeft((long)e.NetworkAddress.Info.m_SteamID);
			}
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x0000E911 File Offset: 0x0000CB11
		private void OnConnectionEstablish(object sender, EstablishConnectionEventArgs e)
		{
			if (this.OnConnectionEstablished != null)
			{
				this.OnConnectionEstablished();
			}
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x0000E92B File Offset: 0x0000CB2B
		private void OnConnectionFail(object sender, FailConnectionEventArgs e)
		{
			if (this.OnConnectionFailed != null)
			{
				this.OnConnectionFailed(e.Motivation);
			}
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x0000E94B File Offset: 0x0000CB4B
		private void AddGameModeClientListener()
		{
			GameModeClient getGameMode = this.SteamGameClientManager.GetGameMode;
			getGameMode.onEventReceived = (Action<PacketReceivedEventArgs<ThriftPacket, CSteamID, TBase, ETransportChannel, NetworkAddress<CSteamID>>>)Delegate.Combine(getGameMode.onEventReceived, new Action<PacketReceivedEventArgs<ThriftPacket, CSteamID, TBase, ETransportChannel, NetworkAddress<CSteamID>>>(this.RedispatchEvent));
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x0000E979 File Offset: 0x0000CB79
		private void RemoveGameModeClientListener()
		{
			GameModeClient getGameMode = this.SteamGameClientManager.GetGameMode;
			getGameMode.onEventReceived = (Action<PacketReceivedEventArgs<ThriftPacket, CSteamID, TBase, ETransportChannel, NetworkAddress<CSteamID>>>)Delegate.Remove(getGameMode.onEventReceived, new Action<PacketReceivedEventArgs<ThriftPacket, CSteamID, TBase, ETransportChannel, NetworkAddress<CSteamID>>>(this.RedispatchEvent));
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x0000E9A7 File Offset: 0x0000CBA7
		private void RedispatchEvent(PacketReceivedEventArgs<ThriftPacket, CSteamID, TBase, ETransportChannel, NetworkAddress<CSteamID>> networkPacket)
		{
			if (this.OnNetworkPacketReceived != null)
			{
				this.OnNetworkPacketReceived(networkPacket);
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060011D4 RID: 4564 RVA: 0x0000E9C2 File Offset: 0x0000CBC2
		internal bool IsConnected
		{
			get
			{
				return this.SteamGameClientManager != null && this.SteamGameClientManager.IsInGame();
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060011D5 RID: 4565 RVA: 0x0000E9DD File Offset: 0x0000CBDD
		internal bool IsConnecting
		{
			get
			{
				return this._steamConnectionClientManager.IsConnecting();
			}
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x0000E9EA File Offset: 0x0000CBEA
		internal void Ping()
		{
			this._networkClockManager.RequestServerTime();
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x00065128 File Offset: 0x00063328
		internal void SendPingToHost(long ping)
		{
			NetworkTimeEvent networkTimeEvent = new NetworkTimeEvent
			{
				Ping = ping
			};
			ThriftPacket thriftPacket = new ThriftPacket
			{
				TransportChannel = ETransportChannel.CLOCK_DATA,
				TransportObject = networkTimeEvent,
				TransportType = ETransportType.Unreliable
			};
			this._steamConnectionClientManager.SendPacketToHost(thriftPacket);
		}

		// Token: 0x04001684 RID: 5764
		private SteamConnectionClientManager _steamConnectionClientManager;

		// Token: 0x04001686 RID: 5766
		private CSteamID _hostId;

		// Token: 0x04001687 RID: 5767
		private NetworkClockManager _networkClockManager;

		// Token: 0x0400168E RID: 5774
		private ThriftPacket _playerStatePacket;

		// Token: 0x0400168F RID: 5775
		private PlayerStateEvent _playerStateEvent;
	}
}
