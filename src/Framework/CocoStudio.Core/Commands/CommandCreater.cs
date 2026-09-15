using System;
using Mono.Addins;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;

namespace CocoStudio.Core.Commands
{
	public class CommandCreater : TypeExtensionNode
	{
		public static CommandProxy CreateGlobalCommand(object cmdEnum, string label, CmdGroupEnum groupType, string shortCut = null, string macShortCut = null, ActionType cmdType = ActionType.Normal)
		{
			CommandProxy commandProxy = CommandCreater.CreateActionCommand(cmdEnum, cmdType, label, shortCut, macShortCut, false) as CommandProxy;
			commandProxy.GroupType = groupType;
			commandProxy.IsLocal = false;
			return commandProxy;
		}

		public static CommandProxy CreateLocalCommand(object cmdEnum, string label, CmdGroupEnum groupType, string shortCut = null, string macShortCut = null, ActionType cmdType = ActionType.Normal)
		{
			CommandProxy commandProxy = CommandCreater.CreateGlobalCommand(cmdEnum, label, groupType, shortCut, macShortCut, cmdType);
			commandProxy.IsLocal = true;
			commandProxy.Execute += CommandCreater.EmptyRootCmd_Execute;
			commandProxy.Update += CommandCreater.DisableAndBypass_CanExecute;
			return commandProxy;
		}

		public static CommandArrayProxy CreateCommandArray(object cmdEnum, ActionType cmdType = ActionType.Normal)
		{
			ActionCommand actionCommand = CommandCreater.CreateActionCommand(cmdEnum, cmdType, cmdEnum.ToString() + "队列", null, null, true);
			return actionCommand as CommandArrayProxy;
		}

		private static ActionCommand CreateActionCommand(object cmdEnum, ActionType cmdType, string label, string shortCut, string macShortCut, bool isArray)
		{
			ActionCommand actionCommand;
			if (isArray)
			{
				actionCommand = new CommandArrayProxy();
			}
			else
			{
				actionCommand = new CommandProxy();
			}
			actionCommand.ActionType = cmdType;
			actionCommand.CommandArray = isArray;
			actionCommand.Id = cmdEnum.GetType().FullName + "." + cmdEnum.ToString();
			actionCommand.Text = label;
			if (macShortCut == null)
			{
				macShortCut = shortCut;
			}
			string accelKey = Platform.IsMac ? macShortCut : shortCut;
			if (Platform.IsWindows && !string.IsNullOrEmpty(shortCut))
			{
				accelKey = shortCut;
			}
			actionCommand.AccelKey = accelKey;
			Services.CommandService.RegisterCommand(actionCommand);
			return actionCommand;
		}

		private static void EmptyRootCmd_Execute(object sender, CommandRunArgs e)
		{
		}

		private static void DisableAndBypass_CanExecute(object sender, CommandUpdateArgs e)
		{
			e.Info.Bypass = true;
			e.Info.Enabled = false;
		}
	}
}
