using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x02000066 RID: 102
	public interface IWorkspaceObject : IExtendedDataItem, IFoldeItem, IDisposable
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060002F9 RID: 761
		// (set) Token: 0x060002FA RID: 762
		string Name { get; set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060002FB RID: 763
		FilePath ItemDirectory { get; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060002FC RID: 764
		// (set) Token: 0x060002FD RID: 765
		FilePath BaseDirectory { get; set; }

		// Token: 0x060002FE RID: 766
		void Save(IProgressMonitor monitor);
	}
}
