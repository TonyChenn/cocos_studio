using MonoDevelop.Components.Commands;

namespace MonoDevelop.Debugger
{
	internal class ExpressionEvaluatorCommand : CommandHandler
	{
		protected override void Run()
		{
			DebuggingService.ShowExpressionEvaluator(null);
		}

		protected override void Update(CommandInfo info)
		{
			info.Visible = DebuggingService.IsDebuggingSupported;
			info.Enabled = DebuggingService.CurrentFrame != null;
		}
	}
}
