using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Basic;
using Mono.Addins;

namespace Cocos.Launcher.Core.ExtensionModel
{
	internal class TabContentManager
	{
		static TabContentManager()
		{
			TabContentManager.LoadTabContents();
		}

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

		public static IEnumerable<ITabContent> GetTabContents()
		{
			return TabContentManager.tabContentList;
		}

		private static List<ITabContent> tabContentList;
	}
}
