using System.Collections.Generic;
using Gtk;
using MonoDevelop.Components.Docking;
using MonoDevelop.Components.PropertyGrid;

namespace MonoDevelop.DesignerSupport
{
	internal class DockToolbarProvider : PropertyGrid.IToolbarProvider
	{
		private DockItemToolbar tb;

		private List<Widget> buttons = new List<Widget>();

		private bool visible = true;

		public Widget[] Children => buttons.ToArray();

		public bool Visible
		{
			get
			{
				return visible;
			}
			set
			{
				visible = value;
				if (tb != null)
				{
					tb.Visible = value;
				}
			}
		}

		public void Attach(DockItemToolbar tb)
		{
			if (this.tb == tb)
			{
				return;
			}
			this.tb = tb;
			if (tb == null)
			{
				return;
			}
			tb.Visible = visible;
			Widget[] children = tb.Children;
			foreach (Widget widget in children)
			{
				tb.Remove(widget);
			}
			foreach (Widget button in buttons)
			{
				tb.Add(button);
			}
		}

		public void Insert(Widget w, int pos)
		{
			if (tb != null)
			{
				tb.Insert(w, pos);
			}
			if (pos == -1)
			{
				buttons.Add(w);
			}
			else
			{
				buttons.Insert(pos, w);
			}
		}

		public void ShowAll()
		{
			if (tb != null)
			{
				tb.ShowAll();
				return;
			}
			foreach (Widget button in buttons)
			{
				button.Show();
			}
		}
	}
}
