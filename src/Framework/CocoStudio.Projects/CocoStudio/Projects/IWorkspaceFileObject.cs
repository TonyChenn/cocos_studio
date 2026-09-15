using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	public interface IWorkspaceFileObject : IFileItem, IWorkspaceObject, IExtendedDataItem, IFoldeItem, IDisposable
	{
		FilePath FileName { get; set; }
	}
}
