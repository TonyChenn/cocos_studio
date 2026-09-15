using System;
using CocoStudio.Core;

namespace Modules.Communal.PropertyGrid
{
	public class PropertyGridPad : DefaultPadContent
	{
		static PropertyGridPad()
		{
			PropertyManager.Initialize();
		}

		public PropertyGridPad() : base(new PropertyGridUC())
		{
		}
	}
}
