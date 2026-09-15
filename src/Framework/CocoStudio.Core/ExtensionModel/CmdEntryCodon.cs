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
	[ExtensionNode(Description = "Mono中的CommandCodon与CommandItemCodon的合并形式")]
	internal class CmdEntryCodon : InstanceExtensionNode
	{
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

		internal static string GetStockId(RuntimeAddin addin, string icon)
		{
			return icon;
		}

		[NodeAttribute("label", "Label", Localizable = true)]
		private string label;

		[NodeAttribute("description", "Description of the command", Localizable = true)]
		private string description;

		[NodeAttribute("shortcut", "Key combination that triggers the command. Control, Alt, Meta, Super and Shift modifiers can be specified using '+' as a separator. Multi-state key bindings can be specified using a '|' between the mode and accel. For example 'Control+D' or 'Control+X|Control+S'")]
		private string shortcut;

		[NodeAttribute("macShortcut", "Mac version of the shortcut. Format is that same as 'shortcut', but the 'Meta' modifier corresponds to the Command key.")]
		private string macShortcut;

		[NodeAttribute("winShortcut", "Win version of the shortcut. Format is that same as 'shortcut'.")]
		private string winShortcut;

		[NodeAttribute("disabledVisible", "Set to 'false' if the command has to be hidden when disabled. 'true' by default.")]
		private bool disabledVisible = true;

		[NodeAttribute("type", "Type of the command. It can be: normal (the default), check, radio or array.")]
		private string type = "normal";

		[NodeAttribute("handler", "Class that handles this command. This property is optional.")]
		private string handler;

		[NodeAttribute("isInternal", "是否使用编辑器内部使用的命令")]
		private bool isInternal = false;

		private string icon;

		private string widget = null;
	}
}
