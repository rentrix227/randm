using System;
using Aquiris.Ballistic.Network.Address;
using Aquiris.Ballistic.Network.Transmission.Packet;

namespace Aquiris.Ballistic.Network.Transmission.Events
{
	// Token: 0x020003C0 RID: 960
	public class PacketReceivedEventArgs<P, A, O, C, NA> : EventArgs where P : Packet<O, C> where NA : NetworkAddress<A>
	{
		// Token: 0x0600152D RID: 5421 RVA: 0x0000EE33 File Offset: 0x0000D033
		internal PacketReceivedEventArgs()
		{
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x0001056A File Offset: 0x0000E76A
		internal PacketReceivedEventArgs(P p_packet, NA p_sender)
		{
			this.Sender = p_sender;
			this.Packet = p_packet;
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x0600152F RID: 5423 RVA: 0x00010580 File Offset: 0x0000E780
		// (set) Token: 0x06001530 RID: 5424 RVA: 0x00010588 File Offset: 0x0000E788
		internal P Packet { get; set; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06001531 RID: 5425 RVA: 0x00010591 File Offset: 0x0000E791
		// (set) Token: 0x06001532 RID: 5426 RVA: 0x00010599 File Offset: 0x0000E799
		internal NA Sender { get; set; }
	}
}
