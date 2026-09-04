using System;
using System.ComponentModel;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using Stetic;

namespace Modules.Communal.CocosCodeIDE
{
	// Token: 0x0200000D RID: 13
	[ToolboxItem(true)]
	public class SelectPathWidget : Bin
	{
		// Token: 0x06000040 RID: 64 RVA: 0x0000345C File Offset: 0x0000165C
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.CocosCodeIDE.SelectPathWidget";
			this.hbox_main = new HBox();
			this.hbox_main.HeightRequest = 26;
			this.hbox_main.Name = "hbox_main";
			this.hbox_main.Spacing = 6;
			this.label_path = new Label();
			this.label_path.Name = "label_path";
			this.label_path.LabelProp = Catalog.GetString("路径");
			this.hbox_main.Add(this.label_path);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox_main[this.label_path];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.entry_path = new Entry();
			this.entry_path.WidthRequest = 250;
			this.entry_path.HeightRequest = 26;
			this.entry_path.CanFocus = true;
			this.entry_path.Name = "entry_path";
			this.entry_path.IsEditable = true;
			this.entry_path.InvisibleChar = '●';
			this.hbox_main.Add(this.entry_path);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_main[this.entry_path];
			boxChild2.Position = 1;
			this.button_browse = new Button();
			this.button_browse.WidthRequest = 60;
			this.button_browse.CanFocus = true;
			this.button_browse.Name = "button_browse";
			this.button_browse.UseUnderline = true;
			this.button_browse.Label = Catalog.GetString("浏览");
			this.hbox_main.Add(this.button_browse);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_main[this.button_browse];
			boxChild3.Position = 2;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			base.Add(this.hbox_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
			this.button_browse.Clicked += this.OnBtnBrowseClicked;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000041 RID: 65 RVA: 0x0000367F File Offset: 0x0000187F
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00003687 File Offset: 0x00001887
		public string FilePath { get; private set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000043 RID: 67 RVA: 0x00003690 File Offset: 0x00001890
		// (remove) Token: 0x06000044 RID: 68 RVA: 0x000036C8 File Offset: 0x000018C8
		public event EventHandler<EventArgs> PathSelected;

		// Token: 0x06000045 RID: 69 RVA: 0x00003700 File Offset: 0x00001900
		public SelectPathWidget(bool isPreference = false)
		{
			this.Build();
			this.label_path.Text = LanguageInfo.Dialog_Path;
			this.button_browse.WidthRequest = 70;
			this.button_browse.Label = LanguageInfo.Dialog_ButtonBrowse + "...";
			if (isPreference)
			{
				this.entry_path.Text = CocosCodeIDEService.Instance.GetExePath();
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003768 File Offset: 0x00001968
		public void SetEntryEnable(bool isEnable)
		{
			this.entry_path.IsEditable = isEnable;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003778 File Offset: 0x00001978
		public void ApplySetting()
		{
			string text = this.entry_path.Text;
			if (this.CheckIsValidExeFilePath(text))
			{
				Services.RecentFileService.CocosCodeIDEDir = text;
				return;
			}
			text = string.Empty;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000037AC File Offset: 0x000019AC
		private bool CheckIsValidExeFilePath(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				return false;
			}
			if (!File.Exists(filePath))
			{
				return false;
			}
			try
			{
				string extension = System.IO.Path.GetExtension(filePath);
				if (string.IsNullOrEmpty(extension) || !extension.Equals(".exe"))
				{
					return false;
				}
			}
			catch (ArgumentException)
			{
				return false;
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("检测文件扩展名时出错", exception);
				return false;
			}
			return true;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003828 File Offset: 0x00001A28
		protected void OnBtnBrowseClicked(object sender, EventArgs e)
		{
			string[] fileTypes = new string[]
			{
				"*.exe"
			};
			string fileName = FileChooserDialogModel.GetOpenFilePath(fileTypes, LanguageInfo.MessageBox_Content96, false, "").FileName;
			if (!string.IsNullOrEmpty(fileName))
			{
				this.entry_path.Text = fileName;
				this.FilePath = fileName;
				if (this.PathSelected != null)
				{
					this.PathSelected(this, e);
				}
			}
		}

		// Token: 0x04000026 RID: 38
		private HBox hbox_main;

		// Token: 0x04000027 RID: 39
		private Label label_path;

		// Token: 0x04000028 RID: 40
		private Entry entry_path;

		// Token: 0x04000029 RID: 41
		private Button button_browse;
	}
}
