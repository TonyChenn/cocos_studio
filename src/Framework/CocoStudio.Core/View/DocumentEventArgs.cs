using System;

namespace CocoStudio.Core.View
{
	public class DocumentEventArgs : EventArgs
	{
		public DocumentExtend Document { get; private set; }

		public DocumentEventArgs(DocumentExtend doc)
		{
			this.Document = doc;
		}
	}
}
