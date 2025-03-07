using System;
using Aquiris.Ballistic.Network.Transport.Gameplay.State;

namespace Aquiris.Ballistic.Network.Address
{
	// Token: 0x0200033E RID: 830
	public abstract class NetworkAddress<A>
	{
		// Token: 0x06001195 RID: 4501 RVA: 0x0000E6CA File Offset: 0x0000C8CA
		public NetworkAddress(A p_info)
		{
			this.Info = p_info;
			this.IsReady = false;
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06001196 RID: 4502 RVA: 0x0000E6E0 File Offset: 0x0000C8E0
		// (set) Token: 0x06001197 RID: 4503 RVA: 0x0000E6E8 File Offset: 0x0000C8E8
		internal EClientMode clientMode { get; set; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06001198 RID: 4504 RVA: 0x0000E6F1 File Offset: 0x0000C8F1
		// (set) Token: 0x06001199 RID: 4505 RVA: 0x0000E6F9 File Offset: 0x0000C8F9
		internal bool IsReady { get; set; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600119A RID: 4506 RVA: 0x0000E702 File Offset: 0x0000C902
		// (set) Token: 0x0600119B RID: 4507 RVA: 0x0000E70A File Offset: 0x0000C90A
		internal float LastInteractionTime { get; set; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600119C RID: 4508 RVA: 0x0000E713 File Offset: 0x0000C913
		// (set) Token: 0x0600119D RID: 4509 RVA: 0x0000E71B File Offset: 0x0000C91B
		internal A Info { get; private set; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600119E RID: 4510 RVA: 0x0000E724 File Offset: 0x0000C924
		// (set) Token: 0x0600119F RID: 4511 RVA: 0x0000E72C File Offset: 0x0000C92C
		internal bool CheckingKeepAlive { get; set; }

		// Token: 0x060011A0 RID: 4512 RVA: 0x0000E735 File Offset: 0x0000C935
		public override string ToString()
		{
			return string.Format("network info {0}", this.Info);
		}
	}
}
