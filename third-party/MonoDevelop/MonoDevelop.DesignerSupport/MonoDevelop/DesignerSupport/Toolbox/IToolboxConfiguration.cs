namespace MonoDevelop.DesignerSupport.Toolbox
{
	public interface IToolboxConfiguration
	{
		bool AllowEditingComponents { get; set; }

		void SetCategoryPriority(string category, int priority);
	}
}
