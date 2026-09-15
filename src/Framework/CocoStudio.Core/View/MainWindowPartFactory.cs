using System;
using CocoStudio.Core.ExtensionModel;
using Gtk;
using Mono.Addins;

namespace CocoStudio.Core.View
{
	public class MainWindowPartFactory
	{
		public static Widget CreateMainToolbarWidget()
		{
			return MainWindowPartFactory.GetExtensionPart<Widget>("/CocoStudio/Ide/MainToolbar", true);
		}

		public static Widget CreateMainStatus()
		{
			return MainWindowPartFactory.GetExtensionPart<Widget>("/CocoStudio/Ide/MainStatus", true);
		}

		public static Widget CreateMainStartPage()
		{
			return MainWindowPartFactory.GetExtensionPart<Widget>("/CocoStudio/Ide/MainStartPage", true);
		}

		public static IMainRender CreateMainRenderContent()
		{
			return MainWindowPartFactory.GetExtensionPart<IMainRender>("/CocoStudio/Ide/Render", false);
		}

		public static Widget CreateMainMenu()
		{
			return MainWindowPartFactory.GetExtensionPart<Widget>("/CocoStudio/Ide/MainMenuBar", true);
		}

		private static T GetExtensionPart<T>(string extensionPath, bool isCache = true) where T : class
		{
			object[] extensionObjects = AddinManager.GetExtensionObjects(extensionPath, isCache);
			T result;
			if (extensionObjects == null || extensionObjects.Length <= 0)
			{
				result = default(T);
			}
			else
			{
				result = (extensionObjects[0] as T);
			}
			return result;
		}
	}
}
