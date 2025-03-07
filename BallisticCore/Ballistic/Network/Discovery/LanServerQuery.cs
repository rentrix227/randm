using System;
using System.Collections.Generic;
using Aquiris.Ballistic.Game.Utility;
using Steamworks;

namespace Aquiris.Ballistic.Network.Discovery
{
	// Token: 0x02000363 RID: 867
	public class LanServerQuery
	{
		// Token: 0x06001315 RID: 4885 RVA: 0x000685F8 File Offset: 0x000667F8
		public LanServerQuery(gameserveritem_t gameserver, Action<LanServerQuery> OnComplete, Action<LanServerQuery> OnFailed)
		{
			this._gameserver = gameserver;
			this._OnComplete = OnComplete;
			this._OnFailed = OnFailed;
			this._workshopItens = new List<ulong>();
			this._discoveryCount = -1;
			this._response = new ILanSteamMatchmakingRulesResponse(new ILanSteamMatchmakingRulesResponse.RulesResponded(this.OnRulesResponded), new ILanSteamMatchmakingRulesResponse.RulesFailedToRespond(this.OnRulesFailedToRespond), new ILanSteamMatchmakingRulesResponse.RulesRefreshComplete(this.OnRulesRefreshComplete));
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06001316 RID: 4886 RVA: 0x0000F562 File Offset: 0x0000D762
		// (set) Token: 0x06001317 RID: 4887 RVA: 0x0000F56A File Offset: 0x0000D76A
		public HServerQuery Hserver { get; set; }

		// Token: 0x06001318 RID: 4888 RVA: 0x0000F573 File Offset: 0x0000D773
		public void Run()
		{
			SteamMatchmakingServers.ServerRules(this._gameserver.m_NetAdr.GetIP(), this._gameserver.m_NetAdr.GetQueryPort(), this._response);
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x00002A31 File Offset: 0x00000C31
		private void OnRulesRefreshComplete()
		{
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x0000F5A1 File Offset: 0x0000D7A1
		private void OnRulesFailedToRespond()
		{
			this._OnFailed(this);
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x00068664 File Offset: 0x00066864
		private void OnRulesResponded(string rule, string value)
		{
			if (rule == "md")
			{
				this._discoveryCount = int.Parse(value);
			}
			if (rule.StartsWith("mw"))
			{
				this._workshopItens.Add(BytePacker.GetHexUint64(value));
			}
			if (this._discoveryCount >= 0 && this._workshopItens.Count == this._discoveryCount)
			{
				this._OnComplete(this);
			}
		}

		// Token: 0x0400172D RID: 5933
		public gameserveritem_t _gameserver;

		// Token: 0x0400172E RID: 5934
		public List<ulong> _workshopItens;

		// Token: 0x0400172F RID: 5935
		private readonly ILanSteamMatchmakingRulesResponse _response;

		// Token: 0x04001730 RID: 5936
		private Action<LanServerQuery> _OnComplete;

		// Token: 0x04001731 RID: 5937
		private Action<LanServerQuery> _OnFailed;

		// Token: 0x04001732 RID: 5938
		private int _discoveryCount;
	}
}
