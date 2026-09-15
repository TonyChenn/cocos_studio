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
	public class PlistInfoWidget : Bin
	{
		public PlistInfoModel PlistInfoModel { get; private set; }

		public PlistInfoWidget()
		{
			this.Build();
			Gtk.Drag.DestSet(this, DestDefaults.All, PlistInfoWidget.target_table, DragAction.Copy | DragAction.Move | DragAction.Link);
		}

		public void Init(PlistInfoModel model)
		{
			this.PlistInfoModel = model;
			this.InitRender();
			this.InitEvent();
		}

		private void InitRender()
		{
			this.pictureRender = new PlistInfoItemRender(this.PlistInfoModel);
			this.PlistInfoModel.Render = this.pictureRender;
			this.eventbox.Events = EventMask.AllEventsMask;
			this.eventbox.Add(this.pictureRender);
			this.eventbox.ShowAll();
		}

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

		public void CanvasZoom(float delta)
		{
			this.PlistInfoModel.PictureRenderScale = this.PlistInfoModel.PictureRenderScale + (double)delta;
		}

		private void OnPictureRenderReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			if (args.Event.IsContextMenuButton())
			{
				this.ShowContextMenu(args.Event);
			}
		}

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

		protected override bool OnKeyReleaseEvent(EventKey evnt)
		{
			if (evnt.Key == Gdk.Key.Delete || evnt.Key == Gdk.Key.BackSpace)
			{
				this.DeleteItem();
			}
			return base.OnKeyReleaseEvent(evnt);
		}

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

		[CommandHandler(CmdEnum.DeleteCmd)]
		[CommandHandler(CmdEnum.DeleteCmd2)]
		private void Delete_Execute()
		{
			this.DeleteItem();
		}

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

		public static int GetBinaryNum(int num)
		{
			int i;
			for (i = 32; i < num; i *= 2)
			{
			}
			return i;
		}

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

		private PlistInfoItemRender pictureRender;

		private static TargetEntry[] target_table = new TargetEntry[]
		{
			DragTargetType.FileDropTarget,
			DragTargetType.CocoStudioTarget
		};

		private MenuItem deleteItemMenu;

		private Menu contextMenu;

		private VBox dialog1_VBox;

		private HBox hbox1;

		private VBox vbox6;

		private ScrolledWindow midScrollWindow;

		private EventBox eventbox;
	}
}
