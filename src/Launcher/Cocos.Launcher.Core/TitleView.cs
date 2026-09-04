using System;
using System.ComponentModel;
using Cocos.Launcher.Control;
using CocoStudio.Core;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	// Token: 0x0200005C RID: 92
	[ToolboxItem(true)]
	public class TitleView : EventBox
	{
		// Token: 0x06000329 RID: 809 RVA: 0x0000CC93 File Offset: 0x0000AE93
		public TitleView(MainWindow mainWindow)
		{
			this.mainWindow = mainWindow;
			this.InitView();
			this.InitEvent();
			base.ShowAll();
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000CCB4 File Offset: 0x0000AEB4
		private void InitView()
		{
			Label label = new Label();
			label.LabelProp = "Cocos";
			label.ModifyFg(StateType.Normal, ConstantConfig.Colors.LableColor1);
			label.SetFontSize(13.0);
			VBox vbox = new VBox();
			HBox hbox = new HBox();
			if (Platform.IsMac)
			{
				Alignment alignment = new Alignment(0.5f, 0.5f, 1f, 1f);
				alignment.WidthRequest = 38;
				HBox hbox2 = new HBox();
				hbox2.Spacing = 6;
				Alignment alignment2 = new Alignment(0.5f, 0.5f, 1f, 1f);
				alignment2.WidthRequest = 8;
				this.imageButtonMacMin = new ImageButtonView();
				this.imageButtonMacMin.SetNormalBack("CocoStudio.DefaultResource.WindowResource.macMin_normal.png");
				this.imageButtonMacMin.SetMoveBack("CocoStudio.DefaultResource.WindowResource.macMin_hover.png");
				this.imageButtonMacMin.SetPressBack("CocoStudio.DefaultResource.WindowResource.macMin_pressed.png");
				this.imageButtonMacMin.ButtonReleaseEvent += this.mainWindowToIconify;
				this.imageButtonMacClose = new ImageButtonView();
				this.imageButtonMacClose.SetNormalBack("CocoStudio.DefaultResource.WindowResource.macClose_normal.png");
				this.imageButtonMacClose.SetMoveBack("CocoStudio.DefaultResource.WindowResource.macClose_hover.png");
				this.imageButtonMacClose.SetPressBack("CocoStudio.DefaultResource.WindowResource.macClose_pressed.png");
				this.imageButtonMacClose.ButtonReleaseEvent += this.mainWindowToClose;
				hbox2.PackStart(this.imageButtonMacClose, false, false, 0U);
				hbox2.PackStart(this.imageButtonMacMin, false, false, 0U);
				hbox.PackStart(alignment2, false, false, 0U);
				hbox.PackStart(hbox2, false, false, 0U);
				hbox.PackStart(label, true, true, 0U);
				hbox.PackStart(alignment, false, false, 0U);
			}
			else
			{
				VBox vbox2 = new VBox();
				HBox hbox3 = new HBox();
				Alignment alignment3 = new Alignment(0.5f, 0.5f, 1f, 1f);
				alignment3.WidthRequest = 110;
				Alignment alignment4 = new Alignment(0.5f, 0.5f, 1f, 1f);
				alignment4.WidthRequest = 5;
				this.titleSettingBtn = new ImageButtonView();
				this.titleSettingBtn.SetSizeRequest(29, 18);
				this.titleSettingBtn.ButtonReleaseEvent += this.HandleSettingTitleBtnClilcked;
				this.titleSettingBtn.SetNormalBack("CocoStudio.DefaultResource.WindowResource.WinSetting_normal.png");
				this.titleSettingBtn.SetMoveBack("CocoStudio.DefaultResource.WindowResource.WinSetting_hover.png");
				this.titleSettingBtn.SetPressBack("CocoStudio.DefaultResource.WindowResource.WinSetting_normal.png");
				this.titleSettingBtn.TooltipText = LanguageInfo.Dialog_Publish_Setting;
				this.imageButtonWinMin = new ImageButtonView();
				this.imageButtonWinMin.SetSizeRequest(29, 18);
				this.imageButtonWinMin.SetNormalBack("CocoStudio.DefaultResource.WindowResource.WinMin_normal.png");
				this.imageButtonWinMin.SetMoveBack("CocoStudio.DefaultResource.WindowResource.WinMin_hover.png");
				this.imageButtonWinMin.SetPressBack("CocoStudio.DefaultResource.WindowResource.WinMin_hover.png");
				this.imageButtonWinMin.ButtonReleaseEvent += this.mainWindowToIconify;
				this.imageButtonWinMin.TooltipText = LanguageInfo.Menu_Launcher_Minimize;
				this.imageButtonWinClose = new ImageButtonView();
				this.imageButtonWinClose.SetSizeRequest(47, 18);
				this.imageButtonWinClose.SetNormalBack("CocoStudio.DefaultResource.WindowResource.close_1.png");
				this.imageButtonWinClose.SetMoveBack("CocoStudio.DefaultResource.WindowResource.close_2.png");
				this.imageButtonWinClose.SetPressBack("CocoStudio.DefaultResource.WindowResource.close_2.png");
				this.imageButtonWinClose.ButtonReleaseEvent += this.mainWindowToClose;
				this.imageButtonWinClose.TooltipText = LanguageInfo.Dialog_ButtonClose;
				hbox3.PackStart(this.titleSettingBtn, false, false, 0U);
				hbox3.PackStart(this.imageButtonWinMin, false, false, 0U);
				hbox3.PackStart(this.imageButtonWinClose, false, false, 0U);
				hbox3.PackStart(alignment4, false, false, 0U);
				vbox2.PackStart(hbox3, false, false, 0U);
				hbox.PackStart(alignment3, false, false, 0U);
				hbox.PackStart(label, true, true, 0U);
				hbox.PackStart(vbox2, false, false, 0U);
			}
			EventBox eventBox = new EventBox();
			eventBox.ModifyBg(StateType.Normal, ConstantConfig.Colors.LineColor2);
			eventBox.HeightRequest = 1;
			vbox.PackStart(hbox, true, true, 0U);
			vbox.PackStart(eventBox, false, false, 0U);
			base.ModifyBg(StateType.Normal, ConstantConfig.Colors.MainTitleColor);
			base.Add(vbox);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000D0B0 File Offset: 0x0000B2B0
		private void InitEvent()
		{
			base.ButtonPressEvent += this.eventbox_top_ButtonPressEvent;
			base.ButtonReleaseEvent += this.eventbox_top_ButtonReleaseEvent;
			base.MotionNotifyEvent += this.eventbox_top_MotionNotifyEvent;
			this.mainWindow.FocusOutEvent += this.mainWindow_FocusOutEvent;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000D10A File Offset: 0x0000B30A
		private void HandleSettingTitleBtnClilcked(object sender, ButtonReleaseEventArgs e)
		{
			Services.CommandService.ShowContextMenu(this.titleSettingBtn, e.Event, Services.MainWindow.MenuManager.WindowsMenu, null);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000D132 File Offset: 0x0000B332
		private void mainWindowToClose(object o, ButtonReleaseEventArgs args)
		{
			if (args.Event.Button == 1U)
			{
				this.mainWindow.GdkWindow.Hide();
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000D152 File Offset: 0x0000B352
		private void mainWindowToIconify(object o, ButtonReleaseEventArgs args)
		{
			if (args.Event.Button == 1U)
			{
				this.mainWindow.GdkWindow.Hide();
			}
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000D174 File Offset: 0x0000B374
		private void eventbox_top_MotionNotifyEvent(object o, MotionNotifyEventArgs args)
		{
			if (this.isMove)
			{
				double num = args.Event.X - this.mousePress_x;
				double num2 = args.Event.Y - this.mousePress_y;
				int num3;
				int num4;
				base.GdkWindow.GetOrigin(out num3, out num4);
				int x = (int)((double)num3 + num);
				int num5 = (int)((double)num4 + num2);
				if (Platform.IsMac && num5 < 23)
				{
					num5 = 23;
				}
				this.mainWindow.Move(x, num5);
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000D1ED File Offset: 0x0000B3ED
		private void eventbox_top_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			if (args.Event.Button == 1U)
			{
				this.isMove = false;
			}
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000D204 File Offset: 0x0000B404
		private void mainWindow_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			this.isMove = false;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000D20D File Offset: 0x0000B40D
		private void eventbox_top_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			if (args.Event.Button == 1U)
			{
				this.isMove = true;
				this.mousePress_x = args.Event.X;
				this.mousePress_y = args.Event.Y;
			}
		}

		// Token: 0x04000127 RID: 295
		private ImageButtonView imageButtonMacMin;

		// Token: 0x04000128 RID: 296
		private ImageButtonView imageButtonMacClose;

		// Token: 0x04000129 RID: 297
		private ImageButtonView imageButtonWinMin;

		// Token: 0x0400012A RID: 298
		private ImageButtonView imageButtonWinClose;

		// Token: 0x0400012B RID: 299
		private ImageButtonView titleSettingBtn;

		// Token: 0x0400012C RID: 300
		private bool isMove;

		// Token: 0x0400012D RID: 301
		private double mousePress_x;

		// Token: 0x0400012E RID: 302
		private double mousePress_y;

		// Token: 0x0400012F RID: 303
		private MainWindow mainWindow;

		// Token: 0x04000130 RID: 304
		private bool isMaxSize;
	}
}
