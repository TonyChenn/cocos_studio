using System;
using System.Collections.Generic;
using System.Linq;

namespace CocoStudio.Core.View
{
	// Token: 0x02000050 RID: 80
	public class PadCollection : List<Pad>
	{
		// Token: 0x06000323 RID: 803 RVA: 0x0000E561 File Offset: 0x0000C761
		public PadCollection(Workbench workbench)
		{
			this.workbench = workbench;
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000324 RID: 804 RVA: 0x0000E574 File Offset: 0x0000C774
		public Pad PropertyPad
		{
			get
			{
				return this.GetPad("Modules.Communal.PropertyGrid.PropertyGridPad");
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000325 RID: 805 RVA: 0x0000E594 File Offset: 0x0000C794
		public Pad OutputPad
		{
			get
			{
				return this.GetPad("Modules.Communal.Output.OutputPad");
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000E5E0 File Offset: 0x0000C7E0
		private Pad GetPad(string id)
		{
			return this.workbench.Pads.FirstOrDefault((Pad p) => p.Id == id);
		}

		// Token: 0x0400016B RID: 363
		private Workbench workbench;
	}
}
