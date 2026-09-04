using System;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x0200000C RID: 12
	public interface ITreeBuilderContext
	{
		// Token: 0x06000047 RID: 71
		ITreeBuild GetTreeBuilder();

		// Token: 0x06000048 RID: 72
		ITreeBuild GetTreeBuilder(object dataObject);

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000049 RID: 73
		ResourceTreeView Tree { get; }
	}
}
