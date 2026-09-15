using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Core.Events;
using CocoStudio.Model;
using CocoStudio.Projects;
using CocoStudio.Projects.Visiter;
using CocoStudio.UndoManager;
using CocoStudio.UserStatistics;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using Xwt.GtkBackend;

namespace Modules.Communal.ResourcePanel
{
	public class ResourceTreeBuilder : ITreeBuild
	{
		private void InitiaCommand()
		{
			GlobalCommand.ImportFileCmd.Execute += this.ImportResourceFileCmd_Execute;
			GlobalCommand.ImportDirCmd.Execute += this.ImportResourceFolderCmd_Execute;
			GlobalCommand.NewFileCmd.Execute += this.NewFileCmd_Execute;
			ResourceMenu.InitMenu();
		}

		private void NewFileCmd_Execute(object sender, CommandRunArgs args)
		{
			TreeIter iter;
			ResourceFolder resourceFolder = this.GetFolderBySelected(out iter);
			if (resourceFolder == null)
			{
				resourceFolder = Services.ProjectOperations.CurrentResourceGroup.RootFolder;
				this.GetFirstNode(Services.ProjectOperations.CurrentSelectedSolution, out iter);
			}
			if (resourceFolder != null)
			{
				SizeF sceneSize = Services.ProjectsService.CurrentSolution.GetSceneSize();
				Size size;
				if (sceneSize == SizeF.Empty)
				{
					size = new Size(960, 640);
				}
				else
				{
					size = new Size((int)sceneSize.Width, (int)sceneSize.Height);
				}
				NewFileDialog newFileDialog = new NewFileDialog(ApplicationCurrent.MainWindow, resourceFolder.BaseDirectory, size);
				int num = newFileDialog.Run();
				newFileDialog.Destroy();
				if (num == -5)
				{
					ViewRegions region = ViewRegions.UIMenu;
					if (Services.CommandService.CurrentCommandSource is ViewRegions)
					{
						region = (ViewRegions)Services.CommandService.CurrentCommandSource;
					}
					Tracker.Add(region, "NewFile", "New" + newFileDialog.FileType, "");
					IList<ResourceItem> currentSelectes = this.GetCurrentSelectes();
					string name = Path.Combine(resourceFolder.BaseDirectory, newFileDialog.FileName);
					CocosItemCreateInfo cocosItemCreateInfo = new CocosItemCreateInfo(name, currentSelectes, newFileDialog.Width, newFileDialog.Height);
					cocosItemCreateInfo.ContentType = newFileDialog.FileType;
					DocumentExtend documentExtend = Services.Workbench.NewDocument(resourceFolder, cocosItemCreateInfo);
					if (documentExtend != null && documentExtend.Project != null)
					{
						this.AddChildToTreeIter(documentExtend.File, iter, true);
						this.Expanded = true;
						this.SetSelecteResources(new ResourceItem[]
						{
							documentExtend.File
						});
					}
				}
			}
		}

		private void ImportResourceFileCmd_Execute(object sender, CommandRunArgs args)
		{
			string[] fileNames = FileChooserDialogModel.GetOpenFilePath(null, LanguageInfo.Menu_File_ImportFile, true, Services.RecentFileService.LastImportLocation, true).FileNames;
			this.ImportSelectedResources(fileNames);
		}

		private void ImportResourceFolderCmd_Execute(object sender, CommandRunArgs args)
		{
			string[] folders = FileChooserDialogModel.GetBrowseDialogPath(LanguageInfo.Menu_File_ImportFolder, true, Services.RecentFileService.LastImportLocation, false, true).Folders;
			this.ImportSelectedResources(folders);
		}

		private void ImportSelectedResources(string[] paths)
		{
			if (paths != null && paths.Length > 0)
			{
				ResourceFolder rootFolder = Services.ProjectOperations.CurrentResourceGroup.RootFolder;
				TreeIter treeIter;
				ResourceFolder resourceFolder = this.GetFolderBySelected(out treeIter);
				if (resourceFolder == null)
				{
					resourceFolder = rootFolder;
					this.GetFirstNode(Services.ProjectOperations.CurrentSelectedSolution, out treeIter);
				}
				this.ImportResources(paths, resourceFolder);
				Services.RecentFileService.LastImportLocation = Directory.Exists(paths[0]) ? paths[0] : Path.GetDirectoryName(paths[0]);
			}
			if (MonoDevelop.Core.Platform.IsWindows && Services.MainWindow.HasToplevelFocus)
			{
				Services.MainWindow.Present();
			}
		}

		internal async void ImportResources(string[] selectPath, ResourceFolder folder)
		{
			List<ResourceItem> importItems = await Services.ProjectOperations.ImportResourcesAsync(folder, selectPath, null);
			if (importItems != null && importItems.Count > 0)
			{
				this.SetSelecteResources(importItems);
			}
		}

		public void OpenInResourceManageHanlder()
		{
			string text = Services.ProjectOperations.CurrentResourceGroup.RootFolder.FullPath;
			if (this.tree.Selection.GetSelectedRows().Count<TreePath>() > 0)
			{
				if (this.dataItem is ResourceItem)
				{
					text = ((ResourceItem)this.dataItem).FullPath;
				}
				else if (this.dataItem is Solution)
				{
					text = ((Solution)this.dataItem).BaseDirectory.FullPath;
				}
			}
			if (File.Exists(text) || Directory.Exists(text))
			{
				if (MonoDevelop.Core.Platform.IsWindows)
				{
					System.Diagnostics.Process.Start("Explorer", "/select," + string.Format("\"{0}\"", text));
					return;
				}
				System.Diagnostics.Process.Start("open", "-R " + string.Format("\"{0}\"", text));
			}
		}

		public void NewFolderHanlder()
		{
			TreeIter iter;
			ResourceFolder resourceFolder = this.GetFolderBySelected(out iter);
			if (resourceFolder == null)
			{
				resourceFolder = Services.ProjectOperations.CurrentResourceGroup.RootFolder;
				this.GetFirstNode(Services.ProjectOperations.CurrentSelectedSolution, out iter);
			}
			if (resourceFolder == null || resourceFolder == Services.ProjectOperations.CurrentResourceGroup.RootFolder)
			{
				this.GetFirstNode(Services.ProjectOperations.CurrentSelectedSolution, out iter);
			}
			else
			{
				this.GetFirstNode(resourceFolder, out iter);
			}
			if (resourceFolder != null)
			{
				int num = 0;
				FilePath filePath;
				do
				{
					string text = "NewFolder";
					if (num > 0)
					{
						text += num;
					}
					filePath = resourceFolder.BaseDirectory.Combine(new string[]
					{
						text
					});
					num++;
				}
				while (Directory.Exists(filePath));
				FileService.CreateDirectory(filePath);
				IProgressMonitor consoleProgressMonitor = Services.ProgressMonitors.GetConsoleProgressMonitor(false, true);
				ResourceItem resourceItem = Services.ProjectOperations.AddResourceItem(resourceFolder, filePath, consoleProgressMonitor);
				this.AddChildToTreeIter(resourceItem, iter, true);
				this.ExpandToObject(resourceItem, true);
				this.pad.RenameCmd();
			}
		}

