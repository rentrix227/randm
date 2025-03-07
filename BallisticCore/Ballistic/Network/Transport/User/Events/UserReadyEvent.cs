using System;
using System.Text;
using Thrift.Protocol;

namespace Aquiris.Ballistic.Network.Transport.User.Events
{
	// Token: 0x02000471 RID: 1137
	[Serializable]
	public class UserReadyEvent : TBase, TAbstractBase
	{
		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x060018A4 RID: 6308 RVA: 0x000124EE File Offset: 0x000106EE
		// (set) Token: 0x060018A5 RID: 6309 RVA: 0x000124F6 File Offset: 0x000106F6
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

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x060018A6 RID: 6310 RVA: 0x0001250B File Offset: 0x0001070B
		// (set) Token: 0x060018A7 RID: 6311 RVA: 0x00012513 File Offset: 0x00010713
		public bool IsReady
		{
			get
			{
				return this._isReady;
			}
			set
			{
				this.__isset.isReady = true;
				this._isReady = value;
			}
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x0008567C File Offset: 0x0008387C
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
					else if (tfield.Type == 2)
					{
						this.IsReady = iprot.ReadBool();
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

		// Token: 0x060018A9 RID: 6313 RVA: 0x00085748 File Offset: 0x00083948
		public void Write(TProtocol oprot)
		{
			TStruct tstruct;
			tstruct..ctor("UserReadyEvent");
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
			if (this.__isset.isReady)
			{
				tfield.Name = "isReady";
				tfield.Type = 2;
				tfield.ID = 2;
				oprot.WriteFieldBegin(tfield);
				oprot.WriteBool(this.IsReady);
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x00085808 File Offset: 0x00083A08
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("UserReadyEvent(");
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
			if (this.__isset.isReady)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("IsReady: ");
				stringBuilder.Append(this.IsReady);
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x04001B39 RID: 6969
		private long _user;

		// Token: 0x04001B3A RID: 6970
		private bool _isReady;

		// Token: 0x04001B3B RID: 6971
		public UserReadyEvent.Isset __isset;

		// Token: 0x02000472 RID: 1138
		[Serializable]
		public struct Isset
		{
			// Token: 0x04001B3C RID: 6972
			public bool user;

			// Token: 0x04001B3D RID: 6973
			public bool isReady;
		}
	}
}
