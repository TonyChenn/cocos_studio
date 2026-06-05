using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Xml;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.Core.Instrumentation;
using MonoDevelop.Core.ProgressMonitoring;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects.Extensions;
using MonoDevelop.Projects.Formats.MD1;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001B4 RID: 436
	public class MSBuildProjectHandler : MSBuildHandler, IResourceHandler, IPathHandler, IAssemblyReferenceHandler
	{
		// Token: 0x17000394 RID: 916
		// (get) Token: 0x0600106E RID: 4206 RVA: 0x0003CA97 File Offset: 0x0003AC97
		protected SolutionEntityItem EntityItem
		{
			get
			{
				return (SolutionEntityItem)base.Item;
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x0600106F RID: 4207 RVA: 0x0003CAA4 File Offset: 0x0003ACA4
		// (set) Token: 0x06001070 RID: 4208 RVA: 0x0003CAAC File Offset: 0x0003ACAC
		public string ToolsVersion { get; private set; }

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06001071 RID: 4209 RVA: 0x0003CAB5 File Offset: 0x0003ACB5
		internal bool ProjectTypeIsUnsupported
		{
			get
			{
				return this.unknownProjectTypeInfo != null;
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06001072 RID: 4210 RVA: 0x0003CAC3 File Offset: 0x0003ACC3
		internal UnknownProjectTypeNode UnknownProjectTypeInfo
		{
			get
			{
				return this.unknownProjectTypeInfo;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06001073 RID: 4211 RVA: 0x0003CACB File Offset: 0x0003ACCB
		public List<string> TargetImports
		{
			get
			{
				return this.targetImports;
			}
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x0003CAD3 File Offset: 0x0003ACD3
		internal void SetUnsupportedType(UnknownProjectTypeNode typeInfo)
		{
			this.unknownProjectTypeInfo = typeInfo;
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x0003CADC File Offset: 0x0003ACDC
		internal override void SetSolutionFormat(MSBuildFileFormat format, bool converting)
		{
			if (converting)
			{
				this.ToolsVersion = format.DefaultToolsVersion;
				this.productVersion = format.DefaultProductVersion;
				this.schemaVersion = format.DefaultSchemaVersion;
			}
			base.SetSolutionFormat(format, converting);
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x0003CB10 File Offset: 0x0003AD10
		private MSBuildFileFormat GetToolsFormat()
		{
			string toolsVersion;
			if ((toolsVersion = this.ToolsVersion) != null)
			{
				if (toolsVersion == "2.0")
				{
					return new MSBuildFileFormatVS05();
				}
				if (toolsVersion == "3.5")
				{
					return new MSBuildFileFormatVS08();
				}
				if (!(toolsVersion == "4.0"))
				{
					if (toolsVersion == "12.0")
					{
						return new MSBuildFileFormatVS12();
					}
				}
				else
				{
					if (base.SolutionFormat != null && base.SolutionFormat.Id == "MSBuild10")
					{
						return base.SolutionFormat;
					}
					return new MSBuildFileFormatVS12();
				}
			}
			throw new Exception("Unknown ToolsVersion '" + this.ToolsVersion + "'");
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x0003CBB5 File Offset: 0x0003ADB5
		public void SetCustomResourceHandler(IResourceHandler value)
		{
			this.customResourceHandler = value;
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06001078 RID: 4216 RVA: 0x0003CBBE File Offset: 0x0003ADBE
		public List<string> SubtypeGuids
		{
			get
			{
				return this.subtypeGuids;
			}
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x0003CBC6 File Offset: 0x0003ADC6
		public MSBuildProjectHandler()
		{
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x0003CBEF File Offset: 0x0003ADEF
		public MSBuildProjectHandler(string typeGuid, string import, string itemId)
		{
			this.Initialize(typeGuid, import, itemId);
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x0003CC24 File Offset: 0x0003AE24
		internal void Initialize(string typeGuid, string import, string itemId)
		{
			base.Initialize(typeGuid, itemId);
			if (import != null && import.Trim().Length > 0)
			{
				this.targetImports.AddRange(import.Split(new char[]
				{
					':'
				}));
			}
			Runtime.SystemAssemblyService.DefaultRuntimeChanged += this.OnDefaultRuntimeChanged;
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x0003CC80 File Offset: 0x0003AE80
		public override object GetService(Type t)
		{
			foreach (MSBuildExtension msbuildExtension in this.GetMSBuildExtensions())
			{
				object service = msbuildExtension.GetService(t);
				if (service != null)
				{
					return service;
				}
			}
			return null;
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x0003CCD8 File Offset: 0x0003AED8
		private void OnDefaultRuntimeChanged(object o, EventArgs args)
		{
			this.CleanupProjectBuilder();
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x0003CCE8 File Offset: 0x0003AEE8
		private RemoteProjectBuilder GetProjectBuilder()
		{
			SolutionEntityItem solutionEntityItem = (SolutionEntityItem)base.Item;
			IAssemblyProject assemblyProject = solutionEntityItem as IAssemblyProject;
			TargetRuntime targetRuntime = (assemblyProject != null) ? assemblyProject.TargetRuntime : Runtime.SystemAssemblyService.CurrentRuntime;
			Solution parentSolution = solutionEntityItem.ParentSolution;
			FilePath filePath = (parentSolution != null) ? parentSolution.FileName : null;
			lock (this.builderLock)
			{
				if (this.projectBuilder == null || this.lastBuildToolsVersion != this.ToolsVersion || this.lastBuildRuntime != targetRuntime.Id || this.lastFileName != solutionEntityItem.FileName || this.lastSlnFileName != filePath)
				{
					this.CleanupProjectBuilder();
					this.projectBuilder = MSBuildProjectService.GetProjectBuilder(targetRuntime, this.ToolsVersion, solutionEntityItem.FileName, filePath);
					this.projectBuilder.Disconnected += delegate(object param0, EventArgs param1)
					{
						this.CleanupProjectBuilder();
					};
					this.lastBuildToolsVersion = this.ToolsVersion;
					this.lastBuildRuntime = targetRuntime.Id;
					this.lastFileName = solutionEntityItem.FileName;
					this.lastSlnFileName = filePath;
				}
				if (this.modifiedInMemory)
				{
					this.modifiedInMemory = false;
					MSBuildProject msbuildProject = this.SaveProject(new NullProgressMonitor());
					this.projectBuilder.RefreshWithContent(msbuildProject.SaveToString());
				}
			}
			return this.projectBuilder;
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x0003CE88 File Offset: 0x0003B088
		internal void CleanupProjectBuilder()
		{
			if (this.projectBuilder != null)
			{
				this.projectBuilder.Dispose();
				this.projectBuilder = null;
			}
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x0003CEA4 File Offset: 0x0003B0A4
		internal void RefreshProjectBuilder()
		{
			if (this.projectBuilder != null)
			{
				this.projectBuilder.Refresh();
			}
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x0003CEB9 File Offset: 0x0003B0B9
		public override void Dispose()
		{
			base.Dispose();
			this.CleanupProjectBuilder();
			Runtime.SystemAssemblyService.DefaultRuntimeChanged -= this.OnDefaultRuntimeChanged;
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x0003CEDD File Offset: 0x0003B0DD
		private static string GetExplicitPlatform(SolutionItemConfiguration configObject)
		{
			if (string.IsNullOrEmpty(configObject.Platform))
			{
				return "AnyCPU";
			}
			return configObject.Platform;
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x0003CEF8 File Offset: 0x0003B0F8
		private ProjectConfigurationInfo[] GetConfigurations(SolutionEntityItem item, ConfigurationSelector configuration)
		{
			List<ProjectConfigurationInfo> list = new List<ProjectConfigurationInfo>();
			SolutionItemConfiguration configuration2 = item.GetConfiguration(configuration);
			list.Add(new ProjectConfigurationInfo
			{
				ProjectFile = item.FileName,
				Configuration = configuration2.Name,
				Platform = MSBuildProjectHandler.GetExplicitPlatform(configuration2),
				ProjectGuid = ((MSBuildProjectHandler)item.ItemHandler).ItemId
			});
			foreach (Project project in item.GetReferencedItems(configuration).OfType<Project>())
			{
				SolutionItemConfiguration configuration3 = project.GetConfiguration(configuration);
				if (configuration3 != null)
				{
					list.Add(new ProjectConfigurationInfo
					{
						ProjectFile = project.FileName,
						Configuration = configuration3.Name,
						Platform = MSBuildProjectHandler.GetExplicitPlatform(configuration3),
						ProjectGuid = ((MSBuildProjectHandler)project.ItemHandler).ItemId
					});
				}
			}
			return list.ToArray();
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x0003D3F8 File Offset: 0x0003B5F8
		IEnumerable<string> IAssemblyReferenceHandler.GetAssemblyReferences(ConfigurationSelector configuration)
		{
			if (this.UseMSBuildEngineForItem(base.Item, configuration, true))
			{
				SolutionEntityItem item = (SolutionEntityItem)base.Item;
				RemoteProjectBuilder builder = this.GetProjectBuilder();
				ProjectConfigurationInfo[] configs = this.GetConfigurations(item, configuration);
				string[] refs;
				using (Counters.ResolveMSBuildReferencesTimer.BeginTiming(base.Item.GetProjectEventMetadata()))
				{
					refs = builder.ResolveAssemblyReferences(configs);
				}
				foreach (string r in refs)
				{
					yield return r;
				}
			}
			else
			{
				this.CleanupProjectBuilder();
				DotNetProject item2 = base.Item as DotNetProject;
				if (item2 != null)
				{
					foreach (ProjectReference pref in from pr in item2.References
					where pr.ReferenceType != ReferenceType.Project
					select pr)
					{
						foreach (string asm in pref.GetReferencedFileNames(configuration))
						{
							yield return asm;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x0003D41C File Offset: 0x0003B61C
		public override BuildResult RunTarget(IProgressMonitor monitor, string target, ConfigurationSelector configuration)
		{
			if (this.UseMSBuildEngineForItem(base.Item, configuration, true))
			{
				SolutionEntityItem solutionEntityItem = base.Item as SolutionEntityItem;
				if (solutionEntityItem != null)
				{
					LogWriter logWriter = new LogWriter(monitor.Log);
					RemoteProjectBuilder remoteProjectBuilder = this.GetProjectBuilder();
					ProjectConfigurationInfo[] configurations = this.GetConfigurations(solutionEntityItem, configuration);
					TimerCounter timerCounter = null;
					if (target != null)
					{
						if (!(target == "Build"))
						{
							if (target == "Clean")
							{
								timerCounter = Counters.CleanMSBuildProjectTimer;
							}
						}
						else
						{
							timerCounter = Counters.BuildMSBuildProjectTimer;
						}
					}
					ITimeTracker timeTracker = Counters.RunMSBuildTargetTimer.BeginTiming(base.Item.GetProjectEventMetadata());
					ITimeTracker timeTracker2 = (timerCounter != null) ? timerCounter.BeginTiming(base.Item.GetProjectEventMetadata()) : null;
					MSBuildResult msbuildResult;
					try
					{
						msbuildResult = remoteProjectBuilder.Run(configurations, logWriter, MSBuildProjectService.DefaultMSBuildVerbosity, new string[]
						{
							target
						}, null, null);
					}
					finally
					{
						timeTracker.End();
						if (timeTracker2 != null)
						{
							timeTracker2.End();
						}
					}
					RemotingServices.Disconnect(logWriter);
					BuildResult buildResult = new BuildResult();
					foreach (MSBuildTargetResult msbuildTargetResult in msbuildResult.Errors)
					{
						FilePath filePath = null;
						if (msbuildTargetResult.File != null)
						{
							filePath = Path.Combine(Path.GetDirectoryName(msbuildTargetResult.ProjectFile), msbuildTargetResult.File);
						}
						buildResult.Append(new BuildError(filePath, msbuildTargetResult.LineNumber, msbuildTargetResult.ColumnNumber, msbuildTargetResult.Code, msbuildTargetResult.Message)
						{
							Subcategory = msbuildTargetResult.Subcategory,
							EndLine = msbuildTargetResult.EndLineNumber,
							EndColumn = msbuildTargetResult.EndColumnNumber,
							IsWarning = msbuildTargetResult.IsWarning
						});
					}
					return buildResult;
				}
			}
			else
			{
				this.CleanupProjectBuilder();
				if (base.Item is DotNetProject)
				{
					MD1DotNetProjectHandler md1DotNetProjectHandler = new MD1DotNetProjectHandler((DotNetProject)base.Item);
					return md1DotNetProjectHandler.RunTarget(monitor, target, configuration);
				}
			}
			return null;
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x0003D620 File Offset: 0x0003B820
		public string GetDefaultResourceId(ProjectFile file)
		{
			if (this.customResourceHandler != null)
			{
				return this.customResourceHandler.GetDefaultResourceId(file);
			}
			return MSBuildResourceHandler.Instance.GetDefaultResourceId(file);
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x0003D644 File Offset: 0x0003B844
		public string EncodePath(string path, string oldPath)
		{
			string directoryName = Path.GetDirectoryName(this.EntityItem.FileName);
			return FileService.RelativeToAbsolutePath(directoryName, path);
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x0003D670 File Offset: 0x0003B870
		public string DecodePath(string path)
		{
			string directoryName = Path.GetDirectoryName(this.EntityItem.FileName);
			return FileService.AbsoluteToRelativePath(directoryName, path);
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x0003D69C File Offset: 0x0003B89C
		public SolutionEntityItem Load(IProgressMonitor monitor, string fileName, MSBuildFileFormat format, string language, Type itemClass)
		{
			this.timer = Counters.ReadMSBuildProject.BeginTiming();
			this.timer.Trace("Reading project file");
			MSBuildProject msbuildProject = new MSBuildProject();
			msbuildProject.Load(fileName);
			this.ToolsVersion = msbuildProject.ToolsVersion;
			if (string.IsNullOrEmpty(this.ToolsVersion))
			{
				this.ToolsVersion = "2.0";
			}
			this.SetSolutionFormat(format ?? new MSBuildFileFormatVS12(), false);
			this.timer.Trace("Read project guids");
			MSBuildPropertySet msbuildPropertySet = msbuildProject.GetGlobalPropertyGroup();
			if (msbuildPropertySet == null)
			{
				msbuildPropertySet = msbuildProject.AddNewPropertyGroup(false);
			}
			this.productVersion = msbuildPropertySet.GetPropertyValue("ProductVersion", false);
			this.schemaVersion = msbuildPropertySet.GetPropertyValue("SchemaVersion", false);
			string text = msbuildPropertySet.GetPropertyValue("ProjectGuid", false);
			if (text == null)
			{
				throw new UserException("Project file doesn't have a valid ProjectGuid");
			}
			if (!text.StartsWith("{", StringComparison.Ordinal))
			{
				text = "{" + text + "}";
			}
			text = text.ToUpper();
			string propertyValue = msbuildPropertySet.GetPropertyValue("ProjectTypeGuids", false);
			string propertyValue2 = msbuildPropertySet.GetPropertyValue("ItemType", false);
			this.subtypeGuids.Clear();
			if (propertyValue != null)
			{
				foreach (string text2 in propertyValue.Split(new char[]
				{
					';'
				}))
				{
					string text3 = text2.Trim();
					if (text3.Length > 0 && string.Compare(text3, base.TypeGuid, StringComparison.OrdinalIgnoreCase) != 0)
					{
						this.subtypeGuids.Add(text2);
					}
				}
			}
			SolutionEntityItem result;
			try
			{
				this.timer.Trace("Create item instance");
				ProjectExtensionUtil.BeginLoadOperation();
				base.Item = this.CreateSolutionItem(monitor, msbuildProject, fileName, language, propertyValue2, itemClass);
				if (this.subtypeGuids.Any<string>())
				{
					string text4 = string.Join(";", this.subtypeGuids) + ";" + base.TypeGuid;
					base.Item.ExtendedProperties["ProjectTypeGuids"] = text4.ToUpper();
				}
				base.Item.SetItemHandler(this);
				MSBuildProjectService.SetId(base.Item, text);
				SolutionEntityItem solutionEntityItem = (SolutionEntityItem)base.Item;
				solutionEntityItem.FileName = fileName;
				solutionEntityItem.Name = Path.GetFileNameWithoutExtension(fileName);
				this.RemoveDuplicateItems(msbuildProject, fileName);
				this.LoadProject(monitor, msbuildProject);
				result = solutionEntityItem;
			}
			finally
			{
				ProjectExtensionUtil.EndLoadOperation();
				this.timer.End();
			}
			return result;
		}

		/// <summary>Whether to use the MSBuild engine for the specified item.</summary>
		// Token: 0x0600108A RID: 4234 RVA: 0x0003D940 File Offset: 0x0003BB40
		internal bool UseMSBuildEngineForItem(SolutionItem item, ConfigurationSelector sel, bool checkReferences = true)
		{
			if (this.RequireMSBuildEngine)
			{
				return true;
			}
			if (item.UseMSBuildEngine != null)
			{
				return item.UseMSBuildEngine.Value;
			}
			return this.UseMSBuildEngineByDefault && (!checkReferences || item.GetReferencedItems(sel).All(delegate(SolutionItem i)
			{
				MSBuildProjectHandler msbuildProjectHandler = i.ItemHandler as MSBuildProjectHandler;
				return msbuildProjectHandler != null && msbuildProjectHandler.UseMSBuildEngineForItem(i, sel, false);
			}));
		}

		/// <summary>Whether to use the MSBuild engine by default.</summary>
		// Token: 0x1700039A RID: 922
		// (get) Token: 0x0600108B RID: 4235 RVA: 0x0003D9AF File Offset: 0x0003BBAF
		// (set) Token: 0x0600108C RID: 4236 RVA: 0x0003D9B7 File Offset: 0x0003BBB7
		internal bool UseMSBuildEngineByDefault { get; set; }

		/// <summary>Forces the MSBuild engine to be used.</summary>
		// Token: 0x1700039B RID: 923
		// (get) Token: 0x0600108D RID: 4237 RVA: 0x0003D9C0 File Offset: 0x0003BBC0
		// (set) Token: 0x0600108E RID: 4238 RVA: 0x0003D9C8 File Offset: 0x0003BBC8
		internal bool RequireMSBuildEngine { get; set; }

		// Token: 0x0600108F RID: 4239 RVA: 0x0003D9E8 File Offset: 0x0003BBE8
		private SolutionItem CreateSolutionItem(IProgressMonitor monitor, MSBuildProject p, string fileName, string language, string itemType, Type itemClass)
		{
			if (this.ProjectTypeIsUnsupported)
			{
				return new UnknownProject(fileName, this.UnknownProjectTypeInfo.GetInstructions());
			}
			if (this.subtypeGuids.Any<string>())
			{
				DotNetProjectSubtypeNode dotNetProjectSubtypeNode = MSBuildProjectService.GetDotNetProjectSubtype(this.subtypeGuids);
				if (dotNetProjectSubtypeNode != null)
				{
					this.UseMSBuildEngineByDefault = dotNetProjectSubtypeNode.UseXBuild;
					this.RequireMSBuildEngine = dotNetProjectSubtypeNode.RequireXBuild;
					Type migratedType = null;
					if (dotNetProjectSubtypeNode.IsMigration && (migratedType = this.MigrateProject(monitor, dotNetProjectSubtypeNode, p, fileName, language)) != null)
					{
						DotNetProjectSubtypeNode dotNetProjectSubtypeNode2 = dotNetProjectSubtypeNode;
						dotNetProjectSubtypeNode = MSBuildProjectService.GetItemSubtypeNodes().Last((DotNetProjectSubtypeNode t) => t.CanHandleType(migratedType));
						for (int i = 0; i < this.subtypeGuids.Count; i++)
						{
							if (string.Equals(this.subtypeGuids[i], dotNetProjectSubtypeNode2.Guid, StringComparison.OrdinalIgnoreCase))
							{
								this.subtypeGuids[i] = dotNetProjectSubtypeNode.Guid;
								dotNetProjectSubtypeNode2 = null;
								break;
							}
						}
						if (dotNetProjectSubtypeNode2 != null)
						{
							throw new Exception("Unable to correct flavor GUID");
						}
						string text = string.Join(";", this.subtypeGuids) + ";" + base.TypeGuid;
						p.GetGlobalPropertyGroup().SetPropertyValue("ProjectTypeGuids", text.ToUpper(), true, false);
						p.Save(fileName);
					}
					DotNetProject dotNetProject = dotNetProjectSubtypeNode.CreateInstance(language);
					dotNetProjectSubtypeNode.UpdateImports(dotNetProject, this.targetImports);
					return dotNetProject;
				}
				UnknownProjectTypeNode unknownProjectTypeNode = MSBuildProjectService.GetUnknownProjectTypeInfo(this.subtypeGuids.ToArray(), fileName);
				if (unknownProjectTypeNode != null && unknownProjectTypeNode.LoadFiles)
				{
					this.SetUnsupportedType(unknownProjectTypeNode);
					return new UnknownProject(fileName, this.UnknownProjectTypeInfo.GetInstructions());
				}
				throw new UnknownSolutionItemTypeException(this.ProjectTypeIsUnsupported ? base.TypeGuid : string.Join(";", this.subtypeGuids));
			}
			else
			{
				if (itemClass != null)
				{
					return (SolutionItem)Activator.CreateInstance(itemClass);
				}
				if (!string.IsNullOrEmpty(language))
				{
					this.UseMSBuildEngineByDefault = true;
					this.RequireMSBuildEngine = false;
					return new DotNetAssemblyProject(language);
				}
				if (string.IsNullOrEmpty(itemType))
				{
					throw new UnknownSolutionItemTypeException();
				}
				DataType configurationDataType = MSBuildProjectService.DataContext.GetConfigurationDataType(itemType);
				if (configurationDataType == null)
				{
					throw new UnknownSolutionItemTypeException(itemType);
				}
				return (SolutionItem)Activator.CreateInstance(configurationDataType.ValueType);
			}
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x0003DC38 File Offset: 0x0003BE38
		private Type MigrateProject(IProgressMonitor monitor, DotNetProjectSubtypeNode st, MSBuildProject p, string fileName, string language)
		{
			IProjectLoadProgressMonitor projectLoadProgressMonitor = monitor as IProjectLoadProgressMonitor;
			if (projectLoadProgressMonitor == null)
			{
				if (!st.IsMigrationRequired)
				{
					return null;
				}
				LoggingService.LogError(Environment.StackTrace);
				monitor.ReportError("Could not open unmigrated project and no migrator was supplied", null);
				throw new Exception("Could not open unmigrated project and no migrator was supplied");
			}
			else
			{
				MigrationType migrationType = st.MigrationHandler.CanPromptForMigration ? st.MigrationHandler.PromptForMigration(projectLoadProgressMonitor, p, fileName, language) : projectLoadProgressMonitor.ShouldMigrateProject();
				if (migrationType == MigrationType.Ignore)
				{
					if (st.IsMigrationRequired)
					{
						monitor.ReportError(string.Format("{1} cannot open the project '{0}' unless it is migrated.", Path.GetFileName(fileName), BrandingService.ApplicationName), null);
						throw new Exception("The user choose not to migrate the project");
					}
					return null;
				}
				else
				{
					FilePath filePath = Path.GetDirectoryName(fileName);
					if (migrationType == MigrationType.BackupAndMigrate)
					{
						FilePath filePath2 = filePath.Combine(new string[]
						{
							"backup"
						});
						string text = filePath2;
						int num = 0;
						while (Directory.Exists(text))
						{
							text = filePath2 + "-" + num.ToString();
							if (num++ > 20)
							{
								throw new Exception("Too many backup directories");
							}
						}
						Directory.CreateDirectory(text);
						foreach (string text2 in st.MigrationHandler.FilesToBackup(fileName))
						{
							File.Copy(text2, Path.Combine(text, Path.GetFileName(text2)));
						}
					}
					Type type = st.MigrationHandler.Migrate(projectLoadProgressMonitor, p, fileName, language);
					if (type == null)
					{
						throw new Exception("Could not migrate the project");
					}
					return type;
				}
			}
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0003DDD8 File Offset: 0x0003BFD8
		private FileFormat GetFileFormat(MSBuildFileFormat fmt)
		{
			return new FileFormat(fmt, fmt.Id, fmt.Name);
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x0003DDEC File Offset: 0x0003BFEC
		private void RemoveDuplicateItems(MSBuildProject msproject, string fileName)
		{
			this.timer.Trace("Checking for duplicate items");
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			List<MSBuildItem> list = new List<MSBuildItem>();
			foreach (MSBuildItem msbuildItem in msproject.GetAllItems())
			{
				string key = msbuildItem.Name + "<" + msbuildItem.Include;
				object obj;
				if (!dictionary.TryGetValue(key, out obj))
				{
					dictionary[key] = msbuildItem;
				}
				else
				{
					MSBuildItem msbuildItem2 = obj as MSBuildItem;
					if (msbuildItem2 != null)
					{
						if (msbuildItem2.Condition != msbuildItem.Condition || msbuildItem2.Element.InnerXml != msbuildItem.Element.InnerXml)
						{
							dictionary[key] = new List<MSBuildItem>
							{
								msbuildItem2,
								msbuildItem
							};
						}
						else
						{
							list.Add(msbuildItem);
						}
					}
					else
					{
						List<MSBuildItem> list2 = (List<MSBuildItem>)obj;
						bool flag = false;
						foreach (MSBuildItem msbuildItem3 in list2)
						{
							if (msbuildItem3.Condition == msbuildItem.Condition && msbuildItem3.Element.InnerXml == msbuildItem.Element.InnerXml)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							list2.Add(msbuildItem);
						}
						else
						{
							list.Add(msbuildItem);
						}
					}
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			this.timer.Trace("Removing duplicate items");
			foreach (MSBuildItem item in list)
			{
				msproject.RemoveItem(item);
			}
			msproject.Save(fileName);
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x0003E010 File Offset: 0x0003C210
		private void LoadConfiguration(MSBuildSerializer serializer, List<MSBuildProjectHandler.ConfigData> configData, string conf, string platform)
		{
			MSBuildPropertySet mergedConfiguration = this.GetMergedConfiguration(configData, conf, platform, null);
			SolutionItemConfiguration solutionItemConfiguration = this.EntityItem.CreateConfiguration(conf);
			solutionItemConfiguration.Platform = platform;
			DataItem data = this.ReadPropertyGroupMetadata(serializer, mergedConfiguration, solutionItemConfiguration);
			serializer.Deserialize(solutionItemConfiguration, data);
			this.EntityItem.Configurations.Add(solutionItemConfiguration);
			if (solutionItemConfiguration is DotNetProjectConfiguration)
			{
				DotNetProjectConfiguration dotNetProjectConfiguration = (DotNetProjectConfiguration)solutionItemConfiguration;
				if (dotNetProjectConfiguration.CompilationParameters != null)
				{
					data = this.ReadPropertyGroupMetadata(serializer, mergedConfiguration, dotNetProjectConfiguration.CompilationParameters);
					serializer.Deserialize(dotNetProjectConfiguration.CompilationParameters, data);
				}
			}
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x0003E094 File Offset: 0x0003C294
		protected virtual void LoadProject(IProgressMonitor monitor, MSBuildProject msproject)
		{
			this.timer.Trace("Initialize serialization");
			MSBuildSerializer msbuildSerializer = this.CreateSerializer();
			msbuildSerializer.SerializationContext.BaseFile = this.EntityItem.FileName;
			msbuildSerializer.SerializationContext.ProgressMonitor = monitor;
			MSBuildPropertySet globalPropertyGroup = msproject.GetGlobalPropertyGroup();
			base.Item.SetItemHandler(this);
			DotNetProject dotNetProject = base.Item as DotNetProject;
			this.timer.Trace("Read project items");
			this.LoadProjectItems(msproject, msbuildSerializer, ProjectItemFlags.None);
			this.timer.Trace("Read configurations");
			TargetFrameworkMoniker id = null;
			if (dotNetProject != null)
			{
				string propertyValue = globalPropertyGroup.GetPropertyValue("TargetFrameworkIdentifier", false);
				string propertyValue2 = globalPropertyGroup.GetPropertyValue("TargetFrameworkVersion", false);
				string propertyValue3 = globalPropertyGroup.GetPropertyValue("TargetFrameworkProfile", false);
				TargetFrameworkMoniker defaultTargetFrameworkForFormat = dotNetProject.GetDefaultTargetFrameworkForFormat(this.GetFileFormat(this.GetToolsFormat()));
				id = new TargetFrameworkMoniker(string.IsNullOrEmpty(propertyValue) ? defaultTargetFrameworkForFormat.Identifier : propertyValue, string.IsNullOrEmpty(propertyValue2) ? defaultTargetFrameworkForFormat.Version : propertyValue2, string.IsNullOrEmpty(propertyValue3) ? defaultTargetFrameworkForFormat.Profile : propertyValue3);
				if (dotNetProject.LanguageParameters != null)
				{
					DataItem data = this.ReadPropertyGroupMetadata(msbuildSerializer, globalPropertyGroup, dotNetProject.LanguageParameters);
					msbuildSerializer.Deserialize(dotNetProject.LanguageParameters, data);
				}
			}
			List<MSBuildProjectHandler.ConfigData> configData = this.GetConfigData(msproject, false);
			List<MSBuildProjectHandler.ConfigData> list = new List<MSBuildProjectHandler.ConfigData>();
			HashSet<string> hashSet = new HashSet<string>();
			HashSet<string> hashSet2 = new HashSet<string>();
			HashSet<string> hashSet3 = new HashSet<string>();
			MSBuildPropertyGroup grp = this.ExtractMergedtoprojectProperties(msbuildSerializer, globalPropertyGroup, this.EntityItem.CreateConfiguration("Dummy"));
			configData.Insert(0, new MSBuildProjectHandler.ConfigData(null, null, grp));
			for (int i = 1; i < configData.Count; i++)
			{
				MSBuildProjectHandler.ConfigData configData2 = configData[i];
				string platform = configData2.Platform;
				string config = configData2.Config;
				if (platform != null)
				{
					hashSet3.Add(platform);
				}
				if (config != null)
				{
					hashSet2.Add(config);
				}
				if (config == null || platform == null)
				{
					list.Add(configData2);
				}
				else
				{
					string item = config + "|" + platform;
					if (!hashSet.Contains(item))
					{
						this.LoadConfiguration(msbuildSerializer, configData, config, platform);
						hashSet.Add(item);
					}
				}
			}
			if (list.Count > 0)
			{
				if (hashSet3.Count == 0)
				{
					hashSet3.Add(string.Empty);
				}
				foreach (MSBuildProjectHandler.ConfigData configData3 in list)
				{
					if (configData3.Config != null && configData3.Platform == null)
					{
						string config2 = configData3.Config;
						using (HashSet<string>.Enumerator enumerator2 = hashSet3.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								string text = enumerator2.Current;
								string item2 = config2 + "|" + text;
								if (!hashSet.Contains(item2))
								{
									this.LoadConfiguration(msbuildSerializer, configData, config2, text);
									hashSet.Add(item2);
								}
							}
							continue;
						}
					}
					if (configData3.Config == null && configData3.Platform != null)
					{
						string platform2 = configData3.Platform;
						foreach (string text2 in hashSet2)
						{
							string item3 = text2 + "|" + platform2;
							if (!hashSet.Contains(item3))
							{
								this.LoadConfiguration(msbuildSerializer, configData, text2, platform2);
								hashSet.Add(item3);
							}
						}
					}
				}
			}
			this.timer.Trace("Read extended properties");
			DataItem dataItem = this.ReadPropertyGroupMetadata(msbuildSerializer, globalPropertyGroup, base.Item);
			string projectExtensions = msproject.GetProjectExtensions("MonoDevelop");
			if (!string.IsNullOrEmpty(projectExtensions))
			{
				StringReader input = new StringReader(projectExtensions);
				DataItem dataItem2 = (DataItem)XmlConfigurationReader.DefaultReader.Read(new XmlTextReader(input));
				dataItem.ItemData.AddRange(dataItem2.ItemData);
			}
			msbuildSerializer.Deserialize(base.Item, dataItem);
			this.timer.Trace("Final initializations");
			if (dotNetProject != null)
			{
				string value = base.Item.ExtendedProperties["InternalTargetFrameworkVersion"] as string;
				if (!string.IsNullOrEmpty(value))
				{
					id = TargetFrameworkMoniker.Parse(value);
					base.Item.ExtendedProperties.Remove("InternalTargetFrameworkVersion");
				}
				dotNetProject.TargetFramework = Runtime.SystemAssemblyService.GetTargetFramework(id);
			}
			this.LoadFromMSBuildProject(monitor, msproject);
			base.Item.NeedsReload = false;
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x0003E57C File Offset: 0x0003C77C
		internal void LoadProjectItems(MSBuildProject msproject, MSBuildSerializer ser, ProjectItemFlags flags)
		{
			foreach (MSBuildItem buildItem in msproject.GetAllItems())
			{
				ProjectItem projectItem = this.ReadItem(ser, buildItem);
				if (projectItem != null)
				{
					projectItem.Flags = flags;
					if (projectItem is ProjectFile)
					{
						ProjectFile projectFile = (ProjectFile)projectItem;
						if (projectFile.Name.IndexOf('*') > -1)
						{
							foreach (ProjectFile item in MSBuildProjectHandler.ResolveWildcardItems(projectFile))
							{
								this.EntityItem.Items.Add(item);
							}
							this.EntityItem.WildcardItems.Add(projectItem);
							continue;
						}
						if (this.ProjectTypeIsUnsupported && !File.Exists(projectFile.FilePath))
						{
							continue;
						}
					}
					this.EntityItem.Items.Add(projectItem);
					projectItem.ExtendedProperties["MSBuild.SourceProject"] = msproject.FileName;
				}
			}
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x0003E6A4 File Offset: 0x0003C8A4
		protected virtual void LoadFromMSBuildProject(IProgressMonitor monitor, MSBuildProject msproject)
		{
			foreach (MSBuildExtension msbuildExtension in this.GetMSBuildExtensions())
			{
				msbuildExtension.LoadProject(monitor, this.EntityItem, msproject);
			}
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x0003E6F8 File Offset: 0x0003C8F8
		private static string GetWildcardDirectoryName(string path)
		{
			int num = path.LastIndexOfAny(MSBuildProjectHandler.directorySeparators);
			if (num < 0)
			{
				return string.Empty;
			}
			return path.Substring(0, num);
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x0003E724 File Offset: 0x0003C924
		private static string GetWildcardFileName(string path)
		{
			int num = path.LastIndexOfAny(MSBuildProjectHandler.directorySeparators);
			if (num < 0)
			{
				return path;
			}
			if (num == path.Length)
			{
				return string.Empty;
			}
			return path.Substring(num + 1, path.Length - (num + 1));
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x0003E768 File Offset: 0x0003C968
		private static IEnumerable<string> ExpandWildcardFilePath(string filePath)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				throw new ArgumentException("Not a wildcard path");
			}
			string text = MSBuildProjectHandler.GetWildcardDirectoryName(filePath);
			string wildcardFileName = MSBuildProjectHandler.GetWildcardFileName(filePath);
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(wildcardFileName))
			{
				return null;
			}
			SearchOption searchOption = SearchOption.TopDirectoryOnly;
			if (text.EndsWith("**", StringComparison.Ordinal))
			{
				text = text.Substring(0, text.Length - "**".Length);
				searchOption = SearchOption.AllDirectories;
			}
			if (!Directory.Exists(text))
			{
				return null;
			}
			return Directory.GetFiles(text, wildcardFileName, searchOption);
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x0003E9C4 File Offset: 0x0003CBC4
		private static IEnumerable<ProjectFile> ResolveWildcardItems(ProjectFile wildcardFile)
		{
			IEnumerable<string> paths = MSBuildProjectHandler.ExpandWildcardFilePath(wildcardFile.Name);
			if (paths != null)
			{
				foreach (string resolvedFilePath in paths)
				{
					ProjectFile projectFile = (ProjectFile)wildcardFile.Clone();
					projectFile.Name = resolvedFilePath;
					projectFile.IsOriginatedFromWildcard = true;
					yield return projectFile;
				}
			}
			yield break;
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0003E9E4 File Offset: 0x0003CBE4
		private MSBuildPropertyGroup ExtractMergedtoprojectProperties(MSBuildSerializer ser, MSBuildPropertySet pgroup, SolutionItemConfiguration ob)
		{
			XmlDocument xmlDocument = new XmlDocument();
			MSBuildPropertyGroup msbuildPropertyGroup = new MSBuildPropertyGroup(null, xmlDocument.CreateElement("PropGroup"));
			ClassDataType classDataType = (ClassDataType)ser.DataContext.GetConfigurationDataType(ob.GetType());
			foreach (ItemProperty itemProperty in classDataType.GetProperties(ser.SerializationContext, ob))
			{
				MSBuildProperty property = pgroup.GetProperty(itemProperty.Name);
				if (property != null)
				{
					bool preserveExistingCase = itemProperty.DataType is MSBuildBoolDataType;
					msbuildPropertyGroup.SetPropertyValue(property.Name, property.Element.InnerXml, preserveExistingCase, true);
				}
			}
			if (ob is DotNetProjectConfiguration)
			{
				object compilationParameters = ((DotNetProjectConfiguration)ob).CompilationParameters;
				classDataType = (ClassDataType)ser.DataContext.GetConfigurationDataType(compilationParameters.GetType());
				foreach (ItemProperty itemProperty2 in classDataType.GetProperties(ser.SerializationContext, compilationParameters))
				{
					MSBuildProperty property2 = pgroup.GetProperty(itemProperty2.Name);
					if (property2 != null)
					{
						bool preserveExistingCase2 = itemProperty2.DataType is MSBuildBoolDataType;
						msbuildPropertyGroup.SetPropertyValue(property2.Name, property2.Element.InnerXml, preserveExistingCase2, true);
					}
				}
			}
			return msbuildPropertyGroup;
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0003ED6C File Offset: 0x0003CF6C
		private IEnumerable<MSBuildProjectHandler.MergedProperty> GetMergeToProjectProperties(MSBuildSerializer ser, object ob)
		{
			ClassDataType dt = (ClassDataType)ser.DataContext.GetConfigurationDataType(ob.GetType());
			foreach (ItemProperty prop in dt.GetProperties(ser.SerializationContext, ob))
			{
				if (this.IsMergeToProjectProperty(prop))
				{
					yield return new MSBuildProjectHandler.MergedProperty(prop.Name, prop.DataType is MSBuildBoolDataType);
				}
			}
			yield break;
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x0003ED98 File Offset: 0x0003CF98
		internal ProjectItem ReadItem(MSBuildSerializer ser, MSBuildItem buildItem)
		{
			Project project = base.Item as Project;
			DotNetProject dotNetProject = base.Item as DotNetProject;
			DataType dataType = ser.DataContext.GetConfigurationDataType(buildItem.Name);
			if (project != null)
			{
				if (buildItem.Name == "Folder")
				{
					string path = MSBuildProjectService.FromMSBuildPath(project.ItemDirectory, buildItem.Include);
					return new ProjectFile
					{
						Name = Path.GetDirectoryName(path),
						Subtype = Subtype.Directory
					};
				}
				if (buildItem.Name == "Reference" && dotNetProject != null)
				{
					ProjectReference projectReference;
					if (buildItem.HasMetadata("HintPath"))
					{
						string metadata = buildItem.GetMetadata("HintPath", false);
						string text;
						if (!MSBuildProjectService.FromMSBuildPath(dotNetProject.ItemDirectory, metadata, out text))
						{
							projectReference = new ProjectReference(ReferenceType.Assembly, text);
							projectReference.SetInvalid(GettextCatalog.GetString("Invalid file path"));
							projectReference.ExtendedProperties["_OriginalMSBuildReferenceInclude"] = buildItem.Include;
							projectReference.ExtendedProperties["_OriginalMSBuildReferenceHintPath"] = metadata;
						}
						else
						{
							ReferenceType referenceType = File.Exists(text) ? ReferenceType.Assembly : ReferenceType.Package;
							projectReference = new ProjectReference(referenceType, buildItem.Include, text);
							projectReference.ExtendedProperties["_OriginalMSBuildReferenceHintPath"] = metadata;
							if (MSBuildProjectService.IsAbsoluteMSBuildPath(metadata))
							{
								projectReference.ExtendedProperties["_OriginalMSBuildReferenceIsAbsolute"] = true;
							}
						}
					}
					else
					{
						string text2 = buildItem.Include;
						if (text2 == "System.configuration")
						{
							text2 = "System.Configuration";
						}
						else if (text2 == "System.XML")
						{
							text2 = "System.Xml";
						}
						else if (text2 == "system")
						{
							text2 = "System";
						}
						projectReference = new ProjectReference(ReferenceType.Package, text2);
					}
					bool? boolMetadata = buildItem.GetBoolMetadata("Private");
					if (boolMetadata != null)
					{
						projectReference.LocalCopy = boolMetadata.Value;
					}
					projectReference.Condition = buildItem.Condition;
					string metadata2 = buildItem.GetMetadata("SpecificVersion", false);
					if (string.IsNullOrWhiteSpace(metadata2))
					{
						projectReference.SpecificVersion = this.ReferenceStringHasVersion(buildItem.Include);
					}
					else
					{
						bool flag;
						projectReference.SpecificVersion = (bool.TryParse(metadata2, out flag) && flag);
					}
					this.ReadBuildItemMetadata(ser, buildItem, projectReference, typeof(ProjectReference));
					return projectReference;
				}
				if (buildItem.Name == "ProjectReference" && dotNetProject != null)
				{
					string path2 = MSBuildProjectService.FromMSBuildPath(project.ItemDirectory, buildItem.Include);
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path2);
					ProjectReference projectReference2 = new ProjectReference(ReferenceType.Project, fileNameWithoutExtension);
					projectReference2.Condition = buildItem.Condition;
					bool? boolMetadata2 = buildItem.GetBoolMetadata("Private");
					if (boolMetadata2 != null)
					{
						projectReference2.LocalCopy = boolMetadata2.Value;
					}
					bool? boolMetadata3 = buildItem.GetBoolMetadata("ReferenceOutputAssembly");
					projectReference2.ReferenceOutputAssembly = (boolMetadata3 == null || boolMetadata3.Value);
					this.ReadBuildItemMetadata(ser, buildItem, projectReference2, typeof(ProjectReference));
					return projectReference2;
				}
				if (dataType == null && !string.IsNullOrEmpty(buildItem.Include) && !MSBuildProjectHandler.UnsupportedItems.Contains(buildItem.Name) && this.IsValidFile(buildItem.Include))
				{
					return this.ReadProjectFile(ser, project, buildItem, typeof(ProjectFile));
				}
			}
			if (dataType != null && dataType.ValueType == typeof(ProjectReference) && dotNetProject == null)
			{
				dataType = null;
			}
			if (dataType != null && typeof(ProjectItem).IsAssignableFrom(dataType.ValueType))
			{
				ProjectItem projectItem = (ProjectItem)Activator.CreateInstance(dataType.ValueType);
				this.ReadBuildItemMetadata(ser, buildItem, projectItem, dataType.ValueType);
				return projectItem;
			}
			UnknownProjectItem unknownProjectItem = new UnknownProjectItem(buildItem.Name, "");
			this.ReadBuildItemMetadata(ser, buildItem, unknownProjectItem, typeof(UnknownProjectItem));
			return unknownProjectItem;
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x0003F16C File Offset: 0x0003D36C
		private bool ReferenceStringHasVersion(string asmName)
		{
			int num = asmName.IndexOf(',');
			return num >= 0 && asmName.IndexOf("Version", num, StringComparison.Ordinal) >= 0;
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x0003F19C File Offset: 0x0003D39C
		private bool IsValidFile(string path)
		{
			try
			{
				if (Uri.IsWellFormedUriString(path, UriKind.Absolute))
				{
					Uri uri = new Uri(path);
					return uri.Scheme == "file";
				}
			}
			catch
			{
			}
			return true;
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x0003F1E4 File Offset: 0x0003D3E4
		private MSBuildPropertySet GetMergedConfiguration(List<MSBuildProjectHandler.ConfigData> configData, string conf, string platform, MSBuildPropertyGroup propGroupLimit)
		{
			MSBuildPropertySet msbuildPropertySet = null;
			foreach (MSBuildProjectHandler.ConfigData configData2 in configData)
			{
				if (configData2.Group == propGroupLimit)
				{
					break;
				}
				if ((configData2.Config == conf || configData2.Config == null || conf == null) && (configData2.Platform == platform || configData2.Platform == null || platform == null))
				{
					if (msbuildPropertySet == null)
					{
						msbuildPropertySet = configData2.Group;
					}
					else if (msbuildPropertySet is MSBuildPropertyGroupMerged)
					{
						((MSBuildPropertyGroupMerged)msbuildPropertySet).Add(configData2.Group);
					}
					else
					{
						MSBuildPropertyGroupMerged msbuildPropertyGroupMerged = new MSBuildPropertyGroupMerged();
						msbuildPropertyGroupMerged.Add((MSBuildPropertyGroup)msbuildPropertySet);
						msbuildPropertyGroupMerged.Add(configData2.Group);
						msbuildPropertySet = msbuildPropertyGroupMerged;
					}
				}
			}
			return msbuildPropertySet;
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x0003F2D4 File Offset: 0x0003D4D4
		private bool ContainsSpecificPlatformConfiguration(List<MSBuildProjectHandler.ConfigData> configData, string conf)
		{
			foreach (MSBuildProjectHandler.ConfigData configData2 in configData)
			{
				if (configData2.Config == conf && configData2.Platform != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x0003F340 File Offset: 0x0003D540
		public override void OnModified(string hint)
		{
			base.OnModified(hint);
			this.modifiedInMemory = true;
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x0003F350 File Offset: 0x0003D550
		protected override void SaveItem(IProgressMonitor monitor)
		{
			this.modifiedInMemory = false;
			MSBuildProject msbuildProject = this.SaveProject(monitor);
			if (msbuildProject == null)
			{
				return;
			}
			msbuildProject.Save(this.EntityItem.FileName);
			if (this.projectBuilder != null)
			{
				this.projectBuilder.Refresh();
			}
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x0003F3C4 File Offset: 0x0003D5C4
		protected virtual MSBuildProject SaveProject(IProgressMonitor monitor)
		{
			if (base.Item is UnknownSolutionItem)
			{
				return null;
			}
			MSBuildFileFormat toolsFormat = this.GetToolsFormat();
			SolutionEntityItem entityItem = this.EntityItem;
			MSBuildSerializer msbuildSerializer = this.CreateSerializer();
			msbuildSerializer.SerializationContext.BaseFile = entityItem.FileName;
			msbuildSerializer.SerializationContext.ProgressMonitor = monitor;
			DotNetProject dotNetProject = base.Item as DotNetProject;
			MSBuildProject msbuildProject = new MSBuildProject();
			bool flag = this.EntityItem.FileName == null || !File.Exists(this.EntityItem.FileName);
			if (flag)
			{
				msbuildProject.DefaultTargets = "Build";
			}
			else
			{
				msbuildProject.Load(this.EntityItem.FileName);
			}
			MSBuildPropertySet msbuildPropertySet = msbuildProject.GetGlobalPropertyGroup();
			if (msbuildPropertySet == null)
			{
				msbuildPropertySet = msbuildProject.AddNewPropertyGroup(false);
			}
			if (entityItem.Configurations.Count > 0)
			{
				ItemConfiguration itemConfiguration = entityItem.Configurations.FirstOrDefault((ItemConfiguration c) => c.Name == "Debug");
				if (itemConfiguration == null)
				{
					itemConfiguration = entityItem.Configurations[0];
				}
				MSBuildProperty msbuildProperty = msbuildPropertySet.SetPropertyValue("Configuration", itemConfiguration.Name, false, false);
				msbuildProperty.Condition = " '$(Configuration)' == '' ";
				string value = (itemConfiguration.Platform.Length == 0) ? "AnyCPU" : itemConfiguration.Platform;
				msbuildProperty = msbuildPropertySet.SetPropertyValue("Platform", value, false, false);
				msbuildProperty.Condition = " '$(Platform)' == '' ";
			}
			if (base.TypeGuid == "{9344BDBB-3E7F-41FC-A0DD-8665D75EE146}")
			{
				DataType configurationDataType = MSBuildProjectService.DataContext.GetConfigurationDataType(base.Item.GetType());
				msbuildPropertySet.SetPropertyValue("ItemType", configurationDataType.Name, false, false);
			}
			base.Item.ExtendedProperties["ProjectGuid"] = base.Item.ItemId;
			if (this.subtypeGuids.Count > 0)
			{
				string text = "";
				foreach (string str in this.subtypeGuids)
				{
					if (text.Length > 0)
					{
						text += ";";
					}
					text += str;
				}
				text = text + ";" + base.TypeGuid;
				base.Item.ExtendedProperties["ProjectTypeGuids"] = text.ToUpper();
				msbuildPropertySet.SetPropertyValue("ProjectTypeGuids", text.ToUpper(), true, false);
			}
			else
			{
				base.Item.ExtendedProperties.Remove("ProjectTypeGuids");
				msbuildPropertySet.RemoveProperty("ProjectTypeGuids");
			}
			base.Item.ExtendedProperties["ProductVersion"] = this.productVersion;
			base.Item.ExtendedProperties["SchemaVersion"] = this.schemaVersion;
			if (this.ToolsVersion != "2.0")
			{
				msbuildProject.ToolsVersion = this.ToolsVersion;
			}
			else if (string.IsNullOrEmpty(msbuildProject.ToolsVersion))
			{
				msbuildProject.ToolsVersion = null;
			}
			else
			{
				msbuildProject.ToolsVersion = "2.0";
			}
			msbuildSerializer.Serialize(base.Item, base.Item.GetType());
			object obj = null;
			if (dotNetProject != null && dotNetProject.LanguageParameters != null)
			{
				ClassDataType classDataType = (ClassDataType)msbuildSerializer.DataContext.GetConfigurationDataType(dotNetProject.LanguageParameters.GetType());
				foreach (ItemProperty itemProperty in classDataType.GetProperties(msbuildSerializer.SerializationContext, dotNetProject.LanguageParameters))
				{
					DataNode dataNode = msbuildSerializer.InternalItemProperties.ItemData[itemProperty.Name];
					if (dataNode != null)
					{
						msbuildSerializer.InternalItemProperties.ItemData.Remove(dataNode);
					}
				}
				DataItem dataItem = (DataItem)msbuildSerializer.Serialize(dotNetProject.LanguageParameters);
				msbuildSerializer.InternalItemProperties.ItemData.AddRange(dataItem.ItemData);
				obj = dotNetProject.LanguageParameters;
			}
			if (flag)
			{
				msbuildSerializer.InternalItemProperties.ItemData.Sort(MSBuildProjectHandler.globalConfigOrder);
			}
			this.WritePropertyGroupMetadata(msbuildPropertySet, msbuildSerializer.InternalItemProperties.ItemData, msbuildSerializer, new object[]
			{
				base.Item,
				obj
			});
			foreach (SolutionItemConfiguration solutionItemConfiguration in entityItem.Configurations)
			{
				if (flag && solutionItemConfiguration is DotNetProjectConfiguration)
				{
					solutionItemConfiguration.ExtendedProperties["ErrorReport"] = "prompt";
				}
			}
			if (entityItem.Configurations.Count > 0)
			{
				List<MSBuildProjectHandler.ConfigData> configData = this.GetConfigData(msbuildProject, true);
				Dictionary<string, MSBuildProjectHandler.MergedPropertyValue> dictionary = new Dictionary<string, MSBuildProjectHandler.MergedPropertyValue>();
				HashSet<MSBuildProjectHandler.MergedProperty> hashSet = new HashSet<MSBuildProjectHandler.MergedProperty>(this.GetMergeToProjectProperties(msbuildSerializer, entityItem.Configurations[0]));
				HashSet<string> hashSet2 = new HashSet<string>(from p in hashSet
				select p.Name);
				foreach (SolutionItemConfiguration solutionItemConfiguration2 in entityItem.Configurations)
				{
					bool flag2 = false;
					MSBuildProjectHandler.ConfigData configData2 = this.FindPropertyGroup(configData, solutionItemConfiguration2);
					if (configData2 == null)
					{
						MSBuildPropertyGroup msbuildPropertyGroup = msbuildProject.AddNewPropertyGroup(true);
						msbuildPropertyGroup.Condition = this.BuildConfigCondition(solutionItemConfiguration2.Name, solutionItemConfiguration2.Platform);
						configData2 = new MSBuildProjectHandler.ConfigData(solutionItemConfiguration2.Name, solutionItemConfiguration2.Platform, msbuildPropertyGroup);
						configData2.IsNew = true;
						configData.Add(configData2);
						flag2 = true;
					}
					MSBuildPropertyGroup group = configData2.Group;
					configData2.Exists = true;
					MSBuildPropertySet mergedConfiguration = this.GetMergedConfiguration(configData, solutionItemConfiguration2.Name, solutionItemConfiguration2.Platform, group);
					if (mergedConfiguration != null)
					{
						this.ForceDefaultValueSerialization(msbuildSerializer, mergedConfiguration, solutionItemConfiguration2);
					}
					DataItem dataItem2 = (DataItem)msbuildSerializer.Serialize(solutionItemConfiguration2);
					msbuildSerializer.SerializationContext.ResetDefaultValueSerialization();
					DotNetProjectConfiguration dotNetProjectConfiguration = solutionItemConfiguration2 as DotNetProjectConfiguration;
					if (dotNetProjectConfiguration != null && dotNetProjectConfiguration.CompilationParameters != null)
					{
						ClassDataType classDataType2 = (ClassDataType)msbuildSerializer.DataContext.GetConfigurationDataType(dotNetProjectConfiguration.CompilationParameters.GetType());
						foreach (ItemProperty itemProperty2 in classDataType2.GetProperties(msbuildSerializer.SerializationContext, dotNetProjectConfiguration.CompilationParameters))
						{
							DataNode dataNode2 = dataItem2.ItemData[itemProperty2.Name];
							if (dataNode2 != null)
							{
								dataItem2.ItemData.Remove(dataNode2);
							}
						}
						if (mergedConfiguration != null)
						{
							this.ForceDefaultValueSerialization(msbuildSerializer, mergedConfiguration, dotNetProjectConfiguration.CompilationParameters);
						}
						DataItem dataItem3 = (DataItem)msbuildSerializer.Serialize(dotNetProjectConfiguration.CompilationParameters);
						msbuildSerializer.SerializationContext.ResetDefaultValueSerialization();
						dataItem2.ItemData.AddRange(dataItem3.ItemData);
					}
					if (flag2)
					{
						dataItem2.ItemData.Sort(MSBuildProjectHandler.configOrder);
					}
					this.WritePropertyGroupMetadata(group, dataItem2.ItemData, msbuildSerializer, new object[]
					{
						solutionItemConfiguration2,
						(dotNetProjectConfiguration != null) ? dotNetProjectConfiguration.CompilationParameters : null
					});
					this.CollectMergetoprojectProperties(group, hashSet, dictionary);
					if (mergedConfiguration != null)
					{
						group.UnMerge(mergedConfiguration, hashSet2);
					}
				}
				foreach (KeyValuePair<string, MSBuildProjectHandler.MergedPropertyValue> keyValuePair in dictionary)
				{
					msbuildPropertySet.SetPropertyValue(keyValuePair.Key, keyValuePair.Value.XmlValue, keyValuePair.Value.PreserveExistingCase, true);
				}
				foreach (string text2 in hashSet2)
				{
					if (!dictionary.ContainsKey(text2))
					{
						msbuildPropertySet.RemoveProperty(text2);
					}
				}
				foreach (SolutionItemConfiguration config in entityItem.Configurations)
				{
					MSBuildPropertyGroup group2 = this.FindPropertyGroup(configData, config).Group;
					foreach (string name in dictionary.Keys)
					{
						group2.RemoveProperty(name);
					}
				}
				foreach (MSBuildProjectHandler.ConfigData configData3 in configData)
				{
					if ((!configData3.Exists && configData3.FullySpecified) || (configData3.IsNew && !configData3.Group.Properties.Any<MSBuildProperty>()))
					{
						msbuildProject.RemoveGroup(configData3.Group);
					}
				}
			}
			this.SaveProjectItems(monitor, toolsFormat, msbuildSerializer, msbuildProject, null);
			if (dotNetProject != null)
			{
				TargetFrameworkMoniker id = dotNetProject.TargetFramework.Id;
				bool flag3 = toolsFormat.SupportsMonikers || toolsFormat.SupportedFrameworks.Length > 0;
				TargetFrameworkMoniker defaultTargetFrameworkForFormat = dotNetProject.GetDefaultTargetFrameworkForFormat(this.GetFileFormat(toolsFormat));
				if (flag3)
				{
					this.SetIfPresentOrNotDefaultValue(msbuildPropertySet, "TargetFrameworkVersion", "v" + id.Version, "v" + defaultTargetFrameworkForFormat.Version, false);
				}
				if (toolsFormat.SupportsMonikers)
				{
					this.SetIfPresentOrNotDefaultValue(msbuildPropertySet, "TargetFrameworkIdentifier", id.Identifier, defaultTargetFrameworkForFormat.Identifier, false);
					this.SetIfPresentOrNotDefaultValue(msbuildPropertySet, "TargetFrameworkProfile", id.Profile, defaultTargetFrameworkForFormat.Profile, false);
				}
			}
			List<DotNetProjectImport> list = (from i in msbuildProject.Imports
			select new DotNetProjectImport(i.Project, null)).ToList<DotNetProjectImport>();
			List<DotNetProjectImport> list2 = new List<DotNetProjectImport>(list);
			this.UpdateImports(list2, dotNetProject, flag);
			foreach (DotNetProjectImport dotNetProjectImport in list2)
			{
				if (!list.Contains(dotNetProjectImport))
				{
					MSBuildImport msbuildImport = msbuildProject.AddNewImport(dotNetProjectImport.Name, null);
					if (dotNetProjectImport.HasCondition())
					{
						msbuildImport.Condition = dotNetProjectImport.Condition;
					}
					list.Add(dotNetProjectImport);
				}
			}
			foreach (DotNetProjectImport dotNetProjectImport2 in list)
			{
				if (!list2.Contains(dotNetProjectImport2))
				{
					msbuildProject.RemoveImport(dotNetProjectImport2.Name);
				}
			}
			DataItem externalItemProperties = msbuildSerializer.ExternalItemProperties;
			if (externalItemProperties.HasItemData)
			{
				externalItemProperties.Name = "Properties";
				StringWriter stringWriter = new StringWriter();
				XmlConfigurationWriter.DefaultWriter.Write(new XmlTextWriter(stringWriter), externalItemProperties);
				msbuildProject.SetProjectExtensions("MonoDevelop", stringWriter.ToString());
			}
			else
			{
				msbuildProject.RemoveProjectExtensions("MonoDevelop");
			}
			this.SaveToMSBuildProject(monitor, msbuildProject);
			return msbuildProject;
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x0003FFE8 File Offset: 0x0003E1E8
		internal void SaveProjectItems(IProgressMonitor monitor, MSBuildFileFormat toolsFormat, MSBuildSerializer ser, MSBuildProject msproject, string pathPrefix = null)
		{
			Dictionary<string, MSBuildProjectHandler.ItemInfo> dictionary = new Dictionary<string, MSBuildProjectHandler.ItemInfo>();
			foreach (MSBuildItem msbuildItem in msproject.GetAllItems())
			{
				dictionary[string.Concat(new string[]
				{
					msbuildItem.Name,
					"<",
					msbuildItem.UnevaluatedInclude,
					"<",
					msbuildItem.Condition
				})] = new MSBuildProjectHandler.ItemInfo
				{
					Item = msbuildItem
				};
			}
			foreach (object ob in from it in ((SolutionEntityItem)base.Item).Items.Concat(((SolutionEntityItem)base.Item).WildcardItems)
			where !it.Flags.HasFlag(ProjectItemFlags.DontPersist)
			select it)
			{
				this.SaveItem(monitor, toolsFormat, ser, msproject, ob, dictionary, pathPrefix);
			}
			foreach (MSBuildProjectHandler.ItemInfo itemInfo in dictionary.Values)
			{
				if (!itemInfo.Added)
				{
					msproject.RemoveItem(itemInfo.Item);
				}
			}
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x00040170 File Offset: 0x0003E370
		protected void SaveToMSBuildProject(IProgressMonitor monitor, MSBuildProject msproject)
		{
			foreach (MSBuildExtension msbuildExtension in this.GetMSBuildExtensions())
			{
				msbuildExtension.SaveProject(monitor, this.EntityItem, msproject);
			}
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x000401C4 File Offset: 0x0003E3C4
		private void SetIfPresentOrNotDefaultValue(MSBuildPropertySet propGroup, string name, string value, string defaultValue, bool isXml = false)
		{
			bool flag = string.IsNullOrEmpty(value) || value == defaultValue;
			MSBuildProperty property = propGroup.GetProperty(name);
			if (property != null)
			{
				if (!flag)
				{
					property.SetValue(value, isXml);
					return;
				}
				string value2 = property.GetValue(isXml);
				if (!string.IsNullOrEmpty(value2) && !(value2 == defaultValue))
				{
					propGroup.RemoveProperty(name);
					return;
				}
			}
			else if (!flag)
			{
				propGroup.SetPropertyValue(name, value, false, isXml);
			}
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x00040238 File Offset: 0x0003E438
		private void ForceDefaultValueSerialization(MSBuildSerializer ser, MSBuildPropertySet baseGroup, object ob)
		{
			ClassDataType classDataType = (ClassDataType)ser.DataContext.GetConfigurationDataType(ob.GetType());
			foreach (ItemProperty itemProperty in classDataType.GetProperties(ser.SerializationContext, ob))
			{
				if (baseGroup.GetProperty(itemProperty.Name) != null)
				{
					ser.SerializationContext.ForceDefaultValueSerialization(itemProperty);
				}
			}
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x000402B8 File Offset: 0x0003E4B8
		private void CollectMergetoprojectProperties(MSBuildPropertyGroup pgroup, HashSet<MSBuildProjectHandler.MergedProperty> properties, Dictionary<string, MSBuildProjectHandler.MergedPropertyValue> mergeToProjectProperties)
		{
			foreach (MSBuildProjectHandler.MergedProperty item in new List<MSBuildProjectHandler.MergedProperty>(properties))
			{
				MSBuildProperty property = pgroup.GetProperty(item.Name);
				MSBuildProjectHandler.MergedPropertyValue mergedPropertyValue;
				if (!mergeToProjectProperties.TryGetValue(item.Name, out mergedPropertyValue))
				{
					if (property != null)
					{
						mergeToProjectProperties.Add(item.Name, new MSBuildProjectHandler.MergedPropertyValue(property.GetValue(true), item.PreserveExistingCase));
						continue;
					}
				}
				else if (property != null && string.Equals(property.GetValue(true), mergedPropertyValue.XmlValue, StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}
				properties.Remove(item);
				mergeToProjectProperties.Remove(item.Name);
			}
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x0004037C File Offset: 0x0003E57C
		private void SaveItem(IProgressMonitor monitor, MSBuildFileFormat fmt, MSBuildSerializer ser, MSBuildProject msproject, object ob, Dictionary<string, MSBuildProjectHandler.ItemInfo> oldItems, string pathPrefix = null)
		{
			if (ob is ProjectReference)
			{
				this.SaveReference(monitor, fmt, ser, msproject, (ProjectReference)ob, oldItems);
				return;
			}
			if (ob is ProjectFile)
			{
				this.SaveProjectFile(ser, msproject, (ProjectFile)ob, oldItems, pathPrefix);
				return;
			}
			if (ob is UnknownProjectItem)
			{
				UnknownProjectItem unknownProjectItem = (UnknownProjectItem)ob;
				string itemName = unknownProjectItem.ItemName;
				MSBuildItem buildItem = this.AddOrGetBuildItem(msproject, oldItems, itemName, unknownProjectItem.Include, unknownProjectItem.Condition);
				this.WriteBuildItemMetadata(ser, buildItem, ob, oldItems);
				return;
			}
			DataType configurationDataType = ser.DataContext.GetConfigurationDataType(ob.GetType());
			MSBuildItem buildItem2 = msproject.AddNewItem(configurationDataType.Name, "");
			this.WriteBuildItemMetadata(ser, buildItem2, ob, oldItems);
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00040438 File Offset: 0x0003E638
		private void SaveProjectFile(MSBuildSerializer ser, MSBuildProject msproject, ProjectFile file, Dictionary<string, MSBuildProjectHandler.ItemInfo> oldItems, string pathPrefix = null)
		{
			if (file.IsOriginatedFromWildcard)
			{
				return;
			}
			string name = (file.Subtype == Subtype.Directory) ? "Folder" : file.BuildAction;
			string text = pathPrefix + MSBuildProjectService.ToMSBuildPath(base.Item.ItemDirectory, file.FilePath);
			if (text.Length == 0)
			{
				return;
			}
			if (file.Subtype == Subtype.Directory && text[text.Length - 1] != '\\')
			{
				text += "\\";
			}
			MSBuildItem msbuildItem = this.AddOrGetBuildItem(msproject, oldItems, name, text, file.Condition);
			this.WriteBuildItemMetadata(ser, msbuildItem, file, oldItems);
			if (!string.IsNullOrEmpty(file.DependsOn))
			{
				msbuildItem.SetMetadata("DependentUpon", MSBuildProjectService.ToMSBuildPath(Path.GetDirectoryName(file.FilePath), file.DependsOn), false);
			}
			else
			{
				msbuildItem.UnsetMetadata("DependentUpon");
			}
			if (!string.IsNullOrEmpty(file.ContentType))
			{
				msbuildItem.SetMetadata("SubType", file.ContentType, false);
			}
			if (!string.IsNullOrEmpty(file.Generator))
			{
				msbuildItem.SetMetadata("Generator", file.Generator, false);
			}
			else
			{
				msbuildItem.UnsetMetadata("Generator");
			}
			if (!string.IsNullOrEmpty(file.CustomToolNamespace))
			{
				msbuildItem.SetMetadata("CustomToolNamespace", file.CustomToolNamespace, false);
			}
			else
			{
				msbuildItem.UnsetMetadata("CustomToolNamespace");
			}
			if (!string.IsNullOrEmpty(file.LastGenOutput))
			{
				msbuildItem.SetMetadata("LastGenOutput", file.LastGenOutput, false);
			}
			else
			{
				msbuildItem.UnsetMetadata("LastGenOutput");
			}
			if (!string.IsNullOrEmpty(file.Link))
			{
				msbuildItem.SetMetadata("Link", MSBuildProjectService.ToMSBuildPathRelative(base.Item.ItemDirectory, file.Link), false);
			}
			else
			{
				msbuildItem.UnsetMetadata("Link");
			}
			msbuildItem.Condition = file.Condition;
			if (file.CopyToOutputDirectory == FileCopyMode.None)
			{
				msbuildItem.UnsetMetadata("CopyToOutputDirectory");
			}
			else
			{
				msbuildItem.SetMetadata("CopyToOutputDirectory", file.CopyToOutputDirectory.ToString(), false);
			}
			if (!file.Visible)
			{
				msbuildItem.SetMetadata("Visible", "False", false);
			}
			else
			{
				msbuildItem.UnsetMetadata("Visible");
			}
			string text2 = file.ResourceId;
			if (file.BuildAction == "EmbeddedResource" && this.GetDefaultResourceId(file) == text2)
			{
				text2 = null;
			}
			if (!string.IsNullOrEmpty(text2))
			{
				msbuildItem.SetMetadata("LogicalName", text2, false);
				return;
			}
			msbuildItem.UnsetMetadata("LogicalName");
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x000406B8 File Offset: 0x0003E8B8
		private void SaveReference(IProgressMonitor monitor, MSBuildFileFormat fmt, MSBuildSerializer ser, MSBuildProject msproject, ProjectReference pref, Dictionary<string, MSBuildProjectHandler.ItemInfo> oldItems)
		{
			MSBuildItem msbuildItem;
			if (pref.ReferenceType == ReferenceType.Assembly)
			{
				string text = null;
				string value;
				if (pref.ExtendedProperties.Contains("_OriginalMSBuildReferenceInclude"))
				{
					text = (string)pref.ExtendedProperties["_OriginalMSBuildReferenceInclude"];
					value = (string)pref.ExtendedProperties["_OriginalMSBuildReferenceHintPath"];
				}
				else
				{
					if (File.Exists(pref.HintPath))
					{
						try
						{
							AssemblyName assemblyName = AssemblyName.GetAssemblyName(pref.HintPath);
							if (pref.SpecificVersion)
							{
								text = assemblyName.FullName;
							}
							else
							{
								text = assemblyName.Name;
							}
						}
						catch (Exception ex)
						{
							string message = string.Format("Could not get full name for assembly '{0}'.", pref.Reference);
							monitor.ReportWarning(message);
							LoggingService.LogError(message, ex);
						}
					}
					string baseDirectory = base.Item.ItemDirectory;
					if (pref.ExtendedProperties.Contains("_OriginalMSBuildReferenceIsAbsolute"))
					{
						baseDirectory = null;
					}
					value = MSBuildProjectService.ToMSBuildPath(baseDirectory, pref.HintPath);
				}
				if (text == null)
				{
					text = Path.GetFileNameWithoutExtension(pref.Reference);
				}
				msbuildItem = this.AddOrGetBuildItem(msproject, oldItems, "Reference", text, pref.Condition);
				if (!pref.SpecificVersion && this.ReferenceStringHasVersion(text))
				{
					msbuildItem.SetMetadata("SpecificVersion", "False", false);
				}
				else
				{
					msbuildItem.UnsetMetadata("SpecificVersion");
				}
				msbuildItem.SetMetadata("HintPath", value, false);
			}
			else if (pref.ReferenceType == ReferenceType.Package)
			{
				string text2 = pref.StoredReference;
				SystemPackage package = pref.Package;
				if (package != null && package.IsFrameworkPackage)
				{
					int num = text2.IndexOf(',');
					if (num != -1)
					{
						text2 = text2.Substring(0, num).Trim();
					}
				}
				msbuildItem = this.AddOrGetBuildItem(msproject, oldItems, "Reference", text2, pref.Condition);
				if (!pref.SpecificVersion && this.ReferenceStringHasVersion(text2))
				{
					msbuildItem.SetMetadata("SpecificVersion", "False", false);
				}
				else
				{
					msbuildItem.UnsetMetadata("SpecificVersion");
				}
				DotNetProject dotNetProject = pref.OwnerProject as DotNetProject;
				IList supportedFrameworks = fmt.SupportedFrameworks;
				if (supportedFrameworks != null && dotNetProject != null && package != null && dotNetProject.TargetFramework.Id.Identifier == ".NETFramework" && package.IsFrameworkPackage && supportedFrameworks.Contains(package.TargetFramework) && package.TargetFramework.Version != "2.0" && supportedFrameworks.Count > 1)
				{
					TargetFramework targetFramework = Runtime.SystemAssemblyService.GetTargetFramework(package.TargetFramework);
					msbuildItem.SetMetadata("RequiredTargetFramework", targetFramework.Id.Version, false);
				}
				else
				{
					msbuildItem.UnsetMetadata("RequiredTargetFramework");
				}
				string text3 = (string)pref.ExtendedProperties["_OriginalMSBuildReferenceHintPath"];
				if (text3 != null)
				{
					msbuildItem.SetMetadata("HintPath", text3, false);
				}
				else
				{
					msbuildItem.UnsetMetadata("HintPath");
				}
			}
			else if (pref.ReferenceType == ReferenceType.Project)
			{
				Project project = (base.Item.ParentSolution != null) ? base.Item.ParentSolution.FindProjectByName(pref.Reference) : null;
				if (project == null)
				{
					monitor.ReportWarning(GettextCatalog.GetString("Reference to unknown project '{0}' ignored.", pref.Reference));
					return;
				}
				msbuildItem = this.AddOrGetBuildItem(msproject, oldItems, "ProjectReference", MSBuildProjectService.ToMSBuildPath(base.Item.ItemDirectory, project.FileName), pref.Condition);
				MSBuildProjectHandler msbuildProjectHandler = project.ItemHandler as MSBuildProjectHandler;
				if (msbuildProjectHandler != null)
				{
					msbuildItem.SetMetadata("Project", msbuildProjectHandler.Item.ItemId, false);
				}
				else
				{
					msbuildItem.UnsetMetadata("Project");
				}
				msbuildItem.SetMetadata("Name", project.Name, false);
				if (pref.ReferenceOutputAssembly)
				{
					msbuildItem.UnsetMetadata("ReferenceOutputAssembly");
				}
				else
				{
					msbuildItem.SetMetadata("ReferenceOutputAssembly", false);
				}
			}
			else
			{
				DataType configurationDataType = ser.DataContext.GetConfigurationDataType(pref.GetType());
				msbuildItem = this.AddOrGetBuildItem(msproject, oldItems, configurationDataType.Name, pref.Reference, pref.Condition);
			}
			if (pref.LocalCopy != pref.DefaultLocalCopy)
			{
				msbuildItem.SetMetadata("Private", pref.LocalCopy);
			}
			else
			{
				msbuildItem.UnsetMetadata("Private");
			}
			this.WriteBuildItemMetadata(ser, msbuildItem, pref, oldItems);
			msbuildItem.Condition = pref.Condition;
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x00040B3C File Offset: 0x0003ED3C
		private void UpdateImports(List<DotNetProjectImport> imports, DotNetProject project, bool addItemTypeImports)
		{
			if (this.targetImports != null && addItemTypeImports)
			{
				this.AddMissingImports(imports, this.targetImports);
			}
			List<string> list = (from import in imports
			select import.Name).ToList<string>();
			foreach (IMSBuildImportProvider imsbuildImportProvider in AddinManager.GetExtensionObjects("/MonoDevelop/ProjectModel/MSBuildImportProviders"))
			{
				imsbuildImportProvider.UpdateImports(this.EntityItem, list);
			}
			this.UpdateImports(imports, list);
			if (project != null)
			{
				this.AddMissingImports(imports, project.ImportsAdded);
				this.RemoveImports(imports, project.ImportsRemoved);
				project.ImportsSaved();
			}
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x00040BED File Offset: 0x0003EDED
		private void AddMissingImports(List<DotNetProjectImport> existingImports, IEnumerable<string> newImports)
		{
			this.AddMissingImports(existingImports, from import in newImports
			select new DotNetProjectImport(import, null));
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x00040C1C File Offset: 0x0003EE1C
		private void AddMissingImports(List<DotNetProjectImport> existingImports, IEnumerable<DotNetProjectImport> newImports)
		{
			foreach (DotNetProjectImport item in newImports)
			{
				if (!existingImports.Contains(item))
				{
					existingImports.Add(item);
				}
			}
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x00040C70 File Offset: 0x0003EE70
		private void UpdateImports(List<DotNetProjectImport> existingImports, List<string> updatedImports)
		{
			this.RemoveMissingImports(existingImports, updatedImports);
			this.AddMissingImports(existingImports, updatedImports);
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x00040CA0 File Offset: 0x0003EEA0
		private void RemoveMissingImports(List<DotNetProjectImport> existingImports, List<string> updatedImports)
		{
			List<DotNetProjectImport> importsToRemove = (from import in existingImports
			where !updatedImports.Contains(import.Name)
			select import).ToList<DotNetProjectImport>();
			this.RemoveImports(existingImports, importsToRemove);
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x00040CDC File Offset: 0x0003EEDC
		private void RemoveImports(List<DotNetProjectImport> existingImports, IEnumerable<DotNetProjectImport> importsToRemove)
		{
			foreach (DotNetProjectImport item in importsToRemove)
			{
				existingImports.Remove(item);
			}
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x00040EBC File Offset: 0x0003F0BC
		private IEnumerable<MSBuildExtension> GetMSBuildExtensions()
		{
			foreach (MSBuildExtension e in AddinManager.GetExtensionObjects<MSBuildExtension>("/MonoDevelop/ProjectModel/MSBuildExtensions"))
			{
				e.Handler = this;
				yield return e;
				e.Handler = null;
			}
			yield break;
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x00040EDC File Offset: 0x0003F0DC
		private void ReadBuildItemMetadata(DataSerializer ser, MSBuildItem buildItem, object dataItem, Type extendedType)
		{
			DataItem dataItem2 = new DataItem();
			foreach (ItemProperty itemProperty in ser.GetProperties(dataItem))
			{
				string text = this.ToMsbuildItemName(itemProperty.Name);
				if (text == "Include")
				{
					dataItem2.ItemData.Add(new DataValue("Include", buildItem.Include));
				}
				else if (buildItem.HasMetadata(text))
				{
					string metadata = buildItem.GetMetadata(text, !itemProperty.DataType.IsSimpleType);
					dataItem2.ItemData.Add(this.GetDataNode(itemProperty, metadata));
				}
			}
			this.ConvertFromMsbuildFormat(dataItem2);
			ser.Deserialize(dataItem, dataItem2);
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x00040FA8 File Offset: 0x0003F1A8
		private void WriteBuildItemMetadata(DataSerializer ser, MSBuildItem buildItem, object dataItem, Dictionary<string, MSBuildProjectHandler.ItemInfo> oldItems)
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (ItemProperty itemProperty in ser.GetProperties(dataItem))
			{
				hashSet.Add(itemProperty.Name);
			}
			DataItem dataItem2 = (DataItem)ser.Serialize(dataItem, dataItem.GetType());
			if (dataItem2.HasItemData)
			{
				foreach (object obj in dataItem2.ItemData)
				{
					DataNode dataNode = (DataNode)obj;
					hashSet.Remove(dataNode.Name);
					if (dataNode.Name == "Include" && dataNode is DataValue)
					{
						buildItem.Include = ((DataValue)dataNode).Value;
					}
					else
					{
						this.ConvertToMsbuildFormat(dataNode);
						buildItem.SetMetadata(dataNode.Name, this.GetXmlString(dataNode), dataNode is DataItem);
					}
				}
			}
			foreach (string name in hashSet)
			{
				buildItem.UnsetMetadata(name);
			}
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x0004110C File Offset: 0x0003F30C
		private MSBuildItem AddOrGetBuildItem(MSBuildProject msproject, Dictionary<string, MSBuildProjectHandler.ItemInfo> oldItems, string name, string include, string condition)
		{
			string key = string.Concat(new string[]
			{
				name,
				"<",
				include,
				"<",
				condition
			});
			MSBuildProjectHandler.ItemInfo value;
			if (oldItems.TryGetValue(key, out value))
			{
				if (!value.Added)
				{
					value.Added = true;
					oldItems[key] = value;
				}
				return value.Item;
			}
			return msproject.AddNewItem(name, include);
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x0004117C File Offset: 0x0003F37C
		private DataItem ReadPropertyGroupMetadata(DataSerializer ser, MSBuildPropertySet propGroup, object dataItem)
		{
			DataItem dataItem2 = new DataItem();
			foreach (MSBuildProperty msbuildProperty in propGroup.Properties)
			{
				DataNode dataNode = null;
				foreach (object obj in msbuildProperty.Element.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (xmlNode is XmlElement)
					{
						dataNode = XmlConfigurationReader.DefaultReader.Read((XmlElement)xmlNode);
						break;
					}
				}
				if (dataNode == null)
				{
					dataNode = new DataValue(msbuildProperty.Name, msbuildProperty.GetValue(false));
				}
				this.ConvertFromMsbuildFormat(dataNode);
				dataItem2.ItemData.Add(dataNode);
			}
			return dataItem2;
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x00041268 File Offset: 0x0003F468
		private void WritePropertyGroupMetadata(MSBuildPropertySet propGroup, DataCollection itemData, MSBuildSerializer ser, params object[] itemsToReplace)
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (object obj in itemsToReplace)
			{
				if (obj != null)
				{
					ClassDataType classDataType = (ClassDataType)ser.DataContext.GetConfigurationDataType(obj.GetType());
					foreach (ItemProperty itemProperty in classDataType.GetProperties(ser.SerializationContext, obj))
					{
						hashSet.Add(itemProperty.Name);
					}
				}
			}
			foreach (object obj2 in itemData)
			{
				DataNode dataNode = (DataNode)obj2;
				hashSet.Remove(dataNode.Name);
				this.ConvertToMsbuildFormat(dataNode);
				MSBuildBoolDataValue msbuildBoolDataValue = dataNode as MSBuildBoolDataValue;
				string value;
				bool preserveExistingCase;
				if (msbuildBoolDataValue != null)
				{
					value = (msbuildBoolDataValue.RawValue ? "true" : "false");
					preserveExistingCase = true;
				}
				else
				{
					value = this.GetXmlString(dataNode);
					preserveExistingCase = false;
				}
				propGroup.SetPropertyValue(dataNode.Name, value, preserveExistingCase, dataNode is DataItem);
			}
			foreach (string name in hashSet)
			{
				propGroup.RemoveProperty(name);
			}
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x000413F4 File Offset: 0x0003F5F4
		private string ToMsbuildItemName(string name)
		{
			return name.Replace('.', '-');
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x00041400 File Offset: 0x0003F600
		private void ConvertToMsbuildFormat(DataNode node)
		{
			this.ReplaceChar(node, true, '.', '-');
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x0004140E File Offset: 0x0003F60E
		private void ConvertFromMsbuildFormat(DataNode node)
		{
			this.ReplaceChar(node, true, '-', '.');
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x0004141C File Offset: 0x0003F61C
		private void ReplaceChar(DataNode node, bool force, char oldChar, char newChar)
		{
			DataItem dataItem = node as DataItem;
			if ((force || dataItem != null) && node.Name != null)
			{
				node.Name = node.Name.Replace(oldChar, newChar);
			}
			if (dataItem != null)
			{
				foreach (object obj in dataItem.ItemData)
				{
					DataNode node2 = (DataNode)obj;
					this.ReplaceChar(node2, !dataItem.UniqueNames, oldChar, newChar);
				}
			}
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x000414B0 File Offset: 0x0003F6B0
		private List<MSBuildProjectHandler.ConfigData> GetConfigData(MSBuildProject msproject, bool includeGlobalGroups)
		{
			List<MSBuildProjectHandler.ConfigData> list = new List<MSBuildProjectHandler.ConfigData>();
			foreach (MSBuildPropertyGroup msbuildPropertyGroup in msproject.PropertyGroups)
			{
				string conf;
				string plt;
				if (this.ParseConfigCondition(msbuildPropertyGroup.Condition, out conf, out plt) || includeGlobalGroups)
				{
					list.Add(new MSBuildProjectHandler.ConfigData(conf, plt, msbuildPropertyGroup));
				}
			}
			return list;
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x00041528 File Offset: 0x0003F728
		private MSBuildProjectHandler.ConfigData FindPropertyGroup(List<MSBuildProjectHandler.ConfigData> configData, SolutionItemConfiguration config)
		{
			foreach (MSBuildProjectHandler.ConfigData configData2 in configData)
			{
				if (configData2.Config == config.Name && configData2.Platform == config.Platform)
				{
					return configData2;
				}
			}
			return null;
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x0004159C File Offset: 0x0003F79C
		private ProjectFile ReadProjectFile(DataSerializer ser, Project project, MSBuildItem buildItem, Type type)
		{
			string text = MSBuildProjectService.FromMSBuildPath(project.ItemDirectory, buildItem.Include);
			ProjectFile projectFile = (ProjectFile)Activator.CreateInstance(type);
			projectFile.Name = text;
			projectFile.BuildAction = buildItem.Name;
			this.ReadBuildItemMetadata(ser, buildItem, projectFile, type);
			string text2 = buildItem.GetMetadata("DependentUpon", false);
			if (!string.IsNullOrEmpty(text2))
			{
				text2 = MSBuildProjectService.FromMSBuildPath(Path.GetDirectoryName(text), text2);
				projectFile.DependsOn = text2;
			}
			string metadata = buildItem.GetMetadata("CopyToOutputDirectory", false);
			if (!string.IsNullOrEmpty(metadata))
			{
				string a;
				if ((a = metadata) != null)
				{
					if (a == "None")
					{
						goto IL_DD;
					}
					if (a == "Always")
					{
						projectFile.CopyToOutputDirectory = FileCopyMode.Always;
						goto IL_DD;
					}
					if (a == "PreserveNewest")
					{
						projectFile.CopyToOutputDirectory = FileCopyMode.PreserveNewest;
						goto IL_DD;
					}
				}
				LoggingService.LogWarning("Unrecognised value {0} for CopyToOutputDirectory MSBuild property", new object[]
				{
					metadata
				});
			}
			IL_DD:
			if (buildItem.GetMetadataIsFalse("Visible"))
			{
				projectFile.Visible = false;
			}
			string metadata2 = buildItem.GetMetadata("LogicalName", false);
			if (!string.IsNullOrEmpty(metadata2))
			{
				projectFile.ResourceId = metadata2;
			}
			string metadata3 = buildItem.GetMetadata("SubType", false);
			if (!string.IsNullOrEmpty(metadata3))
			{
				projectFile.ContentType = metadata3;
			}
			string metadata4 = buildItem.GetMetadata("Generator", false);
			if (!string.IsNullOrEmpty(metadata4))
			{
				projectFile.Generator = metadata4;
			}
			string metadata5 = buildItem.GetMetadata("CustomToolNamespace", false);
			if (!string.IsNullOrEmpty(metadata5))
			{
				projectFile.CustomToolNamespace = metadata5;
			}
			string metadata6 = buildItem.GetMetadata("LastGenOutput", false);
			if (!string.IsNullOrEmpty(metadata6))
			{
				projectFile.LastGenOutput = metadata6;
			}
			string text3 = buildItem.GetMetadata("Link", false);
			if (!string.IsNullOrEmpty(text3))
			{
				if (!Platform.IsWindows)
				{
					text3 = MSBuildProjectService.UnescapePath(text3);
				}
				projectFile.Link = text3;
			}
			projectFile.Condition = buildItem.Condition;
			return projectFile;
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x00041778 File Offset: 0x0003F978
		private bool ParseConfigCondition(string cond, out string config, out string platform)
		{
			string text;
			platform = (text = null);
			config = text;
			int num = cond.IndexOf("==", StringComparison.Ordinal);
			if (num == -1)
			{
				return false;
			}
			if (cond.Substring(0, num).Trim() == "'$(Configuration)|$(Platform)'")
			{
				if (!this.ExtractConfigName(cond.Substring(num + 2), out cond))
				{
					return false;
				}
				num = cond.IndexOf('|');
				if (num != -1)
				{
					config = cond.Substring(0, num);
					platform = cond.Substring(num + 1);
					if (platform == "AnyCPU")
					{
						platform = string.Empty;
					}
					return true;
				}
				return false;
			}
			else if (cond.Substring(0, num).Trim() == "'$(Configuration)'")
			{
				if (!this.ExtractConfigName(cond.Substring(num + 2), out config))
				{
					return false;
				}
				platform = null;
				return true;
			}
			else
			{
				if (!(cond.Substring(0, num).Trim() == "'$(Platform)'"))
				{
					return false;
				}
				config = null;
				if (!this.ExtractConfigName(cond.Substring(num + 2), out platform))
				{
					return false;
				}
				if (platform == "AnyCPU")
				{
					platform = string.Empty;
				}
				return true;
			}
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x00041888 File Offset: 0x0003FA88
		private bool ExtractConfigName(string name, out string config)
		{
			config = name.Trim(new char[]
			{
				' '
			});
			if (config.Length <= 2)
			{
				return false;
			}
			if (config[0] != '\'' || config[config.Length - 1] != '\'')
			{
				return false;
			}
			config = config.Substring(1, config.Length - 2);
			return config.IndexOf('\'') == -1;
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x000418F8 File Offset: 0x0003FAF8
		private string BuildConfigCondition(string config, string platform)
		{
			if (platform.Length == 0)
			{
				platform = "AnyCPU";
			}
			return string.Concat(new string[]
			{
				" '$(Configuration)|$(Platform)' == '",
				config,
				"|",
				platform,
				"' "
			});
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x00041944 File Offset: 0x0003FB44
		private bool IsMergeToProjectProperty(ItemProperty prop)
		{
			foreach (object obj in prop.CustomAttributes)
			{
				if (obj is MergeToProjectAttribute)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x0004197C File Offset: 0x0003FB7C
		private string GetXmlString(DataNode node)
		{
			if (node is DataValue)
			{
				return ((DataValue)node).Value;
			}
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter writer = new XmlTextWriter(stringWriter);
			XmlConfigurationWriter.DefaultWriter.Write(writer, node);
			return stringWriter.ToString();
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x000419BC File Offset: 0x0003FBBC
		private DataNode GetDataNode(ItemProperty prop, string xmlString)
		{
			if (prop.DataType.IsSimpleType)
			{
				return new DataValue(prop.Name, xmlString);
			}
			StringReader input = new StringReader(xmlString);
			return XmlConfigurationReader.DefaultReader.Read(new XmlTextReader(input));
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x000419FA File Offset: 0x0003FBFA
		internal virtual MSBuildSerializer CreateSerializer()
		{
			return new MSBuildSerializer(this.EntityItem.FileName);
		}

		// Token: 0x040004B6 RID: 1206
		private const string Unspecified = null;

		// Token: 0x040004B7 RID: 1207
		private const string RecursiveDirectoryWildcard = "**";

		// Token: 0x040004B8 RID: 1208
		private List<string> targetImports = new List<string>();

		// Token: 0x040004B9 RID: 1209
		private IResourceHandler customResourceHandler;

		// Token: 0x040004BA RID: 1210
		private List<string> subtypeGuids = new List<string>();

		// Token: 0x040004BB RID: 1211
		private RemoteProjectBuilder projectBuilder;

		// Token: 0x040004BC RID: 1212
		private ITimeTracker timer;

		// Token: 0x040004BD RID: 1213
		private bool modifiedInMemory;

		// Token: 0x040004BE RID: 1214
		private UnknownProjectTypeNode unknownProjectTypeInfo;

		// Token: 0x040004BF RID: 1215
		private string lastBuildToolsVersion;

		// Token: 0x040004C0 RID: 1216
		private string lastBuildRuntime;

		// Token: 0x040004C1 RID: 1217
		private string lastFileName;

		// Token: 0x040004C2 RID: 1218
		private string lastSlnFileName;

		// Token: 0x040004C3 RID: 1219
		private string productVersion;

		// Token: 0x040004C4 RID: 1220
		private string schemaVersion;

		// Token: 0x040004C5 RID: 1221
		private object builderLock = new object();

		// Token: 0x040004C6 RID: 1222
		private static readonly char[] directorySeparators = new char[]
		{
			Path.DirectorySeparatorChar,
			Path.AltDirectorySeparatorChar
		};

		// Token: 0x040004C7 RID: 1223
		private static readonly MSBuildElementOrder globalConfigOrder = new MSBuildElementOrder(new string[]
		{
			"Configuration",
			"Platform",
			"ProductVersion",
			"SchemaVersion",
			"ProjectGuid",
			"OutputType",
			"AppDesignerFolder",
			"RootNamespace",
			"AssemblyName",
			"StartupObject"
		});

		// Token: 0x040004C8 RID: 1224
		private static readonly MSBuildElementOrder configOrder = new MSBuildElementOrder(new string[]
		{
			"DebugSymbols",
			"DebugType",
			"Optimize",
			"OutputPath",
			"DefineConstants",
			"ErrorReport",
			"WarningLevel",
			"TreatWarningsAsErrors",
			"DocumentationFile"
		});

		// Token: 0x040004C9 RID: 1225
		internal static readonly ItemMember[] ExtendedMSBuildProperties = new ItemMember[]
		{
			new ItemMember(typeof(SolutionEntityItem), "ProductVersion"),
			new ItemMember(typeof(SolutionEntityItem), "SchemaVersion"),
			new ItemMember(typeof(SolutionEntityItem), "ProjectGuid"),
			new ItemMember(typeof(DotNetProjectConfiguration), "ErrorReport"),
			new ItemMember(typeof(DotNetProjectConfiguration), "TargetFrameworkVersion", new object[]
			{
				new MergeToProjectAttribute()
			}),
			new ItemMember(typeof(ProjectReference), "RequiredTargetFramework"),
			new ItemMember(typeof(Project), "InternalTargetFrameworkVersion", true)
		};

		// Token: 0x040004CA RID: 1226
		internal static readonly IList<string> UnsupportedItems = new string[]
		{
			"BootstrapperFile",
			"AppDesigner",
			"WebReferences",
			"WebReferenceUrl",
			"Service",
			"ProjectReference",
			"Reference",
			"InternalsVisibleTo",
			"InternalsVisibleToTest"
		};

		// Token: 0x020001B5 RID: 437
		private struct ItemInfo
		{
			// Token: 0x040004D5 RID: 1237
			public MSBuildItem Item;

			// Token: 0x040004D6 RID: 1238
			public bool Added;
		}

		// Token: 0x020001B6 RID: 438
		private struct MergedProperty
		{
			// Token: 0x060010D0 RID: 4304 RVA: 0x00041C20 File Offset: 0x0003FE20
			public MergedProperty(string name, bool preserveExistingCase)
			{
				this.Name = name;
				this.PreserveExistingCase = preserveExistingCase;
			}

			// Token: 0x040004D7 RID: 1239
			public readonly string Name;

			// Token: 0x040004D8 RID: 1240
			public readonly bool PreserveExistingCase;
		}

		// Token: 0x020001B7 RID: 439
		private class ConfigData
		{
			// Token: 0x060010D1 RID: 4305 RVA: 0x00041C30 File Offset: 0x0003FE30
			public ConfigData(string conf, string plt, MSBuildPropertyGroup grp)
			{
				this.Config = conf;
				this.Platform = plt;
				this.Group = grp;
			}

			// Token: 0x1700039C RID: 924
			// (get) Token: 0x060010D2 RID: 4306 RVA: 0x00041C4D File Offset: 0x0003FE4D
			public bool FullySpecified
			{
				get
				{
					return this.Config != null && this.Platform != null;
				}
			}

			// Token: 0x040004D9 RID: 1241
			public string Config;

			// Token: 0x040004DA RID: 1242
			public string Platform;

			// Token: 0x040004DB RID: 1243
			public MSBuildPropertyGroup Group;

			// Token: 0x040004DC RID: 1244
			public bool Exists;

			// Token: 0x040004DD RID: 1245
			public bool IsNew;
		}

		// Token: 0x020001B8 RID: 440
		private struct MergedPropertyValue
		{
			// Token: 0x060010D3 RID: 4307 RVA: 0x00041C6B File Offset: 0x0003FE6B
			public MergedPropertyValue(string xmlValue, bool preserveExistingCase)
			{
				this.XmlValue = xmlValue;
				this.PreserveExistingCase = preserveExistingCase;
			}

			// Token: 0x040004DE RID: 1246
			public readonly string XmlValue;

			// Token: 0x040004DF RID: 1247
			public readonly bool PreserveExistingCase;
		}
	}
}
