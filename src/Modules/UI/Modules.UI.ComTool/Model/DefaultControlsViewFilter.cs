using System;

namespace Modules.UI.ComTool.Model
{
	internal class DefaultControlsViewFilter : IControlsViewFilter
	{
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

		public bool CanShowCategory(string categoryName)
		{
			bool result = false;
			if (categoryName == "Control_BaseObject" || categoryName == "ComToolPad" || categoryName == "Control_Container" || categoryName == "Control_Custom")
			{
				result = true;
			}
			return result;
		}

		public bool CanShowItem(string itemTypeFullName)
		{
			return true;
		}

		private static IControlsViewFilter _defaultFilter;
	}
}
