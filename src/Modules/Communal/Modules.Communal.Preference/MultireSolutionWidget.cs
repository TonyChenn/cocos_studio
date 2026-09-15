using System;
using System.Collections.Generic;
using System.ComponentModel;
using CocoStudio.Basic;
using CocoStudio.Model;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using Stetic;

namespace Modules.Communal.Preference
{
	[ToolboxItem(true)]
	public class MultireSolutionWidget : Bin, IPreferenceWidget
	{
		public EnumPreferenceSetting SettingID
		{
			get
			{
				return EnumPreferenceSetting.Resolution;
			}
		}

		public string DisplayName
		{
			get
			{
				return LanguageInfo.Dialog_Resolution;
			}
		}

		public MultireSolutionWidget()
		{
			this.Build();
			this.InitEvent();
			this.InitTreeView();
			this.InitData();
			this.InitLanguage();
			this.InitStyle();
		}

		private void InitEvent()
		{
			this.btnAdd.Clicked += this.HandleButtonAddClicked;
			this.btnModify.Clicked += this.HandleButtonEditClicked;
			this.btnUp.Clicked += this.HandleButtonUpClicked;
			this.btnDown.Clicked += this.HandleButtonDownClicked;
			this.btnDel.Clicked += this.HandleButtonDeleteClicked;
			this.btnReset.Clicked += this.HandleButtonResetClicked;
			this.chbAll.Clicked += this.HandleSelectAllCheckbuttonClicked;
		}

		private void InitTreeView()
		{
			this.treeStore = new ListStore(new Type[]
			{
				typeof(bool),
				typeof(string),
				typeof(string)
			});
			this.treeview_resolution.Model = this.treeStore;
			this.treeview_resolution.HeadersVisible = false;
			CellRendererToggle cellRendererToggle = new CellRendererToggle();
			CellRendererText cellRendererText = new CellRendererText();
			cellRendererText.Editable = false;
			CellRendererText cellRendererText2 = new CellRendererText();
			cellRendererText2.Editable = false;
			TreeViewColumn treeViewColumn = new TreeViewColumn();
			cellRendererToggle.Toggled += this.HandleItemToggle;
			treeViewColumn.PackStart(cellRendererToggle, false);
			treeViewColumn.AddAttribute(cellRendererToggle, "active", 0);
			this.treeview_resolution.AppendColumn(treeViewColumn);
			treeViewColumn.Clickable = true;
			this.treeview_resolution.AppendColumn(LanguageInfo.Display_Name, cellRendererText, new object[]
			{
				"markup",
				1
			});
			cellRendererText.Width = 180;
			this.treeview_resolution.AppendColumn(LanguageInfo.Dialog_Resolution, cellRendererText2, new object[]
			{
				"markup",
				2
			});
		}

		private void InitData()
		{
			this.configList.AddRange(Option.UserConfig.ResolutionList);
			this.configList.Sort((ResolutionConfig x, ResolutionConfig y) => x.Order.CompareTo(y.Order));
			this.RefreshList();
			foreach (ResolutionConfig resolutionConfig in this.configList)
			{
				this.configListCache.Add(resolutionConfig.Clone() as ResolutionConfig);
			}
			this.treeview_resolution.Selection.SelectPath(new TreePath("0"));
			this.RefreshSelectAllCheckBox();
		}

		private void InitLanguage()
		{
			this.btnAdd.Label = LanguageInfo.Dialog_ButtonAdd + "...";
			this.btnModify.Label = LanguageInfo.Dialog_ButtonEdit + "...";
			this.btnUp.Label = LanguageInfo.Dialog_ButtonMoveUp;
			this.btnDown.Label = LanguageInfo.Dialog_ButtonMoveDown;
			this.btnDel.Label = LanguageInfo.Dialog_ButtonRemove;
			this.btnReset.Label = LanguageInfo.Property_Reset;
			this.label_name.Text = LanguageInfo.Display_Name;
			this.label_resolution.Text = LanguageInfo.Dialog_Resolution;
			this.chbAll.TooltipText = LanguageInfo.Dialog_ResolutionTooltip;
		}

