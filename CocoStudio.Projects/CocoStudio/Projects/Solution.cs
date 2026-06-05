using System;
using System.IO;
using CocoStudio.Basic;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x02000082 RID: 130
	[DataInclude(typeof(SolutionFolder))]
	public class Solution : WorkspaceItem, IPublish
	{
		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0000D5CD File Offset: 0x0000B7CD
		public static string DefaultPublishDirectoryName
		{
			get
			{
				return "res/";
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x0000D5D4 File Offset: 0x0000B7D4
		public static string DefaultPackageDirectoryName
		{
			get
			{
				return "package/";
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x0000D5DB File Offset: 0x0000B7DB
		// (set) Token: 0x060003FB RID: 1019 RVA: 0x0000D5E3 File Offset: 0x0000B7E3
		[ItemProperty("PropertyGroup/Name")]
		public override string Name { get; set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x0000D5EC File Offset: 0x0000B7EC
		// (set) Token: 0x060003FD RID: 1021 RVA: 0x0000D5F9 File Offset: 0x0000B7F9
		public Version Version
		{
			get
			{
				return new Version(this.version);
			}
			set
			{
				this.version = value.ToString();
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x0000D607 File Offset: 0x0000B807
		// (set) Token: 0x060003FF RID: 1023 RVA: 0x0000D62E File Offset: 0x0000B82E
		[ItemProperty("SolutionFolder")]
		public SolutionFolder RootFolder
		{
			get
			{
				if (this.rootFolder == null)
				{
					this.rootFolder = new SolutionFolder();
					this.rootFolder.ParentSolution = this;
				}
				return this.rootFolder;
			}
			internal set
			{
				this.rootFolder = value;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x0000D637 File Offset: 0x0000B837
		public override FilePath ItemDirectory
		{
			get
			{
				return this.itemDirectory;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x0000D640 File Offset: 0x0000B840
		// (set) Token: 0x06000402 RID: 1026 RVA: 0x0000D682 File Offset: 0x0000B882
		public SolutionConfig Config
		{
			get
			{
				if (this._config == null)
				{
					string filePath = SolutionConfig.GetFilePath(this);
					this._config = SolutionConfig.Load(filePath);
					if (this._config == null)
					{
						this._config = new SolutionConfig(filePath);
					}
				}
				return this._config;
			}
			internal set
			{
				this._config = value;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x0000D68C File Offset: 0x0000B88C
		// (set) Token: 0x06000404 RID: 1028 RVA: 0x0000D6D3 File Offset: 0x0000B8D3
		public UserData UserData
		{
			get
			{
				if (this._userData == null)
				{
					string filePath = UserData.GetFilePath(this.FileName);
					this._userData = UserData.Load(filePath);
					if (this._userData == null)
					{
						this._userData = new UserData(filePath);
					}
				}
				return this._userData;
			}
			internal set
			{
				this._userData = value;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x0000D6DC File Offset: 0x0000B8DC
		// (set) Token: 0x06000406 RID: 1030 RVA: 0x0000D6E4 File Offset: 0x0000B8E4
		public string PublishDirectory { get; private set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x0000D6ED File Offset: 0x0000B8ED
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x0000D6F5 File Offset: 0x0000B8F5
		public string PackageDirectory { get; private set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x0000D6FE File Offset: 0x0000B8FE
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x0000D706 File Offset: 0x0000B906
		public bool IsInitialized { get; private set; }

		// Token: 0x0600040C RID: 1036 RVA: 0x0000D732 File Offset: 0x0000B932
		public void Publish(IProgressMonitor monitor, PublishInfo info)
		{
			if (!Directory.Exists(info.PublishDirectory))
			{
				Directory.CreateDirectory(info.PublishDirectory);
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000D750 File Offset: 0x0000B950
		public void Initialize(IProgressMonitor monitor)
		{
			if (this.IsInitialized)
			{
				return;
			}
			if (this.type == "Flash")
			{
				this.ConvertFlashSolution(monitor);
				this.type = "CocosStudio";
			}
			bool flag = SolutionUpgraderManager.Upgrade(this);
			this.SetPublishDirectory();
			this.SetPackageDirectory();
			if (this.Version != Option.EditorVersion)
			{
				flag = true;
			}
			this.Version = Option.EditorVersion;
			if (flag)
			{
				base.Save(ProjectsService.Instance.DefaultMonitor);
			}
			foreach (SolutionEntityItem solutionEntityItem in this.RootFolder.Items)
			{
				IInitialize initialize = solutionEntityItem as IInitialize;
				if (initialize != null && initialize.IsAutoInitialize)
				{
					initialize.Initialize(monitor);
				}
			}
			this.IsInitialized = true;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000D830 File Offset: 0x0000BA30
		public void SetPublishDirectory()
		{
			if (string.IsNullOrEmpty(this.Config.PublishDirectory))
			{
				this.Config.PublishDirectory = Solution.DefaultPublishDirectoryName;
			}
			if (Path.IsPathRooted(this.Config.PublishDirectory))
			{
				this.PublishDirectory = this.Config.PublishDirectory;
				return;
			}
			this.PublishDirectory = this.Config.PublishDirectory.ToAbsolute(base.BaseDirectory);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000D8AC File Offset: 0x0000BAAC
		public void SetPackageDirectory()
		{
			if (string.IsNullOrEmpty(this.Config.PackageDirectory))
			{
				this.Config.PackageDirectory = Solution.DefaultPackageDirectoryName;
			}
			if (Path.IsPathRooted(this.Config.PackageDirectory))
			{
				this.PackageDirectory = this.Config.PackageDirectory;
				return;
			}
			this.PackageDirectory = this.Config.PackageDirectory.ToAbsolute(base.BaseDirectory);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000D928 File Offset: 0x0000BB28
		private void ConvertFlashSolution(IProgressMonitor monitor)
		{
			ResourceFolder parentFolder = ((ResourceGroup)this.RootFolder.Items[0]).RootFolder;
			if (Directory.Exists(this.itemDirectory))
			{
				this.LoadFlashResource(monitor, parentFolder, this.itemDirectory);
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000D978 File Offset: 0x0000BB78
		private void LoadFlashResource(IProgressMonitor monitor, ResourceFolder parentFolder, string parentDirectory)
		{
			string[] fileSystemEntries = Directory.GetFileSystemEntries(parentDirectory);
			foreach (string itemFileName in fileSystemEntries)
			{
				ResourceItem resourceItem = ProjectsService.Instance.ReadResourceItem(monitor, itemFileName);
				if (resourceItem != null)
				{
					parentFolder.Items.Add(resourceItem);
				}
				if (resourceItem is ResourceFolder)
				{
					this.LoadFlashResource(monitor, resourceItem as ResourceFolder, resourceItem.FullPath);
				}
			}
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000D9DC File Offset: 0x0000BBDC
		public override CocosItem GetProjectContainingFile(FilePath fileName)
		{
			return this.RootFolder.GetProjectContainingFile(fileName);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000D9EC File Offset: 0x0000BBEC
		public void SetLocation(FilePath baseDirectory, string name)
		{
			this.FileName = baseDirectory.Combine(new string[]
			{
				name + ".ccs"
			});
			this.Name = name;
			this.itemDirectory = baseDirectory.Combine(new string[]
			{
				"CocosStudio".ToLower()
			});
			if (!Directory.Exists(this.itemDirectory))
			{
				Directory.CreateDirectory(this.itemDirectory);
			}
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000DA68 File Offset: 0x0000BC68
		protected internal override void OnSave(IProgressMonitor monitor)
		{
			this.Config.Save();
			this.UserData.Save();
			base.OnSave(monitor);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000DA88 File Offset: 0x0000BC88
		public override int GetHashCode()
		{
			return this.FileName.GetHashCode();
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000DAA9 File Offset: 0x0000BCA9
		public override bool Equals(object obj)
		{
			return obj is Solution && this.FileName == ((Solution)obj).FileName;
		}

		// Token: 0x0400010F RID: 271
		public const string FileSuffix = ".ccs";

		// Token: 0x04000110 RID: 272
		[ItemProperty("PropertyGroup/Version")]
		private string version = Option.EditorVersion.ToString();

		// Token: 0x04000111 RID: 273
		[ItemProperty("PropertyGroup/Type")]
		private string type = "CocosStudio";

		// Token: 0x04000112 RID: 274
		private SolutionFolder rootFolder;

		// Token: 0x04000113 RID: 275
		private FilePath itemDirectory;

		// Token: 0x04000114 RID: 276
		private SolutionConfig _config;

		// Token: 0x04000115 RID: 277
		private UserData _userData;
	}
}
