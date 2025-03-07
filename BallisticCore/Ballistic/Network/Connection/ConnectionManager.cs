using System;
using System.Collections.Generic;
using System.Diagnostics;
using Aquiris.Ballistic.Network.Address;
using Aquiris.Ballistic.Network.Channel;
using Aquiris.Ballistic.Network.Connection.Events;
using Aquiris.Ballistic.Network.Transmission;
using Aquiris.Ballistic.Network.Transmission.Events;
using Aquiris.Ballistic.Network.Transmission.Packet;

namespace Aquiris.Ballistic.Network.Connection
{
	// Token: 0x0200034C RID: 844
	public abstract class ConnectionManager<P, A, O, C, NA, PT> : ChannelManager<P, A, O, C, NA, PT> where P : Packet<O, C> where NA : NetworkAddress<A> where PT : PacketTransmitter<P, A, O, C, NA>
	{
		// Token: 0x0600124F RID: 4687 RVA: 0x00066470 File Offset: 0x00064670
		public ConnectionManager(PT p_transmitter, C p_keepAliveChannel, C p_connectionChannel)
			: base(p_transmitter)
		{
			this.m_knownClientList = new Dictionary<A, NA>();
			this.m_knownClientListValues = this.m_knownClientList.Values;
			this.m_transmitter = p_transmitter;
			base.AddChannelListener(p_connectionChannel, new ChannelManager<P, A, O, C, NA, PT>.PacketReceivedEventListener(this.ConnectionPacketReceived));
			base.AddChannelListener(p_keepAliveChannel, new ChannelManager<P, A, O, C, NA, PT>.PacketReceivedEventListener(this.KeepAliveReceived));
		}

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06001250 RID: 4688 RVA: 0x000664D0 File Offset: 0x000646D0
		// (remove) Token: 0x06001251 RID: 4689 RVA: 0x00066508 File Offset: 0x00064708
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event EventHandler<ClientConnectionEventArgs<NA, A>> onClientConnection;

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06001252 RID: 4690 RVA: 0x00066540 File Offset: 0x00064740
		// (remove) Token: 0x06001253 RID: 4691 RVA: 0x00066578 File Offset: 0x00064778
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event EventHandler<ClientConnectionEventArgs<NA, A>> onPostClientConnection;

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x06001254 RID: 4692 RVA: 0x000665B0 File Offset: 0x000647B0
		// (remove) Token: 0x06001255 RID: 4693 RVA: 0x000665E8 File Offset: 0x000647E8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event EventHandler<ClientDisconnectionEventArgs<NA, A>> onClientDisconnection;

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x06001256 RID: 4694 RVA: 0x00066620 File Offset: 0x00064820
		// (remove) Token: 0x06001257 RID: 4695 RVA: 0x00066658 File Offset: 0x00064858
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event EventHandler<ClientDisconnectionEventArgs<NA, A>> onPostClientDisconnection;

		// Token: 0x06001258 RID: 4696
		protected abstract void ConnectionPacketReceived(PacketReceivedEventArgs<P, A, O, C, NA> p_packetArgs);

		// Token: 0x06001259 RID: 4697 RVA: 0x00066690 File Offset: 0x00064890
		internal bool BroadcastPacket(P packet)
		{
			bool flag = false;
			foreach (NA na in this.m_knownClientListValues)
			{
				if (base.SendPacket(packet, na))
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x000666F8 File Offset: 0x000648F8
		internal bool BroadcastPacket(P packet, NA ignore)
		{
			bool flag = false;
			foreach (NA na in this.m_knownClientListValues)
			{
				if (na != ignore)
				{
					if (base.SendPacket(packet, na))
					{
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x0000EDE3 File Offset: 0x0000CFE3
		internal override void UpdateConnection(float p_deltaTime)
		{
			base.UpdateConnection(p_deltaTime);
			this.RunKeepAlive(p_deltaTime);
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x00066778 File Offset: 0x00064978
		protected void DispatchClientDisconnected(NA p_Address)
		{
			if (this.onClientDisconnection != null && p_Address != null)
			{
				this.onClientDisconnection(this, new ClientDisconnectionEventArgs<NA, A>(p_Address));
			}
			this.OnClientDisconnected(p_Address);
			if (this.onPostClientDisconnection != null && p_Address != null)
			{
				this.onPostClientDisconnection(this, new ClientDisconnectionEventArgs<NA, A>(p_Address));
			}
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x000667DC File Offset: 0x000649DC
		protected void DispatchClientConnected(NA p_Address)
		{
			if (this.onClientConnection != null && p_Address != null)
			{
				this.onClientConnection(this, new ClientConnectionEventArgs<NA, A>(p_Address));
			}
			this.OnClientConnected(p_Address);
			if (this.onPostClientConnection != null && p_Address != null)
			{
				this.onPostClientConnection(this, new ClientConnectionEventArgs<NA, A>(p_Address));
			}
		}

		// Token: 0x0600125E RID: 4702
		protected abstract void RunKeepAlive(float p_deltaTime);

		// Token: 0x0600125F RID: 4703
		protected abstract void KeepAliveReceived(PacketReceivedEventArgs<P, A, O, C, NA> p_packetArgs);

		// Token: 0x06001260 RID: 4704
		protected abstract P getKeepAliveRequestPacket();

		// Token: 0x06001261 RID: 4705
		protected abstract P getKeepAliveResponsePacket();

		// Token: 0x06001262 RID: 4706
		protected abstract bool isKeepAliveRequestPacket(P p_packet);

		// Token: 0x06001263 RID: 4707
		protected abstract bool isKeepAliveResponsePacket(P p_packet);

		// Token: 0x06001264 RID: 4708
		protected abstract void OnClientDisconnected(NA p_basicClient);

		// Token: 0x06001265 RID: 4709
		protected abstract void OnClientConnected(NA p_basicClient);

		// Token: 0x040016CB RID: 5835
		protected Dictionary<A, NA> m_knownClientList;

		// Token: 0x040016CC RID: 5836
		protected Dictionary<A, NA>.ValueCollection m_knownClientListValues;
	}
}
