using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using CocoStudio.ControlLib;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Render.ExtensionModel;
using Mono.Addins;

namespace Modules.Communal.DragMenu
{
	[Extension(typeof(IObjectDragMenu))]
	public class ObjectDragMenu : IObjectDragMenu
	{
		private VisualObject dragObject { get; set; }

		private ResourceItem dragResourceItem { get; set; }

		private ResourceFilterAttribute GetFileFilter(object obj, string propertyName)
		{
			ResourceFilterAttribute[] array = obj.GetType().GetProperty(propertyName).GetCustomAttributes(typeof(ResourceFilterAttribute), false) as ResourceFilterAttribute[];
			if (array == null || array.Length == 0)
			{
				return null;
			}
			return array[0];
		}

		private bool CheckResource(DragMenuShowingArgs args)
		{
			ResourceData resourceData = args.DragResourceItem.GetResourceData();
			this.propertyNames.Clear();
			if (this.resourceSet != null && this.resourceSet.Count > 0)
			{
				foreach (string text in this.resourceSet)
				{
					ResourceFilterAttribute fileFilter = this.GetFileFilter(args.DragObject, text);
					bool flag = this.CheckResourceType(resourceData.Type, fileFilter);
					if (flag)
					{
						flag = this.CheckFileType(resourceData.Path, fileFilter);
						if (flag && flag)
						{
							this.propertyNames.Add(text);
						}
					}
				}
			}
			if (this.propertyNames.Count == 1)
			{
				string name = this.propertyNames.FirstOrDefault<string>();
				PropertyInfo property = args.DragObject.GetType().GetProperty(name);
				if (property != null && property.PropertyType.IsAssignableFrom(args.DragResourceItem.GetType()))
				{
					property.SetValue(args.DragObject, args.DragResourceItem, null);
					IPlayControl playControl = args.DragObject as IPlayControl;
					if (playControl != null)
					{
						playControl.IsPlaying = true;
					}
				}
				return true;
			}
			if (this.propertyNames.Count > 0)
			{
				this.dragObject = args.DragObject;
				this.dragResourceItem = args.DragResourceItem;
			}
			return this.propertyNames.Count > 0;
		}

		private bool CheckResourceType(EnumResourceType resourceType, ResourceFilterAttribute resourceFilter)
		{
			return this.resourceFiltes == null || resourceFilter.ResourceTypeFilter == null || resourceFilter.ResourceTypeFilter.Contains(resourceType);
		}

		private bool CheckFileType(string fileName, ResourceFilterAttribute resourceFilter)
		{
			if (this.resourceFiltes != null)
			{
				int count = this.resourceFiltes.Count;
				bool result = false;
				foreach (string value in resourceFilter.FileFilter)
				{
					if (fileName.EndsWith(value, StringComparison.CurrentCultureIgnoreCase))
					{
						result = true;
						break;
					}
				}
				return result;
			}
			return true;
		}

		public bool CanShow(DragMenuShowingArgs args)
		{
			this.resourceSet = ResourcePropertyHelp.GetResourceProperties(args.DragObject);
			return this.CheckResource(args);
		}

		public Menu GetPopupMenu()
		{
			if (this.propertyNames.Count > 1)
			{
				Menu menu = new Menu();
				menu.Hidden += this.result_Hidden;
				foreach (string text in this.propertyNames)
				{
					string menuHeader = this.GetMenuHeader(this.dragObject, text);
					StudioMenuItem studioMenuItem = new StudioMenuItem(menuHeader);
					studioMenuItem.ButtonReleaseEvent += this.menuItem_ButtonReleaseEvent;
					studioMenuItem.Tag = text;
					menu.Add(studioMenuItem);
				}
				return menu;
			}
			return null;
		}

		private void result_Hidden(object sender, EventArgs e)
		{
			Menu menu = sender as Menu;
			menu.Hidden -= this.result_Hidden;
			foreach (Widget widget in menu.Children)
			{
				widget.ButtonReleaseEvent -= this.menuItem_ButtonReleaseEvent;
			}
			this.dragObject = null;
			this.dragResourceItem = null;
		}

		private string GetMenuHeader(object obj, string name)
		{
			PropertyInfo property = obj.GetType().GetProperty(name);
			DisplayNameAttribute[] array = property.GetCustomAttributes(typeof(DisplayNameAttribute), false) as DisplayNameAttribute[];
			if (array == null || array.Length == 0)
			{
				return name;
			}
			string displayName = array[0].DisplayName;
			return LanguageOption.GetValueBykey(displayName);
		}

		private void menuItem_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			StudioMenuItem studioMenuItem = o as StudioMenuItem;
			if (studioMenuItem.Tag != null)
			{
				string name = studioMenuItem.Tag.ToString();
				PropertyInfo property = this.dragObject.GetType().GetProperty(name);
				if (property != null)
				{
					using (CompositeTask.Run("设置资源文件", null))
					{
						property.SetValue(this.dragObject, this.dragResourceItem, null);
					}
				}
			}
		}

		private HashSet<string> resourceSet;

		private List<ResourceFilterAttribute> resourceFiltes = new List<ResourceFilterAttribute>();

		private List<string> propertyNames = new List<string>();
	}
}
