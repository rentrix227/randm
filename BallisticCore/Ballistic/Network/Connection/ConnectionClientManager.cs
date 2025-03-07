using System;
using System.Collections.Generic;
using System.Diagnostics;
using Aquiris.Ballistic.Game.Networking;
using Aquiris.Ballistic.Game.SceneSystem;
using Aquiris.Ballistic.Game.Services;
using Aquiris.Ballistic.Network.Address;
using Aquiris.Ballistic.Network.Connection.Events;
using Aquiris.Ballistic.Network.Transmission;
using Aquiris.Ballistic.Network.Transmission.Events;
using Aquiris.Ballistic.Network.Transmission.Packet;
using Aquiris.Ballistic.Network.Transport.Connection.Events;
using Aquiris.Ballistic.Network.Transport.Connection.Responses;
using Aquiris.Ballistic.Network.Transport.Gameplay.State;
using Aquiris.Ballistic.Network.Utils;
using Aquiris.Services;
using UnityEngine;

namespace Aquiris.Ballistic.Network.Connection
{
	// Token: 0x0200034A RID: 842
	public abstract class ConnectionClientManager<P, A, O, C, NA, PT> : ConnectionManager<P, A, O, C, NA, PT> where P : Packet<O, C> where NA : NetworkAddress<A> where PT : BasicClientTransmitter<P, A, O, C, NA>
	{
		// Token: 0x06001216 RID: 4630 RVA: 0x0000EBFA File Offset: 0x0000CDFA
		public ConnectionClientManager(PT transmitter, C keepAliveChannel, C connectionChannel)
			: base(transmitter, keepAliveChannel, connectionChannel)
		{
			this.m_keepAliveAccumTime = 0f;
			this.m_connected = false;
			this.m_connecting = false;
		}

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06001217 RID: 4631 RVA: 0x00065740 File Offset: 0x00063940
		// (remove) Token: 0x06001218 RID: 4632 RVA: 0x00065778 File Offset: 0x00063978
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event EventHandler<EstablishConnectionEventArgs> onEstablishConnection;

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06001219 RID: 4633 RVA: 0x000657B0 File Offset: 0x000639B0
		// (remove) Token: 0x0600121A RID: 4634 RVA: 0x000657E8 File Offset: 0x000639E8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event EventHandler<FailConnectionEventArgs> onFailConnection;

