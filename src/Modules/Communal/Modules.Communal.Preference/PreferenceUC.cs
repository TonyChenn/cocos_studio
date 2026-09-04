using System;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Core.ExtensionModel;
using Modules.Communal.CocosAdapter;
using Mono.Addins;

namespace Modules.Communal.Preference
{
	// Token: 0x0200000C RID: 12
	[Extension(Type = typeof(ICommandHandle))]
	public class PreferenceUC : ICommandHandle
	{
		// Token: 0x0600002B RID: 43 RVA: 0x00002581 File Offset: 0x00000781
		void ICommandHandle.Initialize()
		{
			GlobalCommand.PreferencesCmd.Execute += PreferenceUC.PreferenceExecute;
			Services.MainWindow.InitializeCompleted += this.MainWindowInitializeCompletedHandler;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000025AF File Offset: 0x000007AF
		private void MainWindowInitializeCompletedHandler(object sender, EventArgs e)
		{
			Services.MainWindow.InitializeCompleted -= this.MainWindowInitializeCompletedHandler;
			Cocos2dxServices.InstallerServices.RefreshAndroidConfig();
			PreferenceManager.Instance.RefreshEnvironmentPrompt();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000025DC File Offset: 0x000007DC
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
