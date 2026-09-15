using System;

namespace CocoStudio.Projects
{
	public class DataError
	{
		public string Message { get; private set; }

		public DataError(string message = "")
		{
			this.Message = message;
		}
	}
}
