using System;

namespace Modules.Communal.ResourcePanel
{
	internal class FileTypeItemChangedArgs : EventArgs
	{
		public FileTypeItem Item { get; private set; }

		public FileTypeItemChangedArgs(FileTypeItem item)
		{
			this.Item = item;
		}
	}
}
