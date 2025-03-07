using System;
using Steamworks;

namespace Aquiris.Ballistic.Network.Address.Steam
{
	// Token: 0x02000340 RID: 832
	internal class SteamClientInfo : BasicClientInfo<CSteamID>
	{
		// Token: 0x060011A7 RID: 4519 RVA: 0x0000E7BD File Offset: 0x0000C9BD
		internal SteamClientInfo(CSteamID p_clientInfo)
			: base(p_clientInfo)
		{
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x0000E7C6 File Offset: 0x0000C9C6
		internal override bool isMe()
		{
			return base.Info == SteamUser.GetSteamID();
		}

		// Token: 0x04001682 RID: 5762
		internal bool IsAppOwner;
	}
}
