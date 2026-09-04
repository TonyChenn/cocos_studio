using System;
using Modules.UI.ComTool.Model;

namespace Modules.Communal.Render3D.Model
{
	// Token: 0x02000004 RID: 4
	internal class ThreeDControlsViewFilter : IControlsViewFilter
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002271 File Offset: 0x00000471
		public bool CanShowCategory(string categoryName)
		{
			return categoryName == "Control_3DControl" || categoryName == "Control_3DCustom";
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002290 File Offset: 0x00000490
		public bool CanShowItem(string itemTypeFullName)
		{
			return true;
		}
	}
}
