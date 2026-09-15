using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using CocoStudio.Model.ViewModel;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	public class MenuManager
	{
		public MenuManager()
		{
			this.InitAllMenu();
		}

		private void InitAllMenu()
		{
			this.CreateMenuDictionary();
		}

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

		public NodeObjectMenu getWidgetMenu(Type type)
		{
			NodeObjectMenu result = null;
			this.menuDictionary.TryGetValue(type, out result);
			return result;
		}

		public Dictionary<Type, NodeObjectMenu> menuDictionary = new Dictionary<Type, NodeObjectMenu>();
	}
}
