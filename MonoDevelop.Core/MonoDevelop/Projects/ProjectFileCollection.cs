using System;
using System.Collections.Generic;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x02000140 RID: 320
	[Serializable]
	public class ProjectFileCollection : ProjectItemCollection<ProjectFile>
	{
		// Token: 0x06000C02 RID: 3074 RVA: 0x0002CCA8 File Offset: 0x0002AEA8
		public ProjectFileCollection()
		{
			this.files = new Dictionary<FilePath, ProjectFile>();
			this.root = new ProjectFileNode();
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x0002CCC8 File Offset: 0x0002AEC8
		private void ProjectVirtualPathChanged(object sender, ProjectFileVirtualPathChangedEventArgs e)
		{
			ProjectFileNode projectFileNode;
			if (e.OldVirtualPath.IsNotNull)
			{
				projectFileNode = this.root.Find(e.OldVirtualPath, false);
				if (projectFileNode != null)
				{
					projectFileNode.Parent.Children.Remove(projectFileNode.FileName);
					this.PruneEmptyParents(projectFileNode.Parent);
				}
			}
			projectFileNode = this.root.Find(e.NewVirtualPath, true);
			projectFileNode.ProjectFile = e.ProjectFile;
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x0002CD47 File Offset: 0x0002AF47
		private void FilePathChanged(object sender, ProjectFilePathChangedEventArgs e)
		{
			this.ProjectVirtualPathChanged(sender, e);
			this.files.Remove(e.OldPath);
			this.files[e.NewPath] = e.ProjectFile;
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x0002CD7C File Offset: 0x0002AF7C
		private void AddProjectFile(ProjectFile item)
		{
			item.VirtualPathChanged += this.ProjectVirtualPathChanged;
			item.PathChanged += this.FilePathChanged;
			if (item.Project != null)
			{
				ProjectFileNode projectFileNode = this.root.Find(item.ProjectVirtualPath, true);
				projectFileNode.ProjectFile = item;
			}
			this.files[item.FilePath] = item;
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x0002CDE8 File Offset: 0x0002AFE8
		private void PruneEmptyParents(ProjectFileNode node)
		{
			if (node.Children.Count > 0 || node.ProjectFile != null || node.Parent == null)
			{
				return;
			}
			node.Parent.Children.Remove(node.FileName);
			this.PruneEmptyParents(node.Parent);
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x0002CE38 File Offset: 0x0002B038
		private void RemoveProjectFile(ProjectFile item)
		{
			ProjectFileNode projectFileNode = this.root.Find(item.ProjectVirtualPath, false);
			if (projectFileNode != null)
			{
				projectFileNode.Parent.Children.Remove(projectFileNode.FileName);
				this.PruneEmptyParents(projectFileNode.Parent);
			}
			this.files.Remove(item.FilePath);
			item.VirtualPathChanged -= this.ProjectVirtualPathChanged;
			item.PathChanged -= this.FilePathChanged;
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x0002CEB9 File Offset: 0x0002B0B9
		protected override void OnItemAdded(ProjectFile item)
		{
			this.AddProjectFile(item);
			base.OnItemAdded(item);
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x0002CEC9 File Offset: 0x0002B0C9
		protected override void OnItemRemoved(ProjectFile item)
		{
			this.RemoveProjectFile(item);
			base.OnItemRemoved(item);
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x0002CED9 File Offset: 0x0002B0D9
		protected override void AddItem(ProjectFile item)
		{
			this.AddProjectFile(item);
			base.AddItem(item);
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x0002CEE9 File Offset: 0x0002B0E9
		protected override void RemoveItem(ProjectFile item)
		{
			this.RemoveProjectFile(item);
			base.RemoveItem(item);
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x0002CEFC File Offset: 0x0002B0FC
		public ProjectFile GetFile(FilePath path)
		{
			if (path.IsNull)
			{
				return null;
			}
			ProjectFile result;
			if (this.files.TryGetValue(path.FullPath, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x0002CF30 File Offset: 0x0002B130
		public ProjectFile GetFileWithVirtualPath(string virtualPath)
		{
			if (string.IsNullOrEmpty(virtualPath))
			{
				return null;
			}
			ProjectFileNode projectFileNode = this.root.Find(virtualPath, false);
			if (projectFileNode != null && projectFileNode.ProjectFile != null)
			{
				return projectFileNode.ProjectFile;
			}
			return null;
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x0002D138 File Offset: 0x0002B338
		public IEnumerable<ProjectFile> GetFilesInVirtualPath(string virtualPath)
		{
			if (!string.IsNullOrEmpty(virtualPath))
			{
				ProjectFileNode node = this.root.Find(virtualPath, false);
				if (node != null)
				{
					foreach (ProjectFile pf in node.EnumerateProjectFiles(true))
					{
						yield return pf;
					}
				}
			}
			yield break;
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x0002D15C File Offset: 0x0002B35C
		public ProjectFile[] GetFilesInPath(FilePath path)
		{
			List<ProjectFile> list = new List<ProjectFile>();
			foreach (ProjectFile projectFile in base.Items)
			{
				if (projectFile.FilePath.IsChildPathOf(path))
				{
					list.Add(projectFile);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x0002D1C8 File Offset: 0x0002B3C8
		public void Remove(string fileName)
		{
			fileName = FileService.GetFullPath(fileName);
			for (int i = 0; i < base.Count; i++)
			{
				if (base.Items[i].Name == fileName)
				{
					base.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x0400039A RID: 922
		private Dictionary<FilePath, ProjectFile> files;

		// Token: 0x0400039B RID: 923
		private ProjectFileNode root;
	}
}