		private void InitStyle()
		{
			this.chbAll.Name = "WhiteCheckButton";
			this.treeview_resolution.Name = "DarkTreeView";
			this.evtbx_fakeTitle.ModifyBg(StateType.Normal, new Color(101, 103, 110));
			this.evtbx_treeBorder.ModifyBg(StateType.Normal, WindowStyle.LineDarkColor);
		}

		private void RefreshList()
		{
			this.treeStore.Clear();
			foreach (ResolutionConfig resolutionConfig in this.configList)
			{
				this.treeStore.AppendValues(new object[]
				{
					resolutionConfig.IsSelected,
					this.ConvertToDisplayStr(resolutionConfig.Name),
					resolutionConfig.Size
				});
			}
		}

		private string ConvertToDisplayStr(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}
			return str.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
		}

		private string ConvertToRealStr(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}
			return str.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">");
		}

		private void RefreshSelectAllCheckBox()
		{
			bool active = true;
			foreach (ResolutionConfig resolutionConfig in this.configList)
			{
				if (!resolutionConfig.IsSelected)
				{
					active = false;
					break;
				}
			}
			this.enableSelectAll = false;
			this.chbAll.Active = active;
			this.enableSelectAll = true;
		}

		public void ApplySetting()
		{
			bool flag = false;
			if (this.configList.Count != this.configListCache.Count)
			{
				flag = true;
			}
			else
			{
				for (int i = 0; i < this.configList.Count; i++)
				{
					if (!this.configList[i].Equals(this.configListCache[i]))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				Option.UserConfig.ResolutionList = this.configList;
			}
		}

		public bool CanApply(out string output)
		{
			bool flag = false;
			foreach (ResolutionConfig resolutionConfig in this.configList)
			{
				if (resolutionConfig.IsSelected)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				output = LanguageInfo.MessageBox250_atLeastOneResolution;
				return false;
			}
			output = "";
			return true;
		}

		public Widget GetWidget()
		{
			return this;
		}

		private void HandleItemToggle(object o, ToggledArgs args)
		{
			TreeIter iter;
			if (this.treeStore.GetIterFromString(out iter, args.Path))
			{
				bool flag = !(bool)this.treeStore.GetValue(iter, 0);
				this.treeStore.SetValue(iter, 0, flag);
				string size = (string)this.treeStore.GetValue(iter, 2);
				string name = (string)this.treeStore.GetValue(iter, 1);
				this.configList.Find((ResolutionConfig w) => w.Size == size && w.Name == this.ConvertToRealStr(name)).IsSelected = flag;
			}
			this.RefreshSelectAllCheckBox();
		}

		private void HandleSelectAllCheckbuttonClicked(object sender, EventArgs e)
		{
			if (!this.enableSelectAll)
			{
				return;
			}
			TreePath[] selectedRows = this.treeview_resolution.Selection.GetSelectedRows();
			if (selectedRows[0] == null)
			{
				return;
			}
			int num = Convert.ToInt32(selectedRows[0].ToString());
			this.treeStore.Clear();
			this.configList.ForEach(delegate(ResolutionConfig w)
			{
				w.IsSelected = this.chbAll.Active;
			});
			foreach (ResolutionConfig resolutionConfig in this.configList)
			{
				this.treeStore.AppendValues(new object[]
				{
					resolutionConfig.IsSelected,
					resolutionConfig.Name,
					resolutionConfig.Size
				});
			}
			this.treeview_resolution.Selection.SelectPath(new TreePath(num.ToString()));
		}

		private void HandleButtonResetClicked(object sender, EventArgs e)
		{
			this.configList = ResolutionConfig.CreateDefaultList();
			this.RefreshList();
			this.treeview_resolution.Selection.SelectPath(new TreePath("0"));
			this.RefreshSelectAllCheckBox();
		}

		private void HandleButtonDeleteClicked(object sender, EventArgs e)
		{
			TreeModel treeModel;
			TreeIter iter;
			this.treeview_resolution.Selection.GetSelected(out treeModel, out iter);
			TreePath[] selectedRows = this.treeview_resolution.Selection.GetSelectedRows();
			if (selectedRows[0] == null)
			{
				return;
			}
			int num = Convert.ToInt32(selectedRows[0].ToString());
			if (this.configList.Count == 1)
			{
				return;
			}
			string rep = (string)this.treeStore.GetValue(iter, 2);
			this.configList.Remove(this.configList.Find((ResolutionConfig w) => w.Size == rep));
			this.treeStore.Remove(ref iter);
			int num2 = num - 1;
			num2 = ((num2 < 0) ? 0 : num2);
			this.treeview_resolution.Selection.SelectPath(new TreePath(num2.ToString()));
			this.RefreshSelectAllCheckBox();
		}

		private void HandleButtonDownClicked(object sender, EventArgs e)
		{
			TreePath[] selectedRows = this.treeview_resolution.Selection.GetSelectedRows();
			if (selectedRows[0] == null)
			{
				return;
			}
			int num = Convert.ToInt32(selectedRows[0].ToString());
			if (num == this.configList.Count - 1)
			{
				return;
			}
			int order = this.configList[num].Order;
			this.configList[num].Order = this.configList[num + 1].Order;
			this.configList[num + 1].Order = order;
			this.configList.Sort((ResolutionConfig x, ResolutionConfig y) => x.Order.CompareTo(y.Order));
			this.RefreshList();
			this.treeview_resolution.Selection.SelectPath(new TreePath((num + 1).ToString()));
		}

		private void HandleButtonUpClicked(object sender, EventArgs e)
		{
			TreePath[] selectedRows = this.treeview_resolution.Selection.GetSelectedRows();
			if (selectedRows[0] == null)
			{
				return;
			}
			int num = Convert.ToInt32(selectedRows[0].ToString());
			if (num == 0)
			{
				return;
			}
			int order = this.configList[num].Order;
			this.configList[num].Order = this.configList[num - 1].Order;
			this.configList[num - 1].Order = order;
			this.configList.Sort((ResolutionConfig x, ResolutionConfig y) => x.Order.CompareTo(y.Order));
			this.RefreshList();
			this.treeview_resolution.Selection.SelectPath(new TreePath((num - 1).ToString()));
		}

		private void HandleButtonEditClicked(object sender, EventArgs e)
		{
			TreePath[] selectedRows = this.treeview_resolution.Selection.GetSelectedRows();
			if (selectedRows[0] == null)
			{
				return;
			}
			int index = Convert.ToInt32(selectedRows[0].ToString());
			TreeIter iter;
			if (this.treeStore.GetIterFromString(out iter, index.ToString()))
			{
				string text = (string)this.treeStore.GetValue(iter, 1);
				string sizeStr = (string)this.treeStore.GetValue(iter, 2);
				SizeF sizeF = sizeStr.ConvertToSize();
				ResolutionConfig resolutionConfig = this.configList[index];
				ResolutionSettingDialog resolutionSettingDialog = new ResolutionSettingDialog((int)sizeF.Width, (int)sizeF.Height, resolutionConfig.Name);
				resolutionSettingDialog.Title = LanguageInfo.ScreeSize_Dialog_ModifyTitle;
				if (resolutionSettingDialog.Run() == -5)
				{
					resolutionConfig.Name = resolutionSettingDialog.ResolutionName;
					resolutionConfig.Width = resolutionSettingDialog.Width;
					resolutionConfig.Height = resolutionSettingDialog.Height;
					this.RefreshList();
					this.treeview_resolution.Selection.SelectPath(new TreePath(index.ToString()));
				}
				resolutionSettingDialog.Destroy();
			}
		}

		private void HandleButtonAddClicked(object sender, EventArgs e)
		{
			ResolutionSettingDialog resolutionSettingDialog = new ResolutionSettingDialog(100, 100, null);
			resolutionSettingDialog.Title = LanguageInfo.ScreeSize_Dialog_NewTitle;
			if (resolutionSettingDialog.Run() == -5)
			{
				this.configList.Add(new ResolutionConfig
				{
					IsSelected = true,
					Name = resolutionSettingDialog.ResolutionName,
					Order = this.configList[this.configList.Count - 1].Order + 1,
					Width = resolutionSettingDialog.Width,
					Height = resolutionSettingDialog.Height
				});
				int num = this.configList.Count - 1;
				this.RefreshList();
				this.treeview_resolution.Selection.SelectPath(new TreePath(num.ToString()));
			}
			resolutionSettingDialog.Destroy();
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.Preference.MultireSolutionWidget";
			this.alignment_main = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_main.Name = "alignment_main";
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 6;
			this.hbox_top = new HBox();
			this.hbox_top.Name = "hbox_top";
			this.hbox_top.Spacing = 10;
			this.evtbx_treeBorder = new EventBox();
			this.evtbx_treeBorder.Name = "evtbx_treeBorder";
			this.vbox_tree = new VBox();
			this.vbox_tree.Name = "vbox_tree";
			this.vbox_tree.BorderWidth = 2U;
			this.evtbx_fakeTitle = new EventBox();
			this.evtbx_fakeTitle.HeightRequest = 25;
			this.evtbx_fakeTitle.Name = "evtbx_fakeTitle";
			this.hbox_fakeTitle = new HBox();
			this.hbox_fakeTitle.Name = "hbox_fakeTitle";
			this.alignment_selectAll = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_selectAll.Name = "alignment_selectAll";
			this.alignment_selectAll.LeftPadding = 1U;
			this.chbAll = new CheckButton();
			this.chbAll.CanFocus = true;
			this.chbAll.Name = "chbAll";
			this.chbAll.Label = "";
			this.chbAll.DrawIndicator = true;
			this.chbAll.UseUnderline = true;
			this.alignment_selectAll.Add(this.chbAll);
			this.hbox_fakeTitle.Add(this.alignment_selectAll);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox_fakeTitle[this.alignment_selectAll];
			boxChild.Position = 0;
			boxChild.Expand = false;
			this.alignment_name = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_name.WidthRequest = 184;
			this.alignment_name.Name = "alignment_name";
			this.label_name = new Label();
			this.label_name.Name = "label_name";
			this.label_name.Xalign = 0f;
			this.label_name.LabelProp = Catalog.GetString("名称");
			this.alignment_name.Add(this.label_name);
			this.hbox_fakeTitle.Add(this.alignment_name);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_fakeTitle[this.alignment_name];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			this.label_resolution = new Label();
			this.label_resolution.Name = "label_resolution";
			this.label_resolution.LabelProp = Catalog.GetString("分辨率");
			this.hbox_fakeTitle.Add(this.label_resolution);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_fakeTitle[this.label_resolution];
			boxChild3.Position = 2;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.evtbx_fakeTitle.Add(this.hbox_fakeTitle);
			this.vbox_tree.Add(this.evtbx_fakeTitle);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_tree[this.evtbx_fakeTitle];
			boxChild4.Position = 0;
			boxChild4.Expand = false;
			this.GtkScrolledWindow_tree = new ScrolledWindow();
			this.GtkScrolledWindow_tree.Name = "GtkScrolledWindow_tree";
			this.GtkScrolledWindow_tree.ShadowType = ShadowType.In;
			this.treeview_resolution = new TreeView();
			this.treeview_resolution.CanFocus = true;
			this.treeview_resolution.Name = "treeview_resolution";
			this.GtkScrolledWindow_tree.Add(this.treeview_resolution);
			this.vbox_tree.Add(this.GtkScrolledWindow_tree);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox_tree[this.GtkScrolledWindow_tree];
			boxChild5.Position = 1;
			this.evtbx_treeBorder.Add(this.vbox_tree);
			this.hbox_top.Add(this.evtbx_treeBorder);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox_top[this.evtbx_treeBorder];
			boxChild6.Position = 0;
			this.vbox_rightBtns = new VBox();
			this.vbox_rightBtns.Name = "vbox_rightBtns";
			this.vbox_rightBtns.Spacing = 15;
			this.btnAdd = new Button();
			this.btnAdd.WidthRequest = 75;
			this.btnAdd.HeightRequest = 22;
			this.btnAdd.CanFocus = true;
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseUnderline = true;
			this.btnAdd.Label = Catalog.GetString("添加");
			this.vbox_rightBtns.Add(this.btnAdd);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.vbox_rightBtns[this.btnAdd];
			boxChild7.Position = 0;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			this.btnModify = new Button();
			this.btnModify.WidthRequest = 75;
			this.btnModify.HeightRequest = 22;
			this.btnModify.CanFocus = true;
			this.btnModify.Name = "btnModify";
			this.btnModify.UseUnderline = true;
			this.btnModify.Label = Catalog.GetString("编辑");
			this.vbox_rightBtns.Add(this.btnModify);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox_rightBtns[this.btnModify];
			boxChild8.Position = 1;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			this.btnUp = new Button();
			this.btnUp.WidthRequest = 75;
			this.btnUp.HeightRequest = 22;
			this.btnUp.CanFocus = true;
			this.btnUp.Name = "btnUp";
			this.btnUp.UseUnderline = true;
			this.btnUp.Label = Catalog.GetString("上移");
			this.vbox_rightBtns.Add(this.btnUp);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.vbox_rightBtns[this.btnUp];
			boxChild9.Position = 2;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			this.btnDown = new Button();
			this.btnDown.WidthRequest = 75;
			this.btnDown.HeightRequest = 22;
			this.btnDown.CanFocus = true;
			this.btnDown.Name = "btnDown";
			this.btnDown.UseUnderline = true;
			this.btnDown.Label = Catalog.GetString("下移");
			this.vbox_rightBtns.Add(this.btnDown);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.vbox_rightBtns[this.btnDown];
			boxChild10.Position = 3;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			this.btnDel = new Button();
			this.btnDel.WidthRequest = 75;
			this.btnDel.HeightRequest = 22;
			this.btnDel.CanFocus = true;
			this.btnDel.Name = "btnDel";
			this.btnDel.UseUnderline = true;
			this.btnDel.Label = Catalog.GetString("移除");
			this.vbox_rightBtns.Add(this.btnDel);
			Box.BoxChild boxChild11 = (Box.BoxChild)this.vbox_rightBtns[this.btnDel];
			boxChild11.Position = 4;
			boxChild11.Expand = false;
			boxChild11.Fill = false;
			this.btnReset = new Button();
			this.btnReset.WidthRequest = 75;
			this.btnReset.HeightRequest = 22;
			this.btnReset.CanFocus = true;
			this.btnReset.Name = "btnReset";
			this.btnReset.UseUnderline = true;
			this.btnReset.Label = Catalog.GetString("重置");
			this.vbox_rightBtns.Add(this.btnReset);
			Box.BoxChild boxChild12 = (Box.BoxChild)this.vbox_rightBtns[this.btnReset];
			boxChild12.Position = 5;
			boxChild12.Expand = false;
			boxChild12.Fill = false;
			this.hbox_top.Add(this.vbox_rightBtns);
			Box.BoxChild boxChild13 = (Box.BoxChild)this.hbox_top[this.vbox_rightBtns];
			boxChild13.Position = 1;
			boxChild13.Expand = false;
			boxChild13.Fill = false;
			this.vbox_main.Add(this.hbox_top);
			Box.BoxChild boxChild14 = (Box.BoxChild)this.vbox_main[this.hbox_top];
			boxChild14.Position = 0;
			this.alignment_main.Add(this.vbox_main);
			base.Add(this.alignment_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		private ListStore treeStore;

		private List<ResolutionConfig> configList = new List<ResolutionConfig>();

		private List<ResolutionConfig> configListCache = new List<ResolutionConfig>();

		private bool enableSelectAll = true;

		private Alignment alignment_main;

		private VBox vbox_main;

		private HBox hbox_top;

		private EventBox evtbx_treeBorder;

		private VBox vbox_tree;

		private EventBox evtbx_fakeTitle;

		private HBox hbox_fakeTitle;

		private Alignment alignment_selectAll;

		private CheckButton chbAll;

		private Alignment alignment_name;

		private Label label_name;

		private Label label_resolution;

		private ScrolledWindow GtkScrolledWindow_tree;

		private TreeView treeview_resolution;

		private VBox vbox_rightBtns;

		private Button btnAdd;

		private Button btnModify;

		private Button btnUp;

		private Button btnDown;

		private Button btnDel;

		private Button btnReset;
	}
}
