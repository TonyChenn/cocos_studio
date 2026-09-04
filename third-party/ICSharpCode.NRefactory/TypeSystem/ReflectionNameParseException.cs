using System;
using System.Runtime.Serialization;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents an error while parsing a reflection name.
	/// </summary>
	// Token: 0x020000F8 RID: 248
	[Serializable]
	public class ReflectionNameParseException : Exception
	{
		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x00018918 File Offset: 0x00017918
		public int Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x00018920 File Offset: 0x00017920
		public ReflectionNameParseException(int position)
		{
			this.position = position;
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0001892F File Offset: 0x0001792F
		public ReflectionNameParseException(int position, string message) : base(message)
		{
			this.position = position;
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0001893F File Offset: 0x0001793F
		public ReflectionNameParseException(int position, string message, Exception innerException) : base(message, innerException)
		{
			this.position = position;
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x00018950 File Offset: 0x00017950
		protected ReflectionNameParseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.position = info.GetInt32("position");
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0001896B File Offset: 0x0001796B
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("position", this.position);
		}

		// Token: 0x040002EC RID: 748
		private int position;
	}
}
