using System;
using System.IO;
using System.Text.RegularExpressions;
using CocoStudio.Core;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using CocoStudio.Projects;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;

namespace Modules.Communal.ResourcePanel
{
	public class NewFileDialog : Dialog
	{
		public string FileName { get; private set; }

		public string FileType { get; private set; }

		public float Width { get; private set; }

		public float Height { get; private set; }

		public NewFileDialog(Gtk.Window parentWindow, string parentFolder, Size size)
		{
			this.Build();
			this.defaultFileWidth = size.Width;
			this.defaultFileHeight = size.Height;
			this.parentDir = parentFolder;
			this.InitWidgets();
			this.InitEvent();
			this.InitFileTypeWidgets();
			this.InitLanguages();
			this.SetToDialogStyle(parentWindow, true, true, true);
		}

		private void InitWidgets()
		{
			this.button_OK.Name = "MainButton";
			this.entry_FileName.MaxLength = 50;
			this.evtbx_bottomSeperator.ModifyBg(StateType.Normal, new Color(49, 51, 51));
			this.lab_FileDescribeContent.Sensitive = false;
			this.parentFolder = (Services.ProjectOperations.CurrentResourceGroup.FindResourceItem(this.parentDir) as ResourceFolder);
			this.button_OK.GrabDefault();
			this.entry_FileName.ActivatesDefault = true;
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox_bottomBtn[this.button_OK];
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_bottomBtn[this.button_OK];
			if (Platform.IsWindows)
			{
				boxChild.Position = 1;
				boxChild2.Position = 0;
				return;
			}
			if (Platform.IsMac)
			{
				boxChild.Position = 0;
				boxChild2.Position = 1;
			}
		}

		private void InitEvent()
		{
			base.KeyPressEvent += this.KeyPressEventHandler;
			this.button_OK.Clicked += this.HandleButtonOKClicked;
			this.button_cancel.Clicked += this.HandleButtonCancelClicked;
			this.entry_width.Changed += this.HandleEntryChanged;
			this.entry_height.Changed += this.HandleEntryChanged;
			this.entry_FileName.Changed += this.HandleEntryChanged;
			this.entry_FileName.Changed += this.FileNameEntryChangedHandler;
		}

		private void InitFileTypeWidgets()
		{
			this.itemGroup = new FileTypeItemGroup();
			this.itemGroup.SelectedChanged += this.HandleFileTypeItemChanged;
			foreach (IProjectFileCreator view in ProjectFileTemplateService.ProjectFileTemplateList)
			{
				FileTypeItem fileTypeItem = new FileTypeItem(view);
				fileTypeItem.DoubleClicked += this.IconDoubleClickedHandler;
				this.itemGroup.Add(fileTypeItem);
				this.hbox_FileType.Add(fileTypeItem);
				fileTypeItem.Show();
			}
		}

		private void InitLanguages()
		{
			base.Title = LanguageInfo.NewFile_Title;
			this.button_OK.Label = LanguageInfo.Dialog_ButtonNew;
			this.button_cancel.Label = LanguageInfo.Dialog_ButtonCancel;
			this.lab_FileName.LabelProp = LanguageInfo.NewFile_FileName;
			this.lab_FileType.LabelProp = LanguageInfo.Animation_Type;
			this.lab_FileDescribe.LabelProp = LanguageInfo.NewFile_FileDescription;
			this.lab_FileSize.LabelProp = LanguageInfo.NewFile_FileSize;
			this.label_px1.LabelProp = (this.label_px2.LabelProp = LanguageInfo.NewFile_Pixel);
			this.label_width.LabelProp = LanguageInfo.NewFile_Width;
			this.label_height.LabelProp = LanguageInfo.NewFile_Height;
			this.lab_FileDescribeContent.LabelProp = LanguageInfo.NewFile_SceneDes;
		}

