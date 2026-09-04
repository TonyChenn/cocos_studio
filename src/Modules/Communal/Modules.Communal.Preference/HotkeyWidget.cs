using System;
using System.Collections.Generic;
using System.ComponentModel;
using CocoStudio.Core.Commands;
using Gdk;
using GLib;
using Gtk;
using Mono.Unix;
using MonoDevelop.Components.Commands;
using Stetic;

namespace Modules.Communal.Preference
{
	// Token: 0x02000011 RID: 17
	[ToolboxItem(true)]
	public class HotkeyWidget : Bin, IPreferenceWidget
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00006310 File Offset: 0x00004510
		// (set) Token: 0x06000062 RID: 98 RVA: 0x0000632B File Offset: 0x0000452B
		private string CurrentBinding
		{
			get
			{
				if (string.IsNullOrEmpty(this.realBinding))
				{
					return "";
				}
				return this.realBinding;
			}
			set
			{
				this.realBinding = value;
				if (this.currentEntry != null)
				{
					this.currentEntry.Text = ((this.realBinding == null) ? "" : KeyBindingManager.BindingToDisplayLabel(this.realBinding, false, true));
				}
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00006363 File Offset: 0x00004563
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00006370 File Offset: 0x00004570
		private string WarringInfo
		{
			get
			{
				return this.label_warring.Text;
			}
			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					this.label_warring.Text = "";
					while (this.vbox_bottom.Children.Length > 0)
					{
						this.vbox_bottom.Remove(this.vbox_bottom.Children[0]);
					}
					return;
				}
				this.label_warring.Text = value;
				if (this.vbox_bottom.Children.Length == 0)
				{
					this.vbox_bottom.PackStart(this.hbox_warring, false, false, 0U);
				}
				this.hbox_warring.ShowAll();
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000063FA File Offset: 0x000045FA
		public HotkeyWidget()
		{
			this.Build();
			this.InitTreeView();
			this.InitEvent();
			this.InitStyle();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000641C File Offset: 0x0000461C
		private void InitTreeView()
		{
			TreeViewColumn treeViewColumn = new TreeViewColumn("命令", new CellRendererText(), new object[]
			{
				"text",
				0
			});
			treeViewColumn.MinWidth = 100;
			this.treeview_hotkey.AppendColumn(treeViewColumn);
			this.hotkeyRenderer = new CellRendererText();
			this.hotkeyRenderer.Editable = true;
			this.hotkeyRenderer.EditingStarted += this.HandleEditingStarted;
			this.hotkeyRenderer.Edited += this.HandleEditingEdited;
			this.hotkeyRenderer.EditingCanceled += this.HandleEditingCanceled;
			this.hotkeyColumn = new TreeViewColumn("快捷键", this.hotkeyRenderer, new object[]
			{
				"text",
				1
			});
			this.treeview_hotkey.AppendColumn(this.hotkeyColumn);
			TreeStore treeStore = new TreeStore(new Type[]
			{
				typeof(string),
				typeof(string)
			});
			TreeIter value = treeStore.AppendValues(new object[]
			{
				"主菜单－文件"
			});
			TreeIter value2 = treeStore.AppendValues(new object[]
			{
				"主菜单－编辑"
			});
			TreeIter value3 = treeStore.AppendValues(new object[]
			{
				"主菜单－项目"
			});
			TreeIter value4 = treeStore.AppendValues(new object[]
			{
				"主菜单－窗口"
			});
			TreeIter value5 = treeStore.AppendValues(new object[]
			{
				"主菜单－帮助"
			});
			TreeIter value6 = treeStore.AppendValues(new object[]
			{
				"主菜单－语言"
			});
			TreeIter value7 = treeStore.AppendValues(new object[]
			{
				"资源面板"
			});
			TreeIter value8 = treeStore.AppendValues(new object[]
			{
				"动画面板"
			});
			TreeIter value9 = treeStore.AppendValues(new object[]
			{
				"标签页"
			});
			Dictionary<CmdGroupEnum, TreeIter> dictionary = new Dictionary<CmdGroupEnum, TreeIter>();
			dictionary.Add(CmdGroupEnum.File, value);
			dictionary.Add(CmdGroupEnum.Edit, value2);
			dictionary.Add(CmdGroupEnum.Project, value3);
			dictionary.Add(CmdGroupEnum.Window, value4);
			dictionary.Add(CmdGroupEnum.Help, value5);
			dictionary.Add(CmdGroupEnum.Language, value6);
			dictionary.Add(CmdGroupEnum.ResourcePanel, value7);
			dictionary.Add(CmdGroupEnum.AnimationPanel, value8);
			dictionary.Add(CmdGroupEnum.PageContextMenu, value9);
			this.treeCmdDictionary = new Dictionary<TreeIter, HotkeyWidget.EdittingCmd>();
			foreach (CommandProxy commandProxy in HotkeyManager.EditableCommands)
			{
				TreeIter parent;
				if (dictionary.TryGetValue(commandProxy.GroupType, out parent))
				{
					string text = "";
					if (commandProxy.KeyBinding != null)
					{
						text = commandProxy.AccelKey;
					}
					TreeIter key = treeStore.AppendValues(parent, new object[]
					{
						commandProxy.Text,
						text
					});
					HotkeyWidget.EdittingCmd value10 = new HotkeyWidget.EdittingCmd(commandProxy, commandProxy.AccelKey);
					this.treeCmdDictionary.Add(key, value10);
				}
			}
			this.treeview_hotkey.Model = treeStore;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00006754 File Offset: 0x00004954
		private void InitEvent()
		{
			this.button_reset.CanFocus = false;
			this.button_default.CanFocus = false;
			this.button_remove.CanFocus = false;
			this.button_resetAll.CanFocus = false;
			this.button_reset.Clicked += this.HandleResetButtonClicked;
			this.button_default.Clicked += this.HandleDefaultButtonClicked;
			this.button_remove.Clicked += this.HandleRemoveButtonClicked;
			this.button_resetAll.Clicked += this.HandleResetAllButtonClicked;
			this.treeview_hotkey.Selection.Changed += this.HandleTreeViewSelectionChanged;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000680C File Offset: 0x00004A0C
		private void InitStyle()
		{
			this.evtbx_treeViewBorder.ModifyBg(StateType.Normal, WindowStyle.LineDarkColor);
			this.evtbx_infoBorder.ModifyBg(StateType.Normal, WindowStyle.LineDarkColor);
			this.treeview_hotkey.Name = "DarkTreeView";
			this.treeview_hotkey.ExpandAll();
			this.WarringInfo = null;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00006860 File Offset: 0x00004A60
		private bool CheckIsHotkeyExist(string accel, out HotkeyWidget.EdittingCmd existKeyCmd)
		{
			if (string.IsNullOrEmpty(accel))
			{
				existKeyCmd = null;
				return false;
			}
			foreach (HotkeyWidget.EdittingCmd edittingCmd in this.treeCmdDictionary.Values)
			{
				if (!string.IsNullOrEmpty(edittingCmd.accelKey) && edittingCmd.accelKey.Equals(accel) && edittingCmd != this.currentCmd)
				{
					existKeyCmd = edittingCmd;
					return true;
				}
			}
			existKeyCmd = null;
			return false;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000068F0 File Offset: 0x00004AF0
		private void SetKeybinding(EventKey eventKey)
		{
			Gdk.Key key = eventKey.Key;
			if (key == Gdk.Key.BackSpace || key == Gdk.Key.Delete || key == Gdk.Key.Clear)
			{
				this.CurrentBinding = null;
				this.Clear();
				return;
			}
			bool flag;
			string text = KeyBindingManager.AccelLabelFromKey(eventKey, out flag);
			if (text.Equals("VoidSymbol"))
			{
				return;
			}
			if (flag)
			{
				string warringInfo;
				if (!HotkeyManager.CheckIsKeyValid(key, eventKey.State, this.currentCmd.cmd.IsLocal, out warringInfo))
				{
					this.WarringInfo = warringInfo;
					return;
				}
				this.CurrentBinding = text;
				if (this.CheckIsHotkeyExist(text, out this.existKeyCmd))
				{
					this.WarringInfo = string.Format("“{0}”已被 [{1}] 使用。如果应用，将移除 [{1}] 的快捷键", text, this.existKeyCmd.cmd.Text);
					return;
				}
				this.Clear();
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000069AC File Offset: 0x00004BAC
		private void ApplyCurrentKey()
		{
			TreeIter treeIter;
			this.treeview_hotkey.Selection.GetSelected(out treeIter);
			if (this.treeCmdDictionary.TryGetValue(treeIter, out this.currentCmd))
			{
				this.treeview_hotkey.Model.SetValue(treeIter, 1, this.CurrentBinding);
				this.currentCmd.accelKey = this.CurrentBinding;
			}
			if (this.existKeyCmd != null)
			{
				foreach (TreeIter treeIter2 in this.treeCmdDictionary.Keys)
				{
					HotkeyWidget.EdittingCmd edittingCmd;
					if (this.treeCmdDictionary.TryGetValue(treeIter2, out edittingCmd) && edittingCmd == this.existKeyCmd)
					{
						edittingCmd.accelKey = string.Empty;
						this.treeview_hotkey.Model.SetValue(treeIter2, 1, string.Empty);
						this.WarringInfo = string.Empty;
					}
				}
			}
			this.Clear();
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00006AA4 File Offset: 0x00004CA4
		private void Clear()
		{
			this.WarringInfo = string.Empty;
			this.existKeyCmd = null;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00006AB8 File Offset: 0x00004CB8
		public EnumPreferenceSetting SettingID
		{
			get
			{
				return EnumPreferenceSetting.Hotkey;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00006ABB File Offset: 0x00004CBB
		public string DisplayName
		{
			get
			{
				return "快捷键";
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00006AC4 File Offset: 0x00004CC4
		public void ApplySetting()
		{
			List<Tuple<CommandProxy, string>> list = new List<Tuple<CommandProxy, string>>();
			foreach (HotkeyWidget.EdittingCmd edittingCmd in this.treeCmdDictionary.Values)
			{
				list.Add(new Tuple<CommandProxy, string>(edittingCmd.cmd, edittingCmd.accelKey));
			}
			HotkeyManager.ChangeHotkeys(list);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00006B38 File Offset: 0x00004D38
		public bool CanApply(out string output)
		{
			output = "";
			return true;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00006B42 File Offset: 0x00004D42
		public Widget GetWidget()
		{
			return this;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00006B45 File Offset: 0x00004D45
		private void HandleEditingStarted(object o, EditingStartedArgs args)
		{
			this.currentEntry = (args.Editable as Entry);
			this.currentEntry.KeyPressEvent += this.HandleKeyEntryPressed;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00006B6F File Offset: 0x00004D6F
		private void HandleEditingCanceled(object sender, EventArgs e)
		{
			if (this.currentEntry != null)
			{
				this.currentEntry.KeyPressEvent -= this.HandleKeyEntryPressed;
			}
			this.currentEntry = null;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00006B97 File Offset: 0x00004D97
		private void HandleEditingEdited(object o, EditedArgs args)
		{
			this.ApplyCurrentKey();
			if (this.currentEntry != null)
			{
				this.currentEntry.KeyPressEvent -= this.HandleKeyEntryPressed;
			}
			this.currentEntry = null;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00006C08 File Offset: 0x00004E08
		private void HandleTreeViewSelectionChanged(object sender, EventArgs e)
		{
			TreeIter curIter;
			this.treeview_hotkey.Selection.GetSelected(out curIter);
			if (this.treeCmdDictionary.TryGetValue(curIter, out this.currentCmd))
			{
				if (string.IsNullOrEmpty(this.currentCmd.accelKey))
				{
					this.CurrentBinding = null;
				}
				else
				{
					this.CurrentBinding = this.currentCmd.accelKey;
				}
				GLib.Timeout.Add(10U, delegate
				{
					this.treeview_hotkey.SetCursor(this.treeview_hotkey.Model.GetPath(curIter), this.hotkeyColumn, true);
					return false;
				});
			}
			this.Clear();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00006CA0 File Offset: 0x00004EA0
		[ConnectBefore]
		private void HandleKeyEntryPressed(object o, KeyPressEventArgs args)
		{
			Gdk.Key key = args.Event.Key;
			if (key == Gdk.Key.Escape || key == Gdk.Key.ISO_Enter || key == Gdk.Key.KP_Enter || key == Gdk.Key.Key_3270_Enter || key == Gdk.Key.Return || key == Gdk.Key.Up || key == Gdk.Key.Down)
			{
				return;
			}
			args.RetVal = true;
			this.SetKeybinding(args.Event);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00006D0A File Offset: 0x00004F0A
		private void HandleRemoveButtonClicked(object sender, EventArgs e)
		{
			if (this.currentCmd == null)
			{
				return;
			}
			this.CurrentBinding = null;
			this.Clear();
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00006D22 File Offset: 0x00004F22
		private void HandleResetButtonClicked(object sender, EventArgs e)
		{
			if (this.currentCmd == null)
			{
				return;
			}
			this.CurrentBinding = this.currentCmd.accelKey;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00006D40 File Offset: 0x00004F40
		private void HandleDefaultButtonClicked(object sender, EventArgs e)
		{
			if (this.currentCmd == null)
			{
				return;
			}
			KeyBinding defaultKeyBinding = HotkeyManager.GetDefaultKeyBinding(this.currentCmd.cmd);
			string text;
			if (defaultKeyBinding == null)
			{
				text = string.Empty;
			}
			else
			{
				text = KeyBindingManager.BindingToDisplayLabel(defaultKeyBinding, false);
			}
			this.CurrentBinding = text;
			if (this.CheckIsHotkeyExist(text, out this.existKeyCmd))
			{
				this.WarringInfo = string.Format("[{0}]已经使用了当前快捷键。如果应用，将移除[{0}]的快捷键", this.existKeyCmd.cmd.Text);
				return;
			}
			this.Clear();
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00006DB8 File Offset: 0x00004FB8
		private void HandleResetAllButtonClicked(object sender, EventArgs e)
		{
			foreach (TreeIter treeIter in this.treeCmdDictionary.Keys)
			{
				HotkeyWidget.EdittingCmd edittingCmd;
				if (this.treeCmdDictionary.TryGetValue(treeIter, out edittingCmd))
				{
					KeyBinding defaultKeyBinding = HotkeyManager.GetDefaultKeyBinding(edittingCmd.cmd);
					if (defaultKeyBinding == null)
					{
						edittingCmd.accelKey = string.Empty;
					}
					else
					{
						edittingCmd.accelKey = KeyBindingManager.BindingToDisplayLabel(defaultKeyBinding, false);
					}
					this.treeview_hotkey.Model.SetValue(treeIter, 1, edittingCmd.accelKey);
				}
			}
			this.Clear();
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00006E60 File Offset: 0x00005060
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.Preference.HotkeyWidget";
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 6;
			this.hbox_main = new HBox();
			this.hbox_main.Name = "hbox_main";
			this.hbox_main.Spacing = 10;
			this.evtbx_treeViewBorder = new EventBox();
			this.evtbx_treeViewBorder.Name = "evtbx_treeViewBorder";
			this.GtkScrolledWindow = new ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ShadowType.In;
			this.GtkScrolledWindow.BorderWidth = 1U;
			this.treeview_hotkey = new TreeView();
			this.treeview_hotkey.CanFocus = true;
			this.treeview_hotkey.Name = "treeview_hotkey";
			this.GtkScrolledWindow.Add(this.treeview_hotkey);
			this.evtbx_treeViewBorder.Add(this.GtkScrolledWindow);
			this.hbox_main.Add(this.evtbx_treeViewBorder);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox_main[this.evtbx_treeViewBorder];
			boxChild.Position = 0;
			this.vbox_right = new VBox();
			this.vbox_right.Name = "vbox_right";
			this.vbox_right.Spacing = 15;
			this.button_accept = new Button();
			this.button_accept.WidthRequest = 75;
			this.button_accept.HeightRequest = 22;
			this.button_accept.Name = "button_accept";
			this.button_accept.UseUnderline = true;
			this.button_accept.Label = Catalog.GetString("接受");
			this.vbox_right.Add(this.button_accept);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_right[this.button_accept];
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.button_reset = new Button();
			this.button_reset.WidthRequest = 75;
			this.button_reset.HeightRequest = 22;
			this.button_reset.Name = "button_reset";
			this.button_reset.UseUnderline = true;
			this.button_reset.Label = Catalog.GetString("还原");
			this.vbox_right.Add(this.button_reset);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox_right[this.button_reset];
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.button_default = new Button();
			this.button_default.WidthRequest = 75;
			this.button_default.HeightRequest = 22;
			this.button_default.Name = "button_default";
			this.button_default.UseUnderline = true;
			this.button_default.Label = Catalog.GetString("默认");
			this.vbox_right.Add(this.button_default);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_right[this.button_default];
			boxChild4.Position = 2;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.button_remove = new Button();
			this.button_remove.WidthRequest = 75;
			this.button_remove.HeightRequest = 22;
			this.button_remove.Name = "button_remove";
			this.button_remove.UseUnderline = true;
			this.button_remove.Label = Catalog.GetString("移除");
			this.vbox_right.Add(this.button_remove);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox_right[this.button_remove];
			boxChild5.Position = 3;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.button_resetAll = new Button();
			this.button_resetAll.WidthRequest = 75;
			this.button_resetAll.HeightRequest = 22;
			this.button_resetAll.Name = "button_resetAll";
			this.button_resetAll.UseUnderline = true;
			this.button_resetAll.Label = Catalog.GetString("重置");
			this.vbox_right.Add(this.button_resetAll);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.vbox_right[this.button_resetAll];
			boxChild6.Position = 4;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.hbox_main.Add(this.vbox_right);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.hbox_main[this.vbox_right];
			boxChild7.Position = 1;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			this.vbox_main.Add(this.hbox_main);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox_main[this.hbox_main];
			boxChild8.Position = 0;
			this.evtbx_infoBorder = new EventBox();
			this.evtbx_infoBorder.HeightRequest = 80;
			this.evtbx_infoBorder.Name = "evtbx_infoBorder";
			this.evtbx_info = new EventBox();
			this.evtbx_info.Name = "evtbx_info";
			this.evtbx_info.BorderWidth = 1U;
			this.vbox_bottom = new VBox();
			this.vbox_bottom.Name = "vbox_bottom";
			this.vbox_bottom.Spacing = 6;
			this.vbox_bottom.BorderWidth = 6U;
			this.hbox_warring = new HBox();
			this.hbox_warring.Name = "hbox_warring";
			this.hbox_warring.Spacing = 6;
			this.imagebin_warring = new ImageBin();
			this.imagebin_warring.Events = EventMask.ButtonPressMask;
			this.imagebin_warring.Name = "imagebin_warring";
			this.hbox_warring.Add(this.imagebin_warring);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.hbox_warring[this.imagebin_warring];
			boxChild9.Position = 0;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			this.label_warring = new Label();
			this.label_warring.Name = "label_warring";
			this.label_warring.LabelProp = Catalog.GetString("警告信息文本");
			this.label_warring.Wrap = true;
			this.hbox_warring.Add(this.label_warring);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.hbox_warring[this.label_warring];
			boxChild10.Position = 1;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			this.vbox_bottom.Add(this.hbox_warring);
			Box.BoxChild boxChild11 = (Box.BoxChild)this.vbox_bottom[this.hbox_warring];
			boxChild11.Position = 0;
			boxChild11.Expand = false;
			boxChild11.Fill = false;
			this.evtbx_info.Add(this.vbox_bottom);
			this.evtbx_infoBorder.Add(this.evtbx_info);
			this.vbox_main.Add(this.evtbx_infoBorder);
			Box.BoxChild boxChild12 = (Box.BoxChild)this.vbox_main[this.evtbx_infoBorder];
			boxChild12.Position = 1;
			boxChild12.Expand = false;
			base.Add(this.vbox_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		// Token: 0x04000083 RID: 131
		private Dictionary<TreeIter, HotkeyWidget.EdittingCmd> treeCmdDictionary;

		// Token: 0x04000084 RID: 132
		private HotkeyWidget.EdittingCmd currentCmd;

		// Token: 0x04000085 RID: 133
		private HotkeyWidget.EdittingCmd existKeyCmd;

		// Token: 0x04000086 RID: 134
		private TreeViewColumn hotkeyColumn;

		// Token: 0x04000087 RID: 135
		private CellRendererText hotkeyRenderer;

		// Token: 0x04000088 RID: 136
		private string realBinding;

		// Token: 0x04000089 RID: 137
		private Entry currentEntry;

		// Token: 0x0400008A RID: 138
		private VBox vbox_main;

		// Token: 0x0400008B RID: 139
		private HBox hbox_main;

		// Token: 0x0400008C RID: 140
		private EventBox evtbx_treeViewBorder;

		// Token: 0x0400008D RID: 141
		private ScrolledWindow GtkScrolledWindow;

		// Token: 0x0400008E RID: 142
		private TreeView treeview_hotkey;

		// Token: 0x0400008F RID: 143
		private VBox vbox_right;

		// Token: 0x04000090 RID: 144
		private Button button_accept;

		// Token: 0x04000091 RID: 145
		private Button button_reset;

		// Token: 0x04000092 RID: 146
		private Button button_default;

		// Token: 0x04000093 RID: 147
		private Button button_remove;

		// Token: 0x04000094 RID: 148
		private Button button_resetAll;

		// Token: 0x04000095 RID: 149
		private EventBox evtbx_infoBorder;

		// Token: 0x04000096 RID: 150
		private EventBox evtbx_info;

		// Token: 0x04000097 RID: 151
		private VBox vbox_bottom;

		// Token: 0x04000098 RID: 152
		private HBox hbox_warring;

		// Token: 0x04000099 RID: 153
		private ImageBin imagebin_warring;

		// Token: 0x0400009A RID: 154
		private Label label_warring;

		// Token: 0x02000012 RID: 18
		private class EdittingCmd
		{
			// Token: 0x0600007C RID: 124 RVA: 0x0000756E File Offset: 0x0000576E
			public EdittingCmd(CommandProxy cmdProxy, string key)
			{
				this.cmd = cmdProxy;
				this.accelKey = key;
			}

			// Token: 0x0400009B RID: 155
			public CommandProxy cmd;

			// Token: 0x0400009C RID: 156
			public string accelKey;
		}
	}
}
