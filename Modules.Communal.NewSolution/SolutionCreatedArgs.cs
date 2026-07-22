using System;

namespace Modules.Communal.NewSolution
{
	// Token: 0x02000012 RID: 18
	public class SolutionCreatedArgs : EventArgs
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600007B RID: 123 RVA: 0x0000474C File Offset: 0x0000294C
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00004754 File Offset: 0x00002954
		public string DefaultScenePath { get; private set; }

		// Token: 0x0600007D RID: 125 RVA: 0x0000475D File Offset: 0x0000295D
		public SolutionCreatedArgs(string defaultScenePath)
		{
			this.DefaultScenePath = defaultScenePath;
		}
	}
}
