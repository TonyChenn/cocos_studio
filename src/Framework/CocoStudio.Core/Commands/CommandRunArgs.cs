using System;

namespace CocoStudio.Core.Commands
{
	public class CommandRunArgs : EventArgs
	{
		public object DataItem { get; private set; }

		internal CommandRunArgs(object data)
		{
			this.DataItem = data;
		}
	}
}
