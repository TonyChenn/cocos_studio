using System;
using System.Runtime.Serialization;

namespace Modules.Communal.PList
{
	// Token: 0x0200000A RID: 10
	[Serializable]
	public class PListFormatException : PListException
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00003369 File Offset: 0x00001569
		public PListFormatException()
		{
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003374 File Offset: 0x00001574
		public PListFormatException(string message) : base(message)
		{
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003380 File Offset: 0x00001580
		public PListFormatException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000338D File Offset: 0x0000158D
		protected PListFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
