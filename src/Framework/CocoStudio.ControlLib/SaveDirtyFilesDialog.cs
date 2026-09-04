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
	// Token: 0x02000012 RID: 18
	public class SaveDirtyFilesDialog : Dialog
	{
		// Token: 0x060000C3 RID: 195 RVA: 0x00006F90 File Offset: 0x00005190
		public SaveDirtyFilesDialog(string fileName)
		{
			this.Init(new List<string>
			{
				fileName
			});
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00006FBC File Offset: 0x000051BC
		public SaveDirtyFilesDialog(List<string> fileList = null)
		{
			this.Init(fileList);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00006FD0 File Offset: 0x000051D0
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

		// Token: 0x060000C6 RID: 198 RVA: 0x00007024 File Offset: 0x00005224
		private void InitStyles()
		{
			base.TransientFor = ApplicationCurrent.MainWindow;
			this.CenterToParentWindow(ApplicationCurrent.MainWindow);
			base.BorderWidth = 8U;
			this.buttonYes.Name = "MainButton";
			this.nodeview_fileList.Name = "DarkTreeView";
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00007074 File Offset: 0x00005274
		private void SetDisplayText()
		{
			base.Title = LanguageInfo.Menu_File_SaveProject;
			this.label_title.Text = LanguageInfo.Dialog_PleaseSaveFiles;
			this.buttonYes.Label = LanguageInfo.Command_Save;
			this.buttonNo.Label = LanguageInfo.Dialog_ButtonDontSave;
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonCancel;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000070D4 File Offset: 0x000052D4
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

		// Token: 0x060000C9 RID: 201 RVA: 0x00007144 File Offset: 0x00005344
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

		// Token: 0x060000CA RID: 202 RVA: 0x000071A0 File Offset: 0x000053A0
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

		// Token: 0x060000CB RID: 203 RVA: 0x00007258 File Offset: 0x00005458
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

		// Token: 0x04000078 RID: 120
		private Alignment alignment_title;

		// Token: 0x04000079 RID: 121
		private Label label_title;

		// Token: 0x0400007A RID: 122
		private Alignment alignment_list;

		// Token: 0x0400007B RID: 123
		private EventBox evtbx_bg;

		// Token: 0x0400007C RID: 124
		private ScrolledWindow GtkScrolledWindow;

		// Token: 0x0400007D RID: 125
		private NodeView nodeview_fileList;

		// Token: 0x0400007E RID: 126
		private Button buttonNo;

		// Token: 0x0400007F RID: 127
		private Button buttonCancel;

		// Token: 0x04000080 RID: 128
		private Button buttonYes;

		// Token: 0x02000013 RID: 19
		[TreeNode(ListOnly = true)]
		private class FileNameNode : TreeNode
		{
			// Token: 0x060000CC RID: 204 RVA: 0x000076FD File Offset: 0x000058FD
			public FileNameNode(string filename)
			{
				this.FileName = filename;
			}

			// Token: 0x1700001D RID: 29
			// (get) Token: 0x060000CD RID: 205 RVA: 0x00007710 File Offset: 0x00005910
			// (set) Token: 0x060000CE RID: 206 RVA: 0x00007727 File Offset: 0x00005927
			[TreeNodeValue(Column = 0)]
			public string FileName { get; private set; }
		}
	}
}
