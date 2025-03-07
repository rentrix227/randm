using System;
using Aquiris.Ballistic.Network.Address;

namespace Aquiris.Ballistic.Network.Connection.Events
{
	// Token: 0x0200034E RID: 846
	internal class ClientDisconnectionEventArgs<NA, A> : EventArgs where NA : NetworkAddress<A>
	{
		// Token: 0x06001269 RID: 4713 RVA: 0x0000EE13 File Offset: 0x0000D013
		internal ClientDisconnectionEventArgs(NA p_networkAddress)
		{
			this.NetworkAddress = p_networkAddress;
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600126A RID: 4714 RVA: 0x0000EE22 File Offset: 0x0000D022
		// (set) Token: 0x0600126B RID: 4715 RVA: 0x0000EE2A File Offset: 0x0000D02A
		internal NA NetworkAddress { get; private set; }
	}
}
