using MonoDevelop.Ide.Gui;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	public interface IToolboxCustomizer
	{
		void Customize(IPadWindow padWindow, IToolboxConfiguration config);
	}
}
