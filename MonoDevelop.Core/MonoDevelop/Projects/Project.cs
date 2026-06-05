using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MonoDevelop.Core;
using MonoDevelop.Core.Instrumentation;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects.Formats.MSBuild;

namespace MonoDevelop.Projects
{
	/// <summary>
	/// A project
	/// </summary>
	/// <remarks>
	/// This is the base class for MonoDevelop projects. A project is a solution item which has a list of
	/// source code files and which can be built to generate an output.
	/// </remarks>
	// Token: 0x02000128 RID: 296
	[ProjectModelDataItem(FallbackType = typeof(UnknownProject))]
	[DataInclude(typeof(ProjectFile))]
	public abstract class Project : SolutionEntityItem
	{
		// Token: 0x06000AE1 RID: 2785 RVA: 0x000290AC File Offset: 0x000272AC
		protected Project()
		{
			FileService.FileChanged += this.OnFileChanged;
			this.files = new ProjectFileCollection();
			base.Items.Bind<ProjectFile>(this.files);
			this.DependencyResolutionEnabled = true;
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x00029138 File Offset: 0x00027338
		protected override void OnGetProjectEventMetadata(IDictionary<string, string> metadata)
		{
			base.OnGetProjectEventMetadata(metadata);
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			List<string> projectTypes = this.GetProjectTypes().ToList<string>();
			foreach (string value in from x in projectTypes
			where x != "DotNet" || projectTypes.Count == 1
			select x)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(value);
				flag = false;
			}
			metadata["ProjectTypes"] = stringBuilder.ToString();
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x000291E4 File Offset: 0x000273E4
		protected override void OnEndLoad()
		{
			base.OnEndLoad();
			Project.ProjectOpenedCounter.Inc(1, null, base.GetProjectEventMetadata());
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x000291FE File Offset: 0x000273FE
		// (set) Token: 0x06000AE5 RID: 2789 RVA: 0x00029206 File Offset: 0x00027406
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
				base.NotifyModified("Description");
			}
		}

		/// <summary>
		/// Determines whether the provided file can be as part of this project
		/// </summary>
		/// <returns>
		/// <c>true</c> if the file can be compiled; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="fileName">
		/// File name
		/// </param>
		// Token: 0x06000AE6 RID: 2790 RVA: 0x0002921A File Offset: 0x0002741A
		public virtual bool IsCompileable(string fileName)
		{
			return false;
		}

