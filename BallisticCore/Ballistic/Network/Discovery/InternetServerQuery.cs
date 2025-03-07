using System;
using System.Collections.Generic;
using Aquiris.Ballistic.Game.Utility;
using Steamworks;

namespace Aquiris.Ballistic.Network.Discovery
{
	// Token: 0x02000364 RID: 868
	public class InternetServerQuery
	{
		// Token: 0x0600131C RID: 4892 RVA: 0x000686DC File Offset: 0x000668DC
		public InternetServerQuery(gameserveritem_t gameserver, Action<InternetServerQuery> OnComplete, Action<InternetServerQuery> OnFailed)
		{
			this._gameserver = gameserver;
			this._OnComplete = OnComplete;
			this._OnFailed = OnFailed;
			this._workshopItens = new List<ulong>();
			this._discoveryCount = -1;
			this._response = new IInternetSteamMatchmakingRulesResponse(new IInternetSteamMatchmakingRulesResponse.RulesResponded(this.OnRulesResponded), new IInternetSteamMatchmakingRulesResponse.RulesFailedToRespond(this.OnRulesFailedToRespond), new IInternetSteamMatchmakingRulesResponse.RulesRefreshComplete(this.OnRulesRefreshComplete));
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x0600131D RID: 4893 RVA: 0x0000F5AF File Offset: 0x0000D7AF
		// (set) Token: 0x0600131E RID: 4894 RVA: 0x0000F5B7 File Offset: 0x0000D7B7
		public HServerQuery Hserver { get; set; }

		// Token: 0x0600131F RID: 4895 RVA: 0x0000F5C0 File Offset: 0x0000D7C0
		public void Run()
		{
			this.Hserver = SteamMatchmakingServers.ServerRules(this._gameserver.m_NetAdr.GetIP(), this._gameserver.m_NetAdr.GetQueryPort(), this._response);
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x00002A31 File Offset: 0x00000C31
		private void OnRulesRefreshComplete()
		{
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x0000F5F3 File Offset: 0x0000D7F3
		private void OnRulesFailedToRespond()
		{
			this._OnFailed(this);
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x00068748 File Offset: 0x00066948
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

		// Token: 0x04001734 RID: 5940
		public gameserveritem_t _gameserver;

		// Token: 0x04001735 RID: 5941
		public List<ulong> _workshopItens;

		// Token: 0x04001736 RID: 5942
		private readonly IInternetSteamMatchmakingRulesResponse _response;

		// Token: 0x04001737 RID: 5943
		private Action<InternetServerQuery> _OnComplete;

		// Token: 0x04001738 RID: 5944
		private Action<InternetServerQuery> _OnFailed;

		// Token: 0x04001739 RID: 5945
		private int _discoveryCount;
	}
}
