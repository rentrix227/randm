using System;
using System.Collections.Generic;
using System.Linq;
using Steamworks;

namespace Aquiris.Ballistic.Network.Discovery.Routines
{
	// Token: 0x02000361 RID: 865
	internal class LanDiscoveryRoutine : DiscoveryRoutine
	{
		// Token: 0x06001306 RID: 4870 RVA: 0x0006844C File Offset: 0x0006664C
		internal LanDiscoveryRoutine()
		{
			this.m_lanListListener = new ILanSteamMatchmakingServerListResponse(new ILanSteamMatchmakingServerListResponse.ServerResponded(this.ServerResponded), new ILanSteamMatchmakingServerListResponse.ServerFailedToRespond(this.ServerFailedToRespond), new ILanSteamMatchmakingServerListResponse.RefreshComplete(this.RefreshComplete));
			this.m_queries = new List<LanServerQuery>();
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x0000F42A File Offset: 0x0000D62A
		internal override void Run()
		{
			base.Run();
			this.m_queries.Clear();
			this.m_lanListRequest = SteamMatchmakingServers.RequestLANServerList(SteamUtils.GetAppID(), this.m_lanListListener);
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x0000F453 File Offset: 0x0000D653
		internal override void Cancel()
		{
			base.Cancel();
			this.m_queries.Clear();
			SteamMatchmakingServers.ReleaseRequest(this.m_lanListRequest);
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x0006849C File Offset: 0x0006669C
		private void ServerResponded(HServerListRequest hRequest, int iServer)
		{
			gameserveritem_t serverDetails = SteamMatchmakingServers.GetServerDetails(hRequest, iServer);
			bool flag = !serverDetails.m_bPassword && (serverDetails.m_nServerVersion == BallisticVersion.DEDICATED_SERVER_VERSION_INTEGER || serverDetails.m_nServerVersion == BallisticVersion.OFFICIAL_SERVER_VERSION_INTEGER);
			if (flag)
			{
				if (serverDetails.m_nBotPlayers > 0)
				{
					this.m_queries.Add(new LanServerQuery(serverDetails, new Action<LanServerQuery>(this.OnQueryCompleted), new Action<LanServerQuery>(this.OnQueryFailed)));
				}
				else
				{
					this.TryToCreateServer(serverDetails, new ulong[0]);
				}
			}
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x00002A31 File Offset: 0x00000C31
		private void ServerFailedToRespond(HServerListRequest hRequest, int iServer)
		{
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x0000F471 File Offset: 0x0000D671
		private void RefreshComplete(HServerListRequest hRequest, EMatchMakingServerResponse response)
		{
			SteamMatchmakingServers.ReleaseRequest(hRequest);
			this.Complete();
			if (this.m_queries.Count > 0)
			{
				this.m_queries.Last<LanServerQuery>().Run();
			}
			else
			{
				this.DispatchCompleteAction();
			}
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x0000F4AB File Offset: 0x0000D6AB
		private void OnQueryFailed(LanServerQuery query)
		{
			this.m_queries.Remove(query);
			if (this.m_queries.Count > 0)
			{
				this.m_queries.Last<LanServerQuery>().Run();
			}
			else
			{
				this.DispatchCompleteAction();
			}
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x00068530 File Offset: 0x00066730
		private void OnQueryCompleted(LanServerQuery query)
		{
			this.m_queries.Remove(query);
			this.TryToCreateServer(query._gameserver, query._workshopItens.ToArray());
			if (this.m_queries.Count > 0)
			{
				this.m_queries.Last<LanServerQuery>().Run();
			}
			else
			{
				this.DispatchCompleteAction();
			}
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x00068408 File Offset: 0x00066608
		private void TryToCreateServer(gameserveritem_t gameserver, ulong[] workshopItems)
		{
			HostItem hostItem = DiscoveryUtils.GameServer2HostItem(gameserver, workshopItems);
			if (hostItem == null)
			{
				return;
			}
			if (!hostItem.IsTerminating && DiscoveryUtils.CheckFilters(hostItem, this.m_filters.ToArray<KeyValuePair<string, string>>()))
			{
				this.DispatchItemFoundAction(hostItem);
			}
		}

		// Token: 0x04001729 RID: 5929
		private ILanSteamMatchmakingServerListResponse m_lanListListener;

		// Token: 0x0400172A RID: 5930
		private HServerListRequest m_lanListRequest;

		// Token: 0x0400172B RID: 5931
		private List<LanServerQuery> m_queries;
	}
}
