using MonoDevelop.Components.PropertyGrid;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.DesignerSupport
{
	public interface IPropertyPadCustomizer
	{
		void Customize(IPadWindow padWindow, PropertyGrid propertyGrid);
	}
}
