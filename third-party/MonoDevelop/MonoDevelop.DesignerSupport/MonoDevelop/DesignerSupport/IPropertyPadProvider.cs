namespace MonoDevelop.DesignerSupport
{
	public interface IPropertyPadProvider
	{
		object GetActiveComponent();

		object GetProvider();

		void OnEndEditing(object obj);

		void OnChanged(object obj);
	}
}
