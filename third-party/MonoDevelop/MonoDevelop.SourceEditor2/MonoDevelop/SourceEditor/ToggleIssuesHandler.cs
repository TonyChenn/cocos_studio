using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;

namespace MonoDevelop.SourceEditor
{
	internal class ToggleIssuesHandler : CommandHandler
	{
		protected override void Run(object data)
		{
			if (data is Action action)
			{
				action();
			}
		}

		protected override void Update(CommandArrayInfo ainfo)
		{
			CommandInfo commandInfo = ainfo.Add(GettextCatalog.GetString("_Errors & Warnings"), (Action)delegate
			{
				IdeApp.Preferences.ShowMessageBubbles = ShowMessageBubbles.ForErrorsAndWarnings;
			});
			commandInfo.Checked = IdeApp.Preferences.ShowMessageBubbles == ShowMessageBubbles.ForErrorsAndWarnings;
			commandInfo = ainfo.Add(GettextCatalog.GetString("E_rrors only"), (Action)delegate
			{
				IdeApp.Preferences.ShowMessageBubbles = ShowMessageBubbles.ForErrors;
			});
			commandInfo.Checked = IdeApp.Preferences.ShowMessageBubbles == ShowMessageBubbles.ForErrors;
		}
	}
}
