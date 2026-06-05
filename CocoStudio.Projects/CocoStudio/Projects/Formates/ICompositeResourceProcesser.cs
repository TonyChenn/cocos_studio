using System;
using System.Collections.Generic;
using Mono.Addins;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x02000012 RID: 18
	[TypeExtensionPoint]
	public interface ICompositeResourceProcesser
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000050 RID: 80
		// (set) Token: 0x06000051 RID: 81
		bool IsHiddenCompositeFile { get; set; }

		// Token: 0x06000052 RID: 82
		bool CanProcess(string filePath);

		// Token: 0x06000053 RID: 83
		List<string> GetFiles(string filePath);

		// Token: 0x06000054 RID: 84
		List<string> GetPretreatmentTypes();

		// Token: 0x06000055 RID: 85
		List<string> GetAfterTypes();

		// Token: 0x06000056 RID: 86
		List<string> GetFilterTypes();
	}
}
