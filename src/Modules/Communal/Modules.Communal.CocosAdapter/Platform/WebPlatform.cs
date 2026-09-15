using System;
using System.IO;
using System.Text;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Projects;
using Microsoft.Win32;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Core;

namespace Modules.Communal.CocosAdapter.Platform
{
	[Extension(typeof(IPlatform))]
	internal class WebPlatform : BasePlatform
	{
		public override EnumPlatform PlatformType
		{
			get
			{
				return EnumPlatform.Web;
			}
		}

		public override int Order
		{
			get
			{
				return 4;
			}
		}

		protected override string PlatformName
		{
			get
			{
				return "web";
			}
		}

		public override bool IsShowConsoleWhenRun
		{
			get
			{
				return true;
			}
		}

		protected string SimulatorPath
		{
			get
			{
				if (!string.IsNullOrEmpty(this._SimulatorPath))
				{
					return this._SimulatorPath;
				}
				string text = string.Empty;
				if (MonoDevelop.Core.Platform.IsWindows)
				{
					try
					{
						object value = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\CocosSimulator.exe", null, null);
						if (value == null)
						{
							return string.Empty;
						}
						text = value.ToString();
						goto IL_61;
					}
					catch (Exception exception)
					{
						LogConfig.Logger.Error("获取模拟器安装目录时出错", exception);
						return string.Empty;
					}
				}
				text = "/Applications/Cocos/CocosSimulator/CocosSimulator.app/Contents/MacOS/Chromium";
				IL_61:
				if (File.Exists(text))
				{
					this._SimulatorPath = text;
				}
				return this._SimulatorPath;
			}
		}

		public override string GetDisplayName(EnumOperationType opType)
		{
			switch (opType)
			{
			case EnumOperationType.Package:
				return "HTML5";
			case EnumOperationType.Run:
				return LanguageInfo.Run_Web;
			default:
				return string.Empty;
			}
		}

		public override bool CanShow(EnumOperationType opType)
		{
			return (opType == EnumOperationType.Package || opType == EnumOperationType.Run) && Cocos2dxServices.CocosProperties.ProgramLanguage == EnumProgramLanguage.js;
		}

		protected override bool OnCanExecute(EnumOperationType opType, PackageParams prms)
		{
			if (Cocos2dxServices.CocosProperties.ProgramLanguage != EnumProgramLanguage.js)
			{
				return false;
			}
			if (opType == EnumOperationType.Run)
			{
				if (this.CanSolutionUseSimulator() && !this.HasSimulatorInstalled())
				{
					AskDownloadSimulatorDialog askDownloadSimulatorDialog = new AskDownloadSimulatorDialog(true);
					int num = askDownloadSimulatorDialog.Run();
					askDownloadSimulatorDialog.Destroy();
					if (num == -8)
					{
						GlobalCommand.StartLauncherCmd.RaiseExecute(null);
						return false;
					}
					if (num != -9)
					{
						return false;
					}
				}
				if (this.CanUseSimulator() && Services.RemindService.IsShowSimulatorHint)
				{
					AskDownloadSimulatorDialog askDownloadSimulatorDialog2 = new AskDownloadSimulatorDialog(false);
					int num2 = askDownloadSimulatorDialog2.Run();
					Services.RemindService.IsShowSimulatorHint = !askDownloadSimulatorDialog2.IsChecked;
					Services.RemindService.Save();
					askDownloadSimulatorDialog2.Destroy();
					if (num2 == -8)
					{
						Cocos2dxServices.PlatformServices.RaiseRunTypeListChanged();
						Cocos2dxServices.RecentServices.LastRunType = EnumPlatform.Simulator;
						GlobalCommand.RunLastCmd.RaiseExecute(null);
						return false;
					}
					if (num2 != -9)
					{
						return false;
					}
				}
			}
			return true;
		}

		protected override string OnCreateConsoleArguments(EnumOperationType opType, PackageParams prms)
		{
			string value = base.CreateGeneralArguments(opType, prms.Directory, false);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(value);
			if (opType == EnumOperationType.Package)
			{
				stringBuilder.Append(" -j 3");
				stringBuilder.Append(" --compile-script 1");
				if (prms.EnableSourceMap)
				{
					stringBuilder.Append(" --source-map");
				}
				if (prms.EnableHTML5Advanced)
				{
					stringBuilder.Append(" --advanced");
				}
			}
			return stringBuilder.ToString();
		}

		protected bool CanUseSimulator()
		{
			return this.CanSolutionUseSimulator() && this.HasSimulatorInstalled();
		}

		private bool CanSolutionUseSimulator()
		{
			if (Cocos2dxServices.CocosProperties.ProgramLanguage != EnumProgramLanguage.js)
			{
				return false;
			}
			string currentFrameworkVersion = Cocos2dxServices.CocosProperties.CurrentFrameworkVersion;
			Version v = FrameworkHelper.TryParseVersion(currentFrameworkVersion);
			return !(v == null) && !(v < new Version("3.7"));
		}

		private bool HasSimulatorInstalled()
		{
			return !string.IsNullOrEmpty(this.SimulatorPath) && File.Exists(this.SimulatorPath);
		}

		private string _SimulatorPath = string.Empty;
	}
}
