using System;
using System.Text;
using Thrift.Protocol;

namespace Aquiris.Ballistic.Network.Transport.Connection.Events
{
	// Token: 0x020003D5 RID: 981
	[Serializable]
	public class ServerTerminationEvent : TBase, TAbstractBase
	{
		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06001590 RID: 5520 RVA: 0x0001094A File Offset: 0x0000EB4A
		// (set) Token: 0x06001591 RID: 5521 RVA: 0x00010952 File Offset: 0x0000EB52
		public ServerTerminationReason Reason
		{
			get
			{
				return this._reason;
			}
			set
			{
				this.__isset.reason = true;
				this._reason = value;
			}
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x00074BD8 File Offset: 0x00072DD8
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
				else if (tfield.Type == 8)
				{
					this.Reason = (ServerTerminationReason)iprot.ReadI32();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tfield.Type);
				}
				iprot.ReadFieldEnd();
			}
			iprot.ReadStructEnd();
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x00074C6C File Offset: 0x00072E6C
		public void Write(TProtocol oprot)
		{
			TStruct tstruct;
			tstruct..ctor("ServerTerminationEvent");
			oprot.WriteStructBegin(tstruct);
			TField tfield = default(TField);
			if (this.__isset.reason)
			{
				tfield.Name = "reason";
				tfield.Type = 8;
				tfield.ID = 1;
				oprot.WriteFieldBegin(tfield);
				oprot.WriteI32((int)this.Reason);
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x00074CE8 File Offset: 0x00072EE8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("ServerTerminationEvent(");
			bool flag = true;
			if (this.__isset.reason)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("Reason: ");
				stringBuilder.Append(this.Reason);
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x040018AC RID: 6316
		private ServerTerminationReason _reason;

		// Token: 0x040018AD RID: 6317
		public ServerTerminationEvent.Isset __isset;

		// Token: 0x020003D6 RID: 982
		[Serializable]
		public struct Isset
		{
			// Token: 0x040018AE RID: 6318
			public bool reason;
		}
	}
}
