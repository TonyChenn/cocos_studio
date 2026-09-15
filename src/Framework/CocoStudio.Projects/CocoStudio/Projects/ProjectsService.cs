using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CocoStudio.Basic;
using CocoStudio.Model;
using CocoStudio.Projects.Formates;
using CocoStudio.UserStatistics;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	public class ProjectsService
	{
		internal ResourceChangeService ResourceChangeService
		{
			get
			{
				return ResourceChangeService.Instance;
			}
		}

		internal FileFormatManager FormatManager { get; private set; }

		internal CocosItemBindingManager ProjectBindingManager { get; private set; }

		internal ProcesserManager ProcesserManager { get; private set; }

		internal PreviewImageService PreviewImageService { get; private set; }

		public DataModelManager DataModelManager { get; private set; }

		public ISerializeManager SerializeManager { get; private set; }

		public Solution CurrentSolution
		{
			get
			{
				return this.currentSolution;
			}
			set
			{
				if (this.currentSolution != value)
				{
					this.currentSolution = value;
					this.OnCurrentSolutionChanged(value);
				}
			}
		}

		public ResourceGroup CurrentResourceGroup
		{
			get
			{
				return this.GetResourceGroup(this.CurrentSolution);
			}
		}

		public List<ResourceItem> CurrentResourceItems { get; internal set; }

		public IProgressMonitor DefaultMonitor
		{
			get
			{
				return new ConsoleProgressFullExceptionMonitor(true, true);
			}
		}

		public static ProjectsService Instance
		{
			get
			{
				if (ProjectsService.instance == null)
				{
					ProjectsService.instance = new ProjectsService();
				}
				return ProjectsService.instance;
			}
		}

		private ProjectsService()
		{
			this.FormatManager = new FileFormatManager();
			this.ProjectBindingManager = new CocosItemBindingManager();
			this.ProcesserManager = new ProcesserManager();
			this.SerializeManager = new SerializeManager();
			this.PreviewImageService = new PreviewImageService();
			this.DataModelManager = new DataModelManager();
			FrameworkHelper.Initialize();
		}

		private void OnCurrentSolutionChanged(Solution value)
		{
			this.PreviewImageService.Clear();
		}

		public Solution GetWrapperSolution(IProgressMonitor monitor, string filename)
		{
			Solution result;
			try
			{
				List<FileFormat> fileFormats = this.FormatManager.GetFileFormats(filename, typeof(Solution));
				if (fileFormats.Count == 0)
				{
					Solution solution = new Solution();
					ProjectsService.SetSolutionLocation(filename, solution);
					CocosItem item = this.ReadCocosItem(monitor, filename);
					ResourceGroup resourceGroup = new ResourceGroup(solution);
					resourceGroup.RootFolder.Items.Add(item);
					solution.RootFolder.Items.Add(resourceGroup);
					result = solution;
				}
				else
				{
					Solution solution = this.ReadWorkspaceItem(monitor, filename);
					ProjectsService.SetSolutionLocation(filename, solution);
					result = solution;
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error(string.Format("使用路径{0}获取解决方案时出错", filename), exception);
				result = null;
			}
			return result;
		}

		public ResourceItem ReadResourceItem(IProgressMonitor monitor, string itemFileName)
		{
			Type typeFromHandle = typeof(ResourceItem);
			List<FileFormat> fileFormats = this.FormatManager.GetFileFormats(itemFileName, typeof(ResourceItem));
			IFileFormat format;
			if (fileFormats != null && fileFormats.Count > 0)
			{
				format = fileFormats[0];
			}
			else
			{
				format = this.FormatManager.GetDefaultResourceFormat();
			}
			ResourceItem resourceItem = this.ReadFile(monitor, itemFileName, typeFromHandle, format) as ResourceItem;
			if (resourceItem == null)
			{
				throw new InvalidOperationException("Invalid file format: " + itemFileName);
			}
			return resourceItem;
		}

		public CocosItem ReadCocosItem(IProgressMonitor monitor, string filename)
		{
			IFileFormat fileFormat;
			return this.ReadFile(monitor, filename, typeof(CocosItem), out fileFormat) as CocosItem;
		}

		internal Solution ReadWorkspaceItem(IProgressMonitor monitor, string filename)
		{
			return this.ReadSolution(monitor, filename);
		}

		internal Solution ReadSolution(IProgressMonitor monitor, string filename)
		{
			SolutionUpgraderManager.Upgrade(filename);
			IFileFormat fileFormat;
			return this.ReadFile(monitor, filename, typeof(Solution), out fileFormat) as Solution;
		}

		public void ChangeSolutionName(IProgressMonitor monitor, FilePath newSolutionPath, string oldName, string newName)
		{
			Solution solution = this.ReadSolution(monitor, newSolutionPath);
			solution.SetLocation(newSolutionPath.ParentDirectory, newName);
			solution.Save(monitor);
			string filePath = SolutionConfig.GetFilePath(solution);
			string text = Path.Combine(newSolutionPath.ParentDirectory, oldName + ".cfg");
			if (File.Exists(text))
			{
				if (File.Exists(filePath))
				{
					File.Delete(filePath);
				}
				FileService.RenameFile(text, filePath);
			}
			string filePath2 = UserData.GetFilePath(solution.FileName);
			string text2 = Path.Combine(newSolutionPath.ParentDirectory, oldName + ".udf");
			if (File.Exists(text2))
			{
				if (File.Exists(filePath2))
				{
					File.Delete(filePath2);
				}
				FileService.RenameFile(text2, filePath2);
			}
		}

		private static void SetSolutionLocation(string filename, Solution solution)
		{
			string directoryName = Path.GetDirectoryName(filename);
			solution.SetLocation(directoryName, Path.GetFileNameWithoutExtension(filename));
		}

		public bool IsCocosFile(string fileName)
		{
			return this.FormatManager.GetFileFormats(fileName, typeof(CocosItem)).Count > 0;
		}

		public bool IsCocosFile(ResourceData resourceData)
		{
			string fileName = this.ToAbsolute(resourceData);
			return this.IsCocosFile(fileName);
		}

		internal bool IsImageFile(string fileName)
		{
			return this.FormatManager.GetFileFormats(fileName, typeof(ImageFile)).Count > 0;
		}

		internal bool IsImageFile(ResourceData resourceData)
		{
			string fileName = this.ToAbsolute(resourceData);
			return this.IsImageFile(fileName);
		}

		public bool IsPicture(FilePath fileName)
		{
			string extension = fileName.Extension;
			return extension.Equals(".png", StringComparison.OrdinalIgnoreCase) || extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase);
		}

		public bool IsTTFFile(string fileName)
		{
			return this.FormatManager.GetFileFormats(fileName, typeof(TTFFile)).Count > 0;
		}

		internal string ToAbsolute(ResourceData resourceData)
		{
			return ((FilePath)resourceData.Path).ToAbsolute(this.CurrentSolution.ItemDirectory);
		}

		private object ReadFile(IProgressMonitor monitor, string file, Type expectedType, IFileFormat format)
		{
			object obj = format.ReadFile(file, expectedType, monitor);
			if (obj == null)
			{
				throw new InvalidOperationException("Invalid file format: " + file);
			}
			return obj;
		}

		private object ReadFile(IProgressMonitor monitor, string file, Type expectedType, out IFileFormat format)
		{
			List<FileFormat> fileFormats = this.FormatManager.GetFileFormats(file, expectedType);
			if (fileFormats.Count == 0)
			{
				throw new InvalidOperationException("Unknown file format: " + file);
			}
			format = fileFormats[0];
			return this.ReadFile(monitor, file, expectedType, format);
		}

		public object ReadFile(IProgressMonitor monitor, string file, Type expectedType)
		{
			IFileFormat fileFormat;
			return this.ReadFile(monitor, file, expectedType, out fileFormat);
		}

		public FilePath WriteFile(IProgressMonitor monitor, FilePath file, object item)
		{
			IFileFormat defaultFormat = this.GetDefaultFormat(item);
			defaultFormat.WriteFile(file, item, monitor);
			if (monitor.AsyncOperation.Success)
			{
				return file;
			}
			return null;
		}

		private string GetTargetFile(string file)
		{
			return file;
		}

		internal void Save(IProgressMonitor monitor, WorkspaceItem workspaceItem)
		{
			workspaceItem.OnSave(monitor);
		}

		internal void InternalWriteWorkspaceItem(IProgressMonitor monitor, FilePath file, WorkspaceItem item)
		{
			FilePath filePath = this.WriteFile(monitor, file, item, item.FileFormat);
			if (filePath != null)
			{
				item.FileName = filePath;
				return;
			}
			throw new InvalidOperationException("FileFormat not provided for workspace item '" + item.Name + "'");
		}

		internal CocosFile InternalReadCocosFile(IProgressMonitor monitor, string file)
		{
			IFileFormat fileFormat;
			CocosFile cocosFile = this.ReadFile(monitor, file, typeof(CocosFile), out fileFormat) as CocosFile;
			cocosFile.FileName = file;
			return cocosFile;
		}

		internal FilePath WriteFile(IProgressMonitor monitor, FilePath file, object item, FileFormat fileFormat)
		{
			fileFormat.WriteFile(file, item, monitor);
			if (monitor.AsyncOperation.Success)
			{
				return file;
			}
			return null;
		}

		internal FileFormat GetDefaultFormat(object obj)
		{
			List<FileFormat> fileFormatsForObject = this.FormatManager.GetFileFormatsForObject(obj);
			if (fileFormatsForObject.Count == 0)
			{
				throw new InvalidOperationException("Can't handle objects of type '" + obj.GetType() + "'");
			}
			return fileFormatsForObject[0];
		}

		internal void InternalWriteCocosFile(IProgressMonitor monitor, FilePath file, CocosFile item)
		{
			FilePath filePath = this.WriteFile(monitor, file, item, item.FileFormat);
			if (filePath != null)
			{
				item.FileName = filePath;
				return;
			}
			throw new InvalidOperationException("FileFormat not provided for workspace item '" + item.Name + "'");
		}

		public CocosItem CreateCocosItem(string type, CocosItemCreateInfo info)
		{
			info.ContentType = type;
			foreach (ICocosItemBinding cocosItemBinding in this.ProjectBindingManager.ProjectBindings)
			{
				if (cocosItemBinding.CanCreateItem(type))
				{
					return cocosItemBinding.CreateItem(info);
				}
			}
			throw new InvalidOperationException("File type '" + type + "' not found");
		}

		public Solution CreateSolution(string directory, string name)
		{
			string text = Path.Combine(directory, name);
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			Solution solution = new Solution();
			solution.SetLocation(text, name);
			ResourceGroup item = new ResourceGroup(solution);
			solution.RootFolder.Items.Add(item);
			return solution;
		}

		private ResourceGroup GetResourceGroup(Solution solution)
		{
			if (solution != null)
			{
				return solution.RootFolder.Items[0] as ResourceGroup;
			}
			return null;
		}

		internal IPublishProcesser GetPublishProcesser(ResourceData resourceData)
		{
			return this.ProcesserManager.GetPublishProcesser(resourceData);
		}

		public FilePath GetFullPath(ResourceData resourceData)
		{
			if (resourceData.Type == EnumResourceType.Default)
			{
				return Option.GetEditorResourceFullPath(resourceData.Path);
			}
			if (resourceData.Type == EnumResourceType.None || resourceData.Type == EnumResourceType.PlistSubImage)
			{
				throw new ArgumentException("Can't convert to full path.");
			}
			return ((FilePath)resourceData.Path).ToAbsolute(this.CurrentSolution.ItemDirectory);
		}

		public ICompositeResourceProcesser GetCompositeResourceProcesser(string filePath)
		{
			if (!Path.IsPathRooted(filePath))
			{
				throw new ArgumentException("File path must be a absolute path.");
			}
			return this.ProcesserManager.GetCompositeResourceProcesser(filePath);
		}

		internal void NotifyResourceFileChanged(ChangedResourceCollection changedResourceCollection)
		{
			if (this.CurrentResourceGroup == null)
			{
				return;
			}
			ICocosFile rootFolder = this.CurrentResourceGroup.RootFolder;
			rootFolder.UpdateUsedResources(this.DefaultMonitor, changedResourceCollection);
		}

		public string SerializeGameFile(PublishInfo info, GameFile gameFile)
		{
			IGameFileSerializer currentSerializer = this.SerializeManager.CurrentSerializer;
			if (currentSerializer == null)
			{
				return "There is no publisher";
			}
			return currentSerializer.Serialize(info, gameFile);
		}

		public void Publish(IProgressMonitor monitor, PublishInfo info)
		{
			IGameFileSerializer currentSerializer = this.SerializeManager.CurrentSerializer;
			this.ReportSerializerData(currentSerializer);
			if (currentSerializer != null)
			{
				currentSerializer.ContextInitialize(info);
			}
			HashSet<ResourceData> hashSet = new HashSet<ResourceData>();
			if (info.PublishType == PublishType.Reference || info.PublishType == PublishType.All)
			{
				HashSet<ResourceData> usedResources = this.CurrentResourceGroup.GetUsedResources(monitor);
				this.CheckPublishFilePath(monitor, usedResources);
				if (info.PublishType == PublishType.All)
				{
					hashSet = this.CurrentResourceGroup.GetAllResources();
					usedResources.UnionWith(hashSet);
				}
				hashSet = this.ProcessResourceDatas(monitor, usedResources);
			}
			if (!monitor.AsyncOperation.Success)
			{
				return;
			}
			Parallel.ForEach<ResourceData>(hashSet, delegate(ResourceData item)
			{
				if (this.IsCocosFile(item))
				{
					return;
				}
				this.CopyResourceData(info.PublishDirectory, item);
			});
			this.PublishCocosItems(this.CurrentResourceGroup.RootFolder, monitor, info);
			if (currentSerializer != null)
			{
				currentSerializer.ContextFinalize(info);
			}
		}

		private void CheckPublishFilePath(IProgressMonitor monitor, IEnumerable<ResourceData> resourceDatas)
		{
			if (resourceDatas == null || resourceDatas.Count<ResourceData>() == 0)
			{
				return;
			}
			FilePath baseDirectory = this.CurrentResourceGroup.RootFolder.BaseDirectory;
			string[] fileSystemEntries = Directory.GetFileSystemEntries(baseDirectory, "*", SearchOption.AllDirectories);
			HashSet<string> hashSet = new HashSet<string>(fileSystemEntries);
			bool flag = false;
			foreach (ResourceData resourceData in resourceDatas)
			{
				if (resourceData.Type != EnumResourceType.Default)
				{
					string text = baseDirectory.Combine(new string[]
					{
						resourceData.Path
					});
					if (resourceData.Type == EnumResourceType.PlistSubImage)
					{
						text = baseDirectory.Combine(new string[]
						{
							resourceData.Plist
						});
					}
					text = FileService.MakePathSeparatorsNative(text);
					if (File.Exists(text) && !hashSet.Contains(text))
					{
						flag = true;
						string message = string.Format(LanguageInfo.Dialog_Publish_FileCheck, resourceData.Path);
						LogConfig.Output.Info(message, true);
					}
				}
			}
			if (flag)
			{
				monitor.ReportWarning(string.Empty);
			}
		}

		private void ReportSerializerData(IGameFileSerializer serializer)
		{
			if (serializer == null)
			{
				return;
			}
			string newValue = "Custom";
			BaseCocosFileSerializer baseCocosFileSerializer = serializer as BaseCocosFileSerializer;
			if (baseCocosFileSerializer != null && baseCocosFileSerializer.IsDefault)
			{
				newValue = baseCocosFileSerializer.ID;
			}
			Tracker.Add(ViewRegions.None, "Publish", "DataFormat", newValue);
		}

		private void PublishCocosItems(ResourceFolder folder, IProgressMonitor monitor, PublishInfo info)
		{
			foreach (ResourceItem resourceItem in folder.Items)
			{
				CocosItem cocosItem = resourceItem as CocosItem;
				if (cocosItem != null)
				{
					cocosItem.Publish(monitor, info);
				}
				else
				{
					ResourceFolder resourceFolder = resourceItem as ResourceFolder;
					if (resourceFolder != null)
					{
						this.PublishCocosItems(resourceFolder, monitor, info);
					}
				}
			}
		}

		public HashSet<ResourceData> ProcessResourceDatas(IProgressMonitor monitor, HashSet<ResourceData> resourceDatas)
		{
			ResourceDataSet source = new ResourceDataSet(resourceDatas);
			ConcurrentBag<ResourceData> processedResources = new ConcurrentBag<ResourceData>();
			Parallel.ForEach<ResourceData>(source, delegate(ResourceData item)
			{
				if (item == null || string.IsNullOrEmpty(item.Path))
				{
					return;
				}
				if (item.Type == EnumResourceType.Normal)
				{
					ResourceItem resourceItem = ProjectsService.Instance.CurrentResourceGroup.FindResourceItem(item);
					DataError dataError;
					if (resourceItem == null)
					{
						dataError = new DataError(string.Format("File not exist, the file is {0}", item.Path));
					}
					else
					{
						dataError = resourceItem.DataError;
					}
					if (dataError != null)
					{
						monitor.ReportError(dataError.Message, null);
						LogConfig.Output.Error(dataError.Message);
						return;
					}
				}
				try
				{
					IPublishProcesser publishProcesser = ProjectsService.Instance.GetPublishProcesser(item);
					if (publishProcesser != null)
					{
						HashSet<ResourceData> hashSet = publishProcesser.Process(item);
						if (hashSet == null)
						{
							goto IL_D9;
						}
						using (HashSet<ResourceData>.Enumerator enumerator = hashSet.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								ResourceData item2 = enumerator.Current;
								processedResources.Add(item2);
							}
							goto IL_D9;
						}
					}
					processedResources.Add(item);
					IL_D9:;
				}
				catch (Exception ex)
				{
					string message = string.Format("Processer resource failed, type is {0}, path is {1}, plist is {2}. The error is {3}", new object[]
					{
						item.Type,
						item.Path,
						item.Plist,
						ex.Message
					});
					monitor.ReportError(message, ex);
					LogConfig.Output.Error(message, ex);
				}
			});
			return new ResourceDataSet(processedResources);
		}

		private void CopyResourceData(FilePath publishDirectory, ResourceData resourceData)
		{
			if (resourceData.Type == EnumResourceType.Addin)
			{
				return;
			}
			FilePath fullPath = this.GetFullPath(resourceData);
			FileInfo fileInfo = new FileInfo(fullPath);
			if (!fileInfo.Exists)
			{
				return;
			}
			FilePath filePath = ((FilePath)resourceData.Path).ToAbsolute(publishDirectory);
			if (!Directory.Exists(filePath.ParentDirectory))
			{
				Directory.CreateDirectory(filePath.ParentDirectory);
			}
			FileInfo fileInfo2 = new FileInfo(filePath);
			if (!fileInfo2.Exists || fileInfo.LastWriteTime != fileInfo2.LastWriteTime)
			{
				FileService.CopyFile(fullPath, filePath);
			}
		}

		private Solution currentSolution;

		private static ProjectsService instance;
	}
}
