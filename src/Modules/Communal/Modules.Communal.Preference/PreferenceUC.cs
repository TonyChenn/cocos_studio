using System;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Core.ExtensionModel;
using Modules.Communal.CocosAdapter;
using Mono.Addins;

namespace Modules.Communal.Preference
{
	[Extension(Type = typeof(ICommandHandle))]
	public class PreferenceUC : ICommandHandle
	{
		void ICommandHandle.Initialize()
		{
			GlobalCommand.PreferencesCmd.Execute += PreferenceUC.PreferenceExecute;
			Services.MainWindow.InitializeCompleted += this.MainWindowInitializeCompletedHandler;
		}

		private void MainWindowInitializeCompletedHandler(object sender, EventArgs e)
		{
			Services.MainWindow.InitializeCompleted -= this.MainWindowInitializeCompletedHandler;
			Cocos2dxServices.InstallerServices.RefreshAndroidConfig();
			PreferenceManager.Instance.RefreshEnvironmentPrompt();
		}

		private static void PreferenceExecute(object sender, CommandRunArgs args)
		{
			EnumPreferenceSetting initPageType = EnumPreferenceSetting.Default;
			if (args.DataItem != null && !Enum.TryParse<EnumPreferenceSetting>(args.DataItem.ToString(), out initPageType))
			{
				initPageType = EnumPreferenceSetting.Default;
			}
			PreferenceManager.Instance.OpenPreferenceDialog(initPageType);
		}
	}
}
