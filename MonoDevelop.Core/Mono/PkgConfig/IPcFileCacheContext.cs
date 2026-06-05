using System;

namespace Mono.PkgConfig
{
	// Token: 0x020000A0 RID: 160
	internal interface IPcFileCacheContext<TP> where TP : PackageInfo, new()
	{
		// Token: 0x06000580 RID: 1408
		void StoreCustomData(PcFile pcfile, TP pkg);

		// Token: 0x06000581 RID: 1409
		bool IsCustomDataComplete(string pcfile, TP pkg);

		// Token: 0x06000582 RID: 1410
		void ReportError(string message, Exception ex);
	}
}
