using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using CocoStudio.Model.ViewModel;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x02000010 RID: 16
	public class MenuManager
	{
		// Token: 0x06000055 RID: 85 RVA: 0x000030EC File Offset: 0x000012EC
		public MenuManager()
		{
			this.InitAllMenu();
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003109 File Offset: 0x00001309
		private void InitAllMenu()
		{
			this.CreateMenuDictionary();
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003114 File Offset: 0x00001314
		private void CreateMenuDictionary()
		{
			try
			{
				ICustomMenu[] extensionObjects = AddinManager.GetExtensionObjects<ICustomMenu>();
				foreach (ICustomMenu customMenu in extensionObjects)
				{
					Type objectType = customMenu.GetObjectType();
					if (!objectType.IsSubclassOf(typeof(AbstractNodeObject)))
					{
						LogConfig.Logger.Error("Custom menu object type must be subclass of nodeObject.", null);
					}
					else if (!this.menuDictionary.ContainsKey(objectType))
					{
						this.menuDictionary.Add(customMenu.GetObjectType(), customMenu as NodeObjectMenu);
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Create custom menu failed", exception);
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000031D8 File Offset: 0x000013D8
		public NodeObjectMenu getWidgetMenu(Type type)
		{
			NodeObjectMenu result = null;
			this.menuDictionary.TryGetValue(type, out result);
			return result;
		}

		// Token: 0x04000025 RID: 37
		public Dictionary<Type, NodeObjectMenu> menuDictionary = new Dictionary<Type, NodeObjectMenu>();
	}
}
