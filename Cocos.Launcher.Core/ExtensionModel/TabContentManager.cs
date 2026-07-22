using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Basic;
using Mono.Addins;

namespace Cocos.Launcher.Core.ExtensionModel
{
	// Token: 0x02000009 RID: 9
	internal class TabContentManager
	{
		// Token: 0x0600003B RID: 59 RVA: 0x00002838 File Offset: 0x00000A38
		static TabContentManager()
		{
			TabContentManager.LoadTabContents();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002848 File Offset: 0x00000A48
		private static void LoadTabContents()
		{
			TabContentManager.tabContentList = new List<ITabContent>();
			try
			{
				ITabContent[] extensionObjects = AddinManager.GetExtensionObjects<ITabContent>();
				IOrderedEnumerable<ITabContent> collection = from a in extensionObjects
				orderby a.Order
				select a;
				TabContentManager.tabContentList.AddRange(collection);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("LoadTabContents failed.", exception);
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000028BC File Offset: 0x00000ABC
		public static IEnumerable<ITabContent> GetTabContents()
		{
			return TabContentManager.tabContentList;
		}

		// Token: 0x04000014 RID: 20
		private static List<ITabContent> tabContentList;
	}
}
