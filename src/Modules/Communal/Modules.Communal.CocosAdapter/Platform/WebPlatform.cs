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
	// Token: 0x02000015 RID: 21
	[Extension(typeof(IPlatform))]
	internal class WebPlatform : BasePlatform
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00004482 File Offset: 0x00002682
		public override EnumPlatform PlatformType
		{
			get
			{
				return EnumPlatform.Web;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00004485 File Offset: 0x00002685
		public override int Order
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00004488 File Offset: 0x00002688
		protected override string PlatformName
		{
			get
			{
				return "web";
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x0000448F File Offset: 0x0000268F
		public override bool IsShowConsoleWhenRun
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00004494 File Offset: 0x00002694
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

		// Token: 0x060000A4 RID: 164 RVA: 0x0000452C File Offset: 0x0000272C
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

		// Token: 0x060000A5 RID: 165 RVA: 0x0000455E File Offset: 0x0000275E
		public override bool CanShow(EnumOperationType opType)
		{
			return (opType == EnumOperationType.Package || opType == EnumOperationType.Run) && Cocos2dxServices.CocosProperties.ProgramLanguage == EnumProgramLanguage.js;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000457C File Offset: 0x0000277C
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

		// Token: 0x060000A7 RID: 167 RVA: 0x00004654 File Offset: 0x00002854
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

		// Token: 0x060000A8 RID: 168 RVA: 0x000046CD File Offset: 0x000028CD
		protected bool CanUseSimulator()
		{
			return this.CanSolutionUseSimulator() && this.HasSimulatorInstalled();
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000046E4 File Offset: 0x000028E4
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

		// Token: 0x060000AA RID: 170 RVA: 0x00004732 File Offset: 0x00002932
		private bool HasSimulatorInstalled()
		{
			return !string.IsNullOrEmpty(this.SimulatorPath) && File.Exists(this.SimulatorPath);
		}

		// Token: 0x0400002A RID: 42
		private string _SimulatorPath = string.Empty;
	}
}
