using System;
using Modules.UI.ComTool.Model;

namespace Modules.Communal.Render.Model.ViewMode
{
	internal class Default2DContolsViewFilter : IControlsViewFilter
	{
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
	}
}
