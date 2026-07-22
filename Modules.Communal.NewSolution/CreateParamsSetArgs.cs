using System;
using Modules.Communal.CocosAdapter;

namespace Modules.Communal.NewSolution
{
	// Token: 0x02000017 RID: 23
	public class CreateParamsSetArgs : EventArgs
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000AA RID: 170 RVA: 0x000075BF File Offset: 0x000057BF
		// (set) Token: 0x060000AB RID: 171 RVA: 0x000075C7 File Offset: 0x000057C7
		public CreateParams Params { get; private set; }

		// Token: 0x060000AC RID: 172 RVA: 0x000075D0 File Offset: 0x000057D0
		public CreateParamsSetArgs(CreateParams prms)
		{
			this.Params = prms;
		}
	}
}
