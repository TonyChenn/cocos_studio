using System;
using System.Runtime.Serialization;

namespace Modules.Communal.PList
{
	[Serializable]
	public class PListFormatException : PListException
	{
		public PListFormatException()
		{
		}

		public PListFormatException(string message) : base(message)
		{
		}

		public PListFormatException(string message, Exception inner) : base(message, inner)
		{
		}

		protected PListFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
