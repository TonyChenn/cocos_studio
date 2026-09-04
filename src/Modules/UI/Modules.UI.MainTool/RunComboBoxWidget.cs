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
	// Token: 0x0200000A RID: 10
	internal class RunComboBoxWidget : BaseToolbarWidget
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00003280 File Offset: 0x00001480
		public override Widget GtkWidget
		{
			get
			{
				return this.mainHbox;
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003298 File Offset: 0x00001498
		public RunComboBoxWidget()
		{
			this.InitWidget();
			this.Refresh();
			this.InitEvent();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000032B8 File Offset: 0x000014B8
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

		// Token: 0x0600003D RID: 61 RVA: 0x000033F8 File Offset: 0x000015F8
		private void InitEvent()
		{
			this.button_run.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.ButtonClickedHandler);
			this.mainComboBox.Changed += this.ComboBoxChangedHandler;
			Cocos2dxServices.SupplymentServices.SupplymentFinished += this.SupplymentFinishedHandler;
			Cocos2dxServices.RecentServices.LastRunTypeChanged += this.LastRunTypeChanged;
			Cocos2dxServices.PlatformServices.RunTypeListChanged += this.RunTypeListChangedHandler;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000347C File Offset: 0x0000167C
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

		// Token: 0x0600003F RID: 63 RVA: 0x000035B0 File Offset: 0x000017B0
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

		// Token: 0x06000040 RID: 64 RVA: 0x00003688 File Offset: 0x00001888
		private void ComboBoxChangedHandler(object sender, EventArgs e)
		{
			if (this.mainComboBox.Active != -1)
			{
				IPlatform platform = this.platformRunList[this.mainComboBox.Active];
				CocosRecentServices.Instance.LastRunType = platform.PlatformType;
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000036D8 File Offset: 0x000018D8
		private void ButtonClickedHandler(object sender, EventArgs e)
		{
			if (this.mainComboBox.Active != -1)
			{
				IPlatform platform = this.platformRunList[this.mainComboBox.Active];
				CocosRecentServices.Instance.LastRunType = platform.PlatformType;
				GlobalCommand.RunLastCmd.RaiseExecute(null);
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003731 File Offset: 0x00001931
		private void LastRunTypeChanged(object sender, EventArgs e)
		{
			this.SetLastRunType(CocosRecentServices.Instance.LastRunType);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003745 File Offset: 0x00001945
		public override void OnSolutionChanged(SolutionEventArgs args)
		{
			this.Refresh();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000374F File Offset: 0x0000194F
		private void SupplymentFinishedHandler(object sender, EventArgs e)
		{
			this.Refresh();
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003759 File Offset: 0x00001959
		private void RunTypeListChangedHandler(object sender, EventArgs e)
		{
			this.Refresh();
		}

		// Token: 0x04000019 RID: 25
		private IconButton button_run;

		// Token: 0x0400001A RID: 26
		private ComboBox mainComboBox;

		// Token: 0x0400001B RID: 27
		private HBox mainHbox;

		// Token: 0x0400001C RID: 28
		private List<IPlatform> platformRunList;

		// Token: 0x0400001D RID: 29
		private Dictionary<EnumPlatform, Xwt.Drawing.Image> runImgDictionary;
	}
}
