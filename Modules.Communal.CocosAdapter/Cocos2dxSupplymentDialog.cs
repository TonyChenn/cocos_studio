using System;
using System.IO;
using CocoStudio.DefaultResource;
using CocoStudio.Projects;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using Pango;
using Stetic;
using Xwt.Drawing;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x02000029 RID: 41
	public class Cocos2dxSupplymentDialog : Dialog
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600014E RID: 334 RVA: 0x0000641C File Offset: 0x0000461C
		// (set) Token: 0x0600014F RID: 335 RVA: 0x00006424 File Offset: 0x00004624
		public EnumProgramLanguage Language { get; private set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000150 RID: 336 RVA: 0x0000642D File Offset: 0x0000462D
		// (set) Token: 0x06000151 RID: 337 RVA: 0x00006435 File Offset: 0x00004635
		public string FrameworkVersion { get; private set; }

		// Token: 0x06000152 RID: 338 RVA: 0x0000643E File Offset: 0x0000463E
		public Cocos2dxSupplymentDialog(bool isIDE, EnumOperationType operationType)
		{
			this.Build();
			this.isSupplymentIDE = isIDE;
			this.InitEvent();
			this.InitWidgets();
			this.InitStyle(operationType);
			this.SetToDialogStyle(null, true, true, true);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00006470 File Offset: 0x00004670
		private void InitEvent()
		{
			this.combobox_engine.Changed += this.ComboboxEngineChangedHandler;
			this.radiobutton1_lua.Toggled += this.RadioButtonToggledHandler;
			this.radiobutton2_cpp.Toggled += this.RadioButtonToggledHandler;
			this.radiobutton3_js.Toggled += this.RadioButtonToggledHandler;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000064DC File Offset: 0x000046DC
		private void InitWidgets()
		{
			Stream resourceStream = Resources.GetResourceStream("CocoStudio.DefaultResource.Images.MessageBoxIcon.Warning.png");
			Xwt.Drawing.Image imageView = Xwt.Drawing.Image.FromStream(resourceStream);
			this.imagebin_warring.SetImageView(imageView);
			HelpButton helpButton = new HelpButton(null);
			this.alignment_help.Add(helpButton);
			helpButton.Show();
			helpButton.URL = LanguageAdapter.GetLocalizedUrl(HelpLinkUrl.PublishDataFormat);
			if (Cocos2dxServices.CocosProperties.SolutionCodeType == EnumSolutionCodeType.CodeIDE)
			{
				this.Language = Cocos2dxServices.CocosProperties.ProgramLanguage;
				this.table_main.Remove(this.label_language);
				this.table_main.Remove(this.table_language);
			}
			else
			{
				this.radiobutton1_lua.Active = true;
				this.Language = EnumProgramLanguage.lua;
				if (this.isSupplymentIDE)
				{
					this.radiobutton2_cpp.Sensitive = false;
				}
			}
			int count = FrameworkHelper.EnabledVersions.Count;
			if (count == 0)
			{
				this.combobox_engine.AppendText(LanguageInfo.Color_none);
				this.combobox_engine.Active = 0;
				this.FrameworkVersion = "IDE cocos";
			}
			else
			{
				foreach (string text in FrameworkHelper.EnabledVersions)
				{
					this.combobox_engine.AppendText(text);
					this.combobox_engine.Active = count - 1;
					this.FrameworkVersion = this.combobox_engine.ActiveText;
				}
			}
			this.RefreshPubishDirWarning();
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00006640 File Offset: 0x00004840
		private void InitStyle(EnumOperationType operationType)
		{
			this.buttonOk.Name = "MainButton";
			this.buttonOk.Label = LanguageInfo.Dialog_ButtonOK;
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonCancel;
			this.label_prompt.Text = LanguageOption.GetValueBykey("CodeSupplyment_NeedUpgrade" + operationType.ToString());
			this.label_prompt.LineWrapMode = Pango.WrapMode.WordChar;
			this.label_publishDir.LineWrapMode = Pango.WrapMode.WordChar;
			this.GtkLabel_upgrade.Text = " " + LanguageInfo.CodeSupplyment_Setting + " ";
			this.label_framework.Text = LanguageInfo.Dialog_New_EngineVersion;
			this.label_language.Text = LanguageInfo.Dialog_Publish_Language;
			this.label_luaDes.Text = LanguageInfo.NewSolution_LuaDes;
			this.label_cppDes.Text = LanguageInfo.NewSolution_CppDes;
			this.label_jsDes.Text = LanguageInfo.NewSolution_JsDes;
			base.Title = LanguageInfo.CodeSupplyment_ProjectUpgrade;
			if (MonoDevelop.Core.Platform.IsWindows)
			{
				HButtonBox actionArea = base.ActionArea;
				ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.buttonOk];
				ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[this.buttonCancel];
				buttonBoxChild.Position = 0;
				buttonBoxChild2.Position = 1;
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00006774 File Offset: 0x00004974
		private void RefreshPubishDirWarning()
		{
			if (Cocos2dxServices.SupplymentServices.CheckNeedResetPublishDir(this.Language))
			{
				string defaultPublishDir = Cocos2dxServices.SupplymentServices.GetDefaultPublishDir(this.Language);
				this.label_publishDir.Text = string.Format(LanguageInfo.CodeSupplyment_ResetPublishDir, defaultPublishDir);
				this.alignment_publishWarning.AddChild(this.hbox_publishDirWarning);
				return;
			}
			this.alignment_publishWarning.RemoveChild();
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000067D8 File Offset: 0x000049D8
		private void ComboboxEngineChangedHandler(object sender, EventArgs e)
		{
			if (this.combobox_engine.Active == -1)
			{
				return;
			}
			if (this.combobox_engine.ActiveText.Equals(LanguageInfo.Color_none))
			{
				this.FrameworkVersion = "IDE cocos";
				this.radiobutton2_cpp.Sensitive = false;
				if (this.radiobutton2_cpp.Active)
				{
					this.radiobutton1_lua.Active = true;
					return;
				}
			}
			else
			{
				this.FrameworkVersion = this.combobox_engine.ActiveText;
				if (!this.isSupplymentIDE)
				{
					this.radiobutton2_cpp.Sensitive = true;
				}
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00006861 File Offset: 0x00004A61
		private void RadioButtonToggledHandler(object sender, EventArgs e)
		{
			if (this.radiobutton1_lua.Active)
			{
				this.Language = EnumProgramLanguage.lua;
			}
			else if (this.radiobutton2_cpp.Active)
			{
				this.Language = EnumProgramLanguage.cpp;
			}
			else
			{
				this.Language = EnumProgramLanguage.js;
			}
			this.RefreshPubishDirWarning();
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000689C File Offset: 0x00004A9C
		protected void DialogSizeAllocatedHandler(object o, SizeAllocatedArgs args)
		{
			this.label_prompt.WidthRequest = this.alignment_top.Allocation.Width;
			this.label_publishDir.WidthRequest = this.vbox_warringText.Allocation.Width;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000068D4 File Offset: 0x00004AD4
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "Modules.Communal.CocosAdapter.Cocos2dxSupplymentDialog";
			base.Title = Catalog.GetString("项目升级");
			base.TypeHint = WindowTypeHint.Dialog;
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog_VBox";
			vbox.BorderWidth = 2U;
			this.alignment_main = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_main.Name = "alignment_main";
			this.alignment_main.TopPadding = 10U;
			this.alignment_main.BottomPadding = 10U;
			this.alignment_main.BorderWidth = 10U;
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 4;
			this.hbox_top = new HBox();
			this.hbox_top.Name = "hbox_top";
			this.hbox_top.Spacing = 6;
			this.alignment_top = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_top.Name = "alignment_top";
			this.alignment_top.BottomPadding = 8U;
			this.label_prompt = new Label();
			this.label_prompt.Name = "label_prompt";
			this.label_prompt.Xalign = 0f;
			this.label_prompt.LabelProp = Catalog.GetString("执行当前操作前需要对项目进行升级");
			this.label_prompt.Wrap = true;
			this.alignment_top.Add(this.label_prompt);
			this.hbox_top.Add(this.alignment_top);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox_top[this.alignment_top];
			boxChild.Position = 0;
			this.vbox_help = new VBox();
			this.vbox_help.Name = "vbox_help";
			this.alignment_help = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_help.Name = "alignment_help";
			this.vbox_help.Add(this.alignment_help);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_help[this.alignment_help];
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			this.hbox_top.Add(this.vbox_help);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_top[this.vbox_help];
			boxChild3.PackType = PackType.End;
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			this.vbox_main.Add(this.hbox_top);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_main[this.hbox_top];
			boxChild4.Position = 0;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.frame_option = new Frame();
			this.frame_option.Name = "frame_option";
			this.GtkAlignment_option = new Gtk.Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_option.Name = "GtkAlignment_option";
			this.GtkAlignment_option.BorderWidth = 12U;
			this.table_main = new Table(2U, 2U, false);
			this.table_main.Name = "table_main";
			this.table_main.RowSpacing = 6U;
			this.table_main.ColumnSpacing = 15U;
			this.hbox_engine = new HBox();
			this.hbox_engine.Name = "hbox_engine";
			this.hbox_engine.Spacing = 6;
			this.combobox_engine = ComboBox.NewText();
			this.combobox_engine.WidthRequest = 200;
			this.combobox_engine.Name = "combobox_engine";
			this.hbox_engine.Add(this.combobox_engine);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.hbox_engine[this.combobox_engine];
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.table_main.Add(this.hbox_engine);
			Table.TableChild tableChild = (Table.TableChild)this.table_main[this.hbox_engine];
			tableChild.LeftAttach = 1U;
			tableChild.RightAttach = 2U;
			tableChild.XOptions = AttachOptions.Fill;
			tableChild.YOptions = AttachOptions.Fill;
			this.label_framework = new Label();
			this.label_framework.Name = "label_framework";
			this.label_framework.Xalign = 1f;
			this.label_framework.LabelProp = Catalog.GetString("引擎版本");
			this.table_main.Add(this.label_framework);
			Table.TableChild tableChild2 = (Table.TableChild)this.table_main[this.label_framework];
			tableChild2.XOptions = AttachOptions.Fill;
			tableChild2.YOptions = AttachOptions.Fill;
			this.label_language = new Label();
			this.label_language.Name = "label_language";
			this.label_language.Xalign = 1f;
			this.label_language.Yalign = 0.08f;
			this.label_language.LabelProp = Catalog.GetString("项目语言");
			this.table_main.Add(this.label_language);
			Table.TableChild tableChild3 = (Table.TableChild)this.table_main[this.label_language];
			tableChild3.TopAttach = 1U;
			tableChild3.BottomAttach = 2U;
			tableChild3.XOptions = AttachOptions.Fill;
			tableChild3.YOptions = AttachOptions.Fill;
			this.table_language = new Table(3U, 2U, false);
			this.table_language.Name = "table_language";
			this.table_language.RowSpacing = 2U;
			this.table_language.ColumnSpacing = 20U;
			this.label_cppDes = new Label();
			this.label_cppDes.Sensitive = false;
			this.label_cppDes.Name = "label_cppDes";
			this.label_cppDes.Xalign = 0f;
			this.label_cppDes.LabelProp = Catalog.GetString("具有更高效的性能");
			this.table_language.Add(this.label_cppDes);
			Table.TableChild tableChild4 = (Table.TableChild)this.table_language[this.label_cppDes];
			tableChild4.TopAttach = 1U;
			tableChild4.BottomAttach = 2U;
			tableChild4.LeftAttach = 1U;
			tableChild4.RightAttach = 2U;
			tableChild4.XOptions = AttachOptions.Fill;
			tableChild4.YOptions = AttachOptions.Fill;
			this.label_jsDes = new Label();
			this.label_jsDes.Sensitive = false;
			this.label_jsDes.Name = "label_jsDes";
			this.label_jsDes.Xalign = 0f;
			this.label_jsDes.LabelProp = Catalog.GetString("高性能HTML5跨终端解决方案");
			this.table_language.Add(this.label_jsDes);
			Table.TableChild tableChild5 = (Table.TableChild)this.table_language[this.label_jsDes];
			tableChild5.TopAttach = 2U;
			tableChild5.BottomAttach = 3U;
			tableChild5.LeftAttach = 1U;
			tableChild5.RightAttach = 2U;
			tableChild5.XOptions = AttachOptions.Fill;
			tableChild5.YOptions = AttachOptions.Fill;
			this.label_luaDes = new Label();
			this.label_luaDes.Sensitive = false;
			this.label_luaDes.Name = "label_luaDes";
			this.label_luaDes.Xalign = 0f;
			this.label_luaDes.LabelProp = Catalog.GetString("支持更多的扩展控件");
			this.table_language.Add(this.label_luaDes);
			Table.TableChild tableChild6 = (Table.TableChild)this.table_language[this.label_luaDes];
			tableChild6.LeftAttach = 1U;
			tableChild6.RightAttach = 2U;
			tableChild6.XOptions = AttachOptions.Fill;
			tableChild6.YOptions = AttachOptions.Fill;
			this.radiobutton1_lua = new RadioButton(Catalog.GetString("Lua"));
			this.radiobutton1_lua.CanFocus = true;
			this.radiobutton1_lua.Name = "radiobutton1_lua";
			this.radiobutton1_lua.Active = true;
			this.radiobutton1_lua.DrawIndicator = true;
			this.radiobutton1_lua.UseUnderline = true;
			this.radiobutton1_lua.Group = new SList(IntPtr.Zero);
			this.table_language.Add(this.radiobutton1_lua);
			Table.TableChild tableChild7 = (Table.TableChild)this.table_language[this.radiobutton1_lua];
			tableChild7.XOptions = AttachOptions.Fill;
			tableChild7.YOptions = AttachOptions.Fill;
			this.radiobutton2_cpp = new RadioButton(Catalog.GetString("C++"));
			this.radiobutton2_cpp.CanFocus = true;
			this.radiobutton2_cpp.Name = "radiobutton2_cpp";
			this.radiobutton2_cpp.DrawIndicator = true;
			this.radiobutton2_cpp.UseUnderline = true;
			this.radiobutton2_cpp.Group = this.radiobutton1_lua.Group;
			this.table_language.Add(this.radiobutton2_cpp);
			Table.TableChild tableChild8 = (Table.TableChild)this.table_language[this.radiobutton2_cpp];
			tableChild8.TopAttach = 1U;
			tableChild8.BottomAttach = 2U;
			tableChild8.XOptions = AttachOptions.Fill;
			tableChild8.YOptions = AttachOptions.Fill;
			this.radiobutton3_js = new RadioButton(Catalog.GetString("JavaScript"));
			this.radiobutton3_js.CanFocus = true;
			this.radiobutton3_js.Name = "radiobutton3_js";
			this.radiobutton3_js.DrawIndicator = true;
			this.radiobutton3_js.UseUnderline = true;
			this.radiobutton3_js.Group = this.radiobutton1_lua.Group;
			this.table_language.Add(this.radiobutton3_js);
			Table.TableChild tableChild9 = (Table.TableChild)this.table_language[this.radiobutton3_js];
			tableChild9.TopAttach = 2U;
			tableChild9.BottomAttach = 3U;
			tableChild9.XOptions = AttachOptions.Fill;
			tableChild9.YOptions = AttachOptions.Fill;
			this.table_main.Add(this.table_language);
			Table.TableChild tableChild10 = (Table.TableChild)this.table_main[this.table_language];
			tableChild10.TopAttach = 1U;
			tableChild10.BottomAttach = 2U;
			tableChild10.LeftAttach = 1U;
			tableChild10.RightAttach = 2U;
			tableChild10.YOptions = AttachOptions.Fill;
			this.GtkAlignment_option.Add(this.table_main);
			this.frame_option.Add(this.GtkAlignment_option);
			this.GtkLabel_upgrade = new Label();
			this.GtkLabel_upgrade.Name = "GtkLabel_upgrade";
			this.GtkLabel_upgrade.LabelProp = Catalog.GetString(" 升级设置 ");
			this.GtkLabel_upgrade.UseMarkup = true;
			this.frame_option.LabelWidget = this.GtkLabel_upgrade;
			this.vbox_main.Add(this.frame_option);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.vbox_main[this.frame_option];
			boxChild6.Position = 1;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.alignment_publishWarning = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_publishWarning.Name = "alignment_publishWarning";
			this.alignment_publishWarning.LeftPadding = 5U;
			this.hbox_publishDirWarning = new HBox();
			this.hbox_publishDirWarning.Name = "hbox_publishDirWarning";
			this.hbox_publishDirWarning.Spacing = 6;
			this.imagebin_warring = new ImageBin();
			this.imagebin_warring.Events = EventMask.ButtonPressMask;
			this.imagebin_warring.Name = "imagebin_warring";
			this.hbox_publishDirWarning.Add(this.imagebin_warring);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.hbox_publishDirWarning[this.imagebin_warring];
			boxChild7.Position = 0;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			this.vbox_warringText = new VBox();
			this.vbox_warringText.Name = "vbox_warringText";
			this.alignment_warringTop = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_warringTop.Name = "alignment_warringTop";
			this.vbox_warringText.Add(this.alignment_warringTop);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox_warringText[this.alignment_warringTop];
			boxChild8.Position = 0;
			this.label_publishDir = new Label();
			this.label_publishDir.Name = "label_publishDir";
			this.label_publishDir.Xalign = 0f;
			this.label_publishDir.LabelProp = Catalog.GetString("升级后项目的发布路径将重定向至“res”");
			this.label_publishDir.Wrap = true;
			this.vbox_warringText.Add(this.label_publishDir);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.vbox_warringText[this.label_publishDir];
			boxChild9.Position = 1;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			this.alignment_warringBottom = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_warringBottom.Name = "alignment_warringBottom";
			this.vbox_warringText.Add(this.alignment_warringBottom);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.vbox_warringText[this.alignment_warringBottom];
			boxChild10.Position = 2;
			this.hbox_publishDirWarning.Add(this.vbox_warringText);
			Box.BoxChild boxChild11 = (Box.BoxChild)this.hbox_publishDirWarning[this.vbox_warringText];
			boxChild11.Position = 1;
			this.alignment_publishWarning.Add(this.hbox_publishDirWarning);
			this.vbox_main.Add(this.alignment_publishWarning);
			Box.BoxChild boxChild12 = (Box.BoxChild)this.vbox_main[this.alignment_publishWarning];
			boxChild12.Position = 2;
			boxChild12.Expand = false;
			boxChild12.Fill = false;
			this.alignment_main.Add(this.vbox_main);
			vbox.Add(this.alignment_main);
			Box.BoxChild boxChild13 = (Box.BoxChild)vbox[this.alignment_main];
			boxChild13.Position = 0;
			boxChild13.Expand = false;
			boxChild13.Fill = false;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog_ActionArea";
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
			base.DefaultWidth = 397;
			base.DefaultHeight = 292;
			base.Hide();
			base.SizeAllocated += this.DialogSizeAllocatedHandler;
		}

		// Token: 0x0400007A RID: 122
		private bool isSupplymentIDE;

		// Token: 0x0400007B RID: 123
		private Gtk.Alignment alignment_main;

		// Token: 0x0400007C RID: 124
		private VBox vbox_main;

		// Token: 0x0400007D RID: 125
		private HBox hbox_top;

		// Token: 0x0400007E RID: 126
		private Gtk.Alignment alignment_top;

		// Token: 0x0400007F RID: 127
		private Label label_prompt;

		// Token: 0x04000080 RID: 128
		private VBox vbox_help;

		// Token: 0x04000081 RID: 129
		private Gtk.Alignment alignment_help;

		// Token: 0x04000082 RID: 130
		private Frame frame_option;

		// Token: 0x04000083 RID: 131
		private Gtk.Alignment GtkAlignment_option;

		// Token: 0x04000084 RID: 132
		private Table table_main;

		// Token: 0x04000085 RID: 133
		private HBox hbox_engine;

		// Token: 0x04000086 RID: 134
		private ComboBox combobox_engine;

		// Token: 0x04000087 RID: 135
		private Label label_framework;

		// Token: 0x04000088 RID: 136
		private Label label_language;

		// Token: 0x04000089 RID: 137
		private Table table_language;

		// Token: 0x0400008A RID: 138
		private Label label_cppDes;

		// Token: 0x0400008B RID: 139
		private Label label_jsDes;

		// Token: 0x0400008C RID: 140
		private Label label_luaDes;

		// Token: 0x0400008D RID: 141
		private RadioButton radiobutton1_lua;

		// Token: 0x0400008E RID: 142
		private RadioButton radiobutton2_cpp;

		// Token: 0x0400008F RID: 143
		private RadioButton radiobutton3_js;

		// Token: 0x04000090 RID: 144
		private Label GtkLabel_upgrade;

		// Token: 0x04000091 RID: 145
		private Gtk.Alignment alignment_publishWarning;

		// Token: 0x04000092 RID: 146
		private HBox hbox_publishDirWarning;

		// Token: 0x04000093 RID: 147
		private ImageBin imagebin_warring;

		// Token: 0x04000094 RID: 148
		private VBox vbox_warringText;

		// Token: 0x04000095 RID: 149
		private Gtk.Alignment alignment_warringTop;

		// Token: 0x04000096 RID: 150
		private Label label_publishDir;

		// Token: 0x04000097 RID: 151
		private Gtk.Alignment alignment_warringBottom;

		// Token: 0x04000098 RID: 152
		private Button buttonCancel;

		// Token: 0x04000099 RID: 153
		private Button buttonOk;
	}
}
