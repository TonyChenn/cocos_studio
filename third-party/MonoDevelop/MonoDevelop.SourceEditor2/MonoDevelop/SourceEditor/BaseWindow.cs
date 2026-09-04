using Gdk;
using Gtk;

namespace MonoDevelop.SourceEditor
{
	public class BaseWindow : Gtk.Window
	{
		public BaseWindow()
			: base(Gtk.WindowType.Toplevel)
		{
			base.SkipPagerHint = true;
			base.SkipTaskbarHint = true;
			base.Decorated = false;
			base.BorderWidth = 2u;
			base.TypeHint = WindowTypeHint.PopupMenu;
			base.AllowShrink = false;
			base.AllowGrow = false;
		}

		protected override bool OnExposeEvent(EventExpose evnt)
		{
			GetSize(out var width, out var height);
			evnt.Window.DrawRectangle(base.Style.BaseGC(StateType.Normal), filled: true, 0, 0, width - 1, height - 1);
			evnt.Window.DrawRectangle(base.Style.MidGC(StateType.Normal), filled: false, 0, 0, width - 1, height - 1);
			Widget[] children = base.Children;
			foreach (Widget child in children)
			{
				PropagateExpose(child, evnt);
			}
			return false;
		}
	}
}
