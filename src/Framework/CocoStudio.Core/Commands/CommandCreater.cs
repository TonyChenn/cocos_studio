using System;
using Mono.Addins;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;

namespace CocoStudio.Core.Commands
{
	// Token: 0x0200000C RID: 12
	public class CommandCreater : TypeExtensionNode
	{
		// Token: 0x06000055 RID: 85 RVA: 0x000035A4 File Offset: 0x000017A4
		public static CommandProxy CreateGlobalCommand(object cmdEnum, string label, CmdGroupEnum groupType, string shortCut = null, string macShortCut = null, ActionType cmdType = ActionType.Normal)
		{
			CommandProxy commandProxy = CommandCreater.CreateActionCommand(cmdEnum, cmdType, label, shortCut, macShortCut, false) as CommandProxy;
			commandProxy.GroupType = groupType;
			commandProxy.IsLocal = false;
			return commandProxy;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000035DC File Offset: 0x000017DC
		public static CommandProxy CreateLocalCommand(object cmdEnum, string label, CmdGroupEnum groupType, string shortCut = null, string macShortCut = null, ActionType cmdType = ActionType.Normal)
		{
			CommandProxy commandProxy = CommandCreater.CreateGlobalCommand(cmdEnum, label, groupType, shortCut, macShortCut, cmdType);
			commandProxy.IsLocal = true;
			commandProxy.Execute += CommandCreater.EmptyRootCmd_Execute;
			commandProxy.Update += CommandCreater.DisableAndBypass_CanExecute;
			return commandProxy;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000362C File Offset: 0x0000182C
		public static CommandArrayProxy CreateCommandArray(object cmdEnum, ActionType cmdType = ActionType.Normal)
		{
			ActionCommand actionCommand = CommandCreater.CreateActionCommand(cmdEnum, cmdType, cmdEnum.ToString() + "队列", null, null, true);
			return actionCommand as CommandArrayProxy;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003660 File Offset: 0x00001860
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

		// Token: 0x06000059 RID: 89 RVA: 0x0000370F File Offset: 0x0000190F
		private static void EmptyRootCmd_Execute(object sender, CommandRunArgs e)
		{
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003712 File Offset: 0x00001912
		private static void DisableAndBypass_CanExecute(object sender, CommandUpdateArgs e)
		{
			e.Info.Bypass = true;
			e.Info.Enabled = false;
		}
	}
}
