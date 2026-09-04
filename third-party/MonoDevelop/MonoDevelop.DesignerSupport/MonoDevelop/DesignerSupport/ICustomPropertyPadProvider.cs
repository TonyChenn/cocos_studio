using Gtk;

namespace MonoDevelop.DesignerSupport
{
	public interface ICustomPropertyPadProvider
	{
		Widget GetCustomPropertyWidget();

		void DisposeCustomPropertyWidget();
	}
}
