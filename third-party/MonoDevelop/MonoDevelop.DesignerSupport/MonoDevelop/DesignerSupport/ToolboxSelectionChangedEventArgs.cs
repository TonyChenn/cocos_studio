using MonoDevelop.DesignerSupport.Toolbox;

namespace MonoDevelop.DesignerSupport
{
	public class ToolboxSelectionChangedEventArgs
	{
		private ItemToolboxNode item;

		public ItemToolboxNode Item => item;

		public ToolboxSelectionChangedEventArgs(ItemToolboxNode item)
		{
			this.item = item;
		}
	}
}