		/// <summary>
		/// Files of the project
		/// </summary>
		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x0002921D File Offset: 0x0002741D
		public ProjectFileCollection Files
		{
			get
			{
				return this.files;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x00029228 File Offset: 0x00027428
		// (set) Token: 0x06000AE9 RID: 2793 RVA: 0x00029267 File Offset: 0x00027467
		public virtual FilePath BaseIntermediateOutputPath
		{
			get
			{
				if (!this.baseIntermediateOutputPath.IsNullOrEmpty)
				{
					return this.baseIntermediateOutputPath;
				}
				return base.BaseDirectory.Combine(new string[]
				{
					"obj"
				});
			}
			set
			{
				if (value.IsNullOrEmpty)
				{
					value = FilePath.Null;
				}
				if (this.baseIntermediateOutputPath == value)
				{
					return;
				}
				base.NotifyModified("BaseIntermediateOutputPath");
			}
		}

		/// <summary>
		/// Gets the type of the project.
		/// </summary>
		/// <value>
		/// The type of the project.
		/// </value>
		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000AEA RID: 2794 RVA: 0x00029293 File Offset: 0x00027493
		[Obsolete("Use GetProjectTypes")]
		public virtual string ProjectType
		{
			get
			{
				return this.GetProjectTypes().First<string>();
			}
		}

		/// <summary>
		/// Gets the project type and its base types.
		/// </summary>
		// Token: 0x06000AEB RID: 2795
		public abstract IEnumerable<string> GetProjectTypes();

		/// <summary>
		/// Gets or sets the icon of the project.
		/// </summary>
		/// <value>
		/// The stock icon.
		/// </value>
		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x000292A0 File Offset: 0x000274A0
		// (set) Token: 0x06000AED RID: 2797 RVA: 0x000292A8 File Offset: 0x000274A8
		public virtual IconId StockIcon
		{
			get
			{
				return this.stockIcon;
			}
			set
			{
				this.stockIcon = value;
				base.NotifyModified("StockIcon");
			}
		}

		/// <summary>
		/// List of languages that this project supports
		/// </summary>
		/// <value>
		/// The identifiers of the supported languages.
		/// </value>
		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x000292BC File Offset: 0x000274BC
		public virtual string[] SupportedLanguages
		{
			get
			{
				return new string[]
				{
					""
				};
			}
		}

		/// <summary>
		/// Gets the default build action for a file
		/// </summary>
		/// <returns>
		/// The default build action.
		/// </returns>
		/// <param name="fileName">
		/// File name.
		/// </param>
		// Token: 0x06000AEF RID: 2799 RVA: 0x000292D9 File Offset: 0x000274D9
		public virtual string GetDefaultBuildAction(string fileName)
		{
			if (!this.IsCompileable(fileName))
			{
				return "None";
			}
			return "Compile";
		}

		/// <summary>
		/// Gets a project file.
		/// </summary>
		/// <returns>
		/// The project file.
		/// </returns>
		/// <param name="fileName">
		/// File name.
		/// </param>
		// Token: 0x06000AF0 RID: 2800 RVA: 0x000292EF File Offset: 0x000274EF
		public ProjectFile GetProjectFile(string fileName)
		{
			return this.files.GetFile(fileName);
		}

		/// <summary>
		/// Determines whether a file belongs to this project
		/// </summary>
		/// <param name="fileName">
		/// File name
		/// </param>
		// Token: 0x06000AF1 RID: 2801 RVA: 0x00029302 File Offset: 0x00027502
		public bool IsFileInProject(string fileName)
		{
			return this.files.GetFile(fileName) != null;
		}

		/// <summary>
		/// Gets a list of build actions supported by this project
		/// </summary>
		/// <remarks>
		/// Common actions are grouped at the top, separated by a "--" entry *IF* there are 
		/// more "uncommon" actions than "common" actions
		/// </remarks>
		// Token: 0x06000AF2 RID: 2802 RVA: 0x0002931C File Offset: 0x0002751C
		public string[] GetBuildActions()
		{
			if (this.buildActions != null)
			{
				return this.buildActions;
			}
			Hashtable hashtable = new Hashtable();
			object value = new object();
			foreach (string key in this.GetStandardBuildActions())
			{
				hashtable[key] = value;
			}
			foreach (ProjectFile projectFile in this.files)
			{
				if (!hashtable.ContainsKey(projectFile.BuildAction))
				{
					hashtable[projectFile.BuildAction] = value;
				}
			}
			IList<string> commonBuildActions = this.GetCommonBuildActions();
			foreach (string key2 in commonBuildActions)
			{
				if (hashtable.Contains(key2))
				{
					hashtable.Remove(key2);
				}
			}
			int count = commonBuildActions.Count;
			bool flag = commonBuildActions.Count > 0 && hashtable.Count > 0;
			int num = commonBuildActions.Count + hashtable.Count;
			int num2 = flag ? (count + 1) : count;
			if (flag)
			{
				num++;
			}
			this.buildActions = new string[num];
			if (commonBuildActions.Count > 0)
			{
				commonBuildActions.CopyTo(this.buildActions, 0);
			}
			if (flag)
			{
				this.buildActions[count] = "--";
			}
			if (hashtable.Count > 0)
			{
				hashtable.Keys.CopyTo(this.buildActions, num2);
			}
			if (flag)
			{
				Array.Sort<string>(this.buildActions, num2, num - num2, StringComparer.Ordinal);
			}
			else
			{
				Array.Sort<string>(this.buildActions, StringComparer.Ordinal);
			}
			return this.buildActions;
		}

		/// <summary>
		/// Gets a list of standard build actions.
		/// </summary>
		// Token: 0x06000AF3 RID: 2803 RVA: 0x00029500 File Offset: 0x00027700
		protected virtual IEnumerable<string> GetStandardBuildActions()
		{
			return BuildAction.StandardActions;
		}

		/// <summary>
		/// Gets a list of common build actions (common actions are shown first in the project build action list)
		/// </summary>
		// Token: 0x06000AF4 RID: 2804 RVA: 0x00029507 File Offset: 0x00027707
		protected virtual IList<string> GetCommonBuildActions()
		{
			return BuildAction.StandardActions;
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x00029510 File Offset: 0x00027710
		public static Project LoadProject(string filename, IProgressMonitor monitor)
		{
			Project project = Services.ProjectService.ReadSolutionItem(monitor, filename) as Project;
			if (project == null)
			{
				throw new InvalidOperationException("Invalid project file: " + filename);
			}
			return project;
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x00029544 File Offset: 0x00027744
		public override void Dispose()
		{
			FileService.FileChanged -= this.OnFileChanged;
			base.Dispose();
		}

		/// <summary>
		/// Adds a file to the project
		/// </summary>
		/// <returns>
		/// The file instance.
		/// </returns>
		/// <param name="filename">
		/// Absolute path to the file.
		/// </param>
		// Token: 0x06000AF7 RID: 2807 RVA: 0x0002955E File Offset: 0x0002775E
		public ProjectFile AddFile(string filename)
		{
			return this.AddFile(filename, null);
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x00029568 File Offset: 0x00027768
		public IEnumerable<ProjectFile> AddFiles(IEnumerable<FilePath> files)
		{
			return this.AddFiles(files, null);
		}

		/// <summary>
		/// Adds a file to the project
		/// </summary>
		/// <returns>
		/// The file instance.
		/// </returns>
		/// <param name="filename">
		/// Absolute path to the file.
		/// </param>
		/// <param name="buildAction">
		/// Build action to assign to the file.
		/// </param>
		// Token: 0x06000AF9 RID: 2809 RVA: 0x00029574 File Offset: 0x00027774
		public ProjectFile AddFile(string filename, string buildAction)
		{
			foreach (ProjectFile projectFile in this.Files)
			{
				if (projectFile.Name == filename)
				{
					return projectFile;
				}
			}
			if (string.IsNullOrEmpty(buildAction))
			{
				buildAction = this.GetDefaultBuildAction(filename);
			}
			ProjectFile projectFile2 = new ProjectFile(filename, buildAction);
			this.Files.Add(projectFile2);
			return projectFile2;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x000295F4 File Offset: 0x000277F4
		public IEnumerable<ProjectFile> AddFiles(IEnumerable<FilePath> files, string buildAction)
		{
			List<ProjectFile> list = new List<ProjectFile>();
			foreach (FilePath filePath in files)
			{
				string text = buildAction;
				if (string.IsNullOrEmpty(text))
				{
					text = this.GetDefaultBuildAction(filePath);
				}
				ProjectFile item = new ProjectFile(filePath, text);
				list.Add(item);
			}
			this.Files.AddRange(list);
			return list;
		}

		/// <summary>
		/// Adds a file to the project
		/// </summary>
		/// <param name="projectFile">
		/// The file.
		/// </param>
		// Token: 0x06000AFB RID: 2811 RVA: 0x00029678 File Offset: 0x00027878
		public void AddFile(ProjectFile projectFile)
		{
			this.Files.Add(projectFile);
		}

		/// <summary>
		/// Adds a directory to the project.
		/// </summary>
		/// <returns>
		/// The directory instance.
		/// </returns>
		/// <param name="relativePath">
		/// Relative path of the directory.
		/// </param>
		/// <remarks>
		/// The directory is created if it doesn't exist
		/// </remarks>
		// Token: 0x06000AFC RID: 2812 RVA: 0x00029688 File Offset: 0x00027888
		public ProjectFile AddDirectory(string relativePath)
		{
			string text = Path.Combine(base.BaseDirectory, relativePath);
			foreach (ProjectFile projectFile in this.Files)
			{
				if (projectFile.Name == text && projectFile.Subtype == Subtype.Directory)
				{
					return projectFile;
				}
			}
			if (!Directory.Exists(text))
			{
				if (File.Exists(text))
				{
					string @string = GettextCatalog.GetString("Cannot create directory {0}, as a file with that name exists.", text);
					throw new InvalidOperationException(@string);
				}
				FileService.CreateDirectory(text);
			}
			ProjectFile projectFile2 = new ProjectFile(text);
			projectFile2.Subtype = Subtype.Directory;
			this.AddFile(projectFile2);
			return projectFile2;
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x00029744 File Offset: 0x00027944
		private bool UsingMSBuildEngine(ConfigurationSelector sel)
		{
			MSBuildProjectHandler msbuildProjectHandler = base.ItemHandler as MSBuildProjectHandler;
			return msbuildProjectHandler != null && msbuildProjectHandler.UseMSBuildEngineForItem(this, sel, true);
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0002976C File Offset: 0x0002796C
		protected override BuildResult OnBuild(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			ProjectConfiguration projectConfiguration = this.GetConfiguration(configuration) as ProjectConfiguration;
			if (projectConfiguration == null)
			{
				BuildResult buildResult = new BuildResult();
				buildResult.AddError(GettextCatalog.GetString("Configuration '{0}' not found in project '{1}'", configuration.ToString(), this.Name));
				return buildResult;
			}
			StringParserService.Properties["Project"] = this.Name;
			if (this.UsingMSBuildEngine(configuration))
			{
				return this.DoBuild(monitor, configuration);
			}
			string text = projectConfiguration.OutputDirectory;
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(text);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Can't create project output directory " + text + " original exception:\n" + ex.ToString());
			}
			this.CopySupportFiles(monitor, configuration);
			monitor.Log.WriteLine("Performing main compilation...");
			BuildResult buildResult2 = this.DoBuild(monitor, configuration);
			if (buildResult2 != null)
			{
				string pluralString = GettextCatalog.GetPluralString("{0} error", "{0} errors", buildResult2.ErrorCount, buildResult2.ErrorCount);
				string pluralString2 = GettextCatalog.GetPluralString("{0} warning", "{0} warnings", buildResult2.WarningCount, buildResult2.WarningCount);
				monitor.Log.WriteLine(GettextCatalog.GetString("Build complete -- ") + pluralString + ", " + pluralString2);
			}
			return buildResult2;
		}

		/// <summary>
		/// Copies the support files to the output directory
		/// </summary>
		/// <param name="monitor">
		/// Progress monitor.
		/// </param>
		/// <param name="configuration">
		/// Configuration for which to copy the files.
		/// </param>
		/// <remarks>
		/// Copies all support files to the output directory of the given configuration. Support files
		/// include: assembly references with the Local Copy flag, data files with the Copy to Output option, etc.
		/// </remarks>
		// Token: 0x06000AFF RID: 2815 RVA: 0x000298BC File Offset: 0x00027ABC
		public void CopySupportFiles(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			ProjectConfiguration projectConfiguration = (ProjectConfiguration)this.GetConfiguration(configuration);
			foreach (FileCopySet.Item item in ((IEnumerable<FileCopySet.Item>)this.GetSupportFileList(configuration)))
			{
				FilePath filePath = Path.GetFullPath(Path.Combine(projectConfiguration.OutputDirectory, item.Target));
				FilePath filePath2 = Path.GetFullPath(item.Src);
				try
				{
					if (!(filePath == filePath2))
					{
						if (!item.CopyOnlyIfNewer || !File.Exists(filePath) || !(File.GetLastWriteTimeUtc(filePath) >= File.GetLastWriteTimeUtc(filePath2)))
						{
							if (!Directory.Exists(Path.GetDirectoryName(filePath)))
							{
								Directory.CreateDirectory(Path.GetDirectoryName(filePath));
							}
							if (File.Exists(filePath2))
							{
								filePath.Delete();
								FileService.CopyFile(filePath2, filePath);
								FileAttributes attributes = File.GetAttributes(filePath);
								if (attributes.HasFlag(FileAttributes.ReadOnly))
								{
									File.SetAttributes(filePath, attributes & ~FileAttributes.ReadOnly);
								}
							}
							else
							{
								monitor.ReportError(GettextCatalog.GetString("Could not find support file '{0}'.", filePath2), null);
							}
						}
					}
				}
				catch (IOException exception)
				{
					monitor.ReportError(GettextCatalog.GetString("Error copying support file '{0}'.", filePath), exception);
				}
			}
		}

		/// <summary>
		/// Removes all support files from the output directory
		/// </summary>
		/// <param name="monitor">
		/// Progress monitor.
		/// </param>
		/// <param name="configuration">
		/// Configuration for which to delete the files.
		/// </param>
		/// <remarks>
		/// Deletes all support files from the output directory of the given configuration. Support files
		/// include: assembly references with the Local Copy flag, data files with the Copy to Output option, etc.
		/// </remarks>
		// Token: 0x06000B00 RID: 2816 RVA: 0x00029A70 File Offset: 0x00027C70
		public void DeleteSupportFiles(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			ProjectConfiguration projectConfiguration = (ProjectConfiguration)this.GetConfiguration(configuration);
			foreach (FileCopySet.Item item in ((IEnumerable<FileCopySet.Item>)this.GetSupportFileList(configuration)))
			{
				FilePath filePath = Path.Combine(projectConfiguration.OutputDirectory, item.Target);
				if (!(Path.GetFullPath(filePath) == Path.GetFullPath(item.Src)))
				{
					try
					{
						filePath.Delete();
					}
					catch (IOException exception)
					{
						monitor.ReportError(GettextCatalog.GetString("Error deleting support file '{0}'.", filePath), exception);
					}
				}
			}
		}

		/// <summary>
		/// Gets a list of files required to use the project output
		/// </summary>
		/// <returns>
		/// A list of files.
		/// </returns>
		/// <param name="configuration">
		/// Build configuration for which get the list
		/// </param>
		/// <remarks>
		/// Returns a list of all files that are required to use the project output binary, for example: data files with
		/// the Copy to Output option, debug information files, generated resource files, etc.
		/// </remarks>
		// Token: 0x06000B01 RID: 2817 RVA: 0x00029B3C File Offset: 0x00027D3C
		public FileCopySet GetSupportFileList(ConfigurationSelector configuration)
		{
			FileCopySet fileCopySet = new FileCopySet();
			this.PopulateSupportFileList(fileCopySet, configuration);
			return fileCopySet;
		}

		/// <summary>
		/// Gets a list of files required to use the project output
		/// </summary>
		/// <param name="list">
		/// List where to add the support files.
		/// </param>
		/// <param name="configuration">
		/// Build configuration for which get the list
		/// </param>
		/// <remarks>
		/// Returns a list of all files that are required to use the project output binary, for example: data files with
		/// the Copy to Output option, debug information files, generated resource files, etc.
		/// </remarks>
		// Token: 0x06000B02 RID: 2818 RVA: 0x00029B58 File Offset: 0x00027D58
		protected internal virtual void PopulateSupportFileList(FileCopySet list, ConfigurationSelector configuration)
		{
			foreach (ProjectFile projectFile in this.Files)
			{
				if (projectFile.CopyToOutputDirectory != FileCopyMode.None)
				{
					list.Add(projectFile.FilePath, projectFile.CopyToOutputDirectory == FileCopyMode.PreserveNewest, projectFile.ProjectVirtualPath);
				}
			}
		}

		/// <summary>
		/// Gets a list of files generated when building this project
		/// </summary>
		/// <returns>
		/// A list of files.
		/// </returns>
		/// <param name="configuration">
		/// Build configuration for which get the list
		/// </param>
		/// <remarks>
		/// Returns a list of all files that are generated when this project is built, including: the generated binary,
		/// debug information files, satellite assemblies.
		/// </remarks>
		// Token: 0x06000B03 RID: 2819 RVA: 0x00029BC4 File Offset: 0x00027DC4
		public List<FilePath> GetOutputFiles(ConfigurationSelector configuration)
		{
			List<FilePath> list = new List<FilePath>();
			this.PopulateOutputFileList(list, configuration);
			return list;
		}

		/// <summary>
		/// Gets a list of files retuired to use the project output
		/// </summary>
		/// <param name="list">
		/// List where to add the support files.
		/// </param>
		/// <param name="configuration">
		/// Build configuration for which get the list
		/// </param>
		/// <remarks>
		/// Returns a list of all files that are required to use the project output binary, for example: data files with
		/// the Copy to Output option, debug information files, generated resource files, etc.
		/// </remarks>
		// Token: 0x06000B04 RID: 2820 RVA: 0x00029BE0 File Offset: 0x00027DE0
		protected internal virtual void PopulateOutputFileList(List<FilePath> list, ConfigurationSelector configuration)
		{
			string text = this.GetOutputFileName(configuration);
			if (text != null)
			{
				list.Add(text);
			}
		}

		/// <summary>
		/// Builds the project.
		/// </summary>
		/// <returns>
		/// The build result.
		/// </returns>
		/// <param name="monitor">
		/// Progress monitor.
		/// </param>
		/// <param name="configuration">
		/// Configuration to build.
		/// </param>
		/// <remarks>
		/// This method is invoked to build the project. Support files such as files with the Copy to Output flag will
		/// be copied before calling this method.
		/// </remarks>
		// Token: 0x06000B05 RID: 2821 RVA: 0x00029C0C File Offset: 0x00027E0C
		protected virtual BuildResult DoBuild(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			BuildResult buildResult = base.ItemHandler.RunTarget(monitor, "Build", configuration);
			return buildResult ?? new BuildResult();
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x00029C38 File Offset: 0x00027E38
		protected override void OnClean(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			ProjectConfiguration projectConfiguration = this.GetConfiguration(configuration) as ProjectConfiguration;
			if (projectConfiguration == null)
			{
				monitor.ReportError(GettextCatalog.GetString("Configuration '{0}' not found in project '{1}'", configuration, this.Name), null);
				return;
			}
			if (this.UsingMSBuildEngine(configuration))
			{
				this.DoClean(monitor, projectConfiguration.Selector);
				return;
			}
			monitor.Log.WriteLine("Removing output files...");
			foreach (FilePath filePath in this.GetOutputFiles(configuration))
			{
				if (File.Exists(filePath))
				{
					filePath.Delete();
					if (filePath.ParentDirectory.CanonicalPath != projectConfiguration.OutputDirectory.CanonicalPath && Directory.GetFiles(filePath.ParentDirectory).Length == 0)
					{
						filePath.ParentDirectory.Delete();
					}
				}
			}
			this.DeleteSupportFiles(monitor, configuration);
			this.DoClean(monitor, projectConfiguration.Selector);
			monitor.Log.WriteLine(GettextCatalog.GetString("Clean complete"));
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x00029D60 File Offset: 0x00027F60
		protected virtual void DoClean(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			base.ItemHandler.RunTarget(monitor, "Clean", configuration);
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x00029D78 File Offset: 0x00027F78
		protected internal override void OnExecute(IProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration)
		{
			if (!(this.GetConfiguration(configuration) is ProjectConfiguration))
			{
				monitor.ReportError(GettextCatalog.GetString("Configuration '{0}' not found in project '{1}'", configuration, this.Name), null);
				return;
			}
			this.DoExecute(monitor, context, configuration);
		}

		/// <summary>
		/// Executes the project
		/// </summary>
		/// <param name="monitor">
		/// Progress monitor.
		/// </param>
		/// <param name="context">
		/// Execution context.
		/// </param>
		/// <param name="configuration">
		/// Configuration to execute.
		/// </param>
		// Token: 0x06000B09 RID: 2825 RVA: 0x00029DB7 File Offset: 0x00027FB7
		protected virtual void DoExecute(IProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration)
		{
		}

		/// <summary>
		/// Gets the absolute path to the output file generated by this project.
		/// </summary>
		/// <returns>
		/// Absolute path the the output file.
		/// </returns>
		/// <param name="configuration">
		/// Build configuration.
		/// </param>
		// Token: 0x06000B0A RID: 2826 RVA: 0x00029DB9 File Offset: 0x00027FB9
		public virtual FilePath GetOutputFileName(ConfigurationSelector configuration)
		{
			return FilePath.Null;
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00029DC0 File Offset: 0x00027FC0
		protected internal override bool OnGetNeedsBuilding(ConfigurationSelector configuration)
		{
			return this.CheckNeedsBuild(configuration);
		}

		/// <summary>
		/// Checks if the project needs to be built
		/// </summary>
		/// <returns>
		/// <c>True</c> if the project needs to be built (it has changes)
		/// </returns>
		/// <param name="configuration">
		/// Build configuration.
		/// </param>
		// Token: 0x06000B0C RID: 2828 RVA: 0x00029DCC File Offset: 0x00027FCC
		protected virtual bool CheckNeedsBuild(ConfigurationSelector configuration)
		{
			DateTime lastBuildTime = base.GetLastBuildTime(configuration);
			if (lastBuildTime == DateTime.MinValue)
			{
				return true;
			}
			foreach (ProjectFile projectFile in this.Files)
			{
				if (!(projectFile.BuildAction == "Content") && !(projectFile.BuildAction == "None"))
				{
					try
					{
						if (File.GetLastWriteTime(projectFile.FilePath) > lastBuildTime)
						{
							return true;
						}
					}
					catch (IOException)
					{
					}
				}
			}
			foreach (SolutionItem solutionItem in this.GetReferencedItems(configuration))
			{
				if (solutionItem.GetLastBuildTime(configuration) > lastBuildTime)
				{
					return true;
				}
			}
			try
			{
				if (File.GetLastWriteTime(this.FileName) > lastBuildTime)
				{
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00029F00 File Offset: 0x00028100
		protected internal override DateTime OnGetLastBuildTime(ConfigurationSelector configuration)
		{
			string text = this.GetOutputFileName(configuration);
			if (text == null)
			{
				return DateTime.MinValue;
			}
			FileInfo fileInfo = new FileInfo(text);
			if (!fileInfo.Exists)
			{
				return DateTime.MinValue;
			}
			return fileInfo.LastWriteTime;
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00029F40 File Offset: 0x00028140
		internal virtual void OnFileChanged(object source, FileEventArgs e)
		{
			foreach (FileEventInfo fileEventInfo in e)
			{
				ProjectFile projectFile = this.GetProjectFile(fileEventInfo.FileName);
				if (projectFile != null)
				{
					base.SetFastBuildCheckDirty();
					try
					{
						this.NotifyFileChangedInProject(projectFile);
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x00029FB4 File Offset: 0x000281B4
		protected internal override List<FilePath> OnGetItemFiles(bool includeReferencedFiles)
		{
			List<FilePath> list = base.OnGetItemFiles(includeReferencedFiles);
			if (includeReferencedFiles)
			{
				foreach (ProjectFile projectFile in this.Files)
				{
					if (projectFile.Subtype != Subtype.Directory)
					{
						list.Add(projectFile.FilePath);
					}
				}
			}
			return list;
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0002A01C File Offset: 0x0002821C
		protected internal override void OnItemsAdded(IEnumerable<ProjectItem> objs)
		{
			base.OnItemsAdded(objs);
			this.NotifyFileAddedToProject(objs.OfType<ProjectFile>());
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x0002A031 File Offset: 0x00028231
		protected internal override void OnItemsRemoved(IEnumerable<ProjectItem> objs)
		{
			base.OnItemsRemoved(objs);
			this.NotifyFileRemovedFromProject(objs.OfType<ProjectFile>());
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x0002A046 File Offset: 0x00028246
		internal void NotifyFileChangedInProject(ProjectFile file)
		{
			this.OnFileChangedInProject(new ProjectFileEventArgs(this, file));
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x0002A055 File Offset: 0x00028255
		internal void NotifyFilePropertyChangedInProject(ProjectFile file, string property)
		{
			base.NotifyModified("Files");
			this.OnFilePropertyChangedInProject(new ProjectFileEventArgs(this, file, property));
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x0002A070 File Offset: 0x00028270
		private void NotifyFileRemovedFromProject(IEnumerable<ProjectFile> objs)
		{
			if (!objs.Any<ProjectFile>())
			{
				return;
			}
			ProjectFileEventArgs projectFileEventArgs = new ProjectFileEventArgs();
			foreach (ProjectFile projectFile in objs)
			{
				projectFile.SetProject(null);
				projectFileEventArgs.Add(new ProjectFileEventInfo(this, projectFile));
				if (this.DependencyResolutionEnabled)
				{
					this.unresolvedDeps.Remove(projectFile);
					foreach (ProjectFile projectFile2 in projectFile.DependentChildren)
					{
						projectFile2.DependsOnFile = null;
						if (!string.IsNullOrEmpty(projectFile2.DependsOn))
						{
							this.unresolvedDeps.Add(projectFile2);
						}
					}
					projectFile.DependsOn = null;
				}
			}
			base.NotifyModified("Files");
			this.OnFileRemovedFromProject(projectFileEventArgs);
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x0002A164 File Offset: 0x00028364
		private void NotifyFileAddedToProject(IEnumerable<ProjectFile> objs)
		{
			if (!objs.Any<ProjectFile>())
			{
				return;
			}
			ProjectFileEventArgs projectFileEventArgs = new ProjectFileEventArgs();
			foreach (ProjectFile projectFile in objs)
			{
				if (projectFile.Project != null)
				{
					throw new InvalidOperationException("ProjectFile already belongs to a project");
				}
				projectFile.SetProject(this);
				projectFileEventArgs.Add(new ProjectFileEventInfo(this, projectFile));
				this.ResolveDependencies(projectFile);
			}
			base.NotifyModified("Files");
			this.OnFileAddedToProject(projectFileEventArgs);
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x0002A1F4 File Offset: 0x000283F4
		internal void UpdateDependency(ProjectFile file, FilePath oldPath)
		{
			this.unresolvedDeps.Remove(file, oldPath);
			this.ResolveDependencies(file);
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x0002A20C File Offset: 0x0002840C
		internal void ResolveDependencies(ProjectFile file)
		{
			if (!this.DependencyResolutionEnabled)
			{
				return;
			}
			if (!file.ResolveParent())
			{
				this.unresolvedDeps.Add(file);
			}
			List<ProjectFile> list = null;
			foreach (ProjectFile projectFile in this.unresolvedDeps.GetUnresolvedFilesForPath(file.FilePath))
			{
				if (string.IsNullOrEmpty(projectFile.DependsOn))
				{
					if (list == null)
					{
						list = new List<ProjectFile>();
					}
					list.Add(projectFile);
				}
				if (projectFile.ResolveParent(file))
				{
					if (list == null)
					{
						list = new List<ProjectFile>();
					}
					list.Add(projectFile);
				}
			}
			if (list != null)
			{
				foreach (ProjectFile file2 in list)
				{
					this.unresolvedDeps.Remove(file2);
				}
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000B18 RID: 2840 RVA: 0x0002A2F8 File Offset: 0x000284F8
		// (set) Token: 0x06000B19 RID: 2841 RVA: 0x0002A308 File Offset: 0x00028508
		private bool DependencyResolutionEnabled
		{
			get
			{
				return this.unresolvedDeps != null;
			}
			set
			{
				if (value)
				{
					if (this.unresolvedDeps != null)
					{
						return;
					}
					this.unresolvedDeps = new UnresolvedFileCollection();
					using (IEnumerator<ProjectFile> enumerator = this.files.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ProjectFile file = enumerator.Current;
							this.ResolveDependencies(file);
						}
						return;
					}
				}
				this.unresolvedDeps = null;
			}
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0002A374 File Offset: 0x00028574
		internal void NotifyFileRenamedInProject(ProjectFileRenamedEventArgs args)
		{
			base.NotifyModified("Files");
			this.OnFileRenamedInProject(args);
		}

		/// <summary>
		/// Raises the FileRemovedFromProject event.
		/// </summary>
		// Token: 0x06000B1B RID: 2843 RVA: 0x0002A388 File Offset: 0x00028588
		protected virtual void OnFileRemovedFromProject(ProjectFileEventArgs e)
		{
			this.buildActions = null;
			if (this.FileRemovedFromProject != null)
			{
				this.FileRemovedFromProject(this, e);
			}
		}

		/// <summary>
		/// Raises the FileAddedToProject event.
		/// </summary>
		// Token: 0x06000B1C RID: 2844 RVA: 0x0002A3A6 File Offset: 0x000285A6
		protected virtual void OnFileAddedToProject(ProjectFileEventArgs e)
		{
			this.buildActions = null;
			if (this.FileAddedToProject != null)
			{
				this.FileAddedToProject(this, e);
			}
		}

		/// <summary>
		/// Raises the FileChangedInProject event.
		/// </summary>
		// Token: 0x06000B1D RID: 2845 RVA: 0x0002A3C4 File Offset: 0x000285C4
		protected virtual void OnFileChangedInProject(ProjectFileEventArgs e)
		{
			if (this.FileChangedInProject != null)
			{
				this.FileChangedInProject(this, e);
			}
		}

		/// <summary>
		/// Raises the FilePropertyChangedInProject event.
		/// </summary>
		// Token: 0x06000B1E RID: 2846 RVA: 0x0002A3DB File Offset: 0x000285DB
		protected virtual void OnFilePropertyChangedInProject(ProjectFileEventArgs e)
		{
			this.buildActions = null;
			if (this.FilePropertyChangedInProject != null)
			{
				this.FilePropertyChangedInProject(this, e);
			}
		}

		/// <summary>
		/// Raises the FileRenamedInProject event.
		/// </summary>
		// Token: 0x06000B1F RID: 2847 RVA: 0x0002A3F9 File Offset: 0x000285F9
		protected virtual void OnFileRenamedInProject(ProjectFileRenamedEventArgs e)
		{
			if (this.FileRenamedInProject != null)
			{
				this.FileRenamedInProject(this, e);
			}
		}

		/// <summary>
		/// Occurs when a file is removed from this project.
		/// </summary>
		// Token: 0x14000038 RID: 56
		// (add) Token: 0x06000B20 RID: 2848 RVA: 0x0002A410 File Offset: 0x00028610
		// (remove) Token: 0x06000B21 RID: 2849 RVA: 0x0002A448 File Offset: 0x00028648
		public event ProjectFileEventHandler FileRemovedFromProject;

		/// <summary>
		/// Occurs when a file is added to this project.
		/// </summary>
		// Token: 0x14000039 RID: 57
		// (add) Token: 0x06000B22 RID: 2850 RVA: 0x0002A480 File Offset: 0x00028680
		// (remove) Token: 0x06000B23 RID: 2851 RVA: 0x0002A4B8 File Offset: 0x000286B8
		public event ProjectFileEventHandler FileAddedToProject;

		/// <summary>
		/// Occurs when a file of this project has been modified
		/// </summary>
		// Token: 0x1400003A RID: 58
		// (add) Token: 0x06000B24 RID: 2852 RVA: 0x0002A4F0 File Offset: 0x000286F0
		// (remove) Token: 0x06000B25 RID: 2853 RVA: 0x0002A528 File Offset: 0x00028728
		public event ProjectFileEventHandler FileChangedInProject;

		/// <summary>
		/// Occurs when a property of a file of this project has changed
		/// </summary>
		// Token: 0x1400003B RID: 59
		// (add) Token: 0x06000B26 RID: 2854 RVA: 0x0002A560 File Offset: 0x00028760
		// (remove) Token: 0x06000B27 RID: 2855 RVA: 0x0002A598 File Offset: 0x00028798
		public event ProjectFileEventHandler FilePropertyChangedInProject;

		/// <summary>
		/// Occurs when a file of this project has been renamed
		/// </summary>
		// Token: 0x1400003C RID: 60
		// (add) Token: 0x06000B28 RID: 2856 RVA: 0x0002A5D0 File Offset: 0x000287D0
		// (remove) Token: 0x06000B29 RID: 2857 RVA: 0x0002A608 File Offset: 0x00028808
		public event ProjectFileRenamedEventHandler FileRenamedInProject;

		// Token: 0x04000347 RID: 839
		private static Counter ProjectOpenedCounter = InstrumentationService.CreateCounter("Project Opened", "Project Model", false, "Ide.Project.Open");

		// Token: 0x04000348 RID: 840
		private string[] buildActions;

		/// <summary>
		/// Description of the project.
		/// </summary>
		// Token: 0x04000349 RID: 841
		[ItemProperty("Description", DefaultValue = "")]
		private string description = "";

		// Token: 0x0400034A RID: 842
		private ProjectFileCollection files;

		// Token: 0x0400034B RID: 843
		[ProjectPathItemProperty("BaseIntermediateOutputPath")]
		private FilePath baseIntermediateOutputPath;

		// Token: 0x0400034C RID: 844
		private IconId stockIcon = "md-project";

		// Token: 0x0400034D RID: 845
		private UnresolvedFileCollection unresolvedDeps;
	}
}
