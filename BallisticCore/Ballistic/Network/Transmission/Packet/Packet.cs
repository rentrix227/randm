using System;

namespace Aquiris.Ballistic.Network.Transmission.Packet
{
	// Token: 0x020003C8 RID: 968
	public class Packet<O, C>
	{
		// Token: 0x0600154E RID: 5454 RVA: 0x000106CD File Offset: 0x0000E8CD
		public Packet()
		{
			this.TransportType = ETransportType.Unreliable;
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x0600154F RID: 5455 RVA: 0x000106DC File Offset: 0x0000E8DC
		// (set) Token: 0x06001550 RID: 5456 RVA: 0x000106E4 File Offset: 0x0000E8E4
		internal O TransportObject { get; set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06001551 RID: 5457 RVA: 0x000106ED File Offset: 0x0000E8ED
		// (set) Token: 0x06001552 RID: 5458 RVA: 0x000106F5 File Offset: 0x0000E8F5
		internal C TransportChannel { get; set; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06001553 RID: 5459 RVA: 0x000106FE File Offset: 0x0000E8FE
		// (set) Token: 0x06001554 RID: 5460 RVA: 0x00010706 File Offset: 0x0000E906
		internal ETransportType TransportType { get; set; }

		// Token: 0x06001555 RID: 5461 RVA: 0x0001070F File Offset: 0x0000E90F
		internal virtual bool isValid()
		{
			return this.TransportObject != null && this.TransportChannel != null;
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x00073D90 File Offset: 0x00071F90
		public override string ToString()
		{
			string text = "CHANNEL = {0}; OBJECT = {1}";
			C transportChannel = this.TransportChannel;
			object obj = transportChannel.ToString();
			O transportObject = this.TransportObject;
			return string.Format(text, obj, transportObject.ToString());
		}
	}
}
