using System;
using System.Runtime.Serialization;

namespace Nuclex.Game.Packing
{
	[Serializable]
	public class OutOfSpaceException : Exception
	{
		public OutOfSpaceException()
		{
		}

		public OutOfSpaceException(string message) : base(message)
		{
		}

		public OutOfSpaceException(string message, Exception inner) : base(message, inner)
		{
		}

		protected OutOfSpaceException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
