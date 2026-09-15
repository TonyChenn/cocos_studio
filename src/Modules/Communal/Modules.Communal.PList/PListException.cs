using System;
using System.Runtime.Serialization;

namespace Modules.Communal.PList
{
	[Serializable]
	public class PListException : Exception
	{
		public PListException()
		{
		}

		public PListException(string message) : base(message)
		{
		}

		public PListException(string message, Exception inner) : base(message, inner)
		{
		}

		protected PListException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
