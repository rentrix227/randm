using System;

namespace Aquiris.Ballistic.Utils
{
	// Token: 0x020004BF RID: 1215
	[Serializable]
	public class EnumBitMask<T> : BigBitMask where T : struct
	{
		// Token: 0x06001A4E RID: 6734 RVA: 0x00089DB8 File Offset: 0x00087FB8
		public bool IsSet(T p_enumValue)
		{
			int num = (int)((object)p_enumValue);
			int num2 = num / 32;
			if (num2 >= this.m_bits.Count)
			{
				return false;
			}
			int num3 = 1 << num % 32;
			return (this.m_bits[num2] & num3) == num3;
		}

		// Token: 0x06001A4F RID: 6735 RVA: 0x0001350E File Offset: 0x0001170E
		public void Set(T p_bitPosition)
		{
			base.SetState((int)((object)p_bitPosition), true);
		}

		// Token: 0x06001A50 RID: 6736 RVA: 0x00013522 File Offset: 0x00011722
		public void Clear(T p_bitPosition)
		{
			base.SetState((int)((object)p_bitPosition), false);
		}
	}
}
