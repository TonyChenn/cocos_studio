using System;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Projects;
using Modules.Communal.CocosAdapter;
using Modules.Communal.Status;

namespace Modules.Communal.Preference
{
	public class PreferenceManager
	{
		public static PreferenceManager Instance { get; private set; } = new PreferenceManager();

		private PreferenceManager()
		{
			Cocos2dxServices.InstallerServices.InstallFinished += this.InstallFinishedHandler;
		}

		private bool IsEnvironmentConfigComplete
		{
			get
			{
				return FrameworkHelper.EnabledVersions.Count != 0 && !string.IsNullOrEmpty(Option.UserConfig.NDKPath) && !string.IsNullOrEmpty(Option.UserConfig.SDKPath) && !string.IsNullOrEmpty(Option.UserConfig.JDKPath) && !string.IsNullOrEmpty(Option.UserConfig.ANTPath);
			}
		}

		public void RefreshEnvironmentPrompt()
		{
			IStatusBar service = Services.GetService<IStatusBar>();
			if (service == null)
			{
				return;
			}
			if (this.IsEnvironmentConfigComplete)
			{
				service.HideWarningInfo();
				return;
			}
			service.ShowWarningInfo(new EnvironmentWarningInfo());
		}

		public void OpenPreferenceDialog(EnumPreferenceSetting initPageType = EnumPreferenceSetting.Default)
		{
			PreferencesDialog preferencesDialog = new PreferencesDialog(initPageType);
			preferencesDialog.Run();
			preferencesDialog.Destroy();
		}

		private void InstallFinishedHandler(object sender, EventArgs e)
		{
			this.RefreshEnvironmentPrompt();
		}
	}
}
