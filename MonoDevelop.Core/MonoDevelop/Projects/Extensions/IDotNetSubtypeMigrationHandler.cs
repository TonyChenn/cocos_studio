using System;
using System.Collections.Generic;
using MonoDevelop.Core;
using MonoDevelop.Projects.Formats.MSBuild;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x0200019D RID: 413
	public interface IDotNetSubtypeMigrationHandler
	{
		// Token: 0x06000FD7 RID: 4055
		IEnumerable<string> FilesToBackup(string filename);

		// Token: 0x06000FD8 RID: 4056
		Type Migrate(IProjectLoadProgressMonitor monitor, MSBuildProject project, string fileName, string language);

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000FD9 RID: 4057
		bool CanPromptForMigration { get; }

		// Token: 0x06000FDA RID: 4058
		MigrationType PromptForMigration(IProjectLoadProgressMonitor monitor, MSBuildProject project, string fileName, string language);
	}
}
