using System;
using System.Diagnostics;
using Aquiris.Ballistic.Network.Address;
using Aquiris.Ballistic.Network.Transmission.Events;
using Aquiris.Ballistic.Network.Transmission.Packet;
using Aquiris.Ballistic.Network.Utils;

namespace Aquiris.Ballistic.Network.Transmission
{
	// Token: 0x020003CB RID: 971
	public abstract class PacketTransmitter<P, A, O, C, NA> where P : Packet<O, C> where NA : NetworkAddress<A>
	{
		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06001559 RID: 5465 RVA: 0x00073DD0 File Offset: 0x00071FD0
		// (remove) Token: 0x0600155A RID: 5466 RVA: 0x00073E08 File Offset: 0x00072008
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event EventHandler<PacketReceivedEventArgs<P, A, O, C, NA>> onPacketReceived;

		// Token: 0x0600155B RID: 5467
		internal abstract bool SendPacket(P p_packet, NA p_networkAddress);

		// Token: 0x0600155C RID: 5468 RVA: 0x00073E40 File Offset: 0x00072040
		protected void receivePacket(P p_packet, NA p_sender)
		{
			p_sender.LastInteractionTime = TimeUtils.getCurrentTime();
			p_sender.CheckingKeepAlive = false;
			if (this.onPacketReceived != null)
			{
				this._reclycledArgs.Packet = p_packet;
				this._reclycledArgs.Sender = p_sender;
				this.onPacketReceived(this, this._reclycledArgs);
			}
		}

		// Token: 0x0600155D RID: 5469
		internal abstract void UpdateTransmitter(float p_deltaTime);

		// Token: 0x0600155E RID: 5470 RVA: 0x00002A31 File Offset: 0x00000C31
		internal virtual void Close()
		{
		}

		// Token: 0x04001891 RID: 6289
		private PacketReceivedEventArgs<P, A, O, C, NA> _reclycledArgs = new PacketReceivedEventArgs<P, A, O, C, NA>();
	}
}
