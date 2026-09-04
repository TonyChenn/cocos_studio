using System;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	[Serializable]
	public class ProjectConfigurationInfo
	{
		public string ProjectFile;

		public string ProjectGuid;

		public string Configuration;

		public string Platform;
	}
}
