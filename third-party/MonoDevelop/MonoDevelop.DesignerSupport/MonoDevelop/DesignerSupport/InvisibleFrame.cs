using Gtk;

namespace MonoDevelop.DesignerSupport
{
	internal class InvisibleFrame : Alignment
	{
		public InvisibleFrame()
			: base(0f, 0f, 1f, 1f)
		{
		}

		public Widget ReplaceChild(Widget widget)
		{
			Widget child = base.Child;
			if (child != null)
			{
				Remove(child);
			}
			Add(widget);
			return child;
		}
	}
}
