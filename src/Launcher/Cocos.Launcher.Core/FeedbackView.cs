using System;
using System.ComponentModel;
using Cocos.Launcher.Control;
using CocoStudio.Basic;
using GLib;
using Gtk;
using Modules.Communal.CocoaChina;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using Stetic;

namespace Cocos.Launcher.Core
{
	[ToolboxItem(true)]
	public class FeedbackView : Bin
	{
		public FeedbackView()
		{
			this.Build();
			this.feedbackInfo = new FeedbackInfo();
			this.InitView();
			this.InitLanguage();
			base.ShowAll();
		}

		private void InitView()
		{
			this.button_submit = new ButtonView();
			this.button_submit.SetNormalBack("Cocos.Launcher.Resource.LauncherResource.newNormal.png");
			this.button_submit.SetMoveBack("Cocos.Launcher.Resource.LauncherResource.newMove.png");
			this.button_submit.SetIsEditBack("Cocos.Launcher.Resource.LauncherResource.newEdit.png");
			this.button_submit.SetPressBack("Cocos.Launcher.Resource.LauncherResource.newPress.png");
			this.button_submit.SetLableFontSize(14.0);
			this.button_submit.ButtonReleaseEvent += this.button_submit_ButtonReleaseEvent;
			this.button_submit.SetLableNormalColor(ConstantConfig.Colors.MainContentColor);
			this.button_submit.SetSize(80, 26);
			this.table1.Add(this.button_submit);
			Table.TableChild tableChild = (Table.TableChild)this.table1[this.button_submit];
			tableChild.TopAttach = 2U;
			tableChild.BottomAttach = 3U;
			tableChild.LeftAttach = 2U;
			tableChild.RightAttach = 3U;
			tableChild.XOptions = AttachOptions.Fill;
			tableChild.YOptions = AttachOptions.Fill;
			this.eventbox_infoLine.ModifyBg(StateType.Normal, ConstantConfig.Colors.LineColor1);
			this.label1.ModifyFg(StateType.Normal, ConstantConfig.Colors.RedColor);
			this.label2.ModifyFg(StateType.Normal, ConstantConfig.Colors.RedColor);
			this.label_classify.SetFontSize(14.0);
			this.label_describe.SetFontSize(14.0);
			this.textview_info.SetFontSize(14.0);
			foreach (string text in this.feedbackInfo.QuestionClassifyList)
			{
				this.combobox_classify.AppendText(text);
			}
			this.combobox_classify.Active = 0;
			this.linkView = new LinkView();
			this.linkView.ButtonReleaseEvent += this.linkView_ButtonReleaseEvent;
			this.linkView.SetFontSize(14.0);
			this.linkView.SetForeGroundColor(ConstantConfig.Colors.TabFontPressColor);
			this.hbox3.PackStart(this.linkView, false, false, 0U);
		}

		private void InitLanguage()
		{
			this.label_classify.Text = LanguageInfo.Launcher_QUClassify;
			this.label_describe.Text = LanguageInfo.Launcher_QUDescribe;
			this.button_submit.SetLabelText(LanguageInfo.Launcher_Submit);
			this.linkView.SetLableText(LanguageInfo.Launcher_UsualQuestions);
		}

		private void linkView_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			WebHelper.OpenWeb(ConstantConfig.Constant.UsualQuestionsUrl);
		}

