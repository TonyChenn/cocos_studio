using System;

namespace Modules.UI.ComTool.Model
{
	// Token: 0x02000005 RID: 5
	internal class DefaultControlsViewFilter : IControlsViewFilter
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002060 File Offset: 0x00000260
		internal static IControlsViewFilter DefaultFilterInstace
		{
			get
			{
				if (null == DefaultControlsViewFilter._defaultFilter)
				{
					DefaultControlsViewFilter._defaultFilter = new DefaultControlsViewFilter();
				}
				return DefaultControlsViewFilter._defaultFilter;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002094 File Offset: 0x00000294
		public bool CanShowCategory(string categoryName)
		{
			bool result = false;
			if (categoryName == "Control_BaseObject" || categoryName == "ComToolPad" || categoryName == "Control_Container" || categoryName == "Control_Custom")
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020E8 File Offset: 0x000002E8
		public bool CanShowItem(string itemTypeFullName)
		{
			return true;
		}

		// Token: 0x04000001 RID: 1
		private static IControlsViewFilter _defaultFilter;
	}
}
