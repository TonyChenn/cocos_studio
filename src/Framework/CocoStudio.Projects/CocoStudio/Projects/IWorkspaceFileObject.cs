using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x02000067 RID: 103
	public interface IWorkspaceFileObject : IFileItem, IWorkspaceObject, IExtendedDataItem, IFoldeItem, IDisposable
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060002FF RID: 767
		// (set) Token: 0x06000300 RID: 768
		FilePath FileName { get; set; }
	}
}
