using System;
using System.Text;
using Thrift.Protocol;

namespace Aquiris.Ballistic.Network.Transport.Connection.Requests
{
	// Token: 0x020003DA RID: 986
	[Serializable]
	public class KeepAliveRequest : TBase, TAbstractBase
	{
		// Token: 0x060015A4 RID: 5540 RVA: 0x00075214 File Offset: 0x00073414
		public void Read(TProtocol iprot)
		{
			iprot.ReadStructBegin();
			for (;;)
			{
				TField tfield = iprot.ReadFieldBegin();
				if (tfield.Type == null)
				{
					break;
				}
				short id = tfield.ID;
				TProtocolUtil.Skip(iprot, tfield.Type);
				iprot.ReadFieldEnd();
			}
			iprot.ReadStructEnd();
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x0007526C File Offset: 0x0007346C
		public void Write(TProtocol oprot)
		{
			TStruct tstruct;
			tstruct..ctor("KeepAliveRequest");
			oprot.WriteStructBegin(tstruct);
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x00075298 File Offset: 0x00073498
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("KeepAliveRequest(");
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}
