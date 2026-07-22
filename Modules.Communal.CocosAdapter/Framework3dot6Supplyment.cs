using System;
using System.IO;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gtk;
using Mono.Addins;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x02000004 RID: 4
	[Extension(typeof(ICocosSupplyment))]
	internal class Framework3dot6Supplyment : BaseCocosSupplyment
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002559 File Offset: 0x00000759
		public override int Order
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000015 RID: 21 RVA: 0x0000255C File Offset: 0x0000075C
		protected override bool needBackup
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000016 RID: 22 RVA: 0x0000255F File Offset: 0x0000075F
		protected override EnumSolutionCodeType supplymentedType
		{
			get
			{
				return EnumSolutionCodeType.Complete;
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002564 File Offset: 0x00000764
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

		// Token: 0x06000018 RID: 24 RVA: 0x000025C0 File Offset: 0x000007C0
		private string CreateConsoleCommand(EnumProgramLanguage language, Cocos2dxInfo cocosInfo)
		{
			string arg = Services.ProjectsService.CurrentSolution.BaseDirectory;
			string arg2 = Path.Combine(cocosInfo.RootPath, "tools", "cocos2d-console", "bin");
			return string.Format(" upgrade -s {0} -l {1} --console-dir {2} --return-error", arg, language.ToString(), arg2);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002618 File Offset: 0x00000818
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