		public void DeleteResourceHandler()
		{
			using (TaskServiceLock.Lock())
			{
				TreePath[] selectedRows = this.tree.Selection.GetSelectedRows();
				if (selectedRows != null && selectedRows.Count<TreePath>() != 0)
				{
					string arg;
					if (!this.DeleteCheck(selectedRows, out arg))
					{
						MessageBox.Show(string.Format(LanguageInfo.MessageBox192_NowEditing, arg), MessageBoxImage.Other, null, null);
						return;
					}
					List<TreeIter> list = new List<TreeIter>();
					List<ResourceItem> list2 = new List<ResourceItem>();
					for (int i = selectedRows.Count<TreePath>() - 1; i >= 0; i--)
					{
						TreePath path = selectedRows[i];
						TreeIter iter;
						this.tree.CurrentModel.GetIter(out iter, path);
						ResourceItem resourceItem = this.GetDateItemByIter(iter) as ResourceItem;
						if (resourceItem != null)
						{
							list2.Add(resourceItem);
						}
					}
					List<ResourceItem> list3 = this.FilterChildren(list2);
					bool flag = false;
					bool flag2 = false;
					foreach (ResourceItem resourceItem2 in list3)
					{
						if (File.Exists(resourceItem2.FullPath) || Directory.Exists(resourceItem2.FullPath))
						{
							flag = true;
						}
					}
					if (flag)
					{
						string info;
						string noText;
						if (MonoDevelop.Core.Platform.IsMac)
						{
							info = string.Format(LanguageInfo.MessageBox202_ConfirmDelete, LanguageInfo.MessageBox265_Trash);
							noText = string.Format(LanguageInfo.MessageBox263_Delete, LanguageInfo.MessageBox265_Trash);
						}
						else
						{
							info = string.Format(LanguageInfo.MessageBox202_ConfirmDelete, LanguageInfo.MessageBox264_RecycleBin);
							noText = string.Format(LanguageInfo.MessageBox263_Delete, LanguageInfo.MessageBox264_RecycleBin);
						}
						ButtonText btnText = new ButtonText(LanguageInfo.Command_Remove, noText, LanguageInfo.Dialog_ButtonCancel, false, true, false);
						MessageBoxResult messageBoxResult = MessageBox.Show(info, btnText, MessageBoxImage.Question, ApplicationCurrent.MainWindow, EnumMainButton.No, null);
						if (messageBoxResult == MessageBoxResult.Cancel)
						{
							return;
						}
						if (messageBoxResult == MessageBoxResult.No)
						{
							flag2 = true;
						}
					}
					if (flag2)
					{
						Tracker.Add(ViewRegions.ResourcePanel, "RightMenuMoveToRecycleBin", "", "");
					}
					else
					{
						Tracker.Add(ViewRegions.ResourcePanel, "RightMenuRemove", "", "");
					}
					foreach (ResourceItem resourceItem3 in list3)
					{
						TreeIter treeIter;
						this.GetFirstNode(resourceItem3, out treeIter);
						IProgressMonitor @default = Services.ProgressMonitors.Default;
						NodeBuilder[] builderByIter = this.GetBuilderByIter(treeIter);
						if (builderByIter != null)
						{
							foreach (NodeBuilder nodeBuilder in builderByIter)
							{
								nodeBuilder.Delete(this, resourceItem3, @default, flag2);
							}
						}
						if (@default.AsyncOperation.Success)
						{
							list.Add(treeIter);
						}
						else
						{
							this.UpdateNodeAndChild(resourceItem3);
						}
					}
					Services.ProjectsService.ResourceChangeService.NotifyResourceChanged();
					list.ForEach(delegate(TreeIter n)
					{
						this.MoveToIter(n, false);
						this.Remove();
					});
				}
				Services.ProjectOperations.CurrentSelectedSolution.Save(Services.ProgressMonitors.Default);
			}
		}

		private bool DeleteCheck(IEnumerable<TreePath> deletePaths, out string hintInfo)
		{
			new List<TreePath>();
			bool result = true;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (DocumentExtend documentExtend in Services.Workbench.Documents)
			{
				TreeIter treeIter;
				this.GetFirstNode(documentExtend.File, out treeIter);
				TreePath docPath;
				if (this.tree.IsSearchState)
				{
					TreeIter iter = this.tree.Filter.ConvertChildIterToIter(treeIter);
					docPath = this.tree.CurrentModel.GetPath(iter);
				}
				else
				{
					docPath = this.tree.CurrentModel.GetPath(treeIter);
				}
				if (deletePaths.Contains(docPath))
				{
					result = false;
					stringBuilder.Append(documentExtend.File.RelativePath + Environment.NewLine);
				}
				IEnumerable<TreePath> source = from n in deletePaths
				where n.IsAncestor(docPath)
				select n;
				if (source.Count<TreePath>() != 0)
				{
					result = false;
					stringBuilder.Append(documentExtend.File.RelativePath + Environment.NewLine);
				}
			}
			hintInfo = stringBuilder.ToString();
			return result;
		}

		internal ResourceFolder GetFolderBySelected(out TreeIter iter)
		{
			TreeIter treeIter;
			this.tree.Selection.GetSelected(out treeIter);
			TreePath[] selectedRows = this.tree.Selection.GetSelectedRows();
			ResourceFolder rootFolder = Services.ProjectOperations.CurrentResourceGroup.RootFolder;
			if (selectedRows.Length == 0)
			{
				this.GetFirstNode(Services.ProjectOperations.CurrentSelectedSolution, out iter);
				return rootFolder;
			}
			NodeInfo nodeInfo = this.GetStoreValue(0) as NodeInfo;
			NodeBuilder[] builderChain = nodeInfo.BuilderChain;
			object dataObject = nodeInfo.DataItem;
			NodeBuilder nodeBuilder = builderChain.FirstOrDefault<NodeBuilder>();
			if (nodeBuilder != null)
			{
				ResourceFolder targetFolder = nodeBuilder.GetTargetFolder(dataObject);
				if (rootFolder == targetFolder)
				{
					this.GetFirstNode(Services.ProjectOperations.CurrentSelectedSolution, out iter);
				}
				else
				{
					this.GetFirstNode(targetFolder, out iter);
				}
				return targetFolder;
			}
			this.tree.CurrentModel.GetIterFirst(out iter);
			return null;
		}

