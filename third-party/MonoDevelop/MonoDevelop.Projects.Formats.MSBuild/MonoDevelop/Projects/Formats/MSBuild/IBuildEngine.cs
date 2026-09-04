using System;
using System.Collections.Generic;
using System.Globalization;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	public interface IBuildEngine : IDisposable
	{
		void SetCulture(CultureInfo uiCulture);

		void SetGlobalProperties(IDictionary<string, string> properties);

		IProjectBuilder LoadProject(string projectFile);

		void UnloadProject(IProjectBuilder pb);

		void Ping();
	}
}
