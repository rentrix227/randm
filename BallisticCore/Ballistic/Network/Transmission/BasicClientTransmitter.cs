using System;
using Aquiris.Ballistic.Network.Address;
using Aquiris.Ballistic.Network.Transmission.Packet;

namespace Aquiris.Ballistic.Network.Transmission
{
	// Token: 0x020003BE RID: 958
	public abstract class BasicClientTransmitter<P, A, O, C, NA> : PacketTransmitter<P, A, O, C, NA> where P : Packet<O, C> where NA : NetworkAddress<A>
	{
		// Token: 0x06001520 RID: 5408 RVA: 0x000104BC File Offset: 0x0000E6BC
		public BasicClientTransmitter(NA p_basicHost)
		{
			this.BasicHost = p_basicHost;
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06001521 RID: 5409 RVA: 0x000104CB File Offset: 0x0000E6CB
		// (set) Token: 0x06001522 RID: 5410 RVA: 0x000104D3 File Offset: 0x0000E6D3
		internal NA BasicHost { get; set; }

		// Token: 0x06001523 RID: 5411 RVA: 0x000104DC File Offset: 0x0000E6DC
		internal bool sendPacketToHost(P p_packet)
		{
			if (this.BasicHost != null)
			{
				return this.SendPacket(p_packet, this.BasicHost);
			}
			throw new InvalidOperationException("Host is not set");
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x00010506 File Offset: 0x0000E706
		internal override void Close()
		{
			this.BasicHost = (NA)((object)null);
			base.Close();
		}
	}
}
