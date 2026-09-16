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
	public class MainWindow : WorkbenchWindow, IWorkbench, IWindowClosed
	{
		public IList<string> LayoutList
		{
			get
			{
				return this._LayoutList;
			}
		}

		public event EventHandler ActiveWorkbenchWindowChanged;

		public event EventHandler<EventArgs> InitializeCompleted;

		public List<PadCodon> PadContentCollection
		{
			get
			{
				return this.padContentCollection;
			}
		}

		public List<IViewContent> InternalViewContentCollection
		{
			get
			{
				return this.viewContentCollection.Cast<IViewContent>().ToList<IViewContent>();
			}
		}

		public event EventHandler LayoutReset;

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

		IWorkbenchWindow IWorkbench.ActiveWorkbenchWindow
		{
			get
			{
				return null;
			}
		}

		public DockFrame DockFrame
		{
			get
			{
				return this.dock;
			}
		}

		public DockNotebook DockNotebook
		{
			get
			{
				return this.tabControl;
			}
		}

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

		private void SetAppIcons()
		{
		}

		protected void OnHiding(object o, DeleteEventArgs e)
		{
			e.RetVal = true;
			base.Visible = false;
		}

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

		public void LockActiveWindowChangeEvent()
		{
			this.activeWindowChangeLock++;
		}

		public void UnlockActiveWindowChangeEvent()
		{
			this.activeWindowChangeLock--;
			this.OnActiveWindowChanged(null, null);
		}

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

		private void InitializeLayout()
		{
			this.CreateComponents();
			this.initializing = true;
			AddinManager.AddExtensionNodeHandler("/CocoStudio/Ide/Pads", new ExtensionNodeEventHandler(this.OnExtensionChanged));
			this.initializing = false;
		}

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
							dockItem.Visible = padCodon.PadId != MainWindow.AnimationPadId;
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
				this.ApplyObjectOutputDefaultDocking();
			}
		}

		private void ApplyObjectOutputDefaultDocking()
		{
			DockItem outputPad = this.dock.GetItem(MainWindow.OutputPadId);
			DockItem objectPad = this.dock.GetItem(MainWindow.ObjectPadId);
			if (outputPad == null || objectPad == null)
			{
				return;
			}
			outputPad.SetDockLocation("Documents/Bottom");
			objectPad.SetDockLocation(MainWindow.OutputPadId + "/CenterBefore");
		}

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

		private void DockTabControlVisibleChanged(object sender, EventArgs e)
		{
			this.FocusActiveDocumentWindow();
		}

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

		private void CreateMenuBar()
		{
			this.topMenu = MainWindowPartFactory.CreateMainMenu();
			Services.CommandService.SetMultiRootWindow(this);
		}

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

		private void UninstallMenuBar()
		{
			if (this.topMenu != null)
			{
				this.rootWidget.Remove(this.topMenu);
				this.topMenu.Destroy();
				this.topMenu = null;
			}
		}

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

		private void OnLayoutsExtensionChanged(object s, ExtensionNodeEventArgs args)
		{
		}

		public void CloseContent(IViewContentExtend content)
		{
			if (this.viewContentCollection.Contains(content))
			{
				this.viewContentCollection.Remove(content);
			}
		}

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

		private void SelectView(IViewContent content)
		{
			if (content is ViewContent)
			{
				(content as ViewContent).WorkbenchWindow.SelectWindow();
			}
		}

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

		public void ShowPad(PadCodon content)
		{
			this.AddPad(content, true);
		}

		public void AddPad(PadCodon content)
		{
			this.AddPad(content, false);
		}

		private void RegisterPad(PadCodon content)
		{
			this.padContentCollection.Add(content);
		}

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

		public void BringToFront(PadCodon content)
		{
			this.BringToFront(content, false);
		}

		public virtual void BringToFront(PadCodon content, bool giveFocus)
		{
			if (!this.IsVisible(content))
			{
				this.ShowPad(content);
			}
			this.ActivatePad(content, giveFocus);
		}

		public void ResetDefaultLayout()
		{
			this.CurrentLayout = "DefaultLayout";
			this.dock.DeleteLayout("CustomLayout");
			this.CurrentLayout = "CustomLayout";
			this.FocusActiveDocumentWindow();
		}

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

		private void HandleCurrentSelectedSolutionChanged(object sender, SolutionEventArgs e)
		{
			this.SetDefaultTitle();
		}

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

		internal void ShowPopup(DockNotebook notebook, int tabIndex, EventButton evt)
		{
			this.tabControl.CurrentTabIndex = tabIndex;
			Services.CommandService.ShowContextMenu(this.tabControl, evt, FileTabCommand.TabPopupMenu, null);
		}

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

		private bool IsInFullViewMode
		{
			get
			{
				return this.dock.CurrentLayout.EndsWith("[FullViewMode]");
			}
		}

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

		internal void CloseClicked(object o, TabEventArgs e)
		{
			this.CloseView(((DocumentWindow)e.Tab.Content).ContentExtend);
		}

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

		internal void ReorderTab(int oldPlacement, int newPlacement)
		{
			DockNotebookTab tab = this.tabControl.GetTab(oldPlacement);
			DockNotebookTab tab2 = this.tabControl.GetTab(newPlacement);
		}

		public IPadWindow GetPadWindow(PadCodon content)
		{
			IPadWindow result;
			this.padWindows.TryGetValue(content, out result);
			return result;
		}

		public bool IsVisible(PadCodon padContent)
		{
			DockItem dockItem = this.GetDockItem(padContent);
			return dockItem != null && dockItem.Visible;
		}

		public bool IsContentVisible(PadCodon padContent)
		{
			DockItem dockItem = this.GetDockItem(padContent);
			return dockItem != null && dockItem.ContentVisible;
		}

		public void HidePad(PadCodon padContent)
		{
			DockItem dockItem = this.GetDockItem(padContent);
			if (dockItem != null)
			{
				dockItem.Visible = false;
			}
		}

		public void ActivatePad(PadCodon padContent, bool giveFocus)
		{
			DockItem dockItem = this.GetDockItem(padContent);
			if (dockItem != null)
			{
				dockItem.Present(giveFocus);
			}
		}

		public bool IsSticky(PadCodon padContent)
		{
			DockItem dockItem = this.GetDockItem(padContent);
			return dockItem != null && (dockItem.Behavior & DockItemBehavior.Sticky) != DockItemBehavior.Normal;
		}

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

		public event EventHandler<CancelEventArgs> Closing;

		public event EventHandler<EventArgs> Closed;

		public const string DefaultLayoutID = "DefaultLayout";

		public const string CustomLayoutID = "CustomLayout";

		private const string AnimationPadId = "Modules.Animation.AnimationPad";

		private const string ObjectPadId = "Modules.UI.ComTool.ComToolPad";

		private const string OutputPadId = "Modules.Communal.Output.OutputPad";

		private const string fullViewModeTag = "[FullViewMode]";

		private const int MinimumWidth = 1000;

		private const int MinimumHeight = 600;

		private static string layoutFilePath = Option.GetUserConfigFileByName("Layouts.config");

		public readonly int MainThreadId;

		private List<string> _LayoutList = new List<string>();

		private List<PadCodon> padContentCollection = new List<PadCodon>();

		private List<IViewContentExtend> viewContentCollection = new List<IViewContentExtend>();

		private Dictionary<PadCodon, IPadWindow> padWindows = new Dictionary<PadCodon, IPadWindow>();

		private Dictionary<IPadWindow, PadCodon> padCodons = new Dictionary<IPadWindow, PadCodon>();

		private IDocumentWindow lastActive;

		private LinkedList<IDocumentWindow> lastActiveWindows = new LinkedList<IDocumentWindow>();

		private bool closeAll;

		private Rectangle normalBounds = new Rectangle(0, 0, 1000, 600);

		private Gtk.Container rootWidget;

		private DockToolbarFrame toolbarFrame;

		private DockFrame dock;

		private DockNotebook tabControl;

		private Widget topMenu;

		private VBox fullViewVBox;

		private DockItem documentDockItem;

		private Widget toolbar;

		private Widget bottomBar;

		private bool initializing;

		private int activeWindowChangeLock = 0;
	}
}
