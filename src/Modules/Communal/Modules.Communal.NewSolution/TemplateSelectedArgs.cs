using System;

namespace Modules.Communal.NewSolution
{
	// Token: 0x02000014 RID: 20
	public class TemplateSelectedArgs : EventArgs
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00005348 File Offset: 0x00003548
		// (set) Token: 0x0600008C RID: 140 RVA: 0x00005350 File Offset: 0x00003550
		public ISolutionTemplate SolutionTemplate { get; private set; }

		// Token: 0x0600008D RID: 141 RVA: 0x00005359 File Offset: 0x00003559
		public TemplateSelectedArgs(ISolutionTemplate slnTemplate)
		{
			this.SolutionTemplate = slnTemplate;
		}
	}
}
