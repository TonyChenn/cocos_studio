using System;
using CocoStudio.Core.Commands;
using CocoStudio.Core.ExtensionModel;
using Mono.Addins;

namespace Modules.Communal.NewSolution
{
	[Extension(Type = typeof(ICommandHandle))]
	public class NewSolutionUC : ICommandHandle
	{
		void ICommandHandle.Initialize()
		{
			GlobalCommand.NewCmd.Execute += this.HandleNewExecuted;
		}

		private void HandleNewExecuted(object sender, CommandRunArgs e)
		{
			NewSolutionWindow newSolutionWindow = new NewSolutionWindow();
			newSolutionWindow.Show();
		}
	}
}
