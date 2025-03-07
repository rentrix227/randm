using System;
using Aquiris.Ballistic.Network.Transport.Gameplay.State;
using Steamworks;

namespace Aquiris.Ballistic.Network.Discovery
{
	// Token: 0x0200035E RID: 862
	internal class HostItem
	{
		// Token: 0x060012C0 RID: 4800 RVA: 0x00067F3C File Offset: 0x0006613C
		internal HostItem(EHostType type, string name, string country, uint ping, bool vacSecure, bool password, string passwordData, uint numPlayers, uint maxPlayers, uint numSpectators, uint maxSpectators, ulong map, EGameMode mode, EServerStatus status, CSteamID steamId, CSteamID ownerId, bool isTerminating, int maxPing, CSteamID[] steamUsers, ulong[] gameMaps)
		{
			this.Type = type;
			this.Name = name;
			this.Country = country;
			this.Ping = ping;
			this.VacSecure = vacSecure;
			this.NumPlayers = numPlayers;
			this.MaxPlayers = maxPlayers;
			this.NumSpectators = numSpectators;
			this.MaxSpectators = maxSpectators;
			this.GameMap = map;
			this.GameMode = mode;
			this.Status = status;
			this.SteamId = steamId;
			this.OwnerId = ownerId;
			this.IsTerminating = isTerminating;
			this.MaxPing = maxPing;
			this.Password = password;
			this.PasswordData = passwordData;
			this.SteamUsers = steamUsers;
			this.GameMaps = gameMaps;
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060012C1 RID: 4801 RVA: 0x0000F131 File Offset: 0x0000D331
		// (set) Token: 0x060012C2 RID: 4802 RVA: 0x0000F139 File Offset: 0x0000D339
		internal EHostType Type { get; private set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060012C3 RID: 4803 RVA: 0x0000F142 File Offset: 0x0000D342
		// (set) Token: 0x060012C4 RID: 4804 RVA: 0x0000F14A File Offset: 0x0000D34A
		internal string Name { get; private set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060012C5 RID: 4805 RVA: 0x0000F153 File Offset: 0x0000D353
		// (set) Token: 0x060012C6 RID: 4806 RVA: 0x0000F15B File Offset: 0x0000D35B
		internal string Country { get; private set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060012C7 RID: 4807 RVA: 0x0000F164 File Offset: 0x0000D364
		// (set) Token: 0x060012C8 RID: 4808 RVA: 0x0000F16C File Offset: 0x0000D36C
		internal uint Ping { get; set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060012C9 RID: 4809 RVA: 0x0000F175 File Offset: 0x0000D375
		// (set) Token: 0x060012CA RID: 4810 RVA: 0x0000F17D File Offset: 0x0000D37D
		internal bool VacSecure { get; private set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060012CB RID: 4811 RVA: 0x0000F186 File Offset: 0x0000D386
		// (set) Token: 0x060012CC RID: 4812 RVA: 0x0000F18E File Offset: 0x0000D38E
		internal bool Password { get; private set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060012CD RID: 4813 RVA: 0x0000F197 File Offset: 0x0000D397
		// (set) Token: 0x060012CE RID: 4814 RVA: 0x0000F19F File Offset: 0x0000D39F
		internal string PasswordData { get; private set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060012CF RID: 4815 RVA: 0x0000F1A8 File Offset: 0x0000D3A8
		// (set) Token: 0x060012D0 RID: 4816 RVA: 0x0000F1B0 File Offset: 0x0000D3B0
		internal uint NumPlayers { get; private set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060012D1 RID: 4817 RVA: 0x0000F1B9 File Offset: 0x0000D3B9
		// (set) Token: 0x060012D2 RID: 4818 RVA: 0x0000F1C1 File Offset: 0x0000D3C1
		internal uint MaxPlayers { get; private set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060012D3 RID: 4819 RVA: 0x0000F1CA File Offset: 0x0000D3CA
		// (set) Token: 0x060012D4 RID: 4820 RVA: 0x0000F1D2 File Offset: 0x0000D3D2
		internal ulong GameMap { get; private set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060012D5 RID: 4821 RVA: 0x0000F1DB File Offset: 0x0000D3DB
		// (set) Token: 0x060012D6 RID: 4822 RVA: 0x0000F1E3 File Offset: 0x0000D3E3
		internal EGameMode GameMode { get; private set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060012D7 RID: 4823 RVA: 0x0000F1EC File Offset: 0x0000D3EC
		// (set) Token: 0x060012D8 RID: 4824 RVA: 0x0000F1F4 File Offset: 0x0000D3F4
		internal EServerStatus Status { get; private set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060012D9 RID: 4825 RVA: 0x0000F1FD File Offset: 0x0000D3FD
		// (set) Token: 0x060012DA RID: 4826 RVA: 0x0000F205 File Offset: 0x0000D405
		internal CSteamID SteamId { get; private set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060012DB RID: 4827 RVA: 0x0000F20E File Offset: 0x0000D40E
		// (set) Token: 0x060012DC RID: 4828 RVA: 0x0000F216 File Offset: 0x0000D416
		internal CSteamID OwnerId { get; private set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060012DD RID: 4829 RVA: 0x0000F21F File Offset: 0x0000D41F
		// (set) Token: 0x060012DE RID: 4830 RVA: 0x0000F227 File Offset: 0x0000D427
		internal bool IsTerminating { get; private set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060012DF RID: 4831 RVA: 0x0000F230 File Offset: 0x0000D430
		// (set) Token: 0x060012E0 RID: 4832 RVA: 0x0000F238 File Offset: 0x0000D438
		internal int MaxPing { get; private set; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060012E1 RID: 4833 RVA: 0x0000F241 File Offset: 0x0000D441
		// (set) Token: 0x060012E2 RID: 4834 RVA: 0x0000F249 File Offset: 0x0000D449
		internal uint NumSpectators { get; private set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060012E3 RID: 4835 RVA: 0x0000F252 File Offset: 0x0000D452
		// (set) Token: 0x060012E4 RID: 4836 RVA: 0x0000F25A File Offset: 0x0000D45A
		internal uint MaxSpectators { get; private set; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060012E5 RID: 4837 RVA: 0x0000F263 File Offset: 0x0000D463
		// (set) Token: 0x060012E6 RID: 4838 RVA: 0x0000F26B File Offset: 0x0000D46B
		internal CSteamID[] SteamUsers { get; private set; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060012E7 RID: 4839 RVA: 0x0000F274 File Offset: 0x0000D474
		// (set) Token: 0x060012E8 RID: 4840 RVA: 0x0000F27C File Offset: 0x0000D47C
		internal ulong[] GameMaps { get; private set; }

		// Token: 0x060012E9 RID: 4841 RVA: 0x00067FEC File Offset: 0x000661EC
		public override string ToString()
		{
			return string.Format("(HostItem Type={0}, Name={1}, Country={2}, Ping={3}, VACSecure={4}, NumPlayers={5}, MaxPlayers={6}, NumSpectators={7}, MaxSpectators={8}, GameMap={9}, GameMode={10}, Status={11}, SteamID={12}, OwnerId={13}, IsTerminating={14}, MaxPing={15}, Password={16}, PasswordData={17} )", new object[]
			{
				this.Type, this.Name, this.Country, this.Ping, this.VacSecure, this.NumPlayers, this.MaxPlayers, this.NumSpectators, this.MaxSpectators, this.GameMap,
				this.GameMode, this.Status, this.SteamId, this.OwnerId, this.IsTerminating, this.MaxPing, this.Password, this.PasswordData
			});
		}
	}
}
