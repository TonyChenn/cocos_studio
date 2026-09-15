using System;
using CocoStudio.Core.Commands;
using CocoStudio.Core.ExtensionModel;
using CocoStudio.Model.ViewModel;
using Mono.Addins;

namespace CocoStudio.Model.Command
{
	[Extension(Type = typeof(ICommandHandle))]
	internal class CommandHandles : ICommandHandle
	{
		public void Initialize()
		{
			GlobalCommand.PlayCmd.Execute += CommandHandles.PlayCmd_Execute;
		}

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
