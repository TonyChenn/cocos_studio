using System;
using Modules.UI.ComTool.Model;

namespace Modules.Communal.Render3D.Model
{
	internal class ThreeDControlsViewFilter : IControlsViewFilter
	{
		public bool CanShowCategory(string categoryName)
		{
			return categoryName == "Control_3DControl" || categoryName == "Control_3DCustom";
		}

		public bool CanShowItem(string itemTypeFullName)
		{
			return true;
		}
	}
}
