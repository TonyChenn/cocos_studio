using System;
using Gtk;

namespace CocoStudio.ControlLib
{
	public class StudioMenuItem : MenuItem
	{
		public StudioMenuItem(string header) : base(header)
		{
		}

		public object Tag
		{
			get
			{
				return this.tag;
			}
			set
			{
				this.tag = value;
			}
		}

		private object tag;
	}
}
