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
	[ToolboxItem(true)]
	public class SelectPathWidget : Bin
	{
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

		public string FilePath { get; private set; }

		public event EventHandler<EventArgs> PathSelected;

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

		public void SetEntryEnable(bool isEnable)
		{
			this.entry_path.IsEditable = isEnable;
		}

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

		private HBox hbox_main;

		private Label label_path;

		private Entry entry_path;

		private Button button_browse;
	}
}
