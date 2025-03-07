using System;
using Aquiris.Ballistic.Utils;
using Thrift.Protocol;
using Thrift.Transport;

namespace Aquiris.Ballistic.Network.Transmission.Mapper.Thrift
{
	// Token: 0x020003C3 RID: 963
	internal class PooledBinaryCompactProtocol : TCompactProtocol
	{
		// Token: 0x06001539 RID: 5433 RVA: 0x000105C7 File Offset: 0x0000E7C7
		internal PooledBinaryCompactProtocol(TTransport trans)
			: base(trans)
		{
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x000739C4 File Offset: 0x00071BC4
		private uint ReadVarint32()
		{
			uint num = 0U;
			int num2 = 0;
			for (;;)
			{
				byte b = (byte)this.ReadByte();
				num |= (uint)((uint)(b & 127) << num2);
				if ((b & 128) != 128)
				{
					break;
				}
				num2 += 7;
			}
			return num;
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x00073A10 File Offset: 0x00071C10
		public override byte[] ReadBinary()
		{
			int num = (int)this.ReadVarint32();
			if (num == 0)
			{
				return ArrayPool<byte>.GetBuffer(0);
			}
			byte[] buffer = ArrayPool<byte>.GetBuffer(num);
			this.trans.ReadAll(buffer, 0, num);
			return buffer;
		}
	}
}
