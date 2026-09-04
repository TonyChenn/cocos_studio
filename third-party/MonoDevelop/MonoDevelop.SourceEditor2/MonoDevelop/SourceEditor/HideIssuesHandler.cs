using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;

namespace MonoDevelop.SourceEditor
{
	internal class HideIssuesHandler : CommandHandler
	{
		protected override void Update(CommandInfo info)
		{
			base.Update(info);
			info.Text = (IdeApp.Preferences.DefaultHideMessageBubbles ? GettextCatalog.GetString("_Show Message Bubbles") : GettextCatalog.GetString("_Hide Message Bubbles"));
		}

		protected override void Run(object data)
		{
			IdeApp.Preferences.DefaultHideMessageBubbles = !IdeApp.Preferences.DefaultHideMessageBubbles;
		}
	}
}
