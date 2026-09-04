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
	// Token: 0x02000029 RID: 41
	public class ResourceTreeBuilder : ITreeBuild
	{
		// Token: 0x0600010E RID: 270 RVA: 0x0000502C File Offset: 0x0000322C
		private void InitiaCommand()
		{
			GlobalCommand.ImportFileCmd.Execute += this.ImportResourceFileCmd_Execute;
			GlobalCommand.ImportDirCmd.Execute += this.ImportResourceFolderCmd_Execute;
			GlobalCommand.NewFileCmd.Execute += this.NewFileCmd_Execute;
			ResourceMenu.InitMenu();
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005060 File Offset: 0x00003260
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

		// Token: 0x06000110 RID: 272 RVA: 0x000051F4 File Offset: 0x000033F4
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

		// Token: 0x06000111 RID: 273 RVA: 0x000053A4 File Offset: 0x000035A4
		internal async void ImportResources(string[] selectPath, ResourceFolder folder)
		{
			List<ResourceItem> importItems = await Services.ProjectOperations.ImportResourcesAsync(folder, selectPath, null);
			if (importItems != null && importItems.Count > 0)
			{
				this.SetSelecteResources(importItems);
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000053F0 File Offset: 0x000035F0
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

		// Token: 0x06000113 RID: 275 RVA: 0x000054D0 File Offset: 0x000036D0
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

		// Token: 0x06000114 RID: 276 RVA: 0x000055FC File Offset: 0x000037FC
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

		// Token: 0x06000115 RID: 277 RVA: 0x00005928 File Offset: 0x00003B28
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

		// Token: 0x06000116 RID: 278 RVA: 0x00005A68 File Offset: 0x00003C68
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

		// Token: 0x06000117 RID: 279 RVA: 0x00005B30 File Offset: 0x00003D30
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

		// Token: 0x06000118 RID: 280 RVA: 0x00005C4E File Offset: 0x00003E4E
		[ConnectBefore]
		private void tree_DragEnd(object o, DragEndArgs args)
		{
			this.lastHoreItem = null;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005D78 File Offset: 0x00003F78
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

		// Token: 0x0600011A RID: 282 RVA: 0x00005DBC File Offset: 0x00003FBC
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

		// Token: 0x0600011B RID: 283 RVA: 0x00005EF8 File Offset: 0x000040F8
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

		// Token: 0x0600011C RID: 284 RVA: 0x00005F46 File Offset: 0x00004146
		private void ProjectOperations_CurrentSelectedSolutionChanged(object sender, SolutionEventArgs e)
		{
			if (e.Solution != null)
			{
				this.pad.LoadTree(e.Solution);
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005F62 File Offset: 0x00004162
		public void AllDrag()
		{
			Gtk.Drag.SourceSet(this.tree, ModifierType.Button1Mask, ResourceTreeBuilder.target_tableWindows, DragAction.Copy | DragAction.Move | DragAction.Link);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005F7B File Offset: 0x0000417B
		public void AllDrop()
		{
			this.tree.EnableModelDragDest(ResourceTreeBuilder.target_tableWindows, DragAction.Copy | DragAction.Move | DragAction.Link);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005F90 File Offset: 0x00004190
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

		// Token: 0x06000120 RID: 288 RVA: 0x00006000 File Offset: 0x00004200
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

		// Token: 0x06000121 RID: 289 RVA: 0x00006054 File Offset: 0x00004254
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

		// Token: 0x06000122 RID: 290 RVA: 0x00006104 File Offset: 0x00004304
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

		// Token: 0x06000123 RID: 291 RVA: 0x00006148 File Offset: 0x00004348
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

		// Token: 0x06000124 RID: 292 RVA: 0x00006290 File Offset: 0x00004490
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

		// Token: 0x06000125 RID: 293 RVA: 0x000062E8 File Offset: 0x000044E8
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

		// Token: 0x06000126 RID: 294 RVA: 0x00006488 File Offset: 0x00004688
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

		// Token: 0x06000127 RID: 295 RVA: 0x00006584 File Offset: 0x00004784
		private bool CheckChildPath(FilePath parent, FilePath childer)
		{
			return !(parent == childer) && childer.IsChildPathOf(parent);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x000065A0 File Offset: 0x000047A0
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

		// Token: 0x06000129 RID: 297 RVA: 0x00006618 File Offset: 0x00004818
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

		// Token: 0x0600012A RID: 298 RVA: 0x000066C4 File Offset: 0x000048C4
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

		// Token: 0x0600012B RID: 299 RVA: 0x00006710 File Offset: 0x00004910
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

		// Token: 0x0600012C RID: 300 RVA: 0x00006744 File Offset: 0x00004944
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

		// Token: 0x0600012D RID: 301 RVA: 0x000067CC File Offset: 0x000049CC
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

		// Token: 0x0600012E RID: 302 RVA: 0x00006848 File Offset: 0x00004A48
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

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600012F RID: 303 RVA: 0x000068EA File Offset: 0x00004AEA
		// (set) Token: 0x06000130 RID: 304 RVA: 0x000068F2 File Offset: 0x00004AF2
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

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000131 RID: 305 RVA: 0x000068FB File Offset: 0x00004AFB
		public object DataItem
		{
			get
			{
				return this.dataItem;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00006903 File Offset: 0x00004B03
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00006924 File Offset: 0x00004B24
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

		// Token: 0x06000134 RID: 308 RVA: 0x000069B3 File Offset: 0x00004BB3
		public ResourceTreeBuilder(ResourceTreeView pad) : this(pad, TreeIter.Zero)
		{
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000069C4 File Offset: 0x00004BC4
		public ResourceTreeBuilder(ResourceTreeView pad, TreeIter iter)
		{
			this.pad = pad;
			this.tree = pad.Tree;
			this.store = pad.Store;
			this.InitiaCommand();
			this.InitEvent();
			this.MoveToIter(iter, false);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00006A18 File Offset: 0x00004C18
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

		// Token: 0x06000137 RID: 311 RVA: 0x00006A9C File Offset: 0x00004C9C
		public void Update()
		{
			NodeInfo nodeInfoByIter = this.GetNodeInfoByIter(this.currentIter);
			if (nodeInfoByIter == null)
			{
				return;
			}
			this.UpdateNode(nodeInfoByIter.BuilderChain, nodeInfoByIter.DataItem);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00006ACC File Offset: 0x00004CCC
		public void UpdateChildren()
		{
			this.FillNode(null);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00006AD8 File Offset: 0x00004CD8
		internal NodeInfo GetNodeInfoByIter(TreeIter iter)
		{
			return this.tree.CurrentModel.GetValue(iter, 0) as NodeInfo;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00006B00 File Offset: 0x00004D00
		internal object GetDateItemByIter(TreeIter iter)
		{
			NodeInfo nodeInfoByIter = this.GetNodeInfoByIter(iter);
			if (nodeInfoByIter != null)
			{
				return nodeInfoByIter.DataItem;
			}
			return null;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00006B20 File Offset: 0x00004D20
		internal NodeBuilder[] GetBuilderByIter(TreeIter iter)
		{
			NodeInfo nodeInfoByIter = this.GetNodeInfoByIter(iter);
			if (nodeInfoByIter != null)
			{
				return nodeInfoByIter.BuilderChain;
			}
			return null;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00006B40 File Offset: 0x00004D40
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

		// Token: 0x0600013D RID: 317 RVA: 0x00006BBC File Offset: 0x00004DBC
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

		// Token: 0x0600013E RID: 318 RVA: 0x00006C5A File Offset: 0x00004E5A
		public void Remove(object dataObject)
		{
			this.MoveToObject(dataObject);
			this.Remove();
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00006C6C File Offset: 0x00004E6C
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

		// Token: 0x06000140 RID: 320 RVA: 0x00006CB5 File Offset: 0x00004EB5
		public void AddChild(object dataObject)
		{
			this.AddChild(dataObject, false);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00006CC0 File Offset: 0x00004EC0
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

		// Token: 0x06000142 RID: 322 RVA: 0x00006D10 File Offset: 0x00004F10
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

		// Token: 0x06000143 RID: 323 RVA: 0x00006E13 File Offset: 0x00005013
		public void AddChildToRoot(object dataObject)
		{
			this.currentIter = TreeIter.Zero;
			this.AddChild(dataObject, false);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00006E28 File Offset: 0x00005028
		public void AddChildren(IEnumerable dataObjects)
		{
			this.pad.Tree.FreezeChildNotify();
			foreach (object dataObject in dataObjects)
			{
				this.AddChild(dataObject);
			}
			this.pad.Tree.ThawChildNotify();
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00006E98 File Offset: 0x00005098
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

		// Token: 0x06000146 RID: 326 RVA: 0x00006F98 File Offset: 0x00005198
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

		// Token: 0x06000147 RID: 327 RVA: 0x00007014 File Offset: 0x00005214
		private void BuildNode(TreeIter it, NodeBuilder[] chain, object dataObject)
		{
			TreeIter it2 = this.currentIter;
			this.InitIter(it, dataObject);
			this.UpdateNode(chain, dataObject);
			this.CreateChildren(chain, dataObject);
			this.InitIter(it2, this.dataItem);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00007050 File Offset: 0x00005250
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

		// Token: 0x06000149 RID: 329 RVA: 0x000070B0 File Offset: 0x000052B0
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

		// Token: 0x0600014A RID: 330 RVA: 0x00007110 File Offset: 0x00005310
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

		// Token: 0x0600014B RID: 331 RVA: 0x00007162 File Offset: 0x00005362
		private void SetNodeInfo(TreeIter it, NodeInfo nodeInfo)
		{
			this.store.SetValue(it, 0, nodeInfo);
			if (this.tree.IsSearchState)
			{
				this.tree.Filter.Refilter();
			}
			this.pad.Tree.QueueDraw();
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000071A0 File Offset: 0x000053A0
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

		// Token: 0x0600014D RID: 333 RVA: 0x000071FC File Offset: 0x000053FC
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

		// Token: 0x0600014E RID: 334 RVA: 0x000072E4 File Offset: 0x000054E4
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

		// Token: 0x0600014F RID: 335 RVA: 0x000073F4 File Offset: 0x000055F4
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

		// Token: 0x06000150 RID: 336 RVA: 0x00007460 File Offset: 0x00005660
		private object GetStoreValue(int column)
		{
			if (this.store.IterIsValid(this.currentIter))
			{
				return this.tree.CurrentModel.GetValue(this.currentIter, column);
			}
			return null;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00007490 File Offset: 0x00005690
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

		// Token: 0x06000152 RID: 338 RVA: 0x00007508 File Offset: 0x00005708
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

		// Token: 0x06000153 RID: 339 RVA: 0x0000752B File Offset: 0x0000572B
		private void InitIter(TreeIter it, object dataObject)
		{
			this.currentIter = it;
			this.dataItem = dataObject;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000753C File Offset: 0x0000573C
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

		// Token: 0x06000155 RID: 341 RVA: 0x000075D4 File Offset: 0x000057D4
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

		// Token: 0x06000156 RID: 342 RVA: 0x0000762C File Offset: 0x0000582C
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

		// Token: 0x06000157 RID: 343 RVA: 0x00007688 File Offset: 0x00005888
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

		// Token: 0x06000158 RID: 344 RVA: 0x000076F8 File Offset: 0x000058F8
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

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00007753 File Offset: 0x00005953
		// (set) Token: 0x0600015A RID: 346 RVA: 0x0000775B File Offset: 0x0000595B
		public ExtendTreeView tree { get; set; }

		// Token: 0x0600015B RID: 347 RVA: 0x00007764 File Offset: 0x00005964
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

		// Token: 0x0600015C RID: 348 RVA: 0x000077F8 File Offset: 0x000059F8
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

		// Token: 0x04000053 RID: 83
		private static TargetEntry[] target_tableMac = new TargetEntry[]
		{
			DragTargetType.FileDropTarget,
			DragTargetType.CocoStudioTarget
		};

		// Token: 0x04000054 RID: 84
		private static TargetEntry[] target_tableWindows = new TargetEntry[]
		{
			DragTargetType.FileDropTarget,
			DragTargetType.CocoStudioTarget
		};

		// Token: 0x04000055 RID: 85
		private ResourceItem lastHoreItem;

		// Token: 0x04000056 RID: 86
		private ResourceTreeView pad;

		// Token: 0x04000057 RID: 87
		private TreeStore store;

		// Token: 0x04000058 RID: 88
		private NodeHashtable nodeHash = new NodeHashtable();

		// Token: 0x04000059 RID: 89
		private TreeIter currentIter;

		// Token: 0x0400005A RID: 90
		private object dataItem;
	}
}
