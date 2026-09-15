using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.CocosAdapter;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using Stetic;
using Xwt.Drawing;

namespace Modules.Communal.NewSolution
{
	[ToolboxItem(true)]
	public class PropertiesWidget : Bin
	{
		public bool IsEnable { get; private set; }

		public event EventHandler<EnableChangedArgs> EnableChanged;

		public event EventHandler<CreateParamsSetArgs> CreateParamsSet;

		public PropertiesWidget()
		{
			throw new Exception("请使用有参构造");
		}

		public PropertiesWidget(ISolutionTemplate slnTemplate)
		{
			this.Build();
			this.solutionTemplate = slnTemplate;
			this.InitStyles();
			this.InitEvent();
			this.InitControls();
			this.InitDisplayText();
			this.RefreshButtonEnable();
		}

		private void InitStyles()
		{
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				Xwt.Drawing.Image icon = ImageIcon.GetIcon("Modules.Communal.NewSolution.Resource.launcher_screenTypeH_normal.png");
				Xwt.Drawing.Image icon2 = ImageIcon.GetIcon("Modules.Communal.NewSolution.Resource.launcher_screenTypeH_select.png");
				Xwt.Drawing.Image icon3 = ImageIcon.GetIcon("Modules.Communal.NewSolution.Resource.launcher_screenTypeH_hover.png");
				ScreenTypeContent content = new ScreenTypeContent(icon, icon2, icon3);
				this.screenWidgetH.UseBgColor = false;
				this.screenWidgetH.SetContent(content);
				Xwt.Drawing.Image icon4 = ImageIcon.GetIcon("Modules.Communal.NewSolution.Resource.launcher_screenTypeV_normal.png");
				Xwt.Drawing.Image icon5 = ImageIcon.GetIcon("Modules.Communal.NewSolution.Resource.launcher_screenTypeV_select.png");
				Xwt.Drawing.Image icon6 = ImageIcon.GetIcon("Modules.Communal.NewSolution.Resource.launcher_screenTypeV_hover.png");
				ScreenTypeContent content2 = new ScreenTypeContent(icon4, icon5, icon6);
				this.screenWidgetV.UseBgColor = false;
				this.screenWidgetV.SetContent(content2);
				this.label_cppDes.Sensitive = true;
				this.label_luaDes.Sensitive = true;
				this.label_jsDes.Sensitive = true;
				this.label_x86DefaultDes.Sensitive = true;
				this.label_x86GCCDes.Sensitive = true;
				this.label_cppDes.ModifyFg(StateType.Normal, NewSolutionStyles.Launcher_TextGray);
				this.label_luaDes.ModifyFg(StateType.Normal, NewSolutionStyles.Launcher_TextGray);
				this.label_jsDes.ModifyFg(StateType.Normal, NewSolutionStyles.Launcher_TextGray);
				this.label_x86DefaultDes.ModifyFg(StateType.Normal, NewSolutionStyles.Launcher_TextGray);
				this.label_x86GCCDes.ModifyFg(StateType.Normal, NewSolutionStyles.Launcher_TextGray);
				this.hbox_screen.Spacing = 12;
				return;
			}
			Xwt.Drawing.Image icon7 = ImageIcon.GetIcon("Modules.Communal.NewSolution.Resource.screenType_H.png");
			ScreenTypeContent content3 = new ScreenTypeContent(icon7, null, null);
			this.screenWidgetH.SetContent(content3);
			this.screenWidgetH.NormalBgColor = NewSolutionStyles.Studio_BgGray;
			this.screenWidgetH.HoverBgColor = NewSolutionStyles.Studio_HoverDarkGray;
			this.screenWidgetH.SelectedBgColor = NewSolutionStyles.Studio_HoverDarkGray;
			this.screenWidgetH.BorderColor = NewSolutionStyles.Studio_Blue;
			this.screenWidgetH.UseBgColor = true;
			Xwt.Drawing.Image icon8 = ImageIcon.GetIcon("Modules.Communal.NewSolution.Resource.screenType_V.png");
			ScreenTypeContent content4 = new ScreenTypeContent(icon8, null, null);
			this.screenWidgetV.SetContent(content4);
			this.screenWidgetV.NormalBgColor = NewSolutionStyles.Studio_BgGray;
			this.screenWidgetV.HoverBgColor = NewSolutionStyles.Studio_HoverDarkGray;
			this.screenWidgetV.SelectedBgColor = NewSolutionStyles.Studio_HoverDarkGray;
			this.screenWidgetV.BorderColor = NewSolutionStyles.Studio_Blue;
			this.screenWidgetV.UseBgColor = true;
		}

		private void SetFontSizeRecursive(Widget widget, int size)
		{
			if (widget == null)
			{
				return;
			}
			widget.SetFontSize((double)size);
			if (widget is Gtk.Container)
			{
				Gtk.Container container = widget as Gtk.Container;
				foreach (object obj in container)
				{
					Widget widget2 = (Widget)obj;
					this.SetFontSizeRecursive(widget2, size);
				}
			}
		}

