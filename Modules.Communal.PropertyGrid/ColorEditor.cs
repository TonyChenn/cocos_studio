using System;
using System.Drawing;
using Gtk;
using Gtk.Controls;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x02000010 RID: 16
	public class ColorEditor : BaseEditor
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00002EE8 File Offset: 0x000010E8
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002EFC File Offset: 0x000010FC
		protected override Widget OnCreateWidget()
		{
			this.widget = new ColorEx();
			base.SetControl();
			this.widget.ColorChanged += this.widget_ColorChanged;
			return this.widget;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002F3E File Offset: 0x0000113E
		private void widget_ColorChanged(object sender, ColorExEvent e)
		{
			base.UpdatePropertyValue(e.Color, null);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002F70 File Offset: 0x00001170
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

		// Token: 0x04000016 RID: 22
		private ColorEx widget;
	}
}
