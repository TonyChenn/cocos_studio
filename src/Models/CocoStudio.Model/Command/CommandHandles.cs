using System;
using CocoStudio.Core.Commands;
using CocoStudio.Core.ExtensionModel;
using CocoStudio.Model.ViewModel;
using Mono.Addins;

namespace CocoStudio.Model.Command
{
	// Token: 0x02000002 RID: 2
	[Extension(Type = typeof(ICommandHandle))]
	internal class CommandHandles : ICommandHandle
	{
		// Token: 0x06000002 RID: 2 RVA: 0x0000205B File Offset: 0x0000025B
		public void Initialize()
		{
			GlobalCommand.PlayCmd.Execute += CommandHandles.PlayCmd_Execute;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002078 File Offset: 0x00000278
		private static void PlayCmd_Execute(object sender, CommandRunArgs e)
		{
			bool isPlaying = TimelineActionManager.Instance.IsPlaying;
			if (isPlaying)
			{
				TimelineActionManager.Instance.Pause(false);
			}
			else
			{
				TimelineActionManager.Instance.Play();
			}
		}
	}
}
