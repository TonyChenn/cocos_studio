using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.ControlLib;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using MonoDevelop.Ide.Codons;
using MonoDevelop.Ide.Gui;
using Xwt.GtkBackend;

namespace CocoStudio.Core.View
{
	// Token: 0x02000056 RID: 86
	public class Workbench
	{
		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000353 RID: 851 RVA: 0x0000ED28 File Offset: 0x0000CF28
		// (remove) Token: 0x06000354 RID: 852 RVA: 0x0000ED64 File Offset: 0x0000CF64
		public event EventHandler ActiveDocumentChanged;

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000355 RID: 853 RVA: 0x0000EDA0 File Offset: 0x0000CFA0
		public IEnumerable<DocumentExtend> Documents
		{
			get
			{
				return this.documents;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000356 RID: 854 RVA: 0x0000EDB8 File Offset: 0x0000CFB8
		public PadCollection Pads
		{
			get
			{
				if (this.pads == null)
				{
					this.pads = new PadCollection(this);
					foreach (PadCodon padContent in this.mainWindow.PadContentCollection)
					{
						this.WrapPad(padContent);
					}
				}
				return this.pads;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000357 RID: 855 RVA: 0x0000EE40 File Offset: 0x0000D040
		public DocumentExtend ActiveDocument
		{
			get
			{
				DocumentExtend result;
				if (this.mainWindow.ActiveWorkbenchWindow == null)
				{
					result = null;
				}
				else
				{
					result = this.WrapDocument(this.mainWindow.ActiveWorkbenchWindow);
				}
				return result;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000358 RID: 856 RVA: 0x0000EE7C File Offset: 0x0000D07C
		public ProgressMonitorManager ProgressMonitors
		{
			get
			{
				return this.monitors;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0000EE94 File Offset: 0x0000D094
		public MainWindow RootWindow
		{
			get
			{
				return this.mainWindow;
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000EED0 File Offset: 0x0000D0D0
		internal void Initialize(IProgressMonitor monitor)
		{
			Services.Workbench = this;
			this.mainWindow = new MainWindow(string.Empty);
			ApplicationCurrent.MainWindow = Services.MainWindow;
			this.mainWindow.Initialize();
			this.mainWindow.ActiveWorkbenchWindowChanged += this.OnDocumentChanged;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000EF24 File Offset: 0x0000D124
		private void OnDocumentChanged(object sender, EventArgs e)
		{
			using (TaskServiceLock.Lock())
			{
				if (this.ActiveDocumentChanged != null)
				{
					this.ActiveDocumentChanged(sender, e);
				}
				if (this.ActiveDocument != null)
				{
					this.ActiveDocument.LastTimeActive = DateTime.Now;
				}
				Services.TaskService.SetCurrentDocument(this.ActiveDocument);
				CSCocosHelp.StopAllEffects();
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000EFB0 File Offset: 0x0000D1B0
		internal void DocumentReloaded(DocumentExtend document)
		{
			if (this.ActiveDocument == document)
			{
				using (TaskServiceLock.Lock())
				{
					if (this.ActiveDocumentChanged != null)
					{
						this.ActiveDocumentChanged(null, null);
					}
				}
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000F014 File Offset: 0x0000D214
		internal void Show(string title)
		{
			this.RootWindow.Title = title;
			this.RootWindow.Realize();
			this.RootWindow.ShowAll();
			this.RootWindow.Show();
			this.RootWindow.CurrentLayout = "CustomLayout";
			if (MonoDevelop.Core.Platform.IsMac)
			{
				GtkWorkarounds.GrabDesktopFocus();
			}
			this.RootWindow.Present();
			this.monitors.Initialize();
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000F090 File Offset: 0x0000D290
		public void SaveAll()
		{
			List<DocumentExtend> list = this.Documents.ToList<DocumentExtend>();
			foreach (DocumentExtend documentExtend in list)
			{
				if (documentExtend != this.ActiveDocument)
				{
					documentExtend.Save();
				}
			}
			if (this.ActiveDocument != null && this.ActiveDocument.File != null)
			{
				this.ActiveDocument.File.ReloadReferencedItem(Services.ProgressMonitors.Default);
				this.ActiveDocument.Save();
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000F14C File Offset: 0x0000D34C
		public bool CloseAll(bool dontCloseCurrent = false)
		{
			DocumentExtend documentExtend;
			if (dontCloseCurrent)
			{
				documentExtend = this.ActiveDocument;
			}
			else
			{
				documentExtend = null;
			}
			bool flag = false;
			List<DocumentExtend> list = new List<DocumentExtend>();
			List<DocumentExtend> list2 = new List<DocumentExtend>();
			foreach (DocumentExtend documentExtend2 in this.Documents)
			{
				if (documentExtend2 != documentExtend)
				{
					list.Add(documentExtend2);
					if (documentExtend2.IsDirty)
					{
						flag = true;
						list2.Add(documentExtend2);
					}
				}
			}
			if (flag)
			{
				List<string> list3 = new List<string>();
				foreach (DocumentExtend documentExtend2 in list2)
				{
					list3.Add(documentExtend2.File.Name);
				}
				SaveDirtyFilesDialog saveDirtyFilesDialog = new SaveDirtyFilesDialog(list3);
				ResponseType responseType = (ResponseType)saveDirtyFilesDialog.Run();
				saveDirtyFilesDialog.Destroy();
				switch (responseType)
				{
				case ResponseType.Yes:
					foreach (DocumentExtend documentExtend2 in list2)
					{
						documentExtend2.Save();
					}
					if (documentExtend != null && documentExtend.Project != null)
					{
						using (TaskServiceLock.Lock())
						{
							documentExtend.File.ReloadReferencedItem(Services.ProgressMonitors.Default);
						}
					}
					break;
				case ResponseType.Cancel:
				case ResponseType.DeleteEvent:
					return false;
				}
			}
			foreach (DocumentExtend documentExtend2 in list)
			{
				documentExtend2.Close(true);
			}
			return true;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000F3AC File Offset: 0x0000D5AC
		public DocumentExtend OpenDocument(ResourceFile file)
		{
			return this.OpenDocument(file.FileName, file, true);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000F408 File Offset: 0x0000D608
		public DocumentExtend OpenDocument(FilePath file, ResourceFile project, bool bringToFront = true)
		{
			DocumentExtend result;
			if (string.IsNullOrEmpty(file.FileName))
			{
				result = null;
			}
			else
			{
				foreach (DocumentExtend documentExtend in this.Documents)
				{
					IBaseViewContent baseViewContent = null;
					if (documentExtend.Window.ViewContent.CanReuseView(file))
					{
						baseViewContent = documentExtend.Window.ViewContent;
					}
					if (baseViewContent != null)
					{
						if (project != null && documentExtend.Project != CocosProject.Instance)
						{
							documentExtend.SetProject(CocosProject.Instance);
							documentExtend.SetFile(project as CocosItem);
						}
						if (bringToFront)
						{
							documentExtend.Select();
							documentExtend.Window.SelectWindow();
						}
						return documentExtend;
					}
				}
				CocosItem cocosItem = project as CocosItem;
				if (cocosItem != null)
				{
					if (!cocosItem.IsInitialized)
					{
						cocosItem.Initialize(Services.ProjectsService.DefaultMonitor);
					}
					if (project.DataError != null)
					{
						LogConfig.Output.Error(project.DataError.Message);
						return null;
					}
				}
				IProgressMonitor statusProgressMonitor = this.ProgressMonitors.GetStatusProgressMonitor();
				FileOpenInfo fileOpenInfo = new FileOpenInfo(file, project, bringToFront);
				this.RealOpenFile(statusProgressMonitor, fileOpenInfo);
				statusProgressMonitor.Dispose();
				if (fileOpenInfo.NewContent != null && fileOpenInfo.NewContent.Project != null)
				{
					DocumentExtend doc = this.WrapDocument(fileOpenInfo.NewContent.DocumentWindow);
					if (doc != null && fileOpenInfo.BringToFront)
					{
						doc.RunWhenLoaded(delegate
						{
							if (doc.Window != null)
							{
								doc.Window.SelectWindow();
							}
						});
					}
					doc.SetProject(CocosProject.Instance);
					doc.SetFile(project as CocosItem);
					result = doc;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000F664 File Offset: 0x0000D864
		public DocumentExtend NewDocument(ResourceFolder parentFolder, CocosItemCreateInfo createInfo)
		{
			CocosItem cocosItem = Services.ProjectOperations.AddNewFile(parentFolder, createInfo);
			DocumentExtend result;
			if (cocosItem != null)
			{
				result = this.OpenDocument(cocosItem);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000F698 File Offset: 0x0000D898
		internal void RecoderDocuments(int oldPlacement, int newPlacement)
		{
			if (this.documents != null)
			{
				DocumentExtend item = this.documents[oldPlacement];
				this.documents.RemoveAt(oldPlacement);
				this.documents.Insert(newPlacement, item);
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000F700 File Offset: 0x0000D900
		private void RealOpenFile(IProgressMonitor monitor, FileOpenInfo openFileInfo)
		{
			FilePath fileName = openFileInfo.FileName;
			if (fileName == null)
			{
				monitor.ReportError("Invalid file name", null);
			}
			else if (fileName.IsDirectory)
			{
				monitor.ReportError(string.Format("{0} is a directory", fileName), null);
			}
			else if (!File.Exists(fileName))
			{
				monitor.ReportError(string.Format("File not found: {0}", fileName), null);
			}
			else
			{
				IDisplayBuilder displayBuilder = null;
				CocosItem cocosItem = openFileInfo.Project as CocosItem;
				IViewDisplayBuilder viewDisplayBuilder;
				if (openFileInfo.DisplayBuilder != null)
				{
					displayBuilder = openFileInfo.DisplayBuilder;
					viewDisplayBuilder = (displayBuilder as IViewDisplayBuilder);
				}
				else
				{
					IEnumerable<IDisplayBuilder> displayBuilders = DisplayBuilderService.GetDisplayBuilders(fileName, null, cocosItem);
					if (displayBuilders != null)
					{
						displayBuilder = displayBuilders.FirstOrDefault((IDisplayBuilder d) => d != null && d.CanUseAsDefault);
					}
					viewDisplayBuilder = (displayBuilder as IViewDisplayBuilder);
				}
				try
				{
					if (displayBuilder != null)
					{
						if (viewDisplayBuilder != null)
						{
							LoadFileWrapper loadFileWrapper = new LoadFileWrapper(monitor, this.mainWindow, viewDisplayBuilder, cocosItem, openFileInfo);
							loadFileWrapper.Invoke(fileName);
						}
					}
				}
				catch (Exception exception)
				{
					monitor.ReportError("Create ViewContent failed.", exception);
				}
			}
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000F86C File Offset: 0x0000DA6C
		internal DocumentExtend WrapDocument(IDocumentWindow window)
		{
			DocumentExtend result;
			if (window == null)
			{
				result = null;
			}
			else
			{
				DocumentExtend documentExtend = this.FindDocument(window);
				if (documentExtend != null)
				{
					result = documentExtend;
				}
				else
				{
					documentExtend = new DocumentExtend(window);
					window.Closing += this.OnWindowClosing;
					window.Closed += this.OnWindowClosed;
					this.documents.Add(documentExtend);
					documentExtend.OnDocumentAttached();
					this.OnDocumentOpened(new DocumentEventArgs(documentExtend));
					result = documentExtend;
				}
			}
			return result;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000F8F0 File Offset: 0x0000DAF0
		private void OnWindowClosed(object sender, WorkbenchWindowEventArgs args)
		{
			IDocumentWindow documentWindow = (IDocumentWindow)sender;
			DocumentExtend documentExtend = this.FindDocument(documentWindow);
			documentWindow.Closing -= this.OnWindowClosing;
			documentWindow.Closed -= this.OnWindowClosed;
			this.documents.Remove(documentExtend);
			this.OnDocumentClosed(documentExtend);
			documentExtend.DisposeDocument();
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000F950 File Offset: 0x0000DB50
		private void OnWindowClosing(object sender, WorkbenchWindowEventArgs args)
		{
			IDocumentWindow documentWindow = (IDocumentWindow)sender;
			if (!args.Forced && documentWindow.ViewContent != null && documentWindow.ViewContent.IsDirty)
			{
				string fileName = "";
				if (documentWindow.ViewContent.ContentName != null)
				{
					fileName = Path.GetFileName(documentWindow.ViewContent.ContentName);
				}
				SaveDirtyFilesDialog saveDirtyFilesDialog = new SaveDirtyFilesDialog(fileName);
				ResponseType responseType = (ResponseType)saveDirtyFilesDialog.Run();
				saveDirtyFilesDialog.Destroy();
				switch (responseType)
				{
				case ResponseType.No:
					args.Cancel = false;
					documentWindow.ViewContent.DiscardChanges();
					break;
				case ResponseType.Yes:
					if (documentWindow.ViewContent.ContentName == null)
					{
						this.FindDocument(documentWindow).Save();
						args.Cancel = documentWindow.ViewContent.IsDirty;
					}
					else
					{
						try
						{
							documentWindow.ContentExtend.Closing();
							documentWindow.ViewContent.Save();
							args.Cancel |= documentWindow.ViewContent.IsDirty;
						}
						catch (Exception)
						{
							args.Cancel = true;
							MessageBox.Show(LanguageInfo.Output_FailedToSaveFile, MessageBoxImage.Other, null, null);
						}
					}
					if (args.Cancel)
					{
						this.FindDocument(documentWindow).Select();
					}
					break;
				case ResponseType.Cancel:
				case ResponseType.DeleteEvent:
					args.Cancel = true;
					break;
				}
			}
			this.OnDocumentClosing(this.FindDocument(documentWindow));
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000FAE8 File Offset: 0x0000DCE8
		private static CocosItem GetProjectContainingFile(FilePath fileName)
		{
			CocosItem cocosItem = null;
			if (Services.ProjectOperations.CurrentSelectedProject != null)
			{
				if (Services.ProjectOperations.CurrentSelectedProject.FileName == fileName)
				{
					cocosItem = Services.ProjectOperations.CurrentSelectedProject;
				}
			}
			if (cocosItem == null && Services.ProjectOperations.CurrentSelectedWorkspaceItem != null)
			{
				cocosItem = Services.ProjectOperations.CurrentSelectedWorkspaceItem.GetProjectContainingFile(fileName);
				if (cocosItem == null)
				{
					WorkspaceItem parentWorkspace = Services.ProjectOperations.CurrentSelectedWorkspaceItem.ParentWorkspace;
					while (parentWorkspace != null && cocosItem == null)
					{
						cocosItem = parentWorkspace.GetProjectContainingFile(fileName);
						parentWorkspace = parentWorkspace.ParentWorkspace;
					}
				}
			}
			return cocosItem;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000FBA4 File Offset: 0x0000DDA4
		private void OnDocumentOpened(DocumentEventArgs documentEventArgs)
		{
			EventHandler<DocumentEventArgs> documentOpened = this.DocumentOpened;
			if (documentOpened != null)
			{
				documentOpened(this, documentEventArgs);
			}
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000FBCC File Offset: 0x0000DDCC
		private void OnDocumentClosed(DocumentExtend doc)
		{
			try
			{
				DocumentEventArgs e = new DocumentEventArgs(doc);
				EventHandler<DocumentEventArgs> documentClosed = this.DocumentClosed;
				if (documentClosed != null)
				{
					documentClosed(this, e);
				}
				Services.TaskService.Clear(doc);
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Exception while closing documents", ex);
			}
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000FC2C File Offset: 0x0000DE2C
		private void OnDocumentClosing(DocumentExtend doc)
		{
			try
			{
				DocumentEventArgs e = new DocumentEventArgs(doc);
				EventHandler<DocumentEventArgs> documentClosing = this.DocumentClosing;
				if (documentClosing != null)
				{
					documentClosing(this, e);
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Exception before closing documents", ex);
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000FCA4 File Offset: 0x0000DEA4
		private Pad WrapPad(PadCodon padContent)
		{
			if (this.pads == null)
			{
				foreach (Pad pad2 in this.Pads)
				{
					if (pad2.InternalContent == padContent)
					{
						return pad2;
					}
				}
			}
			Pad pad = new Pad(this.mainWindow, padContent);
			this.Pads.Add(pad);
			pad.Window.PadDestroyed += delegate(object param0, EventArgs param1)
			{
				this.Pads.Remove(pad);
			};
			return pad;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000FD78 File Offset: 0x0000DF78
		internal DocumentExtend FindDocument(IDocumentWindow window)
		{
			foreach (DocumentExtend documentExtend in this.Documents)
			{
				if (documentExtend.Window == window)
				{
					return documentExtend;
				}
			}
			return null;
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x0600036F RID: 879 RVA: 0x0000FDE4 File Offset: 0x0000DFE4
		// (remove) Token: 0x06000370 RID: 880 RVA: 0x0000FE20 File Offset: 0x0000E020
		public event EventHandler<DocumentEventArgs> DocumentOpened;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000371 RID: 881 RVA: 0x0000FE5C File Offset: 0x0000E05C
		// (remove) Token: 0x06000372 RID: 882 RVA: 0x0000FE98 File Offset: 0x0000E098
		public event EventHandler<DocumentEventArgs> DocumentClosed;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000373 RID: 883 RVA: 0x0000FED4 File Offset: 0x0000E0D4
		// (remove) Token: 0x06000374 RID: 884 RVA: 0x0000FF10 File Offset: 0x0000E110
		public event EventHandler<DocumentEventArgs> DocumentClosing;

		// Token: 0x04000172 RID: 370
		private readonly ProgressMonitorManager monitors = new ProgressMonitorManager();

		// Token: 0x04000173 RID: 371
		private readonly List<DocumentExtend> documents = new List<DocumentExtend>();

		// Token: 0x04000174 RID: 372
		private MainWindow mainWindow;

		// Token: 0x04000175 RID: 373
		private PadCollection pads;
	}
}
