using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Projects;
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
	// Token: 0x0200000A RID: 10
	[ToolboxItem(true)]
	public class PublishWidget : Bin, IProjectSettingWidget
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00002E0F File Offset: 0x0000100F
		public PublishWidget()
		{
			this.Build();
			this.InitWidgets();
			this.SetMultiLanguage();
			this.InitStatus();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002E45 File Offset: 0x00001045
		private void InitWidgets()
		{
			this.InitSerailzerWidget();
			this.InitWarningWidget();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002E54 File Offset: 0x00001054
		private void InitSerailzerWidget()
		{
			this.radiobutton_custom.Active = true;
			List<BaseCocosFileSerializer> list = new List<BaseCocosFileSerializer>();
			this.manager = Services.ProjectsService.SerializeManager;
			foreach (IGameFileSerializer gameFileSerializer in this.manager.GetSerializerList())
			{
				BaseCocosFileSerializer baseCocosFileSerializer = gameFileSerializer as BaseCocosFileSerializer;
				if (baseCocosFileSerializer != null && baseCocosFileSerializer.IsDefault)
				{
					list.Add(baseCocosFileSerializer);
				}
				else
				{
					this.addinSerializers.Add(gameFileSerializer);
				}
			}
			list.Sort();
			foreach (BaseCocosFileSerializer serializer in list)
			{
				SerializerWidget serializerWidget = new SerializerWidget(serializer, this.radiobutton_custom.Group);
				serializerWidget.Selected += this.HandleSerializerSelected;
				this.baseSerializerWidgets.Add(serializerWidget);
				this.vbox_baseSerializer.PackStart(serializerWidget, false, false, 0U);
				serializerWidget.Show();
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002F74 File Offset: 0x00001174
		private void InitWarningWidget()
		{
			this.warningIcon = new TooltipIcon();
			this.warningIcon.Text = LanguageInfo.ProjSetting_VersionNotFit;
			this.warningIcon.Show();
			HelpButton helpButton = new HelpButton(null);
			this.alignment_labelLink.Add(helpButton);
			helpButton.Show();
			helpButton.URL = LanguageAdapter.GetLocalizedUrl(HelpLinkUrl.PublishDataFormat);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002FD0 File Offset: 0x000011D0
		private void SetMultiLanguage()
		{
			this.GtkLabel_publishContent.Text = " " + LanguageInfo.ProjSetting_PublishContent + " ";
			this.GtkLabel_dataFormat.Text = " " + LanguageInfo.ProjSetting_DataFormat + " ";
			this.GtkLabel_publishPath.Text = " " + LanguageInfo.ProjSetting_PublishDir + " ";
			this.radiobutton_publishAll.Label = LanguageInfo.ProjSetting_PublishAll;
			this.radiobutton_publishUsed.Label = LanguageInfo.ProjSetting_ResAndProjFile;
			this.radiobutton_projFileOnly.Label = LanguageInfo.ProjSetting_ProjFileOnly;
			this.radiobutton_publishAll.TooltipText = LanguageInfo.ProjSetting_PublishAll_ToolTip;
			this.radiobutton_publishUsed.TooltipText = LanguageInfo.ProjSetting_ResAndProjFile_ToolTip;
			this.radiobutton_projFileOnly.TooltipText = LanguageInfo.ProjSetting_ProjFileOnly_ToolTip;
			this.radiobutton_custom.Label = LanguageInfo.Dialog_Publish_Custom;
			this.label_custom.Text = LanguageInfo.ProjSetting_customInfo;
			this.button_browse.Label = LanguageInfo.Dialog_ButtonBrowse + "...";
			this.button_openFolder.Label = LanguageInfo.Launcher_Open + "...";
			this.button_openFolder.TooltipText = LanguageInfo.Command_OpenDirectory;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003100 File Offset: 0x00001300
		private void InitStatus()
		{
			if (this.addinSerializers.Count > 0)
			{
				foreach (IGameFileSerializer gameFileSerializer in this.addinSerializers)
				{
					this.combobox_custom.AppendText(gameFileSerializer.Label);
				}
				this.combobox_custom.Active = 0;
				this.combobox_custom.Sensitive = false;
			}
			else
			{
				this.combobox_custom.AppendText(LanguageInfo.Color_none);
				this.combobox_custom.Active = 0;
				this.combobox_custom.Sensitive = false;
				this.radiobutton_custom.Sensitive = false;
				this.label_custom.Sensitive = false;
			}
			Solution currentSolution = Services.ProjectsService.CurrentSolution;
			SolutionConfig config = currentSolution.Config;
			string value = config.CustomSerializer;
			if (string.IsNullOrEmpty(value))
			{
				value = config.DefaultSerializer;
			}
			if (string.IsNullOrEmpty(value))
			{
				value = "Serializer_FlatBuffers";
			}
			this.baseSerializerWidgets[0].IsSelected = true;
			this.originSerializer = this.baseSerializerWidgets[0].Serializer;
			bool flag = false;
			foreach (SerializerWidget serializerWidget in this.baseSerializerWidgets)
			{
				if (serializerWidget.Serializer.ID.Equals(value))
				{
					serializerWidget.IsSelected = true;
					this.originSerializer = serializerWidget.Serializer;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (int i = 0; i < this.addinSerializers.Count; i++)
				{
					if (this.addinSerializers[i].ID.Equals(value))
					{
						this.combobox_custom.Active = i;
						this.originSerializer = this.addinSerializers[i];
						this.radiobutton_custom.Active = true;
						break;
					}
				}
			}
			switch (config.PublishType)
			{
			case PublishType.None:
				this.radiobutton_projFileOnly.Active = true;
				break;
			case PublishType.Reference:
				this.radiobutton_publishUsed.Active = true;
				break;
			case PublishType.All:
				this.radiobutton_publishAll.Active = true;
				break;
			}
			this.entry_publishPath.Text = config.PublishDirectory;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00003350 File Offset: 0x00001550
		public EnumProjectSetting SettingID
		{
			get
			{
				return EnumProjectSetting.Publish;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00003353 File Offset: 0x00001553
		public string DisplayName
		{
			get
			{
				return LanguageInfo.Dialog_Publish_Title;
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000335C File Offset: 0x0000155C
		public void ApplySetting()
		{
			Solution currentSolution = Services.ProjectsService.CurrentSolution;
			SolutionConfig config = currentSolution.Config;
			bool flag = false;
			foreach (SerializerWidget serializerWidget in this.baseSerializerWidgets)
			{
				if (serializerWidget.IsSelected)
				{
					this.manager.CurrentSerializer = serializerWidget.Serializer;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				int active = this.combobox_custom.Active;
				IGameFileSerializer gameFileSerializer = this.addinSerializers[active];
				if (this.originSerializer != gameFileSerializer)
				{
					this.manager.CurrentSerializer = gameFileSerializer;
				}
			}
			if (this.radiobutton_publishUsed.Active)
			{
				config.PublishType = PublishType.Reference;
			}
			else if (this.radiobutton_publishAll.Active)
			{
				config.PublishType = PublishType.All;
			}
			else
			{
				config.PublishType = PublishType.None;
			}
			config.PublishDirectory = this.entry_publishPath.Text;
			currentSolution.SetPublishDirectory();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000345C File Offset: 0x0000165C
		public bool CanApply(out string output)
		{
			if (!ProjectSettingHelper.CheckPathValidity(this.entry_publishPath.Text))
			{
				this.entry_publishPath.SelectAll();
				this.entry_publishPath.HasFocus = true;
				output = LanguageInfo.MessageBox228_IllegalPublishPath;
				return false;
			}
			output = "";
			return true;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003498 File Offset: 0x00001698
		public Widget GetWidget()
		{
			return this;
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000031 RID: 49 RVA: 0x0000349B File Offset: 0x0000169B
		public List<IProjectSettingWidget> SubWidgets
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000349E File Offset: 0x0000169E
		private void OnRadioBtnCustomToggled(object sender, EventArgs e)
		{
			if (this.radiobutton_custom.Active)
			{
				this.combobox_custom.Sensitive = true;
				this.alignment_warring.RemoveChild();
				return;
			}
			this.combobox_custom.Sensitive = false;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000034D4 File Offset: 0x000016D4
		protected void HandlePublishPathBrowseClick(object sender, EventArgs e)
		{
			Solution currentSelectedSolution = Services.ProjectOperations.CurrentSelectedSolution;
			string text = this.entry_publishPath.Text;
			if (!System.IO.Path.IsPathRooted(text))
			{
				text = System.IO.Path.Combine(currentSelectedSolution.BaseDirectory, text);
				text = System.IO.Path.GetFullPath(text);
			}
			if (!Directory.Exists(text))
			{
				text = System.IO.Path.GetFullPath(currentSelectedSolution.BaseDirectory);
			}
			string text2 = "";
			if (Option.IsXP)
			{
				text2 = FileChooserDialogModel.GetBrowseDialogPath(LanguageInfo.Dialog_ProjectPath, false, text, false).Folder;
			}
			else
			{
				SelectFolderDialog selectFolderDialog = new SelectFolderDialog();
				selectFolderDialog.TransientFor = MessageService.GetDefaultModalParent();
				selectFolderDialog.Title = LanguageInfo.Dialog_ProjectPath;
				selectFolderDialog.Action = FileChooserAction.SelectFolder;
				selectFolderDialog.SelectMultiple = false;
				selectFolderDialog.CurrentFolder = text;
				if (selectFolderDialog.Run())
				{
					text2 = selectFolderDialog.SelectedFile;
				}
			}
			if (!string.IsNullOrWhiteSpace(text2))
			{
				this.entry_publishPath.Text = ProjectSettingHelper.GetUnifiedPath(text2, currentSelectedSolution.BaseDirectory);
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000035C4 File Offset: 0x000017C4
		protected void HandleSerializerSelected(object sender, EventArgs args)
		{
			SerializerWidget serializerWidget = sender as SerializerWidget;
			if (serializerWidget.IsFitDefaultSerializer)
			{
				this.alignment_warring.RemoveChild();
				return;
			}
			if (this.alignment_warring.Child == null)
			{
				this.alignment_warring.Add(this.warningIcon);
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000360C File Offset: 0x0000180C
		protected void ButtonOpenFolderClickedHandler(object sender, EventArgs e)
		{
			try
			{
				if (ProjectSettingHelper.CheckPathValidity(this.entry_publishPath.Text))
				{
					string text = ProjectSettingHelper.ConvertToAbsolutePath(this.entry_publishPath.Text);
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					PlatformAdapter.PlatformService.OpenFile(text);
				}
				else
				{
					MessageBox.Show(LanguageInfo.MessageBox228_IllegalPublishPath, MessageBoxImage.Error, null, null);
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("打开发布目录时出错", exception);
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000368C File Offset: 0x0000188C
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.ProjectSetting.PublishWidget";
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 10;
			this.frame_publishContent = new Frame();
			this.frame_publishContent.Name = "frame_publishContent";
			this.GtkAlignment_publishContent = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_publishContent.Name = "GtkAlignment_publishContent";
			this.GtkAlignment_publishContent.LeftPadding = 20U;
			this.GtkAlignment_publishContent.TopPadding = 10U;
			this.GtkAlignment_publishContent.RightPadding = 10U;
			this.GtkAlignment_publishContent.BottomPadding = 10U;
			this.vbox2 = new VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			this.radiobutton_publishAll = new RadioButton(Catalog.GetString("全部发布"));
			this.radiobutton_publishAll.CanFocus = true;
			this.radiobutton_publishAll.Name = "radiobutton_publishAll";
			this.radiobutton_publishAll.DrawIndicator = true;
			this.radiobutton_publishAll.UseUnderline = true;
			this.radiobutton_publishAll.Group = new SList(IntPtr.Zero);
			this.vbox2.Add(this.radiobutton_publishAll);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox2[this.radiobutton_publishAll];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.radiobutton_publishUsed = new RadioButton(Catalog.GetString("发布引用资源与工程文件"));
			this.radiobutton_publishUsed.CanFocus = true;
			this.radiobutton_publishUsed.Name = "radiobutton_publishUsed";
			this.radiobutton_publishUsed.DrawIndicator = true;
			this.radiobutton_publishUsed.UseUnderline = true;
			this.radiobutton_publishUsed.Group = this.radiobutton_publishAll.Group;
			this.vbox2.Add(this.radiobutton_publishUsed);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox2[this.radiobutton_publishUsed];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.radiobutton_projFileOnly = new RadioButton(Catalog.GetString("仅发布工程文件"));
			this.radiobutton_projFileOnly.CanFocus = true;
			this.radiobutton_projFileOnly.Name = "radiobutton_projFileOnly";
			this.radiobutton_projFileOnly.DrawIndicator = true;
			this.radiobutton_projFileOnly.UseUnderline = true;
			this.radiobutton_projFileOnly.Group = this.radiobutton_publishAll.Group;
			this.vbox2.Add(this.radiobutton_projFileOnly);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox2[this.radiobutton_projFileOnly];
			boxChild3.Position = 2;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.GtkAlignment_publishContent.Add(this.vbox2);
			this.frame_publishContent.Add(this.GtkAlignment_publishContent);
			this.GtkLabel_publishContent = new Label();
			this.GtkLabel_publishContent.Name = "GtkLabel_publishContent";
			this.GtkLabel_publishContent.LabelProp = Catalog.GetString("发布内容");
			this.GtkLabel_publishContent.UseMarkup = true;
			this.frame_publishContent.LabelWidget = this.GtkLabel_publishContent;
			this.vbox_main.Add(this.frame_publishContent);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_main[this.frame_publishContent];
			boxChild4.Position = 0;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.frame_publishPath = new Frame();
			this.frame_publishPath.Name = "frame_publishPath";
			this.GtkAlignment_publishPath = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_publishPath.Name = "GtkAlignment_publishPath";
			this.GtkAlignment_publishPath.LeftPadding = 20U;
			this.GtkAlignment_publishPath.TopPadding = 10U;
			this.GtkAlignment_publishPath.RightPadding = 10U;
			this.GtkAlignment_publishPath.BottomPadding = 10U;
			this.hbox_publishPath = new HBox();
			this.hbox_publishPath.Name = "hbox_publishPath";
			this.hbox_publishPath.Spacing = 6;
			this.entry_publishPath = new Entry();
			this.entry_publishPath.CanFocus = true;
			this.entry_publishPath.Name = "entry_publishPath";
			this.entry_publishPath.IsEditable = true;
			this.entry_publishPath.InvisibleChar = '●';
			this.hbox_publishPath.Add(this.entry_publishPath);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.hbox_publishPath[this.entry_publishPath];
			boxChild5.Position = 0;
			this.button_browse = new Button();
			this.button_browse.WidthRequest = 65;
			this.button_browse.CanFocus = true;
			this.button_browse.Name = "button_browse";
			this.button_browse.Label = Catalog.GetString("浏览");
			this.hbox_publishPath.Add(this.button_browse);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox_publishPath[this.button_browse];
			boxChild6.Position = 1;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.button_openFolder = new Button();
			this.button_openFolder.WidthRequest = 65;
			this.button_openFolder.CanFocus = true;
			this.button_openFolder.Name = "button_openFolder";
			this.button_openFolder.UseUnderline = true;
			this.button_openFolder.Label = Catalog.GetString("打开");
			this.hbox_publishPath.Add(this.button_openFolder);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.hbox_publishPath[this.button_openFolder];
			boxChild7.Position = 2;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			this.GtkAlignment_publishPath.Add(this.hbox_publishPath);
			this.frame_publishPath.Add(this.GtkAlignment_publishPath);
			this.GtkLabel_publishPath = new Label();
			this.GtkLabel_publishPath.Name = "GtkLabel_publishPath";
			this.GtkLabel_publishPath.LabelProp = Catalog.GetString("发布路径");
			this.GtkLabel_publishPath.UseMarkup = true;
			this.frame_publishPath.LabelWidget = this.GtkLabel_publishPath;
			this.vbox_main.Add(this.frame_publishPath);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox_main[this.frame_publishPath];
			boxChild8.Position = 1;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			this.frame_dataFormat = new Frame();
			this.frame_dataFormat.Name = "frame_dataFormat";
			this.GtkAlignment_dataFormat = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_dataFormat.Name = "GtkAlignment_dataFormat";
			this.GtkAlignment_dataFormat.LeftPadding = 20U;
			this.GtkAlignment_dataFormat.TopPadding = 5U;
			this.GtkAlignment_dataFormat.RightPadding = 10U;
			this.GtkAlignment_dataFormat.BottomPadding = 10U;
			this.hbox_dataFormat = new HBox();
			this.hbox_dataFormat.Name = "hbox_dataFormat";
			this.hbox_dataFormat.Spacing = 6;
			this.alignment_dataFormat = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_dataFormat.Name = "alignment_dataFormat";
			this.alignment_dataFormat.TopPadding = 5U;
			this.vbox_dataFormat = new VBox();
			this.vbox_dataFormat.Name = "vbox_dataFormat";
			this.vbox_dataFormat.Spacing = 6;
			this.vbox_baseSerializer = new VBox();
			this.vbox_baseSerializer.Name = "vbox_baseSerializer";
			this.vbox_baseSerializer.Spacing = 6;
			this.vbox_dataFormat.Add(this.vbox_baseSerializer);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.vbox_dataFormat[this.vbox_baseSerializer];
			boxChild9.Position = 0;
			this.hbox_custom = new HBox();
			this.hbox_custom.Name = "hbox_custom";
			this.hbox_custom.Spacing = 12;
			this.radiobutton_custom = new RadioButton(Catalog.GetString("自定义"));
			this.radiobutton_custom.CanFocus = true;
			this.radiobutton_custom.Name = "radiobutton_custom";
			this.radiobutton_custom.DrawIndicator = true;
			this.radiobutton_custom.UseUnderline = true;
			this.radiobutton_custom.Group = new SList(IntPtr.Zero);
			this.hbox_custom.Add(this.radiobutton_custom);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.hbox_custom[this.radiobutton_custom];
			boxChild10.Position = 0;
			boxChild10.Expand = false;
			this.combobox_custom = ComboBox.NewText();
			this.combobox_custom.WidthRequest = 150;
			this.combobox_custom.Name = "combobox_custom";
			this.hbox_custom.Add(this.combobox_custom);
			Box.BoxChild boxChild11 = (Box.BoxChild)this.hbox_custom[this.combobox_custom];
			boxChild11.Position = 1;
			boxChild11.Expand = false;
			boxChild11.Fill = false;
			this.vbox_dataFormat.Add(this.hbox_custom);
			Box.BoxChild boxChild12 = (Box.BoxChild)this.vbox_dataFormat[this.hbox_custom];
			boxChild12.Position = 1;
			boxChild12.Expand = false;
			boxChild12.Fill = false;
			this.hbox_customDes = new HBox();
			this.hbox_customDes.Name = "hbox_customDes";
			this.hbox_customDes.Spacing = 6;
			this.alignment_custom = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_custom.Name = "alignment_custom";
			this.alignment_custom.LeftPadding = 20U;
			this.label_custom = new Label();
			this.label_custom.WidthRequest = 320;
			this.label_custom.Name = "label_custom";
			this.label_custom.Xalign = 0f;
			this.label_custom.LabelProp = Catalog.GetString("根据用户需求自行配置数据格式");
			this.label_custom.Wrap = true;
			this.alignment_custom.Add(this.label_custom);
			this.hbox_customDes.Add(this.alignment_custom);
			Box.BoxChild boxChild13 = (Box.BoxChild)this.hbox_customDes[this.alignment_custom];
			boxChild13.Position = 0;
			boxChild13.Expand = false;
			boxChild13.Fill = false;
			this.vbox_dataFormat.Add(this.hbox_customDes);
			Box.BoxChild boxChild14 = (Box.BoxChild)this.vbox_dataFormat[this.hbox_customDes];
			boxChild14.Position = 2;
			boxChild14.Expand = false;
			boxChild14.Fill = false;
			this.alignment_dataFormat.Add(this.vbox_dataFormat);
			this.hbox_dataFormat.Add(this.alignment_dataFormat);
			Box.BoxChild boxChild15 = (Box.BoxChild)this.hbox_dataFormat[this.alignment_dataFormat];
			boxChild15.Position = 0;
			this.vbox_helpLink = new VBox();
			this.vbox_helpLink.Name = "vbox_helpLink";
			this.vbox_helpLink.Spacing = 6;
			this.alignment_labelLink = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_labelLink.Name = "alignment_labelLink";
			this.alignment_labelLink.RightPadding = 10U;
			this.vbox_helpLink.Add(this.alignment_labelLink);
			Box.BoxChild boxChild16 = (Box.BoxChild)this.vbox_helpLink[this.alignment_labelLink];
			boxChild16.Position = 0;
			boxChild16.Expand = false;
			this.alignment_warring = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_warring.Name = "alignment_warring";
			this.alignment_warring.RightPadding = 10U;
			this.vbox_helpLink.Add(this.alignment_warring);
			Box.BoxChild boxChild17 = (Box.BoxChild)this.vbox_helpLink[this.alignment_warring];
			boxChild17.PackType = PackType.End;
			boxChild17.Position = 1;
			boxChild17.Expand = false;
			this.hbox_dataFormat.Add(this.vbox_helpLink);
			Box.BoxChild boxChild18 = (Box.BoxChild)this.hbox_dataFormat[this.vbox_helpLink];
			boxChild18.Position = 1;
			boxChild18.Expand = false;
			this.GtkAlignment_dataFormat.Add(this.hbox_dataFormat);
			this.frame_dataFormat.Add(this.GtkAlignment_dataFormat);
			this.GtkLabel_dataFormat = new Label();
			this.GtkLabel_dataFormat.Name = "GtkLabel_dataFormat";
			this.GtkLabel_dataFormat.LabelProp = Catalog.GetString("数据格式");
			this.GtkLabel_dataFormat.UseMarkup = true;
			this.frame_dataFormat.LabelWidget = this.GtkLabel_dataFormat;
			this.vbox_main.Add(this.frame_dataFormat);
			Box.BoxChild boxChild19 = (Box.BoxChild)this.vbox_main[this.frame_dataFormat];
			boxChild19.Position = 2;
			boxChild19.Expand = false;
			base.Add(this.vbox_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
			this.button_browse.Clicked += this.HandlePublishPathBrowseClick;
			this.button_openFolder.Clicked += this.ButtonOpenFolderClickedHandler;
			this.radiobutton_custom.Toggled += this.OnRadioBtnCustomToggled;
		}

		// Token: 0x0400001A RID: 26
		private List<IGameFileSerializer> addinSerializers = new List<IGameFileSerializer>();

		// Token: 0x0400001B RID: 27
		private List<SerializerWidget> baseSerializerWidgets = new List<SerializerWidget>();

		// Token: 0x0400001C RID: 28
		private IGameFileSerializer originSerializer;

		// Token: 0x0400001D RID: 29
		private ISerializeManager manager;

		// Token: 0x0400001E RID: 30
		private TooltipIcon warningIcon;

		// Token: 0x0400001F RID: 31
		private bool hasLuaRenameAgreed;

		// Token: 0x04000020 RID: 32
		private VBox vbox_main;

		// Token: 0x04000021 RID: 33
		private Frame frame_publishContent;

		// Token: 0x04000022 RID: 34
		private Alignment GtkAlignment_publishContent;

		// Token: 0x04000023 RID: 35
		private VBox vbox2;

		// Token: 0x04000024 RID: 36
		private RadioButton radiobutton_publishAll;

		// Token: 0x04000025 RID: 37
		private RadioButton radiobutton_publishUsed;

		// Token: 0x04000026 RID: 38
		private RadioButton radiobutton_projFileOnly;

		// Token: 0x04000027 RID: 39
		private Label GtkLabel_publishContent;

		// Token: 0x04000028 RID: 40
		private Frame frame_publishPath;

		// Token: 0x04000029 RID: 41
		private Alignment GtkAlignment_publishPath;

		// Token: 0x0400002A RID: 42
		private HBox hbox_publishPath;

		// Token: 0x0400002B RID: 43
		private Entry entry_publishPath;

		// Token: 0x0400002C RID: 44
		private Button button_browse;

		// Token: 0x0400002D RID: 45
		private Button button_openFolder;

		// Token: 0x0400002E RID: 46
		private Label GtkLabel_publishPath;

		// Token: 0x0400002F RID: 47
		private Frame frame_dataFormat;

		// Token: 0x04000030 RID: 48
		private Alignment GtkAlignment_dataFormat;

		// Token: 0x04000031 RID: 49
		private HBox hbox_dataFormat;

		// Token: 0x04000032 RID: 50
		private Alignment alignment_dataFormat;

		// Token: 0x04000033 RID: 51
		private VBox vbox_dataFormat;

		// Token: 0x04000034 RID: 52
		private VBox vbox_baseSerializer;

		// Token: 0x04000035 RID: 53
		private HBox hbox_custom;

		// Token: 0x04000036 RID: 54
		private RadioButton radiobutton_custom;

		// Token: 0x04000037 RID: 55
		private ComboBox combobox_custom;

		// Token: 0x04000038 RID: 56
		private HBox hbox_customDes;

		// Token: 0x04000039 RID: 57
		private Alignment alignment_custom;

		// Token: 0x0400003A RID: 58
		private Label label_custom;

		// Token: 0x0400003B RID: 59
		private VBox vbox_helpLink;

		// Token: 0x0400003C RID: 60
		private Alignment alignment_labelLink;

		// Token: 0x0400003D RID: 61
		private Alignment alignment_warring;

		// Token: 0x0400003E RID: 62
		private Label GtkLabel_dataFormat;
	}
}
