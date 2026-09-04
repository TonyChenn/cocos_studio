using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Default implementation of ISolutionSnapshot.
	/// </summary>
	// Token: 0x02000072 RID: 114
	public class DefaultSolutionSnapshot : ISolutionSnapshot
	{
		/// <summary>
		/// Creates a new DefaultSolutionSnapshot with the specified projects.
		/// </summary>
		// Token: 0x060003A0 RID: 928 RVA: 0x00008C14 File Offset: 0x00007C14
		public DefaultSolutionSnapshot(IEnumerable<IProjectContent> projects)
		{
			foreach (IProjectContent projectContent in projects)
			{
				if (projectContent.ProjectFileName != null)
				{
					this.projectDictionary.Add(projectContent.ProjectFileName, projectContent);
				}
			}
		}

		/// <summary>
		/// Creates a new DefaultSolutionSnapshot that does not support <see cref="T:ICSharpCode.NRefactory.TypeSystem.ProjectReference" />s.
		/// </summary>
		// Token: 0x060003A1 RID: 929 RVA: 0x00008C90 File Offset: 0x00007C90
		public DefaultSolutionSnapshot()
		{
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00008CB4 File Offset: 0x00007CB4
		public IProjectContent GetProjectContent(string projectFileName)
		{
			IProjectContent result;
			lock (this.projectDictionary)
			{
				IProjectContent projectContent;
				if (this.projectDictionary.TryGetValue(projectFileName, out projectContent))
				{
					result = projectContent;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00008D11 File Offset: 0x00007D11
		public ICompilation GetCompilation(IProjectContent project)
		{
			if (project == null)
			{
				throw new ArgumentNullException("project");
			}
			return this.dictionary.GetOrAdd(project, (IProjectContent p) => p.CreateCompilation(this));
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00008D3C File Offset: 0x00007D3C
		public void AddCompilation(IProjectContent project, ICompilation compilation)
		{
			if (project == null)
			{
				throw new ArgumentNullException("project");
			}
			if (compilation == null)
			{
				throw new ArgumentNullException("compilation");
			}
			if (!this.dictionary.TryAdd(project, compilation))
			{
				throw new InvalidOperationException();
			}
			if (project.ProjectFileName != null)
			{
				lock (this.projectDictionary)
				{
					this.projectDictionary.Add(project.ProjectFileName, project);
				}
			}
		}

		// Token: 0x040000E2 RID: 226
		private readonly Dictionary<string, IProjectContent> projectDictionary = new Dictionary<string, IProjectContent>(Platform.FileNameComparer);

		// Token: 0x040000E3 RID: 227
		private ConcurrentDictionary<IProjectContent, ICompilation> dictionary = new ConcurrentDictionary<IProjectContent, ICompilation>();
	}
}
