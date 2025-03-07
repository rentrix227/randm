using System;
using Aquiris.Ballistic.Game.Services;
using Aquiris.Ballistic.Network.Address;
using Aquiris.Ballistic.Network.Connection.Steam;
using Aquiris.Ballistic.Network.Gameplay.GameMode;
using Aquiris.Ballistic.Network.Transmission.Packet.Thrift;
using Aquiris.Ballistic.Network.Transmission.Steam;
using Aquiris.Ballistic.Network.Transport.Gameplay.State;
using Steamworks;
using Thrift.Protocol;

namespace Aquiris.Ballistic.Network.Game.Steam
{
	// Token: 0x02000367 RID: 871
	internal class SteamGameClientManager : GameClientManager<ThriftPacket, CSteamID, TBase, ETransportChannel, NetworkAddress<CSteamID>, SteamClientTransmitter>
	{
		// Token: 0x06001330 RID: 4912 RVA: 0x0000F6C0 File Offset: 0x0000D8C0
		internal SteamGameClientManager(SteamConnectionClientManager connectionManager)
			: base(connectionManager)
		{
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x0000F6C9 File Offset: 0x0000D8C9
		internal void JoinGame(EClientMode mode, string password)
		{
			this.m_connectionManager.JoinGame(mode, password);
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x0000F6D8 File Offset: 0x0000D8D8
		internal void LeaveGame()
		{
			this.m_connectionManager.LeaveGame(CurrentMatchService.ConnectionState.DISCONNECTED);
			this.m_gameModeClient.Dispose();
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x0000F6F1 File Offset: 0x0000D8F1
		internal bool IsInGame()
		{
			return this.m_connectionManager.IsConnected();
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x0000F6FE File Offset: 0x0000D8FE
		internal bool IsJoiningGame()
		{
			return this.m_connectionManager.IsConnecting();
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x0000F70B File Offset: 0x0000D90B
		protected override GameModeClient CreateGameModeClient()
		{
			return new GameModeClient(this.m_connectionManager as SteamConnectionClientManager);
		}
	}
}
