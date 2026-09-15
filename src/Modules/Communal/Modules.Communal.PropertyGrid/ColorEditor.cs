using System;
using System.Drawing;
using Gtk;
using Gtk.Controls;

namespace Modules.Communal.PropertyGrid
{
	public class ColorEditor : BaseEditor
	{
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		protected override Widget OnCreateWidget()
		{
			this.widget = new ColorEx();
			base.SetControl();
			this.widget.ColorChanged += this.widget_ColorChanged;
			return this.widget;
		}

		private void widget_ColorChanged(object sender, ColorExEvent e)
		{
			base.UpdatePropertyValue(e.Color, null);
		}

		protected override void OnSetControl()
		{
			Color color = (Color)base.PropertyItem.Values[0];
			if (PropertyItem.Objects.Count > 1)
			{
				Func<Color, Color, bool> func = (Color a, Color b) => a == b;
				this.widget.MultilShow = base.IsWhip<Color>(func);
			}
			else
			{
				this.widget.MultilShow = false;
			}
			this.widget.Color = color;
		}

		private ColorEx widget;
	}
}
