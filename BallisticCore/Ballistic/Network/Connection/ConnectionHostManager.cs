using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Aquiris.Ballistic.Network.Address;
using Aquiris.Ballistic.Network.Configuration;
using Aquiris.Ballistic.Network.Connection.Events;
using Aquiris.Ballistic.Network.Transmission;
using Aquiris.Ballistic.Network.Transmission.Events;
using Aquiris.Ballistic.Network.Transmission.Packet;
using Aquiris.Ballistic.Network.Utils;
using UnityEngine;

namespace Aquiris.Ballistic.Network.Connection
{
	// Token: 0x0200034B RID: 843
	internal abstract class ConnectionHostManager<P, A, O, C, BC, PT, CFG> : ConnectionManager<P, A, O, C, BC, PT> where P : Packet<O, C> where BC : BasicClientInfo<A> where PT : BasicHostTransmitter<P, A, O, C, BC, CFG> where CFG : Config
	{
		// Token: 0x0600122F RID: 4655 RVA: 0x00065E0C File Offset: 0x0006400C
		internal ConnectionHostManager(PT p_transmitter, C p_keepAliveChannel, C p_connectionChannel, CFG p_config)
			: base(p_transmitter, p_keepAliveChannel, p_connectionChannel)
		{
			this._keepAliveAccumTime = 0f;
			this._toRemove = new List<BC>();
			this._started = false;
			this.m_pingMachine = new PingMachine<A, BC>(this.CurrentConfig.MaxPing);
			PingMachine<A, BC> pingMachine = this.m_pingMachine;
			pingMachine.OnMaxPing = (Action<BC>)Delegate.Combine(pingMachine.OnMaxPing, new Action<BC>(this.OnMaxPing));
		}

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06001230 RID: 4656 RVA: 0x00065E84 File Offset: 0x00064084
		// (remove) Token: 0x06001231 RID: 4657 RVA: 0x00065EBC File Offset: 0x000640BC
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event EventHandler<HostReadyEventArgs> OnHostReady;

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06001232 RID: 4658 RVA: 0x00065EF4 File Offset: 0x000640F4
		// (remove) Token: 0x06001233 RID: 4659 RVA: 0x00065F2C File Offset: 0x0006412C
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event EventHandler<HostFailEventArgs> OnHostFail;

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06001234 RID: 4660 RVA: 0x0000ED23 File Offset: 0x0000CF23
		internal CFG CurrentConfig
		{
			get
			{
				return this.m_transmitter.CurrentConfig;
			}
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00065F64 File Offset: 0x00064164
		protected override void Finalize()
		{
			try
			{
				PingMachine<A, BC> pingMachine = this.m_pingMachine;
				pingMachine.OnMaxPing = (Action<BC>)Delegate.Remove(pingMachine.OnMaxPing, new Action<BC>(this.OnMaxPing));
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x00065FB4 File Offset: 0x000641B4
		internal virtual void StartHost()
		{
			Debug.Log("HOST starting game");
			this._started = true;
			PT pt = this.m_transmitter;
			pt.OnHostReady = (Action)Delegate.Combine(pt.OnHostReady, new Action(this.OnTrasmitterHostReady));
			PT pt2 = this.m_transmitter;
			pt2.OnHostFail = (Action)Delegate.Combine(pt2.OnHostFail, new Action(this.OnTrasmitterHostFail));
			this.m_transmitter.Start();
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x0006603C File Offset: 0x0006423C
		internal virtual void CloseHost()
		{
			Debug.Log("HOST closing game");
			this._started = false;
			PT pt = this.m_transmitter;
			pt.OnHostReady = (Action)Delegate.Remove(pt.OnHostReady, new Action(this.OnTrasmitterHostReady));
			PT pt2 = this.m_transmitter;
			pt2.OnHostFail = (Action)Delegate.Remove(pt2.OnHostFail, new Action(this.OnTrasmitterHostFail));
			this.m_transmitter.Close();
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0000ED36 File Offset: 0x0000CF36
		internal void UpdatePing(BC client, long ping)
		{
			this.m_pingMachine.UpdatePing(client, ping);
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x0000ED45 File Offset: 0x0000CF45
		internal float GetAveragePing(BC client)
		{
			return this.m_pingMachine.GetAveragePing(client);
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x0000ED53 File Offset: 0x0000CF53
		internal void KickPlayer(BC client, LeaveGameMotivation motivation, string additionalInfo)
		{
			base.SendPacket(this.GetLeaveGameEventPacket(client, motivation), client);
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x00002A31 File Offset: 0x00000C31
		private void OnMaxPing(BC client)
		{
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x0000ED65 File Offset: 0x0000CF65
		private void OnTrasmitterHostReady()
		{
			Debug.Log("HOST server ready");
			if (this.OnHostReady != null)
			{
				this.OnHostReady(this, new HostReadyEventArgs());
			}
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x0000ED8D File Offset: 0x0000CF8D
		private void OnTrasmitterHostFail()
		{
			Debug.Log("HOST server fail");
			if (this.OnHostFail != null)
			{
				this.OnHostFail(this, new HostFailEventArgs());
			}
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x000660C4 File Offset: 0x000642C4
		protected override void OnClientDisconnected(BC basicClient)
		{
			Debug.Log(string.Concat(new object[] { "HOST client disconnected: Name[", basicClient.Alias, "] ID[", basicClient, "]" }));
			base.BroadcastPacket(this.GetLeaveGameEventPacket(basicClient, LeaveGameMotivation.DISCONNECTED));
			this.m_knownClientList.Remove(basicClient.Info);
			this.m_transmitter.CloseClient(basicClient);
			this.m_pingMachine.RemoveClient(basicClient);
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x00066158 File Offset: 0x00064358
		protected override void OnClientConnected(BC basicClient)
		{
			Debug.Log(string.Concat(new object[] { "HOST client connected: Name[", basicClient.Alias, "] ID[", basicClient, "]" }));
			Dictionary<A, BC>.ValueCollection values = this.m_knownClientList.Values;
			for (int i = 0; i < values.Count; i++)
			{
				BC bc = values.ElementAt(i);
				base.SendPacket(this.GetJoinGameEventPacket(bc), basicClient);
			}
			this.m_knownClientList.Add(basicClient.Info, basicClient);
			this.m_pingMachine.AddClient(basicClient);
			base.BroadcastPacket(this.GetJoinGameEventPacket(basicClient));
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x00066214 File Offset: 0x00064414
		protected override void RunKeepAlive(float p_deltaTime)
		{
			if (!this._started)
			{
				return;
			}
			this._keepAliveAccumTime += p_deltaTime;
			if (this._keepAliveAccumTime >= 1f)
			{
				this._keepAliveAccumTime %= 1f;
				float currentTime = TimeUtils.getCurrentTime();
				this._toRemove.Clear();
				foreach (BC bc in this.m_knownClientList.Values)
				{
					float num = currentTime - bc.LastInteractionTime;
					if (!bc.CheckingKeepAlive && num >= 12f)
					{
						bc.CheckingKeepAlive = true;
						this.m_transmitter.SendPacket(this.getKeepAliveRequestPacket(), bc);
					}
					if (bc.CheckingKeepAlive && num >= 21f)
					{
						this._toRemove.Add(bc);
					}
				}
				for (int i = 0; i < this._toRemove.Count; i++)
				{
					base.DispatchClientDisconnected(this._toRemove[i]);
					Dictionary<A, BC> knownClientList = this.m_knownClientList;
					BC bc2 = this._toRemove[i];
					knownClientList.Remove(bc2.Info);
				}
			}
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00066394 File Offset: 0x00064594
		protected override void KeepAliveReceived(PacketReceivedEventArgs<P, A, O, C, BC> p_packetArgs)
		{
			if (this.isKeepAliveResponsePacket(p_packetArgs.Packet))
			{
				BC sender = p_packetArgs.Sender;
				sender.CheckingKeepAlive = false;
			}
			else if (this.isKeepAliveRequestPacket(p_packetArgs.Packet))
			{
				this.m_transmitter.SendPacket(this.getKeepAliveResponsePacket(), p_packetArgs.Sender);
			}
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x000663FC File Offset: 0x000645FC
		protected override void ConnectionPacketReceived(PacketReceivedEventArgs<P, A, O, C, BC> packetArgs)
		{
			if (this.IsJoinGameRequestPacket(packetArgs.Packet))
			{
				BC sender = packetArgs.Sender;
				sender.Alias = this.GetUserAlias(packetArgs.Packet);
				this.CheckMaxPlayers(packetArgs.Packet, packetArgs.Sender);
			}
			else if (this.IsLeaveGameRequestPacket(packetArgs.Packet))
			{
				base.DispatchClientDisconnected(packetArgs.Sender);
			}
		}

		// Token: 0x06001243 RID: 4675
		protected abstract void AuthenticateUser(P packet, BC sender);

		// Token: 0x06001244 RID: 4676
		protected abstract void CheckMaxPlayers(P packet, BC sender);

		// Token: 0x06001245 RID: 4677 RVA: 0x0000EDB5 File Offset: 0x0000CFB5
		protected void AuthenticationSuccess(BC client)
		{
			base.DispatchClientConnected(client);
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x0000EDBE File Offset: 0x0000CFBE
		protected void AuthenticationFail(BC client, LeaveGameMotivation motivation = LeaveGameMotivation.NONE)
		{
			base.SendPacket(this.GetFailConnectionResponsePacket(motivation), client);
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x0000EDCF File Offset: 0x0000CFCF
		internal void UpdateConfig(CFG config)
		{
			this.m_transmitter.UpdateConfig(config);
		}

		// Token: 0x06001248 RID: 4680
		protected abstract string GetUserAlias(P packet);

		// Token: 0x06001249 RID: 4681
		protected abstract bool IsJoinGameRequestPacket(P packet);

		// Token: 0x0600124A RID: 4682
		protected abstract bool IsLeaveGameRequestPacket(P packet);

		// Token: 0x0600124B RID: 4683
		protected abstract P GetJoinGameEventPacket(BC basicInfo);

		// Token: 0x0600124C RID: 4684
		protected abstract P GetLeaveGameEventPacket(BC basicInfo, LeaveGameMotivation motivation);

		// Token: 0x0600124D RID: 4685
		protected abstract P GetFailConnectionResponsePacket(LeaveGameMotivation motivation);

		// Token: 0x0600124E RID: 4686
		internal abstract A GetHostID();

		// Token: 0x040016C3 RID: 5827
		internal PingMachine<A, BC> m_pingMachine;

		// Token: 0x040016C4 RID: 5828
		private float _keepAliveAccumTime;

		// Token: 0x040016C5 RID: 5829
		private List<BC> _toRemove;

		// Token: 0x040016C6 RID: 5830
		private bool _started;
	}
}
