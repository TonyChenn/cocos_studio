using System;
using System.Collections.Generic;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x0200010F RID: 271
	public interface IWorkspaceFileObject : IWorkspaceObject, IExtendedDataItem, IFolderItem, IDisposable, IFileItem
	{
		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000A02 RID: 2562
		FileFormat FileFormat { get; }

		// Token: 0x06000A03 RID: 2563
		void ConvertToFormat(FileFormat format, bool convertChildren);

		// Token: 0x06000A04 RID: 2564
		bool SupportsFormat(FileFormat format);

		// Token: 0x06000A05 RID: 2565
		List<FilePath> GetItemFiles(bool includeReferencedFiles);

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000A06 RID: 2566
		// (set) Token: 0x06000A07 RID: 2567
		FilePath FileName { get; set; }

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000A08 RID: 2568
		// (set) Token: 0x06000A09 RID: 2569
		bool NeedsReload { get; set; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000A0A RID: 2570
		bool ItemFilesChanged { get; }
	}
}
