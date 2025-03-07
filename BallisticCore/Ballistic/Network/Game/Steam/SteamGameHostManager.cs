using System;
using Aquiris.Ballistic.Network.Address.Steam;
using Aquiris.Ballistic.Network.Configuration.Steam;
using Aquiris.Ballistic.Network.Connection.Steam;
using Aquiris.Ballistic.Network.Gameplay.GameMode;
using Aquiris.Ballistic.Network.Transmission.Packet.Thrift;
using Aquiris.Ballistic.Network.Transmission.Steam;
using Steamworks;
using Thrift.Protocol;

namespace Aquiris.Ballistic.Network.Game.Steam
{
	// Token: 0x02000368 RID: 872
	internal class SteamGameHostManager : GameHostManager<ThriftPacket, CSteamID, TBase, ETransportChannel, SteamClientInfo, SteamHostTransmitter, SteamConfig>
	{
		// Token: 0x06001336 RID: 4918 RVA: 0x0000F71D File Offset: 0x0000D91D
		internal SteamGameHostManager(SteamConnectionHostManager p_connectionManager)
			: base(p_connectionManager)
		{
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x0000F726 File Offset: 0x0000D926
		internal override CSteamID GetHostID()
		{
			return this.m_connectionManager.GetHostID();
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x0000F733 File Offset: 0x0000D933
		protected override void CreateGameModeHost()
		{
			this.m_gameModeHost = new GameModeHost(this.m_connectionManager as SteamConnectionHostManager);
		}
	}
}
