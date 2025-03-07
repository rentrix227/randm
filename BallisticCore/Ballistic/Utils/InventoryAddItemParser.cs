using System;

namespace Aquiris.Ballistic.Utils
{
	// Token: 0x020004CF RID: 1231
	[Serializable]
	public struct InventoryAddItemParser
	{
		// Token: 0x04001C7A RID: 7290
		public InventoryAddItemParser.ResponseParser response;

		// Token: 0x020004D0 RID: 1232
		[Serializable]
		public struct ResponseParser
		{
			// Token: 0x04001C7B RID: 7291
			public InventoryAddItemParser.ResponseParser.ItemJsonParser[] item_json;

			// Token: 0x04001C7C RID: 7292
			public bool replayed;

			// Token: 0x020004D1 RID: 1233
			[Serializable]
			public struct ItemJsonParser
			{
				// Token: 0x04001C7D RID: 7293
				public string accountid;

				// Token: 0x04001C7E RID: 7294
				public string itemid;

				// Token: 0x04001C7F RID: 7295
				public int quantity;

				// Token: 0x04001C80 RID: 7296
				public string originalitemid;

				// Token: 0x04001C81 RID: 7297
				public string itemdefid;

				// Token: 0x04001C82 RID: 7298
				public int appid;

				// Token: 0x04001C83 RID: 7299
				public string acquired;

				// Token: 0x04001C84 RID: 7300
				public string state;

				// Token: 0x04001C85 RID: 7301
				public string origin;

				// Token: 0x04001C86 RID: 7302
				public string state_changed_timestamp;
			}
		}
	}
}
