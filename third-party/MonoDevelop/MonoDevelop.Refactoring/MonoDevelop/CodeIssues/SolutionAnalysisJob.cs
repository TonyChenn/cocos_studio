using System.Collections.Generic;
using System.Linq;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace MonoDevelop.CodeIssues
{
	public class SolutionAnalysisJob : SimpleAnalysisJob
	{
		private class FilePathComparer : IEqualityComparer<ProjectFile>
		{
			public bool Equals(ProjectFile x, ProjectFile y)
			{
				if (x != y)
				{
					if (x != null && y != null)
					{
						return x.Name == y.Name;
					}
					return false;
				}
				return true;
			}

			public int GetHashCode(ProjectFile obj)
			{
				return obj?.Name.GetHashCode() ?? (-1);
			}
		}

		public SolutionAnalysisJob(Solution solution)
			: base(GetApplicableFiles(solution))
		{
		}

		private static IList<ProjectFile> GetApplicableFiles(Solution solution)
		{
			ConfigurationSelector activeConfiguration = IdeApp.Workspace.ActiveConfiguration;
			SolutionConfiguration configuration = solution.GetConfiguration(activeConfiguration);
			return (from f in solution.GetAllProjects().Where(configuration.BuildEnabledForItem).SelectMany((Project p) => p.Files)
				where f.BuildAction == "Compile"
				select f).Distinct(new FilePathComparer()).ToList();
		}
	}
}
