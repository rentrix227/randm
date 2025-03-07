using System;
using Aquiris.Ballistic.Network.Address;
using Aquiris.Ballistic.Network.Configuration;
using Aquiris.Ballistic.Network.Transmission.Packet;

namespace Aquiris.Ballistic.Network.Transmission
{
	// Token: 0x020003BF RID: 959
	internal abstract class BasicHostTransmitter<P, A, O, C, BC, CFG> : PacketTransmitter<P, A, O, C, BC> where P : Packet<O, C> where BC : BasicClientInfo<A> where CFG : Config
	{
		// Token: 0x06001525 RID: 5413 RVA: 0x0001051A File Offset: 0x0000E71A
		internal BasicHostTransmitter(CFG config)
		{
			this._config = config;
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06001526 RID: 5414 RVA: 0x00010529 File Offset: 0x0000E729
		internal CFG CurrentConfig
		{
			get
			{
				return this._config;
			}
		}

		// Token: 0x06001527 RID: 5415 RVA: 0x00010531 File Offset: 0x0000E731
		internal virtual void UpdateConfig(CFG config)
		{
			this._config = config;
		}

		// Token: 0x06001528 RID: 5416
		internal abstract void CloseClient(BC client);

		// Token: 0x06001529 RID: 5417
		internal abstract void Start();

		// Token: 0x0600152A RID: 5418
		internal abstract A GetHostID();

		// Token: 0x0600152B RID: 5419 RVA: 0x0001053A File Offset: 0x0000E73A
		protected void DispatchOnHostReady()
		{
			if (this.OnHostReady != null)
			{
				this.OnHostReady();
			}
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x00010552 File Offset: 0x0000E752
		protected void DispatchOnHostFail()
		{
			if (this.OnHostFail != null)
			{
				this.OnHostFail();
			}
		}

		// Token: 0x0400186D RID: 6253
		internal Action OnHostReady;

		// Token: 0x0400186E RID: 6254
		internal Action OnHostFail;

		// Token: 0x0400186F RID: 6255
		private CFG _config;
	}
}
