using System;

namespace Aquiris.Ballistic.Network.Transmission.Packet.Thrift
{
	// Token: 0x020003C9 RID: 969
	public enum ETransportChannel : byte
	{
		// Token: 0x04001886 RID: 6278
		KEEP_ALIVE,
		// Token: 0x04001887 RID: 6279
		CONNECTION_MANAGEMENT,
		// Token: 0x04001888 RID: 6280
		GAME_CONTROL,
		// Token: 0x04001889 RID: 6281
		GAME_EVENTS,
		// Token: 0x0400188A RID: 6282
		PLAYER_DATA,
		// Token: 0x0400188B RID: 6283
		SPAWN_GAME_COMPONENT,
		// Token: 0x0400188C RID: 6284
		TEAM_DATA,
		// Token: 0x0400188D RID: 6285
		PLAYER_STATE,
		// Token: 0x0400188E RID: 6286
		CLOCK_DATA,
		// Token: 0x0400188F RID: 6287
		VOTE_MAP
	}
}
