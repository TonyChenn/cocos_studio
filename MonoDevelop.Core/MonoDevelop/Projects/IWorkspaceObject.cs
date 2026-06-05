using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x02000109 RID: 265
	public interface IWorkspaceObject : IExtendedDataItem, IFolderItem, IDisposable
	{
		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x0600098F RID: 2447
		// (set) Token: 0x06000990 RID: 2448
		string Name { get; set; }

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000991 RID: 2449
		FilePath ItemDirectory { get; }

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000992 RID: 2450
		// (set) Token: 0x06000993 RID: 2451
		FilePath BaseDirectory { get; set; }

		// Token: 0x06000994 RID: 2452
		void Save(IProgressMonitor monitor);
	}
}
