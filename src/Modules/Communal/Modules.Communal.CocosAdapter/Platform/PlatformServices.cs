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
	public class PlatformServices
	{
		public IReadOnlyList<IPlatform> PlatformList
		{
			get
			{
				return this._PlatformList;
			}
		}

		public event EventHandler RunTypeListChanged;

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

		private void InitEnvironment(PackageParams prms)
		{
			string directoryName = Path.GetDirectoryName(prms.EngineInfo.RootPath);
			Environment.SetEnvironmentVariable("COCOS_FRAMEWORKS", directoryName);
			Environment.SetEnvironmentVariable("ANDROID_SDK_ROOT", Option.UserConfig.SDKPath);
			Environment.SetEnvironmentVariable("NDK_ROOT", Option.UserConfig.NDKPath);
			Environment.SetEnvironmentVariable("ANT_ROOT", Option.UserConfig.ANTPath);
		}

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

		internal void RaiseRunTypeListChanged()
		{
			if (this.RunTypeListChanged != null)
			{
				this.RunTypeListChanged(this, new EventArgs());
			}
		}

		private List<IPlatform> _PlatformList;
	}
}
