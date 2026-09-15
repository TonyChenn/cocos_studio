using System;
using System.Threading.Tasks;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using Stetic;

namespace CocoStudio.ControlLib
{
	public class ImportFileDialog : Dialog
	{
		public ImportFileDialog(bool keepBothBtnVisible = true)
		{
			this.Build();
			base.Visible = false;
			this.btnKeepBoth.Name = "MainButton";
			this.result = new DialogResult();
			this.RegistEvent();
			this.btnKeepBoth.Visible = keepBothBtnVisible;
			this.SetMultiLanguageInfo();
		}

		public ImportFileDialog(Window parentWindow, bool keepBothBtnVisible = true) : this(keepBothBtnVisible)
		{
			this.SetToDialogStyle(parentWindow, true, true, true);
		}

		private void RegistEvent()
		{
			this.btnKeepBoth.Clicked += this.btnKeepBoth_Clicked;
			this.btnReplace.Clicked += this.btnReplace_Clicked;
			this.btnSkip.Clicked += this.btnSkip_Clicked;
			base.DeleteEvent += this.ImprotFileDialog_DeleteEvent;
		}

		private void SetMultiLanguageInfo()
		{
			base.Title = LanguageInfo.Menu_File_ImportFile;
			this.ckbIsChangeAll.Label = LanguageInfo.Dialog_Button_ApplyToAll;
			this.btnKeepBoth.Label = LanguageInfo.Dialog_Button_SaveBoth;
			this.btnReplace.Label = LanguageInfo.Dialog_ButtonReplace;
			this.btnSkip.Label = LanguageInfo.Dialog_Button_Skip;
		}

		[ConnectBefore]
		private void ImprotFileDialog_DeleteEvent(object o, DeleteEventArgs args)
		{
			this.SetResult(EFileOperate.Skip);
		}

		private void btnSkip_Clicked(object sender, EventArgs e)
		{
			this.SetResult(EFileOperate.Skip);
		}

		private void btnReplace_Clicked(object sender, EventArgs e)
		{
			this.SetResult(EFileOperate.Replace);
		}

		private void btnKeepBoth_Clicked(object sender, EventArgs e)
		{
			this.SetResult(EFileOperate.KeepBoth);
		}

		private void SetResult(EFileOperate btnResult)
		{
			this.result.ButtonResult = btnResult;
			this.Destroy();
			this.Dispose();
		}

		public DialogResult ShowRun()
		{
			base.Run();
			this.result.IsChangedAll = this.ckbIsChangeAll.Active;
			return this.result;
		}

		public Task<DialogResult> ShowRunAsync()
		{
			return Task.Run<DialogResult>(delegate()
			{
				base.Run();
				this.result.IsChangedAll = this.ckbIsChangeAll.Active;
				return this.result;
			});
		}

		public void RefreshMessage(string fileName, bool keepBothBtnVisible = true)
		{
			this.btnKeepBoth.Visible = keepBothBtnVisible;
			this.labDescribe.Text = string.Format(LanguageInfo.MessageBox218_FileAlreadyExists, fileName);
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.WidthRequest = 400;
			base.HeightRequest = 100;
			base.Name = "ImprotFileDialog.Dialog";
			base.Title = Catalog.GetString("导入文件");
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog1_VBox";
			vbox.BorderWidth = 2U;
			this.alignment1 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment1.Name = "alignment1";
			this.alignment1.LeftPadding = 8U;
			this.labDescribe = new Label();
			this.labDescribe.WidthRequest = 392;
			this.labDescribe.HeightRequest = 50;
			this.labDescribe.LineWrap = true;
			this.labDescribe.Name = "labDescribe";
			this.labDescribe.LabelProp = Catalog.GetString("文件已经存在,请选择");
			this.alignment1.Add(this.labDescribe);
			vbox.Add(this.alignment1);
			Box.BoxChild boxChild = (Box.BoxChild)vbox[this.alignment1];
			boxChild.Position = 0;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog1_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 5U;
			actionArea.LayoutStyle = ButtonBoxStyle.Edge;
			this.ckbIsChangeAll = new CheckButton();
			this.ckbIsChangeAll.CanFocus = true;
			this.ckbIsChangeAll.Name = "ckbIsChangeAll";
			this.ckbIsChangeAll.Label = Catalog.GetString("全部应用");
			this.ckbIsChangeAll.DrawIndicator = true;
			this.ckbIsChangeAll.UseUnderline = true;
			actionArea.Add(this.ckbIsChangeAll);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.ckbIsChangeAll];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			this.hbox4 = new HBox();
			this.hbox4.Name = "hbox4";
			this.hbox4.Spacing = 6;
			this.btnKeepBoth = new Button();
			this.btnKeepBoth.WidthRequest = 65;
			this.btnKeepBoth.CanFocus = true;
			this.btnKeepBoth.Name = "btnKeepBoth";
			this.btnKeepBoth.UseUnderline = true;
			this.btnKeepBoth.Label = Catalog.GetString("保留两者");
			this.hbox4.Add(this.btnKeepBoth);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox4[this.btnKeepBoth];
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.btnReplace = new Button();
			this.btnReplace.WidthRequest = 65;
			this.btnReplace.CanDefault = true;
			this.btnReplace.CanFocus = true;
			this.btnReplace.Name = "btnReplace";
			this.btnReplace.UseUnderline = true;
			this.btnReplace.Label = Catalog.GetString("替换");
			this.hbox4.Add(this.btnReplace);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox4[this.btnReplace];
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.btnSkip = new Button();
			this.btnSkip.WidthRequest = 65;
			this.btnSkip.CanDefault = true;
			this.btnSkip.CanFocus = true;
			this.btnSkip.Name = "btnSkip";
			this.btnSkip.UseUnderline = true;
			this.btnSkip.Label = Catalog.GetString("跳过");
			this.hbox4.Add(this.btnSkip);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox4[this.btnSkip];
			boxChild4.Position = 2;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			actionArea.Add(this.hbox4);
			ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[this.hbox4];
			buttonBoxChild2.Position = 1;
			buttonBoxChild2.Expand = false;
			buttonBoxChild2.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 454;
			base.DefaultHeight = 126;
			base.Hide();
		}

		private DialogResult result;

		private Alignment alignment1;

		private Label labDescribe;

		private CheckButton ckbIsChangeAll;

		private HBox hbox4;

		private Button btnKeepBoth;

		private Button btnReplace;

		private Button btnSkip;
	}
}
