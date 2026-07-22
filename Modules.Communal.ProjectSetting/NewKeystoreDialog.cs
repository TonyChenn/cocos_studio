using System;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using Gdk;
using Gtk;
using Modules.Communal.CocosAdapter;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.Ide;
using Stetic;
using Xwt.Drawing;

namespace Modules.Communal.ProjectSetting
{
	// Token: 0x0200000E RID: 14
	public class NewKeystoreDialog : Dialog
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600005F RID: 95 RVA: 0x000067D5 File Offset: 0x000049D5
		// (set) Token: 0x06000060 RID: 96 RVA: 0x000067E2 File Offset: 0x000049E2
		public bool ButtonOKSensitive
		{
			get
			{
				return this.buttonOk.Sensitive;
			}
			set
			{
				this.buttonOk.Sensitive = value;
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000067F0 File Offset: 0x000049F0
		public NewKeystoreDialog()
		{
			this.Build();
			this.parentWnd = MessageService.GetDefaultModalParent();
			if (this.parentWnd == null)
			{
				this.parentWnd = ApplicationCurrent.MainWindow;
			}
			this.SetToDialogStyle(this.parentWnd, true, true, true);
			this.parentModal = this.parentWnd.Modal;
			this.parentWnd.Modal = false;
			this.InitView();
			this.InitEvent();
			this.InitMultiLanuage();
			base.ShowAll();
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00006878 File Offset: 0x00004A78
		private void InitView()
		{
			this.buttonOk.Name = "MainButton";
			if (Platform.IsWindows)
			{
				((ButtonBox.ButtonBoxChild)base.ActionArea[this.buttonCancel]).Position = 1;
				((ButtonBox.ButtonBoxChild)base.ActionArea[this.buttonOk]).Position = 0;
			}
			this.ButtonOKSensitive = false;
			Xwt.Drawing.Image icon = ImageIcon.GetIcon("Modules.Communal.ProjectSetting.Image.StudioMessageWarring.png");
			this.imagebin_warring.SetImageView(icon);
			this.entry_keystorelocation.Text = System.IO.Path.Combine(Services.ProjectsService.CurrentSolution.BaseDirectory, "publish", Services.ProjectsService.CurrentSolution.Name + ".keystore");
			this.entry_keystorePassword.Visibility = false;
			this.entry_affirmKeystorePassword.Visibility = false;
			this.entry_aliasPassword.Visibility = false;
			this.entry_affirmAliasPassword.Visibility = false;
			this.entry_keystorePassword.InvisibleChar = '*';
			this.entry_affirmKeystorePassword.InvisibleChar = '*';
			this.entry_aliasPassword.InvisibleChar = '*';
			this.entry_affirmAliasPassword.InvisibleChar = '*';
			this.entry_keystorePassword.Name = "PasswordEntry";
			this.entry_affirmKeystorePassword.Name = "PasswordEntry";
			this.entry_aliasPassword.Name = "PasswordEntry";
			this.entry_affirmAliasPassword.Name = "PasswordEntry";
			this.ShowErrorInfo();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000069E0 File Offset: 0x00004BE0
		private void InitEvent()
		{
			this.buttonOk.Clicked += this.OnButtonOKClicked;
			this.buttonCancel.Clicked += this.OnButtonCancelClicked;
			this.entry_keystorePassword.Changed += this.entry_Changed;
			this.entry_affirmKeystorePassword.Changed += this.entry_Changed;
			this.entry_aliasName.Changed += this.entry_Changed;
			this.entry_aliasPassword.Changed += this.entry_Changed;
			this.entry_affirmAliasPassword.Changed += this.entry_Changed;
			this.entry_ValidDate.Changed += this.entry_ValidDate_Changed;
			this.entry_firstword.Changed += this.entry_Changed;
			this.entry_unit.Changed += this.entry_Changed;
			this.entry_organization.Changed += this.entry_Changed;
			this.entry_city.Changed += this.entry_Changed;
			this.entry_Country.Changed += this.entry_Changed;
			this.entry_countyCode.Changed += this.entry_Changed;
			base.Destroyed += this.HandleDestroyed;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00006B44 File Offset: 0x00004D44
		private void entry_ValidDate_Changed(object sender, EventArgs e)
		{
			string text = this.entry_ValidDate.Text;
			if (!RegexModel.IsNumber(text) || text.Length > 4)
			{
				this.entry_ValidDate.Text = this.oldKeyValidDate;
				return;
			}
			this.oldKeyValidDate = this.entry_ValidDate.Text;
			this.ShowErrorInfo();
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00006B97 File Offset: 0x00004D97
		private void entry_Changed(object sender, EventArgs e)
		{
			this.ShowErrorInfo();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00006BA0 File Offset: 0x00004DA0
		private void InitMultiLanuage()
		{
			base.Title = LanguageInfo.ProjSetting_NewKeystore;
			this.label_keyCreat.Text = LanguageInfo.Package_Keystore_Creat;
			this.label_KeystoreLocation.Text = LanguageInfo.Package_Keystore_Location;
			this.label_keystorePassword.Text = LanguageInfo.Package_Keystore_Password;
			this.label_affirmKeystorePassword.Text = LanguageInfo.Package_Affirm_Password;
			this.label_aliasName.Text = LanguageInfo.Package_AliasName;
			this.label_aliasPassword.Text = LanguageInfo.Package_AliasPassword;
			this.label_affirmAliasPassword.Text = LanguageInfo.Package_Affirm_Password;
			this.label_ValidDate.Text = LanguageInfo.Package_ValidDate;
			this.label_firstword.Text = LanguageInfo.Package_FirstAndLastWord;
			this.label_unit.Text = LanguageInfo.Package_Unit;
			this.label_organization.Text = LanguageInfo.Package_Organization;
			this.label_city.Text = LanguageInfo.Package_City;
			this.label_Country.Text = LanguageInfo.Package_Country;
			this.label_countyCode.Text = LanguageInfo.Package_CountyCode;
			this.buttonOk.Label = LanguageInfo.Dialog_ButtonOK;
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonCancel;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00006CB8 File Offset: 0x00004EB8
		private void ShowErrorInfo()
		{
			string text = string.Empty;
			try
			{
				if (File.Exists(this.entry_keystorelocation.Text))
				{
					text = LanguageInfo.Package_Keystore_Error12;
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Keystore storage judgement failure: ", exception);
			}
			if (string.IsNullOrEmpty(text))
			{
				if (string.IsNullOrWhiteSpace(this.entry_keystorePassword.Text))
				{
					text = LanguageInfo.Package_Keystore_Error0;
				}
				else if (this.entry_keystorePassword.Text.Length < 6)
				{
					text = LanguageInfo.Package_Keystore_Error1;
				}
				else if (!string.Equals(this.entry_keystorePassword.Text, this.entry_affirmKeystorePassword.Text))
				{
					text = LanguageInfo.Package_Keystore_Error2;
				}
				else if (string.IsNullOrWhiteSpace(this.entry_aliasName.Text))
				{
					text = LanguageInfo.Package_Keystore_Error3;
				}
				else if (string.IsNullOrWhiteSpace(this.entry_aliasPassword.Text))
				{
					text = LanguageInfo.Package_Keystore_Error4;
				}
				else if (this.entry_aliasPassword.Text.Length < 6)
				{
					text = LanguageInfo.Package_Keystore_Error5;
				}
				else if (!string.Equals(this.entry_aliasPassword.Text, this.entry_affirmAliasPassword.Text))
				{
					text = LanguageInfo.Package_Keystore_Error6;
				}
				else if (string.IsNullOrWhiteSpace(this.entry_ValidDate.Text))
				{
					text = LanguageInfo.Package_Keystore_Error7;
				}
				else if (int.Parse(this.entry_ValidDate.Text) > 1000 || int.Parse(this.entry_ValidDate.Text) <= 0)
				{
					text = LanguageInfo.Package_Keystore_Error8;
				}
				else if (string.IsNullOrWhiteSpace(this.entry_firstword.Text) && string.IsNullOrWhiteSpace(this.entry_unit.Text) && string.IsNullOrWhiteSpace(this.entry_organization.Text) && string.IsNullOrWhiteSpace(this.entry_city.Text) && string.IsNullOrWhiteSpace(this.entry_Country.Text) && string.IsNullOrWhiteSpace(this.entry_countyCode.Text))
				{
					text = LanguageInfo.Package_Keystore_Error9;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				this.hbox2.Hide();
				this.ButtonOKSensitive = true;
				return;
			}
			this.label_error.Text = text;
			this.hbox2.Show();
			this.ButtonOKSensitive = false;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00006EF0 File Offset: 0x000050F0
		private void OnButtonOKClicked(object sender, EventArgs e)
		{
			FilePath filePath = new FilePath(this.entry_keystorelocation.Text);
			FilePath parentDirectory = filePath.ParentDirectory;
			try
			{
				if (!Directory.Exists(parentDirectory))
				{
					Directory.CreateDirectory(parentDirectory);
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Keystore storage judgement failure: ", exception);
			}
			string text = string.Format("{0}/keytool", Option.UserConfig.JDKPath);
			string text2 = string.Format("-genkey -storepass \"{0}\" -alias \"{1}\" -keypass \"{2}\" -validity \"{3}\" -dname CN=\"{4}\",OU=\"{5}\",O=\"{6}\",L=\"{7}\",ST=\"{8}\",C=\"{9}\" -keyalg RSA -keystore \"{10}\"", new object[]
			{
				this.entry_keystorePassword.Text,
				this.entry_aliasName.Text,
				this.entry_aliasPassword.Text,
				int.Parse(this.entry_ValidDate.Text) * 365,
				this.entry_firstword.Text,
				this.entry_unit.Text,
				this.entry_organization.Text,
				this.entry_city.Text,
				this.entry_Country.Text,
				this.entry_countyCode.Text,
				this.entry_keystorelocation.Text
			});
			try
			{
				ProcessWrapper processWrapper = Runtime.ProcessService.StartProcess(text, text2, parentDirectory, null);
				processWrapper.WaitForOutput();
				if (processWrapper.ExitCode == 0)
				{
					LogConfig.Logger.Info(GettextCatalog.GetString("Process '{0}' has completed succesfully", text), true);
					PackageServices.Instance.PackageParams.AndroidkeyStore = this.entry_keystorelocation.Text;
					PackageServices.Instance.PackageParams.KeystorePassword = this.entry_keystorePassword.Text;
					PackageServices.Instance.PackageParams.KeystoreAliasName = this.entry_aliasName.Text;
					PackageServices.Instance.PackageParams.KeystoreAliasPassword = this.entry_aliasPassword.Text;
					base.Respond(ResponseType.Ok);
				}
				else
				{
					LogConfig.Logger.Error(GettextCatalog.GetString("Process '{0}' has exited with error code {1}", text, processWrapper.ExitCode));
					MessageBox.Show(LanguageInfo.Package_Keystore_Failure, MessageBoxImage.Error, this, null);
				}
			}
			catch (Exception exception2)
			{
				LogConfig.Logger.Error(GettextCatalog.GetString("External program execution failed.\nError while starting:\n '{0} {1}'", text, text2), exception2);
				MessageBox.Show(LanguageInfo.Package_Keystore_Failure, MessageBoxImage.Error, this, null);
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00007150 File Offset: 0x00005350
		private void CheckLegitimacy(out string info)
		{
			info = string.Empty;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00007159 File Offset: 0x00005359
		private void OnButtonCancelClicked(object sender, EventArgs e)
		{
			base.Respond(ResponseType.Cancel);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00007163 File Offset: 0x00005363
		private void HandleDestroyed(object sender, EventArgs e)
		{
			this.parentWnd.Modal = this.parentModal;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00007178 File Offset: 0x00005378
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.WidthRequest = 520;
			base.Name = "Modules.Communal.ProjectSetting.NewKeystoreDialog";
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog1_VBox";
			vbox.Spacing = 6;
			this.vbox4 = new VBox();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			this.vbox4.BorderWidth = 10U;
			this.vbox2 = new VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			this.eventbox1 = new EventBox();
			this.eventbox1.Name = "eventbox1";
			this.hbox1 = new HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			this.vbox_top = new VBox();
			this.vbox_top.Name = "vbox_top";
			this.vbox_top.Spacing = 6;
			this.label_keyCreat = new Label();
			this.label_keyCreat.Name = "label_keyCreat";
			this.label_keyCreat.Xalign = 0f;
			this.label_keyCreat.LabelProp = Catalog.GetString("Keystore创建");
			this.vbox_top.Add(this.label_keyCreat);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_top[this.label_keyCreat];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.alignment_error = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_error.HeightRequest = 30;
			this.alignment_error.Name = "alignment_error";
			this.hbox2 = new HBox();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			this.vbox_imageWarring = new VBox();
			this.vbox_imageWarring.Name = "vbox_imageWarring";
			this.vbox_imageWarring.Spacing = 6;
			this.alignment_imageWarringTop = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_imageWarringTop.Name = "alignment_imageWarringTop";
			this.vbox_imageWarring.Add(this.alignment_imageWarringTop);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_imageWarring[this.alignment_imageWarringTop];
			boxChild2.Position = 0;
			this.imagebin_warring = new ImageBin();
			this.imagebin_warring.Events = EventMask.ButtonPressMask;
			this.imagebin_warring.Name = "imagebin_warring";
			this.vbox_imageWarring.Add(this.imagebin_warring);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox_imageWarring[this.imagebin_warring];
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.alignment_imageWarringBottom = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_imageWarringBottom.Name = "alignment_imageWarringBottom";
			this.vbox_imageWarring.Add(this.alignment_imageWarringBottom);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_imageWarring[this.alignment_imageWarringBottom];
			boxChild4.Position = 2;
			this.hbox2.Add(this.vbox_imageWarring);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.hbox2[this.vbox_imageWarring];
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.label_error = new Label();
			this.label_error.Name = "label_error";
			this.label_error.Xalign = 0f;
			this.label_error.LabelProp = Catalog.GetString("label2");
			this.hbox2.Add(this.label_error);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox2[this.label_error];
			boxChild6.Position = 1;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.alignment_error.Add(this.hbox2);
			this.vbox_top.Add(this.alignment_error);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.vbox_top[this.alignment_error];
			boxChild7.Position = 1;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			this.hbox1.Add(this.vbox_top);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.hbox1[this.vbox_top];
			boxChild8.Position = 0;
			this.eventbox1.Add(this.hbox1);
			this.vbox2.Add(this.eventbox1);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.vbox2[this.eventbox1];
			boxChild9.Position = 0;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			this.hseparator1 = new HSeparator();
			this.hseparator1.Name = "hseparator1";
			this.vbox2.Add(this.hseparator1);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.vbox2[this.hseparator1];
			boxChild10.Position = 1;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			this.table4 = new Table(16U, 2U, false);
			this.table4.Name = "table4";
			this.table4.RowSpacing = 6U;
			this.table4.ColumnSpacing = 6U;
			this.entry_affirmAliasPassword = new PassWordEntry();
			this.entry_affirmAliasPassword.CanFocus = true;
			this.entry_affirmAliasPassword.Name = "entry_affirmAliasPassword";
			this.entry_affirmAliasPassword.IsEditable = true;
			this.entry_affirmAliasPassword.InvisibleChar = '●';
			this.table4.Add(this.entry_affirmAliasPassword);
			Table.TableChild tableChild = (Table.TableChild)this.table4[this.entry_affirmAliasPassword];
			tableChild.TopAttach = 7U;
			tableChild.BottomAttach = 8U;
			tableChild.LeftAttach = 1U;
			tableChild.RightAttach = 2U;
			tableChild.XOptions = AttachOptions.Fill;
			tableChild.YOptions = AttachOptions.Fill;
			this.entry_affirmKeystorePassword = new PassWordEntry();
			this.entry_affirmKeystorePassword.CanFocus = true;
			this.entry_affirmKeystorePassword.Name = "entry_affirmKeystorePassword";
			this.entry_affirmKeystorePassword.IsEditable = true;
			this.entry_affirmKeystorePassword.InvisibleChar = '●';
			this.table4.Add(this.entry_affirmKeystorePassword);
			Table.TableChild tableChild2 = (Table.TableChild)this.table4[this.entry_affirmKeystorePassword];
			tableChild2.TopAttach = 3U;
			tableChild2.BottomAttach = 4U;
			tableChild2.LeftAttach = 1U;
			tableChild2.RightAttach = 2U;
			tableChild2.XOptions = AttachOptions.Fill;
			tableChild2.YOptions = AttachOptions.Fill;
			this.entry_aliasName = new Entry();
			this.entry_aliasName.CanFocus = true;
			this.entry_aliasName.Name = "entry_aliasName";
			this.entry_aliasName.IsEditable = true;
			this.entry_aliasName.InvisibleChar = '●';
			this.table4.Add(this.entry_aliasName);
			Table.TableChild tableChild3 = (Table.TableChild)this.table4[this.entry_aliasName];
			tableChild3.TopAttach = 5U;
			tableChild3.BottomAttach = 6U;
			tableChild3.LeftAttach = 1U;
			tableChild3.RightAttach = 2U;
			tableChild3.XOptions = AttachOptions.Fill;
			tableChild3.YOptions = AttachOptions.Fill;
			this.entry_aliasPassword = new PassWordEntry();
			this.entry_aliasPassword.CanFocus = true;
			this.entry_aliasPassword.Name = "entry_aliasPassword";
			this.entry_aliasPassword.IsEditable = true;
			this.entry_aliasPassword.InvisibleChar = '●';
			this.table4.Add(this.entry_aliasPassword);
			Table.TableChild tableChild4 = (Table.TableChild)this.table4[this.entry_aliasPassword];
			tableChild4.TopAttach = 6U;
			tableChild4.BottomAttach = 7U;
			tableChild4.LeftAttach = 1U;
			tableChild4.RightAttach = 2U;
			tableChild4.XOptions = AttachOptions.Fill;
			tableChild4.YOptions = AttachOptions.Fill;
			this.entry_city = new Entry();
			this.entry_city.CanFocus = true;
			this.entry_city.Name = "entry_city";
			this.entry_city.IsEditable = true;
			this.entry_city.InvisibleChar = '●';
			this.table4.Add(this.entry_city);
			Table.TableChild tableChild5 = (Table.TableChild)this.table4[this.entry_city];
			tableChild5.TopAttach = 13U;
			tableChild5.BottomAttach = 14U;
			tableChild5.LeftAttach = 1U;
			tableChild5.RightAttach = 2U;
			tableChild5.XOptions = AttachOptions.Fill;
			tableChild5.YOptions = AttachOptions.Fill;
			this.entry_Country = new Entry();
			this.entry_Country.CanFocus = true;
			this.entry_Country.Name = "entry_Country";
			this.entry_Country.IsEditable = true;
			this.entry_Country.InvisibleChar = '●';
			this.table4.Add(this.entry_Country);
			Table.TableChild tableChild6 = (Table.TableChild)this.table4[this.entry_Country];
			tableChild6.TopAttach = 14U;
			tableChild6.BottomAttach = 15U;
			tableChild6.LeftAttach = 1U;
			tableChild6.RightAttach = 2U;
			tableChild6.XOptions = AttachOptions.Fill;
			tableChild6.YOptions = AttachOptions.Fill;
			this.entry_countyCode = new Entry();
			this.entry_countyCode.CanFocus = true;
			this.entry_countyCode.Name = "entry_countyCode";
			this.entry_countyCode.IsEditable = true;
			this.entry_countyCode.InvisibleChar = '●';
			this.table4.Add(this.entry_countyCode);
			Table.TableChild tableChild7 = (Table.TableChild)this.table4[this.entry_countyCode];
			tableChild7.TopAttach = 15U;
			tableChild7.BottomAttach = 16U;
			tableChild7.LeftAttach = 1U;
			tableChild7.RightAttach = 2U;
			tableChild7.XOptions = AttachOptions.Fill;
			tableChild7.YOptions = AttachOptions.Fill;
			this.entry_firstword = new Entry();
			this.entry_firstword.CanFocus = true;
			this.entry_firstword.Name = "entry_firstword";
			this.entry_firstword.IsEditable = true;
			this.entry_firstword.InvisibleChar = '●';
			this.table4.Add(this.entry_firstword);
			Table.TableChild tableChild8 = (Table.TableChild)this.table4[this.entry_firstword];
			tableChild8.TopAttach = 10U;
			tableChild8.BottomAttach = 11U;
			tableChild8.LeftAttach = 1U;
			tableChild8.RightAttach = 2U;
			tableChild8.XOptions = AttachOptions.Fill;
			tableChild8.YOptions = AttachOptions.Fill;
			this.entry_keystorelocation = new Entry();
			this.entry_keystorelocation.CanFocus = true;
			this.entry_keystorelocation.Name = "entry_keystorelocation";
			this.entry_keystorelocation.IsEditable = true;
			this.entry_keystorelocation.InvisibleChar = '●';
			this.table4.Add(this.entry_keystorelocation);
			Table.TableChild tableChild9 = (Table.TableChild)this.table4[this.entry_keystorelocation];
			tableChild9.LeftAttach = 1U;
			tableChild9.RightAttach = 2U;
			tableChild9.YOptions = AttachOptions.Fill;
			this.entry_keystorePassword = new PassWordEntry();
			this.entry_keystorePassword.CanFocus = true;
			this.entry_keystorePassword.Name = "entry_keystorePassword";
			this.entry_keystorePassword.IsEditable = true;
			this.entry_keystorePassword.InvisibleChar = '●';
			this.table4.Add(this.entry_keystorePassword);
			Table.TableChild tableChild10 = (Table.TableChild)this.table4[this.entry_keystorePassword];
			tableChild10.TopAttach = 2U;
			tableChild10.BottomAttach = 3U;
			tableChild10.LeftAttach = 1U;
			tableChild10.RightAttach = 2U;
			tableChild10.XOptions = AttachOptions.Fill;
			tableChild10.YOptions = AttachOptions.Fill;
			this.entry_organization = new Entry();
			this.entry_organization.CanFocus = true;
			this.entry_organization.Name = "entry_organization";
			this.entry_organization.IsEditable = true;
			this.entry_organization.InvisibleChar = '●';
			this.table4.Add(this.entry_organization);
			Table.TableChild tableChild11 = (Table.TableChild)this.table4[this.entry_organization];
			tableChild11.TopAttach = 12U;
			tableChild11.BottomAttach = 13U;
			tableChild11.LeftAttach = 1U;
			tableChild11.RightAttach = 2U;
			tableChild11.XOptions = AttachOptions.Fill;
			tableChild11.YOptions = AttachOptions.Fill;
			this.entry_unit = new Entry();
			this.entry_unit.CanFocus = true;
			this.entry_unit.Name = "entry_unit";
			this.entry_unit.IsEditable = true;
			this.entry_unit.InvisibleChar = '●';
			this.table4.Add(this.entry_unit);
			Table.TableChild tableChild12 = (Table.TableChild)this.table4[this.entry_unit];
			tableChild12.TopAttach = 11U;
			tableChild12.BottomAttach = 12U;
			tableChild12.LeftAttach = 1U;
			tableChild12.RightAttach = 2U;
			tableChild12.XOptions = AttachOptions.Fill;
			tableChild12.YOptions = AttachOptions.Fill;
			this.entry_ValidDate = new Entry();
			this.entry_ValidDate.CanFocus = true;
			this.entry_ValidDate.Name = "entry_ValidDate";
			this.entry_ValidDate.IsEditable = true;
			this.entry_ValidDate.InvisibleChar = '●';
			this.table4.Add(this.entry_ValidDate);
			Table.TableChild tableChild13 = (Table.TableChild)this.table4[this.entry_ValidDate];
			tableChild13.TopAttach = 8U;
			tableChild13.BottomAttach = 9U;
			tableChild13.LeftAttach = 1U;
			tableChild13.RightAttach = 2U;
			tableChild13.XOptions = AttachOptions.Fill;
			tableChild13.YOptions = AttachOptions.Fill;
			this.hseparator2 = new HSeparator();
			this.hseparator2.Name = "hseparator2";
			this.table4.Add(this.hseparator2);
			Table.TableChild tableChild14 = (Table.TableChild)this.table4[this.hseparator2];
			tableChild14.TopAttach = 1U;
			tableChild14.BottomAttach = 2U;
			tableChild14.RightAttach = 2U;
			tableChild14.XOptions = AttachOptions.Fill;
			tableChild14.YOptions = AttachOptions.Fill;
			this.hseparator3 = new HSeparator();
			this.hseparator3.Name = "hseparator3";
			this.table4.Add(this.hseparator3);
			Table.TableChild tableChild15 = (Table.TableChild)this.table4[this.hseparator3];
			tableChild15.TopAttach = 4U;
			tableChild15.BottomAttach = 5U;
			tableChild15.RightAttach = 2U;
			tableChild15.XOptions = AttachOptions.Fill;
			tableChild15.YOptions = AttachOptions.Fill;
			this.hseparator9 = new HSeparator();
			this.hseparator9.Name = "hseparator9";
			this.table4.Add(this.hseparator9);
			Table.TableChild tableChild16 = (Table.TableChild)this.table4[this.hseparator9];
			tableChild16.TopAttach = 9U;
			tableChild16.BottomAttach = 10U;
			tableChild16.RightAttach = 2U;
			tableChild16.XOptions = AttachOptions.Fill;
			tableChild16.YOptions = AttachOptions.Fill;
			this.label_affirmAliasPassword = new Label();
			this.label_affirmAliasPassword.Name = "label_affirmAliasPassword";
			this.label_affirmAliasPassword.Xalign = 0f;
			this.label_affirmAliasPassword.LabelProp = Catalog.GetString("确认密码");
			this.table4.Add(this.label_affirmAliasPassword);
			Table.TableChild tableChild17 = (Table.TableChild)this.table4[this.label_affirmAliasPassword];
			tableChild17.TopAttach = 7U;
			tableChild17.BottomAttach = 8U;
			tableChild17.XOptions = AttachOptions.Fill;
			tableChild17.YOptions = AttachOptions.Fill;
			this.label_affirmKeystorePassword = new Label();
			this.label_affirmKeystorePassword.Name = "label_affirmKeystorePassword";
			this.label_affirmKeystorePassword.Xalign = 0f;
			this.label_affirmKeystorePassword.LabelProp = Catalog.GetString("确认密码");
			this.table4.Add(this.label_affirmKeystorePassword);
			Table.TableChild tableChild18 = (Table.TableChild)this.table4[this.label_affirmKeystorePassword];
			tableChild18.TopAttach = 3U;
			tableChild18.BottomAttach = 4U;
			tableChild18.XOptions = AttachOptions.Fill;
			tableChild18.YOptions = AttachOptions.Fill;
			this.label_aliasName = new Label();
			this.label_aliasName.Name = "label_aliasName";
			this.label_aliasName.Xalign = 0f;
			this.label_aliasName.LabelProp = Catalog.GetString("别名");
			this.table4.Add(this.label_aliasName);
			Table.TableChild tableChild19 = (Table.TableChild)this.table4[this.label_aliasName];
			tableChild19.TopAttach = 5U;
			tableChild19.BottomAttach = 6U;
			tableChild19.XOptions = AttachOptions.Fill;
			tableChild19.YOptions = AttachOptions.Fill;
			this.label_aliasPassword = new Label();
			this.label_aliasPassword.Name = "label_aliasPassword";
			this.label_aliasPassword.Xalign = 0f;
			this.label_aliasPassword.LabelProp = Catalog.GetString("别名密码");
			this.table4.Add(this.label_aliasPassword);
			Table.TableChild tableChild20 = (Table.TableChild)this.table4[this.label_aliasPassword];
			tableChild20.TopAttach = 6U;
			tableChild20.BottomAttach = 7U;
			tableChild20.XOptions = AttachOptions.Fill;
			tableChild20.YOptions = AttachOptions.Fill;
			this.label_city = new Label();
			this.label_city.Name = "label_city";
			this.label_city.Xalign = 0f;
			this.label_city.LabelProp = Catalog.GetString("城市区域");
			this.table4.Add(this.label_city);
			Table.TableChild tableChild21 = (Table.TableChild)this.table4[this.label_city];
			tableChild21.TopAttach = 13U;
			tableChild21.BottomAttach = 14U;
			tableChild21.XOptions = AttachOptions.Fill;
			tableChild21.YOptions = AttachOptions.Fill;
			this.label_Country = new Label();
			this.label_Country.Name = "label_Country";
			this.label_Country.Xalign = 0f;
			this.label_Country.LabelProp = Catalog.GetString("国家或省");
			this.table4.Add(this.label_Country);
			Table.TableChild tableChild22 = (Table.TableChild)this.table4[this.label_Country];
			tableChild22.TopAttach = 14U;
			tableChild22.BottomAttach = 15U;
			tableChild22.XOptions = AttachOptions.Fill;
			tableChild22.YOptions = AttachOptions.Fill;
			this.label_countyCode = new Label();
			this.label_countyCode.Name = "label_countyCode";
			this.label_countyCode.Xalign = 0f;
			this.label_countyCode.LabelProp = Catalog.GetString("国家/地区代码（XX）：");
			this.table4.Add(this.label_countyCode);
			Table.TableChild tableChild23 = (Table.TableChild)this.table4[this.label_countyCode];
			tableChild23.TopAttach = 15U;
			tableChild23.BottomAttach = 16U;
			tableChild23.XOptions = AttachOptions.Fill;
			tableChild23.YOptions = AttachOptions.Fill;
			this.label_firstword = new Label();
			this.label_firstword.Name = "label_firstword";
			this.label_firstword.Xalign = 0f;
			this.label_firstword.LabelProp = Catalog.GetString("第一个和最后一个名词");
			this.table4.Add(this.label_firstword);
			Table.TableChild tableChild24 = (Table.TableChild)this.table4[this.label_firstword];
			tableChild24.TopAttach = 10U;
			tableChild24.BottomAttach = 11U;
			tableChild24.XOptions = AttachOptions.Fill;
			tableChild24.YOptions = AttachOptions.Fill;
			this.label_KeystoreLocation = new Label();
			this.label_KeystoreLocation.Name = "label_KeystoreLocation";
			this.label_KeystoreLocation.Xalign = 0f;
			this.label_KeystoreLocation.LabelProp = Catalog.GetString("Keystore位置");
			this.table4.Add(this.label_KeystoreLocation);
			Table.TableChild tableChild25 = (Table.TableChild)this.table4[this.label_KeystoreLocation];
			tableChild25.XOptions = AttachOptions.Fill;
			tableChild25.YOptions = AttachOptions.Fill;
			this.label_keystorePassword = new Label();
			this.label_keystorePassword.Name = "label_keystorePassword";
			this.label_keystorePassword.Xalign = 0f;
			this.label_keystorePassword.LabelProp = Catalog.GetString("keystore密码");
			this.table4.Add(this.label_keystorePassword);
			Table.TableChild tableChild26 = (Table.TableChild)this.table4[this.label_keystorePassword];
			tableChild26.TopAttach = 2U;
			tableChild26.BottomAttach = 3U;
			tableChild26.XOptions = AttachOptions.Fill;
			tableChild26.YOptions = AttachOptions.Fill;
			this.label_organization = new Label();
			this.label_organization.Name = "label_organization";
			this.label_organization.Xalign = 0f;
			this.label_organization.LabelProp = Catalog.GetString("组织");
			this.table4.Add(this.label_organization);
			Table.TableChild tableChild27 = (Table.TableChild)this.table4[this.label_organization];
			tableChild27.TopAttach = 12U;
			tableChild27.BottomAttach = 13U;
			tableChild27.XOptions = AttachOptions.Fill;
			tableChild27.YOptions = AttachOptions.Fill;
			this.label_unit = new Label();
			this.label_unit.Name = "label_unit";
			this.label_unit.Xalign = 0f;
			this.label_unit.LabelProp = Catalog.GetString("单位");
			this.table4.Add(this.label_unit);
			Table.TableChild tableChild28 = (Table.TableChild)this.table4[this.label_unit];
			tableChild28.TopAttach = 11U;
			tableChild28.BottomAttach = 12U;
			tableChild28.XOptions = AttachOptions.Fill;
			tableChild28.YOptions = AttachOptions.Fill;
			this.label_ValidDate = new Label();
			this.label_ValidDate.Name = "label_ValidDate";
			this.label_ValidDate.Xalign = 0f;
			this.label_ValidDate.LabelProp = Catalog.GetString("有效期（年）");
			this.table4.Add(this.label_ValidDate);
			Table.TableChild tableChild29 = (Table.TableChild)this.table4[this.label_ValidDate];
			tableChild29.TopAttach = 8U;
			tableChild29.BottomAttach = 9U;
			tableChild29.XOptions = AttachOptions.Fill;
			tableChild29.YOptions = AttachOptions.Fill;
			this.vbox2.Add(this.table4);
			Box.BoxChild boxChild11 = (Box.BoxChild)this.vbox2[this.table4];
			boxChild11.PackType = PackType.End;
			boxChild11.Position = 2;
			boxChild11.Expand = false;
			boxChild11.Fill = false;
			this.vbox4.Add(this.vbox2);
			Box.BoxChild boxChild12 = (Box.BoxChild)this.vbox4[this.vbox2];
			boxChild12.Position = 0;
			boxChild12.Expand = false;
			boxChild12.Fill = false;
			this.hseparator8 = new HSeparator();
			this.hseparator8.Name = "hseparator8";
			this.vbox4.Add(this.hseparator8);
			Box.BoxChild boxChild13 = (Box.BoxChild)this.vbox4[this.hseparator8];
			boxChild13.PackType = PackType.End;
			boxChild13.Position = 1;
			boxChild13.Expand = false;
			boxChild13.Fill = false;
			vbox.Add(this.vbox4);
			Box.BoxChild boxChild14 = (Box.BoxChild)vbox[this.vbox4];
			boxChild14.Position = 0;
			boxChild14.Expand = false;
			boxChild14.Fill = false;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog1_ActionArea";
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
			base.DefaultWidth = 520;
			base.DefaultHeight = 590;
			base.Hide();
		}

		// Token: 0x04000079 RID: 121
		private string oldKeyValidDate = string.Empty;

		// Token: 0x0400007A RID: 122
		private Gtk.Window parentWnd;

		// Token: 0x0400007B RID: 123
		private bool parentModal;

		// Token: 0x0400007C RID: 124
		private VBox vbox4;

		// Token: 0x0400007D RID: 125
		private VBox vbox2;

		// Token: 0x0400007E RID: 126
		private EventBox eventbox1;

		// Token: 0x0400007F RID: 127
		private HBox hbox1;

		// Token: 0x04000080 RID: 128
		private VBox vbox_top;

		// Token: 0x04000081 RID: 129
		private Label label_keyCreat;

		// Token: 0x04000082 RID: 130
		private Alignment alignment_error;

		// Token: 0x04000083 RID: 131
		private HBox hbox2;

		// Token: 0x04000084 RID: 132
		private VBox vbox_imageWarring;

		// Token: 0x04000085 RID: 133
		private Alignment alignment_imageWarringTop;

		// Token: 0x04000086 RID: 134
		private ImageBin imagebin_warring;

		// Token: 0x04000087 RID: 135
		private Alignment alignment_imageWarringBottom;

		// Token: 0x04000088 RID: 136
		private Label label_error;

		// Token: 0x04000089 RID: 137
		private HSeparator hseparator1;

		// Token: 0x0400008A RID: 138
		private Table table4;

		// Token: 0x0400008B RID: 139
		private PassWordEntry entry_affirmAliasPassword;

		// Token: 0x0400008C RID: 140
		private PassWordEntry entry_affirmKeystorePassword;

		// Token: 0x0400008D RID: 141
		private Entry entry_aliasName;

		// Token: 0x0400008E RID: 142
		private PassWordEntry entry_aliasPassword;

		// Token: 0x0400008F RID: 143
		private Entry entry_city;

		// Token: 0x04000090 RID: 144
		private Entry entry_Country;

		// Token: 0x04000091 RID: 145
		private Entry entry_countyCode;

		// Token: 0x04000092 RID: 146
		private Entry entry_firstword;

		// Token: 0x04000093 RID: 147
		private Entry entry_keystorelocation;

		// Token: 0x04000094 RID: 148
		private PassWordEntry entry_keystorePassword;

		// Token: 0x04000095 RID: 149
		private Entry entry_organization;

		// Token: 0x04000096 RID: 150
		private Entry entry_unit;

		// Token: 0x04000097 RID: 151
		private Entry entry_ValidDate;

		// Token: 0x04000098 RID: 152
		private HSeparator hseparator2;

		// Token: 0x04000099 RID: 153
		private HSeparator hseparator3;

		// Token: 0x0400009A RID: 154
		private HSeparator hseparator9;

		// Token: 0x0400009B RID: 155
		private Label label_affirmAliasPassword;

		// Token: 0x0400009C RID: 156
		private Label label_affirmKeystorePassword;

		// Token: 0x0400009D RID: 157
		private Label label_aliasName;

		// Token: 0x0400009E RID: 158
		private Label label_aliasPassword;

		// Token: 0x0400009F RID: 159
		private Label label_city;

		// Token: 0x040000A0 RID: 160
		private Label label_Country;

		// Token: 0x040000A1 RID: 161
		private Label label_countyCode;

		// Token: 0x040000A2 RID: 162
		private Label label_firstword;

		// Token: 0x040000A3 RID: 163
		private Label label_KeystoreLocation;

		// Token: 0x040000A4 RID: 164
		private Label label_keystorePassword;

		// Token: 0x040000A5 RID: 165
		private Label label_organization;

		// Token: 0x040000A6 RID: 166
		private Label label_unit;

		// Token: 0x040000A7 RID: 167
		private Label label_ValidDate;

		// Token: 0x040000A8 RID: 168
		private HSeparator hseparator8;

		// Token: 0x040000A9 RID: 169
		private Button buttonCancel;

		// Token: 0x040000AA RID: 170
		private Button buttonOk;
	}
}
