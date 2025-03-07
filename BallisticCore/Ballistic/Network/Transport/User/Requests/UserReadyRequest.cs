using System;
using System.Text;
using Thrift.Protocol;

namespace Aquiris.Ballistic.Network.Transport.User.Requests
{
	// Token: 0x02000473 RID: 1139
	[Serializable]
	public class UserReadyRequest : TBase, TAbstractBase
	{
		// Token: 0x170002AB RID: 683
		// (get) Token: 0x060018AC RID: 6316 RVA: 0x00012528 File Offset: 0x00010728
		// (set) Token: 0x060018AD RID: 6317 RVA: 0x00012530 File Offset: 0x00010730
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

		// Token: 0x060018AE RID: 6318 RVA: 0x000858B0 File Offset: 0x00083AB0
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
				else if (tfield.Type == 2)
				{
					this.IsReady = iprot.ReadBool();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tfield.Type);
				}
				iprot.ReadFieldEnd();
			}
			iprot.ReadStructEnd();
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x00085944 File Offset: 0x00083B44
		public void Write(TProtocol oprot)
		{
			TStruct tstruct;
			tstruct..ctor("UserReadyRequest");
			oprot.WriteStructBegin(tstruct);
			TField tfield = default(TField);
			if (this.__isset.isReady)
			{
				tfield.Name = "isReady";
				tfield.Type = 2;
				tfield.ID = 1;
				oprot.WriteFieldBegin(tfield);
				oprot.WriteBool(this.IsReady);
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x000859C0 File Offset: 0x00083BC0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("UserReadyRequest(");
			bool flag = true;
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

		// Token: 0x04001B3E RID: 6974
		private bool _isReady;

		// Token: 0x04001B3F RID: 6975
		public UserReadyRequest.Isset __isset;

		// Token: 0x02000474 RID: 1140
		[Serializable]
		public struct Isset
		{
			// Token: 0x04001B40 RID: 6976
			public bool isReady;
		}
	}
}
