using System.Collections.Generic;
using Gtk;

namespace MonoDevelop.DesignerSupport
{
	public interface IOutlinedDocument
	{
		Widget GetOutlineWidget();

		IEnumerable<Widget> GetToolbarWidgets();

		void ReleaseOutlineWidget();
	}
}
