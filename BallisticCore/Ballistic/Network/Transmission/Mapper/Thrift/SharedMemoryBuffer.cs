using System;
using System.IO;
using Aquiris.Ballistic.Utils;
using Thrift.Transport;

namespace Aquiris.Ballistic.Network.Transmission.Mapper.Thrift
{
	// Token: 0x020003C4 RID: 964
	public class SharedMemoryBuffer : TTransport
	{
		// Token: 0x0600153C RID: 5436 RVA: 0x000105D0 File Offset: 0x0000E7D0
		internal SharedMemoryBuffer()
		{
			this.m_byteStream = new MemoryStream();
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x00073A48 File Offset: 0x00071C48
		internal void Reset(byte[] p_data = null)
		{
			this.m_byteStream.Seek(0L, SeekOrigin.Begin);
			this.m_byteStream.SetLength(0L);
			if (p_data != null)
			{
				this.m_byteStream.Write(p_data, 0, p_data.Length);
				this.m_byteStream.Seek(0L, SeekOrigin.Begin);
			}
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x00073A98 File Offset: 0x00071C98
		internal byte[] GetBuffer()
		{
			int num = (int)this.m_byteStream.Length;
			byte[] buffer = ArrayPool<byte>.GetBuffer(num);
			this.m_byteStream.Seek(0L, SeekOrigin.Begin);
			this.m_byteStream.Read(buffer, 0, num);
			return buffer;
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600153F RID: 5439 RVA: 0x00003043 File Offset: 0x00001243
		public override bool IsOpen
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x00002A31 File Offset: 0x00000C31
		public override void Open()
		{
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x00002A31 File Offset: 0x00000C31
		public override void Close()
		{
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x000105E3 File Offset: 0x0000E7E3
		public override int Read(byte[] buf, int off, int len)
		{
			return this.m_byteStream.Read(buf, off, len);
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x000105F3 File Offset: 0x0000E7F3
		public override void Write(byte[] buf, int off, int len)
		{
			this.m_byteStream.Write(buf, off, len);
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x00010603 File Offset: 0x0000E803
		protected override void Dispose(bool disposing)
		{
			if (!this._IsDisposed && disposing && this.m_byteStream != null)
			{
				this.m_byteStream.Dispose();
			}
			this._IsDisposed = true;
		}

		// Token: 0x04001872 RID: 6258
		private readonly MemoryStream m_byteStream;

		// Token: 0x04001873 RID: 6259
		private bool _IsDisposed;
	}
}
