using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Aquiris.Ballistic.Network.Transport.Gameplay.State;
using UnityEngine;

namespace Aquiris.Ballistic.Dedicated
{
	// Token: 0x0200005A RID: 90
	[Serializable]
	internal class ConfigParser
	{
		// Token: 0x0600008F RID: 143 RVA: 0x00015CDC File Offset: 0x00013EDC
		internal ConfigParser(string fileName)
		{
			this.Error = false;
			this.ServerName = "No Name";
			this.ServerPort = 27015;
			this.MaxPlayers = 16;
			this.MatchTime = 600;
			this.MaxPing = 120;
			this.GameMap = 1L;
			this.GameMode = EGameMode.TeamDeathMatch;
			this.BannerUrl = null;
			this.ClickUrl = null;
			this.UpdateRate = 40;
			this.NicknameTags = new Dictionary<string, List<long>>();
			this.DedicatedBroadcast = 0;
			this.Autobalance = 0;
			this.Password = null;
			this.Owner = 0L;
			this.Rounds = 7;
			this.RoundTime = 90;
			this.WarmUpTime = 30;
			try
			{
				StreamReader streamReader = new StreamReader(Application.dataPath + "/../" + fileName, Encoding.Default);
				while (!streamReader.EndOfStream)
				{
					string text = streamReader.ReadLine();
					if (text != null)
					{
						text = text.Trim();
						if (!string.IsNullOrEmpty(text) && text[0] != '#')
						{
							this.ReadString(text, "ServerName", ref this.ServerName);
							this.ReadInt(text, "ServerPort", ref this.ServerPort);
							this.ReadInt(text, "QueryPort", ref this.QueryPort);
							this.ReadInt(text, "MaxPlayers", ref this.MaxPlayers);
							this.ReadInt(text, "MaxSpectators", ref this.MaxSpectators);
							this.ReadInt(text, "MaxPing", ref this.MaxPing);
							this.ReadInt(text, "ServerPort", ref this.ServerPort);
							this.ReadLong(text, "GameMap", ref this.GameMap);
							int num = 0;
							if (this.ReadInt(text, "GameMode", ref num))
							{
								this.GameMode = (EGameMode)num;
							}
							this.ReadInt(text, "MatchTime", ref this.MatchTime);
							this.ReadInt(text, "Rounds", ref this.Rounds);
							this.ReadInt(text, "RoundTime", ref this.RoundTime);
							this.ReadInt(text, "WarmUpTime", ref this.WarmUpTime);
							this.ReadInt(text, "Autobalance", ref this.Autobalance);
							this.ReadInt(text, "DedicatedBroadcast", ref this.DedicatedBroadcast);
							this.ReadString(text, "BannerUrl", ref this.BannerUrl);
							this.ReadString(text, "ClickUrl", ref this.ClickUrl);
							this.ReadString(text, "Password", ref this.Password);
							this.ReadString(text, "PipeId", ref this.PipeId);
							this.ReadInt(text, "UpdateRate", ref this.UpdateRate);
							this.ReadLong(text, "OwnerId", ref this.Owner);
							string text2 = null;
							if (this.ReadString(text, "UserTag", ref text2))
							{
								string[] array = text2.Split(new char[] { ',' });
								if (array.Length > 1)
								{
									string text3 = array[0];
									for (int i = 1; i < array.Length; i++)
									{
										long num2;
										if (long.TryParse(array[i].Trim(), out num2))
										{
											this.AddNicknameTag(text3, num2);
										}
									}
								}
							}
						}
					}
				}
				streamReader.Close();
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				this.Error = true;
			}
			if (this.MaxPlayers < 2)
			{
				this.MaxPlayers = 2;
			}
			else if (this.MaxPlayers > 16)
			{
				this.MaxPlayers = 16;
			}
			if (this.MaxPing < 0)
			{
				this.MaxPing = 0;
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00016074 File Offset: 0x00014274
		private List<string> ReadRegex(string line, string regex, RegexOptions options = RegexOptions.None)
		{
			List<string> list = new List<string>();
			MatchCollection matchCollection = Regex.Matches(line, regex, options);
			for (int i = 0; i < matchCollection.Count; i++)
			{
				Match match = matchCollection[i];
				for (int j = 1; j < match.Groups.Count; j++)
				{
					list.Add(match.Groups[j].Value);
				}
			}
			return list;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000160E8 File Offset: 0x000142E8
		private bool ReadString(string line, string paramName, ref string result)
		{
			string text = string.Format("\\s*{0}\\s*=\\s*(.*)", paramName);
			string text2 = this.ReadRegex(line, text, RegexOptions.IgnoreCase).FirstOrDefault<string>();
			if (string.IsNullOrEmpty(text2))
			{
				return false;
			}
			result = text2;
			return true;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00016124 File Offset: 0x00014324
		private bool ReadInt(string line, string paramName, ref int result)
		{
			string text = null;
			if (!this.ReadString(line, paramName, ref text))
			{
				return false;
			}
			int num;
			if (!int.TryParse(text, out num))
			{
				return false;
			}
			result = num;
			return true;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00016158 File Offset: 0x00014358
		private bool ReadLong(string line, string paramName, ref long result)
		{
			string text = null;
			if (!this.ReadString(line, paramName, ref text))
			{
				return false;
			}
			long num;
			if (!long.TryParse(text, out num))
			{
				return false;
			}
			result = num;
			return true;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0001618C File Offset: 0x0001438C
		private bool ReadEnum<T>(string line, string paramName, ref T result)
		{
			string text = null;
			if (!this.ReadString(line, paramName, ref text))
			{
				return false;
			}
			bool flag;
			try
			{
				result = (T)((object)Enum.Parse(typeof(T), text));
				flag = true;
			}
			catch (ArgumentException)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000161E8 File Offset: 0x000143E8
		private void LogList<T>(List<T> list)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (T t in list)
			{
				stringBuilder.AppendLine(t.ToString());
			}
			Debug.Log(stringBuilder.ToString());
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00016260 File Offset: 0x00014460
		private void AddNicknameTag(string tag, long user)
		{
			List<long> list = null;
			if (!this.NicknameTags.TryGetValue(tag, out list))
			{
				list = new List<long>();
				this.NicknameTags.Add(tag, list);
			}
			if (!list.Contains(user))
			{
				list.Add(user);
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000162A8 File Offset: 0x000144A8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[");
			foreach (KeyValuePair<string, List<long>> keyValuePair in this.NicknameTags)
			{
				stringBuilder.Append("{");
				stringBuilder.AppendFormat("tag:'{0}' users:[", keyValuePair.Key);
				foreach (long num in keyValuePair.Value)
				{
					stringBuilder.AppendFormat("{0},", num);
				}
				stringBuilder.Append("]},");
			}
			stringBuilder.Append("]");
			return string.Format("(ConfigParser ServerName={0}, ServerPort={1}, MaxPlayers={2},MaxSpectators={3}, MaxPing={4}, GameMap={5}, GameMode={6}, BannerURL={7}, ClickURL={8} PipeId={9} UpdateRate={10} TagData:{11})", new object[]
			{
				this.ServerName, this.ServerPort, this.MaxPlayers, this.MaxSpectators, this.MaxPing, this.GameMap, this.GameMode, this.BannerUrl, this.ClickUrl, this.PipeId,
				this.UpdateRate, stringBuilder
			});
		}

		// Token: 0x040002B7 RID: 695
		internal bool Error;

		// Token: 0x040002B8 RID: 696
		internal string ServerName;

		// Token: 0x040002B9 RID: 697
		internal int ServerPort;

		// Token: 0x040002BA RID: 698
		internal int QueryPort;

		// Token: 0x040002BB RID: 699
		internal int MaxPlayers;

		// Token: 0x040002BC RID: 700
		internal int MaxPing;

		// Token: 0x040002BD RID: 701
		internal long GameMap;

		// Token: 0x040002BE RID: 702
		internal EGameMode GameMode;

		// Token: 0x040002BF RID: 703
		internal int MatchTime;

		// Token: 0x040002C0 RID: 704
		internal int Rounds;

		// Token: 0x040002C1 RID: 705
		internal int RoundTime;

		// Token: 0x040002C2 RID: 706
		internal int WarmUpTime;

		// Token: 0x040002C3 RID: 707
		internal string BannerUrl;

		// Token: 0x040002C4 RID: 708
		internal string ClickUrl;

		// Token: 0x040002C5 RID: 709
		internal string Password;

		// Token: 0x040002C6 RID: 710
		internal int MaxSpectators;

		// Token: 0x040002C7 RID: 711
		internal string PipeId;

		// Token: 0x040002C8 RID: 712
		internal int UpdateRate;

		// Token: 0x040002C9 RID: 713
		internal int DedicatedBroadcast;

		// Token: 0x040002CA RID: 714
		internal int Autobalance;

		// Token: 0x040002CB RID: 715
		internal long Owner;

		// Token: 0x040002CC RID: 716
		internal Dictionary<string, List<long>> NicknameTags;
	}
}
