using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x02000009 RID: 9
	public class Cocos2dxSupplymentServices
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002BDD File Offset: 0x00000DDD
		public string DefaultCppPublishDir
		{
			get
			{
				return "Resources/res/";
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000045 RID: 69 RVA: 0x00002BE4 File Offset: 0x00000DE4
		// (remove) Token: 0x06000046 RID: 70 RVA: 0x00002C1C File Offset: 0x00000E1C
		public event EventHandler SupplymentFinished;

		// Token: 0x06000047 RID: 71 RVA: 0x00002C5C File Offset: 0x00000E5C
		internal Cocos2dxSupplymentServices()
		{
			List<ICocosSupplyment> list = new List<ICocosSupplyment>();
			ICocosSupplyment[] extensionObjects = AddinManager.GetExtensionObjects<ICocosSupplyment>();
			foreach (ICocosSupplyment item in extensionObjects)
			{
				list.Add(item);
			}
			this.supplymentList = from items in list
			orderby items.Order
			select items;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002CC4 File Offset: 0x00000EC4
		public bool Supplyment(EnumSolutionCodeType dstCodeType, EnumOperationType operationType)
		{
			if (dstCodeType == EnumSolutionCodeType.Resource)
			{
				return true;
			}
			if (FrameworkHelper.EnabledVersions.Count == 0 && (dstCodeType == EnumSolutionCodeType.Complete || (dstCodeType == EnumSolutionCodeType.CodeIDE && Cocos2dxInfo.GetSimplifiedConsole() == null)))
			{
				if (MessageBox.Show(LanguageInfo.CodeSupplyment_NeedFramework, MessageBoxButton.YesNo, MessageBoxImage.Other, null, EnumMainButton.Yes, null) == MessageBoxResult.Yes)
				{
					Cocos2dxServices.InstallerServices.StartInstaller(false, true);
				}
				return false;
			}
			if (this.CheckNeedSupplyment(dstCodeType))
			{
				Cocos2dxSupplymentDialog cocos2dxSupplymentDialog = new Cocos2dxSupplymentDialog(dstCodeType == EnumSolutionCodeType.CodeIDE, operationType);
				int num = cocos2dxSupplymentDialog.Run();
				cocos2dxSupplymentDialog.Destroy();
				return num == -5 && this.RunSupplyment(cocos2dxSupplymentDialog.FrameworkVersion, cocos2dxSupplymentDialog.Language);
			}
			return true;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002D78 File Offset: 0x00000F78
		private bool RunSupplyment(string frameworkVersion, EnumProgramLanguage language)
		{
			ICocosSupplyment supplymenter = null;
			foreach (ICocosSupplyment cocosSupplyment in this.supplymentList)
			{
				if (cocosSupplyment.CanSupplyment(frameworkVersion))
				{
					supplymenter = cocosSupplyment;
					break;
				}
			}
			if (supplymenter == null)
			{
				return false;
			}
			CSCocosHelp.StopAllEffects();
			CocosMonitor monitor = new CocosMonitor(true);
			Task task = new Task(delegate()
			{
				supplymenter.RunSupplyment(frameworkVersion, language, monitor);
			});
			task.Start();
			ProcessDialog processDialog = new ProcessDialog(LanguageInfo.CodeSupplyment_ProjectUpgrade, true, false);
			processDialog.StartRunning(monitor);
			processDialog.Run();
			processDialog.Destroy();
			bool isSuccessed = monitor.IsSuccessed;
			if (isSuccessed)
			{
				if (this.SupplymentFinished != null)
				{
					this.SupplymentFinished(this, new EventArgs());
				}
				LogConfig.Output.Info(LanguageInfo.CodeSupplyment_UpgradeSuccess, true);
			}
			else
			{
				LogConfig.Output.Info(LanguageInfo.CodeSupplyment_UpgradeFailed, true);
			}
			return isSuccessed;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002EA4 File Offset: 0x000010A4
		private bool CheckNeedSupplyment(EnumSolutionCodeType dstCodeType)
		{
			EnumSolutionCodeType solutionCodeType = Cocos2dxServices.CocosProperties.SolutionCodeType;
			return solutionCodeType < dstCodeType;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002EC4 File Offset: 0x000010C4
		internal bool CheckNeedResetPublishDir(EnumProgramLanguage language)
		{
			if (Services.ProjectsService.CurrentSolution == null)
			{
				return false;
			}
			Solution currentSolution = Services.ProjectsService.CurrentSolution;
			SolutionConfig config = currentSolution.Config;
			string text = config.PublishDirectory.ToLower();
			if (language == EnumProgramLanguage.cpp)
			{
				if (text.Equals("resources/res") || text.Equals(this.DefaultCppPublishDir.ToLower()))
				{
					return false;
				}
			}
			else if (text.Equals("res") || text.Equals("res\\") || text.Equals("res/"))
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002F4C File Offset: 0x0000114C
		internal string GetDefaultPublishDir(EnumProgramLanguage language)
		{
			string result;
			if (language == EnumProgramLanguage.cpp)
			{
				result = this.DefaultCppPublishDir;
			}
			else
			{
				result = Solution.DefaultPublishDirectoryName;
			}
			return result;
		}

		// Token: 0x04000009 RID: 9
		private IEnumerable<ICocosSupplyment> supplymentList;
	}
}
