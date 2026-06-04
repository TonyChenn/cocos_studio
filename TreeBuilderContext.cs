using System;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x02000021 RID: 33
	public class TreeBuilderContext : ITreeBuilderContext
	{
		// Token: 0x060000F1 RID: 241 RVA: 0x00004223 File Offset: 0x00002423
		internal TreeBuilderContext(ResourceTreeView pad)
		{
			this.pad = pad;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00004232 File Offset: 0x00002432
		public ITreeBuild GetTreeBuilder()
		{
			return this.pad.Builder;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000423F File Offset: 0x0000243F
		public ITreeBuild GetTreeBuilder(object dataObject)
		{
			return this.pad.Builder;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x0000424C File Offset: 0x0000244C
		public ResourceTreeView Tree
		{
			get
			{
				return this.pad;
			}
		}

		// Token: 0x0400003F RID: 63
		private ResourceTreeView pad;
	}
}
