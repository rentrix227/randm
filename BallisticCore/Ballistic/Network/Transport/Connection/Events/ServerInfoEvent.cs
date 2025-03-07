using System;
using System.Text;
using Thrift.Protocol;

namespace Aquiris.Ballistic.Network.Transport.Connection.Events
{
	// Token: 0x020003D3 RID: 979
	[Serializable]
	public class ServerInfoEvent : TBase, TAbstractBase
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06001588 RID: 5512 RVA: 0x00010910 File Offset: 0x0000EB10
		// (set) Token: 0x06001589 RID: 5513 RVA: 0x00010918 File Offset: 0x0000EB18
		public string Banner
		{
			get
			{
				return this._banner;
			}
			set
			{
				this.__isset.banner = true;
				this._banner = value;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x0600158A RID: 5514 RVA: 0x0001092D File Offset: 0x0000EB2D
		// (set) Token: 0x0600158B RID: 5515 RVA: 0x00010935 File Offset: 0x0000EB35
		public string Click
		{
			get
			{
				return this._click;
			}
			set
			{
				this.__isset.click = true;
				this._click = value;
			}
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x00074978 File Offset: 0x00072B78
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
						this.Click = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tfield.Type);
					}
				}
				else if (tfield.Type == 11)
				{
					this.Banner = iprot.ReadString();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tfield.Type);
				}
				iprot.ReadFieldEnd();
			}
			iprot.ReadStructEnd();
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x00074A44 File Offset: 0x00072C44
		public void Write(TProtocol oprot)
		{
			TStruct tstruct;
			tstruct..ctor("ServerInfoEvent");
			oprot.WriteStructBegin(tstruct);
			TField tfield = default(TField);
			if (this.Banner != null && this.__isset.banner)
			{
				tfield.Name = "banner";
				tfield.Type = 11;
				tfield.ID = 1;
				oprot.WriteFieldBegin(tfield);
				oprot.WriteString(this.Banner);
				oprot.WriteFieldEnd();
			}
			if (this.Click != null && this.__isset.click)
			{
				tfield.Name = "click";
				tfield.Type = 11;
				tfield.ID = 2;
				oprot.WriteFieldBegin(tfield);
				oprot.WriteString(this.Click);
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x00074B1C File Offset: 0x00072D1C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("ServerInfoEvent(");
			bool flag = true;
			if (this.Banner != null && this.__isset.banner)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				flag = false;
				stringBuilder.Append("Banner: ");
				stringBuilder.Append(this.Banner);
			}
			if (this.Click != null && this.__isset.click)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("Click: ");
				stringBuilder.Append(this.Click);
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x040018A7 RID: 6311
		private string _banner;

		// Token: 0x040018A8 RID: 6312
		private string _click;

		// Token: 0x040018A9 RID: 6313
		public ServerInfoEvent.Isset __isset;

		// Token: 0x020003D4 RID: 980
		[Serializable]
		public struct Isset
		{
			// Token: 0x040018AA RID: 6314
			public bool banner;

			// Token: 0x040018AB RID: 6315
			public bool click;
		}
	}
}