		// Token: 0x0600121B RID: 4635 RVA: 0x0000EC29 File Offset: 0x0000CE29
		internal virtual void JoinGame(EClientMode mode, string password)
		{
			Debug.Log("CLIENT JoiningGame");
			this.SendPacketToHost(this.GetJoinGameRequestPacket(mode, password));
			this.m_loginRequestTime = TimeUtils.getCurrentTime();
			this.m_connecting = true;
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x00065820 File Offset: 0x00063A20
		internal virtual void LeaveGame(CurrentMatchService.ConnectionState motivation)
		{
			if (this.m_selfBasicClient != null)
			{
				if (ServiceProvider.GetService<CurrentMatchService>().State == CurrentMatchService.ConnectionState.CONNECTED)
				{
					ServiceProvider.GetService<CurrentMatchService>().State = motivation;
				}
				Debug.Log("CLIENT LeavingGame:" + motivation);
				this.SendPacketToHost(this.GetLeaveGameRequestPacket());
				base.DispatchClientDisconnected(this.m_selfBasicClient);
			}
			this.m_connecting = false;
			this.m_connected = false;
			this.m_selfBasicClient = (NA)((object)null);
			this.m_transmitter.Close();
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x0000EC56 File Offset: 0x0000CE56
		internal bool IsConnecting()
		{
			return this.m_connecting;
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x0000EC5E File Offset: 0x0000CE5E
		internal bool IsConnected()
		{
			return this.m_connected;
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x0000EC66 File Offset: 0x0000CE66
		internal bool SendPacketToHost(P packet)
		{
			return this.m_transmitter.sendPacketToHost(packet);
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x000658B4 File Offset: 0x00063AB4
		protected override void RunKeepAlive(float deltaTime)
		{
			if (!this.m_connected)
			{
				return;
			}
			this.m_keepAliveAccumTime += deltaTime;
			float currentTime = TimeUtils.getCurrentTime();
			if (this.m_keepAliveAccumTime >= 4f)
			{
				float num = currentTime;
				NA basicHost = this.m_transmitter.BasicHost;
				float num2 = num - basicHost.LastInteractionTime;
				if (ServiceProvider.GetService<SceneService>().IsLoadingScene)
				{
					this.m_keepAliveAccumTime %= 4f;
				}
				else
				{
					NA basicHost2 = this.m_transmitter.BasicHost;
					if (!basicHost2.CheckingKeepAlive && num2 >= 15f)
					{
						this.m_keepAliveAccumTime %= 4f;
						NA basicHost3 = this.m_transmitter.BasicHost;
						basicHost3.CheckingKeepAlive = true;
						this.SendPacketToHost(this.getKeepAliveRequestPacket());
					}
					else
					{
						NA basicHost4 = this.m_transmitter.BasicHost;
						if (basicHost4.CheckingKeepAlive && num2 >= 30f)
						{
							if (ServiceProvider.GetService<NetworkGameService>().ServerIsTerminating)
							{
								this.LeaveGame(CurrentMatchService.ConnectionState.SERVERTERMINATED);
							}
							else
							{
								this.LeaveGame(CurrentMatchService.ConnectionState.LOST_CONNECTION);
							}
						}
					}
				}
				foreach (NA na in this.m_knownClientList.Values)
				{
					float num3 = currentTime - na.LastInteractionTime;
					if (!na.CheckingKeepAlive && num3 >= 15f)
					{
						na.CheckingKeepAlive = true;
						this.m_transmitter.SendPacket(this.getKeepAliveRequestPacket(), na);
					}
					if (!na.CheckingKeepAlive || num3 >= 30f)
					{
					}
				}
			}
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x00065AC4 File Offset: 0x00063CC4
		protected override void KeepAliveReceived(PacketReceivedEventArgs<P, A, O, C, NA> packet)
		{
			if (this.isKeepAliveResponsePacket(packet.Packet))
			{
				if (packet.Sender == this.m_transmitter.BasicHost)
				{
					NA basicHost = this.m_transmitter.BasicHost;
					basicHost.CheckingKeepAlive = false;
				}
				else
				{
					NA sender = packet.Sender;
					sender.CheckingKeepAlive = false;
				}
			}
			else if (this.isKeepAliveRequestPacket(packet.Packet))
			{
				if (packet.Sender == this.m_transmitter.BasicHost)
				{
					this.SendPacketToHost(this.getKeepAliveResponsePacket());
				}
				else
				{
					Dictionary<A, NA> knownClientList = this.m_knownClientList;
					NA sender2 = packet.Sender;
					if (knownClientList.ContainsKey(sender2.Info))
					{
						base.SendPacket(this.getKeepAliveResponsePacket(), packet.Sender);
					}
				}
			}
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x00065BC8 File Offset: 0x00063DC8
		protected override void ConnectionPacketReceived(PacketReceivedEventArgs<P, A, O, C, NA> packet)
		{
			if (this.IsJoinGameEventPacket(packet.Packet))
			{
				NA connectingClient = this.GetConnectingClient(packet.Packet);
				if (connectingClient is BasicClientInfo<A>)
				{
					BasicClientInfo<A> basicClientInfo = connectingClient as BasicClientInfo<A>;
					if (basicClientInfo.isMe())
					{
						this.m_connecting = false;
						this.m_connected = true;
						this.m_selfBasicClient = connectingClient;
						this.EstablishConnection();
					}
					else
					{
						this.m_knownClientList.Add(connectingClient.Info, connectingClient);
					}
					base.DispatchClientConnected(connectingClient);
				}
			}
			else if (this.IsLeaveGameEventPacket(packet.Packet))
			{
				NA connectingClient2 = this.GetConnectingClient(packet.Packet);
				if (connectingClient2 is BasicClientInfo<A>)
				{
					BasicClientInfo<A> basicClientInfo2 = connectingClient2 as BasicClientInfo<A>;
					if (basicClientInfo2.isMe())
					{
						CurrentMatchService.ConnectionState connectionState = CurrentMatchService.ConnectionState.KICKED;
						P packet2 = packet.Packet;
						LeaveGameEvent leaveGameEvent = packet2.TransportObject as LeaveGameEvent;
						if (leaveGameEvent != null)
						{
							switch (leaveGameEvent.Motivation)
							{
							case 1:
								connectionState = CurrentMatchService.ConnectionState.DISCONNECTED;
								break;
							case 2:
								connectionState = CurrentMatchService.ConnectionState.BADCONNECTION;
								break;
							case 3:
								connectionState = CurrentMatchService.ConnectionState.GRENADEHACK;
								break;
							case 4:
								connectionState = CurrentMatchService.ConnectionState.SPEEDHACK;
								break;
							case 5:
								connectionState = CurrentMatchService.ConnectionState.INACTIVITY;
								break;
							case 6:
								connectionState = CurrentMatchService.ConnectionState.MAXPING;
								break;
							case 10:
								connectionState = CurrentMatchService.ConnectionState.HOST_KICKED;
								break;
							case 11:
								connectionState = CurrentMatchService.ConnectionState.HOST_BLACKLISTED;
								break;
							}
						}
						this.LeaveGame(connectionState);
					}
					else
					{
						this.m_knownClientList.Remove(connectingClient2.Info);
						base.DispatchClientDisconnected(connectingClient2);
					}
				}
			}
			else if (this.IsFailConnectionResponsePacket(packet.Packet))
			{
				CurrentMatchService.ConnectionState connectionState2 = CurrentMatchService.ConnectionState.FAILED_CONNECTION;
				P packet3 = packet.Packet;
				FailConnectionResponse failConnectionResponse = packet3.TransportObject as FailConnectionResponse;
				if (failConnectionResponse != null)
				{
					LeaveGameMotivation leaveGameMotivation = (LeaveGameMotivation)failConnectionResponse.Motivation;
					if (leaveGameMotivation != LeaveGameMotivation.SERVERFULL)
					{
						if (leaveGameMotivation == LeaveGameMotivation.VACBANNED)
						{
							connectionState2 = CurrentMatchService.ConnectionState.VACBANNED;
						}
					}
					else
					{
						connectionState2 = CurrentMatchService.ConnectionState.SERVERFULL;
					}
				}
				this.LeaveGame(connectionState2);
				this.FailConnection((LeaveGameMotivation)failConnectionResponse.Motivation);
			}
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x0000EC7A File Offset: 0x0000CE7A
		internal override void UpdateConnection(float deltaTime)
		{
			base.UpdateConnection(deltaTime);
			this.CheckLoginTimeout(deltaTime);
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x0000EC8A File Offset: 0x0000CE8A
		private void CheckLoginTimeout(float deltaTime)
		{
			if (this.m_connecting && this.m_loginRequestTime + this.LOGIN_TIMEOUT < TimeUtils.getCurrentTime())
			{
				Debug.Log("CLIENT login timeout");
				this.LeaveGame(CurrentMatchService.ConnectionState.FAILED_CONNECTION);
				this.FailConnection(LeaveGameMotivation.NONE);
			}
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x0000ECC6 File Offset: 0x0000CEC6
		protected virtual void EstablishConnection()
		{
			Debug.Log("CLIENT connection established");
			if (this.onEstablishConnection != null)
			{
				this.onEstablishConnection(this, new EstablishConnectionEventArgs());
			}
			this.OnConnectionEstablished();
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0000ECF4 File Offset: 0x0000CEF4
		protected virtual void FailConnection(LeaveGameMotivation motivation = LeaveGameMotivation.NONE)
		{
			Debug.Log("CLIENT connection failed");
			if (this.onFailConnection != null)
			{
				this.onFailConnection(this, new FailConnectionEventArgs(motivation));
			}
			this.OnConnectionFailed();
		}

		// Token: 0x06001227 RID: 4647
		protected abstract void OnConnectionEstablished();

		// Token: 0x06001228 RID: 4648
		protected abstract void OnConnectionFailed();

		// Token: 0x06001229 RID: 4649
		protected abstract NA GetConnectingClient(P packet);

		// Token: 0x0600122A RID: 4650
		protected abstract P GetJoinGameRequestPacket(EClientMode mode, string password);

		// Token: 0x0600122B RID: 4651
		protected abstract P GetLeaveGameRequestPacket();

		// Token: 0x0600122C RID: 4652
		protected abstract bool IsJoinGameEventPacket(P packet);

		// Token: 0x0600122D RID: 4653
		protected abstract bool IsLeaveGameEventPacket(P packet);

		// Token: 0x0600122E RID: 4654
		protected abstract bool IsFailConnectionResponsePacket(P packet);

		// Token: 0x040016BB RID: 5819
		private readonly float LOGIN_TIMEOUT = 25f;

		// Token: 0x040016BC RID: 5820
		private float m_loginRequestTime;

		// Token: 0x040016BD RID: 5821
		private float m_keepAliveAccumTime;

		// Token: 0x040016BE RID: 5822
		private bool m_connecting;

		// Token: 0x040016BF RID: 5823
		private bool m_connected;

		// Token: 0x040016C0 RID: 5824
		private NA m_selfBasicClient;
	}
}
