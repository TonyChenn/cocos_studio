using System;
using Mono.Addins;

namespace CocoStudio.Core.ModulesInterface
{
	// Token: 0x02000041 RID: 65
	[TypeExtensionPoint]
	public interface IAutoSaveManager
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000233 RID: 563
		// (set) Token: 0x06000234 RID: 564
		int SaveTimeInterval { get; set; }

		// Token: 0x06000235 RID: 565
		void Initialize();

		// Token: 0x06000236 RID: 566
		void StartAutoSave();

		// Token: 0x06000237 RID: 567
		void StopAutoSave();
	}
}
