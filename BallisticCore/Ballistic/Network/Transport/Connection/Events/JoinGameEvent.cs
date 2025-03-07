using System;
using System.Text;
using Thrift.Protocol;

namespace Aquiris.Ballistic.Network.Transport.Connection.Events
{
	// Token: 0x020003CF RID: 975
	[Serializable]
	public class JoinGameEvent : TBase, TAbstractBase
	{
		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06001578 RID: 5496 RVA: 0x0001089C File Offset: 0x0000EA9C
		// (set) Token: 0x06001579 RID: 5497 RVA: 0x000108A4 File Offset: 0x0000EAA4
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

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x0600157A RID: 5498 RVA: 0x000108B9 File Offset: 0x0000EAB9
		// (set) Token: 0x0600157B RID: 5499 RVA: 0x000108C1 File Offset: 0x0000EAC1
		public string UserAlias
		{
			get
			{
				return this._userAlias;
			}
			set
			{
				this.__isset.userAlias = true;
				this._userAlias = value;
			}
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x000744F8 File Offset: 0x000726F8
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
					else if (tfield.Type == 11)
					{
						this.UserAlias = iprot.ReadString();
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

		// Token: 0x0600157D RID: 5501 RVA: 0x000745C4 File Offset: 0x000727C4
		public void Write(TProtocol oprot)
		{
			TStruct tstruct;
			tstruct..ctor("JoinGameEvent");
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
			if (this.UserAlias != null && this.__isset.userAlias)
			{
				tfield.Name = "userAlias";
				tfield.Type = 11;
				tfield.ID = 2;
				oprot.WriteFieldBegin(tfield);
				oprot.WriteString(this.UserAlias);
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x00074690 File Offset: 0x00072890
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("JoinGameEvent(");
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
			if (this.UserAlias != null && this.__isset.userAlias)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("UserAlias: ");
				stringBuilder.Append(this.UserAlias);
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x0400189D RID: 6301
		private long _user;

		// Token: 0x0400189E RID: 6302
		private string _userAlias;

		// Token: 0x0400189F RID: 6303
		public JoinGameEvent.Isset __isset;

		// Token: 0x020003D0 RID: 976
		[Serializable]
		public struct Isset
		{
			// Token: 0x040018A0 RID: 6304
			public bool user;

			// Token: 0x040018A1 RID: 6305
			public bool userAlias;
		}
	}
}
