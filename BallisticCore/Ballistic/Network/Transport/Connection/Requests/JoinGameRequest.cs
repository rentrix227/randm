using System;
using System.Text;
using Aquiris.Ballistic.Network.Transport.Gameplay.State;
using Thrift.Protocol;

namespace Aquiris.Ballistic.Network.Transport.Connection.Requests
{
	// Token: 0x020003D8 RID: 984
	[Serializable]
	public class JoinGameRequest : TBase, TAbstractBase
	{
		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06001596 RID: 5526 RVA: 0x00010967 File Offset: 0x0000EB67
		// (set) Token: 0x06001597 RID: 5527 RVA: 0x0001096F File Offset: 0x0000EB6F
		public byte[] AuthTicket
		{
			get
			{
				return this._AuthTicket;
			}
			set
			{
				this.__isset.AuthTicket = true;
				this._AuthTicket = value;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06001598 RID: 5528 RVA: 0x00010984 File Offset: 0x0000EB84
		// (set) Token: 0x06001599 RID: 5529 RVA: 0x0001098C File Offset: 0x0000EB8C
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

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600159A RID: 5530 RVA: 0x000109A1 File Offset: 0x0000EBA1
		// (set) Token: 0x0600159B RID: 5531 RVA: 0x000109A9 File Offset: 0x0000EBA9
		public string Version
		{
			get
			{
				return this._version;
			}
			set
			{
				this.__isset.version = true;
				this._version = value;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600159C RID: 5532 RVA: 0x000109BE File Offset: 0x0000EBBE
		// (set) Token: 0x0600159D RID: 5533 RVA: 0x000109C6 File Offset: 0x0000EBC6
		public EClientMode ClientMode
		{
			get
			{
				return this._clientMode;
			}
			set
			{
				this.__isset.clientMode = true;
				this._clientMode = value;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600159E RID: 5534 RVA: 0x000109DB File Offset: 0x0000EBDB
		// (set) Token: 0x0600159F RID: 5535 RVA: 0x000109E3 File Offset: 0x0000EBE3
		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				this.__isset.password = true;
				this._password = value;
			}
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x00074D58 File Offset: 0x00072F58
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
				switch (tfield.ID)
				{
				case 1:
					if (tfield.Type == 11)
					{
						this.AuthTicket = iprot.ReadBinary();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tfield.Type);
					}
					break;
				case 2:
					if (tfield.Type == 11)
					{
						this.UserAlias = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tfield.Type);
					}
					break;
				case 3:
					if (tfield.Type == 11)
					{
						this.Version = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tfield.Type);
					}
					break;
				case 4:
					if (tfield.Type == 8)
					{
						this.ClientMode = (EClientMode)iprot.ReadI32();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tfield.Type);
					}
					break;
				case 5:
					if (tfield.Type == 11)
					{
						this.Password = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tfield.Type);
					}
					break;
				default:
					TProtocolUtil.Skip(iprot, tfield.Type);
					break;
				}
				iprot.ReadFieldEnd();
			}
			iprot.ReadStructEnd();
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x00074EC4 File Offset: 0x000730C4
		public void Write(TProtocol oprot)
		{
			TStruct tstruct;
			tstruct..ctor("JoinGameRequest");
			oprot.WriteStructBegin(tstruct);
			TField tfield = default(TField);
			if (this.AuthTicket != null && this.__isset.AuthTicket)
			{
				tfield.Name = "AuthTicket";
				tfield.Type = 11;
				tfield.ID = 1;
				oprot.WriteFieldBegin(tfield);
				oprot.WriteBinary(this.AuthTicket);
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
			if (this.Version != null && this.__isset.version)
			{
				tfield.Name = "version";
				tfield.Type = 11;
				tfield.ID = 3;
				oprot.WriteFieldBegin(tfield);
				oprot.WriteString(this.Version);
				oprot.WriteFieldEnd();
			}
			if (this.__isset.clientMode)
			{
				tfield.Name = "clientMode";
				tfield.Type = 8;
				tfield.ID = 4;
				oprot.WriteFieldBegin(tfield);
				oprot.WriteI32((int)this.ClientMode);
				oprot.WriteFieldEnd();
			}
			if (this.Password != null && this.__isset.password)
			{
				tfield.Name = "password";
				tfield.Type = 11;
				tfield.ID = 5;
				oprot.WriteFieldBegin(tfield);
				oprot.WriteString(this.Password);
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x00075084 File Offset: 0x00073284
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("JoinGameRequest(");
			bool flag = true;
			if (this.AuthTicket != null && this.__isset.AuthTicket)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				flag = false;
				stringBuilder.Append("AuthTicket: ");
				stringBuilder.Append(this.AuthTicket);
			}
			if (this.UserAlias != null && this.__isset.userAlias)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				flag = false;
				stringBuilder.Append("UserAlias: ");
				stringBuilder.Append(this.UserAlias);
			}
			if (this.Version != null && this.__isset.version)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				flag = false;
				stringBuilder.Append("Version: ");
				stringBuilder.Append(this.Version);
			}
			if (this.__isset.clientMode)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				flag = false;
				stringBuilder.Append("ClientMode: ");
				stringBuilder.Append(this.ClientMode);
			}
			if (this.Password != null && this.__isset.password)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("Password: ");
				stringBuilder.Append(this.Password);
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x040018B2 RID: 6322
		private byte[] _AuthTicket;

		// Token: 0x040018B3 RID: 6323
		private string _userAlias;

		// Token: 0x040018B4 RID: 6324
		private string _version;

		// Token: 0x040018B5 RID: 6325
		private EClientMode _clientMode;

		// Token: 0x040018B6 RID: 6326
		private string _password;

		// Token: 0x040018B7 RID: 6327
		public JoinGameRequest.Isset __isset;

		// Token: 0x020003D9 RID: 985
		[Serializable]
		public struct Isset
		{
			// Token: 0x040018B8 RID: 6328
			public bool AuthTicket;

			// Token: 0x040018B9 RID: 6329
			public bool userAlias;

			// Token: 0x040018BA RID: 6330
			public bool version;

			// Token: 0x040018BB RID: 6331
			public bool clientMode;

			// Token: 0x040018BC RID: 6332
			public bool password;
		}
	}
}
