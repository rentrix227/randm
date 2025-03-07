using System;
using System.Collections.Generic;
using Steamworks;

namespace Aquiris.Ballistic.Network.Address.Steam
{
	// Token: 0x0200033F RID: 831
	internal class SteamClientFactory
	{
		// Token: 0x060011A2 RID: 4514 RVA: 0x0000E74C File Offset: 0x0000C94C
		internal static SteamClientInfo GetClient(long p_steamID)
		{
			return SteamClientFactory.GetClient(new CSteamID((ulong)p_steamID));
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x0000E759 File Offset: 0x0000C959
		internal static SteamClientInfo GetClient(CSteamID p_steamID)
		{
			if (!SteamClientFactory.m_objectPoll.ContainsKey(p_steamID))
			{
				SteamClientFactory.m_objectPoll[p_steamID] = new SteamClientInfo(p_steamID);
			}
			return SteamClientFactory.m_objectPoll[p_steamID];
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x0000E787 File Offset: 0x0000C987
		internal static void FreeClient(CSteamID p_steamID)
		{
			if (SteamClientFactory.m_objectPoll.ContainsKey(p_steamID))
			{
				SteamClientFactory.m_objectPoll.Remove(p_steamID);
			}
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x0000E7A5 File Offset: 0x0000C9A5
		internal static void FreeAll()
		{
			SteamClientFactory.m_objectPoll.Clear();
		}

		// Token: 0x04001681 RID: 5761
		private static Dictionary<CSteamID, SteamClientInfo> m_objectPoll = new Dictionary<CSteamID, SteamClientInfo>();
	}
}
