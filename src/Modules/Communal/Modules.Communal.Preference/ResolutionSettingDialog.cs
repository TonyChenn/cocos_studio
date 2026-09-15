using System;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using Stetic;

namespace Modules.Communal.Preference
{
	public class ResolutionSettingDialog : Dialog
	{
		public string ResolutionName
		{
			get
			{
				return this.entry_name.Text;
			}
		}

		public int Width
		{
			get
			{
				int result;
				if (int.TryParse(this.entry_width.Text, out result))
				{
					return result;
				}
				return 100;
			}
		}

		public int Height
		{
			get
			{
				int result;
				if (int.TryParse(this.entry_height.Text, out result))
				{
					return result;
				}
				return 100;
			}
		}

		public ResolutionSettingDialog(int width, int height, string name = null)
		{
			this.Build();
			this.Init();
			this.entry_width.Text = width.ToString();
			this.entry_height.Text = height.ToString();
			this.entry_name.Text = name;
		}

		private void Init()
		{
			this.label_name.Text = LanguageInfo.Display_Name;
			this.label_width.Text = LanguageInfo.Display_Width;
			this.label_height.Text = LanguageInfo.Display_Height;
			this.buttonOk.Label = LanguageInfo.Dialog_ButtonOK;
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonCancel;
			this.buttonOk.Name = "MainButton";
			this.entry_name.MaxLength = 30;
			if (Platform.IsWindows)
			{
				Box.BoxChild boxChild = (Box.BoxChild)this.hbox_bottomButton[this.buttonCancel];
				Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_bottomButton[this.buttonOk];
				boxChild2.Position = 0;
				boxChild.Position = 1;
			}
			this.parentWnd = MessageService.GetDefaultModalParent();
			this.isParentModal = this.parentWnd.Modal;
			this.parentWnd.Modal = false;
			base.Destroyed += this.HandleDialogDestroyed;
			this.SetToDialogStyle(this.parentWnd, true, true, true);
		}

		private void Apply()
		{
			if (this.CanApply())
			{
				base.Respond(ResponseType.Ok);
				return;
			}
			this.entry_name.GrabFocus();
		}

		private bool CanApply()
		{
			bool flag = false;
			int num;
			if (int.TryParse(this.entry_width.Text, out num) && num >= 1 && num <= 100000)
			{
				flag = true;
			}
			if (!flag)
			{
				MessageBox.Show(string.Format(LanguageInfo.MessageBox251_resolutionSizeLimit, 1, 100000), MessageBoxImage.Other, null, null);
				GLib.Timeout.Add(10U, delegate
				{
					this.entry_width.GrabFocus();
					return false;
				});
				return false;
			}
			flag = false;
			if (int.TryParse(this.entry_height.Text, out num) && num >= 1 && num <= 100000)
			{
				flag = true;
			}
			if (!flag)
			{
				MessageBox.Show(string.Format(LanguageInfo.MessageBox251_resolutionSizeLimit, 1, 100000), MessageBoxImage.Other, null, null);
				GLib.Timeout.Add(10U, delegate
				{
					this.entry_height.GrabFocus();
					return false;
				});
				return false;
			}
			return true;
		}

		protected void HandleButtonOKClicked(object sender, EventArgs e)
		{
			this.Apply();
		}

		protected void HandleButtonCancelClicked(object sender, EventArgs e)
		{
			base.Respond(ResponseType.Cancel);
		}

		protected void HandleDialogDestroyed(object sender, EventArgs e)
		{
			this.parentWnd.Modal = this.isParentModal;
		}

