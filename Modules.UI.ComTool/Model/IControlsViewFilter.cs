using System;

namespace Modules.UI.ComTool.Model
{
	// Token: 0x02000004 RID: 4
	public interface IControlsViewFilter
	{
		// Token: 0x06000004 RID: 4
		bool CanShowCategory(string categoryName);

		// Token: 0x06000005 RID: 5
		bool CanShowItem(string itemTypeFullName);
	}
}
