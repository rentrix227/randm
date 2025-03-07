using System;
using System.Runtime.Serialization;

namespace Aquiris.Ballistic.Network.Transmission.Mapper
{
	// Token: 0x020003C1 RID: 961
	internal class MapperException : NetworkException
	{
		// Token: 0x06001533 RID: 5427 RVA: 0x000105A2 File Offset: 0x0000E7A2
		internal MapperException()
		{
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x000105AA File Offset: 0x0000E7AA
		internal MapperException(string message)
			: base(message)
		{
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x000105B3 File Offset: 0x0000E7B3
		internal MapperException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x000105BD File Offset: 0x0000E7BD
		protected MapperException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
