using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	public interface IWorkspaceObject : IExtendedDataItem, IFoldeItem, IDisposable
	{
		string Name { get; set; }

		FilePath ItemDirectory { get; }

		FilePath BaseDirectory { get; set; }

		void Save(IProgressMonitor monitor);
	}
}
