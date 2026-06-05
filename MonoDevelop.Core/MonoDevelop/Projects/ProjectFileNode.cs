using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MonoDevelop.Projects
{
	// Token: 0x0200013C RID: 316
	internal class ProjectFileNode
	{
		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000BD9 RID: 3033 RVA: 0x0002C1AD File Offset: 0x0002A3AD
		// (set) Token: 0x06000BDA RID: 3034 RVA: 0x0002C1B5 File Offset: 0x0002A3B5
		public SortedList<string, ProjectFileNode> Children { get; private set; }

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000BDB RID: 3035 RVA: 0x0002C1BE File Offset: 0x0002A3BE
		// (set) Token: 0x06000BDC RID: 3036 RVA: 0x0002C1C6 File Offset: 0x0002A3C6
		public ProjectFileNode Parent { get; private set; }

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000BDD RID: 3037 RVA: 0x0002C1CF File Offset: 0x0002A3CF
		// (set) Token: 0x06000BDE RID: 3038 RVA: 0x0002C1D7 File Offset: 0x0002A3D7
		public ProjectFile ProjectFile { get; set; }

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000BDF RID: 3039 RVA: 0x0002C1E0 File Offset: 0x0002A3E0
		// (set) Token: 0x06000BE0 RID: 3040 RVA: 0x0002C1E8 File Offset: 0x0002A3E8
		public string FileName { get; set; }

		// Token: 0x06000BE1 RID: 3041 RVA: 0x0002C1F1 File Offset: 0x0002A3F1
		public ProjectFileNode() : this(null, string.Empty)
		{
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x0002C200 File Offset: 0x0002A400
		public ProjectFileNode(ProjectFileNode parent, ProjectFile file)
		{
			this.Children = new SortedList<string, ProjectFileNode>();
			this.FileName = file.ProjectVirtualPath.FileName;
			this.ProjectFile = file;
			this.Parent = parent;
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x0002C240 File Offset: 0x0002A440
		public ProjectFileNode(ProjectFileNode parent, string fileName)
		{
			this.Children = new SortedList<string, ProjectFileNode>();
			this.FileName = fileName;
			this.ProjectFile = null;
			this.Parent = parent;
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x0002C268 File Offset: 0x0002A468
		private ProjectFileNode Find(string[] path, int pathIndex, bool create)
		{
			ProjectFileNode projectFileNode;
			if (this.Children.TryGetValue(path[pathIndex], out projectFileNode))
			{
				if (pathIndex + 1 == path.Length)
				{
					return projectFileNode;
				}
				return projectFileNode.Find(path, pathIndex + 1, create);
			}
			else
			{
				if (!create)
				{
					return null;
				}
				projectFileNode = new ProjectFileNode(this, path[pathIndex]);
				this.Children.Add(projectFileNode.FileName, projectFileNode);
				if (pathIndex + 1 == path.Length)
				{
					return projectFileNode;
				}
				return projectFileNode.Find(path, pathIndex + 1, create);
			}
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x0002C2D4 File Offset: 0x0002A4D4
		public ProjectFileNode Find(string vpath, bool create)
		{
			if (string.IsNullOrEmpty(vpath))
			{
				return this;
			}
			string[] path = vpath.Split(new char[]
			{
				Path.DirectorySeparatorChar
			}, StringSplitOptions.None);
			return this.Find(path, 0, create);
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x0002C5C0 File Offset: 0x0002A7C0
		public IEnumerable<ProjectFile> EnumerateProjectFiles(bool recursive)
		{
			foreach (ProjectFileNode child in from x in this.Children
			select x.Value)
			{
				if (child.ProjectFile != null)
				{
					yield return child.ProjectFile;
				}
				if (recursive)
				{
					foreach (ProjectFile pf in child.EnumerateProjectFiles(recursive))
					{
						yield return pf;
					}
				}
			}
			yield break;
		}
	}
}