		private void InitEvent()
		{
			this.OnlyDragDropOut();
			this.tree.Selection.Changed += this.OnSelectionChanged;
			this.tree.ButtonReleaseEvent += this.tree_ButtonReleaseEvent;
			this.tree.ButtonPressEvent += this.tree_ButtonPressEvent;
			this.tree.DragBegin += this.tree_DragBegin;
			this.tree.DragDrop += this.tree_DragDrop;
			this.tree.DragMotion += this.tree_DragMotion;
			this.tree.DragEnd += this.tree_DragEnd;
			this.tree.OnMouseDoubleClick += this.tree_OnMouseDoubleClick;
			Services.ProjectOperations.CurrentSelectedSolutionChanged += this.ProjectOperations_CurrentSelectedSolutionChanged;
			Services.EventsService.GetEvent<AddResourcesEvent>().Subscribe(new Action<AddResourcesArgs>(this.ImportResources));
			this.tree.Parent.DragDataReceived += this.Parent_DragDataReceived;
		}

		[ConnectBefore]
		private void tree_DragEnd(object o, DragEndArgs args)
		{
			this.lastHoreItem = null;
		}

		[ConnectBefore]
		private async void Parent_DragDataReceived(object o, DragDataReceivedArgs args)
		{
			if (args.SelectionData.Type != null)
			{
				if (Services.ProjectOperations.CurrentSelectedSolution != null && !this.tree.IsSearchState)
				{
					IEnumerable<string> fileArray = args.SelectionData.GetFileArray().FileArray;
					if (fileArray != null && fileArray.Count<string>() > 0)
					{
						ResourceFolder folder = this.GetImportTargetFolder(o as Widget, args.X, args.Y);
						this.ImportResources(fileArray.ToArray<string>(), folder);
					}
				}
			}
		}

		private ResourceFolder GetImportTargetFolder(Widget dragWidget, int x, int y)
		{
			ResourceFolder rootFolder = Services.ProjectOperations.CurrentResourceGroup.RootFolder;
			if (dragWidget != null && dragWidget != this.tree)
			{
				int treeX;
				int treeY;
				if (!dragWidget.TranslateCoordinates(this.tree, x, y, out treeX, out treeY))
				{
					return rootFolder;
				}
				x = treeX;
				y = treeY;
			}
			TreePath path;
			if (!this.tree.GetPathAtPos(x, y, out path))
			{
				return rootFolder;
			}
			TreeIter iter;
			if (!this.tree.CurrentModel.GetIter(out iter, path))
			{
				return rootFolder;
			}
			ResourceItem resourceItem = this.GetDateItemByIter(iter) as ResourceItem;
			ResourceFolder folder = resourceItem as ResourceFolder;
			if (folder != null)
			{
				return folder;
			}
			ResourceFolder parentFolder = resourceItem == null ? null : resourceItem.Parent as ResourceFolder;
			return parentFolder ?? rootFolder;
		}

		private void ImportResources(AddResourcesArgs obj)
		{
			if (obj.AddItems == null)
			{
				return;
			}
			List<ResourceItem> list = obj.AddItems.ToList<ResourceItem>();
			list.Sort(new Comparison<ResourceItem>(this.ResourceComparison));
			ResourceFolder parent = obj.Parent;
			TreeIter iter;
			if (parent == null || parent == Services.ProjectOperations.CurrentResourceGroup.RootFolder)
			{
				this.GetFirstNode(Services.ProjectOperations.CurrentSelectedSolution, out iter);
			}
			else
			{
				this.GetFirstNode(parent, out iter);
			}
			foreach (ResourceItem resourceItem in list)
			{
				if (this.nodeHash.ContainsKey(resourceItem.Parent))
				{
					TreeIter treeIter = (TreeIter)this.nodeHash[resourceItem.Parent];
					if (!TreeIter.Zero.Equals(treeIter))
					{
						iter = treeIter;
					}
				}
				else if (resourceItem.Parent == Services.ProjectsService.CurrentResourceGroup.RootFolder)
				{
					this.GetFirstNode(Services.ProjectOperations.CurrentSelectedSolution, out iter);
				}
				this.AddChildToTreeIter(resourceItem, iter, false);
				if (obj.IsExpand)
				{
					this.ExpandToObject(resourceItem, true);
				}
			}
		}

		public int ResourceComparison(ResourceItem x, ResourceItem y)
		{
			if (x == y)
			{
				return 0;
			}
			if (x == null)
			{
				if (y == null)
				{
					return 0;
				}
				return -1;
			}
			else
			{
				if (y == null)
				{
					return 1;
				}
				FilePath filePath = x.FullPath;
				FilePath filePath2 = y.FullPath;
				if (filePath.IsChildPathOf(filePath2))
				{
					return 1;
				}
				return filePath.CompareTo(filePath2);
			}
		}

		private void ProjectOperations_CurrentSelectedSolutionChanged(object sender, SolutionEventArgs e)
		{
			if (e.Solution != null)
			{
				this.pad.LoadTree(e.Solution);
			}
		}

		public void AllDrag()
		{
			Gtk.Drag.SourceSet(this.tree, ModifierType.Button1Mask, ResourceTreeBuilder.target_tableWindows, DragAction.Copy | DragAction.Move | DragAction.Link);
		}

		public void AllDrop()
		{
			this.tree.EnableModelDragDest(ResourceTreeBuilder.target_tableWindows, DragAction.Copy | DragAction.Move | DragAction.Link);
		}

		public void OnlyDragDropOut()
		{
			TreePath[] selectedRows = this.tree.Selection.GetSelectedRows();
			if (selectedRows.Length == 0)
			{
				Gtk.Drag.SourceSet(this.tree, ModifierType.None, null, (DragAction)0);
				return;
			}
			if (MonoDevelop.Core.Platform.IsMac)
			{
				this.tree.EnableModelDragDest(ResourceTreeBuilder.target_tableMac, DragAction.Copy | DragAction.Move | DragAction.Link);
				Gtk.Drag.SourceSet(this.tree, ModifierType.Button1Mask, ResourceTreeBuilder.target_tableMac, DragAction.Copy | DragAction.Move | DragAction.Link);
				return;
			}
			this.AllDrop();
			this.AllDrag();
		}

