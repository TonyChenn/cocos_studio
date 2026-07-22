using System;
using System.Collections.Generic;
using CocoStudio.Core;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using Stetic;

namespace Modules.Communal.ProjectSetting
{
	// Token: 0x02000009 RID: 9
	public class ProjectSettingDialog : Dialog
	{
		// Token: 0x0600001B RID: 27 RVA: 0x000023E0 File Offset: 0x000005E0
		public ProjectSettingDialog(EnumProjectSetting initWidget = EnumProjectSetting.Default)
		{
			this.Build();
			Window defaultModalParent = MessageService.GetDefaultModalParent();
			this.SetToDialogStyle(defaultModalParent, true, false, true);
			this.CenterToParentWindow(ApplicationCurrent.MainWindow);
			this.InitWidgets();
			this.InitTreeView(initWidget);
			this.InitEvent();
			this.ChangeBtnPosion();
			this.InitMultiLanuage();
			this.buttonOk.Name = "MainButton";
			this.treeview_main.Name = "DarkTreeView";
			this.evtbx_left.ModifyBg(StateType.Normal, WindowStyle.LineDarkColor);
			this.evtbx_line.ModifyBg(StateType.Normal, WindowStyle.LineDarkColor);
			this.evtbx_right.Add(this.currentWidget.GetWidget());
			this.currentWidget.GetWidget().ShowAll();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000024B1 File Offset: 0x000006B1
		private void InitWidgets()
		{
			this.projSettingWidgets.Add(new PublishWidget());
			this.projSettingWidgets.Add(new PackageWidget());
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000024D4 File Offset: 0x000006D4
		private void InitTreeView(EnumProjectSetting initWidget)
		{
			TreeStore treeStore = new TreeStore(new Type[]
			{
				typeof(string)
			});
			TreeIter iter = default(TreeIter);
			foreach (IProjectSettingWidget projectSettingWidget in this.projSettingWidgets)
			{
				TreeIter treeIter = treeStore.AppendValues(new object[]
				{
					projectSettingWidget.DisplayName
				});
				this.widgetsDictionary.Add(treeIter, projectSettingWidget);
				if (this.currentWidget == null || projectSettingWidget.SettingID == initWidget)
				{
					this.currentWidget = projectSettingWidget;
					iter = treeIter;
				}
				if (projectSettingWidget.SubWidgets != null)
				{
					foreach (IProjectSettingWidget projectSettingWidget2 in projectSettingWidget.SubWidgets)
					{
						TreeIter treeIter2 = treeStore.AppendValues(treeIter, new object[]
						{
							projectSettingWidget2.DisplayName
						});
						this.widgetsDictionary.Add(treeIter2, projectSettingWidget2);
						if (this.currentWidget == null || projectSettingWidget2.SettingID == initWidget)
						{
							this.currentWidget = projectSettingWidget2;
							iter = treeIter2;
						}
					}
				}
			}
			this.oldTreeIter = iter;
			this.treeview_main.Model = treeStore;
			CellRendererText cell = new CellRendererText();
			TreeViewColumn treeViewColumn = new TreeViewColumn();
			treeViewColumn.PackStart(cell, false);
			treeViewColumn.AddAttribute(cell, "text", 0);
			treeViewColumn.Alignment = -40f;
			this.treeview_main.AppendColumn(treeViewColumn);
			this.treeview_main.HasTooltip = false;
			this.treeview_main.HeadersVisible = false;
			this.treeview_main.ExpandAll();
			this.treeview_main.Selection.SelectIter(iter);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000026A8 File Offset: 0x000008A8
		private void InitEvent()
		{
			this.buttonOk.Clicked += this.OnButtonOKClicked;
			this.buttonCancel.Clicked += this.OnButtonCancelClicked;
			this.treeview_main.CursorChanged += this.OnTreeCursorChanged;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000026FC File Offset: 0x000008FC
		private void ChangeBtnPosion()
		{
			if (Platform.IsWindows)
			{
				Box.BoxChild boxChild = (Box.BoxChild)this.hbox_bottomBtns[this.buttonOk];
				Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_bottomBtns[this.buttonOk];
				boxChild2.Position = 0;
				boxChild.Position = 1;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000274C File Offset: 0x0000094C
		private void InitMultiLanuage()
		{
			base.Title = LanguageInfo.ProjSetting;
			this.buttonOk.Label = LanguageInfo.Dialog_ButtonOK;
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonCancel;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000277C File Offset: 0x0000097C
		private void ChangeWidget(IProjectSettingWidget newWidget)
		{
			if (this.currentWidget != null)
			{
				this.evtbx_right.Remove(this.currentWidget.GetWidget());
			}
			this.evtbx_right.Add(newWidget.GetWidget());
			this.evtbx_right.ShowAll();
			this.currentWidget = newWidget;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000027CC File Offset: 0x000009CC
		private void OnButtonOKClicked(object sender, EventArgs e)
		{
			foreach (IProjectSettingWidget projectSettingWidget in this.widgetsDictionary.Values)
			{
				string text;
				if (!projectSettingWidget.CanApply(out text))
				{
					if (this.currentWidget != projectSettingWidget)
					{
						this.ChangeWidget(projectSettingWidget);
					}
					if (!string.IsNullOrEmpty(text))
					{
						MessageBox.Show(text, MessageBoxImage.Warning, null, null);
					}
					return;
				}
			}
			foreach (IProjectSettingWidget projectSettingWidget2 in this.widgetsDictionary.Values)
			{
				projectSettingWidget2.ApplySetting();
			}
			Services.ProjectsService.CurrentSolution.UserData.Save();
			Services.ProjectsService.CurrentSolution.Config.Save();
			base.Respond(ResponseType.Ok);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000028C4 File Offset: 0x00000AC4
		private void OnButtonCancelClicked(object sender, EventArgs e)
		{
			base.Respond(ResponseType.Cancel);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000028D0 File Offset: 0x00000AD0
		private void OnTreeCursorChanged(object sender, EventArgs e)
		{
			TreeIter key;
			if (this.treeview_main.Selection.GetSelected(out key))
			{
				IProjectSettingWidget projectSettingWidget = null;
				this.widgetsDictionary.TryGetValue(key, out projectSettingWidget);
				if (projectSettingWidget != null)
				{
					Widget widget = this.currentWidget.GetWidget();
					Widget widget2 = projectSettingWidget.GetWidget();
					string text;
					if (widget != widget2 && !this.currentWidget.CanApply(out text))
					{
						this.treeview_main.Selection.SelectIter(this.oldTreeIter);
						if (!string.IsNullOrEmpty(text))
						{
							MessageBox.Show(text, MessageBoxImage.Warning, null, null);
						}
						return;
					}
					this.ChangeWidget(projectSettingWidget);
					this.oldTreeIter = key;
				}
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002964 File Offset: 0x00000B64
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.WidthRequest = 780;
			base.HeightRequest = 580;
			base.Name = "Modules.Communal.ProjectSetting.ProjectSettingDialog";
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.BorderWidth = 8U;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog1_VBox";
			vbox.BorderWidth = 2U;
			this.hbox_main = new HBox();
			this.hbox_main.Name = "hbox_main";
			this.hbox_main.Spacing = 6;
			this.hbox_main.BorderWidth = 4U;
			this.evtbx_left = new EventBox();
			this.evtbx_left.WidthRequest = 150;
			this.evtbx_left.Name = "evtbx_left";
			this.alignment_left = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_left.Name = "alignment_left";
			this.alignment_left.LeftPadding = 2U;
			this.alignment_left.TopPadding = 2U;
			this.alignment_left.RightPadding = 2U;
			this.alignment_left.BottomPadding = 2U;
			this.treeview_main = new TreeView();
			this.treeview_main.CanFocus = true;
			this.treeview_main.Name = "treeview_main";
			this.treeview_main.EnableSearch = false;
			this.treeview_main.HeadersVisible = false;
			this.alignment_left.Add(this.treeview_main);
			this.evtbx_left.Add(this.alignment_left);
			this.hbox_main.Add(this.evtbx_left);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox_main[this.evtbx_left];
			boxChild.Position = 0;
			boxChild.Expand = false;
			this.evtbx_right = new EventBox();
			this.evtbx_right.Name = "evtbx_right";
			this.hbox_main.Add(this.evtbx_right);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_main[this.evtbx_right];
			boxChild2.Position = 1;
			vbox.Add(this.hbox_main);
			Box.BoxChild boxChild3 = (Box.BoxChild)vbox[this.hbox_main];
			boxChild3.Position = 0;
			this.evtbx_line = new EventBox();
			this.evtbx_line.HeightRequest = 1;
			this.evtbx_line.Name = "evtbx_line";
			vbox.Add(this.evtbx_line);
			Box.BoxChild boxChild4 = (Box.BoxChild)vbox[this.evtbx_line];
			boxChild4.Position = 1;
			boxChild4.Expand = false;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog1_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 5U;
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			this.alignment_bottomBtns = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_bottomBtns.Name = "alignment_bottomBtns";
			this.hbox_bottomBtns = new HBox();
			this.hbox_bottomBtns.Name = "hbox_bottomBtns";
			this.hbox_bottomBtns.Spacing = 6;
			this.buttonOk = new Button();
			this.buttonOk.WidthRequest = 80;
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Label = Catalog.GetString("确定");
			this.hbox_bottomBtns.Add(this.buttonOk);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.hbox_bottomBtns[this.buttonOk];
			boxChild5.PackType = PackType.End;
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.buttonCancel = new Button();
			this.buttonCancel.WidthRequest = 80;
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Label = Catalog.GetString("取消");
			this.hbox_bottomBtns.Add(this.buttonCancel);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox_bottomBtns[this.buttonCancel];
			boxChild6.PackType = PackType.End;
			boxChild6.Position = 1;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.alignment_bottomBtns.Add(this.hbox_bottomBtns);
			actionArea.Add(this.alignment_bottomBtns);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.alignment_bottomBtns];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 780;
			base.DefaultHeight = 580;
			base.Hide();
		}

		// Token: 0x0400000C RID: 12
		private IProjectSettingWidget currentWidget;

		// Token: 0x0400000D RID: 13
		private TreeIter oldTreeIter;

		// Token: 0x0400000E RID: 14
		private List<IProjectSettingWidget> projSettingWidgets = new List<IProjectSettingWidget>();

		// Token: 0x0400000F RID: 15
		private Dictionary<TreeIter, IProjectSettingWidget> widgetsDictionary = new Dictionary<TreeIter, IProjectSettingWidget>();

		// Token: 0x04000010 RID: 16
		private HBox hbox_main;

		// Token: 0x04000011 RID: 17
		private EventBox evtbx_left;

		// Token: 0x04000012 RID: 18
		private Alignment alignment_left;

		// Token: 0x04000013 RID: 19
		private TreeView treeview_main;

		// Token: 0x04000014 RID: 20
		private EventBox evtbx_right;

		// Token: 0x04000015 RID: 21
		private EventBox evtbx_line;

		// Token: 0x04000016 RID: 22
		private Alignment alignment_bottomBtns;

		// Token: 0x04000017 RID: 23
		private HBox hbox_bottomBtns;

		// Token: 0x04000018 RID: 24
		private Button buttonOk;

		// Token: 0x04000019 RID: 25
		private Button buttonCancel;
	}
}
