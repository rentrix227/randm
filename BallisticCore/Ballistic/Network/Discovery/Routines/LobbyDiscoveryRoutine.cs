using System;
using System.Collections.Generic;
using System.Linq;
using Steamworks;

namespace Aquiris.Ballistic.Network.Discovery.Routines
{
	// Token: 0x02000362 RID: 866
	internal class LobbyDiscoveryRoutine : DiscoveryRoutine
	{
		// Token: 0x06001310 RID: 4880 RVA: 0x0000F4EE File Offset: 0x0000D6EE
		internal override void Run()
		{
			base.Run();
			this.handle = SteamMatchmaking.RequestLobbyList();
			SteamCallbacks.LobbyMatchList_t.RegisterCallResult(new Action<LobbyMatchList_t, bool>(this.OnLobbyMatchList), this.handle);
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x0000F518 File Offset: 0x0000D718
		internal override void Cancel()
		{
			base.Cancel();
			SteamCallbacks.LobbyMatchList_t.UnregisterCallResult(this.handle);
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x0000F52B File Offset: 0x0000D72B
		private void OnLobbyMatchList(LobbyMatchList_t pCallback, bool bIOFailure)
		{
			if (!bIOFailure)
			{
				if (pCallback.m_nLobbiesMatching > 0U)
				{
					this.DispatchLobbyList(pCallback.m_nLobbiesMatching);
				}
				this.Complete();
				this.DispatchCompleteAction();
			}
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x0000F559 File Offset: 0x0000D759
		private void DispatchLobbyList()
		{
			this.DispatchLobbyList(0U);
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x00068590 File Offset: 0x00066790
		private void DispatchLobbyList(uint numberOfLobbies)
		{
			int num = 0;
			while ((long)num < (long)((ulong)numberOfLobbies))
			{
				CSteamID lobbyByIndex = SteamMatchmaking.GetLobbyByIndex(num);
				if (DiscoveryUtils.CheckSteamLobbyVersion(lobbyByIndex))
				{
					HostItem hostItem = DiscoveryUtils.SteamLobby2HostItem(lobbyByIndex);
					if (hostItem != null)
					{
						if (DiscoveryUtils.CheckFilters(hostItem, this.m_filters.ToArray<KeyValuePair<string, string>>()))
						{
							this.DispatchItemFoundAction(hostItem);
						}
					}
				}
				num++;
			}
		}

		// Token: 0x0400172C RID: 5932
		private SteamAPICall_t handle;
	}
}
