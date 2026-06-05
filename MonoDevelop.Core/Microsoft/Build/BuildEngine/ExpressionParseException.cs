using System;
using System.Runtime.Serialization;

namespace Microsoft.Build.BuildEngine
{
	// Token: 0x020001E6 RID: 486
	[Serializable]
	internal class ExpressionParseException : Exception
	{
		// Token: 0x06001263 RID: 4707 RVA: 0x0004AC08 File Offset: 0x00048E08
		public ExpressionParseException() : base("Exception occured when parsing an expression.")
		{
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x0004AC15 File Offset: 0x00048E15
		public ExpressionParseException(string message) : base(message)
		{
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x0004AC1E File Offset: 0x00048E1E
		public ExpressionParseException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x0004AC28 File Offset: 0x00048E28
		protected ExpressionParseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
