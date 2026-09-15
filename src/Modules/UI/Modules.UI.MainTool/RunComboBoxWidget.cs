using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Core.Events;
using Gtk;
using Modules.Communal.CocosAdapter;
using Modules.Communal.CocosAdapter.Platform;
using Modules.Communal.MultiLanguage;
using Xwt.Drawing;

namespace Modules.UI.MainTool
{
	internal class RunComboBoxWidget : BaseToolbarWidget
	{
		public override Widget GtkWidget
		{
			get
			{
				return this.mainHbox;
			}
		}

		public RunComboBoxWidget()
		{
			this.InitWidget();
			this.Refresh();
			this.InitEvent();
		}

		private void InitWidget()
		{
			this.runImgDictionary = new Dictionary<EnumPlatform, Xwt.Drawing.Image>();
			foreach (IPlatform platform in Cocos2dxServices.PlatformServices.PlatformList)
			{
				string resourceID = string.Format("Modules.UI.MainTool.Images.Run.{0}.png", platform.PlatformType.ToString());
				Xwt.Drawing.Image icon = ImageIcon.GetIcon(resourceID);
				if (icon != null)
				{
					this.runImgDictionary[platform.PlatformType] = icon;
				}
			}
			this.platformRunList = new List<IPlatform>();
			this.button_run = new IconButton(this.runImgDictionary[CocosRecentServices.Instance.LastRunType]);
			this.button_run.TooltipText = LanguageInfo.Menu_Project_RunProject;
			this.mainComboBox = ComboBox.NewText();
			this.mainComboBox.Name = "MainComboBox";
			this.mainHbox = new HBox();
			this.mainHbox.PackStart(this.button_run, false, false, 0U);
			this.mainHbox.PackStart(this.mainComboBox, false, false, 0U);
			this.mainHbox.ShowAll();
		}

		private void InitEvent()
		{
			this.button_run.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.ButtonClickedHandler);
			this.mainComboBox.Changed += this.ComboBoxChangedHandler;
			Cocos2dxServices.SupplymentServices.SupplymentFinished += this.SupplymentFinishedHandler;
			Cocos2dxServices.RecentServices.LastRunTypeChanged += this.LastRunTypeChanged;
			Cocos2dxServices.PlatformServices.RunTypeListChanged += this.RunTypeListChangedHandler;
		}

		private void Refresh()
		{
			this.platformRunList.Clear();
			foreach (IPlatform platform in Cocos2dxServices.PlatformServices.PlatformList)
			{
				if (platform.CanShow(EnumOperationType.Run))
				{
					this.platformRunList.Add(platform);
				}
			}
			if (this.platformRunList.Count != 0)
			{
				this.mainComboBox.RemoveAllText();
				foreach (IPlatform platform in this.platformRunList)
				{
					this.mainComboBox.AppendText(platform.GetDisplayName(EnumOperationType.Run));
				}
				bool sensitive = Services.ProjectsService.CurrentSolution != null;
				this.mainComboBox.Sensitive = sensitive;
				this.button_run.Sensitive = sensitive;
				this.SetLastRunType(CocosRecentServices.Instance.LastRunType);
			}
		}

		private void SetLastRunType(EnumPlatform lastType)
		{
			int num = -1;
			for (int i = 0; i < this.platformRunList.Count; i++)
			{
				if (this.platformRunList[i].PlatformType == lastType)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				num = 0;
			}
			EnumPlatform key = lastType;
			if (this.platformRunList.FirstOrDefault((IPlatform a) => a.PlatformType == lastType) == null)
			{
				key = this.platformRunList[0].PlatformType;
			}
			this.mainComboBox.Active = num;
			this.button_run.ChangeImage(this.runImgDictionary[key]);
		}

		private void ComboBoxChangedHandler(object sender, EventArgs e)
		{
			if (this.mainComboBox.Active != -1)
			{
				IPlatform platform = this.platformRunList[this.mainComboBox.Active];
				CocosRecentServices.Instance.LastRunType = platform.PlatformType;
			}
		}

		private void ButtonClickedHandler(object sender, EventArgs e)
		{
			if (this.mainComboBox.Active != -1)
			{
				IPlatform platform = this.platformRunList[this.mainComboBox.Active];
				CocosRecentServices.Instance.LastRunType = platform.PlatformType;
				GlobalCommand.RunLastCmd.RaiseExecute(null);
			}
		}

		private void LastRunTypeChanged(object sender, EventArgs e)
		{
			this.SetLastRunType(CocosRecentServices.Instance.LastRunType);
		}

		public override void OnSolutionChanged(SolutionEventArgs args)
		{
			this.Refresh();
		}

		private void SupplymentFinishedHandler(object sender, EventArgs e)
		{
			this.Refresh();
		}

		private void RunTypeListChangedHandler(object sender, EventArgs e)
		{
			this.Refresh();
		}

		private IconButton button_run;

		private ComboBox mainComboBox;

		private HBox mainHbox;

		private List<IPlatform> platformRunList;

		private Dictionary<EnumPlatform, Xwt.Drawing.Image> runImgDictionary;
	}
}
