using System;
using System.Text;
using Thrift.Protocol;

namespace Aquiris.Ballistic.Network.Transport.Connection.Responses
{
	// Token: 0x020003DF RID: 991
	[Serializable]
	public class KeepAliveResponse : TBase, TAbstractBase
	{
		// Token: 0x060015B4 RID: 5556 RVA: 0x00075214 File Offset: 0x00073414
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

		// Token: 0x060015B5 RID: 5557 RVA: 0x000755BC File Offset: 0x000737BC
		public void Write(TProtocol oprot)
		{
			TStruct tstruct;
			tstruct..ctor("KeepAliveResponse");
			oprot.WriteStructBegin(tstruct);
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x000755E8 File Offset: 0x000737E8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("KeepAliveResponse(");
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}
