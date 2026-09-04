using System;
using System.Runtime.Serialization;

namespace Modules.Communal.PList
{
	// Token: 0x02000009 RID: 9
	[Serializable]
	public class PListException : Exception
	{
		// Token: 0x06000051 RID: 81 RVA: 0x00003338 File Offset: 0x00001538
		public PListException()
		{
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003343 File Offset: 0x00001543
		public PListException(string message) : base(message)
		{
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000334F File Offset: 0x0000154F
		public PListException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000335C File Offset: 0x0000155C
		protected PListException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
