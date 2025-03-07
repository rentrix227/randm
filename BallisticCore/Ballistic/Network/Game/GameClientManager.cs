using System;
using Aquiris.Ballistic.Network.Address;
using Aquiris.Ballistic.Network.Connection;
using Aquiris.Ballistic.Network.Gameplay.GameMode;
using Aquiris.Ballistic.Network.Transmission;
using Aquiris.Ballistic.Network.Transmission.Packet;

namespace Aquiris.Ballistic.Network.Game
{
	// Token: 0x02000365 RID: 869
	internal abstract class GameClientManager<P, A, O, C, NA, PT> where P : Packet<O, C> where NA : NetworkAddress<A> where PT : BasicClientTransmitter<P, A, O, C, NA>
	{
		// Token: 0x06001323 RID: 4899 RVA: 0x0000F601 File Offset: 0x0000D801
		internal GameClientManager(ConnectionClientManager<P, A, O, C, NA, PT> p_connectionManager)
		{
			this.m_connectionManager = p_connectionManager;
			this.m_gameModeClient = this.CreateGameModeClient();
		}

		// Token: 0x06001324 RID: 4900
		protected abstract GameModeClient CreateGameModeClient();

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06001325 RID: 4901 RVA: 0x0000F61C File Offset: 0x0000D81C
		internal GameModeClient GetGameMode
		{
			get
			{
				return this.m_gameModeClient;
			}
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x0000F624 File Offset: 0x0000D824
		internal void UpdateGameMode(float deltaTime)
		{
			this.m_connectionManager.UpdateConnection(deltaTime);
			this.m_gameModeClient.UpdateGameMode(deltaTime);
		}

		// Token: 0x0400173B RID: 5947
		protected ConnectionClientManager<P, A, O, C, NA, PT> m_connectionManager;

		// Token: 0x0400173C RID: 5948
		protected GameModeClient m_gameModeClient;
	}
}
