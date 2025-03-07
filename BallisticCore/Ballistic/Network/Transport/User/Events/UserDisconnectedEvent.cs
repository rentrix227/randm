using System;
using System.Text;
using Aquiris.Ballistic.Network.Transport.Gameplay.State;
using Thrift.Protocol;

namespace Aquiris.Ballistic.Network.Transport.User.Events
{
	// Token: 0x0200046F RID: 1135
	[Serializable]
	public class UserDisconnectedEvent : TBase, TAbstractBase
	{
		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x0600189C RID: 6300 RVA: 0x000124B4 File Offset: 0x000106B4
		// (set) Token: 0x0600189D RID: 6301 RVA: 0x000124BC File Offset: 0x000106BC
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

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x0600189E RID: 6302 RVA: 0x000124D1 File Offset: 0x000106D1
		// (set) Token: 0x0600189F RID: 6303 RVA: 0x000124D9 File Offset: 0x000106D9
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

		// Token: 0x060018A0 RID: 6304 RVA: 0x0008540C File Offset: 0x0008360C
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

		// Token: 0x060018A1 RID: 6305 RVA: 0x000854E4 File Offset: 0x000836E4
		public void Write(TProtocol oprot)
		{
			TStruct tstruct;
			tstruct..ctor("UserDisconnectedEvent");
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

		// Token: 0x060018A2 RID: 6306 RVA: 0x000855B0 File Offset: 0x000837B0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("UserDisconnectedEvent(");
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

		// Token: 0x04001B34 RID: 6964
		private long _user;

		// Token: 0x04001B35 RID: 6965
		private ClientCommonMetaData _clientMetaData;

		// Token: 0x04001B36 RID: 6966
		public UserDisconnectedEvent.Isset __isset;

		// Token: 0x02000470 RID: 1136
		[Serializable]
		public struct Isset
		{
			// Token: 0x04001B37 RID: 6967
			public bool user;

			// Token: 0x04001B38 RID: 6968
			public bool clientMetaData;
		}
	}
}