		private void InitEvent()
		{
			this.entry_name.Changed += this.OnEntryNameChanged;
			this.combobox_framework.Changed += this.ComboboxChangedHandler;
			this.entry_name.ActivatesDefault = true;
			this.entry_path.ActivatesDefault = true;
		}

		private void InitControls()
		{
			RadioGroup radioGroup = new RadioGroup("未命名");
			radioGroup.AddItem(this.screenWidgetH);
			radioGroup.AddItem(this.screenWidgetV);
			this.screenWidgetH.Select();
			string lastCreatePrjDirectory = Services.RecentFileService.LastCreatePrjDirectory;
			this.entry_name.Text = this.GetDefaultSolutionName(lastCreatePrjDirectory);
			this.hasNameChanged = false;
			this.entry_path.Text = System.IO.Path.Combine(new string[]
			{
				lastCreatePrjDirectory
			});
			if (Cocos2dxInfo.GetSimplifiedConsole() != null)
			{
				this.combobox_framework.AppendText(LanguageInfo.Color_none);
			}
			IReadOnlyList<string> enabledVersions = FrameworkHelper.EnabledVersions;
			foreach (string text in enabledVersions)
			{
				this.combobox_framework.AppendText(text);
			}
			this.combobox_framework.Active = this.combobox_framework.TextCount() - 1;
			if (!this.solutionTemplate.Info.NeedFramework)
			{
				this.table_property.Remove(this.label_framework);
				this.table_property.Remove(this.hbox_framework);
				this.table_property.Remove(this.label_language);
				this.table_property.Remove(this.table_language);
				this.table_property.Remove(this.label_addition);
				this.table_property.Remove(this.hbox_x86);
			}
			if (this.solutionTemplate.Info.SampleInfo != null)
			{
				this.table_property.Remove(this.label_screen);
				this.table_property.Remove(this.hbox_screen);
			}
		}

		private void InitDisplayText()
		{
			this.label_projTypeName.Text = this.solutionTemplate.Info.Name;
			this.label_type.Text = LanguageInfo.NewSolution_SolutionType;
			this.label_name.Text = LanguageInfo.Dialog_ProjectName;
			this.label_path.Text = LanguageInfo.Dialog_ProjectPath;
			this.label_framework.Text = LanguageInfo.Dialog_New_EngineVersion;
			this.label_language.Text = LanguageInfo.Dialog_Publish_Language;
			this.label_screen.Text = LanguageInfo.NewSolution_ScreenOrientation;
			this.label_addition.Text = LanguageInfo.Dialog_New_Addition;
			this.label_luaDes.Text = LanguageInfo.NewSolution_LuaDes;
			this.label_cppDes.Text = LanguageInfo.NewSolution_CppDes;
			this.label_jsDes.Text = LanguageInfo.NewSolution_JsDes;
			this.checkbutton_x86.Label = LanguageInfo.NewSolution_SupportX86;
			this.label_optimizeType.Text = LanguageInfo.NewSolution_OptimizeType;
			this.radiobutton_x86Default.Label = LanguageInfo.Dialog_New_X86Null;
			this.label_x86DefaultDes.Text = LanguageInfo.NewSolution_X86DefaultDes;
			this.label_x86GCCDes.Text = LanguageInfo.NewSolution_X86GCCDes;
			this.button_browse.Label = LanguageInfo.Dialog_ButtonBrowse;
			if (FrameworkHelper.EnabledVersions.Count == 0)
			{
				this.tipicon_noFramework.Text = LanguageInfo.MessageBox228_FrameworkNotFound + "\n" + LanguageInfo.NewSolution_NoFwTooltip;
			}
			else
			{
				this.tipicon_noFramework.Text = LanguageInfo.NewSolution_NoFwTooltip;
			}
			this.SetFontSizeRecursive(this.table_property, 12);
		}

		protected override void OnActivate()
		{
			string info;
			CreateParams createParams = this.GetCreateParams(out info);
			if (createParams != null)
			{
				if (this.CreateParamsSet != null)
				{
					this.CreateParamsSet(this, new CreateParamsSetArgs(createParams));
					return;
				}
			}
			else
			{
				MessageBox.Show(info, MessageBoxImage.Warning, null, null);
			}
		}

