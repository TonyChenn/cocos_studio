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
	// Token: 0x02000002 RID: 2
	[Extension(typeof(IObjectDragMenu))]
	public class ObjectDragMenu : IObjectDragMenu
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		private VisualObject dragObject { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002069 File Offset: 0x00000269
		private ResourceItem dragResourceItem { get; set; }

		// Token: 0x06000005 RID: 5 RVA: 0x00002074 File Offset: 0x00000274
		private ResourceFilterAttribute GetFileFilter(object obj, string propertyName)
		{
			ResourceFilterAttribute[] array = obj.GetType().GetProperty(propertyName).GetCustomAttributes(typeof(ResourceFilterAttribute), false) as ResourceFilterAttribute[];
			if (array == null || array.Length == 0)
			{
				return null;
			}
			return array[0];
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020B0 File Offset: 0x000002B0
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

		// Token: 0x06000007 RID: 7 RVA: 0x00002220 File Offset: 0x00000420
		private bool CheckResourceType(EnumResourceType resourceType, ResourceFilterAttribute resourceFilter)
		{
			return this.resourceFiltes == null || resourceFilter.ResourceTypeFilter == null || resourceFilter.ResourceTypeFilter.Contains(resourceType);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002240 File Offset: 0x00000440
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

		// Token: 0x06000009 RID: 9 RVA: 0x00002290 File Offset: 0x00000490
		public bool CanShow(DragMenuShowingArgs args)
		{
			this.resourceSet = ResourcePropertyHelp.GetResourceProperties(args.DragObject);
			return this.CheckResource(args);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000022B8 File Offset: 0x000004B8
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

		// Token: 0x0600000B RID: 11 RVA: 0x00002364 File Offset: 0x00000564
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

		// Token: 0x0600000C RID: 12 RVA: 0x000023C4 File Offset: 0x000005C4
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

		// Token: 0x0600000D RID: 13 RVA: 0x00002410 File Offset: 0x00000610
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

		// Token: 0x04000001 RID: 1
		private HashSet<string> resourceSet;

		// Token: 0x04000002 RID: 2
		private List<ResourceFilterAttribute> resourceFiltes = new List<ResourceFilterAttribute>();

		// Token: 0x04000003 RID: 3
		private List<string> propertyNames = new List<string>();
	}
}
