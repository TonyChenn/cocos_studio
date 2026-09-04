using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.UserStatistics;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.Communal.CocosAdapter.Platform
{
	// Token: 0x02000014 RID: 20
	public class PlatformServices
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00003EB6 File Offset: 0x000020B6
		public IReadOnlyList<IPlatform> PlatformList
		{
			get
			{
				return this._PlatformList;
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000093 RID: 147 RVA: 0x00003EC0 File Offset: 0x000020C0
		// (remove) Token: 0x06000094 RID: 148 RVA: 0x00003EF8 File Offset: 0x000020F8
		public event EventHandler RunTypeListChanged;

		// Token: 0x06000095 RID: 149 RVA: 0x00003F30 File Offset: 0x00002130
		internal PlatformServices()
		{
			this._PlatformList = new List<IPlatform>();
			IPlatform[] extensionObjects = AddinManager.GetExtensionObjects<IPlatform>();
			foreach (IPlatform item in extensionObjects)
			{
				this._PlatformList.Add(item);
			}
			this._PlatformList.Sort();
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003F80 File Offset: 0x00002180
		public bool CheckCanPackage(PackageParams prms)
		{
			if (!this.CheckFramework(prms))
			{
				return false;
			}
			int num = 0;
			foreach (IPlatform platform in this._PlatformList)
			{
				if (platform.CanShow(EnumOperationType.Package) && prms.Platform.HasFlag(platform.PlatformType))
				{
					num++;
					if (!platform.CanExecute(EnumOperationType.Package, prms))
					{
						return false;
					}
				}
			}
			return num != 0;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000401C File Offset: 0x0000221C
		public int GetPackagePlatformCount(PackageParams prms)
		{
			int num = 0;
			foreach (IPlatform platform in this._PlatformList)
			{
				if (platform.CanShow(EnumOperationType.Package) && prms.Platform.HasFlag(platform.PlatformType))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004098 File Offset: 0x00002298
		public void StartPackage(PackageParams prms, CocosMonitor monitor)
		{
			monitor.Start();
			if (prms == null)
			{
				monitor.SendInfo("The package params is null");
				monitor.Finish(false);
				return;
			}
			this.InitEnvironment(prms);
			List<IPlatform> list = new List<IPlatform>();
			foreach (IPlatform platform in this._PlatformList)
			{
				if (platform.CanShow(EnumOperationType.Package) && prms.Platform.HasFlag(platform.PlatformType))
				{
					list.Add(platform);
				}
			}
			if (list.Count == 0)
			{
				monitor.SendInfo("The num of destination platform is 0");
				monitor.Finish(false);
				return;
			}
			if (!this.UpgradeFramework(monitor, prms))
			{
				monitor.Finish(false);
				return;
			}
			foreach (IPlatform platform2 in list)
			{
				string text = "PackageTo" + platform2;
				if (!platform2.Execute(EnumOperationType.Package, prms, monitor))
				{
					monitor.Finish(false);
					Tracker.Add(ViewRegions.UIMainTool, "Package", text + "Failed", "");
					return;
				}
				Tracker.Add(ViewRegions.UIMainTool, "Package", text, "");
			}
			monitor.Finish(true);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000041F4 File Offset: 0x000023F4
		public IPlatform CheckCanRun(PackageParams prms)
		{
			if (!this.CheckFramework(prms))
			{
				return null;
			}
			IPlatform platform = null;
			foreach (IPlatform platform2 in this._PlatformList)
			{
				if (platform2.CanShow(EnumOperationType.Run) && platform2.PlatformType == prms.RunPlatform)
				{
					platform = platform2;
					break;
				}
			}
			if (platform == null)
			{
				LogConfig.Logger.Error(string.Format("未找到选择的平台枚举类型对应的平台对象", prms.RunPlatform));
				return null;
			}
			if (platform.CanExecute(EnumOperationType.Run, prms))
			{
				return platform;
			}
			return null;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000429C File Offset: 0x0000249C
		public void StartRun(IPlatform platform, PackageParams prms, CocosMonitor monitor)
		{
			monitor.Start();
			if (prms == null)
			{
				monitor.SendInfo("The package params is null");
				monitor.Finish(false);
				return;
			}
			this.InitEnvironment(prms);
			if (platform == null)
			{
				monitor.SendInfo(string.Format("Cannot find the platform {0}", prms.RunPlatform));
				monitor.Finish(false);
				return;
			}
			if (!this.UpgradeFramework(monitor, prms))
			{
				monitor.Finish(false);
				return;
			}
			bool isSuccess = platform.Execute(EnumOperationType.Run, prms, monitor);
			monitor.Finish(isSuccess);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004318 File Offset: 0x00002518
		private void InitEnvironment(PackageParams prms)
		{
			string directoryName = Path.GetDirectoryName(prms.EngineInfo.RootPath);
			Environment.SetEnvironmentVariable("COCOS_FRAMEWORKS", directoryName);
			Environment.SetEnvironmentVariable("ANDROID_SDK_ROOT", Option.UserConfig.SDKPath);
			Environment.SetEnvironmentVariable("NDK_ROOT", Option.UserConfig.NDKPath);
			Environment.SetEnvironmentVariable("ANT_ROOT", Option.UserConfig.ANTPath);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000437D File Offset: 0x0000257D
		private bool CheckFramework(PackageParams prms)
		{
			prms.RefreshEngineInfo();
			if (prms.EngineInfo == null)
			{
				if (MessageBox.Show(LanguageInfo.MessageBox255_disabledFramework, MessageBoxButton.YesNo, MessageBoxImage.Other, null, EnumMainButton.Yes, null) == MessageBoxResult.Yes)
				{
					GlobalCommand.ProjectSettingCmd.RaiseExecute("Package");
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000043B4 File Offset: 0x000025B4
		private bool UpgradeFramework(CocosMonitor monitor, PackageParams prms)
		{
			string currentFrameworkVersion = Cocos2dxServices.CocosProperties.CurrentFrameworkVersion;
			string frameworkVersion = prms.FrameworkVersion;
			if (string.IsNullOrEmpty(frameworkVersion))
			{
				return false;
			}
			if (frameworkVersion.Equals(currentFrameworkVersion))
			{
				return true;
			}
			string arg = Services.ProjectsService.CurrentSolution.BaseDirectory + "-backup";
			string cmd = string.Format(" upgrade -s \"{0}\" -e {1} --backup-dir \"{2}\"", prms.Directory, prms.FrameworkVersion, arg);
			CocosPythonTool cocosPythonTool = new CocosPythonTool(monitor);
			bool flag = cocosPythonTool.RunPython(prms.EngineInfo, cmd, false);
			if (flag)
			{
				Cocos2dxServices.CocosProperties.CurrentFrameworkVersion = prms.FrameworkVersion;
				Services.ProjectsService.CurrentSolution.Config.Save();
			}
			return flag;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004467 File Offset: 0x00002667
		internal void RaiseRunTypeListChanged()
		{
			if (this.RunTypeListChanged != null)
			{
				this.RunTypeListChanged(this, new EventArgs());
			}
		}

		// Token: 0x04000028 RID: 40
		private List<IPlatform> _PlatformList;
	}
}
