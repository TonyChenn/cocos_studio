using System;
using System.Collections.Generic;
using System.Linq;

namespace CocoStudio.Core.View
{
	public class PadCollection : List<Pad>
	{
		public PadCollection(Workbench workbench)
		{
			this.workbench = workbench;
		}

		public Pad PropertyPad
		{
			get
			{
				return this.GetPad("Modules.Communal.PropertyGrid.PropertyGridPad");
			}
		}

		public Pad OutputPad
		{
			get
			{
				return this.GetPad("Modules.Communal.Output.OutputPad");
			}
		}

		private Pad GetPad(string id)
		{
			return this.workbench.Pads.FirstOrDefault((Pad p) => p.Id == id);
		}

		private Workbench workbench;
	}
}
