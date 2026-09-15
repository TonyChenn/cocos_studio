using System;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using CocoStudio.Projects;
using GLib;
using MonoDevelop.Core;

namespace Modules.Communal.CocosAdapter
{
	public class CocosRecentServices
	{
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

		public event EventHandler LastPublishOperationChanged;

		public event EventHandler LastRunTypeChanged;

		static CocosRecentServices()
		{
			Services.ProjectOperations.CurrentSelectedSolutionClosed += CocosRecentServices.SolutionClosedHandler;
		}

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

		private static void SolutionClosedHandler(object sender, SolutionEventArgs e)
		{
			CocosRecentServices.Instance.RecentOperation = null;
		}

		private CocosRecentOperation recentOperation;

		private static CocosRecentServices instance = new CocosRecentServices();
	}
}
