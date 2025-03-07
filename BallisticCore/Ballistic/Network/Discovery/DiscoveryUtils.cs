using System;
using System.Collections.Generic;
using Aquiris.Ballistic.Game.Utility;
using Aquiris.Ballistic.Network.Transport.Gameplay.State;
using Steamworks;
using UnityEngine;

namespace Aquiris.Ballistic.Network.Discovery
{
	// Token: 0x02000359 RID: 857
	internal class DiscoveryUtils
	{
		// Token: 0x060012A3 RID: 4771 RVA: 0x00067054 File Offset: 0x00065254
		internal static HostItem GameServer2HostItem(gameserveritem_t gameServerItem, ulong[] workshopMaps)
		{
			try
			{
				string[] array = gameServerItem.GetMap().Split(new char[] { '/' });
				ulong num = ulong.Parse(array[0]);
				EGameMode egameMode = (EGameMode)Enum.Parse(typeof(EGameMode), array[1], true);
				string[] array2 = gameServerItem.GetGameTags().Split(new char[] { ',' });
				bool booleanValue = BytePacker.GetBooleanValue(array2[0][0]);
				uint hex64Value = BytePacker.GetHex64Value(array2[0][1]);
				uint hex64Value2 = BytePacker.GetHex64Value(array2[0][2]);
				uint hex64Value3 = BytePacker.GetHex64Value(array2[0][3]);
				uint hex64Value4 = BytePacker.GetHex64Value(array2[0][4]);
				int num2 = int.Parse(array2[1]);
				bool flag = array2[2] != "null";
				string text = ((!flag) ? string.Empty : array2[2]);
				CSteamID[] array3 = new CSteamID[0];
				if (array2[3] != "null")
				{
					ulong[] array4 = BytePacker.UnpackString(array2[3]);
					array3 = new CSteamID[array4.Length];
					for (int i = 0; i < array4.Length; i++)
					{
						array3[i] = new CSteamID(array4[i]);
					}
				}
				EServerStatus eserverStatus;
				if (gameServerItem.m_nServerVersion == BallisticVersion.OFFICIAL_SERVER_VERSION_INTEGER)
				{
					eserverStatus = EServerStatus.OFFICIAL;
				}
				else if (gameServerItem.m_nServerVersion == BallisticVersion.DEDICATED_SERVER_VERSION_INTEGER)
				{
					eserverStatus = EServerStatus.DEDICATED;
				}
				else
				{
					eserverStatus = EServerStatus.IN_GAME;
				}
				return new HostItem(EHostType.LAN, gameServerItem.GetServerName(), "XX", (uint)gameServerItem.m_nPing, gameServerItem.m_bSecure, flag, text, hex64Value, hex64Value2, hex64Value3, hex64Value4, num, egameMode, eserverStatus, gameServerItem.m_steamID, new CSteamID(0UL), booleanValue, num2, array3, workshopMaps);
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x00067234 File Offset: 0x00065434
		internal static string GameServerItemFormattedString(gameserveritem_t gsi)
		{
			return string.Concat(new object[]
			{
				"m_NetAdr: ",
				gsi.m_NetAdr.GetConnectionAddressString(),
				"\nm_nPing: ",
				gsi.m_nPing,
				"\nm_bHadSuccessfulResponse: ",
				gsi.m_bHadSuccessfulResponse,
				"\nm_bDoNotRefresh: ",
				gsi.m_bDoNotRefresh,
				"\nm_szGameDir: ",
				gsi.GetGameDir(),
				"\nm_szMap: ",
				gsi.GetMap(),
				"\nm_szGameDescription: ",
				gsi.GetGameDescription(),
				"\nm_nAppID: ",
				gsi.m_nAppID,
				"\nm_nPlayers: ",
				gsi.m_nPlayers,
				"\nm_nMaxPlayers: ",
				gsi.m_nMaxPlayers,
				"\nm_nBotPlayers: ",
				gsi.m_nBotPlayers,
				"\nm_bPassword: ",
				gsi.m_bPassword,
				"\nm_bSecure: ",
				gsi.m_bSecure,
				"\nm_ulTimeLastPlayed: ",
				gsi.m_ulTimeLastPlayed,
				"\nm_nServerVersion: ",
				gsi.m_nServerVersion,
				"\nm_szServerName: ",
				gsi.GetServerName(),
				"\nm_szGameTags: ",
				gsi.GetGameTags(),
				"\nm_steamID: ",
				gsi.m_steamID,
				"\n"
			});
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x000673E4 File Offset: 0x000655E4
		internal static HostItem SteamLobby2HostItem(CSteamID lobbyId)
		{
			string lobbyData = SteamMatchmaking.GetLobbyData(lobbyId, "sn");
			ulong num = ulong.Parse(SteamMatchmaking.GetLobbyData(lobbyId, "ma"));
			string lobbyData2 = SteamMatchmaking.GetLobbyData(lobbyId, "gm");
			uint num2 = uint.Parse(SteamMatchmaking.GetLobbyData(lobbyId, "np"));
			uint num3 = uint.Parse(SteamMatchmaking.GetLobbyData(lobbyId, "mp"));
			uint num4 = uint.Parse(SteamMatchmaking.GetLobbyData(lobbyId, "ns"));
			uint num5 = uint.Parse(SteamMatchmaking.GetLobbyData(lobbyId, "ms"));
			int num6 = int.Parse(SteamMatchmaking.GetLobbyData(lobbyId, "pi"));
			string lobbyData3 = SteamMatchmaking.GetLobbyData(lobbyId, "r");
			bool flag = bool.Parse(SteamMatchmaking.GetLobbyData(lobbyId, "gs"));
			bool flag2 = SteamMatchmaking.GetLobbyData(lobbyId, "pw") != "null";
			string text = ((!flag2) ? string.Empty : SteamMatchmaking.GetLobbyData(lobbyId, "pw"));
			string lobbyData4 = SteamMatchmaking.GetLobbyData(lobbyId, "md");
			string lobbyData5 = SteamMatchmaking.GetLobbyData(lobbyId, "su");
			ulong[] array = new ulong[0];
			ulong[] array2 = new ulong[0];
			if (!string.IsNullOrEmpty(lobbyData4))
			{
				array = BytePacker.UnpackString(lobbyData4);
			}
			if (!string.IsNullOrEmpty(lobbyData5))
			{
				array2 = BytePacker.UnpackString(lobbyData5);
			}
			CSteamID[] array3 = new CSteamID[array2.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				array3[i] = new CSteamID(array2[i]);
			}
			CSteamID lobbyOwner = SteamMatchmaking.GetLobbyOwner(lobbyId);
			EGameMode egameMode = (EGameMode)Enum.Parse(typeof(EGameMode), lobbyData2, true);
			EServerStatus eserverStatus = ((!flag) ? EServerStatus.IN_LOBBY : EServerStatus.IN_GAME);
			return new HostItem(EHostType.Lobby, lobbyData, lobbyData3, 0U, true, flag2, text, num2, num3, num4, num5, num, egameMode, eserverStatus, lobbyId, lobbyOwner, false, num6, array3, array);
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x000675B0 File Offset: 0x000657B0
		internal static bool CheckSteamLobbyVersion(CSteamID lobbyId)
		{
			string lobbyData = SteamMatchmaking.GetLobbyData(lobbyId, "v");
			return !string.IsNullOrEmpty(lobbyData) && lobbyData == BallisticVersion.VERSION;
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x000675E4 File Offset: 0x000657E4
		internal static bool CheckFilters(HostItem item, KeyValuePair<string, string>[] filters)
		{
			foreach (KeyValuePair<string, string> keyValuePair in filters)
			{
				try
				{
					string key = keyValuePair.Key;
					if (key != null)
					{
						if (!(key == "map_name"))
						{
							if (!(key == "game_mode"))
							{
								if (key == "ping")
								{
									if (item.Ping > uint.Parse(keyValuePair.Value))
									{
										return false;
									}
								}
							}
							else if (item.GameMode.ToString() != keyValuePair.Value)
							{
								return false;
							}
						}
						else if (item.GameMap.ToString() != keyValuePair.Value)
						{
							return false;
						}
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning("Error parsing filters: " + ex);
				}
			}
			return true;
		}

		// Token: 0x040016EF RID: 5871
		private const int _lobbyTimeTimeout = 5;

		// Token: 0x040016F0 RID: 5872
		private const string nil = "null";
	}
}
