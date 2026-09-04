using System;
using System.Collections.Generic;
using Mono.Addins;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x0200007E RID: 126
	[Extension(Type = typeof(IUserData))]
	public class TabsInfo : IUserData
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x0000D145 File Offset: 0x0000B345
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x0000D14D File Offset: 0x0000B34D
		[ItemProperty("OpenedDocuments")]
		public List<FilePathData> OpenedDocuments { get; set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x0000D156 File Offset: 0x0000B356
		// (set) Token: 0x060003C3 RID: 963 RVA: 0x0000D15E File Offset: 0x0000B35E
		[ItemProperty("ActiveDocument")]
		public FilePathData ActiveDocument { get; set; }

		// Token: 0x040000F9 RID: 249
		public const string TabsParamsKey = "TabsParamsKey";
	}
}