		private void tree_DragBegin(object o, DragBeginArgs args)
		{
			args.RetVal = false;
			this.SetDragData(args.Context);
		}

		internal void SetDragData(DragContext context)
		{
			if (!this.CanDrag())
			{
				context.SetDragData(null);
				return;
			}
			IList<ResourceItem> dragContext = this.GetDragContext();
			if (dragContext == null)
			{
				context.SetDragData(null);
				return;
			}
			if (dragContext != null)
			{
				ResourceInfoDragData data = new ResourceInfoDragData(dragContext);
				context.SetDragData(data);
			}
		}

		[ConnectBefore]
		private void tree_DragMotion(object o, DragMotionArgs args)
		{
			CellRenderHelper.HoverdItem = null;
			DragContext context = args.Context;
			Widget sourceWidget = context.GetSourceWidget();
			if (this.tree.IsSearchState || (sourceWidget != this.tree && sourceWidget != null))
			{
				Gdk.Drag.Status(args.Context, (DragAction)0, args.Time);
				args.RetVal = true;
				return;
			}
			context.GetDragData();
			TreePath treePath;
			TreeViewDropPosition pos;
			this.tree.GetDestRowAtPos(args.X, args.Y, out treePath, out pos);
			if (treePath == null)
			{
				return;
			}
			TreeIter iter;
			this.tree.CurrentModel.GetIter(out iter, treePath);
			ResourceItem hoverdItem = this.GetHoverdItem(iter, pos);
			CellRenderHelper.HoverdItem = hoverdItem;
			this.lastHoreItem = hoverdItem;
		}

		private ResourceItem GetHoverdItem(TreeIter iter, TreeViewDropPosition pos)
		{
			ResourceItem resourceItem = this.GetDateItemByIter(iter) as ResourceItem;
			if (resourceItem == null)
			{
				return Services.ProjectOperations.CurrentResourceGroup.RootFolder;
			}
			switch (pos)
			{
			case TreeViewDropPosition.IntoOrBefore:
			case TreeViewDropPosition.IntoOrAfter:
				return resourceItem;
			default:
				return null;
			}
		}

		private void tree_DragDrop(object o, DragDropArgs args)
		{
			ResourceInfoDragData resourceInfoDragData = args.Context.GetDragData() as ResourceInfoDragData;
			if (resourceInfoDragData != null && !this.tree.IsSearchState)
			{
				TreePath treePath;
				TreeViewDropPosition pos;
				this.tree.GetDestRowAtPos(args.X, args.Y, out treePath, out pos);
				if (treePath == null)
				{
					return;
				}
				TreeIter iter;
				this.tree.CurrentModel.GetIter(out iter, treePath);
				ResourceItem target = this.GetTarget(iter, pos);
				List<ResourceItem> list = this.FilterChildren(resourceInfoDragData.Items);
				foreach (ResourceItem resourceItem in list)
				{
					NodeBuilder[] builderChain = this.pad.GetBuilderChain(resourceItem.GetType());
					foreach (NodeBuilder nodeBuilder in builderChain)
					{
						string text = nodeBuilder.CanMove(resourceItem, target, pos);
						if (!string.IsNullOrWhiteSpace(text))
						{
							LogConfig.Output.Debug(text);
							return;
						}
					}
				}
				ResourceFolder resourceFolder;
				if (target is ResourceFile)
				{
					resourceFolder = (target.Parent as ResourceFolder);
				}
				else
				{
					resourceFolder = (target as ResourceFolder);
				}
				if (resourceFolder != null)
				{
					this.MoveToTargetFolder(list, resourceFolder);
				}
			}
		}

		private ResourceItem GetTarget(TreeIter iter, TreeViewDropPosition pos)
		{
			ResourceItem resourceItem = this.GetDateItemByIter(iter) as ResourceItem;
			if (resourceItem == null)
			{
				return Services.ProjectOperations.CurrentResourceGroup.RootFolder;
			}
			if (resourceItem is ResourceFile)
			{
				return resourceItem.Parent;
			}
			switch (pos)
			{
			case TreeViewDropPosition.Before:
			case TreeViewDropPosition.After:
				return resourceItem.Parent;
			default:
				return resourceItem;
			}
		}

		private void MoveToTargetFolder(IList<ResourceItem> moveResoruces, ResourceFolder targetFolder)
		{
			this.pad.Tree.FreezeChildNotify();
			foreach (ResourceItem resourceItem in moveResoruces)
			{
				string name = Path.Combine(targetFolder.FullPath, resourceItem.Name);
				string fullPath = resourceItem.FullPath;
				try
				{
					ResourceFolder resourceFolder = resourceItem.Parent as ResourceFolder;
					resourceFolder.Items.Remove(resourceItem);
					targetFolder.Items.Add(resourceItem);
					resourceItem.Move(name);
					Services.Workspace.SaveCurrentSolution();
					TreeIter treeIter;
					this.GetFirstNode(resourceItem, out treeIter);
					TreeIter iter;
					if (targetFolder == Services.ProjectOperations.CurrentResourceGroup.RootFolder)
					{
						this.GetFirstNode(Services.ProjectOperations.CurrentSelectedSolution, out iter);
					}
					else
					{
						this.GetFirstNode(targetFolder, out iter);
					}
					this.Remove(resourceItem);
					this.AddChildToTreeIter(resourceItem, iter, false);
				}
				catch (Exception ex)
				{
					LogConfig.Output.Error(ex.Message, ex);
				}
			}
			Services.ProjectsService.ResourceChangeService.NotifyResourceChanged();
			Services.Workspace.SaveCurrentSolution();
			this.pad.Tree.ThawChildNotify();
		}

		internal List<ResourceItem> FilterChildren(IList<ResourceItem> resoruces)
		{
			IEnumerable<ResourceItem> enumerable = from n in resoruces
			where n is ResourceFolder
			select n;
			if (enumerable != null)
			{
				List<ResourceItem> list = new List<ResourceItem>();
				using (IEnumerator<ResourceItem> enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ResourceFolder item = (ResourceFolder)enumerator.Current;
						IEnumerable<ResourceItem> collection = resoruces.Where(delegate(ResourceItem n)
						{
							string name = n.FullPath;
							if (n is PlistImageFile)
							{
								name = n.PreviewImagePath;
							}
							return this.CheckChildPath(item.BaseDirectory, name);
						});
						list.AddRange(collection);
					}
				}
				foreach (ResourceItem item2 in list)
				{
					resoruces.Remove(item2);
				}
			}
			return resoruces.ToList<ResourceItem>();
		}

