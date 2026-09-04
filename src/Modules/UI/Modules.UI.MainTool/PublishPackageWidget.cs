using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Core.Events;
using Gtk;
using Modules.Communal.CocosAdapter;
using Xwt.Drawing;

namespace Modules.UI.MainTool
{
	// Token: 0x02000009 RID: 9
	internal class PublishPackageWidget : BaseToolbarWidget
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002F78 File Offset: 0x00001178
		public override Widget GtkWidget
		{
			get
			{
				return this.mainComboBox;
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002F90 File Offset: 0x00001190
		public PublishPackageWidget()
		{
			this.imgResource = ImageIcon.GetIcon("Modules.UI.MainTool.Images.PublishResource.png");
			this.imgCodeIDE = ImageIcon.GetIcon("Modules.UI.MainTool.Images.PublishToCodeIDE.png");
			this.imgVisualStudio = ImageIcon.GetIcon("Modules.UI.MainTool.Images.PublishToVS.png");
			this.imgXcode = ImageIcon.GetIcon("Modules.UI.MainTool.Images.PublishToXcode.png");
			this.imgDictionary = new Dictionary<EnumPublishType, Xwt.Drawing.Image>();
			this.imgDictionary[EnumPublishType.Resource] = this.imgResource;
			this.imgDictionary[EnumPublishType.CodeIDE] = this.imgCodeIDE;
			this.imgDictionary[EnumPublishType.VS] = this.imgVisualStudio;
			this.imgDictionary[EnumPublishType.XCode] = this.imgXcode;
			this.imgPackage = ImageIcon.GetIcon("Modules.UI.MainTool.Images.Package.png");
			this.mainComboBox = ComboBox.NewText();
			this.mainComboBox.Name = "MainComboBox";
			this.mainButton = new IconButton(this.imgResource);
			this.mainComboBox.Add(this.mainButton);
			this.mainComboBox.ShowAll();
			this.Refresh();
			this.mainButton.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.ButtonClickedHandler);
			this.mainComboBox.Changed += this.ComboBoxChangedHandler;
			Cocos2dxServices.RecentServices.LastPublishOperationChanged += this.LastPublishOpChangedHandler;
			Cocos2dxServices.SupplymentServices.SupplymentFinished += this.SupplymentFinishedHandler;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003100 File Offset: 0x00001300
		private void Refresh()
		{
			bool flag = true;
			if (Services.ProjectsService.CurrentSolution == null)
			{
				this.mainComboBox.Sensitive = false;
			}
			else
			{
				this.mainComboBox.Sensitive = true;
				flag = CocosRecentServices.Instance.IsLastPublish;
			}
			if (flag)
			{
				EnumPublishType lastPublishType = CocosRecentServices.Instance.LastPublishType;
				this.mainButton.ChangeImage(this.imgDictionary[lastPublishType]);
			}
			else
			{
				this.mainButton.ChangeImage(this.imgPackage);
			}
			this.mainButton.TooltipText = GlobalCommand.PublishPackageLastCmd.GetTooltipText(true);
			this.mainComboBox.RemoveAllText();
			this.mainComboBox.AppendText(GlobalCommand.PublishPackageCmd.Text);
			this.mainComboBox.AppendText(GlobalCommand.PublishPackageLastCmd.GetTooltipText(false));
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000031E2 File Offset: 0x000013E2
		public override void OnSolutionChanged(SolutionEventArgs args)
		{
			this.Refresh();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000031EC File Offset: 0x000013EC
		private void ComboBoxChangedHandler(object sender, EventArgs e)
		{
			if (this.mainComboBox.ActiveText != null)
			{
				if (this.mainComboBox.ActiveText.Equals(GlobalCommand.PublishPackageCmd.Text))
				{
					GlobalCommand.PublishPackageCmd.RaiseExecute(null);
				}
				else
				{
					GlobalCommand.PublishPackageLastCmd.RaiseExecute(null);
				}
				this.mainComboBox.Active = -1;
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000325D File Offset: 0x0000145D
		private void ButtonClickedHandler(object sender, EventArgs e)
		{
			GlobalCommand.PublishPackageLastCmd.RaiseExecute(null);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000326C File Offset: 0x0000146C
		private void LastPublishOpChangedHandler(object sender, EventArgs e)
		{
			this.Refresh();
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003276 File Offset: 0x00001476
		private void SupplymentFinishedHandler(object sender, EventArgs e)
		{
			this.Refresh();
		}

		// Token: 0x04000011 RID: 17
		private ComboBox mainComboBox;

		// Token: 0x04000012 RID: 18
		private IconButton mainButton;

		// Token: 0x04000013 RID: 19
		private Xwt.Drawing.Image imgResource;

		// Token: 0x04000014 RID: 20
		private Xwt.Drawing.Image imgCodeIDE;

		// Token: 0x04000015 RID: 21
		private Xwt.Drawing.Image imgVisualStudio;

		// Token: 0x04000016 RID: 22
		private Xwt.Drawing.Image imgXcode;

		// Token: 0x04000017 RID: 23
		private Dictionary<EnumPublishType, Xwt.Drawing.Image> imgDictionary;

		// Token: 0x04000018 RID: 24
		private Xwt.Drawing.Image imgPackage;
	}
}
