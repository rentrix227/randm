using System;
using Aquiris.Ballistic.Network.Configuration.Server;

namespace Aquiris.Ballistic.Network.Configuration.Steam
{
	// Token: 0x02000349 RID: 841
	internal class SteamConfig : ServerConfig
	{
		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600120D RID: 4621 RVA: 0x0000EBB6 File Offset: 0x0000CDB6
		// (set) Token: 0x0600120E RID: 4622 RVA: 0x0000EBBE File Offset: 0x0000CDBE
		internal ushort SteamPort { get; set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600120F RID: 4623 RVA: 0x0000EBC7 File Offset: 0x0000CDC7
		// (set) Token: 0x06001210 RID: 4624 RVA: 0x0000EBCF File Offset: 0x0000CDCF
		internal ushort GamePort { get; set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06001211 RID: 4625 RVA: 0x0000EBD8 File Offset: 0x0000CDD8
		// (set) Token: 0x06001212 RID: 4626 RVA: 0x0000EBE0 File Offset: 0x0000CDE0
		internal ushort QueryPort { get; set; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06001213 RID: 4627 RVA: 0x0000EBE9 File Offset: 0x0000CDE9
		// (set) Token: 0x06001214 RID: 4628 RVA: 0x0000EBF1 File Offset: 0x0000CDF1
		internal bool PrivateMatch { get; set; }

		// Token: 0x06001215 RID: 4629 RVA: 0x000656E8 File Offset: 0x000638E8
		public override string ToString()
		{
			return string.Format("(SteamConfig SteamPort={0}, GamePort={1}, QueryPort={2}, PrivateMatch={3})", new object[] { this.SteamPort, this.GamePort, this.QueryPort, this.PrivateMatch });
		}
	}
}
