using System;
using System.Collections;
using System.Collections.Generic;
using Aquiris.Ballistic.Network.Transport.Gameplay.Missions;
using Steamworks;
using UnityEngine;
using UnityEngine.Networking;

namespace Aquiris.Ballistic.Utils
{
	// Token: 0x020004C6 RID: 1222
	internal static class HttpServiceConstants
	{
		// Token: 0x06001A62 RID: 6754 RVA: 0x0008A06C File Offset: 0x0008826C
		internal static IEnumerator GetMissions(ulong playerId, Action<ulong, Missions> onDownload)
		{
			string uri = "https://yptg3ccnw0.execute-api.us-east-1.amazonaws.com/prod/BO_MSS_MISSIONS";
			UnityWebRequest www = UnityWebRequest.Put(uri, "{ \"steamid\": \"" + playerId.ToString() + "\" }");
			www.method = "POST";
			www.SetRequestHeader("Content-Type", "application/json");
			yield return www.SendWebRequest();
			if (!www.isHttpError && !www.isNetworkError)
			{
				try
				{
					Missions missions = JsonUtility.FromJson<Missions>(www.downloadHandler.text);
					onDownload(playerId, missions);
				}
				catch (Exception ex)
				{
					Debug.Log(string.Concat(new string[]
					{
						"[GetMissions] Error:",
						ex.Message,
						":",
						ex.StackTrace,
						":",
						www.downloadHandler.text
					}));
				}
			}
			else
			{
				Debug.Log("[GetMissions] Error:" + www.error);
			}
			yield break;
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x0008A090 File Offset: 0x00088290
		internal static IEnumerator SaveStats(string apikey, ulong playerId, Dictionary<string, int> stats, Action<ulong> OnStatsSaved)
		{
			WWWForm form = new WWWForm();
			form.AddField("key", apikey);
			form.AddField("appid", "296300");
			form.AddField("steamid", playerId.ToString());
			form.AddField("count", stats.Count.ToString());
			int i = 0;
			foreach (KeyValuePair<string, int> keyValuePair in stats)
			{
				form.AddField("name[" + i + "]", keyValuePair.Key);
				form.AddField("value[" + i + "]", keyValuePair.Value);
				i++;
			}
			WWW www = new WWW("https://partner.steam-api.com/ISteamUserStats/SetUserStatsForGame/v1/", form);
			yield return www;
			if (string.IsNullOrEmpty(www.error))
			{
				if (OnStatsSaved != null)
				{
					OnStatsSaved(playerId);
				}
			}
			else
			{
				Debug.Log("[SaveStats] Error:" + www.error);
			}
			yield break;
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x0008A0C0 File Offset: 0x000882C0
		internal static IEnumerator UploadMissionRewards(string apikey, string nickname, ulong playerId, int pointerId, EMissionCategory category, int missionIndex, Action<ulong, int, EMissionCategory, int, InventoryAddItemParser, string> onMissionReward)
		{
			Debug.Log(string.Concat(new object[] { "Uploading rewards of pointerId[", pointerId, "] for Player[", nickname, " ", playerId, "]" }));
			WWWForm form = new WWWForm();
			form.AddField("key", apikey);
			form.AddField("appid", "296300");
			switch (category)
			{
			case EMissionCategory.EASY:
				form.AddField("itemdefid[0]", 900);
				form.AddField("itemdefid[1]", 900);
				break;
			case EMissionCategory.MEDIUM:
				form.AddField("itemdefid[0]", 900);
				form.AddField("itemdefid[1]", 900);
				form.AddField("itemdefid[2]", 900);
				break;
			case EMissionCategory.HARD:
				form.AddField("itemdefid[0]", 900);
				form.AddField("itemdefid[1]", 900);
				form.AddField("itemdefid[2]", 900);
				form.AddField("itemdefid[3]", 900);
				break;
			case EMissionCategory.GOLD:
				form.AddField("itemdefid[0]", 1043);
				break;
			}
			form.AddField("steamid", playerId.ToString());
			form.AddField("notify", 1);
			WWW www = new WWW("https://partner.steam-api.com/IInventoryService/AddItem/v1/", form);
			yield return www;
			if (string.IsNullOrEmpty(www.error))
			{
				try
				{
					string text = www.text.Replace("\\", string.Empty).Replace(" ", string.Empty).Replace("\"[", "[")
						.Replace("]\"", "]");
					InventoryAddItemParser inventoryAddItemParser = JsonUtility.FromJson<InventoryAddItemParser>(text);
					if (onMissionReward != null)
					{
						onMissionReward(playerId, pointerId, category, missionIndex, inventoryAddItemParser, www.text);
					}
				}
				catch (Exception ex)
				{
					Debug.Log(string.Concat(new string[] { "[InventoryRewardContent] Error:", ex.Message, ":", ex.StackTrace, ":", www.text }));
				}
			}
			else
			{
				Debug.Log("[InventoryRewardContent] Error:" + www.error);
			}
			yield break;
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x0008A108 File Offset: 0x00088308
		internal static IEnumerator GetCurrentWeekMonth(Action<bool, int, int> OnCurrentWeekLoaded)
		{
			WWW www = new WWW("https://yptg3ccnw0.execute-api.us-east-1.amazonaws.com/prod/BO_LE_CURRENT_LEADER");
			yield return www;
			int _currentWeek = -1;
			int _currentMonth = -1;
			if (!string.IsNullOrEmpty(www.error))
			{
				Debug.LogWarning("Error at loading current week.");
				OnCurrentWeekLoaded(false, _currentWeek, _currentMonth);
			}
			else
			{
				string[] array = www.text.Split(new char[] { '\n', '\r', ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length > 0)
				{
					if (!int.TryParse(array[0], out _currentWeek) || !int.TryParse(array[1], out _currentMonth))
					{
						OnCurrentWeekLoaded(false, _currentWeek, _currentMonth);
					}
					else
					{
						OnCurrentWeekLoaded(true, _currentWeek, _currentMonth);
					}
				}
				else
				{
					OnCurrentWeekLoaded(false, _currentWeek, _currentMonth);
				}
			}
			yield break;
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x0008A124 File Offset: 0x00088324
		internal static IEnumerator UploadScore(CSteamID user, string name, string apikey, Leaderboards leader, int leaderboardSuffix, int score)
		{
			if (score <= 0)
			{
				yield break;
			}
			string leaderboardKey = StatisticsConstants.gperf_leaderboard_name(leader, leaderboardSuffix);
			if (!HttpServiceConstants._leaderboardIds.ContainsKey(leaderboardKey))
			{
				HttpServiceConstants._leaderboardIds.Add(leaderboardKey, 0UL);
				WWWForm formf = new WWWForm();
				formf.AddField("key", apikey);
				formf.AddField("appid", "296300");
				formf.AddField("name", leaderboardKey);
				formf.AddField("sortmethod", "Descending");
				formf.AddField("onlytrustedwrites", "1");
				WWW wwwf = new WWW("https://partner.steam-api.com/ISteamLeaderboards/FindOrCreateLeaderboard/v2", formf);
				yield return wwwf;
				if (!string.IsNullOrEmpty(wwwf.error))
				{
					Debug.Log(string.Concat(new object[] { "Data of [", user.m_SteamID, "] not uploaded!\nError:", wwwf.error }));
					yield break;
				}
				LeaderboardFindOrCreateParser parser = JsonUtility.FromJson<LeaderboardFindOrCreateParser>(wwwf.text);
				if (parser.result.result <= 0)
				{
					Debug.Log("Data of [" + user.m_SteamID + "] not uploaded!\nError: LeaderboardFind returned 0 items.");
					yield break;
				}
				HttpServiceConstants._leaderboardIds[leaderboardKey] = parser.result.leaderboard.leaderBoardID;
			}
			else if (HttpServiceConstants._leaderboardIds[leaderboardKey] == 0UL)
			{
				float timeout = 20f;
				do
				{
					timeout -= Time.deltaTime;
					yield return null;
				}
				while (HttpServiceConstants._leaderboardIds[leaderboardKey] == 0UL && timeout > 0f);
				if (HttpServiceConstants._leaderboardIds[leaderboardKey] == 0UL)
				{
					Debug.Log("Data of [" + user.m_SteamID + "] not uploaded!\nError: LeaderboardID timeout!");
					yield break;
				}
			}
			string method = ((score >= 100) ? "ForceUpdate" : "KeepBest");
			WWWForm form = new WWWForm();
			form.AddField("key", apikey);
			form.AddField("appid", "296300");
			form.AddField("leaderboardid", HttpServiceConstants._leaderboardIds[leaderboardKey].ToString());
			form.AddField("steamid", user.m_SteamID.ToString());
			form.AddField("score", score.ToString());
			form.AddField("scoremethod", method);
			WWW www = new WWW("https://partner.steam-api.com/ISteamLeaderboards/SetLeaderboardScore/v1/", form);
			yield return www;
			if (!string.IsNullOrEmpty(www.error))
			{
				Debug.Log(string.Concat(new object[] { "Data of [", user.m_SteamID, "] not uploaded!\nError:", www.error }));
			}
			yield break;
		}

		// Token: 0x06001A67 RID: 6759 RVA: 0x00013632 File Offset: 0x00011832
		internal static void Flush()
		{
			if (HttpServiceConstants._leaderboardIds.Count > 50)
			{
				HttpServiceConstants._leaderboardIds.Clear();
			}
		}

		// Token: 0x04001C32 RID: 7218
		private static Dictionary<string, ulong> _leaderboardIds = new Dictionary<string, ulong>();
	}
}
