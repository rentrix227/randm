using System;
using System.Collections.Generic;
using Steamworks;

namespace Aquiris.Ballistic.Network.Address.Steam
{
	// Token: 0x02000341 RID: 833
	internal class SteamHostFactory
	{
		// Token: 0x060011AA RID: 4522 RVA: 0x0000E7D8 File Offset: 0x0000C9D8
		internal static SteamHostInfo getHost(CSteamID p_steamID)
		{
			if (!SteamHostFactory.m_objectPoll.ContainsKey(p_steamID))
			{
				SteamHostFactory.m_objectPoll[p_steamID] = new SteamHostInfo(p_steamID);
			}
			return SteamHostFactory.m_objectPoll[p_steamID];
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x0000E806 File Offset: 0x0000CA06
		internal static void freeHost(CSteamID p_steamID)
		{
			if (SteamHostFactory.m_objectPoll.ContainsKey(p_steamID))
			{
				SteamHostFactory.m_objectPoll.Remove(p_steamID);
			}
		}

		// Token: 0x04001683 RID: 5763
		private static Dictionary<CSteamID, SteamHostInfo> m_objectPoll = new Dictionary<CSteamID, SteamHostInfo>();
	}
}
