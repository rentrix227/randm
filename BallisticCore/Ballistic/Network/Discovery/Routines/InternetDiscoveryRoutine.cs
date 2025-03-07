using System;
using System.Collections.Generic;
using System.Linq;
using Steamworks;

namespace Aquiris.Ballistic.Network.Discovery.Routines
{
	// Token: 0x02000360 RID: 864
	internal class InternetDiscoveryRoutine : DiscoveryRoutine
	{
		// Token: 0x060012FD RID: 4861 RVA: 0x00068238 File Offset: 0x00066438
		internal InternetDiscoveryRoutine()
		{
			this.m_internetListListener = new IInternetSteamMatchmakingServerListResponse(new IInternetSteamMatchmakingServerListResponse.ServerResponded(this.ServerResponded), new IInternetSteamMatchmakingServerListResponse.ServerFailedToRespond(this.ServerFailedToRespond), new IInternetSteamMatchmakingServerListResponse.RefreshComplete(this.RefreshComplete));
			this.m_queries = new List<InternetServerQuery>();
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x00068288 File Offset: 0x00066488
		internal override void Run()
		{
			base.Run();
			this.m_queries.Clear();
			MatchMakingKeyValuePair_t[] array = new MatchMakingKeyValuePair_t[1];
			int num = 0;
			MatchMakingKeyValuePair_t matchMakingKeyValuePair_t = default(MatchMakingKeyValuePair_t);
			matchMakingKeyValuePair_t.m_szKey = "appid";
			matchMakingKeyValuePair_t.m_szValue = SteamUtils.GetAppID().m_AppId.ToString();
			array[num] = matchMakingKeyValuePair_t;
			MatchMakingKeyValuePair_t[] array2 = array;
			this.m_internetListRequest = SteamMatchmakingServers.RequestInternetServerList(SteamUtils.GetAppID(), array2, (uint)array2.Length, this.m_internetListListener);
			SteamMatchmakingServers.RefreshQuery(this.m_internetListRequest);
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x0000F397 File Offset: 0x0000D597
		internal override void Cancel()
		{
			base.Cancel();
			this.m_queries.Clear();
			SteamMatchmakingServers.ReleaseRequest(this.m_internetListRequest);
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x00068314 File Offset: 0x00066514
		private void ServerResponded(HServerListRequest hRequest, int iServer)
		{
			gameserveritem_t serverDetails = SteamMatchmakingServers.GetServerDetails(hRequest, iServer);
			bool flag = !serverDetails.m_bPassword && (serverDetails.m_nServerVersion == BallisticVersion.DEDICATED_SERVER_VERSION_INTEGER || serverDetails.m_nServerVersion == BallisticVersion.OFFICIAL_SERVER_VERSION_INTEGER);
			if (flag)
			{
				if (serverDetails.m_nBotPlayers > 0)
				{
					this.m_queries.Add(new InternetServerQuery(serverDetails, new Action<InternetServerQuery>(this.OnQueryCompleted), new Action<InternetServerQuery>(this.OnQueryFailed)));
				}
				else
				{
					this.TryToCreateServer(serverDetails, new ulong[0]);
				}
			}
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x00002A31 File Offset: 0x00000C31
		private void ServerFailedToRespond(HServerListRequest hRequest, int iServer)
		{
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x0000F3B5 File Offset: 0x0000D5B5
		private void RefreshComplete(HServerListRequest hRequest, EMatchMakingServerResponse response)
		{
			SteamMatchmakingServers.ReleaseRequest(hRequest);
			this.Complete();
			if (this.m_queries.Count > 0)
			{
				this.m_queries.Last<InternetServerQuery>().Run();
			}
			else
			{
				this.DispatchCompleteAction();
			}
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x0000F3EF File Offset: 0x0000D5EF
		private void OnQueryFailed(InternetServerQuery query)
		{
			this.m_queries.Remove(query);
			if (this.m_queries.Count > 0)
			{
				this.m_queries.Last<InternetServerQuery>().Run();
			}
			else
			{
				this.DispatchCompleteAction();
			}
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x000683A8 File Offset: 0x000665A8
		private void OnQueryCompleted(InternetServerQuery query)
		{
			this.m_queries.Remove(query);
			this.TryToCreateServer(query._gameserver, query._workshopItens.ToArray());
			if (this.m_queries.Count > 0)
			{
				this.m_queries.Last<InternetServerQuery>().Run();
			}
			else
			{
				this.DispatchCompleteAction();
			}
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x00068408 File Offset: 0x00066608
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

		// Token: 0x04001726 RID: 5926
		private IInternetSteamMatchmakingServerListResponse m_internetListListener;

		// Token: 0x04001727 RID: 5927
		private HServerListRequest m_internetListRequest;

		// Token: 0x04001728 RID: 5928
		private List<InternetServerQuery> m_queries;
	}
}
