using System;

namespace Modules.UI.ComTool.Model
{
	public interface IControlsViewFilter
	{
		bool CanShowCategory(string categoryName);

		bool CanShowItem(string itemTypeFullName);
	}
}
