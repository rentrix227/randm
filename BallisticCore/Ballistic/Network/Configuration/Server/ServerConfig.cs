using System;
using Aquiris.Ballistic.Network.Transport.Gameplay.State;

namespace Aquiris.Ballistic.Network.Configuration.Server
{
	// Token: 0x02000348 RID: 840
	internal class ServerConfig : Config
	{
		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060011F4 RID: 4596 RVA: 0x0000EAE2 File Offset: 0x0000CCE2
		// (set) Token: 0x060011F5 RID: 4597 RVA: 0x0000EAEA File Offset: 0x0000CCEA
		internal ulong GameMap { get; set; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060011F6 RID: 4598 RVA: 0x0000EAF3 File Offset: 0x0000CCF3
		// (set) Token: 0x060011F7 RID: 4599 RVA: 0x0000EAFB File Offset: 0x0000CCFB
		internal EGameMode GameMode { get; set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060011F8 RID: 4600 RVA: 0x0000EB04 File Offset: 0x0000CD04
		// (set) Token: 0x060011F9 RID: 4601 RVA: 0x0000EB0C File Offset: 0x0000CD0C
		internal bool DedicatedServer { get; set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060011FA RID: 4602 RVA: 0x0000EB15 File Offset: 0x0000CD15
		// (set) Token: 0x060011FB RID: 4603 RVA: 0x0000EB1D File Offset: 0x0000CD1D
		internal bool OfficialServer { get; set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060011FC RID: 4604 RVA: 0x0000EB26 File Offset: 0x0000CD26
		// (set) Token: 0x060011FD RID: 4605 RVA: 0x0000EB2E File Offset: 0x0000CD2E
		internal string OfficialApi { get; set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060011FE RID: 4606 RVA: 0x0000EB37 File Offset: 0x0000CD37
		// (set) Token: 0x060011FF RID: 4607 RVA: 0x0000EB3F File Offset: 0x0000CD3F
		internal bool Autobalance { get; set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06001200 RID: 4608 RVA: 0x0000EB48 File Offset: 0x0000CD48
		// (set) Token: 0x06001201 RID: 4609 RVA: 0x0000EB50 File Offset: 0x0000CD50
		internal EConnectionQuality BroadcastQuality { get; set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06001202 RID: 4610 RVA: 0x0000EB59 File Offset: 0x0000CD59
		// (set) Token: 0x06001203 RID: 4611 RVA: 0x0000EB61 File Offset: 0x0000CD61
		internal bool DedicatedOnDemand { get; set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06001204 RID: 4612 RVA: 0x0000EB6A File Offset: 0x0000CD6A
		// (set) Token: 0x06001205 RID: 4613 RVA: 0x0000EB72 File Offset: 0x0000CD72
		internal string BannerUrl { get; set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06001206 RID: 4614 RVA: 0x0000EB7B File Offset: 0x0000CD7B
		// (set) Token: 0x06001207 RID: 4615 RVA: 0x0000EB83 File Offset: 0x0000CD83
		internal string ClickUrl { get; set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06001208 RID: 4616 RVA: 0x0000EB8C File Offset: 0x0000CD8C
		// (set) Token: 0x06001209 RID: 4617 RVA: 0x0000EB94 File Offset: 0x0000CD94
		internal ulong Owner { get; set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600120A RID: 4618 RVA: 0x0000EB9D File Offset: 0x0000CD9D
		// (set) Token: 0x0600120B RID: 4619 RVA: 0x0000EBA5 File Offset: 0x0000CDA5
		internal bool Beta { get; set; }
	}
}
