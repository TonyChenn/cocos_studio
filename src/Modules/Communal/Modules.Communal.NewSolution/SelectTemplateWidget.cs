using System;
using System.Collections.Generic;
using System.ComponentModel;
using CocoStudio.Basic;
using Gtk;
using Mono.Unix;
using Stetic;

namespace Modules.Communal.NewSolution
{
	[ToolboxItem(true)]
	public class SelectTemplateWidget : Bin
	{
		public event EventHandler<TemplateSelectedArgs> TemplateSelected;

		public ISolutionTemplate CurrentTemplate { get; private set; }

		public SelectTemplateWidget()
		{
			this.Build();
			this.InitStyles();
			this.InitRadioItems();
		}

		private void InitStyles()
		{
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				this.evtbx_vSeperator.ModifyBg(StateType.Normal, NewSolutionStyles.Launcher_BorderGray);
				this.evtbx_rightHSeperator.ModifyBg(StateType.Normal, NewSolutionStyles.Launcher_BorderGray);
			}
			else
			{
				this.evtbx_vSeperator.ModifyBg(StateType.Normal, NewSolutionStyles.Studio_HoverDarkGray);
				this.evtbx_rightHSeperator.ModifyBg(StateType.Normal, NewSolutionStyles.Studio_HoverDarkGray);
				this.evtbx_leftListBg.ModifyBg(StateType.Normal, NewSolutionStyles.Studio_BgGray);
			}
			this.label_itemDes.SetFontSize(12.0);
			this.label_itemTitle.SetFontSize(12.0);
		}

		private void InitRadioItems()
		{
			List<RadioItemWidget> solutionTemplates = TemplateManager.Instance.GetSolutionTemplates();
			foreach (RadioItemWidget radioItemWidget in solutionTemplates)
			{
				radioItemWidget.DoubleClicked += this.OnCardWidgetDoubleClicked;
			}
			RadioGroup radioGroup = new RadioGroup("未命名");
			radioGroup.RadioItemSelected += this.OnGroupTypeSelected;
			List<RadioItemWidget> soltuionGroups = TemplateManager.Instance.GetSoltuionGroups(solutionTemplates);
			foreach (RadioItemWidget radioItemWidget2 in soltuionGroups)
			{
				GroupTypeContent groupTypeContent = radioItemWidget2.GtkContent as GroupTypeContent;
				groupTypeContent.Group.RadioItemSelected += this.OnSolutionItemSelected;
				radioGroup.AddItem(radioItemWidget2);
				this.vbox_leftList.PackStart(radioItemWidget2, false, false, 0U);
				radioItemWidget2.Show();
			}
			soltuionGroups[0].Select();
		}

		private void RefreshItemsTable(RadioGroup itemGroup)
		{
			foreach (object obj in this.table_slnItems)
			{
				VBox vbox = (VBox)obj;
				foreach (object obj2 in vbox)
				{
					Widget widget = (Widget)obj2;
					vbox.Remove(widget);
				}
				this.table_slnItems.Remove(vbox);
			}
			uint num = 0U;
			uint num2 = 0U;
			foreach (IRadioItem radioItem in itemGroup.RadioItemList)
			{
				Widget gtkWidget = radioItem.GetGtkWidget();
				VBox vbox2 = new VBox();
				vbox2.Spacing = 0;
				vbox2.Add(gtkWidget);
				Box.BoxChild boxChild = (Box.BoxChild)vbox2[gtkWidget];
				boxChild.Expand = false;
				boxChild.Fill = false;
				this.table_slnItems.Attach(vbox2, num, num + 1U, num2, num2 + 1U, (AttachOptions)0, (AttachOptions)0, 0U, 0U);
				Table.TableChild tableChild = (Table.TableChild)this.table_slnItems[vbox2];
				tableChild.YOptions = AttachOptions.Fill;
				vbox2.Show();
				gtkWidget.Show();
				num += 1U;
				if (num >= 4U)
				{
					num = 0U;
					num2 += 1U;
				}
			}
			itemGroup.RadioItemList[0].Select();
		}

