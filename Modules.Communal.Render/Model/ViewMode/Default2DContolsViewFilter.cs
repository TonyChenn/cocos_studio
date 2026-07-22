using System;
using Modules.UI.ComTool.Model;

namespace Modules.Communal.Render.Model.ViewMode
{
	// Token: 0x0200002C RID: 44
	internal class Default2DContolsViewFilter : IControlsViewFilter
	{
		// Token: 0x060001AC RID: 428 RVA: 0x00009CFC File Offset: 0x00007EFC
		public bool CanShowCategory(string categoryName)
		{
			bool result = false;
			if (categoryName == "Control_BaseObject" || categoryName == "ComToolPad" || categoryName == "Control_Container" || categoryName == "Control_Custom")
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00009D50 File Offset: 0x00007F50
		public bool CanShowItem(string itemTypeFullName)
		{
			return true;
		}
	}
}
