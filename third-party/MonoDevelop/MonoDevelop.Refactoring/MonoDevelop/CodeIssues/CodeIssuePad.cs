using Gtk;
using MonoDevelop.Ide.Gui;
using Xwt;

namespace MonoDevelop.CodeIssues
{
	public class CodeIssuePad : AbstractPadContent
	{
		private CodeIssuePadControl issueControl;

		public override Gtk.Widget Control
		{
			get
			{
				if (issueControl == null)
				{
					issueControl = new CodeIssuePadControl();
				}
				return (Gtk.Widget)Toolkit.CurrentEngine.GetNativeWidget(issueControl);
			}
		}
	}
}
