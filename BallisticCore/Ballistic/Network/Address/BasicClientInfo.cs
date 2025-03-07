using System;

namespace Aquiris.Ballistic.Network.Address
{
	// Token: 0x0200033C RID: 828
	internal abstract class BasicClientInfo<A> : NetworkAddress<A>
	{
		// Token: 0x06001190 RID: 4496 RVA: 0x0000E6B0 File Offset: 0x0000C8B0
		internal BasicClientInfo(A p_clientInfo)
			: base(p_clientInfo)
		{
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06001191 RID: 4497 RVA: 0x0000E6B9 File Offset: 0x0000C8B9
		// (set) Token: 0x06001192 RID: 4498 RVA: 0x0000E6C1 File Offset: 0x0000C8C1
		internal string Alias { get; set; }

		// Token: 0x06001193 RID: 4499
		internal abstract bool isMe();
	}
}
