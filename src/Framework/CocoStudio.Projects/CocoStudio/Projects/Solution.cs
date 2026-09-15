using System;
using System.IO;
using CocoStudio.Basic;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	[DataInclude(typeof(SolutionFolder))]
	public class Solution : WorkspaceItem, IPublish
	{
		public static string DefaultPublishDirectoryName
		{
			get
			{
				return "res/";
			}
		}

		public static string DefaultPackageDirectoryName
		{
			get
			{
				return "package/";
			}
		}

		[ItemProperty("PropertyGroup/Name")]
		public override string Name { get; set; }

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

		public override FilePath ItemDirectory
		{
			get
			{
				return this.itemDirectory;
			}
		}

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

		public string PublishDirectory { get; private set; }

		public string PackageDirectory { get; private set; }

		public bool IsInitialized { get; private set; }

		public void Publish(IProgressMonitor monitor, PublishInfo info)
		{
			if (!Directory.Exists(info.PublishDirectory))
			{
				Directory.CreateDirectory(info.PublishDirectory);
			}
		}

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
			this.PublishDirectory = ((FilePath)this.Config.PublishDirectory).ToAbsolute(base.BaseDirectory);
		}

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
			this.PackageDirectory = ((FilePath)this.Config.PackageDirectory).ToAbsolute(base.BaseDirectory);
		}

		private void ConvertFlashSolution(IProgressMonitor monitor)
		{
			ResourceFolder parentFolder = ((ResourceGroup)this.RootFolder.Items[0]).RootFolder;
			if (Directory.Exists(this.itemDirectory))
			{
				this.LoadFlashResource(monitor, parentFolder, this.itemDirectory);
			}
		}

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

		public override CocosItem GetProjectContainingFile(FilePath fileName)
		{
			return this.RootFolder.GetProjectContainingFile(fileName);
		}

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

		protected internal override void OnSave(IProgressMonitor monitor)
		{
			this.Config.Save();
			this.UserData.Save();
			base.OnSave(monitor);
		}

		public override int GetHashCode()
		{
			return this.FileName.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return obj is Solution && this.FileName == ((Solution)obj).FileName;
		}

		public const string FileSuffix = ".ccs";

		[ItemProperty("PropertyGroup/Version")]
		private string version = Option.EditorVersion.ToString();

		[ItemProperty("PropertyGroup/Type")]
		private string type = "CocosStudio";

		private SolutionFolder rootFolder;

		private FilePath itemDirectory;

		private SolutionConfig _config;

		private UserData _userData;
	}
}
