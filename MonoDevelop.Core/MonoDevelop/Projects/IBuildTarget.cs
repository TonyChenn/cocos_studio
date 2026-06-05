using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x0200010A RID: 266
	public interface IBuildTarget : IWorkspaceObject, IExtendedDataItem, IFolderItem, IDisposable
	{
		// Token: 0x06000995 RID: 2453
		BuildResult RunTarget(IProgressMonitor monitor, string target, ConfigurationSelector configuration);

		// Token: 0x06000996 RID: 2454
		bool SupportsTarget(string target);

		// Token: 0x06000997 RID: 2455
		[Obsolete("This method will be removed in future releases")]
		bool NeedsBuilding(ConfigurationSelector configuration);

		// Token: 0x06000998 RID: 2456
		[Obsolete("This method will be removed in future releases")]
		void SetNeedsBuilding(bool needsBuilding, ConfigurationSelector configuration);

		// Token: 0x06000999 RID: 2457
		void Execute(IProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration);

		// Token: 0x0600099A RID: 2458
		bool CanExecute(ExecutionContext context, ConfigurationSelector configuration);
	}
}
