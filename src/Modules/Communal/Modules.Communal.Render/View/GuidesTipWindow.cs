using System;
using Cairo;
using Gdk;
using Gtk;
using MonoDevelop.Components;

namespace Modules.Communal.Render.View
{
	internal class GuidesTipWindow : PopoverWindow
	{
		public GuidesTipWindow()
		{
			base.Theme.SetFlatColor(new Cairo.Color(0.0784313725490196, 0.0784313725490196, 0.0784313725490196));
			base.Theme.BorderColor = new Cairo.Color(0.3215686274509804, 0.3215686274509804, 0.3215686274509804);
			base.ShowArrow = true;
			this.Initialize();
		}

		private void Initialize()
		{
			Alignment alignment = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.label = new Label();
			alignment.Add(this.label);
			base.ContentBox.Add(alignment);
			alignment.ShowAll();
		}

		public string Text
		{
			get
			{
				return this.label.Text;
			}
			set
			{
				this.label.Text = value;
			}
		}

		public void MoveWindow(Widget parent, Gdk.Window window, double screenX, double screenY)
		{
			int num;
			int num2;
			window.GetOrigin(out num, out num2);
			Gdk.Point point = new Gdk.Point(num + (int)screenX, num2 + (int)screenY);
			base.Move(point.X + 10, point.Y + 10);
		}

		private Label label;
	}
}
