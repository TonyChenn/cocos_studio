using System;
using System.Linq;
using CocoStudio.Basic;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Components.Commands;
using MonoDevelop.Components.Commands.ExtensionNodes;
using MonoDevelop.Core;

namespace CocoStudio.Core.ExtensionModel
{
	// Token: 0x02000004 RID: 4
	[ExtensionNode(Description = "Mono中的CommandCodon与CommandItemCodon的合并形式")]
	internal class CmdEntryCodon : InstanceExtensionNode
	{
		// Token: 0x0600000E RID: 14 RVA: 0x000029A0 File Offset: 0x00000BA0
		public override object CreateInstance()
		{
			object result;
			if (this.isInternal)
			{
				Command command = Services.CommandService.GetCommand(base.Id);
				if (command == null)
				{
					result = null;
				}
				else
				{
					result = new CommandEntry(command);
				}
			}
			else
			{
				if (this.label == null)
				{
					this.label = base.Id;
				}
				this.label = LanguageOption.GetValueBykey(this.label);
				Command command = this.CreateCommand();
				Services.CommandService.RegisterCommand(command);
				result = new CommandEntry(command);
			}
			return result;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002A30 File Offset: 0x00000C30
		private Command CreateCommand()
		{
			ActionType actionType = ActionType.Normal;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			string[] array = this.type.Split(new char[]
			{
				'|'
			});
			int i = 0;
			while (i < array.Length)
			{
				string text = array[i];
				string text2 = text;
				if (text2 != null)
				{
					if (!(text2 == "check"))
					{
						if (!(text2 == "radio"))
						{
							if (!(text2 == "normal"))
							{
								if (!(text2 == "custom"))
								{
									if (!(text2 == "array"))
									{
										goto IL_100;
									}
									flag = true;
								}
								else
								{
									if (this.widget == null)
									{
										throw new InvalidOperationException("Widget type not specified in custom command.");
									}
									flag2 = true;
								}
							}
							else
							{
								actionType = ActionType.Normal;
								if (flag3)
								{
									throw new InvalidOperationException("Action type specified twice.");
								}
								flag3 = true;
							}
						}
						else
						{
							actionType = ActionType.Radio;
							if (flag3)
							{
								throw new InvalidOperationException("Action type specified twice.");
							}
							flag3 = true;
						}
					}
					else
					{
						actionType = ActionType.Check;
						if (flag3)
						{
							throw new InvalidOperationException("Action type specified twice.");
						}
						flag3 = true;
					}
					i++;
					continue;
				}
				IL_100:
				throw new InvalidOperationException("Unknown command type: " + text);
			}
			if (flag3 && flag2)
			{
				throw new InvalidOperationException("Invalid command type combination: " + this.type);
			}
			Command command;
			if (flag2)
			{
				if (flag)
				{
					throw new InvalidOperationException("Array custom commands are not allowed.");
				}
				CustomCommand customCommand = new CustomCommand();
				customCommand.Text = this.label;
				customCommand.WidgetType = base.Addin.GetType(this.widget);
				if (customCommand.WidgetType == null)
				{
					throw new InvalidOperationException("Could not find command type '" + this.widget + "'.");
				}
				command = customCommand;
			}
			else
			{
				if (this.widget != null)
				{
					throw new InvalidOperationException("Widget type can only be specified for custom commands.");
				}
				ActionCommand actionCommand = new ActionCommand();
				actionCommand.ActionType = actionType;
				actionCommand.CommandArray = flag;
				if (this.handler != null)
				{
					try
					{
						Type type = base.Addin.GetType(this.handler, true);
						MenuHandler menuHandler = (MenuHandler)Activator.CreateInstance(type);
						if (menuHandler != null)
						{
							actionCommand.DefaultHandler = new CcsCmdHandler(menuHandler);
							actionCommand.DefaultHandlerType = actionCommand.DefaultHandler.GetType();
						}
					}
					catch
					{
						LogConfig.Output.Error(string.Format("Failed to create MenuHandler: {0}", this.handler));
					}
				}
				command = actionCommand;
			}
			command.Id = CmdEntryCodon.ParseCommandId(this);
			command.Text = this.label;
			if (this.description != null && this.description.Length > 0)
			{
				command.Description = BrandingService.BrandApplicationName(this.description);
			}
			command.Description = command.Description;
			if (this.icon != null)
			{
				command.Icon = CmdEntryCodon.GetStockId(base.Addin, this.icon);
			}
			string text3 = Platform.IsMac ? this.macShortcut : this.shortcut;
			if (Platform.IsWindows && !string.IsNullOrEmpty(this.winShortcut))
			{
				text3 = this.winShortcut;
			}
			string[] array2 = (text3 ?? "").Split(new char[]
			{
				' '
			});
			command.AccelKey = KeyBindingManager.CanonicalizeBinding(array2[0]);
			if (array2.Length > 1)
			{
				command.AlternateAccelKeys = array2.Skip(1).ToArray<string>();
			}
			command.DisabledVisible = this.disabledVisible;
			CommandCategoryCodon commandCategoryCodon = base.Parent as CommandCategoryCodon;
			if (commandCategoryCodon != null)
			{
				command.Category = commandCategoryCodon.Name;
			}
			return command;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002E4C File Offset: 0x0000104C
		internal static object ParseCommandId(ExtensionNode codon)
		{
			string id = codon.Id;
			object result;
			if (id.StartsWith("@"))
			{
				result = id.Substring(1);
			}
			else
			{
				result = id;
			}
			return result;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002E84 File Offset: 0x00001084
		internal static string GetStockId(RuntimeAddin addin, string icon)
		{
			return icon;
		}

		// Token: 0x04000020 RID: 32
		[NodeAttribute("label", "Label", Localizable = true)]
		private string label;

		// Token: 0x04000021 RID: 33
		[NodeAttribute("description", "Description of the command", Localizable = true)]
		private string description;

		// Token: 0x04000022 RID: 34
		[NodeAttribute("shortcut", "Key combination that triggers the command. Control, Alt, Meta, Super and Shift modifiers can be specified using '+' as a separator. Multi-state key bindings can be specified using a '|' between the mode and accel. For example 'Control+D' or 'Control+X|Control+S'")]
		private string shortcut;

		// Token: 0x04000023 RID: 35
		[NodeAttribute("macShortcut", "Mac version of the shortcut. Format is that same as 'shortcut', but the 'Meta' modifier corresponds to the Command key.")]
		private string macShortcut;

		// Token: 0x04000024 RID: 36
		[NodeAttribute("winShortcut", "Win version of the shortcut. Format is that same as 'shortcut'.")]
		private string winShortcut;

		// Token: 0x04000025 RID: 37
		[NodeAttribute("disabledVisible", "Set to 'false' if the command has to be hidden when disabled. 'true' by default.")]
		private bool disabledVisible = true;

		// Token: 0x04000026 RID: 38
		[NodeAttribute("type", "Type of the command. It can be: normal (the default), check, radio or array.")]
		private string type = "normal";

		// Token: 0x04000027 RID: 39
		[NodeAttribute("handler", "Class that handles this command. This property is optional.")]
		private string handler;

		// Token: 0x04000028 RID: 40
		[NodeAttribute("isInternal", "是否使用编辑器内部使用的命令")]
		private bool isInternal = false;

		// Token: 0x04000029 RID: 41
		private string icon;

		// Token: 0x0400002A RID: 42
		private string widget = null;
	}
}
