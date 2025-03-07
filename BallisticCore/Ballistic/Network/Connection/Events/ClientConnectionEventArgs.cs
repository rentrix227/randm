using System;
using Aquiris.Ballistic.Network.Address;

namespace Aquiris.Ballistic.Network.Connection.Events
{
	// Token: 0x0200034D RID: 845
	internal class ClientConnectionEventArgs<NA, A> : EventArgs where NA : NetworkAddress<A>
	{
		// Token: 0x06001266 RID: 4710 RVA: 0x0000EDF3 File Offset: 0x0000CFF3
		internal ClientConnectionEventArgs(NA p_networkAddress)
		{
			this.NetworkAddress = p_networkAddress;
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06001267 RID: 4711 RVA: 0x0000EE02 File Offset: 0x0000D002
		// (set) Token: 0x06001268 RID: 4712 RVA: 0x0000EE0A File Offset: 0x0000D00A
		internal NA NetworkAddress { get; private set; }
	}
}
