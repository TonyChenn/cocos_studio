using System;

namespace Modules.Communal.NewSolution
{
	// Token: 0x02000020 RID: 32
	public class RadioItemArgs : EventArgs
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000083B6 File Offset: 0x000065B6
		// (set) Token: 0x060000FD RID: 253 RVA: 0x000083BE File Offset: 0x000065BE
		public IRadioItem RadioItem { get; private set; }

		// Token: 0x060000FE RID: 254 RVA: 0x000083C7 File Offset: 0x000065C7
		public RadioItemArgs(IRadioItem item)
		{
			this.RadioItem = item;
		}
	}
}
