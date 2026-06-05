using System;
using System.Collections.Generic;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects
{
	/// <summary>
	/// This class represent a file information in an IProject object.
	/// </summary>
	// Token: 0x0200012E RID: 302
	public class ProjectFile : ProjectItem, ICloneable, IFileItem, IDisposable
	{
		// Token: 0x06000B3C RID: 2876 RVA: 0x0002AA0C File Offset: 0x00028C0C
		public ProjectFile()
		{
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x0002AA48 File Offset: 0x00028C48
		public ProjectFile(string filename)
		{
			this.filename = FileService.GetFullPath(filename);
			this.subtype = Subtype.Code;
			this.buildaction = "Compile";
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0002AAB4 File Offset: 0x00028CB4
		public ProjectFile(string filename, string buildAction)
		{
			this.filename = FileService.GetFullPath(filename);
			this.subtype = Subtype.Code;
			this.buildaction = buildAction;
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000B3F RID: 2879 RVA: 0x0002AB19 File Offset: 0x00028D19
		// (set) Token: 0x06000B40 RID: 2880 RVA: 0x0002AB21 File Offset: 0x00028D21
		public Subtype Subtype
		{
			get
			{
				return this.subtype;
			}
			set
			{
				this.subtype = value;
				this.OnChanged("Subtype");
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000B41 RID: 2881 RVA: 0x0002AB35 File Offset: 0x00028D35
		// (set) Token: 0x06000B42 RID: 2882 RVA: 0x0002AB3D File Offset: 0x00028D3D
		public string Data
		{
			get
			{
				return this.data;
			}
			set
			{
				this.data = value;
				this.OnChanged("Data");
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000B43 RID: 2883 RVA: 0x0002AB51 File Offset: 0x00028D51
		// (set) Token: 0x06000B44 RID: 2884 RVA: 0x0002AB60 File Offset: 0x00028D60
		public string Name
		{
			get
			{
				return this.filename;
			}
			set
			{
				FilePath projectVirtualPath = this.ProjectVirtualPath;
				FilePath filePath = this.filename;
				this.filename = FileService.GetFullPath(value);
				if (this.HasChildren)
				{
					foreach (ProjectFile projectFile in this.DependentChildren)
					{
						projectFile.dependsOn = Path.GetFileName(this.FilePath);
					}
				}
				if (this.IsLink && this.Link.FileName == filePath.FileName)
				{
					this.link = Path.Combine(Path.GetDirectoryName(this.link), this.filename.FileName);
				}
				this.OnPathChanged(filePath, this.filename, projectVirtualPath, this.ProjectVirtualPath);
				if (this.project != null)
				{
					this.project.NotifyFileRenamedInProject(new ProjectFileRenamedEventArgs(this.project, this, filePath));
				}
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000B45 RID: 2885 RVA: 0x0002AC60 File Offset: 0x00028E60
		// (set) Token: 0x06000B46 RID: 2886 RVA: 0x0002AC68 File Offset: 0x00028E68
		public string BuildAction
		{
			get
			{
				return this.buildaction;
			}
			set
			{
				this.buildaction = (string.IsNullOrEmpty(value) ? "None" : value);
				this.OnChanged("BuildAction");
			}
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x0002AC8B File Offset: 0x00028E8B
		internal string GetResourceId(IResourceHandler resourceHandler)
		{
			if (string.IsNullOrEmpty(this.resourceId))
			{
				return resourceHandler.GetDefaultResourceId(this);
			}
			return this.resourceId;
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000B48 RID: 2888 RVA: 0x0002ACA8 File Offset: 0x00028EA8
		public FilePath FilePath
		{
			get
			{
				return this.filename;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000B49 RID: 2889 RVA: 0x0002ACB0 File Offset: 0x00028EB0
		FilePath IFileItem.FileName
		{
			get
			{
				return this.FilePath;
			}
		}

		/// <summary>
		/// Set to true if this ProjectFile was created at load time by
		/// a ProjectFile containing wildcards.  If true, this instance
		/// should not be saved to a csproj file.
		/// </summary>
		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000B4A RID: 2890 RVA: 0x0002ACB8 File Offset: 0x00028EB8
		// (set) Token: 0x06000B4B RID: 2891 RVA: 0x0002ACC0 File Offset: 0x00028EC0
		internal bool IsOriginatedFromWildcard { get; set; }

		/// <summary>
		/// The file should be treated as effectively having this relative path within the project. If the file is
		/// a link or outside the project root, this will not be the same as the physical file.
		/// </summary>
		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000B4C RID: 2892 RVA: 0x0002ACCC File Offset: 0x00028ECC
		public FilePath ProjectVirtualPath
		{
			get
			{
				if (!this.Link.IsNullOrEmpty)
				{
					return this.Link;
				}
				if (this.project != null)
				{
					FilePath relativeChildPath = this.project.GetRelativeChildPath(this.FilePath);
					if (!relativeChildPath.ToString().StartsWith("..", StringComparison.Ordinal))
					{
						return relativeChildPath;
					}
				}
				return this.FilePath.FileName;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000B4D RID: 2893 RVA: 0x0002AD39 File Offset: 0x00028F39
		public Project Project
		{
			get
			{
				return this.project;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000B4E RID: 2894 RVA: 0x0002AD41 File Offset: 0x00028F41
		// (set) Token: 0x06000B4F RID: 2895 RVA: 0x0002AD49 File Offset: 0x00028F49
		public string ContentType
		{
			get
			{
				return this.contentType;
			}
			set
			{
				this.contentType = value;
				this.OnChanged("ContentType");
			}
		}

		/// <summary>
		/// Whether the file should be shown to the user.
		/// </summary>
		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000B50 RID: 2896 RVA: 0x0002AD5D File Offset: 0x00028F5D
		// (set) Token: 0x06000B51 RID: 2897 RVA: 0x0002AD65 File Offset: 0x00028F65
		public bool Visible
		{
			get
			{
				return this.visible;
			}
			set
			{
				if (this.visible != value)
				{
					this.visible = value;
					this.OnChanged("Visible");
				}
			}
		}

		/// <summary>
		/// The ID of a custom code generator.
		/// </summary>
		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000B52 RID: 2898 RVA: 0x0002AD82 File Offset: 0x00028F82
		// (set) Token: 0x06000B53 RID: 2899 RVA: 0x0002AD8A File Offset: 0x00028F8A
		public string Generator
		{
			get
			{
				return this.generator;
			}
			set
			{
				if (this.generator != value)
				{
					this.generator = value;
					this.OnChanged("Generator");
				}
			}
		}

		/// <summary>
		/// Overrides the namespace in which the custom code generator should generate code.
		/// </summary>
		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000B54 RID: 2900 RVA: 0x0002ADAC File Offset: 0x00028FAC
		// (set) Token: 0x06000B55 RID: 2901 RVA: 0x0002ADB4 File Offset: 0x00028FB4
		public string CustomToolNamespace
		{
			get
			{
				return this.customToolNamespace;
			}
			set
			{
				if (this.customToolNamespace != value)
				{
					this.customToolNamespace = value;
					this.OnChanged("CustomToolNamespace");
				}
			}
		}

		/// <summary>
		/// The file most recently generated by the custom tool. Relative to this file's parent directory.
		/// </summary>
		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000B56 RID: 2902 RVA: 0x0002ADD6 File Offset: 0x00028FD6
		// (set) Token: 0x06000B57 RID: 2903 RVA: 0x0002ADDE File Offset: 0x00028FDE
		public string LastGenOutput
		{
			get
			{
				return this.lastGenOutput;
			}
			set
			{
				if (this.lastGenOutput != value)
				{
					this.lastGenOutput = value;
					this.OnChanged("LastGenOutput");
				}
			}
		}

		/// <summary>
		/// If the file's real path is outside the project root, this value can be used to set its virtual path
		/// within the project root. Use ProjectVirtualPath to read the effective virtual path for any file.
		/// </summary>
		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000B58 RID: 2904 RVA: 0x0002AE00 File Offset: 0x00029000
		// (set) Token: 0x06000B59 RID: 2905 RVA: 0x0002AE10 File Offset: 0x00029010
		public FilePath Link
		{
			get
			{
				return this.link;
			}
			set
			{
				if (this.link != value)
				{
					if (value.IsAbsolute || value.ToString().StartsWith("..", StringComparison.Ordinal))
					{
						throw new ArgumentException("value");
					}
					string name = this.link;
					this.link = value;
					this.OnVirtualPathChanged(name, this.link);
					this.OnChanged("Link");
				}
			}
		}

		/// <summary>
		/// Whether the file is a link.
		/// </summary>
		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000B5A RID: 2906 RVA: 0x0002AE94 File Offset: 0x00029094
		public bool IsLink
		{
			get
			{
				return !this.Link.IsNullOrEmpty || (this.project != null && !this.FilePath.IsChildPathOf(this.project.BaseDirectory));
			}
		}

		/// <summary>
		/// Whether the file is outside the project base directory.
		/// </summary>
		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000B5B RID: 2907 RVA: 0x0002AEDC File Offset: 0x000290DC
		public bool IsExternalToProject
		{
			get
			{
				return !this.FilePath.IsChildPathOf(this.project.BaseDirectory);
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000B5C RID: 2908 RVA: 0x0002AF05 File Offset: 0x00029105
		// (set) Token: 0x06000B5D RID: 2909 RVA: 0x0002AF0D File Offset: 0x0002910D
		public FileCopyMode CopyToOutputDirectory
		{
			get
			{
				return this.copyToOutputDirectory;
			}
			set
			{
				if (this.copyToOutputDirectory != value)
				{
					this.copyToOutputDirectory = value;
					this.OnChanged("CopyToOutputDirectory");
				}
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000B5E RID: 2910 RVA: 0x0002AF2A File Offset: 0x0002912A
		// (set) Token: 0x06000B5F RID: 2911 RVA: 0x0002AF34 File Offset: 0x00029134
		public string DependsOn
		{
			get
			{
				return this.dependsOn;
			}
			set
			{
				if (this.dependsOn != value)
				{
					FilePath oldPath = (!string.IsNullOrEmpty(this.dependsOn)) ? this.FilePath.ParentDirectory.Combine(new string[]
					{
						Path.GetFileName(this.dependsOn)
					}) : FilePath.Empty;
					this.dependsOn = value;
					if (this.dependsOnFile != null)
					{
						this.dependsOnFile.dependentChildren.Remove(this);
						this.dependsOnFile = null;
					}
					if (this.project != null && value != null)
					{
						this.project.UpdateDependency(this, oldPath);
					}
					this.OnChanged("DependsOn");
				}
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000B60 RID: 2912 RVA: 0x0002AFDE File Offset: 0x000291DE
		// (set) Token: 0x06000B61 RID: 2913 RVA: 0x0002AFE6 File Offset: 0x000291E6
		public ProjectFile DependsOnFile
		{
			get
			{
				return this.dependsOnFile;
			}
			internal set
			{
				this.dependsOnFile = value;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000B62 RID: 2914 RVA: 0x0002AFEF File Offset: 0x000291EF
		public bool HasChildren
		{
			get
			{
				return this.dependentChildren != null && this.dependentChildren.Count > 0;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000B63 RID: 2915 RVA: 0x0002B009 File Offset: 0x00029209
		public IList<ProjectFile> DependentChildren
		{
			get
			{
				return this.dependentChildren ?? ((IList<ProjectFile>)new ProjectFile[0]);
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000B64 RID: 2916 RVA: 0x0002B020 File Offset: 0x00029220
		internal FilePath DependencyPath
		{
			get
			{
				return this.FilePath.ParentDirectory.Combine(new string[]
				{
					Path.GetFileName(this.DependsOn)
				});
			}
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0002B05C File Offset: 0x0002925C
		internal bool ResolveParent(ProjectFile potentialParentFile)
		{
			if (potentialParentFile.FilePath == this.DependencyPath)
			{
				this.dependsOnFile = potentialParentFile;
				if (this.dependsOnFile.dependentChildren == null)
				{
					this.dependsOnFile.dependentChildren = new List<ProjectFile>();
				}
				this.dependsOnFile.dependentChildren.Add(this);
				return true;
			}
			return false;
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0002B0B4 File Offset: 0x000292B4
		internal bool ResolveParent()
		{
			if (this.dependsOnFile != null || string.IsNullOrEmpty(this.dependsOn) || this.project == null)
			{
				return true;
			}
			FilePath dependencyPath = this.DependencyPath;
			if (dependencyPath == this.FilePath)
			{
				LoggingService.LogWarning("Cyclic dependency in project '{0}': file '{1}' depends on '{2}'", new object[]
				{
					(this.project == null) ? "(none)" : this.project.Name,
					this.FilePath,
					dependencyPath
				});
				return true;
			}
			this.dependsOnFile = this.project.Files.GetFile(dependencyPath);
			if (this.dependsOnFile != null)
			{
				if (this.dependsOnFile.dependentChildren == null)
				{
					this.dependsOnFile.dependentChildren = new List<ProjectFile>();
				}
				this.dependsOnFile.dependentChildren.Add(this);
				return true;
			}
			return false;
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000B67 RID: 2919 RVA: 0x0002B198 File Offset: 0x00029398
		// (set) Token: 0x06000B68 RID: 2920 RVA: 0x0002B1F0 File Offset: 0x000293F0
		public string ResourceId
		{
			get
			{
				if (this.BuildAction == "EmbeddedResource" && string.IsNullOrEmpty(this.resourceId) && this.project is DotNetProject)
				{
					return ((DotNetProject)this.project).ResourceHandler.GetDefaultResourceId(this);
				}
				return this.resourceId;
			}
			set
			{
				if (this.resourceId != value)
				{
					string b = this.ResourceId;
					this.resourceId = value;
					if (this.ResourceId != b)
					{
						this.OnChanged("ResourceId");
					}
				}
			}
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0002B232 File Offset: 0x00029432
		internal void SetProject(Project project)
		{
			this.project = project;
			if (project != null)
			{
				this.OnVirtualPathChanged(FilePath.Null, this.ProjectVirtualPath);
			}
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0002B24F File Offset: 0x0002944F
		public override string ToString()
		{
			return "[ProjectFile: FileName=" + this.filename + "]";
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0002B26C File Offset: 0x0002946C
		public object Clone()
		{
			ProjectFile projectFile = (ProjectFile)base.MemberwiseClone();
			projectFile.dependsOnFile = null;
			projectFile.dependentChildren = null;
			projectFile.project = null;
			projectFile.VirtualPathChanged = null;
			projectFile.PathChanged = null;
			return projectFile;
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0002B2A9 File Offset: 0x000294A9
		public virtual void Dispose()
		{
		}

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06000B6D RID: 2925 RVA: 0x0002B2AC File Offset: 0x000294AC
		// (remove) Token: 0x06000B6E RID: 2926 RVA: 0x0002B2E4 File Offset: 0x000294E4
		internal event EventHandler<ProjectFileVirtualPathChangedEventArgs> VirtualPathChanged;

		// Token: 0x06000B6F RID: 2927 RVA: 0x0002B31C File Offset: 0x0002951C
		private void OnVirtualPathChanged(FilePath oldVirtualPath, FilePath newVirtualPath)
		{
			EventHandler<ProjectFileVirtualPathChangedEventArgs> virtualPathChanged = this.VirtualPathChanged;
			if (virtualPathChanged != null)
			{
				virtualPathChanged(this, new ProjectFileVirtualPathChangedEventArgs(this, oldVirtualPath, newVirtualPath));
			}
		}

		// Token: 0x1400003E RID: 62
		// (add) Token: 0x06000B70 RID: 2928 RVA: 0x0002B344 File Offset: 0x00029544
		// (remove) Token: 0x06000B71 RID: 2929 RVA: 0x0002B37C File Offset: 0x0002957C
		internal event EventHandler<ProjectFilePathChangedEventArgs> PathChanged;

		// Token: 0x06000B72 RID: 2930 RVA: 0x0002B3B4 File Offset: 0x000295B4
		private void OnPathChanged(FilePath oldPath, FilePath newPath, FilePath oldVirtualPath, FilePath newVirtualPath)
		{
			EventHandler<ProjectFilePathChangedEventArgs> pathChanged = this.PathChanged;
			if (pathChanged != null)
			{
				pathChanged(this, new ProjectFilePathChangedEventArgs(this, oldPath, newPath, oldVirtualPath, newVirtualPath));
			}
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0002B3DD File Offset: 0x000295DD
		protected virtual void OnChanged(string property)
		{
			if (this.project != null)
			{
				this.project.NotifyFilePropertyChangedInProject(this, property);
			}
		}

		// Token: 0x0400035B RID: 859
		[ItemProperty("subtype")]
		private Subtype subtype;

		// Token: 0x0400035C RID: 860
		[ItemProperty("data", DefaultValue = "")]
		private string data = "";

		// Token: 0x0400035D RID: 861
		[ItemProperty("buildaction")]
		private string buildaction = "None";

		// Token: 0x0400035E RID: 862
		[ItemProperty("resource_id", DefaultValue = "")]
		private string resourceId = string.Empty;

		// Token: 0x0400035F RID: 863
		private FilePath filename;

		// Token: 0x04000360 RID: 864
		private Project project;

		// Token: 0x04000361 RID: 865
		[ItemProperty("SubType")]
		private string contentType = string.Empty;

		// Token: 0x04000362 RID: 866
		[ItemProperty("Visible", DefaultValue = true)]
		private bool visible = true;

		// Token: 0x04000363 RID: 867
		[ItemProperty("Generator", DefaultValue = "")]
		private string generator;

		// Token: 0x04000364 RID: 868
		[ItemProperty("CustomToolNamespace", DefaultValue = "")]
		private string customToolNamespace;

		// Token: 0x04000365 RID: 869
		[ItemProperty("LastGenOutput", DefaultValue = "")]
		private string lastGenOutput;

		// Token: 0x04000366 RID: 870
		[RelativeProjectPathItemProperty("Link", DefaultValue = "")]
		private string link;

		// Token: 0x04000367 RID: 871
		[ItemProperty("copyToOutputDirectory", DefaultValue = FileCopyMode.None)]
		private FileCopyMode copyToOutputDirectory;

		// Token: 0x04000368 RID: 872
		private string dependsOn;

		// Token: 0x04000369 RID: 873
		private ProjectFile dependsOnFile;

		// Token: 0x0400036A RID: 874
		private List<ProjectFile> dependentChildren;
	}
}
