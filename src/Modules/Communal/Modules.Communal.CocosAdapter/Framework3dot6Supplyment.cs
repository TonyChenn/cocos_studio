using System;
using System.IO;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gtk;
using Mono.Addins;

namespace Modules.Communal.CocosAdapter
{
	[Extension(typeof(ICocosSupplyment))]
	internal class Framework3dot6Supplyment : BaseCocosSupplyment
	{
		public override int Order
		{
			get
			{
				return 0;
			}
		}

		protected override bool needBackup
		{
			get
			{
				return false;
			}
		}

		protected override EnumSolutionCodeType supplymentedType
		{
			get
			{
				return EnumSolutionCodeType.Complete;
			}
		}

		protected override bool OnRunSupplyment(string frameworkVersion, EnumProgramLanguage language, string tempDir, CocosMonitor monitor)
		{
			Cocos2dxInfo framework = Cocos2dxInfo.GetFramework(frameworkVersion);
			if (framework == null)
			{
				monitor.SendInfo(string.Format("Failed to get Framework: {0}", frameworkVersion));
				return false;
			}
			string text = this.CreateConsoleCommand(language, framework);
			if (string.IsNullOrEmpty(text))
			{
				monitor.SendInfo("Failed to create command params");
				return false;
			}
			CocosPythonTool cocosPythonTool = new CocosPythonTool(monitor);
			return cocosPythonTool.RunPython(framework, text, false);
		}

		private string CreateConsoleCommand(EnumProgramLanguage language, Cocos2dxInfo cocosInfo)
		{
			string arg = Services.ProjectsService.CurrentSolution.BaseDirectory;
			string arg2 = Path.Combine(cocosInfo.RootPath, "tools", "cocos2d-console", "bin");
			return string.Format(" upgrade -s {0} -l {1} --console-dir {2} --return-error", arg, language.ToString(), arg2);
		}

		protected override bool OnCanSupplyment(string frameworkVersion)
		{
			if ("IDE cocos".Equals(frameworkVersion))
			{
				return false;
			}
			if (!FrameworkHelper.IsVersionEnabled(frameworkVersion))
			{
				return false;
			}
			bool flag = false;
			foreach (string text in FrameworkHelper.disableSupplymentVersions)
			{
				if (text.Equals(frameworkVersion))
				{
					flag = true;
					break;
				}
			}
			return !flag;
		}
	}
}
