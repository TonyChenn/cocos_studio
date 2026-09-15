using System;
using System.IO;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gtk;
using Mono.Addins;

namespace Modules.Communal.CocosAdapter
{
	[Extension(typeof(ICocosSupplyment))]
	internal class IdeSupplyment : BaseCocosSupplyment
	{
		public override int Order
		{
			get
			{
				return 2;
			}
		}

		protected override bool needBackup
		{
			get
			{
				return true;
			}
		}

		protected override EnumSolutionCodeType supplymentedType
		{
			get
			{
				return EnumSolutionCodeType.CodeIDE;
			}
		}

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

		private string CreateConsoleCommand(EnumProgramLanguage language, Cocos2dxInfo cocosInfo)
		{
			string arg = Services.ProjectsService.CurrentSolution.BaseDirectory;
			string arg2 = Path.Combine(cocosInfo.RootPath, "tools", "cocos2d-console", "bin");
			return string.Format(" upgrade -s {0} -l {1} --console-dir \"{2}\" --return-error", arg, language.ToString(), arg2);
		}

		protected override bool OnCanSupplyment(string frameworkVersion)
		{
			return "IDE cocos".Equals(frameworkVersion);
		}
	}
}
