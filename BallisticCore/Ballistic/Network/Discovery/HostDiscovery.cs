using System;
using System.Collections.Generic;
using System.Diagnostics;
using Aquiris.Ballistic.Network.Discovery.Routines;
using Steamworks;
using UnityEngine;

namespace Aquiris.Ballistic.Network.Discovery
{
	// Token: 0x0200035C RID: 860
	internal class HostDiscovery
	{
		// Token: 0x060012A8 RID: 4776 RVA: 0x0000F0B7 File Offset: 0x0000D2B7
		internal HostDiscovery()
		{
			this._routines = new Dictionary<EHostType, DiscoveryRoutine>();
			this._hostList = new List<HostItem>();
			this.Running = false;
			this.CreateRoutines();
		}

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x060012A9 RID: 4777 RVA: 0x00067714 File Offset: 0x00065914
		// (remove) Token: 0x060012AA RID: 4778 RVA: 0x0006774C File Offset: 0x0006594C
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action OnDiscoveryStarted;

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x060012AB RID: 4779 RVA: 0x00067784 File Offset: 0x00065984
		// (remove) Token: 0x060012AC RID: 4780 RVA: 0x000677BC File Offset: 0x000659BC
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<HostItem> OnDiscoveryUpdated;

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x060012AD RID: 4781 RVA: 0x000677F4 File Offset: 0x000659F4
		// (remove) Token: 0x060012AE RID: 4782 RVA: 0x0006782C File Offset: 0x00065A2C
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action OnDiscoveryEnded;

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x060012AF RID: 4783 RVA: 0x00067864 File Offset: 0x00065A64
		// (remove) Token: 0x060012B0 RID: 4784 RVA: 0x0006789C File Offset: 0x00065A9C
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action OnDiscoveryCanceled;

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060012B1 RID: 4785 RVA: 0x0000F0E2 File Offset: 0x0000D2E2
		// (set) Token: 0x060012B2 RID: 4786 RVA: 0x0000F0EA File Offset: 0x0000D2EA
		internal bool Running { get; private set; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060012B3 RID: 4787 RVA: 0x0000F0F3 File Offset: 0x0000D2F3
		internal List<HostItem> HostList
		{
			get
			{
				return new List<HostItem>(this._hostList);
			}
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x000678D4 File Offset: 0x00065AD4
		~HostDiscovery()
		{
			this.DestroyRoutines();
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x0000F100 File Offset: 0x0000D300
		internal virtual void Go()
		{
			this.Go(false);
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x00067904 File Offset: 0x00065B04
		internal virtual void Go(bool filterByFriends)
		{
			if (this.Running)
			{
				this.Cancel();
			}
			this.Running = true;
			this._hostList.Clear();
			this._routineCount = 0U;
			if (this.OnDiscoveryStarted != null)
			{
				this.OnDiscoveryStarted();
			}
			foreach (DiscoveryRoutine discoveryRoutine in this._routines.Values)
			{
				this._routineCount += 1U;
				discoveryRoutine.Run();
			}
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x000679B4 File Offset: 0x00065BB4
		internal virtual void Cancel()
		{
			this.Running = false;
			foreach (DiscoveryRoutine discoveryRoutine in this._routines.Values)
			{
				if (discoveryRoutine.Running)
				{
					discoveryRoutine.Cancel();
				}
			}
			if (this.OnDiscoveryCanceled != null)
			{
				this.OnDiscoveryCanceled();
			}
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x00067A3C File Offset: 0x00065C3C
		internal virtual void AddFilter(string key, int value, ComparisonFilter filter)
		{
			foreach (KeyValuePair<EHostType, DiscoveryRoutine> keyValuePair in this._routines)
			{
				keyValuePair.Value.AddFilter(key, value, filter);
			}
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x00067AA0 File Offset: 0x00065CA0
		internal virtual void AddFilter(string key, string value, ComparisonFilter filter)
		{
			foreach (KeyValuePair<EHostType, DiscoveryRoutine> keyValuePair in this._routines)
			{
				keyValuePair.Value.AddFilter(key, value, filter);
			}
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x00067B04 File Offset: 0x00065D04
		protected virtual void CreateRoutines()
		{
			DiscoveryRoutine discoveryRoutine = new LanDiscoveryRoutine();
			discoveryRoutine.OnItemFound += this.OnItemFound;
			discoveryRoutine.OnCompleted += this.OnRoutineComplete;
			this._routines.Add(EHostType.LAN, discoveryRoutine);
			DiscoveryRoutine discoveryRoutine2 = new InternetDiscoveryRoutine();
			discoveryRoutine2.OnItemFound += this.OnItemFound;
			discoveryRoutine2.OnCompleted += this.OnRoutineComplete;
			this._routines.Add(EHostType.Internet, discoveryRoutine2);
			DiscoveryRoutine discoveryRoutine3 = new LobbyDiscoveryRoutine();
			discoveryRoutine3.OnItemFound += this.OnItemFound;
			discoveryRoutine3.OnCompleted += this.OnRoutineComplete;
			this._routines.Add(EHostType.Lobby, discoveryRoutine3);
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x00067BB8 File Offset: 0x00065DB8
		protected virtual void DestroyRoutines()
		{
			foreach (DiscoveryRoutine discoveryRoutine in this._routines.Values)
			{
				discoveryRoutine.OnItemFound -= this.OnItemFound;
				discoveryRoutine.OnCompleted -= this.OnRoutineComplete;
				if (discoveryRoutine.Running)
				{
					discoveryRoutine.Cancel();
				}
			}
			this.Running = false;
			this._hostList.Clear();
			this._routines.Clear();
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x0000F109 File Offset: 0x0000D309
		private void OnItemFound(DiscoveryRoutine routine, HostItem item)
		{
			this.TryAddHostItem(item);
			if (this.OnDiscoveryUpdated != null)
			{
				this.OnDiscoveryUpdated(item);
			}
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x00067C64 File Offset: 0x00065E64
		private void TryAddHostItem(HostItem item)
		{
			List<HostItem> list = new List<HostItem>();
			bool flag = true;
			for (int i = 0; i < this._hostList.Count; i++)
			{
				HostItem hostItem = this._hostList[i];
				if (item.SteamId.m_SteamID == hostItem.SteamId.m_SteamID)
				{
					flag = false;
					break;
				}
				if (item.Type == EHostType.Lobby && hostItem.Type != EHostType.Lobby)
				{
					uint num;
					ushort num2;
					CSteamID csteamID;
					bool lobbyGameServer = SteamMatchmaking.GetLobbyGameServer(item.SteamId, ref num, ref num2, ref csteamID);
					if (lobbyGameServer && csteamID.m_SteamID == hostItem.SteamId.m_SteamID)
					{
						list.Add(hostItem);
						if (item.Ping == 0U || (hostItem.Ping > 0U && hostItem.Ping < item.Ping))
						{
							item.Ping = hostItem.Ping;
						}
					}
				}
				else if (item.Type != EHostType.Lobby && hostItem.Type == EHostType.Lobby)
				{
					uint num;
					ushort num2;
					CSteamID csteamID;
					bool lobbyGameServer2 = SteamMatchmaking.GetLobbyGameServer(hostItem.SteamId, ref num, ref num2, ref csteamID);
					if (lobbyGameServer2 && csteamID.m_SteamID == item.SteamId.m_SteamID)
					{
						flag = false;
						if (hostItem.Ping == 0U || (item.Ping > 0U && item.Ping < hostItem.Ping))
						{
							hostItem.Ping = item.Ping;
						}
						break;
					}
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				this._hostList.Remove(list[j]);
			}
			if (flag)
			{
				this._hostList.Add(item);
			}
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x0000F129 File Offset: 0x0000D329
		private void OnRoutineComplete(DiscoveryRoutine routine)
		{
			this.CheckComplete();
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x00067E28 File Offset: 0x00066028
		private void CheckComplete()
		{
			uint num = 0U;
			foreach (DiscoveryRoutine discoveryRoutine in this._routines.Values)
			{
				if (discoveryRoutine.Canceled || discoveryRoutine.Completed)
				{
					num += 1U;
				}
			}
			if (num >= this._routineCount)
			{
				this.Running = false;
				foreach (DiscoveryRoutine discoveryRoutine2 in this._routines.Values)
				{
					discoveryRoutine2.ClearFilters();
				}
				if (this.OnDiscoveryEnded != null)
				{
					this.OnDiscoveryEnded();
				}
				Debug.Log("Host Discovery Completed! " + this._hostList.Count + " host(s) found");
			}
		}

		// Token: 0x04001703 RID: 5891
		private readonly List<HostItem> _hostList;

		// Token: 0x04001704 RID: 5892
		private readonly Dictionary<EHostType, DiscoveryRoutine> _routines;

		// Token: 0x04001705 RID: 5893
		private uint _routineCount;
	}
}
