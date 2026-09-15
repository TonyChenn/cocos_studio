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
	public class Cocos2dxSupplymentServices
	{
		public string DefaultCppPublishDir
		{
			get
			{
				return "Resources/res/";
			}
		}

		public event EventHandler SupplymentFinished;

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

		private bool CheckNeedSupplyment(EnumSolutionCodeType dstCodeType)
		{
			EnumSolutionCodeType solutionCodeType = Cocos2dxServices.CocosProperties.SolutionCodeType;
			return solutionCodeType < dstCodeType;
		}

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

		private IEnumerable<ICocosSupplyment> supplymentList;
	}
}
