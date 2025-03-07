using System;
using System.Text;
using Thrift.Protocol;

namespace Aquiris.Ballistic.Network.Transport.Connection.Events
{
	// Token: 0x020003D1 RID: 977
	[Serializable]
	public class LeaveGameEvent : TBase, TAbstractBase
	{
		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06001580 RID: 5504 RVA: 0x000108D6 File Offset: 0x0000EAD6
		// (set) Token: 0x06001581 RID: 5505 RVA: 0x000108DE File Offset: 0x0000EADE
		public long User
		{
			get
			{
				return this._user;
			}
			set
			{
				this.__isset.user = true;
				this._user = value;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06001582 RID: 5506 RVA: 0x000108F3 File Offset: 0x0000EAF3
		// (set) Token: 0x06001583 RID: 5507 RVA: 0x000108FB File Offset: 0x0000EAFB
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

		// Token: 0x06001584 RID: 5508 RVA: 0x00074744 File Offset: 0x00072944
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
					if (id != 2)
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
				}
				else if (tfield.Type == 10)
				{
					this.User = iprot.ReadI64();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tfield.Type);
				}
				iprot.ReadFieldEnd();
			}
			iprot.ReadStructEnd();
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x00074810 File Offset: 0x00072A10
		public void Write(TProtocol oprot)
		{
			TStruct tstruct;
			tstruct..ctor("LeaveGameEvent");
			oprot.WriteStructBegin(tstruct);
			TField tfield = default(TField);
			if (this.__isset.user)
			{
				tfield.Name = "user";
				tfield.Type = 10;
				tfield.ID = 1;
				oprot.WriteFieldBegin(tfield);
				oprot.WriteI64(this.User);
				oprot.WriteFieldEnd();
			}
			if (this.__isset.motivation)
			{
				tfield.Name = "motivation";
				tfield.Type = 6;
				tfield.ID = 2;
				oprot.WriteFieldBegin(tfield);
				oprot.WriteI16(this.Motivation);
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x000748D0 File Offset: 0x00072AD0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("LeaveGameEvent(");
			bool flag = true;
			if (this.__isset.user)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				flag = false;
				stringBuilder.Append("User: ");
				stringBuilder.Append(this.User);
			}
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

		// Token: 0x040018A2 RID: 6306
		private long _user;

		// Token: 0x040018A3 RID: 6307
		private short _motivation;

		// Token: 0x040018A4 RID: 6308
		public LeaveGameEvent.Isset __isset;

		// Token: 0x020003D2 RID: 978
		[Serializable]
		public struct Isset
		{
			// Token: 0x040018A5 RID: 6309
			public bool user;

			// Token: 0x040018A6 RID: 6310
			public bool motivation;
		}
	}
}
