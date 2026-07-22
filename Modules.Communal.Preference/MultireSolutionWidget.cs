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
	// Token: 0x02000013 RID: 19
	[ToolboxItem(true)]
	public class MultireSolutionWidget : Bin, IPreferenceWidget
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00007584 File Offset: 0x00005784
		public EnumPreferenceSetting SettingID
		{
			get
			{
				return EnumPreferenceSetting.Resolution;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00007587 File Offset: 0x00005787
		public string DisplayName
		{
			get
			{
				return LanguageInfo.Dialog_Resolution;
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00007590 File Offset: 0x00005790
		public MultireSolutionWidget()
		{
			this.Build();
			this.InitEvent();
			this.InitTreeView();
			this.InitData();
			this.InitLanguage();
			this.InitStyle();
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000075E4 File Offset: 0x000057E4
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

		// Token: 0x06000081 RID: 129 RVA: 0x00007694 File Offset: 0x00005894
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

		// Token: 0x06000082 RID: 130 RVA: 0x000077EC File Offset: 0x000059EC
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

		// Token: 0x06000083 RID: 131 RVA: 0x000078B4 File Offset: 0x00005AB4
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

		// Token: 0x06000084 RID: 132 RVA: 0x00007968 File Offset: 0x00005B68
		private void InitStyle()
		{
			this.chbAll.Name = "WhiteCheckButton";
			this.treeview_resolution.Name = "DarkTreeView";
			this.evtbx_fakeTitle.ModifyBg(StateType.Normal, new Color(101, 103, 110));
			this.evtbx_treeBorder.ModifyBg(StateType.Normal, WindowStyle.LineDarkColor);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000079C0 File Offset: 0x00005BC0
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

		// Token: 0x06000086 RID: 134 RVA: 0x00007A54 File Offset: 0x00005C54
		private string ConvertToDisplayStr(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}
			return str.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00007A8E File Offset: 0x00005C8E
		private string ConvertToRealStr(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}
			return str.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">");
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00007AC8 File Offset: 0x00005CC8
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

		// Token: 0x06000089 RID: 137 RVA: 0x00007B3C File Offset: 0x00005D3C
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

		// Token: 0x0600008A RID: 138 RVA: 0x00007BB4 File Offset: 0x00005DB4
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

		// Token: 0x0600008B RID: 139 RVA: 0x00007C24 File Offset: 0x00005E24
		public Widget GetWidget()
		{
			return this;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00007C64 File Offset: 0x00005E64
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

		// Token: 0x0600008D RID: 141 RVA: 0x00007D20 File Offset: 0x00005F20
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

		// Token: 0x0600008E RID: 142 RVA: 0x00007E14 File Offset: 0x00006014
		private void HandleButtonResetClicked(object sender, EventArgs e)
		{
			this.configList = ResolutionConfig.CreateDefaultList();
			this.RefreshList();
			this.treeview_resolution.Selection.SelectPath(new TreePath("0"));
			this.RefreshSelectAllCheckBox();
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00007E64 File Offset: 0x00006064
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

		// Token: 0x06000090 RID: 144 RVA: 0x00007F64 File Offset: 0x00006164
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

		// Token: 0x06000091 RID: 145 RVA: 0x00008064 File Offset: 0x00006264
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

		// Token: 0x06000092 RID: 146 RVA: 0x00008134 File Offset: 0x00006334
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

		// Token: 0x06000093 RID: 147 RVA: 0x00008244 File Offset: 0x00006444
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

		// Token: 0x06000094 RID: 148 RVA: 0x0000830C File Offset: 0x0000650C
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

		// Token: 0x0400009D RID: 157
		private ListStore treeStore;

		// Token: 0x0400009E RID: 158
		private List<ResolutionConfig> configList = new List<ResolutionConfig>();

		// Token: 0x0400009F RID: 159
		private List<ResolutionConfig> configListCache = new List<ResolutionConfig>();

		// Token: 0x040000A0 RID: 160
		private bool enableSelectAll = true;

		// Token: 0x040000A1 RID: 161
		private Alignment alignment_main;

		// Token: 0x040000A2 RID: 162
		private VBox vbox_main;

		// Token: 0x040000A3 RID: 163
		private HBox hbox_top;

		// Token: 0x040000A4 RID: 164
		private EventBox evtbx_treeBorder;

		// Token: 0x040000A5 RID: 165
		private VBox vbox_tree;

		// Token: 0x040000A6 RID: 166
		private EventBox evtbx_fakeTitle;

		// Token: 0x040000A7 RID: 167
		private HBox hbox_fakeTitle;

		// Token: 0x040000A8 RID: 168
		private Alignment alignment_selectAll;

		// Token: 0x040000A9 RID: 169
		private CheckButton chbAll;

		// Token: 0x040000AA RID: 170
		private Alignment alignment_name;

		// Token: 0x040000AB RID: 171
		private Label label_name;

		// Token: 0x040000AC RID: 172
		private Label label_resolution;

		// Token: 0x040000AD RID: 173
		private ScrolledWindow GtkScrolledWindow_tree;

		// Token: 0x040000AE RID: 174
		private TreeView treeview_resolution;

		// Token: 0x040000AF RID: 175
		private VBox vbox_rightBtns;

		// Token: 0x040000B0 RID: 176
		private Button btnAdd;

		// Token: 0x040000B1 RID: 177
		private Button btnModify;

		// Token: 0x040000B2 RID: 178
		private Button btnUp;

		// Token: 0x040000B3 RID: 179
		private Button btnDown;

		// Token: 0x040000B4 RID: 180
		private Button btnDel;

		// Token: 0x040000B5 RID: 181
		private Button btnReset;
	}
}
