using System;
using System.Runtime.Serialization;

namespace Nuclex.Game.Packing
{
	// Token: 0x0200000E RID: 14
	[Serializable]
	public class OutOfSpaceException : Exception
	{
		// Token: 0x0600005A RID: 90 RVA: 0x000042DB File Offset: 0x000024DB
		public OutOfSpaceException()
		{
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000042E6 File Offset: 0x000024E6
		public OutOfSpaceException(string message) : base(message)
		{
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000042F2 File Offset: 0x000024F2
		public OutOfSpaceException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000042FF File Offset: 0x000024FF
		protected OutOfSpaceException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
