using System;
using System.Text;
using Thrift.Protocol;

namespace Aquiris.Ballistic.Network.Transport.Player
{
	// Token: 0x0200046B RID: 1131
	[Serializable]
	public class PlayerStateEvent : TBase, TAbstractBase
	{
		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x0600188C RID: 6284 RVA: 0x00012440 File Offset: 0x00010640
		// (set) Token: 0x0600188D RID: 6285 RVA: 0x00012448 File Offset: 0x00010648
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

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x0600188E RID: 6286 RVA: 0x0001245D File Offset: 0x0001065D
		// (set) Token: 0x0600188F RID: 6287 RVA: 0x00012465 File Offset: 0x00010665
		public byte[] PlayerState
		{
			get
			{
				return this._playerState;
			}
			set
			{
				this.__isset.playerState = true;
				this._playerState = value;
			}
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x00084F50 File Offset: 0x00083150
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
						this.PlayerState = iprot.ReadBinary();
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

		// Token: 0x06001891 RID: 6289 RVA: 0x0008501C File Offset: 0x0008321C
		public void Write(TProtocol oprot)
		{
			TStruct tstruct;
			tstruct..ctor("PlayerStateEvent");
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
			if (this.PlayerState != null && this.__isset.playerState)
			{
				tfield.Name = "playerState";
				tfield.Type = 11;
				tfield.ID = 2;
				oprot.WriteFieldBegin(tfield);
				oprot.WriteBinary(this.PlayerState);
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x000850E8 File Offset: 0x000832E8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("PlayerStateEvent(");
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
			if (this.PlayerState != null && this.__isset.playerState)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("PlayerState: ");
				stringBuilder.Append(this.PlayerState);
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x04001B2A RID: 6954
		private long _user;

		// Token: 0x04001B2B RID: 6955
		private byte[] _playerState;

		// Token: 0x04001B2C RID: 6956
		public PlayerStateEvent.Isset __isset;

		// Token: 0x0200046C RID: 1132
		[Serializable]
		public struct Isset
		{
			// Token: 0x04001B2D RID: 6957
			public bool user;

			// Token: 0x04001B2E RID: 6958
			public bool playerState;
		}
	}
}
