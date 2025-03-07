using System;

namespace Aquiris.Ballistic
{
	// Token: 0x020004E7 RID: 1255
	public class BallisticVersion
	{
		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06001AF1 RID: 6897 RVA: 0x00003043 File Offset: 0x00001243
		public static int MAJOR
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06001AF2 RID: 6898 RVA: 0x00006B09 File Offset: 0x00004D09
		public static int MINOR
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06001AF3 RID: 6899 RVA: 0x00013C25 File Offset: 0x00011E25
		public static int REVISION
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06001AF4 RID: 6900 RVA: 0x00003C49 File Offset: 0x00001E49
		public static int HOTFIX
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06001AF5 RID: 6901 RVA: 0x0008B9F0 File Offset: 0x00089BF0
		public static string VERSION
		{
			get
			{
				return string.Format("{0}.{1}.{2}.{3}", new object[]
				{
					BallisticVersion.MAJOR,
					BallisticVersion.MINOR,
					BallisticVersion.REVISION,
					BallisticVersion.HOTFIX
				});
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06001AF6 RID: 6902 RVA: 0x00013C28 File Offset: 0x00011E28
		public static string CLIENT_VERSION
		{
			get
			{
				return string.Format("{0}.0", BallisticVersion.VERSION);
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06001AF7 RID: 6903 RVA: 0x00013C39 File Offset: 0x00011E39
		public static string DEDICATED_SERVER_VERSION
		{
			get
			{
				return string.Format("{0}.1", BallisticVersion.VERSION);
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06001AF8 RID: 6904 RVA: 0x00013C4A File Offset: 0x00011E4A
		public static string OFFICIAL_SERVER_VERSION
		{
			get
			{
				return string.Format("{0}.2", BallisticVersion.VERSION);
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06001AF9 RID: 6905 RVA: 0x0008BA44 File Offset: 0x00089C44
		public static int CLIENT_VERSION_INTEGER
		{
			get
			{
				return int.Parse(string.Format("{0}{1}{2}{3}0", new object[]
				{
					BallisticVersion.MAJOR,
					BallisticVersion.MINOR,
					BallisticVersion.REVISION,
					BallisticVersion.HOTFIX
				}));
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06001AFA RID: 6906 RVA: 0x0008BA9C File Offset: 0x00089C9C
		public static int DEDICATED_SERVER_VERSION_INTEGER
		{
			get
			{
				return int.Parse(string.Format("{0}{1}{2}{3}1", new object[]
				{
					BallisticVersion.MAJOR,
					BallisticVersion.MINOR,
					BallisticVersion.REVISION,
					BallisticVersion.HOTFIX
				}));
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06001AFB RID: 6907 RVA: 0x0008BAF4 File Offset: 0x00089CF4
		public static int OFFICIAL_SERVER_VERSION_INTEGER
		{
			get
			{
				return int.Parse(string.Format("{0}{1}{2}{3}2", new object[]
				{
					BallisticVersion.MAJOR,
					BallisticVersion.MINOR,
					BallisticVersion.REVISION,
					BallisticVersion.HOTFIX
				}));
			}
		}
	}
}
