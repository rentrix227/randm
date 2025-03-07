using System;
using System.Runtime.Serialization;

namespace Aquiris.Ballistic.Network
{
	// Token: 0x020003BB RID: 955
	internal class NetworkException : ApplicationException
	{
		// Token: 0x06001515 RID: 5397 RVA: 0x0001043F File Offset: 0x0000E63F
		internal NetworkException()
		{
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x00010447 File Offset: 0x0000E647
		internal NetworkException(string message)
			: base(message)
		{
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x00010450 File Offset: 0x0000E650
		internal NetworkException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x0001045A File Offset: 0x0000E65A
		protected NetworkException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
