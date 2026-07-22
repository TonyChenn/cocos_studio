using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CocoStudio.Core;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using CocoStudio.UserStatistics;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;
using MonoDevelop.Core;
using Xwt;
using Xwt.GtkBackend;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200005A RID: 90
	public class ImageEventBox : EventBox
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000318 RID: 792 RVA: 0x0000C9AC File Offset: 0x0000ABAC
		public string PropertyName
		{
			get
			{
				return this.propertyDescriptor.Name;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000319 RID: 793 RVA: 0x0000C9CC File Offset: 0x0000ABCC
		public string DescriptorName
		{
			get
			{
				return this.propertyDescriptor.Name;
			}
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000C9EC File Offset: 0x0000ABEC
		public ImageEventBox(PropertyItem pItem, PropertyDescriptor pDescriptor, int? size = null)
		{
			base.DragMotion += this.ImageEventBox_DragMotion;
			this.isDefaultFileMarker = true;
			Gtk.Drag.DestSet(this, DestDefaults.All, ImageEventBox.target_tableWindows, DragAction.Copy | DragAction.Move | DragAction.Link);
			Gtk.Drag.SourceSet(this, ModifierType.Button1Mask, ImageEventBox.target_tableWindows, DragAction.Copy | DragAction.Move | DragAction.Link);
			this.propertyItem = pItem;
			this.propertyDescriptor = pDescriptor;
			this.filterAttr = (this.propertyDescriptor.Attributes[typeof(ResourceFilterAttribute)] as ResourceFilterAttribute);
			if (this.propertyItem != null)
			{
				if (size != null)
				{
					this.scaleNum = size.Value;
				}
				this.imageWidget = new Gtk.Image();
				this.imageWidget.WidthRequest = this.scaleNum;
				this.imageWidget.HeightRequest = this.scaleNum;
				base.Add(this.imageWidget);
				this.imageWidget.Show();
				this.Refresh();
				this.InitContextMenu();
				base.Destroyed += this.WidgetDestroyedEventHandler;
			}
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000CB14 File Offset: 0x0000AD14
		private void InitContextMenu()
		{
			this.contextMenu = new Gtk.Menu();
			Gtk.MenuItem menuItem = new Gtk.MenuItem(LanguageInfo.Command_OpenDirectory);
			Gtk.MenuItem menuItem2 = new Gtk.MenuItem(LanguageInfo.Property_CopyFileName);
			Gtk.MenuItem menuItem3 = new Gtk.MenuItem(LanguageInfo.Property_CopyPhyDir);
			this.miReset = new Gtk.MenuItem(LanguageInfo.Scene_Menucontext_ResetDefault);
			this.miClear = new Gtk.MenuItem(LanguageInfo.Menucontext_ClearFiles);
			menuItem.ButtonReleaseEvent += this.menuItemOpen_ButtonReleaseEvent;
			menuItem2.ButtonPressEvent += this.menuItemCoypName_ButtonPressEvent;
			menuItem3.ButtonPressEvent += this.menuItemCopyPhyDir_ButtonPressEvent;
			this.miReset.ButtonPressEvent += this.menuItemReset_ButtonPressEvent;
			this.miClear.ButtonPressEvent += this.menuItemClear_ButtonPressEvent;
			this.contextMenu.Add(menuItem);
			this.contextMenu.Add(menuItem2);
			this.contextMenu.Add(menuItem3);
			this.contextMenu.Add(this.miReset);
			this.contextMenu.Add(this.miClear);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000CC24 File Offset: 0x0000AE24
		private void menuItemOpen_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			if (args.Event.Button == 1U)
			{
				this.OpenFile();
			}
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000CC4E File Offset: 0x0000AE4E
		private void menuItemClear_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			this.propertyDescriptor.SetValue(PropertyItem.FirstObject, null);
			this.Refresh();
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000CC6A File Offset: 0x0000AE6A
		private void menuItemReset_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			this.propertyDescriptor.SetValue(PropertyItem.FirstObject, ResourceFile.DefaultMarker);
			this.Refresh();
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000CC8C File Offset: 0x0000AE8C
		private void menuItemCopyPhyDir_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			if (this.resourceFile != null)
			{
				Xwt.Clipboard.SetText(this.resourceFile.FullPath);
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000CCB8 File Offset: 0x0000AEB8
		private void menuItemCoypName_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			if (this.resourceFile != null)
			{
				Xwt.Clipboard.SetText(this.resourceFile.Name);
			}
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000CCE4 File Offset: 0x0000AEE4
		private void OpenFile()
		{
			try
			{
				ResourceFolder rootFolder = Services.ProjectOperations.CurrentResourceGroup.RootFolder;
				string text = rootFolder.FullPath;
				if (this.resourceFile != null && !this.resourceFile.IsDefault)
				{
					text = this.resourceFile.FileName.FullPath;
				}
				if (MonoDevelop.Core.Platform.IsWindows)
				{
					Process.Start("Explorer", "/select," + text);
				}
				else
				{
					Process.Start("open", "-R " + string.Format("\"{0}\"", text));
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000CD9C File Offset: 0x0000AF9C
		private async void SelectFile()
		{
			ResourceFolder parent = Services.ProjectOperations.CurrentResourceGroup.RootFolder;
			string[] fileTypes;
			if (this.filterAttr == null)
			{
				fileTypes = new string[]
				{
					"*.png",
					"*.jpg"
				};
			}
			else
			{
				fileTypes = new string[this.filterAttr.FileFilter.Length];
				for (int i = 0; i < this.filterAttr.FileFilter.Length; i++)
				{
					fileTypes[i] = "*." + this.filterAttr.FileFilter[i];
				}
			}
			string[] projectPath = FileChooserDialogModel.GetOpenFilePath(fileTypes, LanguageInfo.MessageBox_Content96, false, parent.FullPath).FileNames;
			if (MonoDevelop.Core.Platform.IsMac)
			{
				base.HasFocus = false;
			}
			if (projectPath != null && projectPath.Count<string>() != 0)
			{
				FilePath file = projectPath.FirstOrDefault<string>();
				ResourceFolder rootFolder = Services.ProjectOperations.CurrentResourceGroup.RootFolder;
				string parentDir = System.IO.Path.GetDirectoryName((this.resourceFile == null) ? parent.FullPath : this.resourceFile.FullPath);
				parent = (Services.ProjectOperations.CurrentResourceGroup.FindResourceItem(parentDir) as ResourceFolder);
				if (parent == null || file.IsChildPathOf(rootFolder.BaseDirectory))
				{
					parent = rootFolder;
				}
				IProgressMonitor monitor = Services.ProgressMonitors.Default;
				List<ResourceItem> result = await Services.ProjectOperations.ImportResourcesAsync(parent, projectPath, null);
				ResourceFile resFile = result.FirstOrDefault<ResourceItem>() as ResourceFile;
				this.SetValue(resFile);
			}
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000CDD8 File Offset: 0x0000AFD8
		protected override void OnDragDataGet(DragContext context, SelectionData selection_data, uint info, uint time_)
		{
			base.OnDragDataGet(context, selection_data, info, time_);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000CDE8 File Offset: 0x0000AFE8
		private void ImageEventBox_DragMotion(object o, DragMotionArgs args)
		{
			object dragData = args.Context.GetDragData();
			ResourceInfoDragData resourceInfoDragData = dragData as ResourceInfoDragData;
			if (resourceInfoDragData == null)
			{
				args.SetAllowDragAction((DragAction)0);
				args.RetVal = true;
			}
			else
			{
				ResourceFile resourceFile = resourceInfoDragData.Items.FirstOrDefault<ResourceItem>() as ResourceFile;
				if (resourceFile == null || !this.CheckResource(resourceFile))
				{
					args.SetAllowDragAction((DragAction)0);
					args.RetVal = true;
				}
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000CE6C File Offset: 0x0000B06C
		protected override bool OnDragDrop(DragContext context, int x, int y, uint time_)
		{
			object dragData = context.GetDragData();
			ResourceInfoDragData resourceInfoDragData = dragData as ResourceInfoDragData;
			bool result;
			if (resourceInfoDragData == null || resourceInfoDragData.Items.Count < 1 || !(resourceInfoDragData.Items.FirstOrDefault<ResourceItem>() is ResourceFile))
			{
				result = false;
			}
			else
			{
				ResourceFile resourceFile = (ResourceFile)resourceInfoDragData.Items.FirstOrDefault<ResourceItem>();
				if (resourceFile != null)
				{
					if (this.CheckResource(resourceFile))
					{
						this.SetValue(resourceFile);
						this.resourceFile = resourceFile;
					}
				}
				result = base.OnDragDrop(context, x, y, time_);
			}
			return result;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000CF08 File Offset: 0x0000B108
		private bool CheckResource(ResourceFile file)
		{
			bool result;
			if (this.filterAttr != null)
			{
				result = this.filterAttr.CheckResource(file);
			}
			else
			{
				result = Services.ProjectsService.IsPicture(file.FileName);
			}
			return result;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000CF54 File Offset: 0x0000B154
		private void SetValue(ResourceFile item)
		{
			if (item != null)
			{
				if (this.propertyItem != null)
				{
					this.resourceFile = item;
					string arg;
					if (this.filterAttr == null)
					{
						arg = ".png, jpg";
					}
					else
					{
						arg = string.Join(",", this.filterAttr.FileFilter);
					}
					this.imageWidget.TooltipText = string.Format(LanguageInfo.Display_SupportFileTypes + "\r\n{1}", arg, item.FullPath);
					Pixbuf pixbuf = (item.PreviewImageInfo == null) ? null : item.PreviewImageInfo.Image;
					if (pixbuf != null)
					{
						this.ScaleImage(pixbuf);
					}
					using (CompositeTask.Run(this.propertyItem.Name, null))
					{
						this.propertyDescriptor.SetValue(PropertyItem.FirstObject, item);
						if (this.propertyDescriptor.Name == "LeftImage" || this.propertyDescriptor.Name == "RightImage" || this.propertyDescriptor.Name == "ForwardImage" || this.propertyDescriptor.Name == "BackImage" || this.propertyDescriptor.Name == "UpImage" || this.propertyDescriptor.Name == "DownImage")
						{
							Tracker.Add(ViewRegions.PropertyUC, "SkyBox", "", "");
						}
					}
				}
			}
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000D108 File Offset: 0x0000B308
		public void Refresh()
		{
			if (this.propertyItem != null)
			{
				this.resourceFile = (this.propertyDescriptor.GetValue(PropertyItem.FirstObject) as ResourceFile);
				string arg;
				if (this.filterAttr == null)
				{
					arg = ".png, jpg";
				}
				else
				{
					arg = string.Join(",", this.filterAttr.FileFilter);
				}
				if (this.resourceFile != null)
				{
					string tooltipText = string.Empty;
					if (this.resourceFile.IsDefault)
					{
						tooltipText = string.Format(LanguageInfo.Display_SupportFileTypes, arg);
					}
					else if (string.IsNullOrEmpty(this.resourceFile.FullPath))
					{
						tooltipText = string.Format(LanguageInfo.Display_SupportFileTypes, arg);
					}
					else
					{
						tooltipText = string.Format(LanguageInfo.Display_SupportFileTypes, arg) + "\r\n" + this.resourceFile.FullPath;
					}
					this.imageWidget.TooltipText = tooltipText;
					Pixbuf pixbuf = (this.resourceFile.PreviewImageInfo == null) ? null : this.resourceFile.PreviewImageInfo.Image;
					if (pixbuf != null)
					{
						double num = (double)this.resourceFile.PreviewImageInfo.Size.Width;
						double num2 = (double)this.resourceFile.PreviewImageInfo.Size.Height;
						this.ScaleImage(pixbuf);
					}
					else if (this.imageWidget.Pixbuf != null)
					{
						this.imageWidget.Pixbuf.Dispose();
						this.imageWidget.Pixbuf = null;
					}
				}
				else
				{
					this.imageWidget.TooltipText = string.Format(LanguageInfo.Display_SupportFileTypes, arg);
					this.imageWidget.Pixbuf = ImageIcon.GetPixbuf("CocoStudio.DefaultResource.EditorResource.NormalImage.png");
					this.imageWidget.QueueDraw();
				}
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000D305 File Offset: 0x0000B505
		public void SetDefaultFileMarker()
		{
			this.isDefaultFileMarker = this.filterAttr.DefaultFileMarker;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000D31C File Offset: 0x0000B51C
		private void ScaleImage(Pixbuf image)
		{
			double num = (double)((image.Width > image.Height) ? image.Width : image.Height);
			if (num > (double)this.scaleNum)
			{
				double num2 = (double)this.scaleNum / num;
				int dest_width = (int)((double)image.Width * num2);
				int dest_height = (int)((double)image.Height * num2);
				image = image.ScaleSimple(dest_width, dest_height, InterpType.Bilinear);
			}
			this.imageWidget.Pixbuf = image;
			this.imageWidget.QueueDraw();
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000D3AC File Offset: 0x0000B5AC
		protected override bool OnButtonReleaseEvent(EventButton evnt)
		{
			bool result;
			if (evnt.Button == 3U)
			{
				base.IsFocus = true;
				this.contextMenu.ShowAll();
				GtkWorkarounds.ShowContextMenu(this.contextMenu, this, evnt);
				if (this.resourceFile == ResourceFile.DefaultMarker || this.resourceFile == null || this.resourceFile.IsDefault)
				{
					foreach (Gtk.Widget widget in this.contextMenu.Children)
					{
						widget.Sensitive = false;
					}
					this.miReset.Sensitive = true;
				}
				else
				{
					foreach (Gtk.Widget widget in this.contextMenu.Children)
					{
						widget.Sensitive = true;
					}
				}
				if (this.filterAttr.CanReset)
				{
					this.miClear.Visible = true;
					this.miClear.Sensitive = (this.resourceFile != null);
				}
				else
				{
					this.miClear.Visible = false;
				}
				result = true;
			}
			else
			{
				if (evnt.IsDoubleClick(1U))
				{
					this.SelectFile();
				}
				result = base.OnButtonReleaseEvent(evnt);
			}
			return result;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000D4FE File Offset: 0x0000B6FE
		private void WidgetDestroyedEventHandler(object sender, EventArgs e)
		{
			this.imageWidget.Dispose();
			this.imageWidget = null;
		}

		// Token: 0x04000173 RID: 371
		private static TargetEntry[] target_tableWindows = new TargetEntry[]
		{
			DragTargetType.CocoStudioTarget
		};

		// Token: 0x04000174 RID: 372
		protected Gtk.Image imageWidget;

		// Token: 0x04000175 RID: 373
		private ResourceFile resourceFile;

		// Token: 0x04000176 RID: 374
		private bool isDefaultFileMarker;

		// Token: 0x04000177 RID: 375
		private int scaleNum = 46;

		// Token: 0x04000178 RID: 376
		private PropertyItem propertyItem;

		// Token: 0x04000179 RID: 377
		private PropertyDescriptor propertyDescriptor;

		// Token: 0x0400017A RID: 378
		private ResourceFilterAttribute filterAttr;

		// Token: 0x0400017B RID: 379
		private Gtk.Menu contextMenu;

		// Token: 0x0400017C RID: 380
		private Gtk.MenuItem miReset;

		// Token: 0x0400017D RID: 381
		private Gtk.MenuItem miClear;
	}
}
