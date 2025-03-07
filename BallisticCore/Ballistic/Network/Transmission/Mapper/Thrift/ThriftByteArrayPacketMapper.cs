using System;
using Aquiris.Ballistic.Network.Transmission.Packet.Thrift;
using Aquiris.Ballistic.Utils;
using Thrift.Protocol;

namespace Aquiris.Ballistic.Network.Transmission.Mapper.Thrift
{
	// Token: 0x020003C5 RID: 965
	public class ThriftByteArrayPacketMapper : PacketMapper<ThriftPacket, byte[]>
	{
		// Token: 0x06001545 RID: 5445 RVA: 0x00010633 File Offset: 0x0000E833
		public ThriftByteArrayPacketMapper()
		{
			this.protocol = new PooledBinaryCompactProtocol(this.transport);
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x00073AD8 File Offset: 0x00071CD8
		public byte[] compressPacket(ThriftPacket p_uncompressedData)
		{
			this.transport.Reset(null);
			this.protocol.WriteByte((sbyte)p_uncompressedData.TransportChannel);
			this.protocol.WriteByte(TypeByteGenerator.GetInstance().GetByteFromType(p_uncompressedData.TransportObject));
			p_uncompressedData.TransportObject.Write(this.protocol);
			if (this._lastBuffer != null)
			{
				ArrayPool<byte>.FreeBuffer(this._lastBuffer);
			}
			this._lastBuffer = this.transport.GetBuffer();
			return this._lastBuffer;
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x00073B5C File Offset: 0x00071D5C
		public ThriftPacket uncompressPacket(byte[] p_compressedData)
		{
			this.transport.Reset(p_compressedData);
			sbyte b = this.protocol.ReadByte();
			sbyte b2 = this.protocol.ReadByte();
			TBase typeFromByte = TypeByteGenerator.GetInstance().GetTypeFromByte(b2);
			typeFromByte.Read(this.protocol);
			ThriftPacket recycledPacket = this._recycledPacket;
			recycledPacket.TransportObject = typeFromByte;
			recycledPacket.TransportChannel = (ETransportChannel)b;
			return recycledPacket;
		}

		// Token: 0x04001874 RID: 6260
		private TProtocol protocol;

		// Token: 0x04001875 RID: 6261
		private byte[] _lastBuffer;

		// Token: 0x04001876 RID: 6262
		private readonly SharedMemoryBuffer transport = new SharedMemoryBuffer();

		// Token: 0x04001877 RID: 6263
		private readonly ThriftPacket _recycledPacket = new ThriftPacket();
	}
}