		private string GetNewFileName(string fileName, string parentDir, string extension)
		{
			string text = System.IO.Path.Combine(parentDir, fileName + extension);
			string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);
			int num = 1;
			while (File.Exists(text) || this.IsExistChild(this.parentFolder, text))
			{
				string path = fileNameWithoutExtension + num + extension;
				text = System.IO.Path.Combine(parentDir, path);
				num++;
			}
			return System.IO.Path.GetFileNameWithoutExtension(text);
		}

		private bool IsExistChild(ResourceFolder folder, string filePath)
		{
			if (folder == null || folder.Items == null)
			{
				return false;
			}
			foreach (ResourceItem resourceItem in folder.Items)
			{
				if (resourceItem.FullPath == filePath)
				{
					return true;
				}
			}
			return false;
		}

		private void ChangeSelectFileType(IProjectFileCreator view)
		{
			if (!this.hasNameChanged)
			{
				string newFileName = this.GetNewFileName(view.FileType.ToString(), this.parentDir, view.FileExtension);
				this.entry_FileName.Text = newFileName;
				this.hasNameChanged = false;
			}
			this.entry_FileName.SelectAll();
			this.entry_FileName.HasFocus = true;
			this.lab_FileDescribeContent.Text = view.Description;
			if (view.MaxSize > 0)
			{
				this.RefreshWidthHeightWidget(this.defaultFileWidth, this.defaultFileHeight, view.CanEditSize);
				return;
			}
			this.RefreshWidthHeightWidget(view.MaxSize, view.MaxSize, view.CanEditSize);
		}

		private void RefreshWidthHeightWidget(int w, int h, bool editable)
		{
			this.label_px1.Sensitive = editable;
			this.label_px2.Sensitive = editable;
			this.label_width.Sensitive = editable;
			this.label_height.Sensitive = editable;
			this.entry_width.Sensitive = editable;
			this.entry_height.Sensitive = editable;
			if (w != -1)
			{
				this.entry_width.Text = w.ToString();
			}
			else
			{
				this.entry_width.Text = string.Empty;
			}
			if (h != -1)
			{
				this.entry_height.Text = h.ToString();
				return;
			}
			this.entry_height.Text = string.Empty;
		}

		private bool CheckValidity(string extension)
		{
			if (string.IsNullOrWhiteSpace(this.entry_FileName.Text))
			{
				MessageBox.Show(LanguageInfo.MessageBox_Content110, MessageBoxImage.Other, null, null);
				this.entry_FileName.SelectAll();
				this.entry_FileName.HasFocus = true;
				return false;
			}
			if (RegexModel.IsSystemReserveName(this.entry_FileName.Text))
			{
				MessageBox.Show(LanguageInfo.MessageBox215_WindowsNameLimit, MessageBoxImage.Other, null, null);
				this.entry_FileName.SelectAll();
				this.entry_FileName.HasFocus = true;
				return false;
			}
			string text = this.entry_FileName.Text + extension;
			if (!Regex.IsMatch(text, "^[A-Za-z0-9,._-]+$") || Regex.IsMatch(text, "[\\*\\\\/:?<>|\"]"))
			{
				MessageBox.Show(LanguageInfo.MessageBox216_FileNameLimit, MessageBoxImage.Other, null, null);
				this.entry_FileName.SelectAll();
				this.entry_FileName.HasFocus = true;
				return false;
			}
			if (!FileService.IsValidFileName(text))
			{
				string info = LanguageInfo.NewFile_WrongName.Replace("<", "&lt;").Replace(">", "&gt;");
				MessageBox.Show(info, MessageBoxImage.Other, null, null);
				this.entry_FileName.SelectAll();
				this.entry_FileName.HasFocus = true;
				return false;
			}
			string text2 = System.IO.Path.Combine(this.parentDir, text);
			if (File.Exists(text2) || this.IsExistChild(this.parentFolder, text2))
			{
				MessageBox.Show(LanguageInfo.NewFile_SameName, MessageBoxImage.Other, null, null);
				this.entry_FileName.SelectAll();
				this.entry_FileName.HasFocus = true;
				return false;
			}
			return this.CheckIsWidthHeightValid(this.entry_width, true) && this.CheckIsWidthHeightValid(this.entry_height, false);
		}

