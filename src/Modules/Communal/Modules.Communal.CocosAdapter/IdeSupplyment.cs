using System;
using System.IO;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gtk;
using Mono.Addins;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x02000006 RID: 6
	[Extension(typeof(ICocosSupplyment))]
	internal class IdeSupplyment : BaseCocosSupplyment
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000027EC File Offset: 0x000009EC
		public override int Order
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000027EF File Offset: 0x000009EF
		protected override bool needBackup
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000027F2 File Offset: 0x000009F2
		protected override EnumSolutionCodeType supplymentedType
		{
			get
			{
				return EnumSolutionCodeType.CodeIDE;
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000027F8 File Offset: 0x000009F8
		protected override bool OnRunSupplyment(string frameworkVersion, EnumProgramLanguage language, string tempDir, CocosMonitor monitor)
		{
			Cocos2dxInfo simplifiedConsole = Cocos2dxInfo.GetSimplifiedConsole();
			if (simplifiedConsole == null)
			{
				monitor.SendInfo(string.Format("Failed to get Framework: {0}", frameworkVersion));
				return false;
			}
			string text = this.CreateConsoleCommand(language, simplifiedConsole);
			if (string.IsNullOrEmpty(text))
			{
				monitor.SendInfo("Failed to create command params");
				return false;
			}
			CocosPythonTool cocosPythonTool = new CocosPythonTool(monitor);
			return cocosPythonTool.RunPython(simplifiedConsole, text, false);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002854 File Offset: 0x00000A54
		private string CreateConsoleCommand(EnumProgramLanguage language, Cocos2dxInfo cocosInfo)
		{
			string arg = Services.ProjectsService.CurrentSolution.BaseDirectory;
			string arg2 = Path.Combine(cocosInfo.RootPath, "tools", "cocos2d-console", "bin");
			return string.Format(" upgrade -s {0} -l {1} --console-dir \"{2}\" --return-error", arg, language.ToString(), arg2);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000028AA File Offset: 0x00000AAA
		protected override bool OnCanSupplyment(string frameworkVersion)
		{
			return "IDE cocos".Equals(frameworkVersion);
		}
	}
}
