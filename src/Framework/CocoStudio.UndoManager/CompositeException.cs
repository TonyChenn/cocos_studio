using System;
using System.Collections.Generic;

namespace CocoStudio.UndoManager
{
	[Serializable]
	public class CompositeException : Exception
	{
		public IEnumerable<Exception> Exceptions { get; private set; }

		public CompositeException(string message, IEnumerable<Exception> exceptions) : base(message)
		{
			this.Exceptions = exceptions;
		}
	}
}