		private bool CheckChildPath(FilePath parent, FilePath childer)
		{
			return !(parent == childer) && childer.IsChildPathOf(parent);
		}

		private bool FinderChilder(ResourceFolder folder, ResourceItem childer)
		{
			if (folder.Items.Contains(childer))
			{
				return true;
			}
			foreach (ResourceItem resourceItem in folder.Items)
			{
				if (resourceItem is ResourceFolder)
				{
					bool flag = this.FinderChilder((ResourceFolder)resourceItem, childer);
					if (flag)
					{
						return flag;
					}
				}
			}
			return false;
		}

		private void tree_OnMouseDoubleClick(object sender, WidgetEventArgs e)
		{
			TreePath[] selectedRows = this.tree.Selection.GetSelectedRows();
			if (selectedRows.Length == 0)
			{
				return;
			}
			TreeIter iter;
			this.tree.CurrentModel.GetIter(out iter, selectedRows[0]);
			object dateItemByIter = this.GetDateItemByIter(iter);
			ResourceFile resourceFile = dateItemByIter as ResourceFile;
			if (resourceFile != null)
			{
				this.OpenResource(resourceFile);
			}
			else
			{
				this.Expanded = !this.Expanded;
			}
		}

		[ConnectBefore]
		private void tree_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			Widget widget = o as Widget;
			widget.CanFocus = true;
			widget.HasFocus = true;
			if (args.Event.IsContextMenuButton())
			{
				GtkWorkarounds.ShowContextMenu(ResourceMenu.ContextMenu, this.tree, args.Event);
			}
			widget.HasFocus = false;
		}

		[ConnectBefore]
		private void tree_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			Widget widget = o as Widget;
			if (widget != null && args.Event.Button == 1U)
			{
				TreePath treePath;
				TreeViewColumn treeViewColumn;
				int num;
				int num2;
				if (this.tree.GetPathAtPos((int)args.Event.X, (int)args.Event.Y, out treePath, out treeViewColumn, out num, out num2))
				{
					if (MonoDevelop.Core.Platform.IsMac)
					{
						this.tree.EnableModelDragDest(ResourceTreeBuilder.target_tableMac, DragAction.Copy | DragAction.Move | DragAction.Link);
						Gtk.Drag.SourceSet(this.tree, ModifierType.Button1Mask, ResourceTreeBuilder.target_tableMac, DragAction.Copy | DragAction.Move | DragAction.Link);
					}
					else
					{
						this.AllDrop();
						this.AllDrag();
					}
				}
				widget.GrabFocus();
				widget.HasFocus = true;
			}
		}

		internal void OpenResource(ResourceItem resourceItem)
		{
			ResourceFile resourceFile = resourceItem as ResourceFile;
			if (resourceFile == null)
			{
				return;
			}
			resourceFile.Refresh();
			Services.Workbench.OpenDocument(resourceFile.FullPath, resourceFile as CocosItem, true);
			CocosItem cocosItem = resourceFile as CocosItem;
			if (cocosItem != null)
			{
				Tracker.Add(ViewRegions.ResourcePanel, "OpenFile", "Open" + cocosItem.ContentType, "");
			}
		}

		[ConnectBefore]
		private void OnSelectionChanged(object sender, EventArgs e)
		{
			this.OnlyDragDropOut();
			this.tree.Selection.GetSelectedRows();
			List<ResourceItem> currentSelectes = this.GetCurrentSelectes();
			if (currentSelectes != null)
			{
				object obj = currentSelectes.FirstOrDefault<ResourceItem>();
				if (obj == Services.ProjectOperations.CurrentResourceGroup.RootFolder)
				{
					obj = Services.ProjectOperations.CurrentSelectedSolution;
				}
				TreeIter it;
				this.GetFirstNode(obj, out it);
				this.InitIter(it, obj);
				SelectedResourceItemsChangedArgs payload = new SelectedResourceItemsChangedArgs(currentSelectes);
				Services.EventsService.GetEvent<SelectedResourceItemsChangedEvent>().Publish(payload);
			}
			Services.ProjectsService.CurrentResourceItems = currentSelectes;
		}

		private IList<ResourceItem> GetDragContext()
		{
			TreePath[] selectedRows = this.tree.Selection.GetSelectedRows();
			if (selectedRows != null && selectedRows.Length > 0)
			{
				List<ResourceItem> list = new List<ResourceItem>();
				foreach (TreePath path in selectedRows)
				{
					TreeIter iter;
					this.tree.CurrentModel.GetIter(out iter, path);
					ResourceItem resourceItem = this.GetDateItemByIter(iter) as ResourceItem;
					if (resourceItem != null)
					{
						list.Add(resourceItem);
					}
				}
				return list;
			}
			return null;
		}

		private bool CanDrag()
		{
			TreePath[] selectedRows = this.tree.Selection.GetSelectedRows();
			if (selectedRows != null && selectedRows.Length > 0)
			{
				new List<ResourceItem>();
				foreach (TreePath path in selectedRows)
				{
					TreeIter iter;
					this.tree.CurrentModel.GetIter(out iter, path);
					NodeBuilder[] builderByIter = this.GetBuilderByIter(iter);
					object dateItemByIter = this.GetDateItemByIter(iter);
					if (builderByIter != null)
					{
						foreach (NodeBuilder nodeBuilder in builderByIter)
						{
							if (!nodeBuilder.CanDrag(dateItemByIter))
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}

		public TreeIter CurrentIter
		{
			get
			{
				return this.currentIter;
			}
			set
			{
				this.currentIter = value;
			}
		}

		public object DataItem
		{
			get
			{
				return this.dataItem;
			}
		}

		public bool Expanded
		{
			get
			{
				return this.tree.GetRowExpanded(this.store.GetPath(this.currentIter));
			}
			set
			{
				if (!this.store.IterIsValid(this.currentIter))
				{
					return;
				}
				if (value && !this.Expanded)
				{
					TreePath path = this.store.GetPath(this.currentIter);
					this.tree.ExpandRow(path, false);
					this.tree.SetCursor(path, this.pad.Completecolumn, false);
					return;
				}
				if (!value && this.Expanded)
				{
					TreePath path2 = this.store.GetPath(this.currentIter);
					this.tree.CollapseRow(path2);
				}
			}
		}

		public ResourceTreeBuilder(ResourceTreeView pad) : this(pad, TreeIter.Zero)
		{
		}

		public ResourceTreeBuilder(ResourceTreeView pad, TreeIter iter)
		{
			this.pad = pad;
			this.tree = pad.Tree;
			this.store = pad.Store;
			this.InitiaCommand();
			this.InitEvent();
			this.MoveToIter(iter, false);
		}

		public void UpdateAll()
		{
			if (Services.ProjectOperations.CurrentResourceGroup == null)
			{
				return;
			}
			Services.ProjectOperations.CurrentResourceGroup.RootFolder.Refresh();
			List<KeyValuePair<object, object>> list = this.nodeHash.ToList<KeyValuePair<object, object>>();
			foreach (KeyValuePair<object, object> keyValuePair in list)
			{
				this.Update(keyValuePair.Key);
			}
		}

		public void Update()
		{
			NodeInfo nodeInfoByIter = this.GetNodeInfoByIter(this.currentIter);
			if (nodeInfoByIter == null)
			{
				return;
			}
			this.UpdateNode(nodeInfoByIter.BuilderChain, nodeInfoByIter.DataItem);
		}

		public void UpdateChildren()
		{
			this.FillNode(null);
		}

		internal NodeInfo GetNodeInfoByIter(TreeIter iter)
		{
			return this.tree.CurrentModel.GetValue(iter, 0) as NodeInfo;
		}

		internal object GetDateItemByIter(TreeIter iter)
		{
			NodeInfo nodeInfoByIter = this.GetNodeInfoByIter(iter);
			if (nodeInfoByIter != null)
			{
				return nodeInfoByIter.DataItem;
			}
			return null;
		}

		internal NodeBuilder[] GetBuilderByIter(TreeIter iter)
		{
			NodeInfo nodeInfoByIter = this.GetNodeInfoByIter(iter);
			if (nodeInfoByIter != null)
			{
				return nodeInfoByIter.BuilderChain;
			}
			return null;
		}

		public void Clear()
		{
			object[] array = new object[this.nodeHash.Count];
			this.nodeHash.Keys.CopyTo(array, 0);
			foreach (object dataObject in array)
			{
				this.NotifyNodeRemoved(dataObject, null);
			}
			this.nodeHash = null;
			this.nodeHash = new NodeHashtable();
			this.store.Clear();
			this.currentIter = TreeIter.Zero;
			this.dataItem = null;
		}

		public void Remove()
		{
			if (this.store.IterIsValid(this.currentIter))
			{
				TreeIter treeIter = this.currentIter;
				if (this.tree.IsSearchState)
				{
					this.currentIter = this.tree.Filter.ConvertChildIterToIter(this.currentIter);
				}
				object dateItemByIter = this.GetDateItemByIter(this.currentIter);
				this.RemoveChildren(treeIter);
				this.UnregisterNode(dateItemByIter, treeIter, null, true);
				if (this.store.Remove(ref treeIter) && !treeIter.Equals(TreeIter.Zero))
				{
					this.MoveToIter(treeIter, false);
				}
			}
		}

		public void Remove(object dataObject)
		{
			this.MoveToObject(dataObject);
			this.Remove();
		}

		private void RemoveChildren(TreeIter it)
		{
			TreeIter treeIter;
			while (this.store.IterChildren(out treeIter, it))
			{
				this.RemoveChildren(treeIter);
				object dateItemByIter = this.GetDateItemByIter(treeIter);
				if (dateItemByIter != null)
				{
					this.UnregisterNode(dateItemByIter, treeIter, null, true);
				}
				this.store.Remove(ref treeIter);
			}
		}

		public void AddChild(object dataObject)
		{
			this.AddChild(dataObject, false);
		}

		public void AddChild(object parent, object dataObject, bool moveToChild)
		{
			if (parent == null || (parent is ResourceFolder && (ResourceFolder)parent == Services.ProjectOperations.CurrentResourceGroup.RootFolder))
			{
				parent = Services.ProjectOperations.CurrentSelectedSolution;
			}
			TreeIter iter;
			this.GetFirstNode(parent, out iter);
			this.AddChildToTreeIter(dataObject, iter, moveToChild);
		}

		public void AddChildToTreeIter(object dataObject, TreeIter iter, bool moveToChild = false)
		{
			if (dataObject == null)
			{
				throw new ArgumentNullException("dataObject");
			}
			if (dataObject is ResourceFolder && dataObject == Services.ProjectOperations.CurrentResourceGroup.RootFolder)
			{
				dataObject = Services.ProjectOperations.CurrentSelectedSolution;
			}
			if (this.nodeHash.ContainsKey(dataObject))
			{
				this.FillNode(dataObject);
				return;
			}
			TreeIter zero = TreeIter.Zero;
			this.store.GetIterFirst(out zero);
			NodeBuilder[] builderChain = this.pad.GetBuilderChain(dataObject.GetType());
			if (builderChain == null)
			{
				return;
			}
			TreeIter it;
			if (!this.currentIter.Equals(TreeIter.Zero))
			{
				it = this.store.AppendValues(iter, new object[0]);
			}
			else if (dataObject is Solution)
			{
				it = this.store.AppendNode();
			}
			else
			{
				it = this.store.AppendValues(zero, new object[0]);
			}
			this.RegisterNode(it, dataObject, builderChain, true);
			this.BuildNode(it, builderChain, dataObject);
			if (moveToChild)
			{
				this.MoveToIter(iter, moveToChild);
			}
			this.Update(dataObject);
		}

		public void AddChildToRoot(object dataObject)
		{
			this.currentIter = TreeIter.Zero;
			this.AddChild(dataObject, false);
		}

		public void AddChildren(IEnumerable dataObjects)
		{
			this.pad.Tree.FreezeChildNotify();
			foreach (object dataObject in dataObjects)
			{
				this.AddChild(dataObject);
			}
			this.pad.Tree.ThawChildNotify();
		}

		public void AddChild(object dataObject, bool moveToChild)
		{
			if (dataObject == null)
			{
				throw new ArgumentNullException("dataObject");
			}
			if (dataObject is ResourceFolder && dataObject == Services.ProjectOperations.CurrentResourceGroup.RootFolder)
			{
				dataObject = Services.ProjectOperations.CurrentSelectedSolution;
			}
			if (this.nodeHash.ContainsKey(dataObject))
			{
				this.FillNode(dataObject);
				return;
			}
			TreeIter zero = TreeIter.Zero;
			this.store.GetIterFirst(out zero);
			TreeIter iter = this.currentIter;
			NodeBuilder[] builderChain = this.pad.GetBuilderChain(dataObject.GetType());
			if (builderChain == null)
			{
				return;
			}
			TreeIter it;
			if (!this.currentIter.Equals(TreeIter.Zero))
			{
				it = this.store.AppendNode(this.currentIter);
			}
			else if (dataObject is Solution)
			{
				it = this.store.AppendNode();
			}
			else
			{
				it = this.store.AppendValues(zero, new object[0]);
			}
			this.RegisterNode(it, dataObject, builderChain, true);
			this.BuildNode(it, builderChain, dataObject);
			this.MoveToIter(iter, false);
		}

		internal bool GetFirstNode(object dataObject, out TreeIter iter)
		{
			if (dataObject == Services.ProjectsService.CurrentResourceGroup.RootFolder)
			{
				dataObject = Services.ProjectsService.CurrentSolution;
			}
			object obj;
			if (dataObject == null || !this.nodeHash.TryGetValue(dataObject, out obj))
			{
				iter = TreeIter.Zero;
				return false;
			}
			if (obj is TreeIter)
			{
				iter = (TreeIter)obj;
			}
			else
			{
				iter = ((TreeIter[])obj)[0];
			}
			return true;
		}

		private void BuildNode(TreeIter it, NodeBuilder[] chain, object dataObject)
		{
			TreeIter it2 = this.currentIter;
			this.InitIter(it, dataObject);
			this.UpdateNode(chain, dataObject);
			this.CreateChildren(chain, dataObject);
			this.InitIter(it2, this.dataItem);
		}

		public void Update(object objecData)
		{
			if (objecData is ResourceFolder && objecData == Services.ProjectOperations.CurrentResourceGroup.RootFolder)
			{
				objecData = Services.ProjectOperations.CurrentSelectedSolution;
			}
			TreeIter iter;
			this.GetFirstNode(objecData, out iter);
			NodeInfo nodeInfo = (NodeInfo)this.store.GetValue(iter, 0);
			this.UpdateNode(nodeInfo.BuilderChain, objecData);
		}

		internal void UpdateNodeAndChild(ResourceItem node)
		{
			this.Update(node);
			ResourceFolder resourceFolder = node as ResourceFolder;
			if (resourceFolder != null)
			{
				foreach (ResourceItem node2 in resourceFolder.Items)
				{
					this.UpdateNodeAndChild(node2);
				}
			}
		}

		private void UpdateNode(NodeBuilder[] chain, object dataObject)
		{
			TreeIter treeIter;
			this.GetFirstNode(dataObject, out treeIter);
			NodeInfo nodeInfo = (NodeInfo)this.store.GetValue(treeIter, 0);
			if (nodeInfo == null)
			{
				nodeInfo = new NodeInfo();
			}
			nodeInfo.BuilderChain = chain;
			this.GetNodeInfo(this.pad, this, chain, dataObject, nodeInfo);
			this.SetNodeInfo(treeIter, nodeInfo);
		}

		private void SetNodeInfo(TreeIter it, NodeInfo nodeInfo)
		{
			this.store.SetValue(it, 0, nodeInfo);
			if (this.tree.IsSearchState)
			{
				this.tree.Filter.Refilter();
			}
			this.pad.Tree.QueueDraw();
		}

		private void GetNodeInfo(ResourceTreeView pad, ITreeBuild tb, NodeBuilder[] chain, object dataObject, NodeInfo nodeInfo)
		{
			foreach (NodeBuilder nodeBuilder in chain)
			{
				try
				{
					nodeBuilder.BuildNode(tb, dataObject, nodeInfo);
				}
				catch (Exception message)
				{
					LogConfig.Logger.Debug(message);
				}
				this.dataItem = this.GetStoreValue(0);
			}
		}

		internal void RegisterNode(TreeIter it, object dataObject, NodeBuilder[] chain, bool fireAddedEvent)
		{
			object obj;
			if (!this.nodeHash.TryGetValue(dataObject, out obj))
			{
				this.nodeHash[dataObject] = it;
				if (chain == null)
				{
					chain = this.pad.GetBuilderChain(dataObject.GetType());
				}
				if (fireAddedEvent)
				{
					foreach (NodeBuilder nodeBuilder in chain)
					{
						nodeBuilder.OnNodeAdded(dataObject);
					}
					return;
				}
			}
			else
			{
				if (obj is TreeIter[])
				{
					TreeIter[] array2 = (TreeIter[])obj;
					TreeIter[] array3 = new TreeIter[array2.Length + 1];
					array2.CopyTo(array3, 0);
					array3[array2.Length] = it;
					this.nodeHash[dataObject] = array3;
					return;
				}
				this.nodeHash[dataObject] = new TreeIter[]
				{
					it,
					(TreeIter)obj
				};
			}
		}

		internal void UnregisterNode(object dataObject, TreeIter iter, NodeBuilder[] chain, bool fireRemovedEvent)
		{
			object obj;
			this.nodeHash.TryGetValue(dataObject, out obj);
			if (obj is TreeIter[])
			{
				TreeIter[] array = (TreeIter[])obj;
				TreePath treePath = null;
				List<TreeIter> list = new List<TreeIter>();
				if (this.store.IterIsValid(iter))
				{
					treePath = this.store.GetPath(iter);
				}
				foreach (TreeIter treeIter in array)
				{
					if (this.store.IterIsValid(treeIter) && (treePath == null || !treePath.Equals(this.store.GetPath(treeIter))))
					{
						list.Add(treeIter);
					}
				}
				if (list.Count > 1)
				{
					this.nodeHash[dataObject] = list.ToArray();
				}
				else if (list.Count == 1)
				{
					this.nodeHash[dataObject] = list[0];
				}
				else
				{
					this.nodeHash.Remove(dataObject);
				}
			}
			else
			{
				this.nodeHash.Remove(dataObject);
			}
			if (fireRemovedEvent)
			{
				this.NotifyNodeRemoved(dataObject, chain);
			}
		}

		private void NotifyNodeRemoved(object dataObject, NodeBuilder[] chain)
		{
			if (chain == null)
			{
				chain = this.pad.GetBuilderChain(dataObject.GetType());
			}
			foreach (NodeBuilder nodeBuilder in chain)
			{
				try
				{
					nodeBuilder.OnNodeRemoved(dataObject);
					nodeBuilder.Dispose();
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("Node removed error.", exception);
				}
			}
		}

		private object GetStoreValue(int column)
		{
			if (this.store.IterIsValid(this.currentIter))
			{
				return this.tree.CurrentModel.GetValue(this.currentIter, column);
			}
			return null;
		}

		internal void MoveToIter(TreeIter iter, bool isSelected = false)
		{
			this.currentIter = iter;
			if (!iter.Equals(TreeIter.Zero))
			{
				this.dataItem = this.GetStoreValue(0);
				if (this.store.IterHasChild(iter) && isSelected)
				{
					TreePath path = this.store.GetPath(iter);
					this.tree.SetCursor(path, this.pad.Completecolumn, false);
					return;
				}
			}
			else
			{
				this.dataItem = null;
			}
		}

		public bool MoveToObject(object dataObject)
		{
			TreeIter iter;
			if (!this.GetFirstNode(dataObject, out iter))
			{
				return false;
			}
			this.MoveToIter(iter, false);
			return true;
		}

		private void InitIter(TreeIter it, object dataObject)
		{
			this.currentIter = it;
			this.dataItem = dataObject;
		}

		public void FillNode(object fillObject)
		{
			if (this.nodeHash.ContainsKey(fillObject))
			{
				this.Update(fillObject);
			}
			else
			{
				ResourceItem parent = ((ResourceItem)fillObject).Parent;
				TreeIter iter;
				this.GetFirstNode(parent, out iter);
				this.AddChildToTreeIter(fillObject, iter, false);
			}
			if (fillObject is ResourceFolder)
			{
				ResourceFolder resourceFolder = fillObject as ResourceFolder;
				foreach (ResourceItem fillObject2 in resourceFolder.Items)
				{
					this.FillNode(fillObject2);
				}
			}
		}

		private void CreateChildren(NodeBuilder[] chain, object dataObject)
		{
			TreeIter iter = this.currentIter;
			foreach (NodeBuilder nodeBuilder in chain)
			{
				try
				{
					nodeBuilder.BuildChildNodes(this, dataObject);
				}
				catch (Exception value)
				{
					Console.WriteLine(value);
				}
				this.MoveToIter(iter, false);
			}
		}

		private void ExpandToObject(object dataObjct, bool isSelected = true)
		{
			TreeIter iter;
			this.GetFirstNode(dataObjct, out iter);
			if (this.store.IterIsValid(iter))
			{
				TreePath path = this.store.GetPath(iter);
				this.tree.ExpandToPath(path);
				if (isSelected)
				{
					this.tree.SetCursor(path, this.pad.Completecolumn, false);
				}
			}
		}

		public object GetFristSelecteValue(int colum, out TreeIter curiter)
		{
			TreePath[] selectedRows = this.tree.Selection.GetSelectedRows();
			if (selectedRows != null && selectedRows.Length > 0 && this.store != null)
			{
				TreeIter treeIter;
				this.tree.CurrentModel.GetIter(out treeIter, selectedRows[0]);
				object value = this.tree.CurrentModel.GetValue(treeIter, colum);
				curiter = treeIter;
				return value;
			}
			curiter = TreeIter.Zero;
			return null;
		}

		public object GetFristSelecteValue(int colum)
		{
			TreePath[] selectedRows = this.tree.Selection.GetSelectedRows();
			if (selectedRows != null && selectedRows.Length > 0 && this.store != null)
			{
				TreeIter iter;
				this.tree.CurrentModel.GetIter(out iter, selectedRows[0]);
				return this.tree.CurrentModel.GetValue(iter, colum);
			}
			return null;
		}

		public ExtendTreeView tree { get; set; }

		public List<ResourceItem> GetCurrentSelectes()
		{
			TreePath[] selectedRows = this.tree.Selection.GetSelectedRows();
			if (selectedRows != null && selectedRows.Length != 0)
			{
				List<ResourceItem> list = new List<ResourceItem>();
				foreach (TreePath path in selectedRows)
				{
					TreeIter iter;
					this.tree.CurrentModel.GetIter(out iter, path);
					object dateItemByIter = this.GetDateItemByIter(iter);
					ResourceItem resourceItem = dateItemByIter as ResourceItem;
					if (dateItemByIter is Solution)
					{
						resourceItem = ((Solution)dateItemByIter).GetRootFolder();
					}
					if (resourceItem != null)
					{
						list.Add(resourceItem);
					}
				}
				return list;
			}
			return null;
		}

		public void SetSelecteResources(IEnumerable<ResourceItem> seletes)
		{
			if (seletes == null)
			{
				return;
			}
			this.tree.Selection.UnselectAll();
			List<TreePath> list = new List<TreePath>();
			foreach (ResourceItem key in seletes)
			{
				object selectionKey = key;
				if (Services.ProjectsService.CurrentResourceGroup != null && key == Services.ProjectsService.CurrentResourceGroup.RootFolder)
				{
					selectionKey = Services.ProjectsService.CurrentSolution;
				}
				object obj;
				if (this.nodeHash.ContainsKey(selectionKey) && this.nodeHash.TryGetValue(selectionKey, out obj))
				{
					TreeIter[] candidateIters = obj is TreeIter ? new TreeIter[]
					{
						(TreeIter)obj
					} : (TreeIter[])obj;
					foreach (TreeIter treeIter in candidateIters)
					{
						TreePath path = this.store.GetPath(treeIter);
						if (this.tree.IsSearchState && this.tree.Filter != null)
						{
							path = this.tree.Filter.ConvertChildPathToPath(path);
							if (path == null)
							{
								continue;
							}
						}
						list.Add(path);
						if (!this.tree.IsSearchState)
						{
							TreeIter zero = TreeIter.Zero;
							if (this.store.IterParent(out zero, treeIter))
							{
								TreePath path2 = this.store.GetPath(zero);
								this.tree.ExpandToPath(path2);
							}
						}
						break;
					}
				}
			}
			this.tree.SetSelectes(list);
		}

		private static TargetEntry[] target_tableMac = new TargetEntry[]
		{
			DragTargetType.FileDropTarget,
			DragTargetType.CocoStudioTarget
		};

		private static TargetEntry[] target_tableWindows = new TargetEntry[]
		{
			DragTargetType.FileDropTarget,
			DragTargetType.CocoStudioTarget
		};

		private ResourceItem lastHoreItem;

		private ResourceTreeView pad;

		private TreeStore store;

		private NodeHashtable nodeHash = new NodeHashtable();

		private TreeIter currentIter;

		private object dataItem;
	}
}