		public CreateParams GetCreateParams(out string output)
		{
			if (!this.CheckParamsValidity(out output))
			{
				return null;
			}
			string text = this.entry_name.Text;
			string text2 = this.entry_path.Text;
			bool isSelected = this.screenWidgetH.IsSelected;
			CreateParams createParams;
			if (this.solutionTemplate.Info.NeedFramework)
			{
				string packageName = this.GetPackageName(text);
				EnumProgramLanguage language;
				if (this.radiobutton1_lua.Active)
				{
					language = EnumProgramLanguage.lua;
				}
				else if (this.radiobutton2_cpp.Active)
				{
					language = EnumProgramLanguage.cpp;
				}
				else
				{
					language = EnumProgramLanguage.js;
				}
				string activeText = this.combobox_framework.ActiveText;
				Cocos2dxInfo cocos2dxInfo;
				if (activeText.Equals(LanguageInfo.Color_none))
				{
					cocos2dxInfo = Cocos2dxInfo.GetSimplifiedConsole();
				}
				else
				{
					cocos2dxInfo = Cocos2dxInfo.GetFramework(activeText);
				}
				if (cocos2dxInfo == null)
				{
					output = LanguageInfo.MessageBox229_FailedToGetFramework;
					return null;
				}
				createParams = new CreateParams(text, text2, isSelected, packageName, cocos2dxInfo, language);
				createParams.UseX86 = this.checkbutton_x86.Active;
				if (createParams.UseX86)
				{
					if (this.radiobutton_x86Default.Active)
					{
						createParams.X86Type = ECompilerType.Null;
					}
					else
					{
						createParams.X86Type = ECompilerType.GCC;
					}
				}
			}
			else
			{
				createParams = new CreateParams(text, text2, isSelected);
			}
			return createParams;
		}

		private bool CheckParamsValidity(out string output)
		{
			string text = this.entry_name.Text;
			if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
			{
				this.entry_name.HasFocus = true;
				output = LanguageInfo.MessageBox_Content41;
				return false;
			}
			if (!Regex.IsMatch(text, "^[A-Za-z0-9,._-]+$"))
			{
				this.entry_name.HasFocus = true;
				output = LanguageInfo.MessageBox209_PrjNameLimit;
				return false;
			}
			if (text.EndsWith("."))
			{
				this.entry_name.HasFocus = true;
				output = LanguageInfo.MessageBox210_CantEndWithPeriod;
				return false;
			}
			if (Platform.IsWindows && RegexModel.IsSystemReserveName(text))
			{
				this.entry_name.HasFocus = true;
				output = LanguageInfo.MessageBox215_WindowsNameLimit;
				return false;
			}
			text = this.entry_path.Text;
			if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
			{
				this.button_browse.HasFocus = true;
				output = LanguageInfo.MessageBox_Content144;
				return false;
			}
			if (!Regex.IsMatch(text, "^[\\\\*\\\\\\\\/:A-Za-z0-9,._-]+$"))
			{
				this.button_browse.HasFocus = true;
				output = LanguageInfo.MessageBox232_SlnDirLimit;
				return false;
			}
			int length = this.entry_name.Text.Length;
			int length2 = this.entry_path.Text.Length;
			if (length2 + length * 2 > 200)
			{
				if (this.entry_name.Text.Length > 25)
				{
					this.entry_name.HasFocus = true;
				}
				else
				{
					this.button_browse.HasFocus = true;
				}
				output = LanguageInfo.Messagebox267_TooLongPath;
				return false;
			}
			try
			{
				text = System.IO.Path.Combine(this.entry_path.Text, this.entry_name.Text);
				if (Directory.Exists(text))
				{
					this.entry_name.HasFocus = true;
					output = LanguageInfo.MessageBox_Content42;
					return false;
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("检查新项目路径时出错", exception);
				output = LanguageInfo.MessageBox_Content144;
				return false;
			}
			output = "";
			return true;
		}

		private string GetDefaultSolutionName(string path)
		{
			string defaultSolutionName = this.solutionTemplate.Info.DefaultSolutionName;
			string path2 = System.IO.Path.Combine(path, defaultSolutionName);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
				return defaultSolutionName;
			}
			if (this.hasNameChanged)
			{
				return defaultSolutionName;
			}
			if (!Directory.Exists(path2))
			{
				return defaultSolutionName;
			}
			int num = 1;
			for (;;)
			{
				path2 = System.IO.Path.Combine(path, defaultSolutionName + num.ToString());
				if (!Directory.Exists(path2))
				{
					break;
				}
				num++;
			}
			return defaultSolutionName + num.ToString();
		}

		private string GetPackageName(string projName)
		{
			string text = projName.Replace(",", "").Replace("_", "").Replace("-", "");
			if (string.IsNullOrEmpty(text))
			{
				text = "CocosProject";
			}
			return "org.cocos." + text;
		}

		private void RefreshButtonEnable()
		{
			bool isEnable = this.IsEnable;
			if (string.IsNullOrEmpty(this.entry_name.Text) || string.IsNullOrWhiteSpace(this.entry_name.Text) || string.IsNullOrEmpty(this.entry_path.Text))
			{
				this.IsEnable = false;
			}
			else
			{
				this.IsEnable = true;
			}
			if (isEnable != this.IsEnable && this.EnableChanged != null)
			{
				this.EnableChanged(this, new EnableChangedArgs(this.IsEnable));
			}
		}

		public override void Destroy()
		{
			this.tipicon_noFramework.HideTooltip();
			base.Destroy();
		}

