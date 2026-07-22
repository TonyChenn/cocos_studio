using System;
using System.Collections.Generic;
using AppKit;
using CocoStudio.Basic;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using Stetic;

namespace Modules.Communal.Preference
{
	// Token: 0x0200000D RID: 13
	public class PreferencesDialog : Dialog
	{
		// Token: 0x0600002F RID: 47 RVA: 0x0000261C File Offset: 0x0000081C
		public PreferencesDialog(EnumPreferenceSetting initWidget = EnumPreferenceSetting.Default)
		{
			this.Build();
			this.InitWidgetList();
			this.InitStyle();
			this.InitTreeView(initWidget);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002654 File Offset: 0x00000854
		private void InitWidgetList()
		{
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				this.environmentWidgets.Add(new CocosRoutineWidget());
				return;
			}
			this.environmentWidgets.Add(new GeneralWidget());
			this.environmentWidgets.Add(new PlatformWidget());
			this.environmentWidgets.Add(new MultireSolutionWidget());
			this.environmentWidgets.Add(new GuidesSettingsWidget());
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000026BC File Offset: 0x000008BC
		private void InitStyle()
		{
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				CustomTitleBar customTitleBar = new CustomTitleBar();
				customTitleBar.HeightRequest = 26;
				customTitleBar.Title = LanguageInfo.Menu_Edit_Preferences;
				customTitleBar.SetParentWindow(this);
				customTitleBar.CloseClicked += this.HandleCustomTitleBarCloseClicked;
				this.alignment_title.Add(customTitleBar);
				customTitleBar.Show();
				this.treeview_preferences.Name = "DarkTreeView";
				this.treeview_preferences.SetFontSize(13.0);
				this.alignment_left.BorderWidth = 1U;
				this.evtbx_left.ModifyBg(StateType.Normal, WindowStyle.LineDimColor);
				this.evtbx_line.ModifyBg(StateType.Normal, WindowStyle.LineDimColor);
				this.CenterToParentWindow(ApplicationCurrent.MainWindow);
				base.TransientFor = ApplicationCurrent.MainWindow;
				if (Platform.IsMac)
				{
					NSWindowStyle style = NSWindowStyle.Titled | NSWindowStyle.DocModal;
					if (base.GdkWindow == null)
					{
						base.Show();
					}
					NativeGdkMac.SetNSWindowStyle(base.GdkWindow, style);
					base.VBox.BorderWidth = 0U;
				}
				else if (Platform.IsWindows)
				{
					base.Decorated = false;
					base.ModifyBg(StateType.Normal, WindowStyle.LineDimColor);
					base.SizeAllocated += this.HanldeDialogSizeAllocated;
				}
			}
			else
			{
				this.treeview_preferences.Name = "DarkTreeView";
				this.evtbx_left.ModifyBg(StateType.Normal, WindowStyle.LineDarkColor);
				this.evtbx_line.ModifyBg(StateType.Normal, WindowStyle.LineDarkColor);
				this.SetToDialogStyle(null, true, true, true);
			}
			base.Title = LanguageInfo.Menu_Edit_Preferences;
			this.InitOkCancelButton();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002830 File Offset: 0x00000A30
		private void InitOkCancelButton()
		{
			Bin bin;
			Bin bin2;
			if (Platform.IsMac)
			{
				bin = this.alignment_bottomBtnRight;
				bin2 = this.alignment_bottomBtnLeft;
			}
			else
			{
				bin = this.alignment_bottomBtnLeft;
				bin2 = this.alignment_bottomBtnRight;
			}
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				GeneralLauncherButton generalLauncherButton = new GeneralLauncherButton();
				generalLauncherButton.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.HandleButtonOKClicked);
				generalLauncherButton.SetButtonStyle(true);
				generalLauncherButton.Text = LanguageInfo.Dialog_ButtonOK;
				generalLauncherButton.SetFontSize(13.0);
				bin.Add(generalLauncherButton);
				GeneralLauncherButton generalLauncherButton2 = new GeneralLauncherButton();
				generalLauncherButton2.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.HandleButttonCancelClicked);
				generalLauncherButton2.SetButtonStyle(false);
				generalLauncherButton2.Text = LanguageInfo.Dialog_ButtonCancel;
				generalLauncherButton2.SetFontSize(13.0);
				bin2.Add(generalLauncherButton2);
			}
			else
			{
				Button button = new Button(LanguageInfo.Dialog_ButtonOK);
				button.Name = "MainButton";
				button.Clicked += this.HandleButtonOKClicked;
				bin.Add(button);
				Button button2 = new Button(LanguageInfo.Dialog_ButtonCancel);
				button2.Clicked += this.HandleButttonCancelClicked;
				bin2.Add(button2);
			}
			this.hbox_bottomBtn.ShowAll();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002954 File Offset: 0x00000B54
		private void InitTreeView(EnumPreferenceSetting initWidget)
		{
			TreeStore treeStore = new TreeStore(new Type[]
			{
				typeof(string)
			});
			TreeIter iter = default(TreeIter);
			foreach (IPreferenceWidget preferenceWidget in this.environmentWidgets)
			{
				TreeIter treeIter = treeStore.AppendValues(new object[]
				{
					preferenceWidget.DisplayName
				});
				this.widgetsDictionary.Add(treeIter, preferenceWidget);
				if (this.currentWidget == null || preferenceWidget.SettingID == initWidget)
				{
					this.currentWidget = preferenceWidget;
					iter = treeIter;
				}
			}
			this.oldTreeIter = iter;
			this.treeview_preferences.Model = treeStore;
			CellRendererText cell = new CellRendererText();
			TreeViewColumn treeViewColumn = new TreeViewColumn();
			treeViewColumn.PackStart(cell, false);
			treeViewColumn.AddAttribute(cell, "text", 0);
			treeViewColumn.Alignment = -40f;
			this.treeview_preferences.AppendColumn(treeViewColumn);
			this.treeview_preferences.CursorChanged += this.HandleTreeSelectChanged;
			this.treeview_preferences.HasTooltip = false;
			this.treeview_preferences.HeadersVisible = false;
			this.treeview_preferences.ExpandAll();
			this.treeview_preferences.Selection.SelectIter(iter);
			this.ChangeWidget(this.currentWidget);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002AB4 File Offset: 0x00000CB4
		private void ChangeWidget(IPreferenceWidget newWidget)
		{
			if (this.currentWidget != null)
			{
				this.evtbx_right.Remove(this.currentWidget.GetWidget());
			}
			this.evtbx_right.Add(newWidget.GetWidget());
			newWidget.GetWidget().Show();
			this.currentWidget = newWidget;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002B04 File Offset: 0x00000D04
		protected void HandleButtonOKClicked(object sender, EventArgs e)
		{
			foreach (IPreferenceWidget preferenceWidget in this.widgetsDictionary.Values)
			{
				string text;
				if (!preferenceWidget.CanApply(out text))
				{
					if (this.currentWidget != preferenceWidget)
					{
						this.ChangeWidget(preferenceWidget);
					}
					if (!string.IsNullOrEmpty(text))
					{
						MessageBox.Show(text, MessageBoxImage.Warning, null, null);
					}
					return;
				}
			}
			foreach (IPreferenceWidget preferenceWidget2 in this.environmentWidgets)
			{
				preferenceWidget2.ApplySetting();
			}
			Option.UserConfig.Save();
			if (Option.CurrentApp == EnumApp.Studio)
			{
				PreferenceManager.Instance.RefreshEnvironmentPrompt();
			}
			base.Respond(ResponseType.Ok);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002BE8 File Offset: 0x00000DE8
		protected void HandleTreeSelectChanged(object sender, EventArgs e)
		{
			TreeIter key;
			if (this.treeview_preferences.Selection.GetSelected(out key))
			{
				IPreferenceWidget preferenceWidget = null;
				this.widgetsDictionary.TryGetValue(key, out preferenceWidget);
				if (preferenceWidget != null)
				{
					Widget widget = this.currentWidget.GetWidget();
					Widget widget2 = preferenceWidget.GetWidget();
					string text;
					if (widget != widget2 && !this.currentWidget.CanApply(out text))
					{
						this.treeview_preferences.Selection.SelectIter(this.oldTreeIter);
						if (!string.IsNullOrEmpty(text))
						{
							MessageBox.Show(text, MessageBoxImage.Warning, null, null);
						}
						return;
					}
					this.ChangeWidget(preferenceWidget);
					this.oldTreeIter = key;
				}
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002C7B File Offset: 0x00000E7B
		protected void HandleButttonCancelClicked(object sender, EventArgs e)
		{
			base.Respond(ResponseType.Cancel);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002C85 File Offset: 0x00000E85
		private void HandleCustomTitleBarCloseClicked(object sender, EventArgs e)
		{
			base.Respond(ResponseType.Close);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002C8F File Offset: 0x00000E8F
		private void HanldeDialogSizeAllocated(object o, SizeAllocatedArgs args)
		{
			base.VBox.BorderWidth = 1U;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002CA0 File Offset: 0x00000EA0
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.WidthRequest = 700;
			base.HeightRequest = 450;
			base.Name = "Modules.Communal.Preference.PreferencesDialog";
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog_VBox";
			this.vbox_window = new VBox();
			this.vbox_window.Name = "vbox_window";
			this.alignment_title = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_title.Name = "alignment_title";
			this.vbox_window.Add(this.alignment_title);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_window[this.alignment_title];
			boxChild.Position = 0;
			boxChild.Expand = false;
			this.evtbx_bg = new EventBox();
			this.evtbx_bg.Name = "evtbx_bg";
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 6;
			this.vbox_main.BorderWidth = 12U;
			this.hbox_main = new HBox();
			this.hbox_main.Name = "hbox_main";
			this.hbox_main.Spacing = 6;
			this.hbox_main.BorderWidth = 4U;
			this.evtbx_left = new EventBox();
			this.evtbx_left.WidthRequest = 150;
			this.evtbx_left.Name = "evtbx_left";
			this.alignment_left = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_left.Name = "alignment_left";
			this.alignment_left.BorderWidth = 2U;
			this.treeview_preferences = new TreeView();
			this.treeview_preferences.CanFocus = true;
			this.treeview_preferences.Name = "treeview_preferences";
			this.treeview_preferences.EnableSearch = false;
			this.treeview_preferences.HeadersVisible = false;
			this.alignment_left.Add(this.treeview_preferences);
			this.evtbx_left.Add(this.alignment_left);
			this.hbox_main.Add(this.evtbx_left);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_main[this.evtbx_left];
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			this.evtbx_right = new EventBox();
			this.evtbx_right.Name = "evtbx_right";
			this.hbox_main.Add(this.evtbx_right);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_main[this.evtbx_right];
			boxChild3.Position = 1;
			this.vbox_main.Add(this.hbox_main);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_main[this.hbox_main];
			boxChild4.Position = 0;
			this.evtbx_line = new EventBox();
			this.evtbx_line.HeightRequest = 1;
			this.evtbx_line.Name = "evtbx_line";
			this.vbox_main.Add(this.evtbx_line);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox_main[this.evtbx_line];
			boxChild5.Position = 1;
			boxChild5.Expand = false;
			this.hbox_bottomBtn = new HBox();
			this.hbox_bottomBtn.Name = "hbox_bottomBtn";
			this.hbox_bottomBtn.Spacing = 6;
			this.alignment_bottomBtnRight = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_bottomBtnRight.WidthRequest = 80;
			this.alignment_bottomBtnRight.HeightRequest = 26;
			this.alignment_bottomBtnRight.Name = "alignment_bottomBtnRight";
			this.hbox_bottomBtn.Add(this.alignment_bottomBtnRight);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox_bottomBtn[this.alignment_bottomBtnRight];
			boxChild6.PackType = PackType.End;
			boxChild6.Position = 0;
			boxChild6.Expand = false;
			this.alignment_bottomBtnLeft = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_bottomBtnLeft.WidthRequest = 80;
			this.alignment_bottomBtnLeft.HeightRequest = 26;
			this.alignment_bottomBtnLeft.Name = "alignment_bottomBtnLeft";
			this.hbox_bottomBtn.Add(this.alignment_bottomBtnLeft);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.hbox_bottomBtn[this.alignment_bottomBtnLeft];
			boxChild7.PackType = PackType.End;
			boxChild7.Position = 1;
			boxChild7.Expand = false;
			this.vbox_main.Add(this.hbox_bottomBtn);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox_main[this.hbox_bottomBtn];
			boxChild8.Position = 2;
			boxChild8.Expand = false;
			this.evtbx_bg.Add(this.vbox_main);
			this.vbox_window.Add(this.evtbx_bg);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.vbox_window[this.evtbx_bg];
			boxChild9.Position = 1;
			vbox.Add(this.vbox_window);
			Box.BoxChild boxChild10 = (Box.BoxChild)vbox[this.vbox_window];
			boxChild10.Position = 0;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog_ActionArea";
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			this.alignment_occupy = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_occupy.Name = "alignment_occupy";
			actionArea.Add(this.alignment_occupy);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.alignment_occupy];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 700;
			base.DefaultHeight = 450;
			actionArea.Hide();
			base.Hide();
		}

		// Token: 0x04000014 RID: 20
		private IPreferenceWidget currentWidget;

		// Token: 0x04000015 RID: 21
		private TreeIter oldTreeIter;

		// Token: 0x04000016 RID: 22
		private List<IPreferenceWidget> environmentWidgets = new List<IPreferenceWidget>();

		// Token: 0x04000017 RID: 23
		private Dictionary<TreeIter, IPreferenceWidget> widgetsDictionary = new Dictionary<TreeIter, IPreferenceWidget>();

		// Token: 0x04000018 RID: 24
		private VBox vbox_window;

		// Token: 0x04000019 RID: 25
		private Alignment alignment_title;

		// Token: 0x0400001A RID: 26
		private EventBox evtbx_bg;

		// Token: 0x0400001B RID: 27
		private VBox vbox_main;

		// Token: 0x0400001C RID: 28
		private HBox hbox_main;

		// Token: 0x0400001D RID: 29
		private EventBox evtbx_left;

		// Token: 0x0400001E RID: 30
		private Alignment alignment_left;

		// Token: 0x0400001F RID: 31
		private TreeView treeview_preferences;

		// Token: 0x04000020 RID: 32
		private EventBox evtbx_right;

		// Token: 0x04000021 RID: 33
		private EventBox evtbx_line;

		// Token: 0x04000022 RID: 34
		private HBox hbox_bottomBtn;

		// Token: 0x04000023 RID: 35
		private Alignment alignment_bottomBtnRight;

		// Token: 0x04000024 RID: 36
		private Alignment alignment_bottomBtnLeft;

		// Token: 0x04000025 RID: 37
		private Alignment alignment_occupy;
	}
}
