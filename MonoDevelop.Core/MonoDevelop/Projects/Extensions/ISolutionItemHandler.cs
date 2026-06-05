using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects.Extensions
{
	/// <summary>
	/// An abstraction of some solution item operations that may be specific to the underlying file format.
	/// </summary>
	// Token: 0x02000166 RID: 358
	public interface ISolutionItemHandler : IDisposable
	{
		/// <summary>
		/// Executes a build target
		/// </summary>
		/// <returns>
		/// The result of the operation
		/// </returns>
		/// <param name="monitor">
		/// A progress monitor
		/// </param>
		/// <param name="target">
		/// Name of the target to execute
		/// </param>
		/// <param name="configuration">
		/// Selector to be used to get the target configuration
		/// </param>
		// Token: 0x06000E3F RID: 3647
		BuildResult RunTarget(IProgressMonitor monitor, string target, ConfigurationSelector configuration);

		/// <summary>
		/// Saves the solution item
		/// </summary>
		/// <param name="monitor">
		/// A progress monitor
		/// </param>
		// Token: 0x06000E40 RID: 3648
		void Save(IProgressMonitor monitor);

		/// <summary>
		/// Gets a value indicating whether the name of the solution item should be the same as the name of the file
		/// </summary>
		/// <value>
		/// <c>true</c> if the file name must be in sync with the solution item name; otherwise, <c>false</c>.
		/// </value>
		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000E41 RID: 3649
		bool SyncFileName { get; }

		/// <summary>
		/// Unique and immutable identifier of the solution item inside the solution
		/// </summary>
		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000E42 RID: 3650
		string ItemId { get; }

		/// <summary>
		/// Notifies that this solution item has been modified
		/// </summary>
		/// <param name="hint">
		/// Hint about which part of the solution item has been modified. This will typically be the property name.
		/// </param>
		// Token: 0x06000E43 RID: 3651
		void OnModified(string hint);

		/// <summary>
		/// Gets a service instance of a given type
		/// </summary>
		/// <returns>
		/// The service.
		/// </returns>
		/// <param name="t">
		/// Type of the service
		/// </param>
		/// <remarks>
		/// This method looks for an imlpementation of a service of the given type.
		/// </remarks>
		// Token: 0x06000E44 RID: 3652
		object GetService(Type t);
	}
}
