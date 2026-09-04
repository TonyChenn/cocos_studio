using System;
using Modules.UI.ComTool.Model;

namespace Modules.Communal.Skeleton.ViewModel
{
	// Token: 0x0200000D RID: 13
	internal class SkeletonControlsViewFilter : IControlsViewFilter
	{
		// Token: 0x0600004D RID: 77 RVA: 0x0000335C File Offset: 0x0000155C
		public bool CanShowCategory(string categoryName)
		{
			bool result = false;
			if (categoryName == "Control_BaseObject" || categoryName == "ComToolPad" || categoryName == "Control_Container" || categoryName == "Control_Custom")
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000033A2 File Offset: 0x000015A2
		public bool CanShowItem(string itemTypeFullName)
		{
			return !(itemTypeFullName == "CocoStudio.Model.ViewModel.ArmatureNodeObject");
		}
	}
}
