using System;

namespace Aquiris.Ballistic.Network.Transmission.Mapper
{
	// Token: 0x020003C2 RID: 962
	public interface PacketMapper<A, B>
	{
		// Token: 0x06001537 RID: 5431
		B compressPacket(A p_uncompressedData);

		// Token: 0x06001538 RID: 5432
		A uncompressPacket(B p_compressedData);
	}
}