		private bool CheckIsWidthHeightValid(Entry entry, bool isWidth)
		{
			if (this.itemGroup.SelectedItem.FileView.FileType != NodeType.Layer)
			{
				return true;
			}
			bool flag = false;
			int num;
			if (int.TryParse(entry.Text, out num) && num >= 1 && num <= 4096)
			{
				flag = true;
			}
			if (!flag)
			{
				string arg = isWidth ? LanguageInfo.Display_Width : LanguageInfo.Display_Height;
				MessageBox.Show(string.Format(LanguageInfo.MessageBox262_invalidSize, arg, 1, 4096), MessageBoxImage.Other, null, null);
				entry.SelectAll();
				entry.HasFocus = true;
			}
			return flag;
		}

		private void RefreshOKButton()
		{
			if (string.IsNullOrWhiteSpace(this.entry_FileName.Text))
			{
				this.button_OK.Sensitive = false;
				return;
			}
			if (this.itemGroup.SelectedItem.FileView.FileType == NodeType.Plist)
			{
				this.button_OK.Sensitive = true;
				return;
			}
			if (string.IsNullOrWhiteSpace(this.entry_width.Text) || string.IsNullOrWhiteSpace(this.entry_height.Text))
			{
				this.button_OK.Sensitive = false;
				return;
			}
			this.button_OK.Sensitive = true;
		}

		private void FileNameEntryChangedHandler(object sender, EventArgs e)
		{
			this.hasNameChanged = true;
		}

		private void HandleEntryChanged(object sender, EventArgs e)
		{
			this.RefreshOKButton();
		}

		private void HandleButtonOKClicked(object sender, EventArgs e)
		{
			IProjectFileCreator fileView = this.itemGroup.SelectedItem.FileView;
			if (this.CheckValidity(fileView.FileExtension))
			{
				this.FileName = this.entry_FileName.Text + fileView.FileExtension;
				this.FileType = fileView.FileType.ToString();
				int num;
				if (int.TryParse(this.entry_width.Text, out num))
				{
					this.Width = (float)num;
				}
				int num2;
				if (int.TryParse(this.entry_height.Text, out num2))
				{
					this.Height = (float)num2;
				}
				base.Respond(ResponseType.Ok);
			}
		}

		private void HandleButtonCancelClicked(object sender, EventArgs e)
		{
			base.Respond(ResponseType.Cancel);
		}

		private void HandleFileTypeItemChanged(object sender, FileTypeItemChangedArgs e)
		{
			this.ChangeSelectFileType(e.Item.FileView);
		}

		[ConnectBefore]
		private void KeyPressEventHandler(object o, KeyPressEventArgs args)
		{
			if (KeyboardExtend.IsEnterKey(args.Event.Key) && this.button_OK.Sensitive && !this.entry_FileName.HasFocus)
			{
				this.HandleButtonOKClicked(o, args);
			}
		}

		private void IconDoubleClickedHandler(object sender, EventArgs e)
		{
			if (this.button_OK.Sensitive)
			{
				this.button_OK.Activate();
			}
		}

