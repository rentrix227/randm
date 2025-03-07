using System;
using System.Collections.Generic;
using Aquiris.Ballistic.Network.Address;
using Aquiris.Ballistic.Network.Transmission;
using Aquiris.Ballistic.Network.Transmission.Events;
using Aquiris.Ballistic.Network.Transmission.Packet;

namespace Aquiris.Ballistic.Network.Channel
{
	// Token: 0x02000345 RID: 837
	public class ChannelManager<P, A, O, C, NA, PT> where P : Packet<O, C> where NA : NetworkAddress<A> where PT : PacketTransmitter<P, A, O, C, NA>
	{
		// Token: 0x060011E7 RID: 4583 RVA: 0x00065588 File Offset: 0x00063788
		public ChannelManager(PT p_transmitter)
		{
			this.m_eventActionMap = new Dictionary<C, ChannelManager<P, A, O, C, NA, PT>.PacketReceivedEventListener>();
			this.m_transmitter = p_transmitter;
			this.m_eventHandler = new EventHandler<PacketReceivedEventArgs<P, A, O, C, NA>>(this.OnPacketReceived);
			this.m_transmitter.onPacketReceived += this.m_eventHandler;
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x000655D8 File Offset: 0x000637D8
		~ChannelManager()
		{
			this.m_transmitter.onPacketReceived -= this.m_eventHandler;
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x00065618 File Offset: 0x00063818
		internal void AddChannelListener(C p_channel, ChannelManager<P, A, O, C, NA, PT>.PacketReceivedEventListener p_listener)
		{
			if (!this.m_eventActionMap.ContainsKey(p_channel))
			{
				this.m_eventActionMap[p_channel] = p_listener;
			}
			else
			{
				Dictionary<C, ChannelManager<P, A, O, C, NA, PT>.PacketReceivedEventListener> eventActionMap;
				(eventActionMap = this.m_eventActionMap)[p_channel] = (ChannelManager<P, A, O, C, NA, PT>.PacketReceivedEventListener)Delegate.Combine(eventActionMap[p_channel], p_listener);
			}
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x0006566C File Offset: 0x0006386C
		internal void RemoveChannelListener(C p_channel, ChannelManager<P, A, O, C, NA, PT>.PacketReceivedEventListener p_listener)
		{
			if (this.m_eventActionMap.ContainsKey(p_channel))
			{
				Dictionary<C, ChannelManager<P, A, O, C, NA, PT>.PacketReceivedEventListener> eventActionMap;
				(eventActionMap = this.m_eventActionMap)[p_channel] = (ChannelManager<P, A, O, C, NA, PT>.PacketReceivedEventListener)Delegate.Remove(eventActionMap[p_channel], p_listener);
			}
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x000656AC File Offset: 0x000638AC
		protected void OnPacketReceived(object p_sender, PacketReceivedEventArgs<P, A, O, C, NA> p_packetArgs)
		{
			P packet = p_packetArgs.Packet;
			C transportChannel = packet.TransportChannel;
			ChannelManager<P, A, O, C, NA, PT>.PacketReceivedEventListener packetReceivedEventListener;
			if (this.m_eventActionMap.TryGetValue(transportChannel, out packetReceivedEventListener))
			{
				packetReceivedEventListener(p_packetArgs);
			}
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x0000EAB1 File Offset: 0x0000CCB1
		internal bool SendPacket(P p_packet, NA p_networkAddress)
		{
			return this.m_transmitter.SendPacket(p_packet, p_networkAddress);
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x0000EAC6 File Offset: 0x0000CCC6
		internal virtual void UpdateConnection(float p_deltaTime)
		{
			this.m_transmitter.UpdateTransmitter(p_deltaTime);
		}

		// Token: 0x0400169B RID: 5787
		private Dictionary<C, ChannelManager<P, A, O, C, NA, PT>.PacketReceivedEventListener> m_eventActionMap;

		// Token: 0x0400169C RID: 5788
		protected PT m_transmitter;

		// Token: 0x0400169D RID: 5789
		private EventHandler<PacketReceivedEventArgs<P, A, O, C, NA>> m_eventHandler;

		// Token: 0x02000346 RID: 838
		// (Invoke) Token: 0x060011EF RID: 4591
		internal delegate void PacketReceivedEventListener(PacketReceivedEventArgs<P, A, O, C, NA> args);
	}
}
