using System;
using Aquiris.Ballistic.Network.Address;
using Aquiris.Ballistic.Network.Configuration;
using Aquiris.Ballistic.Network.Connection;
using Aquiris.Ballistic.Network.Gameplay.GameMode;
using Aquiris.Ballistic.Network.Transmission;
using Aquiris.Ballistic.Network.Transmission.Packet;

namespace Aquiris.Ballistic.Network.Game
{
	// Token: 0x02000366 RID: 870
	internal abstract class GameHostManager<P, A, O, C, BC, PT, CFG> where P : Packet<O, C> where BC : BasicClientInfo<A> where PT : BasicHostTransmitter<P, A, O, C, BC, CFG> where CFG : Config
	{
		// Token: 0x06001327 RID: 4903 RVA: 0x0000F63E File Offset: 0x0000D83E
		internal GameHostManager(ConnectionHostManager<P, A, O, C, BC, PT, CFG> p_connectionHostManager)
		{
			this.m_connectionManager = p_connectionHostManager;
		}

		// Token: 0x06001328 RID: 4904
		protected abstract void CreateGameModeHost();

		// Token: 0x06001329 RID: 4905 RVA: 0x0000F64D File Offset: 0x0000D84D
		internal void StartHost()
		{
			this.m_connectionManager.StartHost();
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x0000F65A File Offset: 0x0000D85A
		internal void CloseHost()
		{
			if (this.m_connectionManager != null)
			{
				this.m_connectionManager.CloseHost();
			}
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x0000F672 File Offset: 0x0000D872
		internal void UpdateConfig(CFG p_config)
		{
			this.m_connectionManager.UpdateConfig(p_config);
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x0000F680 File Offset: 0x0000D880
		internal void UpdateGameMode(float p_deltaTime)
		{
			this.m_connectionManager.UpdateConnection(p_deltaTime);
			if (this.m_gameModeHost != null)
			{
				this.m_gameModeHost.UpdateGameMode(p_deltaTime);
			}
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x0000F6A5 File Offset: 0x0000D8A5
		internal void StartGameMode()
		{
			if (this.m_gameModeHost == null)
			{
				this.CreateGameModeHost();
			}
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x0000F6B8 File Offset: 0x0000D8B8
		internal GameModeHost GetGameModeHost()
		{
			return this.m_gameModeHost;
		}

		// Token: 0x0600132F RID: 4911
		internal abstract A GetHostID();

		// Token: 0x0400173D RID: 5949
		protected ConnectionHostManager<P, A, O, C, BC, PT, CFG> m_connectionManager;

		// Token: 0x0400173E RID: 5950
		protected GameModeHost m_gameModeHost;
	}
}
