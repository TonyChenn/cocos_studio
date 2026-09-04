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
	// Token: 0x02000014 RID: 20
	public class ResolutionSettingDialog : Dialog
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00008BF3 File Offset: 0x00006DF3
		public string ResolutionName
		{
			get
			{
				return this.entry_name.Text;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00008C00 File Offset: 0x00006E00
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

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00008C28 File Offset: 0x00006E28
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

		// Token: 0x0600009C RID: 156 RVA: 0x00008C50 File Offset: 0x00006E50
		public ResolutionSettingDialog(int width, int height, string name = null)
		{
			this.Build();
			this.Init();
			this.entry_width.Text = width.ToString();
			this.entry_height.Text = height.ToString();
			this.entry_name.Text = name;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00008CA0 File Offset: 0x00006EA0
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

		// Token: 0x0600009E RID: 158 RVA: 0x00008DA6 File Offset: 0x00006FA6
		private void Apply()
		{
			if (this.CanApply())
			{
				base.Respond(ResponseType.Ok);
				return;
			}
			this.entry_name.GrabFocus();
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00008DE0 File Offset: 0x00006FE0
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

		// Token: 0x060000A0 RID: 160 RVA: 0x00008EBA File Offset: 0x000070BA
		protected void HandleButtonOKClicked(object sender, EventArgs e)
		{
			this.Apply();
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00008EC2 File Offset: 0x000070C2
		protected void HandleButtonCancelClicked(object sender, EventArgs e)
		{
			base.Respond(ResponseType.Cancel);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00008ECC File Offset: 0x000070CC
		protected void HandleDialogDestroyed(object sender, EventArgs e)
		{
			this.parentWnd.Modal = this.isParentModal;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00008EDF File Offset: 0x000070DF
		[ConnectBefore]
		protected void HandleKeyPressEvent(object o, KeyPressEventArgs args)
		{
			if (KeyboardExtend.IsEnterKey(args.Event.Key))
			{
				this.Apply();
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00008EFC File Offset: 0x000070FC
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

		// Token: 0x040000B9 RID: 185
		private const int minValue = 1;

		// Token: 0x040000BA RID: 186
		private const int maxValue = 100000;

		// Token: 0x040000BB RID: 187
		private bool isParentModal;

		// Token: 0x040000BC RID: 188
		private Gtk.Window parentWnd;

		// Token: 0x040000BD RID: 189
		private Table table_main;

		// Token: 0x040000BE RID: 190
		private Entry entry_name;

		// Token: 0x040000BF RID: 191
		private HBox hbox_height;

		// Token: 0x040000C0 RID: 192
		private Entry entry_height;

		// Token: 0x040000C1 RID: 193
		private Label label_px2;

		// Token: 0x040000C2 RID: 194
		private HBox hbox_width;

		// Token: 0x040000C3 RID: 195
		private Entry entry_width;

		// Token: 0x040000C4 RID: 196
		private Label label_px1;

		// Token: 0x040000C5 RID: 197
		private Label label_height;

		// Token: 0x040000C6 RID: 198
		private Label label_name;

		// Token: 0x040000C7 RID: 199
		private Label label_width;

		// Token: 0x040000C8 RID: 200
		private HBox hbox_bottomButton;

		// Token: 0x040000C9 RID: 201
		private Button buttonCancel;

		// Token: 0x040000CA RID: 202
		private Button buttonOk;
	}
}
