using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CocoStudio.Basic;
using CocoStudio.ControlLib;
using CocoStudio.Core.Events;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Projects;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace CocoStudio.Core
{
	// Token: 0x0200002A RID: 42
	internal static class ImportFileService
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00007084 File Offset: 0x00005284
		private static ResourceFolder RootFolder
		{
			get
			{
				return Services.ProjectOperations.CurrentResourceGroup.RootFolder;
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000074CC File Offset: 0x000056CC
		internal static async Task<List<ResourceItem>> MainProcess(ResourceFolder parentDic, IEnumerable<string> paths, IProgressMonitor monitor)
		{
			CSCocosHelp.StopAllEffects();
			List<ResourceItem> result = new List<ResourceItem>();
			int workCount = await ImportFileService.GetPathsFileSystemCount(paths);
			monitor.BeginTask(LanguageInfo.Menu_File_ImportFile, workCount * 3);
			SortedSet<FileCopyInfo> allImportFiles = await ImportFileService.GetCopyFilesAsync(parentDic.BaseDirectory, paths, monitor);
			List<FileCopyInfo> resourceFiles = await ImportFileService.CopyFilesAsync(allImportFiles, monitor);
			List<ResourceItem> resources = await ImportFileService.CreateResourceItemsAsync(parentDic, resourceFiles, monitor);
			IEnumerable<FilePath> importFiles = from s in resourceFiles
			where paths.Contains(s.SourcePath.ToString())
			select s.TargetPath;
			AddResourcesArgs args = new AddResourcesArgs(parentDic, resources, false);
			Services.EventsService.GetEvent<AddResourcesEvent>().Publish(args);
			if (resources != null)
			{
				result = (from n in resources
				where importFiles.Contains(n.FullPath)
				select n).ToList<ResourceItem>();
			}
			return result;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000075C0 File Offset: 0x000057C0
		internal static Task<int> GetPathsFileSystemCount(IEnumerable<string> files)
		{
			return Task.Run<int>(delegate()
			{
				int num = 0;
				try
				{
					foreach (string dirPath in files)
					{
						num += ImportFileService.GetDirFileCount(dirPath);
					}
				}
				catch (Exception message)
				{
					LogConfig.Logger.Error(message);
				}
				return num;
			});
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000075F0 File Offset: 0x000057F0
		internal static int GetDirFileCount(string dirPath)
		{
			int num = 0;
			int result;
			if (!Directory.Exists(dirPath))
			{
				result = 0;
			}
			else
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
				FileInfo[] files = directoryInfo.GetFiles();
				num += files.Count<FileInfo>();
				DirectoryInfo[] directories = directoryInfo.GetDirectories();
				if (directories != null && directories.Count<DirectoryInfo>() != 0)
				{
					foreach (DirectoryInfo directoryInfo2 in directories)
					{
						if (!directoryInfo2.Attributes.HasFlag(FileAttributes.Hidden))
						{
							num += ImportFileService.GetDirFileCount(directoryInfo2.FullName);
						}
					}
				}
				result = num;
			}
			return result;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00007810 File Offset: 0x00005A10
		private static Task<SortedSet<FileCopyInfo>> GetCopyFilesAsync(FilePath parentDir, IEnumerable<string> paths, IProgressMonitor monitor)
		{
			return Task.Run<SortedSet<FileCopyInfo>>(delegate()
			{
				SortedSet<FileCopyInfo> sortedSet = new SortedSet<FileCopyInfo>();
				try
				{
					foreach (string name in paths)
					{
						FilePath filePath = name;
						if (monitor.IsCancelRequested)
						{
							return sortedSet;
						}
						FileCopyInfo fileCopyInfo = new FileCopyInfo(parentDir, filePath);
						if (fileCopyInfo.VerifyPath(monitor))
						{
							if (parentDir.IsChildPathOf(filePath))
							{
								string message = string.Format(LanguageInfo.Output_TagIsSourceChild, filePath);
								monitor.ReportWarning(message);
							}
							else
							{
								sortedSet.Add(fileCopyInfo);
								if (filePath.IsDirectory)
								{
									SortedSet<FileCopyInfo> copyFileByDir = ImportFileService.GetCopyFileByDir(parentDir, filePath, monitor);
									sortedSet.AddRange(copyFileByDir);
								}
							}
						}
					}
				}
				catch (Exception message2)
				{
					monitor.ReportError(LanguageInfo.Dialog_Import_Failed + "get files failure", null);
					LogConfig.Logger.Error(message2);
				}
				return sortedSet;
			});
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00007A0C File Offset: 0x00005C0C
		private static Task<List<FileCopyInfo>> CopyFilesAsync(IEnumerable<FileCopyInfo> files, IProgressMonitor monitor)
		{
			List<string> filterList = new List<string>();
			ConcurrentQueue<FileCopyInfo> copyQueue = new ConcurrentQueue<FileCopyInfo>();
			List<FileCopyInfo> files2 = files.Where(delegate(FileCopyInfo n)
			{
				if (n.PairResources != null && n.IsHiddenCompositeFile)
				{
					filterList.AddRange(n.PairResources);
				}
				if (!n.Exists)
				{
					copyQueue.Enqueue(n);
				}
				return n.Exists;
			}).ToList<FileCopyInfo>();
			bool flag = true;
			AutoResetEvent autoReset = new AutoResetEvent(false);
			Task<List<FileCopyInfo>> result = Task.Run<List<FileCopyInfo>>(delegate()
			{
				List<FileCopyInfo> list = new List<FileCopyInfo>();
				try
				{
					bool flag = true;
					int count = copyQueue.Count;
					int num = 0;
					while (flag)
					{
						num++;
						if (monitor.IsCancelRequested)
						{
							return list;
						}
						if (copyQueue.Count == 0 && flag)
						{
							autoReset.WaitOne();
						}
						flag = (copyQueue.Count > 0);
						FileCopyInfo fileCopyInfo;
						if (copyQueue.TryDequeue(out fileCopyInfo))
						{
							monitor.Step(1);
							if (!filterList.Contains(fileCopyInfo.SourcePath, StringComparer.InvariantCultureIgnoreCase))
							{
								bool flag2 = fileCopyInfo.Copy(monitor);
								if (flag2)
								{
									list.Add(fileCopyInfo);
								}
							}
							else
							{
								monitor.Step(2);
							}
						}
					}
				}
				catch (Exception message)
				{
					monitor.ReportError(LanguageInfo.Dialog_Import_Failed + "copy files failure", null);
					LogConfig.Logger.Error(message);
				}
				return list;
			});
			ImportFileService.SetFileOperate(files2, copyQueue);
			autoReset.Set();
			flag = false;
			return result;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00007BB4 File Offset: 0x00005DB4
		private static Task<List<ResourceItem>> CreateResourceItemsAsync(ResourceFolder root, IEnumerable<FileCopyInfo> files, IProgressMonitor monitor)
		{
			return Task.Run<List<ResourceItem>>(delegate()
			{
				List<ResourceItem> result = null;
				Dictionary<string, ResourceItem> dictionary = new Dictionary<string, ResourceItem>();
				foreach (FileCopyInfo fileCopyInfo in files)
				{
					monitor.Step(1);
					try
					{
						if (monitor.IsCancelRequested)
						{
							return result;
						}
						ImportFileService.CreateItem(root, fileCopyInfo.TargetPath, monitor, dictionary);
					}
					catch (Exception exception)
					{
						monitor.ReportError(string.Format(LanguageInfo.Output_ImportFailed, fileCopyInfo.SourcePath), exception);
						LogConfig.Logger.Error("File to import failure", exception);
					}
				}
				result = dictionary.Values.ToList<ResourceItem>();
				return result;
			});
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00007BF4 File Offset: 0x00005DF4
		private static ResourceItem CreateItem(ResourceFolder root, FilePath fullPath, IProgressMonitor monitor, Dictionary<string, ResourceItem> outResources)
		{
			if (outResources == null)
			{
				outResources = new Dictionary<string, ResourceItem>();
			}
			ResourceFolder resourceFolder;
			if (fullPath.ParentDirectory == root.BaseDirectory)
			{
				resourceFolder = root;
			}
			else if (outResources.ContainsKey(fullPath.ParentDirectory))
			{
				resourceFolder = (outResources[fullPath.ParentDirectory] as ResourceFolder);
			}
			else
			{
				resourceFolder = ImportFileService.FindResourceItem<ResourceFolder>(root, fullPath.ParentDirectory);
			}
			if (resourceFolder == null)
			{
				resourceFolder = (ImportFileService.CreateItem(root, fullPath.ParentDirectory, monitor, outResources) as ResourceFolder);
			}
			ResourceItem resourceItem = ImportFileService.FindResourceItem<ResourceItem>(resourceFolder, fullPath);
			if (resourceItem == null)
			{
				resourceItem = ImportFileService.CreateResourceItemByPath(monitor, resourceFolder, fullPath);
			}
			if (!outResources.ContainsKey(fullPath))
			{
				outResources.Add(fullPath, resourceItem);
			}
			return resourceItem;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00007CEC File Offset: 0x00005EEC
		private static ResourceItem CreateResourceItemByPath(IProgressMonitor monitor, ResourceFolder parent, string itemFileName)
		{
			ResourceItem resourceItem = Services.ProjectsService.ReadResourceItem(monitor, itemFileName);
			if (resourceItem is IInitialize)
			{
				((IInitialize)resourceItem).Initialize(Services.ProgressMonitors.Default);
			}
			parent.Items.Add(resourceItem);
			return resourceItem;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00007D44 File Offset: 0x00005F44
		private static T FindResourceItem<T>(ResourceItem parent, string fullPath) where T : ResourceItem
		{
			return Services.ProjectOperations.CurrentResourceGroup.FindResourceItem(parent, fullPath) as T;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00007DA4 File Offset: 0x00005FA4
		internal static ResourceItem AddResourceItem(ResourceFolder parentResourceItem, FilePath itemFileName, IProgressMonitor monitor, out ResourceItem importRoot)
		{
			Stack<string> stack = ImportFileService.CreateParentStack(itemFileName, parentResourceItem.BaseDirectory);
			ResourceItem resourceItem = null;
			ResourceFolder resourceFolder = parentResourceItem;
			importRoot = null;
			while (stack.Count > 0)
			{
				if (resourceFolder == null)
				{
					resourceFolder = parentResourceItem;
				}
				FilePath filePath = resourceFolder.FullPath;
				string name = stack.Pop();
				ResourceItem resourceItem2 = resourceFolder.Items.FirstOrDefault((ResourceItem n) => n.Name == name);
				if (resourceItem2 == null)
				{
					resourceItem = Services.ProjectsService.ReadResourceItem(monitor, filePath.Combine(new string[]
					{
						name
					}));
					if (resourceItem is ICocosFile)
					{
						((ICocosFile)resourceItem).Initialize(monitor);
					}
					resourceFolder.Items.Add(resourceItem);
					resourceFolder = (resourceItem as ResourceFolder);
					if (importRoot == null)
					{
						importRoot = resourceItem;
					}
				}
				else
				{
					if (resourceItem2.FullPath == itemFileName)
					{
						return resourceItem2;
					}
					resourceFolder = (resourceItem2 as ResourceFolder);
				}
				if (resourceItem == null || !(resourceItem.FullPath == itemFileName))
				{
					continue;
				}
				return resourceItem;
			}
			return resourceItem;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00007F08 File Offset: 0x00006108
		private static Stack<string> CreateParentStack(FilePath importPath, FilePath parent)
		{
			Stack<string> stack = new Stack<string>();
			while (importPath != parent)
			{
				stack.Push(importPath.FileName);
				importPath = importPath.ParentDirectory;
			}
			return stack;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00007F4C File Offset: 0x0000614C
		private static void SetFileOperate(IEnumerable<FileCopyInfo> files, ConcurrentQueue<FileCopyInfo> copyQueue)
		{
			if (files != null && files.Count<FileCopyInfo>() != 0)
			{
				DialogResult dialogResult = new DialogResult();
				foreach (FileCopyInfo fileCopyInfo in files)
				{
					if (fileCopyInfo.SourcePath.IsChildPathOf(ImportFileService.RootFolder.BaseDirectory))
					{
						fileCopyInfo.Operate = EFileOperate.Skip;
						copyQueue.Enqueue(fileCopyInfo);
					}
					else
					{
						if (!dialogResult.IsChangedAll)
						{
							ImportFileDialog importFileDialog = new ImportFileDialog(Services.MainWindow, true);
							importFileDialog.RefreshMessage(fileCopyInfo.SourcePath.FileName, !fileCopyInfo.IsComposite);
							dialogResult = importFileDialog.ShowRun();
						}
						fileCopyInfo.Operate = dialogResult.ButtonResult;
						copyQueue.Enqueue(fileCopyInfo);
					}
				}
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00008044 File Offset: 0x00006244
		private static SortedSet<FileCopyInfo> GetCopyFileByDir(FilePath parentDir, FilePath dir, IProgressMonitor monitor)
		{
			SortedSet<FileCopyInfo> sortedSet = new SortedSet<FileCopyInfo>();
			parentDir = parentDir.Combine(new string[]
			{
				dir.FileName
			});
			string[] files = Directory.GetFiles(dir);
			string[] directories = Directory.GetDirectories(dir);
			if (files != null)
			{
				foreach (string text in files)
				{
					if (monitor.IsCancelRequested)
					{
						return sortedSet;
					}
					FileCopyInfo fileCopyInfo = new FileCopyInfo(parentDir, text);
					if (fileCopyInfo.VerifyPath(monitor))
					{
						sortedSet.Add(fileCopyInfo);
					}
				}
			}
			if (directories != null)
			{
				foreach (string text in directories)
				{
					if (monitor.IsCancelRequested)
					{
						return sortedSet;
					}
					FileCopyInfo fileCopyInfo = new FileCopyInfo(parentDir, text);
					if (fileCopyInfo.VerifyPath(monitor) && !new DirectoryInfo(text).Attributes.HasFlag(FileAttributes.Hidden))
					{
						SortedSet<FileCopyInfo> copyFileByDir = ImportFileService.GetCopyFileByDir(parentDir, text, monitor);
						sortedSet.Add(fileCopyInfo);
						sortedSet.AddRange(copyFileByDir);
					}
				}
			}
			return sortedSet;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000081BC File Offset: 0x000063BC
		public static void AddRange<T>(this SortedSet<T> set, IEnumerable<T> list)
		{
			if (list != null)
			{
				foreach (T item in list)
				{
					set.Add(item);
				}
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00008220 File Offset: 0x00006420
		public static bool IsValidPath(this FilePath path)
		{
			bool flag = FileService.IsValidPath(path);
			if (!flag && Platform.IsMac)
			{
				if (path.ToString().IndexOf(":") > 0 || path.ToString().IndexOf("\\") > 0)
				{
					return false;
				}
			}
			return flag;
		}
	}
}
