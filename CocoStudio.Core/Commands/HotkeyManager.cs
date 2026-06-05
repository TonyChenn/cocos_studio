using System;
using System.Collections.Generic;
using System.Xml.Linq;
using CocoStudio.Basic;
using Gdk;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;

namespace CocoStudio.Core.Commands
{
	// Token: 0x02000003 RID: 3
	public class HotkeyManager
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static IReadOnlyList<CommandProxy> EditableCommands
		{
			get
			{
				return HotkeyManager.editableCmds;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000002 RID: 2 RVA: 0x00002068 File Offset: 0x00000268
		// (remove) Token: 0x06000003 RID: 3 RVA: 0x000020A4 File Offset: 0x000002A4
		public static event EventHandler<EventArgs> HotkeyChanged;

		// Token: 0x06000004 RID: 4 RVA: 0x000020E0 File Offset: 0x000002E0
		static HotkeyManager()
		{
			HotkeyManager.Init();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000022A0 File Offset: 0x000004A0
		internal static void Init()
		{
			if (!HotkeyManager.hasInitialized)
			{
				HotkeyManager.editableCmds = new List<CommandProxy>();
				HotkeyManager.defaultKeyBindings = new Dictionary<CommandProxy, KeyBinding>();
				foreach (Command command in Services.CommandService.GetCommands())
				{
					CommandProxy commandProxy = command as CommandProxy;
					if (commandProxy != null && commandProxy.GroupType != CmdGroupEnum.NoHotkey)
					{
						HotkeyManager.editableCmds.Add(commandProxy);
						HotkeyManager.defaultKeyBindings.Add(commandProxy, commandProxy.KeyBinding);
					}
				}
				HotkeyManager.Load();
				HotkeyManager.illegalKeyComboList = new List<Tuple<Key, ModifierType>>();
				ModifierType item;
				if (Platform.IsMac)
				{
					item = ModifierType.MetaMask;
				}
				else
				{
					item = ModifierType.ControlMask;
				}
				HotkeyManager.illegalKeyComboList.Add(new Tuple<Key, ModifierType>(Key.c, item));
				HotkeyManager.illegalKeyComboList.Add(new Tuple<Key, ModifierType>(Key.v, item));
				HotkeyManager.illegalKeyComboList.Add(new Tuple<Key, ModifierType>(Key.x, item));
				HotkeyManager.illegalKeyComboList.Add(new Tuple<Key, ModifierType>(Key.C, item));
				HotkeyManager.illegalKeyComboList.Add(new Tuple<Key, ModifierType>(Key.V, item));
				HotkeyManager.illegalKeyComboList.Add(new Tuple<Key, ModifierType>(Key.X, item));
				HotkeyManager.hasInitialized = true;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002400 File Offset: 0x00000600
		private static CommandProxy GetCommandByID(string id)
		{
			foreach (CommandProxy commandProxy in HotkeyManager.editableCmds)
			{
				if (commandProxy.Id.Equals(id))
				{
					return commandProxy;
				}
			}
			return null;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002470 File Offset: 0x00000670
		public static bool CheckIsKeyValid(Key key, ModifierType modifier, bool isLocalCmd, out string output)
		{
			foreach (Key key2 in HotkeyManager.illegalKeyList)
			{
				if (key2 == key)
				{
					output = string.Format("“{0}”不可以设置为快捷键", key);
					return false;
				}
			}
			foreach (Tuple<Key, ModifierType> tuple in HotkeyManager.illegalKeyComboList)
			{
				if (tuple.Item1 == key && tuple.Item2 == modifier)
				{
					output = "当前按键不可以设置为快捷键";
					return false;
				}
			}
			if (!isLocalCmd && modifier == ModifierType.None)
			{
				bool flag = false;
				foreach (Key key3 in HotkeyManager.singleKeyList)
				{
					if (key3 == key)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					output = string.Format("“{0}”不能作为当前命令的快捷键，请与修饰键组合或使用其它按键", key.ToString());
					return false;
				}
			}
			output = string.Empty;
			return true;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002604 File Offset: 0x00000804
		public static KeyBinding GetDefaultKeyBinding(CommandProxy cmd)
		{
			KeyBinding result;
			HotkeyManager.defaultKeyBindings.TryGetValue(cmd, out result);
			return result;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002628 File Offset: 0x00000828
		public static void ChangeHotkeys(List<Tuple<CommandProxy, string>> newKeyBindings)
		{
			foreach (Tuple<CommandProxy, string> tuple in newKeyBindings)
			{
				tuple.Item1.AccelKey = tuple.Item2;
			}
			if (HotkeyManager.HotkeyChanged != null)
			{
				HotkeyManager.HotkeyChanged(null, new EventArgs());
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000026A8 File Offset: 0x000008A8
		private static void Load()
		{
			try
			{
				XElement xelement = XElement.Load(Option.HotkeyConfigPath);
				if (xelement.Attribute("Type").Value.Equals("Custom"))
				{
					IEnumerable<XElement> enumerable = xelement.Elements();
					foreach (XElement xelement2 in enumerable)
					{
						string value = xelement2.Attribute("CommandID").Value;
						string value2 = xelement2.Attribute("Hotkey").Value;
						CommandProxy commandByID = HotkeyManager.GetCommandByID(value);
						if (commandByID != null)
						{
							commandByID.AccelKey = KeyBindingManager.CanonicalizeBinding(value2);
						}
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("从配置文件读取快捷键设置时出错", exception);
				HotkeyManager.ResetAllToDefault();
			}
			if (HotkeyManager.HotkeyChanged != null)
			{
				HotkeyManager.HotkeyChanged(null, new EventArgs());
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000027E4 File Offset: 0x000009E4
		private static void Save()
		{
			try
			{
				XElement[] array = new XElement[HotkeyManager.editableCmds.Count];
				for (int i = 0; i < HotkeyManager.editableCmds.Count; i++)
				{
					string value = HotkeyManager.editableCmds[i].Id.ToString();
					string value2 = "";
					if (HotkeyManager.editableCmds[i].KeyBinding != null)
					{
						value2 = KeyBindingManager.BindingToDisplayLabel(HotkeyManager.editableCmds[i].KeyBinding, false);
					}
					array[i] = new XElement("KeyBinding");
					array[i].SetAttributeValue("CommandID", value);
					array[i].SetAttributeValue("Hotkey", value2);
				}
				XElement xelement = new XElement("KeyBindingList", array);
				xelement.SetAttributeValue("Type", "Custom");
				xelement.Save(Option.HotkeyConfigPath);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("保存快捷键配置文件时出错", exception);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002910 File Offset: 0x00000B10
		private static void ResetAllToDefault()
		{
			foreach (CommandProxy commandProxy in HotkeyManager.defaultKeyBindings.Keys)
			{
				KeyBinding keyBinding;
				HotkeyManager.defaultKeyBindings.TryGetValue(commandProxy, out keyBinding);
				if (keyBinding == null)
				{
					commandProxy.AccelKey = null;
				}
				else
				{
					commandProxy.AccelKey = keyBinding.ToString();
				}
			}
		}

		// Token: 0x04000013 RID: 19
		private const string node_keyBindingList = "KeyBindingList";

		// Token: 0x04000014 RID: 20
		private const string node_keyBinding = "KeyBinding";

		// Token: 0x04000015 RID: 21
		private const string attribute_type = "Type";

		// Token: 0x04000016 RID: 22
		private const string attribute_commandID = "CommandID";

		// Token: 0x04000017 RID: 23
		private const string attribute_hotkey = "Hotkey";

		// Token: 0x04000018 RID: 24
		private const string value_custom = "Custom";

		// Token: 0x04000019 RID: 25
		private static bool hasInitialized = false;

		// Token: 0x0400001A RID: 26
		private static List<Key> illegalKeyList = new List<Key>
		{
			Key.Escape,
			Key.Tab,
			Key.Menu,
			Key.Num_Lock,
			Key.Delete,
			Key.BackSpace,
			Key.Clear,
			Key.Up,
			Key.Down,
			Key.Left,
			Key.Right
		};

		// Token: 0x0400001B RID: 27
		private static List<Tuple<Key, ModifierType>> illegalKeyComboList;

		// Token: 0x0400001C RID: 28
		private static List<Key> singleKeyList = new List<Key>
		{
			Key.F1,
			Key.F2,
			Key.F3,
			Key.F4,
			Key.F5,
			Key.F6,
			Key.F7,
			Key.F8,
			Key.F9,
			Key.F10,
			Key.F11,
			Key.F12,
			Key.F13,
			Key.F14,
			Key.F15,
			Key.F16,
			Key.F17,
			Key.F18,
			Key.Insert,
			Key.Home,
			Key.Prior,
			Key.Next
		};

		// Token: 0x0400001D RID: 29
		private static Dictionary<CommandProxy, KeyBinding> defaultKeyBindings;

		// Token: 0x0400001E RID: 30
		private static List<CommandProxy> editableCmds;
	}
}
