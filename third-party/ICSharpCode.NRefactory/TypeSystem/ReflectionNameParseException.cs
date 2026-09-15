using System;
using System.Runtime.Serialization;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents an error while parsing a reflection name.
	/// </summary>
	[Serializable]
	public class ReflectionNameParseException : Exception
	{
		public int Position
		{
			get
			{
				return this.position;
			}
		}

		public ReflectionNameParseException(int position)
		{
			this.position = position;
		}

		public ReflectionNameParseException(int position, string message) : base(message)
		{
			this.position = position;
		}

		public ReflectionNameParseException(int position, string message, Exception innerException) : base(message, innerException)
		{
			this.position = position;
		}

		protected ReflectionNameParseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.position = info.GetInt32("position");
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("position", this.position);
		}

		private int position;
	}
}
