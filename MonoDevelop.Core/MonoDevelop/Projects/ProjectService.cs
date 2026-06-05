using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Mono.Addins;
using Mono.Unix;
using MonoDevelop.Core;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects
{
	// Token: 0x020000FC RID: 252
	public class ProjectService
	{
		// Token: 0x1400002C RID: 44
		// (add) Token: 0x060008E3 RID: 2275 RVA: 0x00023468 File Offset: 0x00021668
		// (remove) Token: 0x060008E4 RID: 2276 RVA: 0x000234A0 File Offset: 0x000216A0
		internal event EventHandler DataContextChanged;

		// Token: 0x060008E5 RID: 2277 RVA: 0x000234D8 File Offset: 0x000216D8
		internal ProjectService()
		{
			AddinManager.AddExtensionNodeHandler("/MonoDevelop/ProjectModel/FileFormats", new ExtensionNodeEventHandler(this.OnFormatExtensionChanged));
			AddinManager.AddExtensionNodeHandler("/MonoDevelop/ProjectModel/SerializableClasses", new ExtensionNodeEventHandler(this.OnSerializableExtensionChanged));
			AddinManager.AddExtensionNodeHandler("/MonoDevelop/ProjectModel/ExtendedProperties", new ExtensionNodeEventHandler(this.OnPropertiesExtensionChanged));
			AddinManager.AddExtensionNodeHandler("/MonoDevelop/ProjectModel/ProjectBindings", new ExtensionNodeEventHandler(this.OnProjectsExtensionChanged));
			AddinManager.ExtensionChanged += this.OnExtensionChanged;
			this.defaultFormat = this.formatManager.GetFileFormat("MSBuild12");
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x000235A1 File Offset: 0x000217A1
		public DataContext DataContext
		{
			get
			{
				return this.dataContext;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x000235A9 File Offset: 0x000217A9
		public FileFormatManager FileFormats
		{
			get
			{
				return this.formatManager;
			}
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x000235B4 File Offset: 0x000217B4
		internal ProjectServiceExtension GetExtensionChain(IBuildTarget target)
		{
			ProjectServiceExtension projectServiceExtension;
			if (target != null)
			{
				lock (target)
				{
					ProjectService.ExtensionChainInfo extensionChainInfo = (ProjectService.ExtensionChainInfo)target.ExtendedProperties[typeof(ProjectService.ExtensionChainInfo)];
					if (extensionChainInfo == null)
					{
						extensionChainInfo = new ProjectService.ExtensionChainInfo();
						ExtensionContext extensionContext = AddinManager.CreateExtensionContext();
						extensionChainInfo.ExtensionContext = extensionContext;
						extensionChainInfo.ItemTypeCondition = new ItemTypeCondition(target.GetType());
						extensionChainInfo.ProjectLanguageCondition = new ProjectLanguageCondition(target);
						extensionContext.RegisterCondition("ItemType", extensionChainInfo.ItemTypeCondition);
						extensionContext.RegisterCondition("ProjectLanguage", extensionChainInfo.ProjectLanguageCondition);
						target.ExtendedProperties[typeof(ProjectService.ExtensionChainInfo)] = extensionChainInfo;
					}
					else
					{
						extensionChainInfo.ItemTypeCondition.ObjType = target.GetType();
						extensionChainInfo.ProjectLanguageCondition.TargetProject = target;
					}
					ProjectServiceExtension[] extensionObjects = extensionChainInfo.ExtensionContext.GetExtensionObjects<ProjectServiceExtension>("/MonoDevelop/ProjectModel/ProjectServiceExtensions");
					projectServiceExtension = this.CreateExtensionChain(extensionObjects);
					extensionChainInfo.ProjectLanguageCondition.TargetProject = null;
					goto IL_156;
				}
			}
			if (this.defaultExtensionChain == null)
			{
				ExtensionContext extensionContext2 = AddinManager.CreateExtensionContext();
				extensionContext2.RegisterCondition("ItemType", new ItemTypeCondition(typeof(UnknownItem)));
				extensionContext2.RegisterCondition("ProjectLanguage", new ProjectLanguageCondition(UnknownItem.Instance));
				ProjectServiceExtension[] extensionObjects2 = extensionContext2.GetExtensionObjects<ProjectServiceExtension>("/MonoDevelop/ProjectModel/ProjectServiceExtensions");
				this.defaultExtensionChain = this.CreateExtensionChain(extensionObjects2);
			}
			projectServiceExtension = this.defaultExtensionChain;
			target = UnknownItem.Instance;
			IL_156:
			if (projectServiceExtension.SupportsItem(target))
			{
				return projectServiceExtension;
			}
			return projectServiceExtension.GetNext(target);
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0002373C File Offset: 0x0002193C
		private ProjectServiceExtension CreateExtensionChain(ProjectServiceExtension[] extensions)
		{
			CustomCommandExtension customCommandExtension = new CustomCommandExtension();
			for (int i = 0; i < extensions.Length - 1; i++)
			{
				extensions[i].Next = extensions[i + 1];
			}
			if (extensions.Length > 0)
			{
				extensions[extensions.Length - 1].Next = this.extensionChainTerminator;
				customCommandExtension.Next = extensions[0];
			}
			else
			{
				customCommandExtension.Next = this.extensionChainTerminator;
			}
			return customCommandExtension;
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060008EA RID: 2282 RVA: 0x0002379C File Offset: 0x0002199C
		// (set) Token: 0x060008EB RID: 2283 RVA: 0x000237A4 File Offset: 0x000219A4
		public string DefaultPlatformTarget
		{
			get
			{
				return this.defaultPlatformTarget;
			}
			set
			{
				this.defaultPlatformTarget = value;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x000237AD File Offset: 0x000219AD
		// (set) Token: 0x060008ED RID: 2285 RVA: 0x000237D2 File Offset: 0x000219D2
		public TargetFramework DefaultTargetFramework
		{
			get
			{
				if (this.defaultTargetFramework == null)
				{
					this.defaultTargetFramework = Runtime.SystemAssemblyService.GetTargetFramework(ProjectService.DefaultTargetFrameworkId);
				}
				return this.defaultTargetFramework;
			}
			set
			{
				this.defaultTargetFramework = value;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060008EE RID: 2286 RVA: 0x000237DB File Offset: 0x000219DB
		public FileFormat DefaultFileFormat
		{
			get
			{
				return this.defaultFormat;
			}
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x000237E4 File Offset: 0x000219E4
		internal FileFormat GetDefaultFormat(object ob)
		{
			if (this.defaultFormat.CanWrite(ob))
			{
				return this.defaultFormat;
			}
			FileFormat[] fileFormatsForObject = this.FileFormats.GetFileFormatsForObject(ob);
			if (fileFormatsForObject.Length == 0)
			{
				throw new InvalidOperationException("Can't handle objects of type '" + ob.GetType() + "'");
			}
			return fileFormatsForObject[0];
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0002389C File Offset: 0x00021A9C
		public SolutionEntityItem ReadSolutionItem(IProgressMonitor monitor, string file)
		{
			file = Path.GetFullPath(file);
			SolutionEntityItem result;
			using (Counters.ReadSolutionItem.BeginTiming("Read project " + file))
			{
				file = this.GetTargetFile(file);
				SolutionEntityItem solutionEntityItem = this.GetExtensionChain(null).LoadSolutionItem(monitor, file, delegate(IProgressMonitor param0, string param1)
				{
					FileFormat fileFormat;
					SolutionEntityItem solutionEntityItem2 = this.ReadFile(monitor, file, typeof(SolutionEntityItem), out fileFormat) as SolutionEntityItem;
					if (solutionEntityItem2 != null)
					{
						solutionEntityItem2.FileFormat = fileFormat;
						return solutionEntityItem2;
					}
					throw new InvalidOperationException("Invalid file format: " + file);
				});
				solutionEntityItem.NeedsReload = false;
				result = solutionEntityItem;
			}
			return result;
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x00023958 File Offset: 0x00021B58
		public SolutionItem ReadSolutionItem(IProgressMonitor monitor, SolutionItemReference reference, params WorkspaceItem[] workspaces)
		{
			if (reference.Id == null)
			{
				FilePath fullPath = reference.Path.FullPath;
				foreach (WorkspaceItem workspaceItem in workspaces)
				{
					foreach (SolutionEntityItem solutionEntityItem in workspaceItem.GetAllSolutionItems<SolutionEntityItem>())
					{
						if (fullPath == solutionEntityItem.FileName)
						{
							return solutionEntityItem;
						}
					}
				}
				return this.ReadSolutionItem(monitor, reference.Path);
			}
			Solution solution = null;
			if (workspaces.Length > 0)
			{
				FilePath fullPath2 = reference.Path.FullPath;
				foreach (WorkspaceItem workspaceItem2 in workspaces)
				{
					foreach (Solution solution2 in workspaceItem2.GetAllSolutions())
					{
						if (solution2.FileName.FullPath == fullPath2)
						{
							solution = solution2;
							break;
						}
					}
					if (solution != null)
					{
						break;
					}
				}
			}
			if (solution == null)
			{
				solution = (this.ReadWorkspaceItem(monitor, reference.Path) as Solution);
			}
			if (reference.Id == ":root:")
			{
				return solution.RootFolder;
			}
			return solution.GetSolutionItem(reference.Id);
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x00023ADC File Offset: 0x00021CDC
		public WorkspaceItem ReadWorkspaceItem(IProgressMonitor monitor, string file)
		{
			file = Path.GetFullPath(file);
			WorkspaceItem result;
			using (Counters.ReadWorkspaceItem.BeginTiming("Read solution " + file))
			{
				file = this.GetTargetFile(file);
				WorkspaceItem workspaceItem = this.GetExtensionChain(null).LoadWorkspaceItem(monitor, file);
				if (workspaceItem == null)
				{
					throw new InvalidOperationException("Invalid file format: " + file);
				}
				workspaceItem.NeedsReload = false;
				result = workspaceItem;
			}
			return result;
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x00023B5C File Offset: 0x00021D5C
		internal void InternalWriteSolutionItem(IProgressMonitor monitor, FilePath file, SolutionEntityItem item)
		{
			FilePath filePath = this.WriteFile(monitor, file, item, null);
			if (filePath != null)
			{
				item.FileName = filePath;
				return;
			}
			throw new InvalidOperationException("FileFormat not provided for solution item '" + item.Name + "'");
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x00023BA4 File Offset: 0x00021DA4
		internal WorkspaceItem InternalReadWorkspaceItem(string file, IProgressMonitor monitor)
		{
			FileFormat format;
			WorkspaceItem workspaceItem = this.ReadFile(monitor, file, typeof(WorkspaceItem), out format) as WorkspaceItem;
			if (workspaceItem == null)
			{
				throw new InvalidOperationException("Invalid file format: " + file);
			}
			if (!workspaceItem.FormatSet)
			{
				workspaceItem.ConvertToFormat(format, false);
			}
			return workspaceItem;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x00023BF0 File Offset: 0x00021DF0
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

		// Token: 0x060008F6 RID: 2294 RVA: 0x00023C40 File Offset: 0x00021E40
		private object ReadFile(IProgressMonitor monitor, string file, Type expectedType, out FileFormat format)
		{
			FileFormat[] fileFormats = this.formatManager.GetFileFormats(file, expectedType);
			if (fileFormats.Length == 0)
			{
				throw new InvalidOperationException("Unknown file format: " + file);
			}
			format = fileFormats[0];
			object obj = format.Format.ReadFile(file, expectedType, monitor);
			if (obj == null)
			{
				throw new InvalidOperationException("Invalid file format: " + file);
			}
			return obj;
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x00023CA4 File Offset: 0x00021EA4
		private FilePath WriteFile(IProgressMonitor monitor, FilePath file, object item, FileFormat format)
		{
			if (format == null)
			{
				if (this.defaultFormat.CanWrite(item))
				{
					format = this.defaultFormat;
				}
				else
				{
					FileFormat[] fileFormatsForObject = this.formatManager.GetFileFormatsForObject(item);
					format = ((fileFormatsForObject.Length > 0) ? fileFormatsForObject[0] : null);
				}
				if (format == null)
				{
					return null;
				}
				file = format.GetValidFileName(item, file);
			}
			FileService.RequestFileEdit(file, true);
			format.Format.WriteFile(file, item, monitor);
			return file;
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x00023D20 File Offset: 0x00021F20
		public string Export(IProgressMonitor monitor, string rootSourceFile, string targetPath, FileFormat format)
		{
			rootSourceFile = this.GetTargetFile(rootSourceFile);
			return this.Export(monitor, rootSourceFile, null, targetPath, format);
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x00023D38 File Offset: 0x00021F38
		public string Export(IProgressMonitor monitor, string rootSourceFile, string[] includedChildIds, string targetPath, FileFormat format)
		{
			IWorkspaceFileObject workspaceFileObject;
			if (this.IsWorkspaceItemFile(rootSourceFile))
			{
				workspaceFileObject = (this.ReadWorkspaceItem(monitor, rootSourceFile) as Solution);
			}
			else
			{
				workspaceFileObject = this.ReadSolutionItem(monitor, rootSourceFile);
				if (workspaceFileObject == null)
				{
					throw new InvalidOperationException("File is not a solution or project.");
				}
			}
			string result;
			using (workspaceFileObject)
			{
				result = this.Export(monitor, workspaceFileObject, includedChildIds, targetPath, format);
			}
			return result;
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x00023DA4 File Offset: 0x00021FA4
		private string Export(IProgressMonitor monitor, IWorkspaceFileObject obj, string[] includedChildIds, string targetPath, FileFormat format)
		{
			string text = obj.FileName;
			string fullPath = Path.GetFullPath(Path.GetDirectoryName(text));
			targetPath = Path.GetFullPath(targetPath);
			if (fullPath != targetPath)
			{
				if (!this.CopyFiles(monitor, obj, obj.GetItemFiles(true), targetPath, true))
				{
					return null;
				}
				string file = Path.Combine(targetPath, Path.GetFileName(text));
				if (this.IsWorkspaceItemFile(text))
				{
					obj = this.ReadWorkspaceItem(monitor, file);
				}
				else
				{
					obj = this.ReadSolutionItem(monitor, file);
				}
				using (obj)
				{
					List<FilePath> itemFiles = obj.GetItemFiles(true);
					this.ExcludeEntries(obj, includedChildIds);
					if (format != null)
					{
						obj.ConvertToFormat(format, true);
					}
					obj.Save(monitor);
					List<FilePath> itemFiles2 = obj.GetItemFiles(true);
					foreach (FilePath item in itemFiles2)
					{
						if (!item.IsChildPathOf(targetPath))
						{
							if (obj is Solution)
							{
								monitor.ReportError(string.Concat(new string[]
								{
									"The solution '",
									obj.Name,
									"' is referencing the file '",
									item.FileName,
									"' which is located outside the root solution directory."
								}), null);
							}
							else
							{
								monitor.ReportError(string.Concat(new string[]
								{
									"The project '",
									obj.Name,
									"' is referencing the file '",
									item.FileName,
									"' which is located outside the project directory."
								}), null);
							}
						}
						itemFiles.Remove(item);
					}
					foreach (FilePath filePath in itemFiles)
					{
						if (File.Exists(filePath))
						{
							File.Delete(filePath);
							FilePath parentDirectory = filePath.ParentDirectory;
							if (Directory.GetFiles(parentDirectory).Length == 0 && Directory.GetDirectories(parentDirectory).Length == 0)
							{
								try
								{
									Directory.Delete(parentDirectory);
								}
								catch (Exception exception)
								{
									monitor.ReportError(null, exception);
								}
							}
						}
					}
					return obj.FileName;
				}
			}
			string result;
			using (obj)
			{
				this.ExcludeEntries(obj, includedChildIds);
				if (format != null)
				{
					obj.ConvertToFormat(format, true);
				}
				obj.Save(monitor);
				result = obj.FileName;
			}
			return result;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0002409C File Offset: 0x0002229C
		private void ExcludeEntries(IWorkspaceFileObject obj, string[] includedChildIds)
		{
			Solution solution = obj as Solution;
			if (solution != null && includedChildIds != null)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				foreach (string text in includedChildIds)
				{
					dictionary[text] = text;
				}
				foreach (SolutionItem solutionItem in solution.GetAllSolutionItems<SolutionItem>())
				{
					if (!dictionary.ContainsKey(solutionItem.ItemId) && solutionItem.ParentFolder != null)
					{
						solutionItem.ParentFolder.Items.Remove(solutionItem);
					}
				}
			}
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0002414C File Offset: 0x0002234C
		private bool CopyFiles(IProgressMonitor monitor, IWorkspaceFileObject obj, IEnumerable<FilePath> files, FilePath targetBasePath, bool ignoreExternalFiles)
		{
			FilePath fullPath = obj.BaseDirectory.FullPath;
			foreach (FilePath filePath in files)
			{
				if (!File.Exists(filePath))
				{
					monitor.ReportWarning(GettextCatalog.GetString("File '{0}' not found.", filePath));
				}
				else
				{
					FilePath fullPath2 = filePath.FullPath;
					if (!fullPath2.IsChildPathOf(fullPath))
					{
						if (!ignoreExternalFiles)
						{
							if (obj is Solution)
							{
								monitor.ReportError(string.Concat(new string[]
								{
									"The solution '",
									obj.Name,
									"' is referencing the file '",
									Path.GetFileName(filePath),
									"' which is located outside the root solution directory."
								}), null);
							}
							else
							{
								monitor.ReportError(string.Concat(new string[]
								{
									"The project '",
									obj.Name,
									"' is referencing the file '",
									Path.GetFileName(filePath),
									"' which is located outside the project directory."
								}), null);
							}
							return false;
						}
					}
					else
					{
						FilePath filePath2 = fullPath2.ToRelative(fullPath).ToAbsolute(targetBasePath);
						if (!Directory.Exists(filePath2.ParentDirectory))
						{
							Directory.CreateDirectory(filePath2.ParentDirectory);
						}
						File.Copy(filePath, filePath2, true);
					}
				}
			}
			return true;
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x000242EC File Offset: 0x000224EC
		public bool CanCreateSingleFileProject(string file)
		{
			foreach (object obj in this.projectBindings)
			{
				ProjectBindingCodon projectBindingCodon = (ProjectBindingCodon)obj;
				if (projectBindingCodon.ProjectBinding.CanCreateSingleFileProject(file))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00024354 File Offset: 0x00022554
		public Project CreateSingleFileProject(string file)
		{
			foreach (object obj in this.projectBindings)
			{
				ProjectBindingCodon projectBindingCodon = (ProjectBindingCodon)obj;
				if (projectBindingCodon.ProjectBinding.CanCreateSingleFileProject(file))
				{
					return projectBindingCodon.ProjectBinding.CreateSingleFileProject(file);
				}
			}
			return null;
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x000243C8 File Offset: 0x000225C8
		public Project CreateProject(string type, ProjectCreateInformation info, XmlElement projectOptions)
		{
			foreach (object obj in this.projectBindings)
			{
				ProjectBindingCodon projectBindingCodon = (ProjectBindingCodon)obj;
				if (projectBindingCodon.ProjectBinding.Name == type)
				{
					return projectBindingCodon.ProjectBinding.CreateProject(info, projectOptions);
				}
			}
			throw new InvalidOperationException("Project type '" + type + "' not found");
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0002445C File Offset: 0x0002265C
		public bool CanCreateProject(string type)
		{
			foreach (object obj in this.projectBindings)
			{
				ProjectBindingCodon projectBindingCodon = (ProjectBindingCodon)obj;
				if (projectBindingCodon.ProjectBinding.Name == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x000244E0 File Offset: 0x000226E0
		public Solution GetWrapperSolution(IProgressMonitor monitor, string filename)
		{
			FileFormat[] array = Services.ProjectService.FileFormats.GetFileFormats(filename, typeof(SolutionEntityItem));
			if (array.Length == 0)
			{
				array = new FileFormat[]
				{
					this.DefaultFileFormat
				};
			}
			Solution tempSolution = new Solution();
			FileFormat fileFormat = array.FirstOrDefault((FileFormat f) => f.CanWrite(tempSolution)) ?? this.DefaultFileFormat;
			string validFileName = fileFormat.GetValidFileName(tempSolution, filename);
			if (File.Exists(validFileName))
			{
				return (Solution)Services.ProjectService.ReadWorkspaceItem(monitor, validFileName);
			}
			tempSolution.SetLocation(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename));
			SolutionEntityItem item = Services.ProjectService.ReadSolutionItem(monitor, filename);
			tempSolution.ConvertToFormat(fileFormat, false);
			tempSolution.RootFolder.Items.Add(item);
			tempSolution.CreateDefaultConfigurations();
			tempSolution.Save(monitor);
			return tempSolution;
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x000245EB File Offset: 0x000227EB
		public bool IsSolutionItemFile(string filename)
		{
			if (filename.StartsWith("file://"))
			{
				filename = new Uri(filename).LocalPath;
			}
			filename = this.GetTargetFile(filename);
			return this.GetExtensionChain(null).IsSolutionItemFile(filename);
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0002461D File Offset: 0x0002281D
		public bool IsWorkspaceItemFile(string filename)
		{
			if (filename.StartsWith("file://"))
			{
				filename = new Uri(filename).LocalPath;
			}
			filename = this.GetTargetFile(filename);
			return this.GetExtensionChain(null).IsWorkspaceItemFile(filename);
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x0002464F File Offset: 0x0002284F
		internal bool IsSolutionItemFileInternal(string filename)
		{
			return this.formatManager.GetFileFormats(filename, typeof(SolutionItem)).Length > 0;
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0002466C File Offset: 0x0002286C
		internal bool IsWorkspaceItemFileInternal(string filename)
		{
			return this.formatManager.GetFileFormats(filename, typeof(WorkspaceItem)).Length > 0;
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0002468C File Offset: 0x0002288C
		internal void InitializeDataContext(DataContext ctx)
		{
			foreach (object obj in AddinManager.GetExtensionNodes("/MonoDevelop/ProjectModel/SerializableClasses"))
			{
				DataTypeCodon dataTypeCodon = (DataTypeCodon)obj;
				ctx.IncludeType(dataTypeCodon.Addin, dataTypeCodon.TypeName, dataTypeCodon.ItemName);
			}
			foreach (object obj2 in AddinManager.GetExtensionNodes("/MonoDevelop/ProjectModel/ExtendedProperties"))
			{
				ItemPropertyCodon itemPropertyCodon = (ItemPropertyCodon)obj2;
				ctx.RegisterProperty(itemPropertyCodon.Addin, itemPropertyCodon.TypeName, itemPropertyCodon.PropertyName, itemPropertyCodon.PropertyTypeName, itemPropertyCodon.External, itemPropertyCodon.SkipEmpty);
			}
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00024770 File Offset: 0x00022970
		private void OnFormatExtensionChanged(object s, ExtensionNodeEventArgs args)
		{
			FileFormatNode fileFormatNode = (FileFormatNode)args.ExtensionNode;
			if (args.Change == ExtensionChange.Add)
			{
				this.formatManager.RegisterFileFormat((IFileFormat)args.ExtensionObject, fileFormatNode.Id, fileFormatNode.Name, fileFormatNode.CanDefault);
				return;
			}
			this.formatManager.UnregisterFileFormat((IFileFormat)args.ExtensionObject);
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x000247D0 File Offset: 0x000229D0
		private void OnSerializableExtensionChanged(object s, ExtensionNodeEventArgs args)
		{
			if (args.Change == ExtensionChange.Add)
			{
				DataTypeCodon dataTypeCodon = (DataTypeCodon)args.ExtensionNode;
				this.DataContext.IncludeType(dataTypeCodon.Addin, dataTypeCodon.TypeName, dataTypeCodon.ItemName);
			}
			if (this.DataContextChanged != null)
			{
				this.DataContextChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x00024828 File Offset: 0x00022A28
		private void OnPropertiesExtensionChanged(object s, ExtensionNodeEventArgs args)
		{
			if (args.Change == ExtensionChange.Add)
			{
				ItemPropertyCodon itemPropertyCodon = (ItemPropertyCodon)args.ExtensionNode;
				this.DataContext.RegisterProperty(itemPropertyCodon.Addin, itemPropertyCodon.TypeName, itemPropertyCodon.PropertyName, itemPropertyCodon.PropertyTypeName, itemPropertyCodon.External, itemPropertyCodon.SkipEmpty);
			}
			else
			{
				ItemPropertyCodon itemPropertyCodon2 = (ItemPropertyCodon)args.ExtensionNode;
				this.DataContext.UnregisterProperty(itemPropertyCodon2.Addin, itemPropertyCodon2.TypeName, itemPropertyCodon2.PropertyName);
			}
			if (this.DataContextChanged != null)
			{
				this.DataContextChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x000248BC File Offset: 0x00022ABC
		private void OnProjectsExtensionChanged(object s, ExtensionNodeEventArgs args)
		{
			if (args.Change == ExtensionChange.Add)
			{
				this.projectBindings.Add(args.ExtensionNode);
			}
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x000248D8 File Offset: 0x00022AD8
		private void OnExtensionChanged(object s, ExtensionEventArgs args)
		{
			if (args.PathChanged("/MonoDevelop/ProjectModel/ProjectServiceExtensions"))
			{
				this.defaultExtensionChain = null;
			}
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x000248F0 File Offset: 0x00022AF0
		private string GetTargetFile(string file)
		{
			if (!Platform.IsWindows)
			{
				try
				{
					UnixSymbolicLinkInfo unixSymbolicLinkInfo = new UnixSymbolicLinkInfo(file);
					if (unixSymbolicLinkInfo.IsSymbolicLink)
					{
						return unixSymbolicLinkInfo.ContentsPath;
					}
				}
				catch
				{
				}
				return file;
			}
			return file;
		}

		// Token: 0x040002CB RID: 715
		public const string BuildTarget = "Build";

		// Token: 0x040002CC RID: 716
		public const string CleanTarget = "Clean";

		// Token: 0x040002CD RID: 717
		private const string FileFormatsExtensionPath = "/MonoDevelop/ProjectModel/FileFormats";

		// Token: 0x040002CE RID: 718
		private const string SerializableClassesExtensionPath = "/MonoDevelop/ProjectModel/SerializableClasses";

		// Token: 0x040002CF RID: 719
		private const string ExtendedPropertiesExtensionPath = "/MonoDevelop/ProjectModel/ExtendedProperties";

		// Token: 0x040002D0 RID: 720
		private const string ProjectBindingsExtensionPath = "/MonoDevelop/ProjectModel/ProjectBindings";

		// Token: 0x040002D1 RID: 721
		private DataContext dataContext = new DataContext();

		// Token: 0x040002D2 RID: 722
		private ArrayList projectBindings = new ArrayList();

		// Token: 0x040002D3 RID: 723
		private ProjectServiceExtension defaultExtensionChain;

		// Token: 0x040002D4 RID: 724
		private DefaultProjectServiceExtension extensionChainTerminator = new DefaultProjectServiceExtension();

		// Token: 0x040002D5 RID: 725
		private FileFormatManager formatManager = new FileFormatManager();

		// Token: 0x040002D6 RID: 726
		private FileFormat defaultFormat;

		// Token: 0x040002D7 RID: 727
		private TargetFramework defaultTargetFramework;

		// Token: 0x040002D8 RID: 728
		private string defaultPlatformTarget = "x86";

		// Token: 0x040002D9 RID: 729
		private static readonly TargetFrameworkMoniker DefaultTargetFrameworkId = TargetFrameworkMoniker.NET_4_5;

		// Token: 0x020000FD RID: 253
		private class ExtensionChainInfo
		{
			// Token: 0x040002DB RID: 731
			public ExtensionContext ExtensionContext;

			// Token: 0x040002DC RID: 732
			public ItemTypeCondition ItemTypeCondition;

			// Token: 0x040002DD RID: 733
			public ProjectLanguageCondition ProjectLanguageCondition;
		}
	}
}
