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
	internal class PublishPackageWidget : BaseToolbarWidget
	{
		public override Widget GtkWidget
		{
			get
			{
				return this.mainComboBox;
			}
		}

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

		public override void OnSolutionChanged(SolutionEventArgs args)
		{
			this.Refresh();
		}

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

		private void ButtonClickedHandler(object sender, EventArgs e)
		{
			GlobalCommand.PublishPackageLastCmd.RaiseExecute(null);
		}

		private void LastPublishOpChangedHandler(object sender, EventArgs e)
		{
			this.Refresh();
		}

		private void SupplymentFinishedHandler(object sender, EventArgs e)
		{
			this.Refresh();
		}

		private ComboBox mainComboBox;

		private IconButton mainButton;

		private Xwt.Drawing.Image imgResource;

		private Xwt.Drawing.Image imgCodeIDE;

		private Xwt.Drawing.Image imgVisualStudio;

		private Xwt.Drawing.Image imgXcode;

		private Dictionary<EnumPublishType, Xwt.Drawing.Image> imgDictionary;

		private Xwt.Drawing.Image imgPackage;
	}
}