		protected void OnEntryNameChanged(object sender, EventArgs e)
		{
			this.hasNameChanged = true;
			this.RefreshButtonEnable();
		}

		protected void OnBtnBrowseClicked(object sender, EventArgs e)
		{
			if (Option.IsXP)
			{
				string folder = FileChooserDialogModel.GetBrowseDialogPath(LanguageInfo.Dialog_ProjectPath, false, this.entry_path.Text, false).Folder;
				if (!string.IsNullOrEmpty(folder))
				{
					this.entry_path.Text = folder;
				}
			}
			else
			{
				SelectFolderDialog selectFolderDialog = new SelectFolderDialog();
				selectFolderDialog.TransientFor = MessageService.GetDefaultModalParent();
				selectFolderDialog.Title = LanguageInfo.Dialog_ProjectPath;
				selectFolderDialog.Action = FileChooserAction.SelectFolder;
				selectFolderDialog.SelectMultiple = false;
				selectFolderDialog.CurrentFolder = this.entry_path.Text;
				if (selectFolderDialog.Run() && !string.IsNullOrEmpty(selectFolderDialog.SelectedFile))
				{
					this.entry_path.Text = selectFolderDialog.SelectedFile;
				}
			}
			if (!this.hasNameChanged)
			{
				this.entry_name.Text = this.GetDefaultSolutionName(this.entry_path.Text);
				this.hasNameChanged = false;
			}
			this.RefreshButtonEnable();
		}

		protected void OnX86CheckBtnToggled(object sender, EventArgs e)
		{
			if (this.checkbutton_x86.Active)
			{
				this.table_buildOptimize.ShowAll();
				return;
			}
			this.table_buildOptimize.HideAll();
		}

