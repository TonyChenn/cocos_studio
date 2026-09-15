using System;
using System.Drawing;

namespace Modules.Communal.Preference.Model
{
	public class GuidesColorInfo
	{
		public string Name { get; set; }

		public Color RenderColor { get; set; }

		public GuidesColorInfo()
		{
		}

		public GuidesColorInfo(string name, Color renderColor)
		{
			this.Name = name;
			this.RenderColor = renderColor;
		}
	}
}
