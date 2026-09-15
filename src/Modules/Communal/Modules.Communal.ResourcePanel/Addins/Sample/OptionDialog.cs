using System;
using System.IO;
using CocoStudio.Basic;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Components;
using MonoDevelop.Core;
using Stetic;

namespace Addins.Sample
{
	public class OptionDialog : Dialog
	{
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "Addins.Sample.OptionDialog";
			base.Title = Catalog.GetString("导出碎图");
			base.TypeHint = WindowTypeHint.Dialog;
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.BorderWidth = 10U;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog_VBox";
			vbox.BorderWidth = 2U;
			this.alignment_main = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_main.Name = "alignment_main";
			this.alignment_main.TopPadding = 15U;
			this.alignment_main.BottomPadding = 15U;
			this.alignment_main.BorderWidth = 6U;
			this.table_main = new Table(3U, 2U, false);
			this.table_main.WidthRequest = 450;
			this.table_main.Name = "table_main";
			this.table_main.RowSpacing = 12U;
			this.table_main.ColumnSpacing = 15U;
			this.hbox_borderOption = new HBox();
			this.hbox_borderOption.Name = "hbox_borderOption";
			this.hbox_borderOption.Spacing = 20;
			this.radiobutton_noEdge = new RadioButton(Catalog.GetString("剪裁空白边缘"));
			this.radiobutton_noEdge.CanFocus = true;
			this.radiobutton_noEdge.Name = "radiobutton_noEdge";
			this.radiobutton_noEdge.DrawIndicator = true;
			this.radiobutton_noEdge.UseUnderline = true;
			this.radiobutton_noEdge.Group = new SList(IntPtr.Zero);
			this.hbox_borderOption.Add(this.radiobutton_noEdge);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox_borderOption[this.radiobutton_noEdge];
			boxChild.Position = 0;
			boxChild.Expand = false;
			this.radiobutton_edge = new RadioButton(Catalog.GetString("保留空白边缘"));
			this.radiobutton_edge.CanFocus = true;
			this.radiobutton_edge.Name = "radiobutton_edge";
			this.radiobutton_edge.DrawIndicator = true;
			this.radiobutton_edge.UseUnderline = true;
			this.radiobutton_edge.Group = this.radiobutton_noEdge.Group;
			this.hbox_borderOption.Add(this.radiobutton_edge);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_borderOption[this.radiobutton_edge];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			this.table_main.Add(this.hbox_borderOption);
			Table.TableChild tableChild = (Table.TableChild)this.table_main[this.hbox_borderOption];
			tableChild.TopAttach = 2U;
			tableChild.BottomAttach = 3U;
			tableChild.LeftAttach = 1U;
			tableChild.RightAttach = 2U;
			tableChild.XOptions = AttachOptions.Fill;
			tableChild.YOptions = AttachOptions.Fill;
			this.hbox_path = new HBox();
			this.hbox_path.Name = "hbox_path";
			this.hbox_path.Spacing = 6;
			this.entry_path = new Entry();
			this.entry_path.HeightRequest = 26;
			this.entry_path.CanFocus = true;
			this.entry_path.Name = "entry_path";
			this.entry_path.IsEditable = false;
			this.entry_path.InvisibleChar = '●';
			this.hbox_path.Add(this.entry_path);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_path[this.entry_path];
			boxChild3.Position = 0;
			this.button_browse = new Button();
			this.button_browse.WidthRequest = 65;
			this.button_browse.HeightRequest = 26;
			this.button_browse.CanFocus = true;
			this.button_browse.Name = "button_browse";
			this.button_browse.UseUnderline = true;
			this.button_browse.Label = Catalog.GetString("浏览");
			this.hbox_path.Add(this.button_browse);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox_path[this.button_browse];
			boxChild4.Position = 1;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.table_main.Add(this.hbox_path);
			Table.TableChild tableChild2 = (Table.TableChild)this.table_main[this.hbox_path];
			tableChild2.TopAttach = 1U;
			tableChild2.BottomAttach = 2U;
			tableChild2.LeftAttach = 1U;
			tableChild2.RightAttach = 2U;
			tableChild2.XOptions = AttachOptions.Fill;
			tableChild2.YOptions = AttachOptions.Fill;
			this.label_border = new Label();
			this.label_border.Name = "label_border";
			this.label_border.Xalign = 1f;
			this.label_border.LabelProp = Catalog.GetString("边缘设置:");
			this.table_main.Add(this.label_border);
			Table.TableChild tableChild3 = (Table.TableChild)this.table_main[this.label_border];
			tableChild3.TopAttach = 2U;
			tableChild3.BottomAttach = 3U;
			tableChild3.XOptions = AttachOptions.Fill;
			tableChild3.YOptions = AttachOptions.Fill;
			this.label_name = new Label();
			this.label_name.Name = "label_name";
			this.label_name.Xalign = 1f;
			this.label_name.LabelProp = Catalog.GetString("合图文件:");
			this.table_main.Add(this.label_name);
			Table.TableChild tableChild4 = (Table.TableChild)this.table_main[this.label_name];
			tableChild4.XOptions = AttachOptions.Fill;
			tableChild4.YOptions = AttachOptions.Fill;
			this.label_path = new Label();
			this.label_path.Name = "label_path";
			this.label_path.Xalign = 1f;
			this.label_path.LabelProp = Catalog.GetString("导出路径:");
			this.table_main.Add(this.label_path);
			Table.TableChild tableChild5 = (Table.TableChild)this.table_main[this.label_path];
			tableChild5.TopAttach = 1U;
			tableChild5.BottomAttach = 2U;
			tableChild5.XOptions = AttachOptions.Fill;
			tableChild5.YOptions = AttachOptions.Fill;
			this.label_plist = new Label();
			this.label_plist.Name = "label_plist";
			this.label_plist.Xalign = 0f;
			this.label_plist.LabelProp = Catalog.GetString("plist文件名");
			this.table_main.Add(this.label_plist);
			Table.TableChild tableChild6 = (Table.TableChild)this.table_main[this.label_plist];
			tableChild6.LeftAttach = 1U;
			tableChild6.RightAttach = 2U;
			tableChild6.YOptions = AttachOptions.Fill;
			this.alignment_main.Add(this.table_main);
			vbox.Add(this.alignment_main);
			Box.BoxChild boxChild5 = (Box.BoxChild)vbox[this.alignment_main];
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 5U;
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			this.hbox_bottomButton = new HBox();
			this.hbox_bottomButton.Name = "hbox_bottomButton";
			this.hbox_bottomButton.Spacing = 8;
			this.buttonOk = new Button();
			this.buttonOk.WidthRequest = 75;
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Label = Catalog.GetString("确定");
			this.hbox_bottomButton.Add(this.buttonOk);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox_bottomButton[this.buttonOk];
			boxChild6.PackType = PackType.End;
			boxChild6.Position = 0;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.buttonCancel = new Button();
			this.buttonCancel.WidthRequest = 75;
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Label = Catalog.GetString("取消");
			this.hbox_bottomButton.Add(this.buttonCancel);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.hbox_bottomButton[this.buttonCancel];
			boxChild7.PackType = PackType.End;
			boxChild7.Position = 1;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			actionArea.Add(this.hbox_bottomButton);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.hbox_bottomButton];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 486;
			base.DefaultHeight = 190;
			base.Show();
			this.button_browse.Clicked += this.HandleButtonBrowseClicked;
			this.buttonCancel.Clicked += this.HandleButtonCancelClicked;
			this.buttonOk.Clicked += this.HandleButtonOKClicked;
		}

		public bool IsRetainEdge
		{
			get
			{
				return this.radiobutton_edge.Active;
			}
		}

		public string ExportDirectory
		{
			get
			{
				return this.entry_path.Text;
			}
		}

		public OptionDialog(string fileName = "")
		{
			this.Build();
			this.SetToDialogStyle(null, true, true, true);
			this.ChangeButtonPosition();
			this.InitLanguage();
			this.buttonOk.Name = "MainButton";
			this.label_plist.Text = fileName;
		}

		private void ChangeButtonPosition()
		{
			if (Platform.IsWindows)
			{
				Box.BoxChild boxChild = (Box.BoxChild)this.hbox_bottomButton[this.buttonOk];
				Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_bottomButton[this.buttonCancel];
				boxChild.Position = 1;
				boxChild2.Position = 0;
			}
		}

		private void InitLanguage()
		{
			base.Title = LanguageInfo.Dialog_ExportPlist_windowTile;
			this.label_name.Text = LanguageInfo.Dialog_ExportPlist_file;
			this.label_path.Text = LanguageInfo.Dialog_ExportPlist_Dir;
			this.label_border.Text = LanguageInfo.Dialog_ExportPlist_EdgeSetting;
			this.radiobutton_noEdge.Label = LanguageInfo.Dialog_ExportPlist_radioBtnClip;
			this.radiobutton_edge.Label = LanguageInfo.Dialog_ExportPlist_radioBtnEdge;
			this.button_browse.Label = LanguageInfo.Dialog_ButtonBrowse;
			this.buttonOk.Label = LanguageInfo.Dialog_ButtonOK;
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonCancel;
		}

		protected void HandleButtonBrowseClicked(object sender, EventArgs e)
		{
			if (Option.IsXP)
			{
				string folder = FileChooserDialogModel.GetBrowseDialogPath(LanguageInfo.Dialog_SelectExportPath, false, this.entry_path.Text, false).Folder;
				if (!string.IsNullOrEmpty(folder))
				{
					this.entry_path.Text = folder;
					return;
				}
			}
			else
			{
				SelectFolderDialog selectFolderDialog = new SelectFolderDialog();
				selectFolderDialog.TransientFor = ApplicationCurrent.MainWindow;
				selectFolderDialog.Title = LanguageInfo.Dialog_SelectExportPath;
				selectFolderDialog.Action = FileChooserAction.SelectFolder;
				selectFolderDialog.SelectMultiple = false;
				selectFolderDialog.CurrentFolder = this.entry_path.Text;
				if (selectFolderDialog.Run() && !string.IsNullOrEmpty(selectFolderDialog.SelectedFile))
				{
					this.entry_path.Text = selectFolderDialog.SelectedFile;
				}
			}
		}

		protected void HandleButtonOKClicked(object sender, EventArgs e)
		{
			string text = this.entry_path.Text;
			if (string.IsNullOrEmpty(text))
			{
				MessageBox.Show(LanguageInfo.Dialog_ExportPathEmpty, MessageBoxImage.Other, null, null);
				return;
			}
			if (!System.IO.Path.IsPathRooted(text))
			{
				MessageBox.Show(LanguageInfo.Dialog_IllegalExportPath, MessageBoxImage.Other, null, null);
				return;
			}
			if (!Option.CheckIsWritableDir(text))
			{
				MessageBox.Show(LanguageInfo.Dialog_IllegalExportPath, MessageBoxImage.Other, null, null);
				return;
			}
			base.Respond(ResponseType.Ok);
		}

		protected void HandleButtonCancelClicked(object sender, EventArgs e)
		{
			base.Respond(ResponseType.Cancel);
		}

		private Alignment alignment_main;

		private Table table_main;

		private HBox hbox_borderOption;

		private RadioButton radiobutton_noEdge;

		private RadioButton radiobutton_edge;

		private HBox hbox_path;

		private Entry entry_path;

		private Button button_browse;

		private Label label_border;

		private Label label_name;

		private Label label_path;

		private Label label_plist;

		private HBox hbox_bottomButton;

		private Button buttonOk;

		private Button buttonCancel;
	}
}
