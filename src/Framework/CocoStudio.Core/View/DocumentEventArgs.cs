using System;

namespace CocoStudio.Core.View
{
	// Token: 0x02000042 RID: 66
	public class DocumentEventArgs : EventArgs
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000A300 File Offset: 0x00008500
		// (set) Token: 0x06000239 RID: 569 RVA: 0x0000A317 File Offset: 0x00008517
		public DocumentExtend Document { get; private set; }

		// Token: 0x0600023A RID: 570 RVA: 0x0000A320 File Offset: 0x00008520
		public DocumentEventArgs(DocumentExtend doc)
		{
			this.Document = doc;
		}
	}
}
