using System;
using System.ComponentModel;
using System.Diagnostics;
using Cocos.Launcher.Control;
using CocoStudio.Core;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000061 RID: 97
	[ToolboxItem(true)]
	public class RecentlyCocosItemView : Bin
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000362 RID: 866 RVA: 0x0000FA7A File Offset: 0x0000DC7A
		// (set) Token: 0x06000363 RID: 867 RVA: 0x0000FA84 File Offset: 0x0000DC84
		public bool IsSelected
		{
			get
			{
				return this.isSelected;
			}
			set
			{
				if (this.isSelected == value)
				{
					return;
				}
				this.isSelected = value;
				if (value)
				{
					this.eventbox_bg.ModifyBg(StateType.Normal, ConstantConfig.Colors.RecentlyColor2);
					return;
				}
				this.eventbox_bg.ModifyBg(StateType.Normal, ConstantConfig.Colors.MainContentColor);
			}
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000364 RID: 868 RVA: 0x0000FAD4 File Offset: 0x0000DCD4
		// (remove) Token: 0x06000365 RID: 869 RVA: 0x0000FB0C File Offset: 0x0000DD0C
		public event EventHandler<OpenClickedEventArgs> OpenClicked;

		// Token: 0x06000366 RID: 870 RVA: 0x0000FB41 File Offset: 0x0000DD41
		public RecentlyCocosItemView(CocosItemModel cocosItemModel)
		{
			this.Build();
			this.cocosItemModel = cocosItemModel;
			this.InitView();
			this.InitValue();
			this.InitEvent();
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000FB68 File Offset: 0x0000DD68
		private void InitView()
		{
			this.link_OpenCocoStudio = new LinkView("Cocos.Launcher.Resource.LauncherResource.cocosstudio.png");
			this.hbox_open.PackStart(this.link_OpenCocoStudio, false, false, 0U);
			((Box.BoxChild)this.hbox_open[this.link_OpenCocoStudio]).Position = 0;
			this.imageButton_OpenDir = new ImageButtonView();
			this.imageButton_OpenDir.IsCursor = true;
			this.imageButton_OpenDir.SetNormalBack("Cocos.Launcher.Resource.LauncherResource.openDir_normal.png");
			this.imageButton_OpenDir.SetMoveBack("Cocos.Launcher.Resource.LauncherResource.openDir_move.png");
			this.hbox_open.PackStart(this.imageButton_OpenDir, false, false, 0U);
			((Box.BoxChild)this.hbox_open[this.imageButton_OpenDir]).Position = 2;
			this.label_Title.ModifyFg(StateType.Normal, ConstantConfig.Colors.TabFontNormalColor);
			this.label_path.ModifyFg(StateType.Normal, ConstantConfig.Colors.NewsInfoColor);
			this.link_OpenCocoStudio.SetBackGroundColor(ConstantConfig.Colors.MainContentColor);
			this.link_OpenCocoStudio.SetForeGroundColor(ConstantConfig.Colors.ContentLabelColor1);
			this.eventbox_line1.ModifyBg(StateType.Normal, ConstantConfig.Colors.MainRectColor);
			this.eventbox_line2.ModifyBg(StateType.Normal, ConstantConfig.Colors.MainLineColor);
			this.label_path.SetFontSize(14.0);
			this.label_Title.SetFontSize(14.0);
			this.link_OpenCocoStudio.SetFontSize(14.0);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000FCDC File Offset: 0x0000DEDC
		private void InitValue()
		{
			this.label_Title.LabelProp = "<b>" + this.cocosItemModel.Name + "</b>";
			string text = this.cocosItemModel.LocalPath;
			if (text.Length > 70)
			{
				text = "..." + this.cocosItemModel.LocalPath.Substring(this.cocosItemModel.LocalPath.Length - 70, 70);
			}
			this.label_path.Text = text;
			this.link_OpenCocoStudio.SetLableText(LanguageInfo.Launcher_Open);
			this.imageButton_OpenDir.TooltipText = LanguageInfo.Command_OpenDirectory;
			this.Tag = this.cocosItemModel;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000FD8C File Offset: 0x0000DF8C
		private void ShowMenu()
		{
			if (this.menu == null)
			{
				this.menu = new Menu();
				MenuItem menuItem = new MenuItem(LanguageInfo.Launcher_Open);
				menuItem.ButtonReleaseEvent += this.openMenuItem_ButtonReleaseEvent;
				MenuItem menuItem2 = new MenuItem(LanguageInfo.Command_OpenDirectory);
				menuItem2.ButtonReleaseEvent += this.openDirMenuItem_ButtonReleaseEvent;
				MenuItem menuItem3 = new MenuItem(LanguageInfo.Command_Remove);
				menuItem3.ButtonReleaseEvent += this.removeMenuItem_ButtonReleaseEvent;
				this.menu.Append(menuItem);
				this.menu.Append(menuItem2);
				this.menu.Append(menuItem3);
			}
			this.menu.Popdown();
			this.menu.Popup();
			this.menu.ShowAll();
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000FE4C File Offset: 0x0000E04C
		private void InitEvent()
		{
			this.link_OpenCocoStudio.ButtonReleaseEvent += this.link_OpenCocoStudio_ButtonReleaseEvent;
			base.ButtonPressEvent += this.RecentlyCocosItemView_ButtonPressEvent;
			this.eventbox_bg.EnterNotifyEvent += this.eventbox_bg_EnterNotifyEvent;
			this.eventbox_bg.LeaveNotifyEvent += this.eventbox_bg_LeaveNotifyEvent;
			this.imageButton_OpenDir.EnterNotifyEvent += this.eventbox_bg_EnterNotifyEvent;
			this.imageButton_OpenDir.LeaveNotifyEvent += this.eventbox_bg_LeaveNotifyEvent;
			this.link_OpenCocoStudio.EnterNotifyEvent += this.eventbox_bg_EnterNotifyEvent;
			this.link_OpenCocoStudio.LeaveNotifyEvent += this.eventbox_bg_LeaveNotifyEvent;
			this.eventbox_line1.EnterNotifyEvent += this.eventbox_bg_EnterNotifyEvent;
			this.eventbox_line1.LeaveNotifyEvent += this.eventbox_bg_LeaveNotifyEvent;
			this.imageButton_OpenDir.ButtonReleaseEvent += this.imageButton_OpenDir_ButtonReleaseEvent;
			base.ButtonReleaseEvent += this.RecentlyCocosItemView_ButtonReleaseEvent;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000FF6D File Offset: 0x0000E16D
		private void OpenToCocosStudio()
		{
			base.Sensitive = false;
			GLib.Timeout.Add(3000U, delegate
			{
				base.Sensitive = true;
				return false;
			});
			if (this.OpenClicked != null)
			{
				this.OpenClicked(this, new OpenClickedEventArgs(this.Tag, StartEnum.CocosStudio));
			}
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000FFB0 File Offset: 0x0000E1B0
		private void OpenToLocation()
		{
			if (Platform.IsWindows)
			{
				System.Diagnostics.Process.Start("Explorer", "/select," + this.cocosItemModel.LocalPath);
				return;
			}
			System.Diagnostics.Process.Start("open", "-R " + string.Format("\"{0}\"", this.cocosItemModel.LocalPath));
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0001000F File Offset: 0x0000E20F
		private void RecentlyCocosItemView_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			if (args.Event.Type == EventType.TwoButtonPress)
			{
				this.OpenToCocosStudio();
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00010025 File Offset: 0x0000E225
		private void link_OpenCocoStudio_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			this.OpenToCocosStudio();
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0001002D File Offset: 0x0000E22D
		private void imageButton_OpenDir_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			this.OpenToLocation();
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00010035 File Offset: 0x0000E235
		private void eventbox_bg_LeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
		{
			if (this.IsSelected)
			{
				return;
			}
			this.eventbox_bg.ModifyBg(StateType.Normal, ConstantConfig.Colors.MainContentColor);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00010056 File Offset: 0x0000E256
		private void eventbox_bg_EnterNotifyEvent(object o, EnterNotifyEventArgs args)
		{
			if (this.IsSelected)
			{
				return;
			}
			this.eventbox_bg.ModifyBg(StateType.Normal, ConstantConfig.Colors.RecentlyColor1);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00010077 File Offset: 0x0000E277
		private void removeMenuItem_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			Services.RecentFileService.RemoveCocosItem(this.cocosItemModel);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00010089 File Offset: 0x0000E289
		private void openDirMenuItem_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			this.OpenToLocation();
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00010091 File Offset: 0x0000E291
		private void openMenuItem_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			this.OpenToCocosStudio();
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00010099 File Offset: 0x0000E299
		private void RecentlyCocosItemView_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			if (args.Event.Button == 3U)
			{
				this.ShowMenu();
			}
			args.RetVal = true;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000100BC File Offset: 0x0000E2BC
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.HeightRequest = 65;
			base.Name = "Cocos.Launcher.Core.RecentlyProjectView";
			this.vbox1 = new VBox();
			this.vbox1.Name = "vbox1";
			this.eventbox_bg = new EventBox();
			this.eventbox_bg.Name = "eventbox_bg";
			this.alignment_view = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_view.Name = "alignment_view";
			this.alignment_view.TopPadding = 10U;
			this.alignment_view.BottomPadding = 10U;
			this.hbox1 = new HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			this.alignment4 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment4.WidthRequest = 4;
			this.alignment4.Name = "alignment4";
			this.hbox1.Add(this.alignment4);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox1[this.alignment4];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.vbox3 = new VBox();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			this.label_Title = new Label();
			this.label_Title.Name = "label_Title";
			this.label_Title.Xalign = 0f;
			this.label_Title.LabelProp = Catalog.GetString("label1");
			this.label_Title.UseMarkup = true;
			this.vbox3.Add(this.label_Title);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox3[this.label_Title];
			boxChild2.Position = 0;
			this.label_path = new Label();
			this.label_path.Name = "label_path";
			this.label_path.Xalign = 0f;
			this.label_path.LabelProp = Catalog.GetString("label2");
			this.vbox3.Add(this.label_path);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox3[this.label_path];
			boxChild3.Position = 1;
			this.hbox1.Add(this.vbox3);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox1[this.vbox3];
			boxChild4.Position = 1;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.alignment2 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment2.Name = "alignment2";
			this.hbox1.Add(this.alignment2);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.hbox1[this.alignment2];
			boxChild5.Position = 2;
			this.alignment5 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment5.WidthRequest = 16;
			this.alignment5.Name = "alignment5";
			this.hbox1.Add(this.alignment5);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox1[this.alignment5];
			boxChild6.PackType = PackType.End;
			boxChild6.Position = 3;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.alignment3 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment3.Name = "alignment3";
			this.alignment3.TopPadding = 10U;
			this.alignment3.BottomPadding = 10U;
			this.hbox_open = new HBox();
			this.hbox_open.Name = "hbox_open";
			this.hbox_open.Spacing = 22;
			this.alignment1 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment1.Name = "alignment1";
			this.alignment1.TopPadding = 5U;
			this.alignment1.BottomPadding = 5U;
			this.eventbox_line1 = new EventBox();
			this.eventbox_line1.WidthRequest = 1;
			this.eventbox_line1.Name = "eventbox_line1";
			this.alignment1.Add(this.eventbox_line1);
			this.hbox_open.Add(this.alignment1);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.hbox_open[this.alignment1];
			boxChild7.Position = 1;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			this.alignment3.Add(this.hbox_open);
			this.hbox1.Add(this.alignment3);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.hbox1[this.alignment3];
			boxChild8.PackType = PackType.End;
			boxChild8.Position = 4;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			this.alignment_view.Add(this.hbox1);
			this.eventbox_bg.Add(this.alignment_view);
			this.vbox1.Add(this.eventbox_bg);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.vbox1[this.eventbox_bg];
			boxChild9.Position = 0;
			this.eventbox_line2 = new EventBox();
			this.eventbox_line2.HeightRequest = 1;
			this.eventbox_line2.Name = "eventbox_line2";
			this.vbox1.Add(this.eventbox_line2);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.vbox1[this.eventbox_line2];
			boxChild10.Position = 1;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			base.Add(this.vbox1);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		// Token: 0x0400016E RID: 366
		private LinkView link_OpenCocoStudio;

		// Token: 0x0400016F RID: 367
		private ImageButtonView imageButton_OpenDir;

		// Token: 0x04000170 RID: 368
		public object Tag;

		// Token: 0x04000171 RID: 369
		private CocosItemModel cocosItemModel;

		// Token: 0x04000172 RID: 370
		private bool isSelected;

		// Token: 0x04000174 RID: 372
		private Menu menu;

		// Token: 0x04000175 RID: 373
		private VBox vbox1;

		// Token: 0x04000176 RID: 374
		private EventBox eventbox_bg;

		// Token: 0x04000177 RID: 375
		private Alignment alignment_view;

		// Token: 0x04000178 RID: 376
		private HBox hbox1;

		// Token: 0x04000179 RID: 377
		private Alignment alignment4;

		// Token: 0x0400017A RID: 378
		private VBox vbox3;

		// Token: 0x0400017B RID: 379
		private Label label_Title;

		// Token: 0x0400017C RID: 380
		private Label label_path;

		// Token: 0x0400017D RID: 381
		private Alignment alignment2;

		// Token: 0x0400017E RID: 382
		private Alignment alignment5;

		// Token: 0x0400017F RID: 383
		private Alignment alignment3;

		// Token: 0x04000180 RID: 384
		private HBox hbox_open;

		// Token: 0x04000181 RID: 385
		private Alignment alignment1;

		// Token: 0x04000182 RID: 386
		private EventBox eventbox_line1;

		// Token: 0x04000183 RID: 387
		private EventBox eventbox_line2;
	}
}
