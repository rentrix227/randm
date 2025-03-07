using System;
using System.Collections.Generic;
using Aquiris.Ballistic.Network.Address;

namespace Aquiris.Ballistic.Network.Connection
{
	// Token: 0x02000354 RID: 852
	internal class PingMachine<A, BC> where BC : BasicClientInfo<A>
	{
		// Token: 0x06001270 RID: 4720 RVA: 0x0000EE4A File Offset: 0x0000D04A
		internal PingMachine(int p_maxPing)
		{
			this.m_maxPing = p_maxPing;
			this.m_pingMap = new Dictionary<BC, PingMachine<A, BC>.PingData>();
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x00066840 File Offset: 0x00064A40
		internal void AddClient(BC p_client)
		{
			if (!this.m_pingMap.ContainsKey(p_client))
			{
				this.m_pingMap.Add(p_client, new PingMachine<A, BC>.PingData
				{
					lastPingAverage = 0f,
					list = new List<long>()
				});
			}
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x0000EE64 File Offset: 0x0000D064
		internal void RemoveClient(BC p_client)
		{
			if (this.m_pingMap.ContainsKey(p_client))
			{
				this.m_pingMap.Remove(p_client);
			}
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x0000EE84 File Offset: 0x0000D084
		internal float GetAveragePing(BC p_client)
		{
			if (this.m_pingMap.ContainsKey(p_client))
			{
				return this.m_pingMap[p_client].lastPingAverage;
			}
			return 0f;
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x00066888 File Offset: 0x00064A88
		internal void UpdatePing(BC p_client, long p_ping)
		{
			if (this.m_maxPing == 0)
			{
				return;
			}
			if (this.m_pingMap.ContainsKey(p_client))
			{
				PingMachine<A, BC>.PingData pingData = this.m_pingMap[p_client];
				pingData.list.Add(p_ping);
				if (this.m_maxPing > 0 && pingData.list.Count >= 30)
				{
					long num = 0L;
					for (int i = 0; i < 30; i++)
					{
						num += pingData.list[i];
					}
					num /= 30L;
					pingData.list.RemoveRange(0, 30);
					pingData.lastPingAverage = (float)num;
					if (num >= (long)this.m_maxPing && this.OnMaxPing != null)
					{
						this.OnMaxPing(p_client);
					}
				}
			}
		}

		// Token: 0x040016DD RID: 5853
		internal Action<BC> OnMaxPing;

		// Token: 0x040016DE RID: 5854
		private const int MAX_SAMPLES = 30;

		// Token: 0x040016DF RID: 5855
		private int m_maxPing;

		// Token: 0x040016E0 RID: 5856
		private Dictionary<BC, PingMachine<A, BC>.PingData> m_pingMap;

		// Token: 0x02000355 RID: 853
		internal class PingData
		{
			// Token: 0x040016E1 RID: 5857
			internal float lastPingAverage;

			// Token: 0x040016E2 RID: 5858
			internal List<long> list;
		}
	}
}
