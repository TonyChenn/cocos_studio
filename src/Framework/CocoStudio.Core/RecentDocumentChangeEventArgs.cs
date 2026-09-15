using System;

namespace CocoStudio.Core
{
	public class RecentDocumentChangeEventArgs : EventArgs
	{
		public EnumRecentPrjChangeType ChangeType { get; private set; }

		public CocosItemModel CocosItemModel { get; private set; }

		public RecentDocumentChangeEventArgs(CocosItemModel prj, EnumRecentPrjChangeType changeType)
		{
			this.CocosItemModel = prj;
			this.ChangeType = changeType;
		}
	}
}
