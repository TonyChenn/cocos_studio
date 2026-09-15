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
	public class ProjectSettingDialog : Dialog
	{
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

		private void InitWidgets()
		{
			this.projSettingWidgets.Add(new PublishWidget());
			this.projSettingWidgets.Add(new PackageWidget());
		}

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

		private void InitEvent()
		{
			this.buttonOk.Clicked += this.OnButtonOKClicked;
			this.buttonCancel.Clicked += this.OnButtonCancelClicked;
			this.treeview_main.CursorChanged += this.OnTreeCursorChanged;
		}

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

		private void InitMultiLanuage()
		{
			base.Title = LanguageInfo.ProjSetting;
			this.buttonOk.Label = LanguageInfo.Dialog_ButtonOK;
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonCancel;
		}

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

		private void OnButtonCancelClicked(object sender, EventArgs e)
		{
			base.Respond(ResponseType.Cancel);
		}

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

		private IProjectSettingWidget currentWidget;

		private TreeIter oldTreeIter;

		private List<IProjectSettingWidget> projSettingWidgets = new List<IProjectSettingWidget>();

		private Dictionary<TreeIter, IProjectSettingWidget> widgetsDictionary = new Dictionary<TreeIter, IProjectSettingWidget>();

		private HBox hbox_main;

		private EventBox evtbx_left;

		private Alignment alignment_left;

		private TreeView treeview_main;

		private EventBox evtbx_right;

		private EventBox evtbx_line;

		private Alignment alignment_bottomBtns;

		private HBox hbox_bottomBtns;

		private Button buttonOk;

		private Button buttonCancel;
	}
}
