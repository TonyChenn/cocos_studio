using System;
using CocoStudio.Core;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x0200001D RID: 29
	public class PropertyGridPad : DefaultPadContent
	{
		// Token: 0x060000C6 RID: 198 RVA: 0x00004967 File Offset: 0x00002B67
		static PropertyGridPad()
		{
			PropertyManager.Initialize();
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004970 File Offset: 0x00002B70
		public PropertyGridPad() : base(new PropertyGridUC())
		{
		}
	}
}
