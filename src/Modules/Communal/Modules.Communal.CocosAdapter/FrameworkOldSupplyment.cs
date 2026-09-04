using System;
using System.IO;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gtk;
using Mono.Addins;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x02000005 RID: 5
	[Extension(typeof(ICocosSupplyment))]
	internal class FrameworkOldSupplyment : BaseCocosSupplyment
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002694 File Offset: 0x00000894
		public override int Order
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002697 File Offset: 0x00000897
		protected override bool needBackup
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000269A File Offset: 0x0000089A
		protected override EnumSolutionCodeType supplymentedType
		{
			get
			{
				return EnumSolutionCodeType.Complete;
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000026A0 File Offset: 0x000008A0
		protected override bool OnRunSupplyment(string frameworkVersion, EnumProgramLanguage language, string tempDir, CocosMonitor monitor)
		{
			bool result = false;
			string originSlnDir = Services.ProjectsService.CurrentSolution.BaseDirectory;
			if (Cocos2dxServices.CocosProperties.SolutionCodeType == EnumSolutionCodeType.CodeIDE)
			{
				result = this.CopyFramework(originSlnDir, tempDir, monitor);
			}
			else if (Cocos2dxServices.CocosProperties.SolutionCodeType == EnumSolutionCodeType.Resource)
			{
				result = base.CopySourceCode(originSlnDir, tempDir, monitor);
			}
			return result;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000026F8 File Offset: 0x000008F8
		protected override bool OnCreateTempSolution(string dir, string frameworkVersion, EnumProgramLanguage language, CocosMonitor monitor)
		{
			Cocos2dxInfo framework = Cocos2dxInfo.GetFramework(frameworkVersion);
			if (framework == null)
			{
				monitor.SendInfo(string.Format("Failed to find framework", new object[0]));
				return false;
			}
			return base.ToCreateSolution(dir, framework, language, monitor);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002734 File Offset: 0x00000934
		private bool CopyFramework(string originSlnDir, string tempDir, CocosMonitor monitor)
		{
			string name = Services.ProjectsService.CurrentSolution.Name;
			string srcPath = Path.Combine(tempDir, name, "frameworks");
			string dstPath = Path.Combine(originSlnDir, "frameworks");
			return Cocos2dxServices.CopyFolder(srcPath, dstPath, false, monitor);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002774 File Offset: 0x00000974
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
			bool result = false;
			foreach (string text in FrameworkHelper.disableSupplymentVersions)
			{
				if (text.Equals(frameworkVersion))
				{
					result = true;
					break;
				}
			}
			return result;
		}
	}
}
