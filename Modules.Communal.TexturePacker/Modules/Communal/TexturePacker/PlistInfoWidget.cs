using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Components.Commands;
using Stetic;
using Xwt.GtkBackend;

namespace Modules.Communal.TexturePacker
{
	// Token: 0x02000014 RID: 20
	public class PlistInfoWidget : Bin
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00004FDC File Offset: 0x000031DC
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00004FE4 File Offset: 0x000031E4
		public PlistInfoModel PlistInfoModel { get; private set; }

		// Token: 0x060000C5 RID: 197 RVA: 0x00004FED File Offset: 0x000031ED
		public PlistInfoWidget()
		{
			this.Build();
			Gtk.Drag.DestSet(this, DestDefaults.All, PlistInfoWidget.target_table, DragAction.Copy | DragAction.Move | DragAction.Link);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00005009 File Offset: 0x00003209
		public void Init(PlistInfoModel model)
		{
			this.PlistInfoModel = model;
			this.InitRender();
			this.InitEvent();
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00005020 File Offset: 0x00003220
		private void InitRender()
		{
			this.pictureRender = new PlistInfoItemRender(this.PlistInfoModel);
			this.PlistInfoModel.Render = this.pictureRender;
			this.eventbox.Events = EventMask.AllEventsMask;
			this.eventbox.Add(this.pictureRender);
			this.eventbox.ShowAll();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000507C File Offset: 0x0000327C
		private void InitEvent()
		{
			base.Events = EventMask.AllEventsMask;
			this.eventbox.ButtonPressEvent += this.pictureRender.OnButtonPressEvent;
			this.eventbox.MotionNotifyEvent += this.pictureRender.OnMotionNotifyEvent;
			this.eventbox.ButtonReleaseEvent += this.pictureRender.OnButtonReleaseEvent;
			this.eventbox.KeyPressEvent += this.pictureRender.OnKeyPressEvent;
			this.eventbox.KeyReleaseEvent += this.pictureRender.OnKeyReleaseEvent;
			this.eventbox.ButtonReleaseEvent += this.OnPictureRenderReleaseEvent;
			this.eventbox.ScrollEvent += this.eventbox_ScrollEvent;
			base.DragDrop += this.PackerWidget_DragDrop;
			base.DragDataReceived += this.PackerWidget_DragReceived;
			base.DragMotion += this.PackerWidget_DragMotion;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00005184 File Offset: 0x00003384
		private void eventbox_ScrollEvent(object o, ScrollEventArgs args)
		{
			Adjustment vadjustment = this.midScrollWindow.Vadjustment;
			Adjustment hadjustment = this.midScrollWindow.Hadjustment;
			double num = args.Event.Y;
			double num2 = args.Event.X;
			if (args.Event.Direction == ScrollDirection.Up)
			{
				num -= vadjustment.StepIncrement;
			}
			else if (args.Event.Direction == ScrollDirection.Down)
			{
				num += vadjustment.StepIncrement;
			}
			else if (args.Event.Direction == ScrollDirection.Right)
			{
				num2 += hadjustment.StepIncrement;
			}
			else if (args.Event.Direction == ScrollDirection.Left)
			{
				num2 -= hadjustment.StepIncrement;
			}
			this.pictureRender.ShowToolTip((int)num2, (int)num);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005231 File Offset: 0x00003431
		public void CanvasZoom(float delta)
		{
			this.PlistInfoModel.PictureRenderScale = this.PlistInfoModel.PictureRenderScale + (double)delta;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000524C File Offset: 0x0000344C
		private void OnPictureRenderReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			if (args.Event.IsContextMenuButton())
			{
				this.ShowContextMenu(args.Event);
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005540 File Offset: 0x00003740
		internal async void PackerWidget_DragReceived(object o, DragDataReceivedArgs args)
		{
			using (CompositeTask.Run("KeyReleaseEvent", null))
			{
				DragContext context = args.Context;
				if (context.DragProtocol == DragProtocol.Win32Dropfiles || context.GetSourceWidget() == null)
				{
					IEnumerable<string> fileInfo = args.SelectionData.GetFileArray().FileArray;
					ResourceFolder rootFolder2 = Services.ProjectOperations.CurrentResourceGroup.RootFolder;
					List<string> paths = new List<string>();
					foreach (string item in fileInfo)
					{
						paths.Add(item);
					}
					ResourceFolder rootFolder = Services.ProjectOperations.CurrentResourceGroup.RootFolder;
					IEnumerable<string> filepaths = this.RecursiveImageFilePaths(paths, new HashSet<string>(new List<string>
					{
						".png",
						".jpg"
					}, StringComparer.OrdinalIgnoreCase));
					IEnumerable<ResourceItem> resitems = await Services.ProjectOperations.ImportResourcesAsync(rootFolder, filepaths, null);
					if (resitems != null)
					{
						foreach (ResourceItem item2 in resitems)
						{
							this.AddItemToModelHelper(item2);
						}
					}
				}
				this.PlistInfoModel.Refresh(true);
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00005584 File Offset: 0x00003784
		private IEnumerable<string> RecursiveImageFilePaths(IEnumerable<string> paths, HashSet<string> suffixs)
		{
			List<string> list = new List<string>();
			foreach (string text in paths)
			{
				if (Directory.Exists(text))
				{
					list.AddRange(this.RecursiveImageFilePaths(Directory.EnumerateFiles(text, "*.*", SearchOption.AllDirectories), suffixs));
				}
				else if (File.Exists(text))
				{
					string extension = System.IO.Path.GetExtension(text);
					if (suffixs.Contains(extension))
					{
						list.Add(text);
					}
				}
			}
			return list;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005610 File Offset: 0x00003810
		internal void PackerWidget_DragDrop(object o, DragDropArgs args)
		{
			using (CompositeTask.Run("KeyReleaseEvent", null))
			{
				DragContext context = args.Context;
				ResourceInfoDragData resourceInfoDragData = context.GetDragData() as ResourceInfoDragData;
				if (resourceInfoDragData != null)
				{
					foreach (ResourceItem item in resourceInfoDragData.Items)
					{
						this.AddItemToModelHelper(item);
					}
					this.PlistInfoModel.Refresh(true);
				}
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000056AC File Offset: 0x000038AC
		private void AddItemToModelHelper(ResourceItem item)
		{
			if (item is ResourceFolder)
			{
				ResourceFolder resourceFolder = item as ResourceFolder;
				if (resourceFolder.Items != null)
				{
					foreach (ResourceItem item2 in resourceFolder.Items)
					{
						this.AddItemToModelHelper(item2);
					}
				}
			}
			if (item is ImageFile)
			{
				ImageFile imageFile = item as ImageFile;
				bool flag = false;
				if (!imageFile.IsPacked())
				{
					flag = true;
				}
				else if (!imageFile.HasPackedTo(this.PlistInfoModel.CocosItem))
				{
					string info = imageFile.Name + " : " + LanguageInfo.MessageBox198_PlistAlreadyExist;
					MessageBoxResult messageBoxResult = MessageBox.Show(info, MessageBoxButton.YesNo, MessageBoxImage.Other, null, EnumMainButton.Yes, null);
					if (messageBoxResult == MessageBoxResult.Yes)
					{
						flag = true;
					}
				}
				if (flag)
				{
					imageFile.PackTo(this.PlistInfoModel.CocosItem);
					this.PlistInfoModel.AddItem(imageFile);
				}
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005794 File Offset: 0x00003994
		private bool IsItemSupportPacker(ResourceItem item)
		{
			if (item is ResourceFolder)
			{
				ResourceFolder resourceFolder = item as ResourceFolder;
				if (resourceFolder.Items != null)
				{
					foreach (ResourceItem item2 in resourceFolder.Items)
					{
						if (this.IsItemSupportPacker(item2))
						{
							return true;
						}
					}
				}
			}
			return item is ImageFile;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000580C File Offset: 0x00003A0C
		internal void DeleteItem()
		{
			using (CompositeTask.Run("KeyReleaseEvent", null))
			{
				foreach (PlistInfoItem plistInfoItem in this.PlistInfoModel.SelectedItems)
				{
					ImageFile item = plistInfoItem.ResourceItem as ImageFile;
					this.PlistInfoModel.RemoveItem(item);
					plistInfoItem.Select = false;
				}
				this.PlistInfoModel.SelectedItems.Clear();
				this.PlistInfoModel.Refresh(true);
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000058B8 File Offset: 0x00003AB8
		internal void PackerWidget_DragMotion(object o, DragMotionArgs args)
		{
			object dragData = args.Context.GetDragData();
			if (args.Context.DragProtocol == DragProtocol.Win32Dropfiles || args.Context.GetSourceWidget() == null)
			{
				Gdk.Drag.Status(args.Context, DragAction.Copy, args.Time);
				args.RetVal = true;
				return;
			}
			ResourceInfoDragData resourceInfoDragData = dragData as ResourceInfoDragData;
			if (resourceInfoDragData == null)
			{
				Gdk.Drag.Status(args.Context, (DragAction)0, args.Time);
				args.RetVal = false;
				return;
			}
			foreach (ResourceItem item in resourceInfoDragData.Items)
			{
				if (this.IsItemSupportPacker(item))
				{
					Gdk.Drag.Status(args.Context, DragAction.Copy, args.Time);
					args.RetVal = true;
					return;
				}
			}
			Gdk.Drag.Status(args.Context, (DragAction)0, args.Time);
			args.RetVal = false;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000059B4 File Offset: 0x00003BB4
		protected override bool OnKeyReleaseEvent(EventKey evnt)
		{
			if (evnt.Key == Gdk.Key.Delete || evnt.Key == Gdk.Key.BackSpace)
			{
				this.DeleteItem();
			}
			return base.OnKeyReleaseEvent(evnt);
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x000059E0 File Offset: 0x00003BE0
		private Menu ContextMenu
		{
			get
			{
				if (this.contextMenu == null)
				{
					this.contextMenu = new CommandMenu(Services.CommandService);
					this.deleteItemMenu = MenuCreator.CreateMenuItem(GlobalCommand.DeleteCmd, false, null);
					this.contextMenu.Add(this.deleteItemMenu);
					this.deleteItemMenu.Sensitive = true;
				}
				return this.contextMenu;
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005A3A File Offset: 0x00003C3A
		private void ShowContextMenu(EventButton evnt)
		{
			if (this.PlistInfoModel.SelectedItems.Count == 0)
			{
				return;
			}
			this.ContextMenu.Popdown();
			Services.CommandService.ShowContextMenu(this, evnt, this.ContextMenu, null);
			this.ContextMenu.ShowAll();
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005A78 File Offset: 0x00003C78
		[CommandHandler(CmdEnum.DeleteCmd)]
		[CommandHandler(CmdEnum.DeleteCmd2)]
		private void Delete_Execute()
		{
			this.DeleteItem();
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005A80 File Offset: 0x00003C80
		[CommandUpdateHandler(CmdEnum.DeleteCmd)]
		[CommandUpdateHandler(CmdEnum.DeleteCmd2)]
		private void Delete_CanExecute(CommandInfo info)
		{
			if (this.PlistInfoModel.SelectedItems.Count == 0)
			{
				info.Enabled = false;
				return;
			}
			info.Enabled = true;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00005AA4 File Offset: 0x00003CA4
		public static int GetBinaryNum(int num)
		{
			int i;
			for (i = 32; i < num; i *= 2)
			{
			}
			return i;
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00005ABF File Offset: 0x00003CBF
		// (set) Token: 0x060000DA RID: 218 RVA: 0x00005AD4 File Offset: 0x00003CD4
		public float Zoom
		{
			get
			{
				return (float)this.pictureRender.Scale * 100f;
			}
			set
			{
				this.PlistInfoModel.PictureRenderScale = (this.pictureRender.Scale = (double)(value / 100f));
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005B04 File Offset: 0x00003D04
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.TexturePacker.PackerWidget";
			this.dialog1_VBox = new VBox();
			this.dialog1_VBox.Name = "dialog1_VBox";
			this.dialog1_VBox.BorderWidth = 2U;
			this.hbox1 = new HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			this.vbox6 = new VBox();
			this.vbox6.Name = "vbox6";
			this.vbox6.Spacing = 6;
			this.midScrollWindow = new ScrolledWindow();
			this.midScrollWindow.WidthRequest = 480;
			this.midScrollWindow.HeightRequest = 320;
			this.midScrollWindow.CanFocus = true;
			this.midScrollWindow.Name = "midScrollWindow";
			this.midScrollWindow.ShadowType = ShadowType.In;
			Viewport viewport = new Viewport();
			viewport.ShadowType = ShadowType.None;
			this.eventbox = new EventBox();
			this.eventbox.Name = "eventbox";
			viewport.Add(this.eventbox);
			this.midScrollWindow.Add(viewport);
			this.vbox6.Add(this.midScrollWindow);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox6[this.midScrollWindow];
			boxChild.Position = 0;
			this.hbox1.Add(this.vbox6);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox1[this.vbox6];
			boxChild2.Position = 0;
			this.dialog1_VBox.Add(this.hbox1);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.dialog1_VBox[this.hbox1];
			boxChild3.Position = 0;
			base.Add(this.dialog1_VBox);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Show();
		}

		// Token: 0x0400003B RID: 59
		private PlistInfoItemRender pictureRender;

		// Token: 0x0400003C RID: 60
		private static TargetEntry[] target_table = new TargetEntry[]
		{
			DragTargetType.FileDropTarget,
			DragTargetType.CocoStudioTarget
		};

		// Token: 0x0400003D RID: 61
		private MenuItem deleteItemMenu;

		// Token: 0x0400003E RID: 62
		private Menu contextMenu;

		// Token: 0x0400003F RID: 63
		private VBox dialog1_VBox;

		// Token: 0x04000040 RID: 64
		private HBox hbox1;

		// Token: 0x04000041 RID: 65
		private VBox vbox6;

		// Token: 0x04000042 RID: 66
		private ScrolledWindow midScrollWindow;

		// Token: 0x04000043 RID: 67
		private EventBox eventbox;
	}
}
