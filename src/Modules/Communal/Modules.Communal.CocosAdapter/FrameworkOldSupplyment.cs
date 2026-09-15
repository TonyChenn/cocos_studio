using System;
using System.IO;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gtk;
using Mono.Addins;

namespace Modules.Communal.CocosAdapter
{
	[Extension(typeof(ICocosSupplyment))]
	internal class FrameworkOldSupplyment : BaseCocosSupplyment
	{
		public override int Order
		{
			get
			{
				return 1;
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
				return EnumSolutionCodeType.Complete;
			}
		}

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

		private bool CopyFramework(string originSlnDir, string tempDir, CocosMonitor monitor)
		{
			string name = Services.ProjectsService.CurrentSolution.Name;
			string srcPath = Path.Combine(tempDir, name, "frameworks");
			string dstPath = Path.Combine(originSlnDir, "frameworks");
			return Cocos2dxServices.CopyFolder(srcPath, dstPath, false, monitor);
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
