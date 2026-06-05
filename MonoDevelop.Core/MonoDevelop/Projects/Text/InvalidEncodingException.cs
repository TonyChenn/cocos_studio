using System;

namespace MonoDevelop.Projects.Text
{
	// Token: 0x020001FC RID: 508
	[Serializable]
	public class InvalidEncodingException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:InvalidEncodingException" /> class
		/// </summary>
		// Token: 0x06001357 RID: 4951 RVA: 0x0004F7EF File Offset: 0x0004D9EF
		public InvalidEncodingException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:InvalidEncodingException" /> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the exception. </param>
		// Token: 0x06001358 RID: 4952 RVA: 0x0004F7F7 File Offset: 0x0004D9F7
		public InvalidEncodingException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:InvalidEncodingException" /> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the exception. </param>
		/// <param name="inner">The exception that is the cause of the current exception. </param>
		// Token: 0x06001359 RID: 4953 RVA: 0x0004F800 File Offset: 0x0004DA00
		public InvalidEncodingException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
