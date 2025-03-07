using System;
using System.Text;
using Thrift.Protocol;

namespace Aquiris.Ballistic.Network.Transport.Connection.Responses
{
	// Token: 0x020003DD RID: 989
	[Serializable]
	public class FailConnectionResponse : TBase, TAbstractBase
	{
		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060015AE RID: 5550 RVA: 0x00010A15 File Offset: 0x0000EC15
		// (set) Token: 0x060015AF RID: 5551 RVA: 0x00010A1D File Offset: 0x0000EC1D
		public sbyte Motivation
		{
			get
			{
				return this._motivation;
			}
			set
			{
				this.__isset.motivation = true;
				this._motivation = value;
			}
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x00075440 File Offset: 0x00073640
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
				if (id != 1)
				{
					TProtocolUtil.Skip(iprot, tfield.Type);
				}
				else if (tfield.Type == 3)
				{
					this.Motivation = iprot.ReadByte();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tfield.Type);
				}
				iprot.ReadFieldEnd();
			}
			iprot.ReadStructEnd();
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x000754D4 File Offset: 0x000736D4
		public void Write(TProtocol oprot)
		{
			TStruct tstruct;
			tstruct..ctor("FailConnectionResponse");
			oprot.WriteStructBegin(tstruct);
			TField tfield = default(TField);
			if (this.__isset.motivation)
			{
				tfield.Name = "motivation";
				tfield.Type = 3;
				tfield.ID = 1;
				oprot.WriteFieldBegin(tfield);
				oprot.WriteByte(this.Motivation);
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x00075550 File Offset: 0x00073750
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("FailConnectionResponse(");
			bool flag = true;
			if (this.__isset.motivation)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("Motivation: ");
				stringBuilder.Append(this.Motivation);
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x040018C0 RID: 6336
		private sbyte _motivation;

		// Token: 0x040018C1 RID: 6337
		public FailConnectionResponse.Isset __isset;

		// Token: 0x020003DE RID: 990
		[Serializable]
		public struct Isset
		{
			// Token: 0x040018C2 RID: 6338
			public bool motivation;
		}
	}
}
