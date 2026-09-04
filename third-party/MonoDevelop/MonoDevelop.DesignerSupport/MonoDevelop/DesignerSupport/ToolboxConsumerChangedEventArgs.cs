using MonoDevelop.DesignerSupport.Toolbox;

namespace MonoDevelop.DesignerSupport
{
	public class ToolboxConsumerChangedEventArgs
	{
		private IToolboxConsumer consumer;

		public IToolboxConsumer Consumer => consumer;

		public ToolboxConsumerChangedEventArgs(IToolboxConsumer consumer)
		{
			this.consumer = consumer;
		}
	}
}
