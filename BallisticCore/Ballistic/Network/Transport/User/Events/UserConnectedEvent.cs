using System;
using System.Text;
using Aquiris.Ballistic.Network.Transport.Gameplay.State;
using Thrift.Protocol;

namespace Aquiris.Ballistic.Network.Transport.User.Events
{
	// Token: 0x0200046D RID: 1133
	[Serializable]
	public class UserConnectedEvent : TBase, TAbstractBase
	{
		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06001894 RID: 6292 RVA: 0x0001247A File Offset: 0x0001067A
		// (set) Token: 0x06001895 RID: 6293 RVA: 0x00012482 File Offset: 0x00010682
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

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06001896 RID: 6294 RVA: 0x00012497 File Offset: 0x00010697
		// (set) Token: 0x06001897 RID: 6295 RVA: 0x0001249F File Offset: 0x0001069F
		public ClientCommonMetaData ClientMetaData
		{
			get
			{
				return this._clientMetaData;
			}
			set
			{
				this.__isset.clientMetaData = true;
				this._clientMetaData = value;
			}
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x0008519C File Offset: 0x0008339C
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
					else if (tfield.Type == 12)
					{
						this.ClientMetaData = new ClientCommonMetaData();
						this.ClientMetaData.Read(iprot);
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

		// Token: 0x06001899 RID: 6297 RVA: 0x00085274 File Offset: 0x00083474
		public void Write(TProtocol oprot)
		{
			TStruct tstruct;
			tstruct..ctor("UserConnectedEvent");
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
			if (this.ClientMetaData != null && this.__isset.clientMetaData)
			{
				tfield.Name = "clientMetaData";
				tfield.Type = 12;
				tfield.ID = 2;
				oprot.WriteFieldBegin(tfield);
				this.ClientMetaData.Write(oprot);
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x00085340 File Offset: 0x00083540
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("UserConnectedEvent(");
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
			if (this.ClientMetaData != null && this.__isset.clientMetaData)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("ClientMetaData: ");
				stringBuilder.Append((this.ClientMetaData != null) ? this.ClientMetaData.ToString() : "<null>");
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x04001B2F RID: 6959
		private long _user;

		// Token: 0x04001B30 RID: 6960
		private ClientCommonMetaData _clientMetaData;

		// Token: 0x04001B31 RID: 6961
		public UserConnectedEvent.Isset __isset;

		// Token: 0x0200046E RID: 1134
		[Serializable]
		public struct Isset
		{
			// Token: 0x04001B32 RID: 6962
			public bool user;

			// Token: 0x04001B33 RID: 6963
			public bool clientMetaData;
		}
	}
}
