using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using CocoStudio.Basic;
using CocoStudio.Core.Commands;
using CocoStudio.Core.Events;
using CocoStudio.Core.ExtensionModel;
using CocoStudio.Core.View;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Components;
using MonoDevelop.Components.Commands;
using MonoDevelop.Components.Docking;
using MonoDevelop.Components.DockNotebook;
using MonoDevelop.Components.DockToolbars;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Codons;
using MonoDevelop.Ide.Gui;
using Xwt.Drawing;

namespace CocoStudio.Core
{
	// Token: 0x0200004D RID: 77
	public class MainWindow : WorkbenchWindow, IWorkbench, IWindowClosed
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0000BA70 File Offset: 0x00009C70
		public IList<string> LayoutList
		{
			get
			{
				return this._LayoutList;
			}
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060002AF RID: 687 RVA: 0x0000BA88 File Offset: 0x00009C88
		// (remove) Token: 0x060002B0 RID: 688 RVA: 0x0000BAC4 File Offset: 0x00009CC4
		public event EventHandler ActiveWorkbenchWindowChanged;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060002B1 RID: 689 RVA: 0x0000BB00 File Offset: 0x00009D00
		// (remove) Token: 0x060002B2 RID: 690 RVA: 0x0000BB3C File Offset: 0x00009D3C
		public event EventHandler<EventArgs> InitializeCompleted;

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000BB78 File Offset: 0x00009D78
		public List<PadCodon> PadContentCollection
		{
			get
			{
				return this.padContentCollection;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000BB90 File Offset: 0x00009D90
		public List<IViewContent> InternalViewContentCollection
		{
			get
			{
				return this.viewContentCollection.Cast<IViewContent>().ToList<IViewContent>();
			}
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060002B5 RID: 693 RVA: 0x0000BBB4 File Offset: 0x00009DB4
		// (remove) Token: 0x060002B6 RID: 694 RVA: 0x0000BBF0 File Offset: 0x00009DF0
		public event EventHandler LayoutReset;

		// Token: 0x060002B7 RID: 695 RVA: 0x0000BC2C File Offset: 0x00009E2C
		public MainWindow(string title)
		{
			base.Title = title;
			this.MainThreadId = Thread.CurrentThread.ManagedThreadId;
			base.WidthRequest = this.normalBounds.Width;
			base.HeightRequest = this.normalBounds.Height;
			base.Maximize();
			if (Platform.IsWindows)
			{
				base.DeleteEvent += this.OnClosing;
			}
			if (Platform.IsMac)
			{
				base.DeleteEvent += this.OnHiding;
			}
			Services.MainWindow = this;
			this.SetAppIcons();
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000BD38 File Offset: 0x00009F38
		public IDocumentWindow ActiveWorkbenchWindow
		{
			get
			{
				IDocumentWindow result;
				if (this.tabControl == null || this.tabControl.CurrentTabIndex < 0 || this.tabControl.CurrentTabIndex >= this.tabControl.TabCount)
				{
					result = null;
				}
				else
				{
					result = (IDocumentWindow)this.tabControl.CurrentTab.Content;
				}
				return result;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000BD9C File Offset: 0x00009F9C
		IWorkbenchWindow IWorkbench.ActiveWorkbenchWindow
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0000BDB0 File Offset: 0x00009FB0
		public DockFrame DockFrame
		{
			get
			{
				return this.dock;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0000BDC8 File Offset: 0x00009FC8
		public DockNotebook DockNotebook
		{
			get
			{
				return this.tabControl;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002BC RID: 700 RVA: 0x0000BDE0 File Offset: 0x00009FE0
		// (set) Token: 0x060002BD RID: 701 RVA: 0x0000BDF8 File Offset: 0x00009FF8
		public bool FullScreen
		{
			get
			{
				return DesktopService.GetIsFullscreen(this);
			}
			set
			{
				DesktopService.SetIsFullscreen(this, value);
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0000BE04 File Offset: 0x0000A004
		// (set) Token: 0x060002BF RID: 703 RVA: 0x0000BE8C File Offset: 0x0000A08C
		public string CurrentLayout
		{
			get
			{
				string result;
				if (this.dock != null && this.dock.CurrentLayout != null)
				{
					string text = this.dock.CurrentLayout;
					text = text.Substring(text.IndexOf(".") + 1);
					if (text.EndsWith("[FullViewMode]"))
					{
						result = text.Substring(0, text.Length - "[FullViewMode]".Length);
					}
					else
					{
						result = text;
					}
				}
				else
				{
					result = "";
				}
				return result;
			}
			set
			{
				string currentLayout = this.dock.CurrentLayout;
				this.InitializeLayout(value, false);
				DockToolbarFrame dockToolbarFrame = this.toolbarFrame;
				this.dock.CurrentLayout = value;
				dockToolbarFrame.CurrentLayout = value;
				this.DestroyFullViewLayouts(currentLayout);
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000BED3 File Offset: 0x0000A0D3
		private void SetAppIcons()
		{
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000BED6 File Offset: 0x0000A0D6
		protected void OnHiding(object o, DeleteEventArgs e)
		{
			e.RetVal = true;
			base.Visible = false;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000BEF0 File Offset: 0x0000A0F0
		protected void OnClosing(object o, DeleteEventArgs e)
		{
			if (this.Closing != null)
			{
				CancelEventArgs cancelEventArgs = new CancelEventArgs(false);
				this.Closing(this, cancelEventArgs);
				if (cancelEventArgs.Cancel)
				{
					e.RetVal = true;
					return;
				}
			}
			if (this.Close())
			{
				Application.Quit();
			}
			else
			{
				e.RetVal = true;
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000BF68 File Offset: 0x0000A168
		public bool Quit()
		{
			if (this.Closing != null)
			{
				CancelEventArgs cancelEventArgs = new CancelEventArgs(false);
				this.Closing(this, cancelEventArgs);
				if (cancelEventArgs.Cancel)
				{
					return true;
				}
			}
			bool result;
			if (this.Close())
			{
				Application.Quit();
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000BFCC File Offset: 0x0000A1CC
		protected void OnClosed(EventArgs e)
		{
			foreach (string text in this.dock.Layouts)
			{
				if (text.EndsWith("[FullViewMode]"))
				{
					this.dock.DeleteLayout(text);
				}
			}
			try
			{
				this.dock.SaveLayouts(MainWindow.layoutFilePath);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Error while saving layout.", exception);
			}
			this.UninstallMenuBar();
			base.Remove(this.rootWidget);
			foreach (PadCodon padCodon in this.PadContentCollection)
			{
				if (padCodon.Initialized)
				{
					padCodon.PadContent.Dispose();
				}
			}
			this.rootWidget.Destroy();
			this.Destroy();
			if (this.Closed != null)
			{
				this.Closed(this, new EventArgs());
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000C10C File Offset: 0x0000A30C
		public bool Close()
		{
			bool flag = false;
			foreach (IViewContent viewContent in this.viewContentCollection)
			{
				if (viewContent.IsDirty)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				base.Present();
				ButtonText btnText = new ButtonText(LanguageInfo.Dialog_ButtonYes, LanguageInfo.Dialog_ButtonNo, LanguageInfo.Dialog_ButtonCancel, false, false, false);
				MessageBoxResult messageBoxResult = MessageBox.Show(LanguageInfo.MessageBox_Content57, btnText, MessageBoxImage.Other, null, EnumMainButton.Yes, null);
				if (messageBoxResult == MessageBoxResult.Cancel)
				{
					return false;
				}
				if (messageBoxResult != MessageBoxResult.No)
				{
					Services.Workbench.SaveAll();
				}
			}
			Services.Workspace.CloseWorkspaceItem(Services.ProjectOperations.CurrentSelectedSolution);
			this.CloseAllViews();
			this.OnClosed(null);
			return true;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000C214 File Offset: 0x0000A414
		public void LockActiveWindowChangeEvent()
		{
			this.activeWindowChangeLock++;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000C225 File Offset: 0x0000A425
		public void UnlockActiveWindowChangeEvent()
		{
			this.activeWindowChangeLock--;
			this.OnActiveWindowChanged(null, null);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000C240 File Offset: 0x0000A440
		internal void OnActiveWindowChanged(object sender, EventArgs e)
		{
			if (this.activeWindowChangeLock <= 0)
			{
				if (this.lastActive != this.ActiveWorkbenchWindow)
				{
					if (this.lastActive != null)
					{
						((DocumentWindow)this.lastActive).OnDeactivated();
					}
					this.lastActive = this.ActiveWorkbenchWindow;
					if (!this.closeAll && this.ActiveWorkbenchWindow != null)
					{
						((DocumentWindow)this.ActiveWorkbenchWindow).OnActivated();
					}
					if (this.lastActive != null)
					{
						Services.ProjectOperations.CurrentSelectedProject = this.lastActive.ContentExtend.File;
					}
					else
					{
						Services.ProjectOperations.CurrentSelectedProject = null;
					}
					if (!this.closeAll && this.ActiveWorkbenchWindowChanged != null)
					{
						this.ActiveWorkbenchWindowChanged(this, e);
					}
					if (!this.closeAll && this.ActiveWorkbenchWindow != null)
					{
						((DocumentWindow)this.ActiveWorkbenchWindow).AfterActivated();
					}
				}
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000C35C File Offset: 0x0000A55C
		public void Initialize()
		{
			Services.Initialize();
			ImageService.Initialize();
			this.InitializeLayout();
			this.CreateMenuBar();
			this.InstallMenuBar();
			Services.ProjectOperations.CurrentSelectedSolutionChanged += this.HandleCurrentSelectedSolutionChanged;
			if (this.InitializeCompleted != null)
			{
				this.InitializeCompleted(this, new EventArgs());
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000C3C3 File Offset: 0x0000A5C3
		private void InitializeLayout()
		{
			this.CreateComponents();
			this.initializing = true;
			AddinManager.AddExtensionNodeHandler("/CocoStudio/Ide/Pads", new ExtensionNodeEventHandler(this.OnExtensionChanged));
			this.initializing = false;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000C3F4 File Offset: 0x0000A5F4
		private void InitializeLayout(string name, bool forceRefresh = false)
		{
			if (!this.LayoutList.Contains(name))
			{
				this.LayoutList.Add(name);
			}
			if (!this.dock.Layouts.Contains(name) || forceRefresh)
			{
				this.dock.CreateLayout(name, true);
				this.dock.CurrentLayout = name;
				this.documentDockItem.Visible = true;
				HashSet<string> hashSet = new HashSet<string>();
				foreach (PadCodon padCodon in this.padContentCollection)
				{
					if (!hashSet.Contains(padCodon.PadId) && padCodon.DefaultLayouts != null && (padCodon.DefaultLayouts.Contains("DefaultLayout") || padCodon.DefaultLayouts.Contains("*")))
					{
						DockItem dockItem = this.dock.GetItem(padCodon.PadId);
						if (dockItem != null)
						{
							dockItem.Visible = true;
							if (!string.IsNullOrEmpty(padCodon.DefaultPlacement))
							{
								dockItem.SetDockLocation(this.ToDockLocation(padCodon.DefaultPlacement));
							}
							dockItem.Status = padCodon.DefaultStatus;
							hashSet.Add(padCodon.PadId);
						}
					}
				}
				foreach (DockItem dockItem in this.dock.GetItems())
				{
					if (!hashSet.Contains(dockItem.Id) && (dockItem.Behavior & DockItemBehavior.Sticky) == DockItemBehavior.Normal && dockItem != this.documentDockItem)
					{
						dockItem.Visible = false;
					}
				}
			}
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000C5F0 File Offset: 0x0000A7F0
		private void CreateComponents()
		{
			this.fullViewVBox = new VBox(false, 0);
			this.rootWidget = this.fullViewVBox;
			base.Realize();
			this.toolbar = MainWindowPartFactory.CreateMainToolbarWidget();
			HBox hbox = new HBox(false, 0);
			this.fullViewVBox.PackStart(hbox, false, false, 0U);
			this.toolbarFrame = new DockToolbarFrame();
			this.fullViewVBox.PackStart(this.toolbarFrame, true, true, 0U);
			this.dock = new DockFrame();
			this.dock.DefaultItemWidth = 230;
			this.dock.CompactGuiLevel = 2;
			this.toolbarFrame.ModifyBg(StateType.Normal, new Gdk.Color(byte.MaxValue, 0, 0));
			this.toolbarFrame.AddContent(this.dock);
			this.tabControl = new DocumentNotebook(this);
			DockNotebook.ActiveNotebookChanged += delegate(object param0, EventArgs param1)
			{
				this.OnActiveWindowChanged(null, null);
			};
			base.Add(this.fullViewVBox);
			this.fullViewVBox.ShowAll();
			HBox hbox2 = new HBox(false, 0);
			this.bottomBar = MainWindowPartFactory.CreateMainStatus();
			if (this.bottomBar != null)
			{
				hbox2.PackStart(this.bottomBar, true, true, 0U);
			}
			this.fullViewVBox.PackEnd(hbox2, false, false, 0U);
			hbox2.ShowAll();
			hbox.PackStart(this.toolbar, true, true, 0U);
			this.tabControl.InitSize();
			int barHeight = this.tabControl.BarHeight;
			this.documentDockItem = this.dock.AddItem("Documents");
			this.documentDockItem.Behavior = DockItemBehavior.Locked;
			this.documentDockItem.Expand = true;
			this.documentDockItem.DrawFrame = false;
			this.documentDockItem.Label = "Documents";
			this.documentDockItem.Content = this.tabControl;
			this.documentDockItem.ContentVisibleChanged += this.DockTabControlVisibleChanged;
			DockVisualStyle dockVisualStyle = new DockVisualStyle();
			dockVisualStyle.PadTitleLabelColor = new Gdk.Color?(Styles.PadLabelColor);
			dockVisualStyle.PadBackgroundColor = new Gdk.Color?(Styles.PadBackground);
			dockVisualStyle.InactivePadBackgroundColor = new Gdk.Color?(Styles.InactivePadBackground);
			dockVisualStyle.PadTitleHeight = new int?(barHeight);
			this.dock.DefaultVisualStyle = dockVisualStyle;
			dockVisualStyle = new DockVisualStyle();
			dockVisualStyle.PadTitleLabelColor = new Gdk.Color?(Styles.PadLabelColor);
			dockVisualStyle.PadTitleHeight = new int?(barHeight);
			dockVisualStyle.ShowPadTitleIcon = new bool?(false);
			dockVisualStyle.UppercaseTitles = new bool?(false);
			dockVisualStyle.ExpandedTabs = new bool?(true);
			dockVisualStyle.PadBackgroundColor = new Gdk.Color?(Styles.BrowserPadBackground);
			dockVisualStyle.InactivePadBackgroundColor = new Gdk.Color?(Styles.InactiveBrowserPadBackground);
			dockVisualStyle.TreeBackgroundColor = new Gdk.Color?(Styles.BrowserPadBackground);
			this.dock.SetDockItemStyle("ProjectPad", dockVisualStyle);
			this.dock.SetDockItemStyle("ClassPad", dockVisualStyle);
			this.dock.SetRegionStyle("Documents/Left", dockVisualStyle);
			this.dock.SetRegionStyle("Documents/Right", dockVisualStyle);
			dockVisualStyle = new DockVisualStyle();
			dockVisualStyle.SingleColumnMode = new bool?(true);
			this.dock.SetRegionStyle("Documents/Left;Documents/Right", dockVisualStyle);
			this.dock.SetDockItemStyle("Documents", dockVisualStyle);
			DockItem dockItem = this.dock.AddItem("__left");
			dockItem.DefaultLocation = "Documents/Left";
			dockItem.Behavior = DockItemBehavior.Locked;
			dockItem.DefaultVisible = false;
			dockItem = this.dock.AddItem("__bottom");
			dockItem.DefaultLocation = "Documents/Bottom";
			dockItem.Behavior = DockItemBehavior.Locked;
			dockItem.DefaultVisible = false;
			dockItem = this.dock.AddItem("__right");
			dockItem.DefaultLocation = "Documents/Right";
			dockItem.Behavior = DockItemBehavior.Locked;
			dockItem.DefaultVisible = false;
			dockItem = this.dock.AddItem("__top");
			dockItem.DefaultLocation = "Documents/Top";
			dockItem.Behavior = DockItemBehavior.Locked;
			dockItem.DefaultVisible = false;
			ExtensionNodeList extensionNodes = AddinManager.GetExtensionNodes("/CocoStudio/Ide/Pads");
			foreach (object obj in extensionNodes)
			{
				ExtensionNode node = (ExtensionNode)obj;
				this.ShowPadNode(node);
			}
			this.LoadLayoutFromFile(MainWindow.layoutFilePath);
			this.InitializeLayout("DefaultLayout", true);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000CA78 File Offset: 0x0000AC78
		private void DockTabControlVisibleChanged(object sender, EventArgs e)
		{
			this.FocusActiveDocumentWindow();
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000CA84 File Offset: 0x0000AC84
		private void FocusActiveDocumentWindow()
		{
			if (this.documentDockItem.ContentVisible)
			{
				IDocumentWindow activeWorkbenchWindow = this.ActiveWorkbenchWindow;
				if (activeWorkbenchWindow != null)
				{
					this.LockActiveWindowChangeEvent();
					DockNotebookTab currentTab = this.tabControl.CurrentTab;
					this.tabControl.CurrentTab = null;
					this.tabControl.CurrentTab = currentTab;
					DocumentWindow documentWindow = this.ActiveWorkbenchWindow as DocumentWindow;
					if (documentWindow != null)
					{
						IMainRender mainRender = documentWindow.ViewContent as IMainRender;
						if (mainRender != null)
						{
							mainRender.SwitchView();
						}
					}
					this.UnlockActiveWindowChangeEvent();
				}
			}
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000CB21 File Offset: 0x0000AD21
		private void CreateMenuBar()
		{
			this.topMenu = MainWindowPartFactory.CreateMainMenu();
			Services.CommandService.SetMultiRootWindow(this);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000CB3C File Offset: 0x0000AD3C
		private void InstallMenuBar()
		{
			if (Platform.IsWindows)
			{
				if (this.topMenu != null)
				{
					((VBox)this.rootWidget).PackStart(this.topMenu, false, false, 0U);
					((Box.BoxChild)this.rootWidget[this.topMenu]).Position = 0;
					this.topMenu.ShowAll();
				}
			}
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000CBB0 File Offset: 0x0000ADB0
		private void UninstallMenuBar()
		{
			if (this.topMenu != null)
			{
				this.rootWidget.Remove(this.topMenu);
				this.topMenu.Destroy();
				this.topMenu = null;
			}
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000CBF8 File Offset: 0x0000ADF8
		private void OnExtensionChanged(object s, ExtensionNodeEventArgs args)
		{
			if (!this.initializing)
			{
				if (args.Change == ExtensionChange.Add)
				{
					this.ShowPadNode(args.ExtensionNode);
				}
				else
				{
					this.RemovePadNode(args.ExtensionNode);
				}
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000CC45 File Offset: 0x0000AE45
		private void OnLayoutsExtensionChanged(object s, ExtensionNodeEventArgs args)
		{
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000CC48 File Offset: 0x0000AE48
		public void CloseContent(IViewContentExtend content)
		{
			if (this.viewContentCollection.Contains(content))
			{
				this.viewContentCollection.Remove(content);
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000CC78 File Offset: 0x0000AE78
		public void CloseAllViews()
		{
			try
			{
				this.closeAll = true;
				List<IViewContent> list = new List<IViewContent>(this.viewContentCollection);
				foreach (IViewContent viewContent in list)
				{
					IWorkbenchWindow workbenchWindow = viewContent.WorkbenchWindow;
					if (workbenchWindow != null)
					{
						workbenchWindow.CloseWindow(true);
					}
				}
			}
			finally
			{
				this.closeAll = false;
				this.OnActiveWindowChanged(null, null);
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000CD1C File Offset: 0x0000AF1C
		public virtual void ShowView(IViewContentExtend content, bool bringToFront)
		{
			if (this.viewContentCollection.Contains(content))
			{
				this.SelectView(content);
			}
			else
			{
				this.viewContentCollection.Add(content);
				Xwt.Drawing.Image image = null;
				DockNotebookTab dockNotebookTab = this.tabControl.InsertTab(-1);
				DocumentWindow documentWindow = new DocumentWindow(this, content, this.tabControl, dockNotebookTab);
				documentWindow.Closed += this.CloseWindowEvent;
				documentWindow.Show();
				dockNotebookTab.Content = documentWindow;
				if (image != null)
				{
					dockNotebookTab.Icon = image;
				}
				if (bringToFront)
				{
					documentWindow.SelectWindow();
				}
				documentWindow.ShowAll();
				this.OnActiveWindowChanged(null, null);
			}
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000CDC4 File Offset: 0x0000AFC4
		private void SelectView(IViewContent content)
		{
			if (content is ViewContent)
			{
				(content as ViewContent).WorkbenchWindow.SelectWindow();
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000CDF4 File Offset: 0x0000AFF4
		private void ShowPadNode(ExtensionNode node)
		{
			if (node is PadCodon)
			{
				PadCodon padCodon = (PadCodon)node;
				this.AddPad(padCodon, padCodon.DefaultPlacement, padCodon.DefaultStatus);
			}
			else if (node is CategoryNode)
			{
				foreach (object obj in node.ChildNodes)
				{
					ExtensionNode node2 = (ExtensionNode)obj;
					this.ShowPadNode(node2);
				}
			}
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000CEA0 File Offset: 0x0000B0A0
		private void RemovePadNode(ExtensionNode node)
		{
			if (node is PadCodon)
			{
				this.RemovePad((PadCodon)node);
			}
			else if (node is CategoryNode)
			{
				foreach (object obj in node.ChildNodes)
				{
					ExtensionNode node2 = (ExtensionNode)obj;
					this.RemovePadNode(node2);
				}
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000CF38 File Offset: 0x0000B138
		public void ShowPad(PadCodon content)
		{
			this.AddPad(content, true);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000CF44 File Offset: 0x0000B144
		public void AddPad(PadCodon content)
		{
			this.AddPad(content, false);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000CF50 File Offset: 0x0000B150
		private void RegisterPad(PadCodon content)
		{
			this.padContentCollection.Add(content);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000CF60 File Offset: 0x0000B160
		private void AddPad(PadCodon content, bool show)
		{
			DockItem dockItem = this.GetDockItem(content);
			if (this.padContentCollection.Contains(content))
			{
				if (show && dockItem != null)
				{
					dockItem.Visible = true;
				}
			}
			else
			{
				this.RegisterPad(content);
				if (dockItem != null)
				{
					if (show)
					{
						dockItem.Visible = true;
					}
				}
				else
				{
					this.AddPad(content, content.DefaultPlacement, content.DefaultStatus);
				}
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000CFE0 File Offset: 0x0000B1E0
		public void RemovePad(PadCodon codon)
		{
			if (codon.HasId)
			{
				Command command = Services.CommandService.GetCommand(codon.Id);
				if (command != null)
				{
					Services.CommandService.UnregisterCommand(command);
				}
			}
			DockItem dockItem = this.GetDockItem(codon);
			this.padContentCollection.Remove(codon);
			PadWindow padWindow = (PadWindow)this.GetPadWindow(codon);
			if (padWindow != null)
			{
				padWindow.NotifyDestroyed();
				this.padCodons.Remove(padWindow);
			}
			if (dockItem != null)
			{
				this.dock.RemoveItem(dockItem);
			}
			this.padWindows.Remove(codon);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000D084 File Offset: 0x0000B284
		public void BringToFront(PadCodon content)
		{
			this.BringToFront(content, false);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000D090 File Offset: 0x0000B290
		public virtual void BringToFront(PadCodon content, bool giveFocus)
		{
			if (!this.IsVisible(content))
			{
				this.ShowPad(content);
			}
			this.ActivatePad(content, giveFocus);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000D0BA File Offset: 0x0000B2BA
		public void ResetDefaultLayout()
		{
			this.CurrentLayout = "DefaultLayout";
			this.dock.DeleteLayout("CustomLayout");
			this.CurrentLayout = "CustomLayout";
			this.FocusActiveDocumentWindow();
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000D0F0 File Offset: 0x0000B2F0
		private void LoadLayoutFromFile(string filePath)
		{
			try
			{
				if (File.Exists(filePath))
				{
					this.dock.LoadLayouts(filePath);
					foreach (string text in this.dock.Layouts)
					{
						if (!this.LayoutList.Contains(text) && !text.EndsWith("[FullViewMode]"))
						{
							this.LayoutList.Add(text);
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error(ex.ToString());
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000D1A0 File Offset: 0x0000B3A0
		private void HandleCurrentSelectedSolutionChanged(object sender, SolutionEventArgs e)
		{
			this.SetDefaultTitle();
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000D1AC File Offset: 0x0000B3AC
		private void SetWorkbenchTitle()
		{
			try
			{
				IDocumentWindow activeWorkbenchWindow = this.ActiveWorkbenchWindow;
				if (activeWorkbenchWindow != null)
				{
					if (activeWorkbenchWindow.ViewContent.IsUntitled)
					{
						this.SetDefaultTitle();
					}
					else
					{
						string text = string.Empty;
						if (activeWorkbenchWindow.ViewContent.IsDirty)
						{
							text = "*";
						}
						if (activeWorkbenchWindow.ViewContent.Project != null)
						{
							base.Title = string.Concat(new string[]
							{
								activeWorkbenchWindow.ViewContent.Project.Name,
								" - ",
								activeWorkbenchWindow.ViewContent.PathRelativeToProject,
								text,
								" - ",
								BrandingService.ApplicationName
							});
						}
						else
						{
							base.Title = activeWorkbenchWindow.ViewContent.ContentName + text + " - " + BrandingService.ApplicationName;
						}
					}
				}
				else
				{
					this.SetDefaultTitle();
					if (this.IsInFullViewMode)
					{
						this.ToggleFullViewMode();
					}
				}
			}
			catch (Exception)
			{
				this.SetDefaultTitle();
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000D2E8 File Offset: 0x0000B4E8
		private void SetDefaultTitle()
		{
			if (Services.ProjectOperations.CurrentSelectedSolution != null)
			{
				base.Title = Services.ProjectOperations.CurrentSelectedSolution.Name + " - Cocos Studio";
			}
			else
			{
				base.Title = "Cocos Studio";
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000D33C File Offset: 0x0000B53C
		public Properties GetStoredMemento(IViewContent content)
		{
			if (content != null && content.ContentName != null)
			{
				string text = UserProfile.Current.CacheDir.Combine(new string[]
				{
					"temp"
				});
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				string arg = content.ContentName.Substring(3).Replace('/', '.').Replace('\\', '.').Replace(System.IO.Path.DirectorySeparatorChar, '.');
				string text2 = text + System.IO.Path.DirectorySeparatorChar + arg;
				if (FileService.IsValidPath(text2) && File.Exists(text2))
				{
					return Properties.Load(text2);
				}
			}
			return null;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000D40D File Offset: 0x0000B60D
		internal void ShowPopup(DockNotebook notebook, int tabIndex, EventButton evt)
		{
			this.tabControl.CurrentTabIndex = tabIndex;
			Services.CommandService.ShowContextMenu(this.tabControl, evt, FileTabCommand.TabPopupMenu, null);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000D438 File Offset: 0x0000B638
		internal void OnTabsReordered(Widget widget, int oldPlacement, int newPlacement)
		{
			if (this.viewContentCollection != null)
			{
				IViewContentExtend item = this.viewContentCollection[oldPlacement];
				this.viewContentCollection.RemoveAt(oldPlacement);
				this.viewContentCollection.Insert(newPlacement, item);
				Services.Workbench.RecoderDocuments(oldPlacement, newPlacement);
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000D490 File Offset: 0x0000B690
		private bool IsInFullViewMode
		{
			get
			{
				return this.dock.CurrentLayout.EndsWith("[FullViewMode]");
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000D4B8 File Offset: 0x0000B6B8
		private void DestroyFullViewLayouts(string oldLayout)
		{
			if (oldLayout != null && oldLayout.EndsWith("[FullViewMode]"))
			{
				this.dock.DeleteLayout(oldLayout);
				this.toolbarFrame.DeleteLayout(oldLayout);
			}
			if (this.LayoutReset != null)
			{
				this.LayoutReset(this, null);
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000D518 File Offset: 0x0000B718
		public void ToggleFullViewMode()
		{
			if (this.IsInFullViewMode)
			{
				string currentLayout = this.dock.CurrentLayout;
				this.toolbarFrame.CurrentLayout = (this.dock.CurrentLayout = this.CurrentLayout);
				this.DestroyFullViewLayouts(currentLayout);
			}
			else
			{
				string text = this.CurrentLayout + "[FullViewMode]";
				if (!this.dock.HasLayout(text))
				{
					this.dock.CreateLayout(text, true);
				}
				this.toolbarFrame.CurrentLayout = (this.dock.CurrentLayout = text);
				foreach (DockItem dockItem in this.dock.GetItems())
				{
					if (dockItem.Behavior != DockItemBehavior.Locked && dockItem.Visible)
					{
						dockItem.Status = DockItemStatus.AutoHide;
					}
				}
				foreach (DockToolbar dockToolbar in this.toolbarFrame.Toolbars)
				{
					dockToolbar.Status = new DockToolbarStatus(dockToolbar.Id, false, dockToolbar.Position);
				}
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000D6A4 File Offset: 0x0000B8A4
		public void SetFullViewMode(bool IsFull)
		{
			if (IsFull)
			{
				string currentLayout = this.dock.CurrentLayout;
				this.toolbarFrame.CurrentLayout = (this.dock.CurrentLayout = this.CurrentLayout);
				this.DestroyFullViewLayouts(currentLayout);
			}
			else
			{
				string text = this.CurrentLayout + "[FullViewMode]";
				if (!this.dock.HasLayout(text))
				{
					this.dock.CreateLayout(text, true);
				}
				this.toolbarFrame.CurrentLayout = (this.dock.CurrentLayout = text);
				foreach (DockItem dockItem in this.dock.GetItems())
				{
					if (dockItem.Behavior != DockItemBehavior.Locked && dockItem.Visible)
					{
						dockItem.Status = DockItemStatus.AutoHide;
					}
				}
				foreach (DockToolbar dockToolbar in this.toolbarFrame.Toolbars)
				{
					dockToolbar.Status = new DockToolbarStatus(dockToolbar.Id, false, dockToolbar.Position);
				}
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000D82C File Offset: 0x0000BA2C
		private bool SelectLastActiveWindow(IDocumentWindow cur)
		{
			bool result;
			if (this.lastActiveWindows.Count == 0)
			{
				result = false;
			}
			else
			{
				IDocumentWindow value;
				do
				{
					value = this.lastActiveWindows.Last.Value;
					this.lastActiveWindows.RemoveLast();
				}
				while (this.lastActiveWindows.Count > 0 && (value == cur || value == null || (value != null && value.ViewContent == null)));
				if (value != null && value != cur)
				{
					value.SelectWindow();
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000D8C8 File Offset: 0x0000BAC8
		private void CloseWindowEvent(object sender, WorkbenchWindowEventArgs e)
		{
			DocumentWindow documentWindow = (DocumentWindow)sender;
			this.lastActiveWindows.Remove(documentWindow);
			if (documentWindow.ViewContent != null)
			{
				this.CloseContent(documentWindow.ContentExtend);
				if (e.WasActive && !this.SelectLastActiveWindow(documentWindow))
				{
					this.OnActiveWindowChanged(this, null);
				}
			}
			this.lastActiveWindows.Remove(documentWindow);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000D933 File Offset: 0x0000BB33
		internal void CloseClicked(object o, TabEventArgs e)
		{
			this.CloseView(((DocumentWindow)e.Tab.Content).ContentExtend);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000D954 File Offset: 0x0000BB54
		internal void CloseView(IViewContentExtend viewContent)
		{
			if (this.viewContentCollection.Contains(viewContent))
			{
				if (viewContent.WorkbenchWindow.CloseWindow(false))
				{
					this.CloseContent(viewContent);
				}
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000D994 File Offset: 0x0000BB94
		public void RemoveTab(DockNotebook tabControl, int pageNum, bool animate)
		{
			try
			{
				this.LockActiveWindowChangeEvent();
				IDocumentWindow activeWorkbenchWindow = this.ActiveWorkbenchWindow;
				if (tabControl.Tabs.Count > pageNum)
				{
					tabControl.RemoveTab(pageNum, animate);
				}
			}
			finally
			{
				this.UnlockActiveWindowChangeEvent();
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000D9F0 File Offset: 0x0000BBF0
		internal void ReorderTab(int oldPlacement, int newPlacement)
		{
			DockNotebookTab tab = this.tabControl.GetTab(oldPlacement);
			DockNotebookTab tab2 = this.tabControl.GetTab(newPlacement);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000DA18 File Offset: 0x0000BC18
		public IPadWindow GetPadWindow(PadCodon content)
		{
			IPadWindow result;
			this.padWindows.TryGetValue(content, out result);
			return result;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000DA3C File Offset: 0x0000BC3C
		public bool IsVisible(PadCodon padContent)
		{
			DockItem dockItem = this.GetDockItem(padContent);
			return dockItem != null && dockItem.Visible;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000DA68 File Offset: 0x0000BC68
		public bool IsContentVisible(PadCodon padContent)
		{
			DockItem dockItem = this.GetDockItem(padContent);
			return dockItem != null && dockItem.ContentVisible;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000DA94 File Offset: 0x0000BC94
		public void HidePad(PadCodon padContent)
		{
			DockItem dockItem = this.GetDockItem(padContent);
			if (dockItem != null)
			{
				dockItem.Visible = false;
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000DABC File Offset: 0x0000BCBC
		public void ActivatePad(PadCodon padContent, bool giveFocus)
		{
			DockItem dockItem = this.GetDockItem(padContent);
			if (dockItem != null)
			{
				dockItem.Present(giveFocus);
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000DAE4 File Offset: 0x0000BCE4
		public bool IsSticky(PadCodon padContent)
		{
			DockItem dockItem = this.GetDockItem(padContent);
			return dockItem != null && (dockItem.Behavior & DockItemBehavior.Sticky) != DockItemBehavior.Normal;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000DB14 File Offset: 0x0000BD14
		public void SetSticky(PadCodon padContent, bool sticky)
		{
			DockItem dockItem = this.GetDockItem(padContent);
			if (dockItem != null)
			{
				if (sticky)
				{
					dockItem.Behavior |= DockItemBehavior.Sticky;
				}
				else
				{
					dockItem.Behavior &= ~DockItemBehavior.Sticky;
				}
			}
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000DB60 File Offset: 0x0000BD60
		internal DockItem GetDockItem(PadCodon content)
		{
			DockItem result;
			if (this.padContentCollection.Contains(content))
			{
				DockItem item = this.dock.GetItem(content.PadId);
				result = item;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000DBA0 File Offset: 0x0000BDA0
		private void CreatePadContent(bool force, PadCodon padCodon, PadWindow window, DockItem item)
		{
			if (force || item.Content == null)
			{
				IPadContent padContent = padCodon.InitializePadContent(window);
				Widget child;
				if (padContent is Widget)
				{
					child = padContent.Control;
				}
				else
				{
					PadCommandRouterContainer padCommandRouterContainer = new PadCommandRouterContainer(window, padContent.Control, padContent, true);
					padCommandRouterContainer.Show();
					child = padCommandRouterContainer;
				}
				PadCommandRouterContainer padCommandRouterContainer2 = new PadCommandRouterContainer(window, child, this.toolbarFrame, false);
				padCommandRouterContainer2.Show();
				item.Content = padCommandRouterContainer2;
			}
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000DC28 File Offset: 0x0000BE28
		private string ToDockLocation(string loc)
		{
			string text = "";
			foreach (string text2 in loc.Split(new char[]
			{
				' '
			}))
			{
				if (!string.IsNullOrEmpty(text2))
				{
					if (text.Length > 0)
					{
						text += ";";
					}
					if (text2.IndexOf('/') == -1)
					{
						text = text + "__" + text2.ToLower() + "/CenterBefore";
					}
					else
					{
						text += text2;
					}
				}
			}
			return text;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000DD80 File Offset: 0x0000BF80
		private void AddPad(PadCodon padCodon, string placement, DockItemStatus defaultStatus)
		{
			PadWindow window = new PadWindow(this, padCodon);
			window.Icon = padCodon.Icon;
			this.padWindows[padCodon] = window;
			this.padCodons[window] = padCodon;
			window.StatusChanged += this.UpdatePad;
			string defaultLocation = this.ToDockLocation(placement);
			DockItem item = this.dock.AddItem(padCodon.PadId);
			item.Label = LanguageOption.GetValueBykey(padCodon.Label);
			item.DefaultLocation = defaultLocation;
			item.DefaultVisible = false;
			item.DefaultStatus = defaultStatus;
			item.DockLabelProvider = padCodon;
			window.Item = item;
			if (padCodon.Initialized)
			{
				this.CreatePadContent(true, padCodon, window, item);
			}
			else
			{
				item.ContentRequired += delegate(object param0, EventArgs param1)
				{
					this.CreatePadContent(false, padCodon, window, item);
				};
			}
			item.VisibleChanged += delegate(object param0, EventArgs param1)
			{
				if (item.Visible)
				{
					window.NotifyShown();
				}
				else
				{
					window.NotifyHidden();
				}
			};
			item.ContentVisibleChanged += delegate(object param0, EventArgs param1)
			{
				if (item.ContentVisible)
				{
					window.NotifyContentShown();
				}
				else
				{
					window.NotifyContentHidden();
				}
			};
			if (!this.padContentCollection.Contains(padCodon))
			{
				this.padContentCollection.Add(padCodon);
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000DF4C File Offset: 0x0000C14C
		private void UpdatePad(object source, EventArgs args)
		{
			IPadWindow padWindow = (IPadWindow)source;
			if (this.padCodons.ContainsKey(padWindow))
			{
				PadCodon padCodon = this.padCodons[padWindow];
				DockItem dockItem = this.GetDockItem(padCodon);
				if (dockItem != null)
				{
					string text = padWindow.Title;
					if (string.IsNullOrEmpty(text))
					{
						text = padCodon.Label;
					}
					if (padWindow.HasErrors && !padWindow.ContentVisible)
					{
						text = "<span foreground='red'>" + text + "</span>";
					}
					else if (padWindow.HasNewData && !padWindow.ContentVisible)
					{
						text = "<b>" + text + "</b>";
					}
					dockItem.Label = text;
					dockItem.Icon = ImageService.GetIcon(padWindow.Icon).WithSize(IconSize.Menu);
				}
			}
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060002FF RID: 767 RVA: 0x0000E034 File Offset: 0x0000C234
		// (remove) Token: 0x06000300 RID: 768 RVA: 0x0000E070 File Offset: 0x0000C270
		public event EventHandler<CancelEventArgs> Closing;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000301 RID: 769 RVA: 0x0000E0AC File Offset: 0x0000C2AC
		// (remove) Token: 0x06000302 RID: 770 RVA: 0x0000E0E8 File Offset: 0x0000C2E8
		public event EventHandler<EventArgs> Closed;

		// Token: 0x04000145 RID: 325
		public const string DefaultLayoutID = "DefaultLayout";

		// Token: 0x04000146 RID: 326
		public const string CustomLayoutID = "CustomLayout";

		// Token: 0x04000147 RID: 327
		private const string fullViewModeTag = "[FullViewMode]";

		// Token: 0x04000148 RID: 328
		private const int MinimumWidth = 1000;

		// Token: 0x04000149 RID: 329
		private const int MinimumHeight = 600;

		// Token: 0x0400014A RID: 330
		private static string layoutFilePath = Option.GetUserConfigFileByName("Layouts.config");

		// Token: 0x0400014B RID: 331
		public readonly int MainThreadId;

		// Token: 0x0400014C RID: 332
		private List<string> _LayoutList = new List<string>();

		// Token: 0x0400014D RID: 333
		private List<PadCodon> padContentCollection = new List<PadCodon>();

		// Token: 0x0400014E RID: 334
		private List<IViewContentExtend> viewContentCollection = new List<IViewContentExtend>();

		// Token: 0x0400014F RID: 335
		private Dictionary<PadCodon, IPadWindow> padWindows = new Dictionary<PadCodon, IPadWindow>();

		// Token: 0x04000150 RID: 336
		private Dictionary<IPadWindow, PadCodon> padCodons = new Dictionary<IPadWindow, PadCodon>();

		// Token: 0x04000151 RID: 337
		private IDocumentWindow lastActive;

		// Token: 0x04000152 RID: 338
		private LinkedList<IDocumentWindow> lastActiveWindows = new LinkedList<IDocumentWindow>();

		// Token: 0x04000153 RID: 339
		private bool closeAll;

		// Token: 0x04000154 RID: 340
		private Rectangle normalBounds = new Rectangle(0, 0, 1000, 600);

		// Token: 0x04000155 RID: 341
		private Gtk.Container rootWidget;

		// Token: 0x04000156 RID: 342
		private DockToolbarFrame toolbarFrame;

		// Token: 0x04000157 RID: 343
		private DockFrame dock;

		// Token: 0x04000158 RID: 344
		private DockNotebook tabControl;

		// Token: 0x04000159 RID: 345
		private Widget topMenu;

		// Token: 0x0400015A RID: 346
		private VBox fullViewVBox;

		// Token: 0x0400015B RID: 347
		private DockItem documentDockItem;

		// Token: 0x0400015C RID: 348
		private Widget toolbar;

		// Token: 0x0400015D RID: 349
		private Widget bottomBar;

		// Token: 0x0400015E RID: 350
		private bool initializing;

		// Token: 0x04000162 RID: 354
		private int activeWindowChangeLock = 0;
	}
}
