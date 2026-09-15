using System;
using System.Diagnostics;
using System.Text;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Core;

namespace Modules.Communal.CocosAdapter.Platform
{
	[Extension(typeof(IPlatform))]
	internal class SimulatorPlatform : WebPlatform
	{
		public override EnumPlatform PlatformType
		{
			get
			{
				return EnumPlatform.Simulator;
			}
		}

		public override int Order
		{
			get
			{
				return 5;
			}
		}

		public override string GetDisplayName(EnumOperationType opType)
		{
			if (opType == EnumOperationType.Run)
			{
				return LanguageInfo.Run_Simulator;
			}
			return string.Empty;
		}

		public override bool CanShow(EnumOperationType opType)
		{
			return opType == EnumOperationType.Run && base.CanUseSimulator();
		}

		protected override bool OnCanExecute(EnumOperationType opType, PackageParams prms)
		{
			return opType == EnumOperationType.Run && base.CanUseSimulator();
		}

		protected override bool OnExecuteInitialize(EnumOperationType opType, PackageParams prms, CocosMonitor monitor)
		{
			return this.CloseWebSimulator();
		}

		protected override bool OnExecute(EnumOperationType opType, PackageParams prms, CocosMonitor monitor)
		{
			bool result = base.OnExecute(opType, prms, monitor);
			this.CloseWebSimulator();
			return result;
		}

		protected override string OnCreateConsoleArguments(EnumOperationType opType, PackageParams prms)
		{
			string value = base.CreateGeneralArguments(opType, prms.Directory, false);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(value);
			Solution currentSolution = Services.ProjectsService.CurrentSolution;
			stringBuilder.Append(string.Format(" -b \"{0}\"", base.SimulatorPath));
			stringBuilder.Append(string.Format(" --param \"--cocos-game-url=http://127.0.0.1:8000/index.html --cocos-debug-dir={0}\"", currentSolution.BaseDirectory));
			return stringBuilder.ToString();
		}

		private bool CloseWebSimulator()
		{
			string processName = "CocosSimulator";
			if (MonoDevelop.Core.Platform.IsMac)
			{
				processName = "Chromium";
			}
			try
			{
				foreach (Process process in Process.GetProcessesByName(processName))
				{
					process.Kill();
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("尝试关闭Cocos Web Simulator时出错", exception);
				return false;
			}
			return true;
		}
	}
}
