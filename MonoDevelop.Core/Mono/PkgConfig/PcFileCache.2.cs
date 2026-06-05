using System;

namespace Mono.PkgConfig
{
	// Token: 0x020000B8 RID: 184
	internal abstract class PcFileCache : PcFileCache<PackageInfo>
	{
		// Token: 0x0600064A RID: 1610 RVA: 0x00018635 File Offset: 0x00016835
		public PcFileCache(IPcFileCacheContext ctx) : base(ctx)
		{
		}
	}
}
