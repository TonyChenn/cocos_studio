namespace MonoDevelop.DesignerSupport.Toolbox
{
	public interface ICustomFilteringToolboxConsumer : IToolboxConsumer
	{
		bool SupportsItem(ItemToolboxNode item);
	}
}
