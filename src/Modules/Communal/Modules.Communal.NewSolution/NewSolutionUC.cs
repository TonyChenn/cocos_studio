using System;
using CocoStudio.Core.Commands;
using CocoStudio.Core.ExtensionModel;
using Mono.Addins;

namespace Modules.Communal.NewSolution
{
	// Token: 0x0200000A RID: 10
	[Extension(Type = typeof(ICommandHandle))]
	public class NewSolutionUC : ICommandHandle
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002B41 File Offset: 0x00000D41
		void ICommandHandle.Initialize()
		{
			GlobalCommand.NewCmd.Execute += this.HandleNewExecuted;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002B5C File Offset: 0x00000D5C
		private void HandleNewExecuted(object sender, CommandRunArgs e)
		{
			NewSolutionWindow newSolutionWindow = new NewSolutionWindow();
			newSolutionWindow.Show();
		}
	}
}
