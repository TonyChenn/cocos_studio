using System;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using CocoStudio.Projects;
using GLib;
using MonoDevelop.Core;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x02000008 RID: 8
	public class CocosRecentServices
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000031 RID: 49 RVA: 0x0000291F File Offset: 0x00000B1F
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00002935 File Offset: 0x00000B35
		private CocosRecentOperation RecentOperation
		{
			get
			{
				if (this.recentOperation == null)
				{
					this.InitRecentOperation();
				}
				return this.recentOperation;
			}
			set
			{
				this.recentOperation = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000033 RID: 51 RVA: 0x0000293E File Offset: 0x00000B3E
		public static CocosRecentServices Instance
		{
			get
			{
				if (CocosRecentServices.instance == null)
				{
					CocosRecentServices.instance = new CocosRecentServices();
				}
				return CocosRecentServices.instance;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002956 File Offset: 0x00000B56
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002971 File Offset: 0x00000B71
		public EnumPublishType LastPublishType
		{
			get
			{
				if (Services.ProjectsService.CurrentSolution == null)
				{
					return EnumPublishType.Resource;
				}
				return this.RecentOperation.LastPublishType;
			}
			set
			{
				if (Services.ProjectsService.CurrentSolution == null)
				{
					return;
				}
				this.RecentOperation.LastPublishType = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000036 RID: 54 RVA: 0x0000298C File Offset: 0x00000B8C
		// (set) Token: 0x06000037 RID: 55 RVA: 0x000029C3 File Offset: 0x00000BC3
		public bool IsLastPublish
		{
			get
			{
				return Services.ProjectsService.CurrentSolution == null || this.RecentOperation.IsLastPublish;
			}
			set
			{
				if (Services.ProjectsService.CurrentSolution == null)
				{
					return;
				}
				this.RecentOperation.IsLastPublish = value;
				Timeout.Add(0U, delegate
				{
					if (this.LastPublishOperationChanged != null)
					{
						this.LastPublishOperationChanged(this, new EventArgs());
					}
					return false;
				});
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000029F1 File Offset: 0x00000BF1
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002A32 File Offset: 0x00000C32
		public EnumPlatform LastRunType
		{
			get
			{
				if (Services.ProjectsService.CurrentSolution != null)
				{
					return this.RecentOperation.LastRunType;
				}
				if (MonoDevelop.Core.Platform.IsWindows)
				{
					return EnumPlatform.Windows;
				}
				return EnumPlatform.Mac;
			}
			set
			{
				if (Services.ProjectsService.CurrentSolution == null)
				{
					return;
				}
				this.RecentOperation.LastRunType = value;
				Timeout.Add(0U, delegate
				{
					if (this.LastRunTypeChanged != null)
					{
						this.LastRunTypeChanged(this, new EventArgs());
					}
					return false;
				});
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600003A RID: 58 RVA: 0x00002A60 File Offset: 0x00000C60
		// (remove) Token: 0x0600003B RID: 59 RVA: 0x00002A98 File Offset: 0x00000C98
		public event EventHandler LastPublishOperationChanged;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600003C RID: 60 RVA: 0x00002AD0 File Offset: 0x00000CD0
		// (remove) Token: 0x0600003D RID: 61 RVA: 0x00002B08 File Offset: 0x00000D08
		public event EventHandler LastRunTypeChanged;

		// Token: 0x0600003E RID: 62 RVA: 0x00002B3D File Offset: 0x00000D3D
		static CocosRecentServices()
		{
			Services.ProjectOperations.CurrentSelectedSolutionClosed += CocosRecentServices.SolutionClosedHandler;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002B60 File Offset: 0x00000D60
		private void InitRecentOperation()
		{
			CocosRecentOperation cocosRecentOperation = null;
			Solution currentSolution = Services.ProjectsService.CurrentSolution;
			if (currentSolution != null)
			{
				IUserData userData = null;
				if (currentSolution.UserData.Properties.TryGetValue("CocosRecentOperation", out userData))
				{
					cocosRecentOperation = (userData as CocosRecentOperation);
				}
			}
			if (cocosRecentOperation == null)
			{
				cocosRecentOperation = new CocosRecentOperation();
				if (currentSolution != null)
				{
					currentSolution.UserData.Properties["CocosRecentOperation"] = cocosRecentOperation;
				}
			}
			this.RecentOperation = cocosRecentOperation;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002BC8 File Offset: 0x00000DC8
		private static void SolutionClosedHandler(object sender, SolutionEventArgs e)
		{
			CocosRecentServices.Instance.RecentOperation = null;
		}

		// Token: 0x04000005 RID: 5
		private CocosRecentOperation recentOperation;

		// Token: 0x04000006 RID: 6
		private static CocosRecentServices instance = new CocosRecentServices();
	}
}
