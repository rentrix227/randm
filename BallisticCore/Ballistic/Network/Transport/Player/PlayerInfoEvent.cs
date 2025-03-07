using System;
using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Aquiris.Ballistic.Network.Transport.Player
{
	// Token: 0x02000469 RID: 1129
	[Serializable]
	public class PlayerInfoEvent : TBase, TAbstractBase
	{
		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06001884 RID: 6276 RVA: 0x00012406 File Offset: 0x00010606
		// (set) Token: 0x06001885 RID: 6277 RVA: 0x0001240E File Offset: 0x0001060E
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

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06001886 RID: 6278 RVA: 0x00012423 File Offset: 0x00010623
		// (set) Token: 0x06001887 RID: 6279 RVA: 0x0001242B File Offset: 0x0001062B
		public Dictionary<short, short> PlayerLevels
		{
			get
			{
				return this._playerLevels;
			}
			set
			{
				this.__isset.playerLevels = true;
				this._playerLevels = value;
			}
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x00084C48 File Offset: 0x00082E48
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
					else if (tfield.Type == 13)
					{
						this.PlayerLevels = new Dictionary<short, short>();
						TMap tmap = iprot.ReadMapBegin();
						for (int i = 0; i < tmap.Count; i++)
						{
							short num = iprot.ReadI16();
							short num2 = iprot.ReadI16();
							this.PlayerLevels[num] = num2;
						}
						iprot.ReadMapEnd();
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

		// Token: 0x06001889 RID: 6281 RVA: 0x00084D58 File Offset: 0x00082F58
		public void Write(TProtocol oprot)
		{
			TStruct tstruct;
			tstruct..ctor("PlayerInfoEvent");
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
			if (this.PlayerLevels != null && this.__isset.playerLevels)
			{
				tfield.Name = "playerLevels";
				tfield.Type = 13;
				tfield.ID = 2;
				oprot.WriteFieldBegin(tfield);
				oprot.WriteMapBegin(new TMap(6, 6, this.PlayerLevels.Count));
				foreach (short num in this.PlayerLevels.Keys)
				{
					oprot.WriteI16(num);
					oprot.WriteI16(this.PlayerLevels[num]);
				}
				oprot.WriteMapEnd();
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x00084E9C File Offset: 0x0008309C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("PlayerInfoEvent(");
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
			if (this.PlayerLevels != null && this.__isset.playerLevels)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("PlayerLevels: ");
				stringBuilder.Append(this.PlayerLevels);
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x04001B25 RID: 6949
		private long _user;

		// Token: 0x04001B26 RID: 6950
		private Dictionary<short, short> _playerLevels;

		// Token: 0x04001B27 RID: 6951
		public PlayerInfoEvent.Isset __isset;

		// Token: 0x0200046A RID: 1130
		[Serializable]
		public struct Isset
		{
			// Token: 0x04001B28 RID: 6952
			public bool user;

			// Token: 0x04001B29 RID: 6953
			public bool playerLevels;
		}
	}
}
