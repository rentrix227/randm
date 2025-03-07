using System;

namespace Aquiris.Ballistic.Network.Connection.Events
{
	// Token: 0x02000350 RID: 848
	internal class FailConnectionEventArgs : EventArgs
	{
		// Token: 0x0600126D RID: 4717 RVA: 0x0000EE3B File Offset: 0x0000D03B
		internal FailConnectionEventArgs(LeaveGameMotivation motivation)
		{
			this.Motivation = motivation;
		}

		// Token: 0x040016CF RID: 5839
		internal LeaveGameMotivation Motivation;
	}
}
