using Gtk;
using MonoDevelop.DesignerSupport.Toolbox;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.DesignerSupport
{
	public class ToolboxPad : AbstractPadContent
	{
		private MonoDevelop.DesignerSupport.Toolbox.Toolbox toolbox;

		public override Widget Control => toolbox;

		public override void Initialize(IPadWindow container)
		{
			base.Initialize(container);
			toolbox = new MonoDevelop.DesignerSupport.Toolbox.Toolbox(DesignerSupport.Service.ToolboxService, container);
		}
	}
}