		private void button_submit_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			if (!Services.LoginService.IsLoginSuccessed)
			{
				Services.OutputService.Info(LanguageInfo.Launcher_FeedbackNoLogin);
				return;
			}
			if (string.IsNullOrWhiteSpace(this.textview_info.Buffer.Text))
			{
				MessageBox.Show(LanguageInfo.Launcher_FeedbackCantBeEmpty, MessageBoxImage.Other, null, null);
				return;
			}
			this.feedbackInfo.Type_id = this.combobox_classify.Active;
			this.feedbackInfo.Description = this.textview_info.Buffer.Text;
			HttpSync httpSync = new HttpSync();
			httpSync.OnResived += this.FeedbackInfoResived;
			httpSync.GetSyncResponseOfString(ConstantConfig.Constant.FeedbackUrl, "post", this.feedbackInfo.GetFeedbackInfo(), null);
		}

		private void FeedbackInfoResived(object sender, HttpSync.HttpSyncArgs e)
		{
			GLib.Timeout.Add(0U, delegate
			{
				string empty = string.Empty;
				bool flag = this.IsFeedbackSucceed(e.Message);
				if (flag)
				{
					MessageBox.Show(LanguageInfo.Launcher_SucceedFeedback, MessageBoxImage.Other, null, null);
					this.combobox_classify.Active = 0;
					this.textview_info.Buffer.Text = string.Empty;
				}
				else
				{
					MessageBox.Show(LanguageInfo.Launcher_FailureFeedback, MessageBoxImage.Other, null, null);
				}
				return false;
			});
		}

		private bool IsFeedbackSucceed(string retStr)
		{
			bool result = false;
			try
			{
				if (!string.IsNullOrWhiteSpace(retStr) && retStr.Contains("status"))
				{
					result = bool.Parse(Login.GetJsonAnalysis(retStr, "status"));
				}
			}
			catch (Exception message)
			{
				result = false;
				LogConfig.Logger.Error(message);
			}
			return result;
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Cocos.Launcher.Core.FeedbackView";
			this.eventbox1 = new EventBox();
			this.eventbox1.Name = "eventbox1";
			this.alignment1 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment1.Name = "alignment1";
			this.alignment1.LeftPadding = 28U;
			this.alignment1.TopPadding = 28U;
			this.alignment1.RightPadding = 28U;
			this.table1 = new Table(3U, 3U, false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = 12U;
			this.table1.ColumnSpacing = 10U;
			this.eventbox_infoLine = new EventBox();
			this.eventbox_infoLine.Name = "eventbox_infoLine";
			this.alignment5 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment5.Name = "alignment5";
			this.alignment5.BorderWidth = 1U;
			this.GtkScrolledWindow = new ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ShadowType.In;
			this.textview_info = new TextView();
			this.textview_info.HeightRequest = 470;
			this.textview_info.CanFocus = true;
			this.textview_info.Name = "textview_info";
			this.textview_info.WrapMode = WrapMode.Char;
			this.textview_info.PixelsAboveLines = 2;
			this.textview_info.PixelsInsideWrap = 2;
			this.textview_info.LeftMargin = 10;
			this.textview_info.RightMargin = 10;
			this.GtkScrolledWindow.Add(this.textview_info);
			this.alignment5.Add(this.GtkScrolledWindow);
			this.eventbox_infoLine.Add(this.alignment5);
			this.table1.Add(this.eventbox_infoLine);
			Table.TableChild tableChild = (Table.TableChild)this.table1[this.eventbox_infoLine];
			tableChild.TopAttach = 1U;
			tableChild.BottomAttach = 2U;
			tableChild.LeftAttach = 2U;
			tableChild.RightAttach = 3U;
			tableChild.YOptions = AttachOptions.Fill;
			this.hbox3 = new HBox();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			this.combobox_classify = ComboBox.NewText();
			this.combobox_classify.WidthRequest = 200;
			this.combobox_classify.Name = "combobox_classify";
			this.hbox3.Add(this.combobox_classify);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox3[this.combobox_classify];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.alignment6 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment6.Name = "alignment6";
			this.hbox3.Add(this.alignment6);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox3[this.alignment6];
			boxChild2.Position = 1;
			this.table1.Add(this.hbox3);
			Table.TableChild tableChild2 = (Table.TableChild)this.table1[this.hbox3];
			tableChild2.LeftAttach = 2U;
			tableChild2.RightAttach = 3U;
			tableChild2.XOptions = AttachOptions.Fill;
			tableChild2.YOptions = AttachOptions.Fill;
			this.label_classify = new Label();
			this.label_classify.Name = "label_classify";
			this.label_classify.Xalign = 0f;
			this.label_classify.LabelProp = Catalog.GetString("label3");
			this.table1.Add(this.label_classify);
			Table.TableChild tableChild3 = (Table.TableChild)this.table1[this.label_classify];
			tableChild3.LeftAttach = 1U;
			tableChild3.RightAttach = 2U;
			tableChild3.XOptions = AttachOptions.Fill;
			tableChild3.YOptions = AttachOptions.Fill;
			this.label_describe = new Label();
			this.label_describe.Name = "label_describe";
			this.label_describe.Xalign = 0f;
			this.label_describe.Yalign = 0f;
			this.label_describe.LabelProp = Catalog.GetString("label4");
			this.table1.Add(this.label_describe);
			Table.TableChild tableChild4 = (Table.TableChild)this.table1[this.label_describe];
			tableChild4.TopAttach = 1U;
			tableChild4.BottomAttach = 2U;
			tableChild4.LeftAttach = 1U;
			tableChild4.RightAttach = 2U;
			tableChild4.XOptions = AttachOptions.Fill;
			tableChild4.YOptions = AttachOptions.Fill;
			this.label1 = new Label();
			this.label1.Name = "label1";
			this.label1.LabelProp = Catalog.GetString("*");
			this.table1.Add(this.label1);
			Table.TableChild tableChild5 = (Table.TableChild)this.table1[this.label1];
			tableChild5.XOptions = AttachOptions.Fill;
			tableChild5.YOptions = AttachOptions.Fill;
			this.label2 = new Label();
			this.label2.Name = "label2";
			this.label2.Yalign = 0.01f;
			this.label2.LabelProp = Catalog.GetString("*");
			this.table1.Add(this.label2);
			Table.TableChild tableChild6 = (Table.TableChild)this.table1[this.label2];
			tableChild6.TopAttach = 1U;
			tableChild6.BottomAttach = 2U;
			tableChild6.XOptions = AttachOptions.Fill;
			tableChild6.YOptions = AttachOptions.Fill;
			this.alignment1.Add(this.table1);
			this.eventbox1.Add(this.alignment1);
			base.Add(this.eventbox1);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		private ButtonView button_submit;

		private FeedbackInfo feedbackInfo;

		private LinkView linkView;

		private EventBox eventbox1;

		private Alignment alignment1;

		private Table table1;

		private EventBox eventbox_infoLine;

		private Alignment alignment5;

		private ScrolledWindow GtkScrolledWindow;

		private TextView textview_info;

		private HBox hbox3;

		private ComboBox combobox_classify;

		private Alignment alignment6;

		private Label label_classify;

		private Label label_describe;

		private Label label1;

		private Label label2;
	}
}
