using MonoDevelop.Components.Commands;

namespace MonoDevelop.DesignerSupport
{
	public class SelectItemsCommandHandler : CommandHandler
	{
		protected override void Run()
		{
			DesignerSupport.Service.ToolboxService.AddUserItems();
		}
	}
}
