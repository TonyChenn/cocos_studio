using System;
using System.Collections.Generic;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x0200016A RID: 362
	public interface IExecutableWorkspaceObject : IBuildTarget, IWorkspaceObject, IExtendedDataItem, IFolderItem, IDisposable
	{
		/// <summary>
		/// Gets the build targets that should be built before the project is executed.
		/// If the project itself is not included, it will not be built.
		/// </summary>
		// Token: 0x06000E54 RID: 3668
		IEnumerable<IBuildTarget> GetExecutionDependencies();
	}
}
