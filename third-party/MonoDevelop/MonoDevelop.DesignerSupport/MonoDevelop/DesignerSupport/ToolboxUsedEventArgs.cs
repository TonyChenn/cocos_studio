using MonoDevelop.DesignerSupport.Toolbox;

namespace MonoDevelop.DesignerSupport
{
	public class ToolboxUsedEventArgs
	{
		private ItemToolboxNode item;

		private IToolboxConsumer consumer;

		public ItemToolboxNode Item => item;

		public IToolboxConsumer Consumer => consumer;

		public ToolboxUsedEventArgs(IToolboxConsumer consumer, ItemToolboxNode item)
		{
			this.item = item;
			this.consumer = consumer;
		}
	}
}