		[ConnectBefore]
		protected void HandleKeyPressEvent(object o, KeyPressEventArgs args)
		{
			if (KeyboardExtend.IsEnterKey(args.Event.Key))
			{
				this.Apply();
			}
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "Modules.Communal.Preference.ResolutionSettingDialog";
			base.Title = Catalog.GetString("编辑分辨率");
			base.TypeHint = WindowTypeHint.Dialog;
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.BorderWidth = 8U;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog_VBox";
			vbox.BorderWidth = 2U;
			this.table_main = new Table(3U, 2U, false);
			this.table_main.WidthRequest = 250;
			this.table_main.Name = "table_main";
			this.table_main.RowSpacing = 6U;
			this.table_main.ColumnSpacing = 6U;
			this.table_main.BorderWidth = 12U;
			this.entry_name = new Entry();
			this.entry_name.CanFocus = true;
			this.entry_name.Name = "entry_name";
			this.entry_name.Text = Catalog.GetString("Default");
			this.entry_name.IsEditable = true;
			this.entry_name.InvisibleChar = '●';
			this.table_main.Add(this.entry_name);
			Table.TableChild tableChild = (Table.TableChild)this.table_main[this.entry_name];
			tableChild.LeftAttach = 1U;
			tableChild.RightAttach = 2U;
			tableChild.YOptions = AttachOptions.Fill;
			this.hbox_height = new HBox();
			this.hbox_height.Name = "hbox_height";
			this.hbox_height.Spacing = 6;
			this.entry_height = new Entry();
			this.entry_height.CanFocus = true;
			this.entry_height.Name = "entry_height";
			this.entry_height.IsEditable = true;
			this.entry_height.InvisibleChar = '●';
			this.hbox_height.Add(this.entry_height);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox_height[this.entry_height];
			boxChild.Position = 0;
			this.label_px2 = new Label();
			this.label_px2.Name = "label_px2";
			this.label_px2.LabelProp = Catalog.GetString("px");
			this.hbox_height.Add(this.label_px2);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_height[this.label_px2];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.table_main.Add(this.hbox_height);
			Table.TableChild tableChild2 = (Table.TableChild)this.table_main[this.hbox_height];
			tableChild2.TopAttach = 2U;
			tableChild2.BottomAttach = 3U;
			tableChild2.LeftAttach = 1U;
			tableChild2.RightAttach = 2U;
			tableChild2.YOptions = AttachOptions.Fill;
			this.hbox_width = new HBox();
			this.hbox_width.Name = "hbox_width";
			this.hbox_width.Spacing = 6;
			this.entry_width = new Entry();
			this.entry_width.CanFocus = true;
			this.entry_width.Name = "entry_width";
			this.entry_width.IsEditable = true;
			this.entry_width.InvisibleChar = '●';
			this.hbox_width.Add(this.entry_width);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_width[this.entry_width];
			boxChild3.Position = 0;
			this.label_px1 = new Label();
			this.label_px1.Name = "label_px1";
			this.label_px1.LabelProp = Catalog.GetString("px");
			this.hbox_width.Add(this.label_px1);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox_width[this.label_px1];
			boxChild4.Position = 1;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.table_main.Add(this.hbox_width);
			Table.TableChild tableChild3 = (Table.TableChild)this.table_main[this.hbox_width];
			tableChild3.TopAttach = 1U;
			tableChild3.BottomAttach = 2U;
			tableChild3.LeftAttach = 1U;
			tableChild3.RightAttach = 2U;
			tableChild3.YOptions = AttachOptions.Fill;
			this.label_height = new Label();
			this.label_height.Name = "label_height";
			this.label_height.Xalign = 1f;
			this.label_height.LabelProp = Catalog.GetString("高度");
			this.table_main.Add(this.label_height);
			Table.TableChild tableChild4 = (Table.TableChild)this.table_main[this.label_height];
			tableChild4.TopAttach = 2U;
			tableChild4.BottomAttach = 3U;
			tableChild4.XOptions = AttachOptions.Fill;
			tableChild4.YOptions = AttachOptions.Fill;
			this.label_name = new Label();
			this.label_name.Name = "label_name";
			this.label_name.Xalign = 1f;
			this.label_name.LabelProp = Catalog.GetString("名称");
			this.table_main.Add(this.label_name);
			Table.TableChild tableChild5 = (Table.TableChild)this.table_main[this.label_name];
			tableChild5.XOptions = AttachOptions.Fill;
			tableChild5.YOptions = AttachOptions.Fill;
			this.label_width = new Label();
			this.label_width.Name = "label_width";
			this.label_width.Xalign = 1f;
			this.label_width.LabelProp = Catalog.GetString("宽度");
			this.table_main.Add(this.label_width);
			Table.TableChild tableChild6 = (Table.TableChild)this.table_main[this.label_width];
			tableChild6.TopAttach = 1U;
			tableChild6.BottomAttach = 2U;
			tableChild6.XOptions = AttachOptions.Fill;
			tableChild6.YOptions = AttachOptions.Fill;
			vbox.Add(this.table_main);
			Box.BoxChild boxChild5 = (Box.BoxChild)vbox[this.table_main];
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog1_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 5U;
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			this.hbox_bottomButton = new HBox();
			this.hbox_bottomButton.Name = "hbox_bottomButton";
			this.hbox_bottomButton.Spacing = 6;
			this.buttonCancel = new Button();
			this.buttonCancel.WidthRequest = 75;
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = Catalog.GetString("取消(_C)");
			this.hbox_bottomButton.Add(this.buttonCancel);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox_bottomButton[this.buttonCancel];
			boxChild6.Position = 0;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.buttonOk = new Button();
			this.buttonOk.WidthRequest = 75;
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = Catalog.GetString("确定(_O)");
			this.hbox_bottomButton.Add(this.buttonOk);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.hbox_bottomButton[this.buttonOk];
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
			base.DefaultWidth = 270;
			base.DefaultHeight = 175;
			base.Hide();
			base.KeyPressEvent += this.HandleKeyPressEvent;
			this.buttonCancel.Clicked += this.HandleButtonCancelClicked;
			this.buttonOk.Clicked += this.HandleButtonOKClicked;
		}

		private const int minValue = 1;

		private const int maxValue = 100000;

		private bool isParentModal;

		private Gtk.Window parentWnd;

		private Table table_main;

		private Entry entry_name;

		private HBox hbox_height;

		private Entry entry_height;

		private Label label_px2;

		private HBox hbox_width;

		private Entry entry_width;

		private Label label_px1;

		private Label label_height;

		private Label label_name;

		private Label label_width;

		private HBox hbox_bottomButton;

		private Button buttonCancel;

		private Button buttonOk;
	}
}
