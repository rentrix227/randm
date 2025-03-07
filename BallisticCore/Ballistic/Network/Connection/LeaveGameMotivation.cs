using System;

namespace Aquiris.Ballistic.Network.Connection
{
	// Token: 0x02000353 RID: 851
	public enum LeaveGameMotivation
	{
		// Token: 0x040016D1 RID: 5841
		NONE,
		// Token: 0x040016D2 RID: 5842
		DISCONNECTED,
		// Token: 0x040016D3 RID: 5843
		BADCONNECTION,
		// Token: 0x040016D4 RID: 5844
		GRENADEHACK,
		// Token: 0x040016D5 RID: 5845
		SPEEDHACK,
		// Token: 0x040016D6 RID: 5846
		INACTIVITY,
		// Token: 0x040016D7 RID: 5847
		MAXPING,
		// Token: 0x040016D8 RID: 5848
		SERVERFULL,
		// Token: 0x040016D9 RID: 5849
		VACBANNED,
		// Token: 0x040016DA RID: 5850
		WRONGPASSWORD,
		// Token: 0x040016DB RID: 5851
		HOST_KICKED,
		// Token: 0x040016DC RID: 5852
		HOST_BLACKLISTED
	}
}
