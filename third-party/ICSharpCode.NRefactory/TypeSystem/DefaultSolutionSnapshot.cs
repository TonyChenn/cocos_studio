using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Default implementation of ISolutionSnapshot.
	/// </summary>
	public class DefaultSolutionSnapshot : ISolutionSnapshot
	{
		/// <summary>
		/// Creates a new DefaultSolutionSnapshot with the specified projects.
		/// </summary>
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
		public DefaultSolutionSnapshot()
		{
		}

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

		public ICompilation GetCompilation(IProjectContent project)
		{
			if (project == null)
			{
				throw new ArgumentNullException("project");
			}
			return this.dictionary.GetOrAdd(project, (IProjectContent p) => p.CreateCompilation(this));
		}

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

		private readonly Dictionary<string, IProjectContent> projectDictionary = new Dictionary<string, IProjectContent>(Platform.FileNameComparer);

		private ConcurrentDictionary<IProjectContent, ICompilation> dictionary = new ConcurrentDictionary<IProjectContent, ICompilation>();
	}
}
