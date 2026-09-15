using System;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;

namespace CocoStudio.ControlLib.Windows
{
	public class CoverEachFileDialog : Dialog
	{
		public event EventHandler ConfirmClickHandler;

		public bool IsExportCover { get; set; }

		public bool IsChangeAll { get; set; }

		public bool IsUnCancel { get; set; }

		public CoverEachFileDialog()
		{
			this.Build();
			this.ChangeBtnPosion();
			this.buttonOk.Name = "MainButton";
			this.Init();
			this.ReadMultiLanguageConfig();
			this.buttonOk.GrabDefault();
			this.buttonCancel.Clicked += this.buttonCancel_Clicked;
			this.buttonOk.Clicked += this.buttonOk_Clicked;
			base.DeleteEvent += this.CoverEachFileWindow_DeleteEvent;
		}

		private void ChangeBtnPosion()
		{
			if (!Platform.IsMac)
			{
				HButtonBox actionArea = base.ActionArea;
				ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.buttonOk];
				buttonBoxChild.Position = 0;
				ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[this.buttonCancel];
				buttonBoxChild2.Position = 1;
			}
		}

		private void Init()
		{
			base.AllowGrow = false;
			Rectangle rectangle = new Rectangle(0, 0, 500, 130);
			base.WidthRequest = rectangle.Width;
			base.HeightRequest = rectangle.Height;
			this.SetToDialogStyle(null, true, true, true);
		}

		private void CoverEachFileWindow_DeleteEvent(object o, DeleteEventArgs e)
		{
			if (!this.IsUnCancel)
			{
				this.IsExportCover = false;
				this.IsChangeAll = true;
				if (this.ConfirmClickHandler != null)
				{
					this.ConfirmClickHandler(this, e);
				}
			}
		}

		private void buttonOk_Clicked(object sender, EventArgs e)
		{
			this.IsExportCover = true;
			this.IsChangeAll = this.checkbutton_All.Active;
			this.IsUnCancel = true;
			if (this.ConfirmClickHandler != null)
			{
				this.ConfirmClickHandler(this, e);
			}
			else
			{
				this.Destroy();
			}
		}

		private void buttonCancel_Clicked(object sender, EventArgs e)
		{
			this.IsExportCover = false;
			this.IsChangeAll = this.checkbutton_All.Active;
			this.IsUnCancel = true;
			if (this.ConfirmClickHandler != null)
			{
				this.ConfirmClickHandler(this, e);
			}
			else
			{
				this.Destroy();
			}
		}

		public void RefreshMessage(string fileName, int allCount, string existInfo = "")
		{
			if (existInfo == "")
			{
				existInfo = LanguageInfo.CoverEachIsExistFile;
			}
			this.label_Message.Text = string.Format(existInfo, fileName);
			if (allCount > 0)
			{
				this.checkbutton_All.Label = string.Format(LanguageInfo.CoverEachIsConflictFile, allCount);
			}
		}

		private void ReadMultiLanguageConfig()
		{
			base.Title = LanguageInfo.ResourceReplacementWindow;
			this.buttonOk.Label = LanguageInfo.Dialog_ButtonYes;
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonNo;
			this.checkbutton_All.Label = LanguageInfo.AllReplacement;
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "CocoStudio.ControlLib.Windows.CoverEachFileDialog";
			base.WindowPosition = WindowPosition.CenterOnParent;
			VBox vbox = base.VBox;
			vbox.WidthRequest = 440;
			vbox.HeightRequest = 130;
			vbox.Name = "dialog1_VBox";
			vbox.BorderWidth = 2U;
			this.vbox2 = new VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 72;
			this.vbox2.BorderWidth = 10U;
			this.label_Message = new Label();
			this.label_Message.Name = "label_Message";
			this.label_Message.LabelProp = Catalog.GetString("label1");
			this.vbox2.Add(this.label_Message);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox2[this.label_Message];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.checkbutton_All = new CheckButton();
			this.checkbutton_All.CanFocus = true;
			this.checkbutton_All.Name = "checkbutton_All";
			this.checkbutton_All.Label = Catalog.GetString("全部选择");
			this.checkbutton_All.DrawIndicator = true;
			this.checkbutton_All.UseUnderline = true;
			this.vbox2.Add(this.checkbutton_All);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox2[this.checkbutton_All];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			vbox.Add(this.vbox2);
			Box.BoxChild boxChild3 = (Box.BoxChild)vbox[this.vbox2];
			boxChild3.Position = 0;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog1_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 5U;
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			this.buttonCancel = new Button();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			base.AddActionWidget(this.buttonCancel, -6);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.buttonCancel];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			this.buttonOk = new Button();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			base.AddActionWidget(this.buttonOk, -5);
			ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[this.buttonOk];
			buttonBoxChild2.Position = 1;
			buttonBoxChild2.Expand = false;
			buttonBoxChild2.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 440;
			base.DefaultHeight = 130;
			base.Show();
		}

		private VBox vbox2;

		private Label label_Message;

		private CheckButton checkbutton_All;

		private Button buttonCancel;

		private Button buttonOk;
	}
}
