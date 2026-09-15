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
	public class Workbench
	{
		public event EventHandler ActiveDocumentChanged;

		public IEnumerable<DocumentExtend> Documents
		{
			get
			{
				return this.documents;
			}
		}

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

		public ProgressMonitorManager ProgressMonitors
		{
			get
			{
				return this.monitors;
			}
		}

		public MainWindow RootWindow
		{
			get
			{
				return this.mainWindow;
			}
		}

		internal void Initialize(IProgressMonitor monitor)
		{
			Services.Workbench = this;
			this.mainWindow = new MainWindow(string.Empty);
			ApplicationCurrent.MainWindow = Services.MainWindow;
			this.mainWindow.Initialize();
			this.mainWindow.ActiveWorkbenchWindowChanged += this.OnDocumentChanged;
		}

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

		public DocumentExtend OpenDocument(ResourceFile file)
		{
			return this.OpenDocument(file.FileName, file, true);
		}

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

		internal void RecoderDocuments(int oldPlacement, int newPlacement)
		{
			if (this.documents != null)
			{
				DocumentExtend item = this.documents[oldPlacement];
				this.documents.RemoveAt(oldPlacement);
				this.documents.Insert(newPlacement, item);
			}
		}

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
					viewDisplayBuilder = (IViewDisplayBuilder)(displayBuilder = openFileInfo.DisplayBuilder);
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

		private void OnDocumentOpened(DocumentEventArgs documentEventArgs)
		{
			EventHandler<DocumentEventArgs> documentOpened = this.DocumentOpened;
			if (documentOpened != null)
			{
				documentOpened(this, documentEventArgs);
			}
		}

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

		public event EventHandler<DocumentEventArgs> DocumentOpened;

		public event EventHandler<DocumentEventArgs> DocumentClosed;

		public event EventHandler<DocumentEventArgs> DocumentClosing;

		private readonly ProgressMonitorManager monitors = new ProgressMonitorManager();

		private readonly List<DocumentExtend> documents = new List<DocumentExtend>();

		private MainWindow mainWindow;

		private PadCollection pads;
	}
}
