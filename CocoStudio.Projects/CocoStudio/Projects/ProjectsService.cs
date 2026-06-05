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
	// Token: 0x0200006B RID: 107
	public class ProjectsService
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600033A RID: 826 RVA: 0x0000B964 File Offset: 0x00009B64
		internal ResourceChangeService ResourceChangeService
		{
			get
			{
				return ResourceChangeService.Instance;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600033B RID: 827 RVA: 0x0000B96B File Offset: 0x00009B6B
		// (set) Token: 0x0600033C RID: 828 RVA: 0x0000B973 File Offset: 0x00009B73
		internal FileFormatManager FormatManager { get; private set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600033D RID: 829 RVA: 0x0000B97C File Offset: 0x00009B7C
		// (set) Token: 0x0600033E RID: 830 RVA: 0x0000B984 File Offset: 0x00009B84
		internal CocosItemBindingManager ProjectBindingManager { get; private set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600033F RID: 831 RVA: 0x0000B98D File Offset: 0x00009B8D
		// (set) Token: 0x06000340 RID: 832 RVA: 0x0000B995 File Offset: 0x00009B95
		internal ProcesserManager ProcesserManager { get; private set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000B99E File Offset: 0x00009B9E
		// (set) Token: 0x06000342 RID: 834 RVA: 0x0000B9A6 File Offset: 0x00009BA6
		internal PreviewImageService PreviewImageService { get; private set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000343 RID: 835 RVA: 0x0000B9AF File Offset: 0x00009BAF
		// (set) Token: 0x06000344 RID: 836 RVA: 0x0000B9B7 File Offset: 0x00009BB7
		public DataModelManager DataModelManager { get; private set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000345 RID: 837 RVA: 0x0000B9C0 File Offset: 0x00009BC0
		// (set) Token: 0x06000346 RID: 838 RVA: 0x0000B9C8 File Offset: 0x00009BC8
		public ISerializeManager SerializeManager { get; private set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000347 RID: 839 RVA: 0x0000B9D1 File Offset: 0x00009BD1
		// (set) Token: 0x06000348 RID: 840 RVA: 0x0000B9D9 File Offset: 0x00009BD9
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

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0000B9F2 File Offset: 0x00009BF2
		public ResourceGroup CurrentResourceGroup
		{
			get
			{
				return this.GetResourceGroup(this.CurrentSolution);
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600034A RID: 842 RVA: 0x0000BA00 File Offset: 0x00009C00
		// (set) Token: 0x0600034B RID: 843 RVA: 0x0000BA08 File Offset: 0x00009C08
		public List<ResourceItem> CurrentResourceItems { get; internal set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600034C RID: 844 RVA: 0x0000BA11 File Offset: 0x00009C11
		public IProgressMonitor DefaultMonitor
		{
			get
			{
				return new ConsoleProgressFullExceptionMonitor(true, true);
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600034D RID: 845 RVA: 0x0000BA1A File Offset: 0x00009C1A
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

		// Token: 0x0600034E RID: 846 RVA: 0x0000BA34 File Offset: 0x00009C34
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

		// Token: 0x0600034F RID: 847 RVA: 0x0000BA8E File Offset: 0x00009C8E
		private void OnCurrentSolutionChanged(Solution value)
		{
			this.PreviewImageService.Clear();
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000BA9C File Offset: 0x00009C9C
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

		// Token: 0x06000351 RID: 849 RVA: 0x0000BB54 File Offset: 0x00009D54
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

		// Token: 0x06000352 RID: 850 RVA: 0x0000BBCC File Offset: 0x00009DCC
		public CocosItem ReadCocosItem(IProgressMonitor monitor, string filename)
		{
			IFileFormat fileFormat;
			return this.ReadFile(monitor, filename, typeof(CocosItem), out fileFormat) as CocosItem;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000BBF4 File Offset: 0x00009DF4
		internal Solution ReadWorkspaceItem(IProgressMonitor monitor, string filename)
		{
			return this.ReadSolution(monitor, filename);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000BC00 File Offset: 0x00009E00
		internal Solution ReadSolution(IProgressMonitor monitor, string filename)
		{
			SolutionUpgraderManager.Upgrade(filename);
			IFileFormat fileFormat;
			return this.ReadFile(monitor, filename, typeof(Solution), out fileFormat) as Solution;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000BC30 File Offset: 0x00009E30
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

		// Token: 0x06000356 RID: 854 RVA: 0x0000BCEC File Offset: 0x00009EEC
		private static void SetSolutionLocation(string filename, Solution solution)
		{
			string directoryName = Path.GetDirectoryName(filename);
			solution.SetLocation(directoryName, Path.GetFileNameWithoutExtension(filename));
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000BD12 File Offset: 0x00009F12
		public bool IsCocosFile(string fileName)
		{
			return this.FormatManager.GetFileFormats(fileName, typeof(CocosItem)).Count > 0;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000BD34 File Offset: 0x00009F34
		public bool IsCocosFile(ResourceData resourceData)
		{
			string fileName = this.ToAbsolute(resourceData);
			return this.IsCocosFile(fileName);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000BD50 File Offset: 0x00009F50
		internal bool IsImageFile(string fileName)
		{
			return this.FormatManager.GetFileFormats(fileName, typeof(ImageFile)).Count > 0;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000BD70 File Offset: 0x00009F70
		internal bool IsImageFile(ResourceData resourceData)
		{
			string fileName = this.ToAbsolute(resourceData);
			return this.IsImageFile(fileName);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000BD8C File Offset: 0x00009F8C
		public bool IsPicture(FilePath fileName)
		{
			string extension = fileName.Extension;
			return extension.Equals(".png", StringComparison.OrdinalIgnoreCase) || extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000BDBD File Offset: 0x00009FBD
		public bool IsTTFFile(string fileName)
		{
			return this.FormatManager.GetFileFormats(fileName, typeof(TTFFile)).Count > 0;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000BDE0 File Offset: 0x00009FE0
		internal string ToAbsolute(ResourceData resourceData)
		{
			return resourceData.Path.ToAbsolute(this.CurrentSolution.ItemDirectory);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000BE10 File Offset: 0x0000A010
		private object ReadFile(IProgressMonitor monitor, string file, Type expectedType, IFileFormat format)
		{
			object obj = format.ReadFile(file, expectedType, monitor);
			if (obj == null)
			{
				throw new InvalidOperationException("Invalid file format: " + file);
			}
			return obj;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000BE44 File Offset: 0x0000A044
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

		// Token: 0x06000360 RID: 864 RVA: 0x0000BE90 File Offset: 0x0000A090
		public object ReadFile(IProgressMonitor monitor, string file, Type expectedType)
		{
			IFileFormat fileFormat;
			return this.ReadFile(monitor, file, expectedType, out fileFormat);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000BEA8 File Offset: 0x0000A0A8
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

		// Token: 0x06000362 RID: 866 RVA: 0x0000BEDB File Offset: 0x0000A0DB
		private string GetTargetFile(string file)
		{
			return file;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000BEDE File Offset: 0x0000A0DE
		internal void Save(IProgressMonitor monitor, WorkspaceItem workspaceItem)
		{
			workspaceItem.OnSave(monitor);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000BEE8 File Offset: 0x0000A0E8
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

		// Token: 0x06000365 RID: 869 RVA: 0x0000BF38 File Offset: 0x0000A138
		internal CocosFile InternalReadCocosFile(IProgressMonitor monitor, string file)
		{
			IFileFormat fileFormat;
			CocosFile cocosFile = this.ReadFile(monitor, file, typeof(CocosFile), out fileFormat) as CocosFile;
			cocosFile.FileName = file;
			return cocosFile;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000BF6C File Offset: 0x0000A16C
		internal FilePath WriteFile(IProgressMonitor monitor, FilePath file, object item, FileFormat fileFormat)
		{
			fileFormat.WriteFile(file, item, monitor);
			if (monitor.AsyncOperation.Success)
			{
				return file;
			}
			return null;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000BF90 File Offset: 0x0000A190
		internal FileFormat GetDefaultFormat(object obj)
		{
			List<FileFormat> fileFormatsForObject = this.FormatManager.GetFileFormatsForObject(obj);
			if (fileFormatsForObject.Count == 0)
			{
				throw new InvalidOperationException("Can't handle objects of type '" + obj.GetType() + "'");
			}
			return fileFormatsForObject[0];
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000BFD4 File Offset: 0x0000A1D4
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

		// Token: 0x06000369 RID: 873 RVA: 0x0000C024 File Offset: 0x0000A224
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

		// Token: 0x0600036A RID: 874 RVA: 0x0000C0A8 File Offset: 0x0000A2A8
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

		// Token: 0x0600036B RID: 875 RVA: 0x0000C0FA File Offset: 0x0000A2FA
		private ResourceGroup GetResourceGroup(Solution solution)
		{
			if (solution != null)
			{
				return solution.RootFolder.Items[0] as ResourceGroup;
			}
			return null;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000C117 File Offset: 0x0000A317
		internal IPublishProcesser GetPublishProcesser(ResourceData resourceData)
		{
			return this.ProcesserManager.GetPublishProcesser(resourceData);
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000C128 File Offset: 0x0000A328
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
			return resourceData.Path.ToAbsolute(this.CurrentSolution.ItemDirectory);
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000C18A File Offset: 0x0000A38A
		public ICompositeResourceProcesser GetCompositeResourceProcesser(string filePath)
		{
			if (!Path.IsPathRooted(filePath))
			{
				throw new ArgumentException("File path must be a absolute path.");
			}
			return this.ProcesserManager.GetCompositeResourceProcesser(filePath);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000C1AC File Offset: 0x0000A3AC
		internal void NotifyResourceFileChanged(ChangedResourceCollection changedResourceCollection)
		{
			if (this.CurrentResourceGroup == null)
			{
				return;
			}
			ICocosFile rootFolder = this.CurrentResourceGroup.RootFolder;
			rootFolder.UpdateUsedResources(this.DefaultMonitor, changedResourceCollection);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000C1DC File Offset: 0x0000A3DC
		public string SerializeGameFile(PublishInfo info, GameFile gameFile)
		{
			IGameFileSerializer currentSerializer = this.SerializeManager.CurrentSerializer;
			if (currentSerializer == null)
			{
				return "There is no publisher";
			}
			return currentSerializer.Serialize(info, gameFile);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000C23C File Offset: 0x0000A43C
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

		// Token: 0x06000372 RID: 882 RVA: 0x0000C328 File Offset: 0x0000A528
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

		// Token: 0x06000373 RID: 883 RVA: 0x0000C458 File Offset: 0x0000A658
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

		// Token: 0x06000374 RID: 884 RVA: 0x0000C49C File Offset: 0x0000A69C
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

		// Token: 0x06000375 RID: 885 RVA: 0x0000C680 File Offset: 0x0000A880
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

		// Token: 0x06000376 RID: 886 RVA: 0x0000C6CC File Offset: 0x0000A8CC
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
			FilePath filePath = resourceData.Path.ToAbsolute(publishDirectory);
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

		// Token: 0x040000C3 RID: 195
		private Solution currentSolution;

		// Token: 0x040000C4 RID: 196
		private static ProjectsService instance;
	}
}
