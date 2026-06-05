using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using MonoDevelop.Core;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.Core.Execution;
using MonoDevelop.Core.ProgressMonitoring;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects.Extensions;
using MonoDevelop.Projects.Formats.MD1;
using MonoDevelop.Projects.Formats.MSBuild;
using MonoDevelop.Projects.Policies;

namespace MonoDevelop.Projects
{
	// Token: 0x02000179 RID: 377
	[ProjectModelDataItem("AbstractDotNetProject")]
	[DataInclude(typeof(DotNetProjectConfiguration))]
	public abstract class DotNetProject : Project, IAssemblyProject, IDotNetFileContainer
	{
		// Token: 0x06000EAE RID: 3758 RVA: 0x00035FF0 File Offset: 0x000341F0
		public DotNetProject()
		{
			Runtime.SystemAssemblyService.DefaultRuntimeChanged += this.RuntimeSystemAssemblyServiceDefaultRuntimeChanged;
			this.projectReferences = new ProjectReferenceCollection();
			base.Items.Bind<ProjectReference>(this.projectReferences);
			if (this.IsLibraryBasedProjectType)
			{
				this.CompileTarget = CompileTarget.Library;
			}
			FileService.FileRemoved += this.OnFileRemoved;
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x00036080 File Offset: 0x00034280
		public DotNetProject(string languageName) : this()
		{
			this.languageName = languageName;
			this.languageBinding = this.FindLanguage(languageName);
			if (this.languageBinding != null)
			{
				this.StockIcon = this.languageBinding.ProjectStockIcon;
			}
			this.usePartialTypes = this.SupportsPartialTypes;
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x000360D4 File Offset: 0x000342D4
		public DotNetProject(string languageName, ProjectCreateInformation projectCreateInfo, XmlElement projectOptions) : this(languageName)
		{
			if (projectOptions != null && projectOptions.Attributes["Target"] != null)
			{
				this.CompileTarget = (CompileTarget)Enum.Parse(typeof(CompileTarget), projectOptions.Attributes["Target"].Value);
			}
			else if (this.IsLibraryBasedProjectType)
			{
				this.CompileTarget = CompileTarget.Library;
			}
			if (this.LanguageBinding != null)
			{
				this.LanguageParameters = this.languageBinding.CreateProjectParameters(projectOptions);
				bool flag = false;
				string text = null;
				if (projectOptions != null)
				{
					projectOptions.SetAttribute("DefineDebug", "True");
					if (!projectOptions.HasAttribute("Platform"))
					{
						text = this.GetDefaultTargetPlatform(projectCreateInfo);
						projectOptions = (XmlElement)projectOptions.CloneNode(true);
						projectOptions.SetAttribute("Platform", text);
					}
					else
					{
						text = projectOptions.GetAttribute("Platform");
					}
					if (projectOptions.GetAttribute("ExternalConsole") == "True")
					{
						flag = true;
					}
				}
				string str = string.IsNullOrEmpty(text) ? string.Empty : ("|" + text);
				DotNetProjectConfiguration dotNetProjectConfiguration = this.CreateConfiguration("Debug" + str) as DotNetProjectConfiguration;
				dotNetProjectConfiguration.CompilationParameters = this.languageBinding.CreateCompilationParameters(projectOptions);
				this.DefineSymbols(dotNetProjectConfiguration.CompilationParameters, projectOptions, "DefineConstantsDebug");
				dotNetProjectConfiguration.DebugMode = true;
				dotNetProjectConfiguration.ExternalConsole = flag;
				dotNetProjectConfiguration.PauseConsoleOutput = flag;
				base.Configurations.Add(dotNetProjectConfiguration);
				DotNetProjectConfiguration dotNetProjectConfiguration2 = this.CreateConfiguration("Release" + str) as DotNetProjectConfiguration;
				if (projectOptions != null)
				{
					XmlElement xmlElement = (XmlElement)projectOptions.CloneNode(true);
					xmlElement.SetAttribute("Release", "True");
					dotNetProjectConfiguration2.CompilationParameters = this.languageBinding.CreateCompilationParameters(xmlElement);
					this.DefineSymbols(dotNetProjectConfiguration2.CompilationParameters, projectOptions, "DefineConstantsRelease");
				}
				else
				{
					dotNetProjectConfiguration2.CompilationParameters = this.languageBinding.CreateCompilationParameters(null);
				}
				dotNetProjectConfiguration2.CompilationParameters.RemoveDefineSymbol("DEBUG");
				dotNetProjectConfiguration2.DebugMode = false;
				dotNetProjectConfiguration2.ExternalConsole = flag;
				dotNetProjectConfiguration2.PauseConsoleOutput = flag;
				base.Configurations.Add(dotNetProjectConfiguration2);
			}
			this.targetFramework = this.GetTargetFrameworkForNewProject(projectOptions, this.GetDefaultTargetFrameworkId());
			string path;
			if (projectCreateInfo != null)
			{
				this.Name = projectCreateInfo.ProjectName;
				path = projectCreateInfo.BinPath;
				this.defaultNamespace = DotNetProject.SanitisePotentialNamespace(projectCreateInfo.ProjectName);
			}
			else
			{
				path = ".";
			}
			foreach (SolutionItemConfiguration solutionItemConfiguration in base.Configurations)
			{
				DotNetProjectConfiguration dotNetProjectConfiguration3 = (DotNetProjectConfiguration)solutionItemConfiguration;
				dotNetProjectConfiguration3.OutputDirectory = Path.Combine(path, dotNetProjectConfiguration3.Name);
				if (projectOptions != null && projectOptions.Attributes["PauseConsoleOutput"] != null)
				{
					dotNetProjectConfiguration3.PauseConsoleOutput = bool.Parse(projectOptions.Attributes["PauseConsoleOutput"].Value);
				}
				if (projectCreateInfo != null)
				{
					dotNetProjectConfiguration3.OutputAssembly = projectCreateInfo.ProjectName;
				}
			}
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x000363D8 File Offset: 0x000345D8
		private void DefineSymbols(ConfigurationParameters pars, XmlElement projectOptions, string attributeName)
		{
			if (projectOptions != null)
			{
				string attribute = projectOptions.GetAttribute(attributeName);
				if (!string.IsNullOrEmpty(attribute))
				{
					pars.AddDefineSymbol(attribute);
				}
			}
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x00036400 File Offset: 0x00034600
		private TargetFramework GetTargetFrameworkForNewProject(XmlElement projectOptions, TargetFrameworkMoniker defaultMoniker)
		{
			if (projectOptions == null)
			{
				return Runtime.SystemAssemblyService.GetTargetFramework(defaultMoniker);
			}
			XmlAttribute xmlAttribute = projectOptions.Attributes["TargetFrameworkVersion"];
			if (xmlAttribute == null)
			{
				xmlAttribute = projectOptions.Attributes["TargetFramework"];
				if (xmlAttribute == null)
				{
					return Runtime.SystemAssemblyService.GetTargetFramework(defaultMoniker);
				}
			}
			TargetFrameworkMoniker targetFrameworkMoniker = TargetFrameworkMoniker.Parse(xmlAttribute.Value);
			string text = ".NETFramework";
			if (targetFrameworkMoniker.Identifier == text && !xmlAttribute.Value.StartsWith(text, StringComparison.Ordinal))
			{
				targetFrameworkMoniker = new TargetFrameworkMoniker(defaultMoniker.Identifier, targetFrameworkMoniker.Version, targetFrameworkMoniker.Profile);
			}
			return Runtime.SystemAssemblyService.GetTargetFramework(targetFrameworkMoniker);
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x0003656C File Offset: 0x0003476C
		public override IEnumerable<string> GetProjectTypes()
		{
			yield return "DotNet";
			yield break;
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x00036589 File Offset: 0x00034789
		public string LanguageName
		{
			get
			{
				return this.languageName;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000EB5 RID: 3765 RVA: 0x00036594 File Offset: 0x00034794
		public override string[] SupportedLanguages
		{
			get
			{
				return new string[]
				{
					"",
					this.languageName
				};
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000EB6 RID: 3766 RVA: 0x000365BA File Offset: 0x000347BA
		public virtual bool IsLibraryBasedProjectType
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000EB7 RID: 3767 RVA: 0x000365BD File Offset: 0x000347BD
		public virtual bool GeneratesDebugInfoFile
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x000365C0 File Offset: 0x000347C0
		protected virtual string GetDefaultTargetPlatform(ProjectCreateInformation projectCreateInfo)
		{
			return string.Empty;
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000EB9 RID: 3769 RVA: 0x000365C7 File Offset: 0x000347C7
		public ProjectReferenceCollection References
		{
			get
			{
				return this.projectReferences;
			}
		}

		/// <summary>
		/// Checks the status of references. To be called when referenced files may have been deleted or created.
		/// </summary>
		// Token: 0x06000EBA RID: 3770 RVA: 0x000365D0 File Offset: 0x000347D0
		public void RefreshReferenceStatus()
		{
			for (int i = 0; i < this.References.Count; i++)
			{
				ProjectReference refreshedReference = this.References[i].GetRefreshedReference();
				if (refreshedReference != null)
				{
					this.References[i] = refreshedReference;
				}
			}
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x00036615 File Offset: 0x00034815
		public virtual bool CanReferenceProject(DotNetProject targetProject, out string reason)
		{
			if (!this.TargetFramework.CanReferenceAssembliesTargetingFramework(targetProject.TargetFramework))
			{
				reason = GettextCatalog.GetString("Incompatible target framework: {0}", targetProject.TargetFramework.Id);
				return false;
			}
			reason = null;
			return true;
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000EBC RID: 3772 RVA: 0x00036648 File Offset: 0x00034848
		public IDotNetLanguageBinding LanguageBinding
		{
			get
			{
				if (this.languageBinding == null)
				{
					this.languageBinding = this.FindLanguage(this.languageName);
					if (this.languageBinding != null && this.UsePartialTypes && !this.SupportsPartialTypes)
					{
						LoggingService.LogWarning("Project '{0}' has been set to use partial types but does not support them.", new object[]
						{
							this.Name
						});
						this.UsePartialTypes = false;
					}
				}
				return this.languageBinding;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000EBD RID: 3773 RVA: 0x000366AF File Offset: 0x000348AF
		// (set) Token: 0x06000EBE RID: 3774 RVA: 0x000366B7 File Offset: 0x000348B7
		public CompileTarget CompileTarget
		{
			get
			{
				return this.compileTarget;
			}
			set
			{
				if (!base.Loading && this.IsLibraryBasedProjectType && value != CompileTarget.Library)
				{
					throw new InvalidOperationException("CompileTarget cannot be changed on library-based project type.");
				}
				this.compileTarget = value;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000EBF RID: 3775 RVA: 0x000366DF File Offset: 0x000348DF
		// (set) Token: 0x06000EC0 RID: 3776 RVA: 0x00036709 File Offset: 0x00034909
		[ItemProperty("LanguageParameters")]
		public ProjectParameters LanguageParameters
		{
			get
			{
				if (this.languageParameters == null && this.LanguageBinding != null)
				{
					this.LanguageParameters = this.LanguageBinding.CreateProjectParameters(null);
				}
				return this.languageParameters;
			}
			internal set
			{
				this.languageParameters = value;
				if (this.languageParameters != null)
				{
					this.languageParameters.ParentProject = this;
				}
			}
		}

		/// <summary>
		/// Default namespace setting. May be empty, use GetDefaultNamespace to get a usable value.
		/// </summary>
		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x00036726 File Offset: 0x00034926
		// (set) Token: 0x06000EC2 RID: 3778 RVA: 0x0003672E File Offset: 0x0003492E
		public string DefaultNamespace
		{
			get
			{
				return this.defaultNamespace;
			}
			set
			{
				this.defaultNamespace = value;
				base.NotifyModified("DefaultNamespace");
			}
		}

		/// <summary>
		/// Given a namespace, removes from it the implicit namespace of the project,
		/// if there is one. This depends on the target language. For example, in VB.NET
		/// the default namespace is implicit.
		/// </summary>
		// Token: 0x06000EC3 RID: 3779 RVA: 0x00036744 File Offset: 0x00034944
		public string StripImplicitNamespace(string ns)
		{
			if (this.LanguageParameters is DotNetProjectParameters && ((DotNetProjectParameters)this.LanguageParameters).DefaultNamespaceIsImplicit)
			{
				if (this.DefaultNamespace.Length > 0 && ns.StartsWith(this.DefaultNamespace + "."))
				{
					return ns.Substring(this.DefaultNamespace.Length + 1);
				}
				if (this.DefaultNamespace == ns)
				{
					return string.Empty;
				}
			}
			return ns;
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000EC4 RID: 3780 RVA: 0x000367C0 File Offset: 0x000349C0
		public IResourceHandler ResourceHandler
		{
			get
			{
				if (this.resourceHandler == null)
				{
					DotNetNamingPolicy dotNetNamingPolicy = base.Policies.Get<DotNetNamingPolicy>();
					if (dotNetNamingPolicy.ResourceNamePolicy == ResourceNamePolicy.FileFormatDefault)
					{
						this.resourceHandler = (base.ItemHandler as IResourceHandler);
					}
					else if (dotNetNamingPolicy.ResourceNamePolicy == ResourceNamePolicy.MSBuild)
					{
						this.resourceHandler = MSBuildProjectService.GetResourceHandlerForItem(this);
					}
					if (this.resourceHandler == null)
					{
						this.resourceHandler = DefaultResourceHandler.Instance;
					}
				}
				return this.resourceHandler;
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x0003682C File Offset: 0x00034A2C
		// (set) Token: 0x06000EC6 RID: 3782 RVA: 0x00036860 File Offset: 0x00034A60
		public TargetFramework TargetFramework
		{
			get
			{
				if (this.targetFramework == null)
				{
					TargetFrameworkMoniker defaultTargetFrameworkId = this.GetDefaultTargetFrameworkId();
					this.targetFramework = Runtime.SystemAssemblyService.GetTargetFramework(defaultTargetFrameworkId);
				}
				return this.targetFramework;
			}
			set
			{
				if (!this.SupportsFramework(value))
				{
					throw new ArgumentException("Project does not support framework '" + value.Id.ToString() + "'");
				}
				if (value == null)
				{
					value = Runtime.SystemAssemblyService.GetTargetFramework(this.GetDefaultTargetFrameworkForFormat(base.FileFormat));
				}
				if (this.targetFramework != null && value.Id == this.targetFramework.Id)
				{
					return;
				}
				bool flag = this.targetFramework != null;
				this.targetFramework = value;
				if (flag)
				{
					this.UpdateSystemReferences();
				}
				base.NotifyModified("TargetFramework");
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x000368FA File Offset: 0x00034AFA
		public TargetRuntime TargetRuntime
		{
			get
			{
				return Runtime.SystemAssemblyService.DefaultRuntime;
			}
		}

		/// <summary>
		/// Gets the target framework for new projects
		/// </summary>
		/// <returns>
		/// The default target framework identifier.
		/// </returns>
		// Token: 0x06000EC8 RID: 3784 RVA: 0x00036906 File Offset: 0x00034B06
		public virtual TargetFrameworkMoniker GetDefaultTargetFrameworkId()
		{
			return Services.ProjectService.DefaultTargetFramework.Id;
		}

		/// <summary>
		/// Returns the default framework for a given format
		/// </summary>
		/// <returns>
		/// The default target framework for the format.
		/// </returns>
		/// <param name="format">
		/// A format
		/// </param>
		/// <remarks>
		/// This method is used to determine what's the correct target framework for a project
		/// deserialized using a specific format.
		/// </remarks>
		// Token: 0x06000EC9 RID: 3785 RVA: 0x00036917 File Offset: 0x00034B17
		public virtual TargetFrameworkMoniker GetDefaultTargetFrameworkForFormat(FileFormat format)
		{
			return this.GetDefaultTargetFrameworkId();
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000ECA RID: 3786 RVA: 0x00036920 File Offset: 0x00034B20
		public IAssemblyContext AssemblyContext
		{
			get
			{
				if (this.composedAssemblyContext == null)
				{
					this.composedAssemblyContext = new ComposedAssemblyContext();
					this.composedAssemblyContext.Add(this.PrivateAssemblyContext);
					this.currentRuntimeContext = this.TargetRuntime.AssemblyContext;
					this.composedAssemblyContext.Add(this.currentRuntimeContext);
				}
				return this.composedAssemblyContext;
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000ECB RID: 3787 RVA: 0x00036979 File Offset: 0x00034B79
		public IAssemblyContext PrivateAssemblyContext
		{
			get
			{
				if (this.privateAssemblyContext == null)
				{
					this.privateAssemblyContext = new DirectoryAssemblyContext();
				}
				return this.privateAssemblyContext;
			}
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x00036994 File Offset: 0x00034B94
		public virtual bool SupportsFramework(TargetFramework framework)
		{
			if (this.LanguageBinding == null)
			{
				return false;
			}
			ClrVersion[] supportedClrVersions = this.LanguageBinding.GetSupportedClrVersions();
			if (supportedClrVersions != null && supportedClrVersions.Length > 0 && framework != null)
			{
				foreach (ClrVersion clrVersion in supportedClrVersions)
				{
					if (clrVersion == framework.ClrVersion)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000ECD RID: 3789 RVA: 0x000369EC File Offset: 0x00034BEC
		// (set) Token: 0x06000ECE RID: 3790 RVA: 0x000369F4 File Offset: 0x00034BF4
		[ItemProperty(DefaultValue = true)]
		public bool UsePartialTypes
		{
			get
			{
				return this.usePartialTypes;
			}
			set
			{
				this.usePartialTypes = value;
			}
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x00036A00 File Offset: 0x00034C00
		public override void Dispose()
		{
			if (this.composedAssemblyContext != null)
			{
				this.composedAssemblyContext.Dispose();
			}
			Runtime.SystemAssemblyService.DefaultRuntimeChanged -= this.RuntimeSystemAssemblyServiceDefaultRuntimeChanged;
			FileService.FileRemoved -= this.OnFileRemoved;
			base.Dispose();
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000ED0 RID: 3792 RVA: 0x00036A50 File Offset: 0x00034C50
		public virtual bool SupportsPartialTypes
		{
			get
			{
				if (this.LanguageBinding == null)
				{
					return false;
				}
				CodeDomProvider codeDomProvider = this.LanguageBinding.GetCodeDomProvider();
				return codeDomProvider != null && codeDomProvider.Supports(GeneratorSupport.PartialTypes);
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x00036A84 File Offset: 0x00034C84
		public override string[] SupportedPlatforms
		{
			get
			{
				return new string[]
				{
					"AnyCPU"
				};
			}
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x00036ABC File Offset: 0x00034CBC
		private void CheckReferenceChange(FilePath updatedFile)
		{
			for (int i = 0; i < this.References.Count; i++)
			{
				ProjectReference projectReference = this.References[i];
				if (projectReference.ReferenceType == ReferenceType.Assembly && base.DefaultConfiguration != null)
				{
					if (projectReference.GetReferencedFileNames(base.DefaultConfiguration.Selector).Any((string f) => f == updatedFile))
					{
						projectReference.NotifyStatusChanged();
					}
				}
				else if (projectReference.HintPath == updatedFile)
				{
					ProjectReference refreshedReference = projectReference.GetRefreshedReference();
					if (refreshedReference != null)
					{
						this.References[i] = refreshedReference;
					}
				}
			}
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x00036B78 File Offset: 0x00034D78
		internal override void OnFileChanged(object source, FileEventArgs e)
		{
			if (base.Disposed)
			{
				return;
			}
			base.OnFileChanged(source, e);
			foreach (FileEventInfo fileEventInfo in e)
			{
				this.CheckReferenceChange(fileEventInfo.FileName);
			}
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x00036BD8 File Offset: 0x00034DD8
		internal void RenameReferences(string oldName, string newName)
		{
			ArrayList arrayList = new ArrayList();
			foreach (ProjectReference projectReference in this.References)
			{
				if (projectReference.ReferenceType == ReferenceType.Project && projectReference.Reference == oldName)
				{
					arrayList.Add(projectReference);
				}
			}
			foreach (object obj in arrayList)
			{
				ProjectReference projectReference2 = (ProjectReference)obj;
				this.References.Remove(projectReference2);
				ProjectReference item = ProjectReference.RenameReference(projectReference2, newName);
				this.References.Add(item);
			}
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x00036CAC File Offset: 0x00034EAC
		protected internal override void PopulateOutputFileList(List<FilePath> list, ConfigurationSelector configuration)
		{
			base.PopulateOutputFileList(list, configuration);
			DotNetProjectConfiguration dotNetProjectConfiguration = this.GetConfiguration(configuration) as DotNetProjectConfiguration;
			if (dotNetProjectConfiguration.DebugMode)
			{
				string assemblyDebugInfoFile = this.TargetRuntime.GetAssemblyDebugInfoFile(dotNetProjectConfiguration.CompiledOutputName);
				list.Add(assemblyDebugInfoFile);
			}
			FilePath outputDirectory = dotNetProjectConfiguration.OutputDirectory;
			string text = Path.GetFileNameWithoutExtension(dotNetProjectConfiguration.CompiledOutputName) + ".resources.dll";
			HashSet<string> hashSet = new HashSet<string>();
			foreach (ProjectFile projectFile in base.Files)
			{
				if (projectFile.Subtype != Subtype.Directory && !(projectFile.BuildAction != "EmbeddedResource"))
				{
					string resourceCulture = DotNetProject.GetResourceCulture(projectFile.Name);
					if (resourceCulture != null && hashSet.Add(resourceCulture))
					{
						hashSet.Add(resourceCulture);
						FilePath item = outputDirectory.Combine(new string[]
						{
							resourceCulture,
							text
						});
						list.Add(item);
					}
				}
			}
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x00036DCC File Offset: 0x00034FCC
		protected internal override void PopulateSupportFileList(FileCopySet list, ConfigurationSelector configuration)
		{
			try
			{
				if (DotNetProject.supportReferDistance == 0)
				{
					DotNetProject.processedProjects = new HashSet<DotNetProject>();
				}
				DotNetProject.supportReferDistance++;
				this.PopulateSupportFileListInternal(list, configuration);
			}
			finally
			{
				DotNetProject.supportReferDistance--;
				if (DotNetProject.supportReferDistance == 0)
				{
					DotNetProject.processedProjects = null;
				}
			}
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x00036E70 File Offset: 0x00035070
		private void PopulateSupportFileListInternal(FileCopySet list, ConfigurationSelector configuration)
		{
			if (DotNetProject.supportReferDistance <= 2)
			{
				base.PopulateSupportFileList(list, configuration);
			}
			list.Remove("app.config");
			list.Remove("App.config");
			ProjectFile projectFile = base.Files.FirstOrDefault((ProjectFile f) => f.FilePath.FileName.Equals("app.config", StringComparison.CurrentCultureIgnoreCase));
			if (projectFile != null)
			{
				string fileName = this.GetOutputFileName(configuration).FileName;
				list.Add(projectFile.FilePath, true, fileName + ".config");
			}
			foreach (ProjectReference projectReference in this.References)
			{
				if (projectReference.LocalCopy && projectReference.CanSetLocalCopy)
				{
					if (base.ParentSolution != null && projectReference.ReferenceType == ReferenceType.Project)
					{
						DotNetProject dotNetProject = base.ParentSolution.FindProjectByName(projectReference.Reference) as DotNetProject;
						if (dotNetProject == null)
						{
							LoggingService.LogWarning("Project '{0}' referenced from '{1}' could not be found", new object[]
							{
								projectReference.Reference,
								this.Name
							});
							continue;
						}
						DotNetProjectConfiguration dotNetProjectConfiguration = dotNetProject.GetConfiguration(configuration) as DotNetProjectConfiguration;
						if (!DotNetProject.processedProjects.Add(dotNetProject) && DotNetProject.supportReferDistance != 1)
						{
							continue;
						}
						foreach (FilePath sourcePath in dotNetProject.GetOutputFiles(configuration))
						{
							list.Add(sourcePath, true, sourcePath.CanonicalPath.ToString().Substring(dotNetProjectConfiguration.OutputDirectory.CanonicalPath.ToString().Length + 1));
						}
						using (IEnumerator<FileCopySet.Item> enumerator3 = ((IEnumerable<FileCopySet.Item>)dotNetProject.GetSupportFileList(configuration)).GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								FileCopySet.Item item = enumerator3.Current;
								list.Add(item.Src, item.CopyOnlyIfNewer, item.Target);
							}
							continue;
						}
					}
					if (projectReference.ReferenceType == ReferenceType.Assembly)
					{
						HashSet<string> visitedAssemblies = new HashSet<string>();
						string[] referencedFileNames = projectReference.GetReferencedFileNames(configuration);
						using (IEnumerator<string> enumerator4 = referencedFileNames.SelectMany((string ar) => this.GetAssemblyRefsRec(ar, visitedAssemblies)).GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								string text = enumerator4.Current;
								bool copyOnlyIfNewer = !referencedFileNames.Contains(text);
								list.Add(text, copyOnlyIfNewer);
								if (File.Exists(text + ".config"))
								{
									list.Add(text + ".config", copyOnlyIfNewer);
								}
								string assemblyDebugInfoFile = this.TargetRuntime.GetAssemblyDebugInfoFile(text);
								if (File.Exists(assemblyDebugInfoFile))
								{
									list.Add(assemblyDebugInfoFile, copyOnlyIfNewer);
								}
							}
							continue;
						}
					}
					foreach (string name in projectReference.GetReferencedFileNames(configuration))
					{
						list.Add(name);
					}
				}
			}
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0003721C File Offset: 0x0003541C
		internal static string GetResourceCulture(string fname)
		{
			int num = -1;
			int num2 = -1;
			int i;
			for (i = fname.Length - 1; i >= 0; i--)
			{
				if (fname[i] == '.')
				{
					num = i;
					break;
				}
			}
			if (i < 0)
			{
				return null;
			}
			for (i--; i >= 0; i--)
			{
				if (fname[i] == '.')
				{
					num2 = i;
					break;
				}
			}
			if (num2 < 0)
			{
				return null;
			}
			string text = fname.Substring(num2 + 1, num - num2 - 1);
			if (!DotNetProject.CultureNamesTable.ContainsKey(text))
			{
				return null;
			}
			return text;
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x00037298 File Offset: 0x00035498
		private static Dictionary<string, string> CultureNamesTable
		{
			get
			{
				if (DotNetProject.cultureNamesTable == null)
				{
					DotNetProject.cultureNamesTable = new Dictionary<string, string>();
					foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(CultureTypes.AllCultures))
					{
						DotNetProject.cultureNamesTable[cultureInfo.Name] = cultureInfo.Name;
					}
				}
				return DotNetProject.cultureNamesTable;
			}
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x00037638 File Offset: 0x00035838
		private IEnumerable<string> GetAssemblyRefsRec(string fileName, HashSet<string> visited)
		{
			if (visited.Add(fileName))
			{
				if (!File.Exists(fileName))
				{
					string a = Path.GetExtension(fileName).ToLower();
					if (a == ".dll" || a == ".exe")
					{
						goto IL_1D6;
					}
					if (File.Exists(fileName + ".dll"))
					{
						fileName += ".dll";
					}
					else
					{
						if (!File.Exists(fileName + ".exe"))
						{
							goto IL_1D6;
						}
						fileName += ".exe";
					}
				}
				yield return fileName;
				foreach (string reference in SystemAssemblyService.GetAssemblyReferences(fileName))
				{
					string asmFile = Path.Combine(Path.GetDirectoryName(fileName), reference);
					foreach (string refa in this.GetAssemblyRefsRec(asmFile, visited))
					{
						yield return refa;
					}
				}
			}
			IL_1D6:
			yield break;
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x00037664 File Offset: 0x00035864
		public ProjectReference AddReference(string filename)
		{
			foreach (ProjectReference projectReference in this.References)
			{
				if (projectReference.Reference == filename)
				{
					return projectReference;
				}
			}
			ProjectReference projectReference2 = new ProjectReference(ReferenceType.Assembly, filename);
			this.References.Add(projectReference2);
			return projectReference2;
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x000376D4 File Offset: 0x000358D4
		public override IEnumerable<SolutionItem> GetReferencedItems(ConfigurationSelector configuration)
		{
			List<SolutionItem> list = new List<SolutionItem>(base.GetReferencedItems(configuration));
			if (base.ParentSolution == null)
			{
				return list;
			}
			foreach (ProjectReference projectReference in this.References)
			{
				if (projectReference.ReferenceType == ReferenceType.Project)
				{
					Project project = base.ParentSolution.FindProjectByName(projectReference.Reference);
					if (project != null)
					{
						list.Add(project);
					}
				}
			}
			return list;
		}

		/// <summary>
		/// Returns all assemblies referenced by this project, including assemblies generated
		/// by referenced projects.
		/// </summary>
		/// <param name="configuration">
		/// Configuration for which to get the assemblies.
		/// </param>
		// Token: 0x06000EDD RID: 3805 RVA: 0x00037758 File Offset: 0x00035958
		public IEnumerable<string> GetReferencedAssemblies(ConfigurationSelector configuration)
		{
			return this.GetReferencedAssemblies(configuration, true);
		}

		/// <summary>
		/// Returns all assemblies referenced by this project.
		/// </summary>
		/// <param name="configuration">
		/// Configuration for which to get the assemblies.
		/// </param>
		/// <param name="includeProjectReferences">
		/// When set to true, it will include assemblies generated by referenced project. When set to false,
		/// it will only include package and direct assembly references.
		/// </param>
		// Token: 0x06000EDE RID: 3806 RVA: 0x00037762 File Offset: 0x00035962
		public IEnumerable<string> GetReferencedAssemblies(ConfigurationSelector configuration, bool includeProjectReferences)
		{
			return Services.ProjectService.GetExtensionChain(this).GetReferencedAssemblies(this, configuration, includeProjectReferences);
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x00037D1C File Offset: 0x00035F1C
		protected internal virtual IEnumerable<string> OnGetReferencedAssemblies(ConfigurationSelector configuration, bool includeProjectReferences)
		{
			IAssemblyReferenceHandler handler = base.ItemHandler as IAssemblyReferenceHandler;
			if (handler != null)
			{
				if (includeProjectReferences)
				{
					foreach (ProjectReference pref in from pr in this.References
					where pr.ReferenceType == ReferenceType.Project
					select pr)
					{
						foreach (string asm in pref.GetReferencedFileNames(configuration))
						{
							yield return asm;
						}
					}
				}
				foreach (string file in handler.GetAssemblyReferences(configuration))
				{
					yield return file;
				}
			}
			else
			{
				foreach (ProjectReference pref2 in this.References)
				{
					if (includeProjectReferences || pref2.ReferenceType != ReferenceType.Project)
					{
						foreach (string asm2 in pref2.GetReferencedFileNames(configuration))
						{
							yield return asm2;
						}
					}
				}
			}
			DotNetProjectConfiguration config = (DotNetProjectConfiguration)this.GetConfiguration(configuration);
			bool noStdLib = false;
			if (config != null)
			{
				DotNetConfigurationParameters dotNetConfigurationParameters = config.CompilationParameters as DotNetConfigurationParameters;
				if (dotNetConfigurationParameters != null)
				{
					noStdLib = dotNetConfigurationParameters.NoStdLib;
				}
			}
			if (!noStdLib)
			{
				SystemAssembly sa = this.AssemblyContext.GetAssemblies(this.TargetFramework).FirstOrDefault((SystemAssembly a) => a.Name == "System.Core" && a.Package.IsFrameworkPackage);
				if (sa != null)
				{
					yield return sa.Location;
				}
			}
			yield break;
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x00037D47 File Offset: 0x00035F47
		protected internal override void OnSave(IProgressMonitor monitor)
		{
			if (this.targetFramework == null)
			{
				this.targetFramework = Runtime.SystemAssemblyService.GetTargetFramework(this.GetDefaultTargetFrameworkForFormat(base.FileFormat));
			}
			base.OnSave(monitor);
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x00037D74 File Offset: 0x00035F74
		private IDotNetLanguageBinding FindLanguage(string name)
		{
			return LanguageBindingService.GetBindingPerLanguageName(this.languageName) as IDotNetLanguageBinding;
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x00037D94 File Offset: 0x00035F94
		public override SolutionItemConfiguration CreateConfiguration(string name)
		{
			DotNetProjectConfiguration dotNetProjectConfiguration = new DotNetProjectConfiguration(name);
			string text;
			if (dotNetProjectConfiguration.Platform.Length == 0)
			{
				text = Path.Combine("bin", dotNetProjectConfiguration.Name);
			}
			else
			{
				text = Path.Combine(Path.Combine("bin", dotNetProjectConfiguration.Platform), dotNetProjectConfiguration.Name);
			}
			dotNetProjectConfiguration.OutputDirectory = (string.IsNullOrEmpty(base.BaseDirectory) ? text : Path.Combine(base.BaseDirectory, text));
			dotNetProjectConfiguration.OutputAssembly = this.Name;
			if (this.LanguageBinding != null)
			{
				XmlElement xmlElement = null;
				if (!string.IsNullOrEmpty(dotNetProjectConfiguration.Platform))
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlElement = xmlDocument.CreateElement("Options");
					xmlElement.SetAttribute("Platform", dotNetProjectConfiguration.Platform);
				}
				dotNetProjectConfiguration.CompilationParameters = this.LanguageBinding.CreateCompilationParameters(xmlElement);
			}
			return dotNetProjectConfiguration;
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x00037E70 File Offset: 0x00036070
		public override FilePath GetOutputFileName(ConfigurationSelector configuration)
		{
			DotNetProjectConfiguration dotNetProjectConfiguration = (DotNetProjectConfiguration)this.GetConfiguration(configuration);
			if (dotNetProjectConfiguration != null)
			{
				return dotNetProjectConfiguration.CompiledOutputName;
			}
			return null;
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x00037F1C File Offset: 0x0003611C
		protected override bool CheckNeedsBuild(ConfigurationSelector configuration)
		{
			if (base.CheckNeedsBuild(configuration))
			{
				return true;
			}
			DateTime lastBuildTime = base.GetLastBuildTime(configuration);
			foreach (ProjectReference projectReference in this.References)
			{
				switch (projectReference.ReferenceType)
				{
				case ReferenceType.Assembly:
					using (IEnumerator<string> enumerator2 = this.GetAssemblyRefsRec(projectReference.Reference, new HashSet<string>()).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							string path = enumerator2.Current;
							try
							{
								if (File.GetLastWriteTime(path) > lastBuildTime)
								{
									return true;
								}
							}
							catch (IOException)
							{
							}
						}
						continue;
					}
					break;
				case ReferenceType.Project:
					continue;
				case ReferenceType.Package:
					break;
				default:
					continue;
				}
				if (projectReference.Package != null)
				{
					foreach (SystemAssembly systemAssembly in projectReference.Package.Assemblies)
					{
						try
						{
							if (File.GetLastWriteTime(systemAssembly.Location) > lastBuildTime)
							{
								return true;
							}
						}
						catch (IOException)
						{
						}
					}
				}
			}
			DotNetProjectConfiguration config = (DotNetProjectConfiguration)this.GetConfiguration(configuration);
			return base.Files.Any((ProjectFile file) => file.BuildAction == "EmbeddedResource" && string.Compare(Path.GetExtension(file.FilePath), ".resx", StringComparison.OrdinalIgnoreCase) == 0 && MD1DotNetProjectHandler.IsResgenRequired(file.FilePath, config.IntermediateOutputDirectory.Combine(new string[]
			{
				file.ResourceId
			})));
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x000380B4 File Offset: 0x000362B4
		protected internal override DateTime OnGetLastBuildTime(ConfigurationSelector configuration)
		{
			DateTime dateTime = base.OnGetLastBuildTime(configuration);
			DotNetProjectConfiguration dotNetProjectConfiguration = (DotNetProjectConfiguration)this.GetConfiguration(configuration);
			if (this.GeneratesDebugInfoFile && dotNetProjectConfiguration != null && dotNetProjectConfiguration.DebugMode)
			{
				string text = this.GetOutputFileName(configuration);
				if (text != null)
				{
					text = this.TargetRuntime.GetAssemblyDebugInfoFile(text);
					FileInfo fileInfo = new FileInfo(text);
					if (fileInfo.Exists)
					{
						DateTime lastWriteTime = fileInfo.LastWriteTime;
						if (lastWriteTime > dateTime)
						{
							return lastWriteTime;
						}
					}
				}
			}
			return dateTime;
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x00038154 File Offset: 0x00036354
		public IList<string> GetUserAssemblyPaths(ConfigurationSelector configuration)
		{
			if (base.ParentSolution == null)
			{
				return null;
			}
			return (from d in base.ParentSolution.RootFolder.GetAllBuildableEntries(configuration).OfType<DotNetProject>()
			select d.GetOutputFileName(configuration) into d
			where !string.IsNullOrEmpty(d)
			select d).ToList<string>();
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x000381CC File Offset: 0x000363CC
		protected virtual ExecutionCommand CreateExecutionCommand(ConfigurationSelector configSel, DotNetProjectConfiguration configuration)
		{
			return new DotNetExecutionCommand(configuration.CompiledOutputName)
			{
				Arguments = configuration.CommandLineParameters,
				WorkingDirectory = Path.GetDirectoryName(configuration.CompiledOutputName),
				EnvironmentVariables = configuration.GetParsedEnvironmentVariables(),
				TargetRuntime = this.TargetRuntime,
				UserAssemblyPaths = this.GetUserAssemblyPaths(configSel)
			};
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00038234 File Offset: 0x00036434
		protected internal override bool OnGetCanExecute(ExecutionContext context, ConfigurationSelector configuration)
		{
			DotNetProjectConfiguration dotNetProjectConfiguration = (DotNetProjectConfiguration)this.GetConfiguration(configuration);
			if (dotNetProjectConfiguration == null)
			{
				return false;
			}
			ExecutionCommand executionCommand = this.CreateExecutionCommand(configuration, dotNetProjectConfiguration);
			if (context.ExecutionTarget != null)
			{
				executionCommand.Target = context.ExecutionTarget;
			}
			return (this.compileTarget == CompileTarget.Exe || this.compileTarget == CompileTarget.WinExe) && context.ExecutionHandler.CanExecute(executionCommand);
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00038290 File Offset: 0x00036490
		protected internal override List<FilePath> OnGetItemFiles(bool includeReferencedFiles)
		{
			List<FilePath> list = base.OnGetItemFiles(includeReferencedFiles);
			if (includeReferencedFiles)
			{
				foreach (ProjectReference projectReference in this.References)
				{
					if (projectReference.ReferenceType == ReferenceType.Assembly)
					{
						foreach (string name in projectReference.GetReferencedFileNames(base.DefaultConfiguration.Selector))
						{
							list.Add(name);
						}
					}
				}
				foreach (SolutionItemConfiguration solutionItemConfiguration in base.Configurations)
				{
					DotNetProjectConfiguration dotNetProjectConfiguration = (DotNetProjectConfiguration)solutionItemConfiguration;
					if (dotNetProjectConfiguration.SignAssembly)
					{
						list.Add(dotNetProjectConfiguration.AssemblyKeyFile);
					}
				}
			}
			return list;
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x00038384 File Offset: 0x00036584
		public override bool IsCompileable(string fileName)
		{
			return this.LanguageBinding != null && this.LanguageBinding.IsSourceCodeFile(fileName);
		}

		/// <summary>
		/// Gets the default namespace for the file, according to the naming policy.
		/// </summary>
		/// <remarks>Always returns a valid namespace, even if the fileName is null.</remarks>
		// Token: 0x06000EEB RID: 3819 RVA: 0x000383A1 File Offset: 0x000365A1
		public virtual string GetDefaultNamespace(string fileName)
		{
			return DotNetProject.GetDefaultNamespace(this, this.DefaultNamespace, fileName);
		}

		/// <summary>
		/// Gets the default namespace for the file, according to the naming policy.
		/// </summary>
		/// <remarks>Always returns a valid namespace, even if the fileName is null.</remarks>
		// Token: 0x06000EEC RID: 3820 RVA: 0x000383B0 File Offset: 0x000365B0
		internal static string GetDefaultNamespace(Project project, string defaultNamespace, string fileName)
		{
			DotNetNamingPolicy dotNetNamingPolicy = project.Policies.Get<DotNetNamingPolicy>();
			string text = null;
			string text2 = null;
			string text3 = (!string.IsNullOrEmpty(defaultNamespace)) ? defaultNamespace : (DotNetProject.SanitisePotentialNamespace(project.Name) ?? "Application");
			if (string.IsNullOrEmpty(fileName))
			{
				return text3;
			}
			string directoryName = Path.GetDirectoryName(fileName);
			string text4 = null;
			if (!string.IsNullOrEmpty(directoryName))
			{
				text4 = project.GetRelativeChildPath(directoryName);
				if (string.IsNullOrEmpty(text4) || text4.StartsWith(".."))
				{
					text4 = null;
				}
			}
			if (text4 != null)
			{
				try
				{
					switch (dotNetNamingPolicy.DirectoryNamespaceAssociation)
					{
					case DirectoryNamespaceAssociation.Flat:
						break;
					case DirectoryNamespaceAssociation.Hierarchical:
						goto IL_B6;
					case DirectoryNamespaceAssociation.PrefixedFlat:
						text = text3;
						break;
					case DirectoryNamespaceAssociation.PrefixedHierarchical:
						text = text3;
						goto IL_B6;
					default:
						goto IL_C3;
					}
					text2 = DotNetProject.SanitisePotentialNamespace(Path.GetFileName(text4));
					goto IL_C3;
					IL_B6:
					text2 = DotNetProject.SanitisePotentialNamespace(DotNetProject.GetHierarchicalNamespace(text4));
					IL_C3:;
				}
				catch (IOException ex)
				{
					LoggingService.LogError("Could not determine namespace for file '" + fileName + "'", ex);
				}
			}
			if (text2 != null && text == null)
			{
				return text2;
			}
			if (text2 != null && text != null)
			{
				return text + "." + text2;
			}
			return text3;
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x000384CC File Offset: 0x000366CC
		private static string GetHierarchicalNamespace(string relativePath)
		{
			StringBuilder stringBuilder = new StringBuilder(relativePath);
			for (int i = 0; i < stringBuilder.Length; i++)
			{
				if (stringBuilder[i] == Path.DirectorySeparatorChar)
				{
					stringBuilder[i] = '.';
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00038510 File Offset: 0x00036710
		private static string SanitisePotentialNamespace(string potential)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in potential)
			{
				if (char.IsLetter(c) || c == '_' || (stringBuilder.Length > 0 && (char.IsLetterOrDigit(stringBuilder[stringBuilder.Length - 1]) || stringBuilder[stringBuilder.Length - 1] == '_') && (c == '.' || char.IsNumber(c))))
				{
					stringBuilder.Append(c);
				}
			}
			if (stringBuilder.Length > 0)
			{
				if (stringBuilder[stringBuilder.Length - 1] == '.')
				{
					stringBuilder.Remove(stringBuilder.Length - 1, 1);
				}
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x000385C3 File Offset: 0x000367C3
		private void RuntimeSystemAssemblyServiceDefaultRuntimeChanged(object sender, EventArgs e)
		{
			if (this.composedAssemblyContext != null)
			{
				this.composedAssemblyContext.Replace(this.currentRuntimeContext, this.TargetRuntime.AssemblyContext);
				this.currentRuntimeContext = this.TargetRuntime.AssemblyContext;
			}
			this.UpdateSystemReferences();
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x00038600 File Offset: 0x00036800
		private void UpdateSystemReferences()
		{
			foreach (ProjectReference projectReference in this.References)
			{
				if (projectReference.ReferenceType == ReferenceType.Package)
				{
					SystemPackage package = projectReference.Package;
					string packageName = (package != null && !package.IsFrameworkPackage) ? package.Name : null;
					SystemAssembly assemblyForVersion = this.AssemblyContext.GetAssemblyForVersion(projectReference.Reference, packageName, this.TargetFramework);
					if (assemblyForVersion == null)
					{
						projectReference.ResetReference();
					}
					else if (assemblyForVersion.FullName != projectReference.Reference)
					{
						projectReference.Reference = assemblyForVersion.FullName;
					}
					else if (!projectReference.IsValid || assemblyForVersion.Package != projectReference.Package)
					{
						projectReference.ResetReference();
					}
				}
			}
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x000386D8 File Offset: 0x000368D8
		protected override IEnumerable<string> GetStandardBuildActions()
		{
			return BuildAction.DotNetActions;
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x000386DF File Offset: 0x000368DF
		protected override IList<string> GetCommonBuildActions()
		{
			return BuildAction.DotNetCommonActions;
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x000386E8 File Offset: 0x000368E8
		internal override void SetItemHandler(ISolutionItemHandler handler)
		{
			if (ProjectExtensionUtil.GetItemHandler(this) == null)
			{
				base.SetItemHandler(handler);
				return;
			}
			IResourceHandler oldHandler = this.ResourceHandler;
			base.SetItemHandler(handler);
			this.resourceHandler = null;
			this.MigrateResourceIds(oldHandler, this.ResourceHandler);
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x00038728 File Offset: 0x00036928
		protected override void OnEndLoad()
		{
			this.resourceHandler = null;
			IResourceHandler resourceHandler = base.ItemHandler as IResourceHandler;
			if (resourceHandler != null)
			{
				this.MigrateResourceIds(resourceHandler, this.ResourceHandler);
			}
			base.OnEndLoad();
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x00038760 File Offset: 0x00036960
		public void UpdateResourceHandler(bool keepOldIds)
		{
			IResourceHandler resourceHandler = this.resourceHandler;
			this.resourceHandler = null;
			if (keepOldIds && resourceHandler != null)
			{
				this.MigrateResourceIds(resourceHandler, this.ResourceHandler);
			}
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x000387A0 File Offset: 0x000369A0
		private void MigrateResourceIds(IResourceHandler oldHandler, IResourceHandler newHandler)
		{
			if (oldHandler.GetType() != newHandler.GetType())
			{
				foreach (ProjectFile projectFile in from f in base.Files
				where f.BuildAction == "EmbeddedResource"
				select f)
				{
					if (projectFile.Subtype != Subtype.Directory)
					{
						string resourceId = projectFile.GetResourceId(oldHandler);
						string resourceId2 = projectFile.GetResourceId(newHandler);
						string defaultResourceId = newHandler.GetDefaultResourceId(projectFile);
						if (resourceId != resourceId2)
						{
							if (defaultResourceId == resourceId)
							{
								projectFile.ResourceId = null;
							}
							else
							{
								projectFile.ResourceId = resourceId;
							}
						}
						else if (defaultResourceId == resourceId)
						{
							projectFile.ResourceId = null;
						}
					}
				}
			}
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x00038878 File Offset: 0x00036A78
		protected internal override void OnItemsAdded(IEnumerable<ProjectItem> objs)
		{
			base.OnItemsAdded(objs);
			foreach (ProjectReference projectReference in objs.OfType<ProjectReference>())
			{
				projectReference.SetOwnerProject(this);
				this.NotifyReferenceAddedToProject(projectReference);
			}
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x000388D4 File Offset: 0x00036AD4
		protected internal override void OnItemsRemoved(IEnumerable<ProjectItem> objs)
		{
			base.OnItemsRemoved(objs);
			foreach (ProjectReference projectReference in objs.OfType<ProjectReference>())
			{
				projectReference.SetOwnerProject(null);
				this.NotifyReferenceRemovedFromProject(projectReference);
			}
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x00038930 File Offset: 0x00036B30
		internal void NotifyReferenceRemovedFromProject(ProjectReference reference)
		{
			base.NotifyModified("References");
			this.OnReferenceRemovedFromProject(new ProjectReferenceEventArgs(this, reference));
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x0003894A File Offset: 0x00036B4A
		internal void NotifyReferenceAddedToProject(ProjectReference reference)
		{
			base.NotifyModified("References");
			this.OnReferenceAddedToProject(new ProjectReferenceEventArgs(this, reference));
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x00038964 File Offset: 0x00036B64
		protected virtual void OnReferenceRemovedFromProject(ProjectReferenceEventArgs e)
		{
			if (this.ReferenceRemovedFromProject != null)
			{
				this.ReferenceRemovedFromProject(this, e);
			}
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x0003897B File Offset: 0x00036B7B
		protected virtual void OnReferenceAddedToProject(ProjectReferenceEventArgs e)
		{
			if (this.ReferenceAddedToProject != null)
			{
				this.ReferenceAddedToProject(this, e);
			}
		}

		// Token: 0x14000068 RID: 104
		// (add) Token: 0x06000EFD RID: 3837 RVA: 0x00038994 File Offset: 0x00036B94
		// (remove) Token: 0x06000EFE RID: 3838 RVA: 0x000389CC File Offset: 0x00036BCC
		public event ProjectReferenceEventHandler ReferenceRemovedFromProject;

		// Token: 0x14000069 RID: 105
		// (add) Token: 0x06000EFF RID: 3839 RVA: 0x00038A04 File Offset: 0x00036C04
		// (remove) Token: 0x06000F00 RID: 3840 RVA: 0x00038A3C File Offset: 0x00036C3C
		public event ProjectReferenceEventHandler ReferenceAddedToProject;

		// Token: 0x06000F01 RID: 3841 RVA: 0x00038A74 File Offset: 0x00036C74
		private void OnFileRemoved(object o, FileEventArgs e)
		{
			foreach (FileEventInfo fileEventInfo in e)
			{
				this.CheckReferenceChange(fileEventInfo.FileName);
			}
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x00038AC4 File Offset: 0x00036CC4
		protected override void DoExecute(IProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration)
		{
			DotNetProjectConfiguration dotNetProjectConfiguration = this.GetConfiguration(configuration) as DotNetProjectConfiguration;
			monitor.Log.WriteLine(GettextCatalog.GetString("Running {0} ...", dotNetProjectConfiguration.CompiledOutputName));
			IConsole console = dotNetProjectConfiguration.ExternalConsole ? context.ExternalConsoleFactory.CreateConsole(!dotNetProjectConfiguration.PauseConsoleOutput) : context.ConsoleFactory.CreateConsole(!dotNetProjectConfiguration.PauseConsoleOutput);
			AggregatedOperationMonitor aggregatedOperationMonitor = new AggregatedOperationMonitor(monitor, new IAsyncOperation[0]);
			try
			{
				try
				{
					ExecutionCommand executionCommand = this.CreateExecutionCommand(configuration, dotNetProjectConfiguration);
					if (context.ExecutionTarget != null)
					{
						executionCommand.Target = context.ExecutionTarget;
					}
					if (!context.ExecutionHandler.CanExecute(executionCommand))
					{
						monitor.ReportError(GettextCatalog.GetString("Can not execute \"{0}\". The selected execution mode is not supported for .NET projects.", dotNetProjectConfiguration.CompiledOutputName), null);
					}
					else
					{
						IProcessAsyncOperation processAsyncOperation = context.ExecutionHandler.Execute(executionCommand, console);
						aggregatedOperationMonitor.AddOperation(processAsyncOperation);
						processAsyncOperation.WaitForCompleted();
						monitor.Log.WriteLine(GettextCatalog.GetString("The application exited with code: {0}", processAsyncOperation.ExitCode));
					}
				}
				finally
				{
					console.Dispose();
					aggregatedOperationMonitor.Dispose();
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError(string.Format("Cannot execute \"{0}\"", dotNetProjectConfiguration.CompiledOutputName), ex);
				monitor.ReportError(GettextCatalog.GetString("Cannot execute \"{0}\"", dotNetProjectConfiguration.CompiledOutputName), ex);
			}
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x00038C34 File Offset: 0x00036E34
		public void AddImportIfMissing(string name, string condition)
		{
			this.importsAdded.Add(new DotNetProjectImport(name, condition));
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x00038C48 File Offset: 0x00036E48
		public void RemoveImport(string name)
		{
			this.importsRemoved.Add(new DotNetProjectImport(name, null));
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000F05 RID: 3845 RVA: 0x00038C5C File Offset: 0x00036E5C
		internal IList<DotNetProjectImport> ImportsAdded
		{
			get
			{
				return this.importsAdded;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000F06 RID: 3846 RVA: 0x00038C64 File Offset: 0x00036E64
		internal IList<DotNetProjectImport> ImportsRemoved
		{
			get
			{
				return this.importsRemoved;
			}
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x00038C6C File Offset: 0x00036E6C
		public void ImportsSaved()
		{
			this.importsAdded.Clear();
			this.importsRemoved.Clear();
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x00038C84 File Offset: 0x00036E84
		public void RefreshProjectBuilder()
		{
			MSBuildProjectHandler msbuildProjectHandler = base.ItemHandler as MSBuildProjectHandler;
			if (msbuildProjectHandler != null)
			{
				msbuildProjectHandler.RefreshProjectBuilder();
			}
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x00038CA8 File Offset: 0x00036EA8
		public void DisposeProjectBuilder()
		{
			MSBuildProjectHandler msbuildProjectHandler = base.ItemHandler as MSBuildProjectHandler;
			if (msbuildProjectHandler != null)
			{
				msbuildProjectHandler.CleanupProjectBuilder();
			}
		}

		// Token: 0x04000443 RID: 1091
		private bool usePartialTypes = true;

		// Token: 0x04000444 RID: 1092
		private ProjectParameters languageParameters;

		// Token: 0x04000445 RID: 1093
		private DirectoryAssemblyContext privateAssemblyContext;

		// Token: 0x04000446 RID: 1094
		private ComposedAssemblyContext composedAssemblyContext;

		// Token: 0x04000447 RID: 1095
		private IAssemblyContext currentRuntimeContext;

		// Token: 0x04000448 RID: 1096
		[ItemProperty("OutputType")]
		private CompileTarget compileTarget;

		// Token: 0x04000449 RID: 1097
		private IDotNetLanguageBinding languageBinding;

		// Token: 0x0400044A RID: 1098
		protected ProjectReferenceCollection projectReferences;

		// Token: 0x0400044B RID: 1099
		[ItemProperty("RootNamespace", DefaultValue = "")]
		protected string defaultNamespace = string.Empty;

		// Token: 0x0400044C RID: 1100
		private string languageName;

		// Token: 0x0400044D RID: 1101
		private IResourceHandler resourceHandler;

		// Token: 0x0400044E RID: 1102
		private TargetFramework targetFramework;

		// Token: 0x0400044F RID: 1103
		[ThreadStatic]
		private static int supportReferDistance;

		// Token: 0x04000450 RID: 1104
		[ThreadStatic]
		private static HashSet<DotNetProject> processedProjects;

		// Token: 0x04000451 RID: 1105
		private static Dictionary<string, string> cultureNamesTable;

		// Token: 0x04000454 RID: 1108
		private List<DotNetProjectImport> importsAdded = new List<DotNetProjectImport>();

		// Token: 0x04000455 RID: 1109
		private List<DotNetProjectImport> importsRemoved = new List<DotNetProjectImport>();
	}
}
