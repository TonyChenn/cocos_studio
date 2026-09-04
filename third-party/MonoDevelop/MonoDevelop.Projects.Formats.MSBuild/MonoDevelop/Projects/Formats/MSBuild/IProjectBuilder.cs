using System;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	public interface IProjectBuilder : IDisposable
	{
		void Refresh();

		void RefreshWithContent(string projectContent);

		MSBuildResult Run(ProjectConfigurationInfo[] configurations, ILogWriter logWriter, MSBuildVerbosity verbosity, string[] runTargets, string[] evaluateItems, string[] evaluateProperties);
	}
}
