using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Aquiris.Ballistic.Network.Discovery.Routines
{
	// Token: 0x0200035F RID: 863
	internal class DiscoveryRoutine
	{
		// Token: 0x060012EA RID: 4842 RVA: 0x0000F285 File Offset: 0x0000D485
		internal DiscoveryRoutine()
		{
			this.Completed = false;
			this.Canceled = false;
			this.Running = false;
			this.m_filters = new Dictionary<string, string>();
		}

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x060012EB RID: 4843 RVA: 0x00068100 File Offset: 0x00066300
		// (remove) Token: 0x060012EC RID: 4844 RVA: 0x00068138 File Offset: 0x00066338
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<DiscoveryRoutine, HostItem> OnItemFound;

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x060012ED RID: 4845 RVA: 0x00068170 File Offset: 0x00066370
		// (remove) Token: 0x060012EE RID: 4846 RVA: 0x000681A8 File Offset: 0x000663A8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<DiscoveryRoutine> OnCompleted;

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060012EF RID: 4847 RVA: 0x0000F2AD File Offset: 0x0000D4AD
		// (set) Token: 0x060012F0 RID: 4848 RVA: 0x0000F2B5 File Offset: 0x0000D4B5
		internal bool Completed { get; private set; }

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060012F1 RID: 4849 RVA: 0x0000F2BE File Offset: 0x0000D4BE
		// (set) Token: 0x060012F2 RID: 4850 RVA: 0x0000F2C6 File Offset: 0x0000D4C6
		internal bool Canceled { get; private set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060012F3 RID: 4851 RVA: 0x0000F2CF File Offset: 0x0000D4CF
		// (set) Token: 0x060012F4 RID: 4852 RVA: 0x0000F2D7 File Offset: 0x0000D4D7
		internal bool Running { get; private set; }

		// Token: 0x060012F5 RID: 4853 RVA: 0x0000F2E0 File Offset: 0x0000D4E0
		internal virtual void Run()
		{
			this.Completed = false;
			this.Canceled = false;
			this.Running = true;
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x0000F2F7 File Offset: 0x0000D4F7
		internal virtual void Cancel()
		{
			this.Completed = false;
			this.Canceled = true;
			this.Running = false;
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x000681E0 File Offset: 0x000663E0
		internal virtual void AddFilter(string p_key, int p_value, ComparisonFilter p_filter)
		{
			if (this.m_filters.ContainsKey(p_key))
			{
				this.m_filters[p_key] = p_value.ToString();
			}
			else
			{
				this.m_filters.Add(p_key, p_value.ToString());
			}
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x0000F30E File Offset: 0x0000D50E
		internal virtual void AddFilter(string p_key, string p_value, ComparisonFilter p_filter)
		{
			if (this.m_filters.ContainsKey(p_key))
			{
				this.m_filters[p_key] = p_value;
			}
			else
			{
				this.m_filters.Add(p_key, p_value);
			}
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x0000F340 File Offset: 0x0000D540
		internal virtual void ClearFilters()
		{
			this.m_filters.Clear();
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x0000F34D File Offset: 0x0000D54D
		protected virtual void Complete()
		{
			this.Completed = true;
			this.Canceled = false;
			this.Running = false;
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x0000F364 File Offset: 0x0000D564
		protected virtual void DispatchItemFoundAction(HostItem p_item)
		{
			if (this.OnItemFound != null)
			{
				this.OnItemFound(this, p_item);
			}
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x0000F37E File Offset: 0x0000D57E
		protected virtual void DispatchCompleteAction()
		{
			if (this.OnCompleted != null)
			{
				this.OnCompleted(this);
			}
		}

		// Token: 0x04001722 RID: 5922
		protected Dictionary<string, string> m_filters;
	}
}
