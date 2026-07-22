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
	// Token: 0x02000016 RID: 22
	[Extension(typeof(IPlatform))]
	internal class SimulatorPlatform : WebPlatform
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00004766 File Offset: 0x00002966
		public override EnumPlatform PlatformType
		{
			get
			{
				return EnumPlatform.Simulator;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000AD RID: 173 RVA: 0x0000476A File Offset: 0x0000296A
		public override int Order
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000476D File Offset: 0x0000296D
		public override string GetDisplayName(EnumOperationType opType)
		{
			if (opType == EnumOperationType.Run)
			{
				return LanguageInfo.Run_Simulator;
			}
			return string.Empty;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000477E File Offset: 0x0000297E
		public override bool CanShow(EnumOperationType opType)
		{
			return opType == EnumOperationType.Run && base.CanUseSimulator();
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0000478C File Offset: 0x0000298C
		protected override bool OnCanExecute(EnumOperationType opType, PackageParams prms)
		{
			return opType == EnumOperationType.Run && base.CanUseSimulator();
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000479A File Offset: 0x0000299A
		protected override bool OnExecuteInitialize(EnumOperationType opType, PackageParams prms, CocosMonitor monitor)
		{
			return this.CloseWebSimulator();
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000047A4 File Offset: 0x000029A4
		protected override bool OnExecute(EnumOperationType opType, PackageParams prms, CocosMonitor monitor)
		{
			bool result = base.OnExecute(opType, prms, monitor);
			this.CloseWebSimulator();
			return result;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000047C4 File Offset: 0x000029C4
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

		// Token: 0x060000B4 RID: 180 RVA: 0x00004838 File Offset: 0x00002A38
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
