using System;
using System.Collections.Generic;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x0200012B RID: 299
	internal class UnresolvedFileCollection
	{
		// Token: 0x06000B31 RID: 2865 RVA: 0x0002A670 File Offset: 0x00028870
		public void Remove(ProjectFile file)
		{
			this.Remove(file, null);
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0002A680 File Offset: 0x00028880
		public void Remove(ProjectFile file, FilePath dependencyPath)
		{
			if (dependencyPath.IsNullOrEmpty)
			{
				if (string.IsNullOrEmpty(file.DependsOn))
				{
					return;
				}
				dependencyPath = file.DependencyPath;
			}
			object obj;
			if (this.unresolvedDeps.TryGetValue(dependencyPath, out obj))
			{
				if (obj is ProjectFile && (ProjectFile)obj == file)
				{
					this.unresolvedDeps.Remove(dependencyPath);
					return;
				}
				if (obj is List<ProjectFile>)
				{
					List<ProjectFile> list = (List<ProjectFile>)obj;
					list.Remove(file);
					if (list.Count == 1)
					{
						this.unresolvedDeps[dependencyPath] = list[0];
					}
				}
			}
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0002A710 File Offset: 0x00028910
		public void Add(ProjectFile file)
		{
			object obj;
			if (this.unresolvedDeps.TryGetValue(file.DependencyPath, out obj))
			{
				if (obj is ProjectFile)
				{
					if ((ProjectFile)obj != file)
					{
						List<ProjectFile> list = new List<ProjectFile>();
						list.Add((ProjectFile)obj);
						list.Add(file);
						this.unresolvedDeps[file.DependencyPath] = list;
						return;
					}
				}
				else if (obj is List<ProjectFile>)
				{
					List<ProjectFile> list2 = (List<ProjectFile>)obj;
					if (!list2.Contains(file))
					{
						list2.Add(file);
						return;
					}
				}
			}
			else
			{
				this.unresolvedDeps[file.DependencyPath] = file;
			}
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0002A990 File Offset: 0x00028B90
		public IEnumerable<ProjectFile> GetUnresolvedFilesForPath(FilePath filePath)
		{
			object depFile;
			if (this.unresolvedDeps.TryGetValue(filePath, out depFile))
			{
				if (depFile is ProjectFile)
				{
					yield return (ProjectFile)depFile;
				}
				else
				{
					foreach (ProjectFile f in ((List<ProjectFile>)depFile))
					{
						yield return f;
					}
				}
			}
			yield break;
		}

		// Token: 0x04000354 RID: 852
		private Dictionary<FilePath, object> unresolvedDeps = new Dictionary<FilePath, object>();
	}
}
