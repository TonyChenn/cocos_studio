using System;
using System.IO;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;

namespace CocoStudio.ControlLib.Windows
{
	// Token: 0x0200000A RID: 10
	public class CoverPromptDialog : Dialog
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000036 RID: 54 RVA: 0x00003760 File Offset: 0x00001960
		// (remove) Token: 0x06000037 RID: 55 RVA: 0x0000379C File Offset: 0x0000199C
		public event EventHandler ConfirmClickHandler;

		// Token: 0x06000038 RID: 56 RVA: 0x000037D8 File Offset: 0x000019D8
		public CoverPromptDialog(string folderPath = "")
		{
			this.Build();
			this.ChangeBtnPosion();
			this.buttonOk.Name = "MainButton";
			this.readLanuageConfigFile();
			this.InitEvent();
			this.Init();
			this.oldFolderPath = folderPath;
			this.radiobutton_CoverFolder.Active = true;
			this.oldExportFolderName = System.IO.Path.GetFileNameWithoutExtension(folderPath);
			this.label_Prompt.Text = string.Format(LanguageInfo.TextBlock_Prompt_Text, this.oldExportFolderName);
			this.ChengeExportFolderName();
			this.radiobutton_ChangeFolder.Label = string.Format(LanguageInfo.RadioButton_ChangeFolder_Content, this.newExportFolderName);
			this.radiobutton_ChangeFolder.UseUnderline = false;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000038C8 File Offset: 0x00001AC8
		private void ChangeBtnPosion()
		{
			if (!Platform.IsMac)
			{
				HButtonBox actionArea = base.ActionArea;
				ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.buttonOk];
				ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[this.buttonCancel];
				buttonBoxChild.Position = 0;
				buttonBoxChild2.Position = 1;
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000391C File Offset: 0x00001B1C
		private void Init()
		{
			base.AllowGrow = false;
			Rectangle rectangle = new Rectangle(0, 0, 500, 140);
			base.WidthRequest = rectangle.Width;
			base.HeightRequest = rectangle.Height;
			this.SetToDialogStyle(null, true, true, true);
			this.buttonOk.GrabDefault();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003979 File Offset: 0x00001B79
		private void InitEvent()
		{
			this.buttonCancel.Clicked += this.button_Cancel_Click;
			this.buttonOk.Clicked += this.button_OK_Click;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000039AC File Offset: 0x00001BAC
		private void ChengeExportFolderName()
		{
			string text = string.Format("{0}_{1}", this.oldExportFolderName, this.order);
			string directoryName = System.IO.Path.GetDirectoryName(this.oldFolderPath);
			string newExportFolderPath = System.IO.Path.Combine(directoryName, text);
			this.FileExistJudge(newExportFolderPath, text);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000039F4 File Offset: 0x00001BF4
		private void FileExistJudge(string newExportFolderPath, string newfolderName)
		{
			if (Directory.Exists(newExportFolderPath))
			{
				this.order++;
				this.ChengeExportFolderName();
			}
			else
			{
				this.newExportFolderName = newfolderName;
				this.newFolderPath = newExportFolderPath;
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003A38 File Offset: 0x00001C38
		private void button_OK_Click(object sender, EventArgs e)
		{
			if (this.ConfirmClickHandler != null)
			{
				if (this.radiobutton_ChangeFolder.Active)
				{
					this.ConfirmClickHandler(this.newFolderPath, e);
				}
				else
				{
					this.ConfirmClickHandler(this.oldFolderPath, e);
				}
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003A8F File Offset: 0x00001C8F
		private void button_Cancel_Click(object sender, EventArgs e)
		{
			this.IsCanceled = true;
			this.Destroy();
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003AA0 File Offset: 0x00001CA0
		public void readLanuageConfigFile()
		{
			this.radiobutton_CoverFolder.Label = LanguageInfo.RadioButton_CoverFolder_Content;
			this.buttonOk.Label = LanguageInfo.Dialog_ButtonOK;
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonCancel;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003AD8 File Offset: 0x00001CD8
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.HeightRequest = 140;
			base.Name = "CocoStudio.ControlLib.Windows.CoverPromptDialog";
			base.WindowPosition = WindowPosition.CenterOnParent;
			VBox vbox = base.VBox;
			vbox.Name = "dialog1_VBox";
			vbox.BorderWidth = 2U;
			this.vbox2 = new VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			this.vbox2.BorderWidth = 5U;
			this.label_Prompt = new Label();
			this.label_Prompt.HeightRequest = 20;
			this.label_Prompt.Name = "label_Prompt";
			this.label_Prompt.LabelProp = Catalog.GetString("导出目录下存在A文件夹");
			this.vbox2.Add(this.label_Prompt);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox2[this.label_Prompt];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.radiobutton_CoverFolder = new RadioButton(Catalog.GetString("覆盖文件夹，警告：覆盖可能导致导出资源赘余！"));
			this.radiobutton_CoverFolder.CanFocus = true;
			this.radiobutton_CoverFolder.Name = "radiobutton_CoverFolder";
			this.radiobutton_CoverFolder.DrawIndicator = true;
			this.radiobutton_CoverFolder.UseUnderline = true;
			this.radiobutton_CoverFolder.Group = new SList(IntPtr.Zero);
			this.vbox2.Add(this.radiobutton_CoverFolder);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox2[this.radiobutton_CoverFolder];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.radiobutton_ChangeFolder = new RadioButton(Catalog.GetString("_将导出文件夹名称：A_1 修改为A_1"));
			this.radiobutton_ChangeFolder.CanFocus = true;
			this.radiobutton_ChangeFolder.Name = "radiobutton_ChangeFolder";
			this.radiobutton_ChangeFolder.DrawIndicator = true;
			this.radiobutton_ChangeFolder.UseUnderline = true;
			this.radiobutton_ChangeFolder.Group = this.radiobutton_CoverFolder.Group;
			this.vbox2.Add(this.radiobutton_ChangeFolder);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox2[this.radiobutton_ChangeFolder];
			boxChild3.Position = 2;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			vbox.Add(this.vbox2);
			Box.BoxChild boxChild4 = (Box.BoxChild)vbox[this.vbox2];
			boxChild4.Position = 0;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
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
			base.DefaultWidth = 500;
			base.DefaultHeight = 140;
			base.Show();
		}

		// Token: 0x04000028 RID: 40
		public string newExportFolderName = string.Empty;

		// Token: 0x04000029 RID: 41
		public string oldExportFolderName = string.Empty;

		// Token: 0x0400002A RID: 42
		public int order = 1;

		// Token: 0x0400002B RID: 43
		public string oldFolderPath = string.Empty;

		// Token: 0x0400002C RID: 44
		public string newFolderPath = string.Empty;

		// Token: 0x0400002D RID: 45
		public bool IsCanceled = false;

		// Token: 0x0400002E RID: 46
		private VBox vbox2;

		// Token: 0x0400002F RID: 47
		private Label label_Prompt;

		// Token: 0x04000030 RID: 48
		private RadioButton radiobutton_CoverFolder;

		// Token: 0x04000031 RID: 49
		private RadioButton radiobutton_ChangeFolder;

		// Token: 0x04000032 RID: 50
		private Button buttonCancel;

		// Token: 0x04000033 RID: 51
		private Button buttonOk;
	}
}
