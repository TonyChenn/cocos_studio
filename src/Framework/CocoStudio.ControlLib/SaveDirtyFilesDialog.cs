using System;
using System.Collections.Generic;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;

namespace CocoStudio.ControlLib
{
	public class SaveDirtyFilesDialog : Dialog
	{
		public SaveDirtyFilesDialog(string fileName)
		{
			this.Init(new List<string>
			{
				fileName
			});
		}

		public SaveDirtyFilesDialog(List<string> fileList = null)
		{
			this.Init(fileList);
		}

		private void Init(List<string> fileList)
		{
			this.Build();
			this.InitStyles();
			this.SetDisplayText();
			this.ChangeButtonPositon();
			if (fileList == null)
			{
				fileList = new List<string>();
			}
			this.SetNodeViewContent(fileList);
			this.buttonYes.HasFocus = true;
		}

		private void InitStyles()
		{
			base.TransientFor = ApplicationCurrent.MainWindow;
			this.CenterToParentWindow(ApplicationCurrent.MainWindow);
			base.BorderWidth = 8U;
			this.buttonYes.Name = "MainButton";
			this.nodeview_fileList.Name = "DarkTreeView";
		}

		private void SetDisplayText()
		{
			base.Title = LanguageInfo.Menu_File_SaveProject;
			this.label_title.Text = LanguageInfo.Dialog_PleaseSaveFiles;
			this.buttonYes.Label = LanguageInfo.Command_Save;
			this.buttonNo.Label = LanguageInfo.Dialog_ButtonDontSave;
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonCancel;
		}

		public void SetText(string title, string info, string btnYes, string btnNo, string btnCancel = null)
		{
			base.Title = title;
			this.label_title.Text = info;
			this.buttonYes.Label = btnYes;
			this.buttonNo.Label = btnNo;
			if (string.IsNullOrEmpty(btnCancel))
			{
				this.buttonCancel.Label = LanguageInfo.Dialog_ButtonCancel;
			}
			else
			{
				this.buttonCancel.Label = btnCancel;
			}
		}

		private void ChangeButtonPositon()
		{
			if (Platform.IsWindows)
			{
				HButtonBox actionArea = base.ActionArea;
				actionArea.Remove(this.buttonNo);
				actionArea.Remove(this.buttonCancel);
				base.AddActionWidget(this.buttonNo, -9);
				base.AddActionWidget(this.buttonCancel, -6);
			}
		}

		private void SetNodeViewContent(List<string> fileList)
		{
			NodeStore nodeStore = new NodeStore(typeof(SaveDirtyFilesDialog.FileNameNode));
			foreach (string text in fileList)
			{
				if (!string.IsNullOrEmpty(text))
				{
					nodeStore.AddNode(new SaveDirtyFilesDialog.FileNameNode(text));
				}
			}
			this.nodeview_fileList.AppendColumn("File Name", new CellRendererText(), new object[]
			{
				"text",
				0
			});
			this.nodeview_fileList.NodeStore = nodeStore;
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.HeightRequest = 320;
			base.Name = "CocoStudio.ControlLib.SaveDirtyFilesDialog";
			base.Title = Catalog.GetString("保存项目");
			base.TypeHint = WindowTypeHint.Dialog;
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.BorderWidth = 12U;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "vbox_main";
			vbox.Spacing = 8;
			vbox.BorderWidth = 2U;
			this.alignment_title = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_title.Name = "alignment_title";
			this.alignment_title.TopPadding = 2U;
			this.alignment_title.BottomPadding = 2U;
			this.label_title = new Label();
			this.label_title.Name = "label_title";
			this.label_title.Xalign = 0f;
			this.label_title.LabelProp = Catalog.GetString("请在关闭项目之前保存对下列文件的更改");
			this.alignment_title.Add(this.label_title);
			vbox.Add(this.alignment_title);
			Box.BoxChild boxChild = (Box.BoxChild)vbox[this.alignment_title];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.alignment_list = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_list.Name = "alignment_list";
			this.evtbx_bg = new EventBox();
			this.evtbx_bg.Name = "evtbx_bg";
			this.GtkScrolledWindow = new ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ShadowType.In;
			this.nodeview_fileList = new NodeView();
			this.nodeview_fileList.CanFocus = true;
			this.nodeview_fileList.Name = "nodeview_fileList";
			this.nodeview_fileList.EnableSearch = false;
			this.nodeview_fileList.HeadersVisible = false;
			this.GtkScrolledWindow.Add(this.nodeview_fileList);
			this.evtbx_bg.Add(this.GtkScrolledWindow);
			this.alignment_list.Add(this.evtbx_bg);
			vbox.Add(this.alignment_list);
			Box.BoxChild boxChild2 = (Box.BoxChild)vbox[this.alignment_list];
			boxChild2.Position = 1;
			HButtonBox actionArea = base.ActionArea;
			actionArea.WidthRequest = 300;
			actionArea.Name = "dialog_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 5U;
			actionArea.LayoutStyle = ButtonBoxStyle.Spread;
			this.buttonNo = new Button();
			this.buttonNo.CanFocus = true;
			this.buttonNo.Name = "buttonNo";
			this.buttonNo.UseStock = true;
			this.buttonNo.UseUnderline = true;
			this.buttonNo.Label = "gtk-no";
			base.AddActionWidget(this.buttonNo, -9);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.buttonNo];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			this.buttonCancel = new Button();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			base.AddActionWidget(this.buttonCancel, -6);
			ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[this.buttonCancel];
			buttonBoxChild2.Position = 1;
			buttonBoxChild2.Expand = false;
			buttonBoxChild2.Fill = false;
			this.buttonYes = new Button();
			this.buttonYes.CanDefault = true;
			this.buttonYes.CanFocus = true;
			this.buttonYes.Name = "buttonYes";
			this.buttonYes.UseStock = true;
			this.buttonYes.UseUnderline = true;
			this.buttonYes.Label = "gtk-yes";
			base.AddActionWidget(this.buttonYes, -8);
			ButtonBox.ButtonBoxChild buttonBoxChild3 = (ButtonBox.ButtonBoxChild)actionArea[this.buttonYes];
			buttonBoxChild3.Position = 2;
			buttonBoxChild3.Expand = false;
			buttonBoxChild3.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 328;
			base.DefaultHeight = 320;
			base.Hide();
		}

		private Alignment alignment_title;

		private Label label_title;

		private Alignment alignment_list;

		private EventBox evtbx_bg;

		private ScrolledWindow GtkScrolledWindow;

		private NodeView nodeview_fileList;

		private Button buttonNo;

		private Button buttonCancel;

		private Button buttonYes;

		[TreeNode(ListOnly = true)]
		private class FileNameNode : TreeNode
		{
			public FileNameNode(string filename)
			{
				this.FileName = filename;
			}

			[TreeNodeValue(Column = 0)]
			public string FileName { get; private set; }
		}
	}
}