		private void OnSolutionItemSelected(object sender, RadioItemArgs e)
		{
			this.CurrentTemplate = (e.RadioItem.Tag as ISolutionTemplate);
			this.label_itemDes.Text = this.CurrentTemplate.Info.Description;
			this.label_itemTitle.Text = this.CurrentTemplate.Info.Name;
		}

		private void OnGroupTypeSelected(object sender, RadioItemArgs e)
		{
			GroupTypeContent groupTypeContent = e.RadioItem.GtkContent as GroupTypeContent;
			this.RefreshItemsTable(groupTypeContent.Group);
		}

		private void OnCardWidgetDoubleClicked(object sender, RadioItemArgs e)
		{
			this.CurrentTemplate = (e.RadioItem.Tag as ISolutionTemplate);
			if (this.TemplateSelected != null)
			{
				this.TemplateSelected(this, new TemplateSelectedArgs(this.CurrentTemplate));
			}
		}

		protected void OnRightVobxSizeAllocated(object o, SizeAllocatedArgs args)
		{
			this.label_itemDes.WidthRequest = args.Allocation.Width;
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.NewSolution.SelectTemplateWidget";
			this.hbox_main = new HBox();
			this.hbox_main.Name = "hbox_main";
			this.evtbx_leftListBg = new EventBox();
			this.evtbx_leftListBg.Name = "evtbx_leftListBg";
			this.alignment_leftList = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_leftList.Name = "alignment_leftList";
			this.alignment_leftList.TopPadding = 15U;
			this.vbox_leftList = new VBox();
			this.vbox_leftList.Name = "vbox_leftList";
			this.alignment_leftList.Add(this.vbox_leftList);
			this.evtbx_leftListBg.Add(this.alignment_leftList);
			this.hbox_main.Add(this.evtbx_leftListBg);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox_main[this.evtbx_leftListBg];
			boxChild.Position = 0;
			boxChild.Expand = false;
			this.evtbx_vSeperator = new EventBox();
			this.evtbx_vSeperator.WidthRequest = 1;
			this.evtbx_vSeperator.Name = "evtbx_vSeperator";
			this.hbox_main.Add(this.evtbx_vSeperator);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_main[this.evtbx_vSeperator];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			this.evtbx_rightBg = new EventBox();
			this.evtbx_rightBg.Name = "evtbx_rightBg";
			this.evtbx_rightBorder = new EventBox();
			this.evtbx_rightBorder.Name = "evtbx_rightBorder";
			this.alignment_rightContent = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_rightContent.Name = "alignment_rightContent";
			this.alignment_rightContent.LeftPadding = 15U;
			this.alignment_rightContent.RightPadding = 15U;
			this.vbox_rightMain = new VBox();
			this.vbox_rightMain.Name = "vbox_rightMain";
			this.scrolledwindow_itemTable = new ScrolledWindow();
			this.scrolledwindow_itemTable.WidthRequest = 400;
			this.scrolledwindow_itemTable.HeightRequest = 250;
			this.scrolledwindow_itemTable.CanFocus = true;
			this.scrolledwindow_itemTable.Name = "scrolledwindow_itemTable";
			this.scrolledwindow_itemTable.ShadowType = ShadowType.In;
			Viewport viewport = new Viewport();
			viewport.ShadowType = ShadowType.None;
			this.alignment_table = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_table.Name = "alignment_table";
			this.alignment_table.TopPadding = 15U;
			this.alignment_table.BottomPadding = 5U;
			this.evtbx_tableBg = new EventBox();
			this.evtbx_tableBg.Name = "evtbx_tableBg";
			this.table_slnItems = new Table(2U, 4U, false);
			this.table_slnItems.Name = "table_slnItems";
			this.table_slnItems.RowSpacing = 14U;
			this.table_slnItems.ColumnSpacing = 14U;
			this.evtbx_tableBg.Add(this.table_slnItems);
			this.alignment_table.Add(this.evtbx_tableBg);
			viewport.Add(this.alignment_table);
			this.scrolledwindow_itemTable.Add(viewport);
			this.vbox_rightMain.Add(this.scrolledwindow_itemTable);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox_rightMain[this.scrolledwindow_itemTable];
			boxChild3.Position = 0;
			boxChild3.Expand = false;
			this.alignment_rightSeperator = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_rightSeperator.Name = "alignment_rightSeperator";
			this.evtbx_rightHSeperator = new EventBox();
			this.evtbx_rightHSeperator.HeightRequest = 1;
			this.evtbx_rightHSeperator.Name = "evtbx_rightHSeperator";
			this.alignment_rightSeperator.Add(this.evtbx_rightHSeperator);
			this.vbox_rightMain.Add(this.alignment_rightSeperator);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_rightMain[this.alignment_rightSeperator];
			boxChild4.Position = 1;
			boxChild4.Expand = false;
			this.alignment_itemTitle = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_itemTitle.Name = "alignment_itemTitle";
			this.alignment_itemTitle.TopPadding = 20U;
			this.alignment_itemTitle.BottomPadding = 12U;
			this.hbox_itemTitle = new HBox();
			this.hbox_itemTitle.Name = "hbox_itemTitle";
			this.hbox_itemTitle.Spacing = 6;
			this.label_itemTitle = new Label();
			this.label_itemTitle.Name = "label_itemTitle";
			this.label_itemTitle.Xalign = 0f;
			this.label_itemTitle.LabelProp = Catalog.GetString("游戏项目");
			this.hbox_itemTitle.Add(this.label_itemTitle);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.hbox_itemTitle[this.label_itemTitle];
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.alignment_itemTitle.Add(this.hbox_itemTitle);
			this.vbox_rightMain.Add(this.alignment_itemTitle);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.vbox_rightMain[this.alignment_itemTitle];
			boxChild6.Position = 2;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.alignment_itemDes = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_itemDes.Name = "alignment_itemDes";
			this.hbox_itemDes = new HBox();
			this.hbox_itemDes.Name = "hbox_itemDes";
			this.hbox_itemDes.Spacing = 6;
			this.label_itemDes = new Label();
			this.label_itemDes.Name = "label_itemDes";
			this.label_itemDes.Xalign = 0f;
			this.label_itemDes.LabelProp = Catalog.GetString("一套完整的游戏解决方案，依托Cocos Studio的强大功能，配合Cocos Framework和IDE，从项目创建到游戏发现，一气呵成");
			this.label_itemDes.Wrap = true;
			this.hbox_itemDes.Add(this.label_itemDes);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.hbox_itemDes[this.label_itemDes];
			boxChild7.Position = 0;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			this.alignment_itemDes.Add(this.hbox_itemDes);
			this.vbox_rightMain.Add(this.alignment_itemDes);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox_rightMain[this.alignment_itemDes];
			boxChild8.Position = 3;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			this.alignment_rightContent.Add(this.vbox_rightMain);
			this.evtbx_rightBorder.Add(this.alignment_rightContent);
			this.evtbx_rightBg.Add(this.evtbx_rightBorder);
			this.hbox_main.Add(this.evtbx_rightBg);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.hbox_main[this.evtbx_rightBg];
			boxChild9.Position = 2;
			base.Add(this.hbox_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
			this.vbox_rightMain.SizeAllocated += this.OnRightVobxSizeAllocated;
		}

		private HBox hbox_main;

		private EventBox evtbx_leftListBg;

		private Alignment alignment_leftList;

		private VBox vbox_leftList;

		private EventBox evtbx_vSeperator;

		private EventBox evtbx_rightBg;

		private EventBox evtbx_rightBorder;

		private Alignment alignment_rightContent;

		private VBox vbox_rightMain;

		private ScrolledWindow scrolledwindow_itemTable;

		private Alignment alignment_table;

		private EventBox evtbx_tableBg;

		private Table table_slnItems;

		private Alignment alignment_rightSeperator;

		private EventBox evtbx_rightHSeperator;

		private Alignment alignment_itemTitle;

		private HBox hbox_itemTitle;

		private Label label_itemTitle;

		private Alignment alignment_itemDes;

		private HBox hbox_itemDes;

		private Label label_itemDes;
	}
}