		protected void DialogSizeAllocatedHandler(object o, SizeAllocatedArgs args)
		{
			this.lab_FileDescribeContent.WidthRequest = this.entry_FileName.Allocation.Width;
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "Modules.Communal.ResourcePanel.NewFileDialog";
			base.Title = Catalog.GetString("新建文件");
			base.TypeHint = WindowTypeHint.Dialog;
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog_VBox";
			vbox.BorderWidth = 2U;
			this.alignment_main = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_main.Name = "alignment_main";
			this.tab_Root = new Table(4U, 2U, false);
			this.tab_Root.Name = "tab_Root";
			this.tab_Root.RowSpacing = 12U;
			this.tab_Root.ColumnSpacing = 18U;
			this.tab_Root.BorderWidth = 15U;
			this.entry_FileName = new Entry();
			this.entry_FileName.CanFocus = true;
			this.entry_FileName.Name = "entry_FileName";
			this.entry_FileName.IsEditable = true;
			this.entry_FileName.InvisibleChar = '●';
			this.tab_Root.Add(this.entry_FileName);
			Table.TableChild tableChild = (Table.TableChild)this.tab_Root[this.entry_FileName];
			tableChild.LeftAttach = 1U;
			tableChild.RightAttach = 2U;
			tableChild.XOptions = AttachOptions.Fill;
			tableChild.YOptions = AttachOptions.Fill;
			this.hbox_FileType = new HBox();
			this.hbox_FileType.Name = "hbox_FileType";
			this.tab_Root.Add(this.hbox_FileType);
			Table.TableChild tableChild2 = (Table.TableChild)this.tab_Root[this.hbox_FileType];
			tableChild2.TopAttach = 1U;
			tableChild2.BottomAttach = 2U;
			tableChild2.LeftAttach = 1U;
			tableChild2.RightAttach = 2U;
			tableChild2.XOptions = AttachOptions.Fill;
			tableChild2.YOptions = AttachOptions.Fill;
			this.lab_FileDescribe = new Label();
			this.lab_FileDescribe.Name = "lab_FileDescribe";
			this.lab_FileDescribe.Xalign = 1f;
			this.lab_FileDescribe.Yalign = 0f;
			this.lab_FileDescribe.LabelProp = Catalog.GetString("描述");
			this.tab_Root.Add(this.lab_FileDescribe);
			Table.TableChild tableChild3 = (Table.TableChild)this.tab_Root[this.lab_FileDescribe];
			tableChild3.TopAttach = 2U;
			tableChild3.BottomAttach = 3U;
			tableChild3.XOptions = AttachOptions.Fill;
			tableChild3.YOptions = AttachOptions.Fill;
			this.lab_FileDescribeContent = new Label();
			this.lab_FileDescribeContent.HeightRequest = 60;
			this.lab_FileDescribeContent.Name = "lab_FileDescribeContent";
			this.lab_FileDescribeContent.Xalign = 0f;
			this.lab_FileDescribeContent.Yalign = 0f;
			this.lab_FileDescribeContent.LabelProp = Catalog.GetString("描述文件");
			this.lab_FileDescribeContent.Wrap = true;
			this.tab_Root.Add(this.lab_FileDescribeContent);
			Table.TableChild tableChild4 = (Table.TableChild)this.tab_Root[this.lab_FileDescribeContent];
			tableChild4.TopAttach = 2U;
			tableChild4.BottomAttach = 3U;
			tableChild4.LeftAttach = 1U;
			tableChild4.RightAttach = 2U;
			tableChild4.YOptions = AttachOptions.Fill;
			this.lab_FileName = new Label();
			this.lab_FileName.Name = "lab_FileName";
			this.lab_FileName.Xalign = 1f;
			this.lab_FileName.LabelProp = Catalog.GetString("文件名称");
			this.tab_Root.Add(this.lab_FileName);
			Table.TableChild tableChild5 = (Table.TableChild)this.tab_Root[this.lab_FileName];
			tableChild5.XOptions = AttachOptions.Fill;
			tableChild5.YOptions = AttachOptions.Fill;
			this.lab_FileSize = new Label();
			this.lab_FileSize.Name = "lab_FileSize";
			this.lab_FileSize.Xalign = 1f;
			this.lab_FileSize.Yalign = 0.1f;
			this.lab_FileSize.LabelProp = Catalog.GetString("大小");
			this.tab_Root.Add(this.lab_FileSize);
			Table.TableChild tableChild6 = (Table.TableChild)this.tab_Root[this.lab_FileSize];
			tableChild6.TopAttach = 3U;
			tableChild6.BottomAttach = 4U;
			tableChild6.XOptions = AttachOptions.Fill;
			tableChild6.YOptions = AttachOptions.Fill;
			this.lab_FileType = new Label();
			this.lab_FileType.Name = "lab_FileType";
			this.lab_FileType.Xalign = 1f;
			this.lab_FileType.Yalign = 0.3f;
			this.lab_FileType.LabelProp = Catalog.GetString("类型");
			this.tab_Root.Add(this.lab_FileType);
			Table.TableChild tableChild7 = (Table.TableChild)this.tab_Root[this.lab_FileType];
			tableChild7.TopAttach = 1U;
			tableChild7.BottomAttach = 2U;
			tableChild7.XOptions = AttachOptions.Fill;
			tableChild7.YOptions = AttachOptions.Fill;
			this.table_widthHeight = new Table(2U, 4U, false);
			this.table_widthHeight.Name = "table_widthHeight";
			this.table_widthHeight.RowSpacing = 6U;
			this.table_widthHeight.ColumnSpacing = 6U;
			this.alignment_widthPx = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_widthPx.Name = "alignment_widthPx";
			this.alignment_widthPx.RightPadding = 25U;
			this.label_px1 = new Label();
			this.label_px1.Name = "label_px1";
			this.label_px1.LabelProp = Catalog.GetString("px");
			this.alignment_widthPx.Add(this.label_px1);
			this.table_widthHeight.Add(this.alignment_widthPx);
			Table.TableChild tableChild8 = (Table.TableChild)this.table_widthHeight[this.alignment_widthPx];
			tableChild8.LeftAttach = 1U;
			tableChild8.RightAttach = 2U;
			tableChild8.XOptions = AttachOptions.Fill;
			tableChild8.YOptions = AttachOptions.Fill;
			this.evtbx_height = new EventBox();
			this.evtbx_height.Name = "evtbx_height";
			this.entry_height = new Entry();
			this.entry_height.WidthRequest = 10;
			this.entry_height.CanFocus = true;
			this.entry_height.Name = "entry_height";
			this.entry_height.IsEditable = true;
			this.entry_height.InvisibleChar = '●';
			this.evtbx_height.Add(this.entry_height);
			this.table_widthHeight.Add(this.evtbx_height);
			Table.TableChild tableChild9 = (Table.TableChild)this.table_widthHeight[this.evtbx_height];
			tableChild9.LeftAttach = 2U;
			tableChild9.RightAttach = 3U;
			tableChild9.YOptions = AttachOptions.Fill;
			this.evtbx_width = new EventBox();
			this.evtbx_width.Name = "evtbx_width";
			this.entry_width = new Entry();
			this.entry_width.WidthRequest = 10;
			this.entry_width.CanFocus = true;
			this.entry_width.Name = "entry_width";
			this.entry_width.IsEditable = true;
			this.entry_width.InvisibleChar = '●';
			this.evtbx_width.Add(this.entry_width);
			this.table_widthHeight.Add(this.evtbx_width);
			Table.TableChild tableChild10 = (Table.TableChild)this.table_widthHeight[this.evtbx_width];
			tableChild10.YOptions = AttachOptions.Fill;
			this.label_height = new Label();
			this.label_height.Name = "label_height";
			this.label_height.LabelProp = Catalog.GetString("高");
			this.table_widthHeight.Add(this.label_height);
			Table.TableChild tableChild11 = (Table.TableChild)this.table_widthHeight[this.label_height];
			tableChild11.TopAttach = 1U;
			tableChild11.BottomAttach = 2U;
			tableChild11.LeftAttach = 2U;
			tableChild11.RightAttach = 3U;
			tableChild11.XOptions = AttachOptions.Fill;
			tableChild11.YOptions = AttachOptions.Fill;
			this.label_px2 = new Label();
			this.label_px2.Name = "label_px2";
			this.label_px2.LabelProp = Catalog.GetString("px");
			this.table_widthHeight.Add(this.label_px2);
			Table.TableChild tableChild12 = (Table.TableChild)this.table_widthHeight[this.label_px2];
			tableChild12.LeftAttach = 3U;
			tableChild12.RightAttach = 4U;
			tableChild12.XOptions = AttachOptions.Fill;
			tableChild12.YOptions = AttachOptions.Fill;
			this.label_width = new Label();
			this.label_width.Name = "label_width";
			this.label_width.LabelProp = Catalog.GetString("宽");
			this.table_widthHeight.Add(this.label_width);
			Table.TableChild tableChild13 = (Table.TableChild)this.table_widthHeight[this.label_width];
			tableChild13.TopAttach = 1U;
			tableChild13.BottomAttach = 2U;
			tableChild13.XOptions = AttachOptions.Fill;
			tableChild13.YOptions = AttachOptions.Fill;
			this.tab_Root.Add(this.table_widthHeight);
			Table.TableChild tableChild14 = (Table.TableChild)this.tab_Root[this.table_widthHeight];
			tableChild14.TopAttach = 3U;
			tableChild14.BottomAttach = 4U;
			tableChild14.LeftAttach = 1U;
			tableChild14.RightAttach = 2U;
			tableChild14.XOptions = AttachOptions.Fill;
			tableChild14.YOptions = AttachOptions.Fill;
			this.alignment_main.Add(this.tab_Root);
			vbox.Add(this.alignment_main);
			Box.BoxChild boxChild = (Box.BoxChild)vbox[this.alignment_main];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.evtbx_bottomSeperator = new EventBox();
			this.evtbx_bottomSeperator.HeightRequest = 1;
			this.evtbx_bottomSeperator.Name = "evtbx_bottomSeperator";
			vbox.Add(this.evtbx_bottomSeperator);
			Box.BoxChild boxChild2 = (Box.BoxChild)vbox[this.evtbx_bottomSeperator];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 5U;
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			this.hbox_bottomBtn = new HBox();
			this.hbox_bottomBtn.Name = "hbox_bottomBtn";
			this.hbox_bottomBtn.Spacing = 10;
			this.button_cancel = new Button();
			this.button_cancel.WidthRequest = 75;
			this.button_cancel.CanDefault = true;
			this.button_cancel.CanFocus = true;
			this.button_cancel.Name = "button_cancel";
			this.button_cancel.UseUnderline = true;
			this.button_cancel.Label = Catalog.GetString("取消");
			this.hbox_bottomBtn.Add(this.button_cancel);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_bottomBtn[this.button_cancel];
			boxChild3.Position = 0;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.button_OK = new Button();
			this.button_OK.WidthRequest = 75;
			this.button_OK.CanDefault = true;
			this.button_OK.CanFocus = true;
			this.button_OK.Name = "button_OK";
			this.button_OK.UseUnderline = true;
			this.button_OK.Label = Catalog.GetString("确定");
			this.hbox_bottomBtn.Add(this.button_OK);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox_bottomBtn[this.button_OK];
			boxChild4.Position = 1;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			actionArea.Add(this.hbox_bottomBtn);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.hbox_bottomBtn];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 444;
			base.DefaultHeight = 260;
			base.Hide();
			base.SizeAllocated += this.DialogSizeAllocatedHandler;
		}

		private const int minSize = 1;

		private const int maxSize = 4096;

		private string parentDir;

		private int defaultFileWidth;

		private int defaultFileHeight;

		private FileTypeItemGroup itemGroup;

		private bool hasNameChanged;

		private ResourceFolder parentFolder;

		private Alignment alignment_main;

		private Table tab_Root;

		private Entry entry_FileName;

		private HBox hbox_FileType;

		private Label lab_FileDescribe;

		private Label lab_FileDescribeContent;

		private Label lab_FileName;

		private Label lab_FileSize;

		private Label lab_FileType;

		private Table table_widthHeight;

		private Alignment alignment_widthPx;

		private Label label_px1;

		private EventBox evtbx_height;

		private Entry entry_height;

		private EventBox evtbx_width;

		private Entry entry_width;

		private Label label_height;

		private Label label_px2;

		private Label label_width;

		private EventBox evtbx_bottomSeperator;

		private HBox hbox_bottomBtn;

		private Button button_cancel;

		private Button button_OK;
	}
}
