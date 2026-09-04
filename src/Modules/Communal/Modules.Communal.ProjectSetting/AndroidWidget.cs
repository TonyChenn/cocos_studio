using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using GLib;
using Gtk;
using Modules.Communal.CocosAdapter;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Components;
using MonoDevelop.Ide;
using Stetic;

namespace Modules.Communal.ProjectSetting
{
	// Token: 0x0200000D RID: 13
	[ToolboxItem(true)]
	public class AndroidWidget : Bin, IProjectSettingWidget
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00004E2F File Offset: 0x0000302F
		public AndroidWidget()
		{
			this.Build();
			this.InitEvent();
			this.InitView();
			this.InitMultiLanuage();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004E5C File Offset: 0x0000305C
		private void InitView()
		{
			this.packageParams = PackageServices.Instance.PackageParams;
			this.entry_package.Text = this.packageParams.Android_PackageName;
			string androidVersion = this.packageParams.AndroidVersion;
			List<string> androidVersions = PackageServices.Instance.GetAndroidVersions();
			foreach (string text in androidVersions)
			{
				this.combobox_version.AppendText(text);
			}
			if (androidVersions.Count > 0)
			{
				if (androidVersions.Contains(androidVersion))
				{
					this.combobox_version.Active = androidVersions.IndexOf(androidVersion);
				}
				else
				{
					this.combobox_version.Active = 0;
				}
			}
			string androidkeyStore = this.packageParams.AndroidkeyStore;
			if (!string.IsNullOrWhiteSpace(androidkeyStore) && File.Exists(androidkeyStore))
			{
				this.entry_keystone.Text = androidkeyStore;
			}
			this.entry_keystorepassword.Text = this.packageParams.KeystorePassword;
			this.entry_keystorealias.Text = this.packageParams.KeystoreAliasName;
			this.entry_keystorealiaspassword.Text = this.packageParams.KeystoreAliasPassword;
			this.checkbutton_encrypt.Active = this.packageParams.LuaIsEncrypt;
			this.SetEncryptSensitive(this.checkbutton_encrypt.Active);
			this.entry_encrypyKey.Text = this.packageParams.LuaEncryptKey;
			this.entry_encrypySign.Text = this.packageParams.LuaEncryptSign;
			if (this.packageParams.IsDebugKeystore)
			{
				this.radiobutton_debugKeystore.Active = true;
				this.table_publish.Sensitive = false;
			}
			else
			{
				this.radiobutton_privateKeystore.Active = true;
			}
			this.entry_keystorealiaspassword.Visibility = false;
			this.entry_keystorepassword.Visibility = false;
			this.entry_keystorealiaspassword.InvisibleChar = '*';
			this.entry_keystorepassword.InvisibleChar = '*';
			this.entry_keystorepassword.Name = "PasswordEntry";
			this.entry_keystorealiaspassword.Name = "PasswordEntry";
			EnumSolutionCodeType solutionCodeType = Cocos2dxServices.CocosProperties.SolutionCodeType;
			EnumProgramLanguage programLanguage = Cocos2dxServices.CocosProperties.ProgramLanguage;
			if (solutionCodeType != EnumSolutionCodeType.Complete || programLanguage != EnumProgramLanguage.lua)
			{
				this.table_buildOption.Remove(this.checkbutton_encrypt);
				this.table_buildOption.Remove(this.label_encrypyKey);
				this.table_buildOption.Remove(this.entry_encrypyKey);
				this.table_buildOption.Remove(this.label_encrypySign);
				this.table_buildOption.Remove(this.entry_encrypySign);
				this.table_buildOption.Remove(this.hbox_prompt);
				this.table_buildOption.NRows = 2U;
			}
			LabelLinkButton labelLinkButton = new LabelLinkButton(LanguageInfo.ProjSetting_MoreInfo);
			labelLinkButton.URL = this.helpLinkUrl;
			this.eventbox_link1.Add(labelLinkButton);
			labelLinkButton.Show();
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00005124 File Offset: 0x00003324
		private void InitEvent()
		{
			this.button_browse.Clicked += this.button_browse_Clicked;
			this.button_new.Clicked += this.button_new_Clicked;
			this.checkbutton_encrypt.Clicked += this.checkbutton_encrypt_Clicked;
			this.radiobutton_debugKeystore.Clicked += this.radiobutton_debugKeystore_Clicked;
			this.radiobutton_privateKeystore.Clicked += this.radiobutton_privateKeystore_Clicked;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000051A4 File Offset: 0x000033A4
		private void InitMultiLanuage()
		{
			this.label_keystore.Text = LanguageInfo.Package_Keystore;
			this.label_keystorealias.Text = LanguageInfo.Package_Keystore_Alias;
			this.label_keystorealiaspassword.Text = LanguageInfo.Package_Keystore_Alias_Password;
			this.label_keystorepassword.Text = LanguageInfo.Package_Keystore_Password;
			this.label_package.Text = LanguageInfo.Package_Package_Logo;
			this.label_version.Text = LanguageInfo.Package_Android_Version;
			this.checkbutton_encrypt.Label = LanguageInfo.Package_Encrypt;
			this.label_encrypyKey.Text = LanguageInfo.Package_Encrypt_Key;
			this.label_encrypySign.Text = LanguageInfo.Package_Encrypt_Sign;
			this.label_attention1.Text = LanguageInfo.Package_Attention;
			this.GtkLabel_publish.Text = " " + LanguageInfo.Package_Keystore_Publish + " ";
			this.GtkLabel_publish1.Text = " " + LanguageInfo.Package_Build_Options + " ";
			this.radiobutton_debugKeystore.Label = LanguageInfo.Package_Debug_Keystore;
			this.radiobutton_privateKeystore.Label = LanguageInfo.Package_Private_Keystore;
			this.button_new.Label = LanguageInfo.Dialog_ButtonNew + "...";
			this.button_browse.Label = LanguageInfo.Dialog_ButtonBrowse + "...";
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000052E3 File Offset: 0x000034E3
		private void SetEncryptSensitive(bool isSensitive)
		{
			this.label_encrypyKey.Sensitive = isSensitive;
			this.label_encrypySign.Sensitive = isSensitive;
			this.entry_encrypyKey.Sensitive = isSensitive;
			this.entry_encrypySign.Sensitive = isSensitive;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00005318 File Offset: 0x00003518
		private void button_browse_Clicked(object sender, EventArgs e)
		{
			SelectFileDialog selectFileDialog = new SelectFileDialog();
			selectFileDialog.Title = LanguageInfo.Select_File;
			selectFileDialog.Action = FileChooserAction.Open;
			selectFileDialog.SelectMultiple = false;
			selectFileDialog.TransientFor = MessageService.GetDefaultModalParent();
			selectFileDialog.AddFilter("Keystore Files", new string[]
			{
				"*.keystore"
			});
			if (selectFileDialog.Run())
			{
				this.entry_keystone.Text = selectFileDialog.SelectedFile;
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000538C File Offset: 0x0000358C
		private void button_new_Clicked(object sender, EventArgs e)
		{
			NewKeystoreDialog newKeystoreDialog = new NewKeystoreDialog();
			ResponseType responseType = (ResponseType)newKeystoreDialog.Run();
			newKeystoreDialog.Destroy();
			if (responseType == ResponseType.Ok)
			{
				this.entry_keystone.Text = this.packageParams.AndroidkeyStore;
				this.entry_keystorealias.Text = this.packageParams.KeystoreAliasName;
				this.entry_keystorepassword.Text = this.packageParams.KeystorePassword;
				this.entry_keystorealiaspassword.Text = this.packageParams.KeystoreAliasPassword;
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00005409 File Offset: 0x00003609
		private void checkbutton_encrypt_Clicked(object sender, EventArgs e)
		{
			this.SetEncryptSensitive(this.checkbutton_encrypt.Active);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000541C File Offset: 0x0000361C
		private void radiobutton_debugKeystore_Clicked(object sender, EventArgs e)
		{
			this.table_publish.Sensitive = false;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000542A File Offset: 0x0000362A
		private void radiobutton_privateKeystore_Clicked(object sender, EventArgs e)
		{
			this.table_publish.Sensitive = true;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00005438 File Offset: 0x00003638
		public EnumProjectSetting SettingID
		{
			get
			{
				return EnumProjectSetting.Android;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000059 RID: 89 RVA: 0x0000543B File Offset: 0x0000363B
		public string DisplayName
		{
			get
			{
				return LanguageInfo.Package_Android_Setting;
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00005444 File Offset: 0x00003644
		public void ApplySetting()
		{
			if (this.combobox_version.ActiveText != null)
			{
				this.packageParams.AndroidVersion = this.combobox_version.ActiveText;
			}
			this.packageParams.AndroidkeyStore = this.entry_keystone.Text;
			this.packageParams.KeystorePassword = this.entry_keystorepassword.Text;
			this.packageParams.KeystoreAliasName = this.entry_keystorealias.Text;
			this.packageParams.KeystoreAliasPassword = this.entry_keystorealiaspassword.Text;
			this.packageParams.Android_PackageName = this.entry_package.Text;
			this.packageParams.LuaIsEncrypt = this.checkbutton_encrypt.Active;
			this.packageParams.LuaEncryptKey = this.entry_encrypyKey.Text;
			this.packageParams.LuaEncryptSign = this.entry_encrypySign.Text;
			this.packageParams.IsDebugKeystore = this.radiobutton_debugKeystore.Active;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000553C File Offset: 0x0000373C
		public bool CanApply(out string output)
		{
			if (this.radiobutton_privateKeystore.Active)
			{
				if (string.IsNullOrWhiteSpace(this.entry_keystone.Text))
				{
					output = LanguageInfo.MessageBox237_enterKeystore;
					this.entry_keystone.HasFocus = true;
					return false;
				}
				if (string.IsNullOrWhiteSpace(this.entry_keystorepassword.Text))
				{
					output = LanguageInfo.MessageBox238_enterKeystoreKey;
					this.entry_keystorepassword.HasFocus = true;
					return false;
				}
				if (string.IsNullOrWhiteSpace(this.entry_keystorealias.Text))
				{
					output = LanguageInfo.MessageBox239_enterAlias;
					this.entry_keystorealias.HasFocus = true;
					return false;
				}
				if (string.IsNullOrWhiteSpace(this.entry_keystorealiaspassword.Text))
				{
					output = LanguageInfo.MessageBox240_enterAliasKey;
					this.entry_keystorealiaspassword.HasFocus = true;
					return false;
				}
			}
			if (!PackageServices.Instance.CheckPackageNameValidity(this.entry_package.Text, out output))
			{
				this.entry_package.HasFocus = true;
				return false;
			}
			if (this.checkbutton_encrypt.Active)
			{
				if (string.IsNullOrWhiteSpace(this.entry_encrypyKey.Text))
				{
					output = LanguageInfo.MessageBox241_enterEncryptKey;
					this.entry_encrypyKey.HasFocus = true;
					return false;
				}
				if (string.IsNullOrWhiteSpace(this.entry_encrypySign.Text))
				{
					output = LanguageInfo.MessageBox242_enterEncryptSign;
					this.entry_encrypySign.HasFocus = true;
					return false;
				}
			}
			output = "";
			return true;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000567E File Offset: 0x0000387E
		public Widget GetWidget()
		{
			return this;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00005681 File Offset: 0x00003881
		public List<IProjectSettingWidget> SubWidgets
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00005684 File Offset: 0x00003884
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.ProjectSetting.AndroidWidget";
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 10;
			this.frame_publish = new Frame();
			this.frame_publish.Name = "frame_publish";
			this.GtkAlignment_publish = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_publish.Name = "GtkAlignment_publish";
			this.GtkAlignment_publish.LeftPadding = 15U;
			this.GtkAlignment_publish.TopPadding = 10U;
			this.GtkAlignment_publish.RightPadding = 10U;
			this.GtkAlignment_publish.BottomPadding = 10U;
			this.GtkAlignment_publish.BorderWidth = 5U;
			this.vbox5 = new VBox();
			this.vbox5.Name = "vbox5";
			this.vbox5.Spacing = 10;
			this.hbox1 = new HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			this.radiobutton_debugKeystore = new RadioButton(Catalog.GetString("radiobutton1"));
			this.radiobutton_debugKeystore.CanFocus = true;
			this.radiobutton_debugKeystore.Name = "radiobutton_debugKeystore";
			this.radiobutton_debugKeystore.DrawIndicator = true;
			this.radiobutton_debugKeystore.UseUnderline = true;
			this.radiobutton_debugKeystore.Group = new SList(IntPtr.Zero);
			this.hbox1.Add(this.radiobutton_debugKeystore);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox1[this.radiobutton_debugKeystore];
			boxChild.Position = 0;
			this.radiobutton_privateKeystore = new RadioButton(Catalog.GetString("radiobutton2"));
			this.radiobutton_privateKeystore.CanFocus = true;
			this.radiobutton_privateKeystore.Name = "radiobutton_privateKeystore";
			this.radiobutton_privateKeystore.DrawIndicator = true;
			this.radiobutton_privateKeystore.UseUnderline = true;
			this.radiobutton_privateKeystore.Group = this.radiobutton_debugKeystore.Group;
			this.hbox1.Add(this.radiobutton_privateKeystore);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox1[this.radiobutton_privateKeystore];
			boxChild2.Position = 1;
			this.vbox5.Add(this.hbox1);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox5[this.hbox1];
			boxChild3.Position = 0;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.table_publish = new Table(4U, 2U, false);
			this.table_publish.Name = "table_publish";
			this.table_publish.RowSpacing = 8U;
			this.table_publish.ColumnSpacing = 6U;
			this.entry_keystorealias = new Entry();
			this.entry_keystorealias.CanFocus = true;
			this.entry_keystorealias.Name = "entry_keystorealias";
			this.entry_keystorealias.IsEditable = true;
			this.entry_keystorealias.InvisibleChar = '●';
			this.table_publish.Add(this.entry_keystorealias);
			Table.TableChild tableChild = (Table.TableChild)this.table_publish[this.entry_keystorealias];
			tableChild.TopAttach = 2U;
			tableChild.BottomAttach = 3U;
			tableChild.LeftAttach = 1U;
			tableChild.RightAttach = 2U;
			tableChild.XOptions = AttachOptions.Fill;
			tableChild.YOptions = AttachOptions.Fill;
			this.entry_keystorealiaspassword = new PassWordEntry();
			this.entry_keystorealiaspassword.CanFocus = true;
			this.entry_keystorealiaspassword.Name = "entry_keystorealiaspassword";
			this.entry_keystorealiaspassword.IsEditable = true;
			this.entry_keystorealiaspassword.InvisibleChar = '●';
			this.table_publish.Add(this.entry_keystorealiaspassword);
			Table.TableChild tableChild2 = (Table.TableChild)this.table_publish[this.entry_keystorealiaspassword];
			tableChild2.TopAttach = 3U;
			tableChild2.BottomAttach = 4U;
			tableChild2.LeftAttach = 1U;
			tableChild2.RightAttach = 2U;
			tableChild2.XOptions = AttachOptions.Fill;
			tableChild2.YOptions = AttachOptions.Fill;
			this.entry_keystorepassword = new PassWordEntry();
			this.entry_keystorepassword.CanFocus = true;
			this.entry_keystorepassword.Name = "entry_keystorepassword";
			this.entry_keystorepassword.IsEditable = true;
			this.entry_keystorepassword.InvisibleChar = '●';
			this.table_publish.Add(this.entry_keystorepassword);
			Table.TableChild tableChild3 = (Table.TableChild)this.table_publish[this.entry_keystorepassword];
			tableChild3.TopAttach = 1U;
			tableChild3.BottomAttach = 2U;
			tableChild3.LeftAttach = 1U;
			tableChild3.RightAttach = 2U;
			tableChild3.YOptions = AttachOptions.Fill;
			this.hbox_keystone = new HBox();
			this.hbox_keystone.Name = "hbox_keystone";
			this.hbox_keystone.Spacing = 6;
			this.entry_keystone = new Entry();
			this.entry_keystone.CanFocus = true;
			this.entry_keystone.Name = "entry_keystone";
			this.entry_keystone.IsEditable = true;
			this.entry_keystone.InvisibleChar = '●';
			this.hbox_keystone.Add(this.entry_keystone);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox_keystone[this.entry_keystone];
			boxChild4.Position = 0;
			this.button_browse = new Button();
			this.button_browse.WidthRequest = 65;
			this.button_browse.CanFocus = true;
			this.button_browse.Name = "button_browse";
			this.button_browse.UseUnderline = true;
			this.button_browse.Label = Catalog.GetString("浏览");
			this.hbox_keystone.Add(this.button_browse);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.hbox_keystone[this.button_browse];
			boxChild5.Position = 1;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.button_new = new Button();
			this.button_new.WidthRequest = 65;
			this.button_new.CanFocus = true;
			this.button_new.Name = "button_new";
			this.button_new.UseUnderline = true;
			this.button_new.Label = Catalog.GetString("新建");
			this.hbox_keystone.Add(this.button_new);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox_keystone[this.button_new];
			boxChild6.PackType = PackType.End;
			boxChild6.Position = 2;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.table_publish.Add(this.hbox_keystone);
			Table.TableChild tableChild4 = (Table.TableChild)this.table_publish[this.hbox_keystone];
			tableChild4.LeftAttach = 1U;
			tableChild4.RightAttach = 2U;
			tableChild4.XOptions = AttachOptions.Fill;
			tableChild4.YOptions = AttachOptions.Fill;
			this.label_keystore = new Label();
			this.label_keystore.Name = "label_keystore";
			this.label_keystore.Xalign = 0f;
			this.label_keystore.LabelProp = Catalog.GetString("Keystore");
			this.table_publish.Add(this.label_keystore);
			Table.TableChild tableChild5 = (Table.TableChild)this.table_publish[this.label_keystore];
			tableChild5.XOptions = AttachOptions.Fill;
			tableChild5.YOptions = AttachOptions.Fill;
			this.label_keystorealias = new Label();
			this.label_keystorealias.Name = "label_keystorealias";
			this.label_keystorealias.Xalign = 0f;
			this.label_keystorealias.LabelProp = Catalog.GetString("keystore别名");
			this.table_publish.Add(this.label_keystorealias);
			Table.TableChild tableChild6 = (Table.TableChild)this.table_publish[this.label_keystorealias];
			tableChild6.TopAttach = 2U;
			tableChild6.BottomAttach = 3U;
			tableChild6.XOptions = AttachOptions.Fill;
			tableChild6.YOptions = AttachOptions.Fill;
			this.label_keystorealiaspassword = new Label();
			this.label_keystorealiaspassword.Name = "label_keystorealiaspassword";
			this.label_keystorealiaspassword.Xalign = 0f;
			this.label_keystorealiaspassword.LabelProp = Catalog.GetString("keystore别名密码");
			this.table_publish.Add(this.label_keystorealiaspassword);
			Table.TableChild tableChild7 = (Table.TableChild)this.table_publish[this.label_keystorealiaspassword];
			tableChild7.TopAttach = 3U;
			tableChild7.BottomAttach = 4U;
			tableChild7.XOptions = AttachOptions.Fill;
			tableChild7.YOptions = AttachOptions.Fill;
			this.label_keystorepassword = new Label();
			this.label_keystorepassword.Name = "label_keystorepassword";
			this.label_keystorepassword.Xalign = 0f;
			this.label_keystorepassword.LabelProp = Catalog.GetString("keystore密码");
			this.table_publish.Add(this.label_keystorepassword);
			Table.TableChild tableChild8 = (Table.TableChild)this.table_publish[this.label_keystorepassword];
			tableChild8.TopAttach = 1U;
			tableChild8.BottomAttach = 2U;
			tableChild8.XOptions = AttachOptions.Fill;
			tableChild8.YOptions = AttachOptions.Fill;
			this.vbox5.Add(this.table_publish);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.vbox5[this.table_publish];
			boxChild7.Position = 1;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			this.GtkAlignment_publish.Add(this.vbox5);
			this.frame_publish.Add(this.GtkAlignment_publish);
			this.GtkLabel_publish = new Label();
			this.GtkLabel_publish.Name = "GtkLabel_publish";
			this.GtkLabel_publish.LabelProp = Catalog.GetString(" 发布设置 ");
			this.GtkLabel_publish.UseMarkup = true;
			this.frame_publish.LabelWidget = this.GtkLabel_publish;
			this.vbox_main.Add(this.frame_publish);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox_main[this.frame_publish];
			boxChild8.Position = 0;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			this.frame_publish1 = new Frame();
			this.frame_publish1.Name = "frame_publish1";
			this.GtkAlignment_publish1 = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_publish1.Name = "GtkAlignment_publish1";
			this.GtkAlignment_publish1.LeftPadding = 15U;
			this.GtkAlignment_publish1.TopPadding = 10U;
			this.GtkAlignment_publish1.RightPadding = 10U;
			this.GtkAlignment_publish1.BottomPadding = 10U;
			this.GtkAlignment_publish1.BorderWidth = 5U;
			this.table_buildOption = new Table(6U, 2U, false);
			this.table_buildOption.Name = "table_buildOption";
			this.table_buildOption.RowSpacing = 6U;
			this.table_buildOption.ColumnSpacing = 6U;
			this.checkbutton_encrypt = new CheckButton();
			this.checkbutton_encrypt.CanFocus = true;
			this.checkbutton_encrypt.Name = "checkbutton_encrypt";
			this.checkbutton_encrypt.Label = Catalog.GetString("加密");
			this.checkbutton_encrypt.DrawIndicator = true;
			this.checkbutton_encrypt.UseUnderline = true;
			this.table_buildOption.Add(this.checkbutton_encrypt);
			Table.TableChild tableChild9 = (Table.TableChild)this.table_buildOption[this.checkbutton_encrypt];
			tableChild9.TopAttach = 2U;
			tableChild9.BottomAttach = 3U;
			tableChild9.XOptions = AttachOptions.Fill;
			tableChild9.YOptions = AttachOptions.Fill;
			this.combobox_version = ComboBox.NewText();
			this.combobox_version.Name = "combobox_version";
			this.table_buildOption.Add(this.combobox_version);
			Table.TableChild tableChild10 = (Table.TableChild)this.table_buildOption[this.combobox_version];
			tableChild10.TopAttach = 1U;
			tableChild10.BottomAttach = 2U;
			tableChild10.LeftAttach = 1U;
			tableChild10.RightAttach = 2U;
			tableChild10.YOptions = AttachOptions.Fill;
			this.entry_encrypyKey = new Entry();
			this.entry_encrypyKey.CanFocus = true;
			this.entry_encrypyKey.Name = "entry_encrypyKey";
			this.entry_encrypyKey.IsEditable = true;
			this.entry_encrypyKey.InvisibleChar = '●';
			this.table_buildOption.Add(this.entry_encrypyKey);
			Table.TableChild tableChild11 = (Table.TableChild)this.table_buildOption[this.entry_encrypyKey];
			tableChild11.TopAttach = 3U;
			tableChild11.BottomAttach = 4U;
			tableChild11.LeftAttach = 1U;
			tableChild11.RightAttach = 2U;
			tableChild11.XOptions = AttachOptions.Fill;
			tableChild11.YOptions = AttachOptions.Fill;
			this.entry_encrypySign = new Entry();
			this.entry_encrypySign.CanFocus = true;
			this.entry_encrypySign.Name = "entry_encrypySign";
			this.entry_encrypySign.IsEditable = true;
			this.entry_encrypySign.InvisibleChar = '●';
			this.table_buildOption.Add(this.entry_encrypySign);
			Table.TableChild tableChild12 = (Table.TableChild)this.table_buildOption[this.entry_encrypySign];
			tableChild12.TopAttach = 4U;
			tableChild12.BottomAttach = 5U;
			tableChild12.LeftAttach = 1U;
			tableChild12.RightAttach = 2U;
			tableChild12.XOptions = AttachOptions.Fill;
			tableChild12.YOptions = AttachOptions.Fill;
			this.entry_package = new Entry();
			this.entry_package.CanFocus = true;
			this.entry_package.Name = "entry_package";
			this.entry_package.IsEditable = true;
			this.entry_package.InvisibleChar = '●';
			this.table_buildOption.Add(this.entry_package);
			Table.TableChild tableChild13 = (Table.TableChild)this.table_buildOption[this.entry_package];
			tableChild13.LeftAttach = 1U;
			tableChild13.RightAttach = 2U;
			tableChild13.YOptions = AttachOptions.Fill;
			this.hbox_prompt = new HBox();
			this.hbox_prompt.Name = "hbox_prompt";
			this.hbox_prompt.Spacing = 6;
			this.label_attention1 = new Label();
			this.label_attention1.Name = "label_attention1";
			this.label_attention1.Xalign = 0f;
			this.label_attention1.LabelProp = Catalog.GetString("注意：");
			this.hbox_prompt.Add(this.label_attention1);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.hbox_prompt[this.label_attention1];
			boxChild9.Position = 0;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			this.eventbox_link1 = new EventBox();
			this.eventbox_link1.Name = "eventbox_link1";
			this.hbox_prompt.Add(this.eventbox_link1);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.hbox_prompt[this.eventbox_link1];
			boxChild10.Position = 1;
			boxChild10.Expand = false;
			this.table_buildOption.Add(this.hbox_prompt);
			Table.TableChild tableChild14 = (Table.TableChild)this.table_buildOption[this.hbox_prompt];
			tableChild14.TopAttach = 5U;
			tableChild14.BottomAttach = 6U;
			tableChild14.RightAttach = 2U;
			tableChild14.XOptions = AttachOptions.Fill;
			tableChild14.YOptions = AttachOptions.Fill;
			this.label_encrypyKey = new Label();
			this.label_encrypyKey.Name = "label_encrypyKey";
			this.label_encrypyKey.Xalign = 0f;
			this.label_encrypyKey.LabelProp = Catalog.GetString("加密Key");
			this.table_buildOption.Add(this.label_encrypyKey);
			Table.TableChild tableChild15 = (Table.TableChild)this.table_buildOption[this.label_encrypyKey];
			tableChild15.TopAttach = 3U;
			tableChild15.BottomAttach = 4U;
			tableChild15.XOptions = AttachOptions.Fill;
			tableChild15.YOptions = AttachOptions.Fill;
			this.label_encrypySign = new Label();
			this.label_encrypySign.Name = "label_encrypySign";
			this.label_encrypySign.Xalign = 0f;
			this.label_encrypySign.LabelProp = Catalog.GetString("加密签名");
			this.table_buildOption.Add(this.label_encrypySign);
			Table.TableChild tableChild16 = (Table.TableChild)this.table_buildOption[this.label_encrypySign];
			tableChild16.TopAttach = 4U;
			tableChild16.BottomAttach = 5U;
			tableChild16.XOptions = AttachOptions.Fill;
			tableChild16.YOptions = AttachOptions.Fill;
			this.label_package = new Label();
			this.label_package.Name = "label_package";
			this.label_package.Xalign = 0f;
			this.label_package.LabelProp = Catalog.GetString("包标识符");
			this.table_buildOption.Add(this.label_package);
			Table.TableChild tableChild17 = (Table.TableChild)this.table_buildOption[this.label_package];
			tableChild17.XOptions = AttachOptions.Fill;
			tableChild17.YOptions = AttachOptions.Fill;
			this.label_version = new Label();
			this.label_version.Name = "label_version";
			this.label_version.Xalign = 0f;
			this.label_version.LabelProp = Catalog.GetString("安卓版本");
			this.table_buildOption.Add(this.label_version);
			Table.TableChild tableChild18 = (Table.TableChild)this.table_buildOption[this.label_version];
			tableChild18.TopAttach = 1U;
			tableChild18.BottomAttach = 2U;
			tableChild18.XOptions = AttachOptions.Fill;
			tableChild18.YOptions = AttachOptions.Fill;
			this.GtkAlignment_publish1.Add(this.table_buildOption);
			this.frame_publish1.Add(this.GtkAlignment_publish1);
			this.GtkLabel_publish1 = new Label();
			this.GtkLabel_publish1.Name = "GtkLabel_publish1";
			this.GtkLabel_publish1.LabelProp = Catalog.GetString("构建选项");
			this.GtkLabel_publish1.UseMarkup = true;
			this.frame_publish1.LabelWidget = this.GtkLabel_publish1;
			this.vbox_main.Add(this.frame_publish1);
			Box.BoxChild boxChild11 = (Box.BoxChild)this.vbox_main[this.frame_publish1];
			boxChild11.Position = 1;
			boxChild11.Expand = false;
			boxChild11.Fill = false;
			base.Add(this.vbox_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		// Token: 0x04000053 RID: 83
		private string helpLinkUrl = "http://www.cocos2d-x.org/wiki/Cocos_compile#Attentions";

		// Token: 0x04000054 RID: 84
		private PackageParams packageParams;

		// Token: 0x04000055 RID: 85
		private VBox vbox_main;

		// Token: 0x04000056 RID: 86
		private Frame frame_publish;

		// Token: 0x04000057 RID: 87
		private Alignment GtkAlignment_publish;

		// Token: 0x04000058 RID: 88
		private VBox vbox5;

		// Token: 0x04000059 RID: 89
		private HBox hbox1;

		// Token: 0x0400005A RID: 90
		private RadioButton radiobutton_debugKeystore;

		// Token: 0x0400005B RID: 91
		private RadioButton radiobutton_privateKeystore;

		// Token: 0x0400005C RID: 92
		private Table table_publish;

		// Token: 0x0400005D RID: 93
		private Entry entry_keystorealias;

		// Token: 0x0400005E RID: 94
		private PassWordEntry entry_keystorealiaspassword;

		// Token: 0x0400005F RID: 95
		private PassWordEntry entry_keystorepassword;

		// Token: 0x04000060 RID: 96
		private HBox hbox_keystone;

		// Token: 0x04000061 RID: 97
		private Entry entry_keystone;

		// Token: 0x04000062 RID: 98
		private Button button_browse;

		// Token: 0x04000063 RID: 99
		private Button button_new;

		// Token: 0x04000064 RID: 100
		private Label label_keystore;

		// Token: 0x04000065 RID: 101
		private Label label_keystorealias;

		// Token: 0x04000066 RID: 102
		private Label label_keystorealiaspassword;

		// Token: 0x04000067 RID: 103
		private Label label_keystorepassword;

		// Token: 0x04000068 RID: 104
		private Label GtkLabel_publish;

		// Token: 0x04000069 RID: 105
		private Frame frame_publish1;

		// Token: 0x0400006A RID: 106
		private Alignment GtkAlignment_publish1;

		// Token: 0x0400006B RID: 107
		private Table table_buildOption;

		// Token: 0x0400006C RID: 108
		private CheckButton checkbutton_encrypt;

		// Token: 0x0400006D RID: 109
		private ComboBox combobox_version;

		// Token: 0x0400006E RID: 110
		private Entry entry_encrypyKey;

		// Token: 0x0400006F RID: 111
		private Entry entry_encrypySign;

		// Token: 0x04000070 RID: 112
		private Entry entry_package;

		// Token: 0x04000071 RID: 113
		private HBox hbox_prompt;

		// Token: 0x04000072 RID: 114
		private Label label_attention1;

		// Token: 0x04000073 RID: 115
		private EventBox eventbox_link1;

		// Token: 0x04000074 RID: 116
		private Label label_encrypyKey;

		// Token: 0x04000075 RID: 117
		private Label label_encrypySign;

		// Token: 0x04000076 RID: 118
		private Label label_package;

		// Token: 0x04000077 RID: 119
		private Label label_version;

		// Token: 0x04000078 RID: 120
		private Label GtkLabel_publish1;
	}
}
