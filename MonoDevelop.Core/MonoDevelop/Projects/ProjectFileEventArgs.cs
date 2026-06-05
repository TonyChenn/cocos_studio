using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x02000132 RID: 306
	public class ProjectFileEventArgs : EventArgsChain<ProjectFileEventInfo>
	{
		// Token: 0x06000B84 RID: 2948 RVA: 0x0002B481 File Offset: 0x00029681
		public ProjectFileEventArgs(Project project, ProjectFile file, string property)
		{
			base.Add(new ProjectFileEventInfo(project, file, property));
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0002B497 File Offset: 0x00029697
		public ProjectFileEventArgs(Project project, ProjectFile file)
		{
			base.Add(new ProjectFileEventInfo(project, file));
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0002B4AC File Offset: 0x000296AC
		public ProjectFileEventArgs()
		{
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000B87 RID: 2951 RVA: 0x0002B4B4 File Offset: 0x000296B4
		public Project CommonProject
		{
			get
			{
				if (this.rootDir.IsNull)
				{
					this.CalcRootDir(true, out this.rootDir, out this.singleDir);
				}
				return this.commonProject;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000B88 RID: 2952 RVA: 0x0002B4DC File Offset: 0x000296DC
		public FilePath CommonRootDirectory
		{
			get
			{
				if (this.rootDir.IsNull)
				{
					this.CalcRootDir(false, out this.rootDir, out this.singleDir);
				}
				return this.rootDir;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000B89 RID: 2953 RVA: 0x0002B504 File Offset: 0x00029704
		public bool SingleDirectory
		{
			get
			{
				if (this.rootDir.IsNull)
				{
					this.CalcRootDir(false, out this.rootDir, out this.singleDir);
				}
				return this.singleDir;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000B8A RID: 2954 RVA: 0x0002B52C File Offset: 0x0002972C
		public FilePath CommonVirtualRootDirectory
		{
			get
			{
				if (this.virtualRootDir.IsNull)
				{
					this.CalcRootDir(true, out this.virtualRootDir, out this.singleVirtualDir);
				}
				return this.virtualRootDir;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000B8B RID: 2955 RVA: 0x0002B554 File Offset: 0x00029754
		public bool SingleVirtualDirectory
		{
			get
			{
				if (this.virtualRootDir.IsNull)
				{
					this.CalcRootDir(true, out this.virtualRootDir, out this.singleVirtualDir);
				}
				return this.singleVirtualDir;
			}
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x0002B57C File Offset: 0x0002977C
		private void CalcRootDir(bool calcVirtual, out FilePath baseDir, out bool sameDir)
		{
			baseDir = FilePath.Null;
			sameDir = true;
			this.commonProject = null;
			foreach (ProjectFileEventInfo projectFileEventInfo in this)
			{
				FilePath parentDirectory;
				if (calcVirtual && projectFileEventInfo.ProjectFile.IsLink)
				{
					parentDirectory = projectFileEventInfo.Project.BaseDirectory.Combine(new FilePath[]
					{
						projectFileEventInfo.ProjectFile.ProjectVirtualPath
					}).ParentDirectory;
				}
				else
				{
					parentDirectory = projectFileEventInfo.ProjectFile.FilePath.ParentDirectory;
				}
				if (baseDir.IsNull)
				{
					this.commonProject = projectFileEventInfo.Project;
					baseDir = parentDirectory;
				}
				else
				{
					if (projectFileEventInfo.Project != this.commonProject)
					{
						this.commonProject = null;
					}
					if (!(parentDirectory == baseDir))
					{
						if (baseDir.IsChildPathOf(parentDirectory))
						{
							sameDir = false;
							baseDir = parentDirectory;
						}
						else
						{
							sameDir = false;
							while (!parentDirectory.IsChildPathOf(baseDir))
							{
								baseDir = baseDir.ParentDirectory;
							}
						}
					}
				}
			}
		}

		// Token: 0x04000373 RID: 883
		private FilePath rootDir;

		// Token: 0x04000374 RID: 884
		private FilePath virtualRootDir;

		// Token: 0x04000375 RID: 885
		private bool singleDir;

		// Token: 0x04000376 RID: 886
		private bool singleVirtualDir;

		// Token: 0x04000377 RID: 887
		private Project commonProject;
	}
}
