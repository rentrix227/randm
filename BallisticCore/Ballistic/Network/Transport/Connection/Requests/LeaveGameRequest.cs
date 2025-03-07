using System;
using System.Text;
using Thrift.Protocol;

namespace Aquiris.Ballistic.Network.Transport.Connection.Requests
{
	// Token: 0x020003DB RID: 987
	[Serializable]
	public class LeaveGameRequest : TBase, TAbstractBase
	{
		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060015A8 RID: 5544 RVA: 0x000109F8 File Offset: 0x0000EBF8
		// (set) Token: 0x060015A9 RID: 5545 RVA: 0x00010A00 File Offset: 0x0000EC00
		public short Motivation
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

		// Token: 0x060015AA RID: 5546 RVA: 0x000752C4 File Offset: 0x000734C4
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
				else if (tfield.Type == 6)
				{
					this.Motivation = iprot.ReadI16();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tfield.Type);
				}
				iprot.ReadFieldEnd();
			}
			iprot.ReadStructEnd();
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x00075358 File Offset: 0x00073558
		public void Write(TProtocol oprot)
		{
			TStruct tstruct;
			tstruct..ctor("LeaveGameRequest");
			oprot.WriteStructBegin(tstruct);
			TField tfield = default(TField);
			if (this.__isset.motivation)
			{
				tfield.Name = "motivation";
				tfield.Type = 6;
				tfield.ID = 1;
				oprot.WriteFieldBegin(tfield);
				oprot.WriteI16(this.Motivation);
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x000753D4 File Offset: 0x000735D4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("LeaveGameRequest(");
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

		// Token: 0x040018BD RID: 6333
		private short _motivation;

		// Token: 0x040018BE RID: 6334
		public LeaveGameRequest.Isset __isset;

		// Token: 0x020003DC RID: 988
		[Serializable]
		public struct Isset
		{
			// Token: 0x040018BF RID: 6335
			public bool motivation;
		}
	}
}
