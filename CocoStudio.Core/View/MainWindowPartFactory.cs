using System;
using CocoStudio.Core.ExtensionModel;
using Gtk;
using Mono.Addins;

namespace CocoStudio.Core.View
{
	// Token: 0x02000054 RID: 84
	public class MainWindowPartFactory
	{
		// Token: 0x0600034B RID: 843 RVA: 0x0000EBF0 File Offset: 0x0000CDF0
		public static Widget CreateMainToolbarWidget()
		{
			return MainWindowPartFactory.GetExtensionPart<Widget>("/CocoStudio/Ide/MainToolbar", true);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000EC10 File Offset: 0x0000CE10
		public static Widget CreateMainStatus()
		{
			return MainWindowPartFactory.GetExtensionPart<Widget>("/CocoStudio/Ide/MainStatus", true);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000EC30 File Offset: 0x0000CE30
		public static Widget CreateMainStartPage()
		{
			return MainWindowPartFactory.GetExtensionPart<Widget>("/CocoStudio/Ide/MainStartPage", true);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000EC50 File Offset: 0x0000CE50
		public static IMainRender CreateMainRenderContent()
		{
			return MainWindowPartFactory.GetExtensionPart<IMainRender>("/CocoStudio/Ide/Render", false);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000EC70 File Offset: 0x0000CE70
		public static Widget CreateMainMenu()
		{
			return MainWindowPartFactory.GetExtensionPart<Widget>("/CocoStudio/Ide/MainMenuBar", true);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000EC90 File Offset: 0x0000CE90
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
