using Gdk;
using Gtk;
using MonoDevelop.Components;
using MonoDevelop.Ide;
using Xwt;

namespace MonoDevelop.SourceEditor
{
	internal class PinWindow : BaseWindow
	{
		private Xwt.ImageView icon;

		public PinWindow(Gtk.Window parent)
		{
			base.Events |= EventMask.ButtonPressMask;
			base.TransientFor = parent;
			base.DestroyWithParent = true;
			icon = new Xwt.ImageView();
			Add(icon.ToGtkWidget());
			base.AcceptFocus = false;
		}

		public void SetPinned(bool pinned)
		{
			if (pinned)
			{
				icon.Image = ImageService.GetIcon("md-pin-down", Gtk.IconSize.Menu);
			}
			else
			{
				icon.Image = ImageService.GetIcon("md-pin-up", Gtk.IconSize.Menu);
			}
		}
	}
}
