using System;

namespace Aquiris.Ballistic.Utils
{
	// Token: 0x020004D2 RID: 1234
	[Serializable]
	public struct LeaderboardFindOrCreateParser
	{
		// Token: 0x04001C87 RID: 7303
		public LeaderboardFindOrCreateParser.ResultParser result;

		// Token: 0x020004D3 RID: 1235
		[Serializable]
		public struct ResultParser
		{
			// Token: 0x04001C88 RID: 7304
			public int result;

			// Token: 0x04001C89 RID: 7305
			public LeaderboardFindOrCreateParser.ResultParser.LeaderboardParser leaderboard;

			// Token: 0x020004D4 RID: 1236
			[Serializable]
			public struct LeaderboardParser
			{
				// Token: 0x04001C8A RID: 7306
				public string leaderboardName;

				// Token: 0x04001C8B RID: 7307
				public ulong leaderBoardID;

				// Token: 0x04001C8C RID: 7308
				public int leaderBoardEntries;

				// Token: 0x04001C8D RID: 7309
				public string leaderBoardSortMethod;

				// Token: 0x04001C8E RID: 7310
				public string leaderBoardDisplayType;

				// Token: 0x04001C8F RID: 7311
				public bool onlytrustedwrites;

				// Token: 0x04001C90 RID: 7312
				public bool onlyfriendsreads;
			}
		}
	}
}