		private void ComboboxChangedHandler(object sender, EventArgs e)
		{
			if (this.combobox_framework.Active == -1)
			{
				return;
			}
			if (this.combobox_framework.ActiveText.Equals(LanguageInfo.Color_none))
			{
				this.radiobutton2_cpp.Sensitive = false;
				if (Option.CurrentApp == EnumApp.Launcher)
				{
					this.label_cppDes.Sensitive = false;
				}
				if (this.radiobutton2_cpp.Active)
				{
					this.radiobutton1_lua.Active = true;
				}
				this.checkbutton_x86.Sensitive = false;
				this.checkbutton_x86.Active = false;
				this.tipicon_noFramework.Show();
				return;
			}
			this.radiobutton2_cpp.Sensitive = true;
			this.checkbutton_x86.Sensitive = true;
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				this.label_cppDes.Sensitive = true;
			}
			this.tipicon_noFramework.Hide();
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.NewSolution.PropertiesWidget";
			this.alignment_property = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_property.Name = "alignment_property";
			this.alignment_property.LeftPadding = 30U;
			this.alignment_property.TopPadding = 25U;
			this.alignment_property.RightPadding = 30U;
			this.table_property = new Table(10U, 2U, false);
			this.table_property.Name = "table_property";
			this.table_property.RowSpacing = 9U;
			this.table_property.ColumnSpacing = 15U;
			this.entry_name = new Entry();
			this.entry_name.HeightRequest = 26;
			this.entry_name.CanFocus = true;
			this.entry_name.Name = "entry_name";
			this.entry_name.IsEditable = true;
			this.entry_name.MaxLength = 50;
			this.entry_name.InvisibleChar = '●';
			this.table_property.Add(this.entry_name);
			Table.TableChild tableChild = (Table.TableChild)this.table_property[this.entry_name];
			tableChild.TopAttach = 2U;
			tableChild.BottomAttach = 3U;
			tableChild.LeftAttach = 1U;
			tableChild.RightAttach = 2U;
			tableChild.YOptions = AttachOptions.Fill;
			this.hbox_framework = new HBox();
			this.hbox_framework.Name = "hbox_framework";
			this.hbox_framework.Spacing = 6;
			this.combobox_framework = ComboBox.NewText();
			this.combobox_framework.WidthRequest = 200;
			this.combobox_framework.Name = "combobox_framework";
			this.hbox_framework.Add(this.combobox_framework);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox_framework[this.combobox_framework];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.vbox_tip = new VBox();
			this.vbox_tip.Name = "vbox_tip";
			this.alignment_tipTop = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_tipTop.Name = "alignment_tipTop";
			this.vbox_tip.Add(this.alignment_tipTop);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_tip[this.alignment_tipTop];
			boxChild2.Position = 0;
			this.alignment_tipIcon = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_tipIcon.Name = "alignment_tipIcon";
			this.tipicon_noFramework = new TooltipIcon();
			this.tipicon_noFramework.Events = EventMask.ButtonPressMask;
			this.tipicon_noFramework.Name = "tipicon_noFramework";
			this.alignment_tipIcon.Add(this.tipicon_noFramework);
			this.vbox_tip.Add(this.alignment_tipIcon);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox_tip[this.alignment_tipIcon];
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.alignment_tipBottom = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_tipBottom.Name = "alignment_tipBottom";
			this.vbox_tip.Add(this.alignment_tipBottom);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_tip[this.alignment_tipBottom];
			boxChild4.Position = 2;
			this.hbox_framework.Add(this.vbox_tip);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.hbox_framework[this.vbox_tip];
			boxChild5.Position = 1;
			boxChild5.Expand = false;
			this.table_property.Add(this.hbox_framework);
			Table.TableChild tableChild2 = (Table.TableChild)this.table_property[this.hbox_framework];
			tableChild2.TopAttach = 5U;
			tableChild2.BottomAttach = 6U;
			tableChild2.LeftAttach = 1U;
			tableChild2.RightAttach = 2U;
			tableChild2.XOptions = AttachOptions.Fill;
			tableChild2.YOptions = AttachOptions.Fill;
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
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox_path[this.entry_path];
			boxChild6.Position = 0;
			this.button_browse = new Button();
			this.button_browse.WidthRequest = 65;
			this.button_browse.HeightRequest = 26;
			this.button_browse.CanFocus = true;
			this.button_browse.Name = "button_browse";
			this.button_browse.UseUnderline = true;
			this.button_browse.Label = Catalog.GetString("浏览");
			this.hbox_path.Add(this.button_browse);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.hbox_path[this.button_browse];
			boxChild7.Position = 1;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			this.table_property.Add(this.hbox_path);
			Table.TableChild tableChild3 = (Table.TableChild)this.table_property[this.hbox_path];
			tableChild3.TopAttach = 3U;
			tableChild3.BottomAttach = 4U;
			tableChild3.LeftAttach = 1U;
			tableChild3.RightAttach = 2U;
			tableChild3.YOptions = AttachOptions.Fill;
			this.hbox_screen = new HBox();
			this.hbox_screen.Name = "hbox_screen";
			this.hbox_screen.Spacing = 5;
			this.screenWidgetH = new RadioItemWidget();
			this.screenWidgetH.Events = EventMask.ButtonPressMask;
			this.screenWidgetH.Name = "screenWidgetH";
			this.screenWidgetH.UseBgColor = false;
			this.hbox_screen.Add(this.screenWidgetH);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.hbox_screen[this.screenWidgetH];
			boxChild8.Position = 0;
			boxChild8.Expand = false;
			this.screenWidgetV = new RadioItemWidget();
			this.screenWidgetV.Events = EventMask.ButtonPressMask;
			this.screenWidgetV.Name = "screenWidgetV";
			this.screenWidgetV.UseBgColor = false;
			this.hbox_screen.Add(this.screenWidgetV);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.hbox_screen[this.screenWidgetV];
			boxChild9.Position = 1;
			boxChild9.Expand = false;
			this.table_property.Add(this.hbox_screen);
			Table.TableChild tableChild4 = (Table.TableChild)this.table_property[this.hbox_screen];
			tableChild4.TopAttach = 4U;
			tableChild4.BottomAttach = 5U;
			tableChild4.LeftAttach = 1U;
			tableChild4.RightAttach = 2U;
			tableChild4.XOptions = AttachOptions.Fill;
			tableChild4.YOptions = AttachOptions.Fill;
			this.hbox_x86 = new HBox();
			this.hbox_x86.Name = "hbox_x86";
			this.checkbutton_x86 = new CheckButton();
			this.checkbutton_x86.CanFocus = true;
			this.checkbutton_x86.Name = "checkbutton_x86";
			this.checkbutton_x86.Label = Catalog.GetString("支持x86架构CPU");
			this.checkbutton_x86.DrawIndicator = true;
			this.checkbutton_x86.UseUnderline = true;
			this.hbox_x86.Add(this.checkbutton_x86);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.hbox_x86[this.checkbutton_x86];
			boxChild10.Position = 0;
			boxChild10.Expand = false;
			this.table_property.Add(this.hbox_x86);
			Table.TableChild tableChild5 = (Table.TableChild)this.table_property[this.hbox_x86];
			tableChild5.TopAttach = 7U;
			tableChild5.BottomAttach = 8U;
			tableChild5.LeftAttach = 1U;
			tableChild5.RightAttach = 2U;
			tableChild5.XOptions = AttachOptions.Fill;
			tableChild5.YOptions = AttachOptions.Fill;
			this.label_addition = new Label();
			this.label_addition.Name = "label_addition";
			this.label_addition.Xalign = 1f;
			this.label_addition.LabelProp = Catalog.GetString("附加功能");
			this.table_property.Add(this.label_addition);
			Table.TableChild tableChild6 = (Table.TableChild)this.table_property[this.label_addition];
			tableChild6.TopAttach = 7U;
			tableChild6.BottomAttach = 8U;
			tableChild6.XOptions = AttachOptions.Fill;
			tableChild6.YOptions = AttachOptions.Fill;
			this.label_framework = new Label();
			this.label_framework.Name = "label_framework";
			this.label_framework.Xalign = 1f;
			this.label_framework.LabelProp = Catalog.GetString("引擎版本");
			this.table_property.Add(this.label_framework);
			Table.TableChild tableChild7 = (Table.TableChild)this.table_property[this.label_framework];
			tableChild7.TopAttach = 5U;
			tableChild7.BottomAttach = 6U;
			tableChild7.XOptions = AttachOptions.Fill;
			tableChild7.YOptions = AttachOptions.Fill;
			this.label_language = new Label();
			this.label_language.Name = "label_language";
			this.label_language.Xalign = 1f;
			this.label_language.Yalign = 0.08f;
			this.label_language.LabelProp = Catalog.GetString("项目语言");
			this.table_property.Add(this.label_language);
			Table.TableChild tableChild8 = (Table.TableChild)this.table_property[this.label_language];
			tableChild8.TopAttach = 6U;
			tableChild8.BottomAttach = 7U;
			tableChild8.XOptions = AttachOptions.Fill;
			tableChild8.YOptions = AttachOptions.Fill;
			this.label_name = new Label();
			this.label_name.Name = "label_name";
			this.label_name.Xalign = 1f;
			this.label_name.LabelProp = Catalog.GetString("项目名称");
			this.table_property.Add(this.label_name);
			Table.TableChild tableChild9 = (Table.TableChild)this.table_property[this.label_name];
			tableChild9.TopAttach = 2U;
			tableChild9.BottomAttach = 3U;
			tableChild9.XOptions = AttachOptions.Fill;
			tableChild9.YOptions = AttachOptions.Fill;
			this.label_path = new Label();
			this.label_path.Name = "label_path";
			this.label_path.Xalign = 1f;
			this.label_path.LabelProp = Catalog.GetString("项目路径");
			this.table_property.Add(this.label_path);
			Table.TableChild tableChild10 = (Table.TableChild)this.table_property[this.label_path];
			tableChild10.TopAttach = 3U;
			tableChild10.BottomAttach = 4U;
			tableChild10.XOptions = AttachOptions.Fill;
			tableChild10.YOptions = AttachOptions.Fill;
			this.label_projTypeName = new Label();
			this.label_projTypeName.Name = "label_projTypeName";
			this.label_projTypeName.Xalign = 0f;
			this.label_projTypeName.LabelProp = Catalog.GetString("空白完整项目");
			this.table_property.Add(this.label_projTypeName);
			Table.TableChild tableChild11 = (Table.TableChild)this.table_property[this.label_projTypeName];
			tableChild11.LeftAttach = 1U;
			tableChild11.RightAttach = 2U;
			tableChild11.XOptions = AttachOptions.Fill;
			tableChild11.YOptions = AttachOptions.Fill;
			this.label_screen = new Label();
			this.label_screen.Name = "label_screen";
			this.label_screen.Xalign = 1f;
			this.label_screen.LabelProp = Catalog.GetString("屏幕方向");
			this.table_property.Add(this.label_screen);
			Table.TableChild tableChild12 = (Table.TableChild)this.table_property[this.label_screen];
			tableChild12.TopAttach = 4U;
			tableChild12.BottomAttach = 5U;
			tableChild12.XOptions = AttachOptions.Fill;
			tableChild12.YOptions = AttachOptions.Fill;
			this.label_type = new Label();
			this.label_type.Name = "label_type";
			this.label_type.Xalign = 1f;
			this.label_type.LabelProp = Catalog.GetString("项目类型");
			this.table_property.Add(this.label_type);
			Table.TableChild tableChild13 = (Table.TableChild)this.table_property[this.label_type];
			tableChild13.XOptions = AttachOptions.Fill;
			tableChild13.YOptions = AttachOptions.Fill;
			this.table_buildOptimize = new Table(2U, 3U, false);
			this.table_buildOptimize.Name = "table_buildOptimize";
			this.table_buildOptimize.RowSpacing = 2U;
			this.table_buildOptimize.ColumnSpacing = 12U;
			this.label_optimizeType = new Label();
			this.label_optimizeType.Name = "label_optimizeType";
			this.label_optimizeType.LabelProp = Catalog.GetString("优化方式");
			this.table_buildOptimize.Add(this.label_optimizeType);
			Table.TableChild tableChild14 = (Table.TableChild)this.table_buildOptimize[this.label_optimizeType];
			tableChild14.XOptions = AttachOptions.Fill;
			tableChild14.YOptions = AttachOptions.Fill;
			this.label_x86DefaultDes = new Label();
			this.label_x86DefaultDes.Sensitive = false;
			this.label_x86DefaultDes.Name = "label_x86DefaultDes";
			this.label_x86DefaultDes.Xalign = 0f;
			this.label_x86DefaultDes.LabelProp = Catalog.GetString("基本优化方式，实现对x86架构CPU设备的支持");
			this.table_buildOptimize.Add(this.label_x86DefaultDes);
			Table.TableChild tableChild15 = (Table.TableChild)this.table_buildOptimize[this.label_x86DefaultDes];
			tableChild15.LeftAttach = 2U;
			tableChild15.RightAttach = 3U;
			tableChild15.XOptions = AttachOptions.Fill;
			tableChild15.YOptions = AttachOptions.Fill;
			this.label_x86GCCDes = new Label();
			this.label_x86GCCDes.Sensitive = false;
			this.label_x86GCCDes.Name = "label_x86GCCDes";
			this.label_x86GCCDes.LabelProp = Catalog.GetString("GNU编译器套件优化，实现对x86架构CPU设备的支持");
			this.table_buildOptimize.Add(this.label_x86GCCDes);
			Table.TableChild tableChild16 = (Table.TableChild)this.table_buildOptimize[this.label_x86GCCDes];
			tableChild16.TopAttach = 1U;
			tableChild16.BottomAttach = 2U;
			tableChild16.LeftAttach = 2U;
			tableChild16.RightAttach = 3U;
			tableChild16.XOptions = AttachOptions.Fill;
			tableChild16.YOptions = AttachOptions.Fill;
			this.radiobutton_x86Default = new RadioButton(Catalog.GetString("默认"));
			this.radiobutton_x86Default.CanFocus = true;
			this.radiobutton_x86Default.Name = "radiobutton_x86Default";
			this.radiobutton_x86Default.DrawIndicator = true;
			this.radiobutton_x86Default.UseUnderline = true;
			this.radiobutton_x86Default.Group = new SList(IntPtr.Zero);
			this.table_buildOptimize.Add(this.radiobutton_x86Default);
			Table.TableChild tableChild17 = (Table.TableChild)this.table_buildOptimize[this.radiobutton_x86Default];
			tableChild17.LeftAttach = 1U;
			tableChild17.RightAttach = 2U;
			tableChild17.XOptions = AttachOptions.Fill;
			tableChild17.YOptions = AttachOptions.Fill;
			this.radiobutton_x86GCC = new RadioButton(Catalog.GetString("GCC"));
			this.radiobutton_x86GCC.CanFocus = true;
			this.radiobutton_x86GCC.Name = "radiobutton_x86GCC";
			this.radiobutton_x86GCC.DrawIndicator = true;
			this.radiobutton_x86GCC.UseUnderline = true;
			this.radiobutton_x86GCC.Group = this.radiobutton_x86Default.Group;
			this.table_buildOptimize.Add(this.radiobutton_x86GCC);
			Table.TableChild tableChild18 = (Table.TableChild)this.table_buildOptimize[this.radiobutton_x86GCC];
			tableChild18.TopAttach = 1U;
			tableChild18.BottomAttach = 2U;
			tableChild18.LeftAttach = 1U;
			tableChild18.RightAttach = 2U;
			tableChild18.XOptions = AttachOptions.Fill;
			tableChild18.YOptions = AttachOptions.Fill;
			this.table_property.Add(this.table_buildOptimize);
			Table.TableChild tableChild19 = (Table.TableChild)this.table_property[this.table_buildOptimize];
			tableChild19.TopAttach = 8U;
			tableChild19.BottomAttach = 9U;
			tableChild19.LeftAttach = 1U;
			tableChild19.RightAttach = 2U;
			tableChild19.XOptions = AttachOptions.Fill;
			tableChild19.YOptions = AttachOptions.Fill;
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
			Table.TableChild tableChild20 = (Table.TableChild)this.table_language[this.label_cppDes];
			tableChild20.TopAttach = 1U;
			tableChild20.BottomAttach = 2U;
			tableChild20.LeftAttach = 1U;
			tableChild20.RightAttach = 2U;
			tableChild20.XOptions = AttachOptions.Fill;
			tableChild20.YOptions = AttachOptions.Fill;
			this.label_jsDes = new Label();
			this.label_jsDes.Sensitive = false;
			this.label_jsDes.Name = "label_jsDes";
			this.label_jsDes.Xalign = 0f;
			this.label_jsDes.LabelProp = Catalog.GetString("高性能HTML5跨终端解决方案");
			this.table_language.Add(this.label_jsDes);
			Table.TableChild tableChild21 = (Table.TableChild)this.table_language[this.label_jsDes];
			tableChild21.TopAttach = 2U;
			tableChild21.BottomAttach = 3U;
			tableChild21.LeftAttach = 1U;
			tableChild21.RightAttach = 2U;
			tableChild21.XOptions = AttachOptions.Fill;
			tableChild21.YOptions = AttachOptions.Fill;
			this.label_luaDes = new Label();
			this.label_luaDes.Sensitive = false;
			this.label_luaDes.Name = "label_luaDes";
			this.label_luaDes.Xalign = 0f;
			this.label_luaDes.LabelProp = Catalog.GetString("支持更多的扩展控件");
			this.table_language.Add(this.label_luaDes);
			Table.TableChild tableChild22 = (Table.TableChild)this.table_language[this.label_luaDes];
			tableChild22.LeftAttach = 1U;
			tableChild22.RightAttach = 2U;
			tableChild22.XOptions = AttachOptions.Fill;
			tableChild22.YOptions = AttachOptions.Fill;
			this.radiobutton1_lua = new RadioButton(Catalog.GetString("Lua"));
			this.radiobutton1_lua.CanFocus = true;
			this.radiobutton1_lua.Name = "radiobutton1_lua";
			this.radiobutton1_lua.DrawIndicator = true;
			this.radiobutton1_lua.UseUnderline = true;
			this.radiobutton1_lua.Group = new SList(IntPtr.Zero);
			this.table_language.Add(this.radiobutton1_lua);
			Table.TableChild tableChild23 = (Table.TableChild)this.table_language[this.radiobutton1_lua];
			tableChild23.XOptions = AttachOptions.Fill;
			tableChild23.YOptions = AttachOptions.Fill;
			this.radiobutton2_cpp = new RadioButton(Catalog.GetString("C++"));
			this.radiobutton2_cpp.CanFocus = true;
			this.radiobutton2_cpp.Name = "radiobutton2_cpp";
			this.radiobutton2_cpp.DrawIndicator = true;
			this.radiobutton2_cpp.UseUnderline = true;
			this.radiobutton2_cpp.Group = this.radiobutton1_lua.Group;
			this.table_language.Add(this.radiobutton2_cpp);
			Table.TableChild tableChild24 = (Table.TableChild)this.table_language[this.radiobutton2_cpp];
			tableChild24.TopAttach = 1U;
			tableChild24.BottomAttach = 2U;
			tableChild24.XOptions = AttachOptions.Fill;
			tableChild24.YOptions = AttachOptions.Fill;
			this.radiobutton3_js = new RadioButton(Catalog.GetString("JavaScript"));
			this.radiobutton3_js.CanFocus = true;
			this.radiobutton3_js.Name = "radiobutton3_js";
			this.radiobutton3_js.DrawIndicator = true;
			this.radiobutton3_js.UseUnderline = true;
			this.radiobutton3_js.Group = this.radiobutton1_lua.Group;
			this.table_language.Add(this.radiobutton3_js);
			Table.TableChild tableChild25 = (Table.TableChild)this.table_language[this.radiobutton3_js];
			tableChild25.TopAttach = 2U;
			tableChild25.BottomAttach = 3U;
			tableChild25.XOptions = AttachOptions.Fill;
			tableChild25.YOptions = AttachOptions.Fill;
			this.table_property.Add(this.table_language);
			Table.TableChild tableChild26 = (Table.TableChild)this.table_property[this.table_language];
			tableChild26.TopAttach = 6U;
			tableChild26.BottomAttach = 7U;
			tableChild26.LeftAttach = 1U;
			tableChild26.RightAttach = 2U;
			tableChild26.XOptions = AttachOptions.Fill;
			tableChild26.YOptions = AttachOptions.Fill;
			this.alignment_property.Add(this.table_property);
			base.Add(this.alignment_property);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			this.table_buildOptimize.Hide();
			base.Hide();
			this.checkbutton_x86.Toggled += this.OnX86CheckBtnToggled;
			this.button_browse.Clicked += this.OnBtnBrowseClicked;
		}

		private bool hasNameChanged;

		private ISolutionTemplate solutionTemplate;

		private Alignment alignment_property;

		private Table table_property;

		private Entry entry_name;

		private HBox hbox_framework;

		private ComboBox combobox_framework;

		private VBox vbox_tip;

		private Alignment alignment_tipTop;

		private Alignment alignment_tipIcon;

		private TooltipIcon tipicon_noFramework;

		private Alignment alignment_tipBottom;

		private HBox hbox_path;

		private Entry entry_path;

		private Button button_browse;

		private HBox hbox_screen;

		private RadioItemWidget screenWidgetH;

		private RadioItemWidget screenWidgetV;

		private HBox hbox_x86;

		private CheckButton checkbutton_x86;

		private Label label_addition;

		private Label label_framework;

		private Label label_language;

		private Label label_name;

		private Label label_path;

		private Label label_projTypeName;

		private Label label_screen;

		private Label label_type;

		private Table table_buildOptimize;

		private Label label_optimizeType;

		private Label label_x86DefaultDes;

		private Label label_x86GCCDes;

		private RadioButton radiobutton_x86Default;

		private RadioButton radiobutton_x86GCC;

		private Table table_language;

		private Label label_cppDes;

		private Label label_jsDes;

		private Label label_luaDes;

		private RadioButton radiobutton1_lua;

		private RadioButton radiobutton2_cpp;

		private RadioButton radiobutton3_js;
	}
}
