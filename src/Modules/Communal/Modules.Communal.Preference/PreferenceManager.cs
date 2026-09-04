using System;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Projects;
using Modules.Communal.CocosAdapter;
using Modules.Communal.Status;

namespace Modules.Communal.Preference
{
	// Token: 0x0200000A RID: 10
	public class PreferenceManager
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002464 File Offset: 0x00000664
		// (set) Token: 0x06000021 RID: 33 RVA: 0x0000246B File Offset: 0x0000066B
		public static PreferenceManager Instance { get; private set; } = new PreferenceManager();

		// Token: 0x06000023 RID: 35 RVA: 0x0000247F File Offset: 0x0000067F
		private PreferenceManager()
		{
			Cocos2dxServices.InstallerServices.InstallFinished += this.InstallFinishedHandler;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000024A0 File Offset: 0x000006A0
		private bool IsEnvironmentConfigComplete
		{
			get
			{
				return FrameworkHelper.EnabledVersions.Count != 0 && !string.IsNullOrEmpty(Option.UserConfig.NDKPath) && !string.IsNullOrEmpty(Option.UserConfig.SDKPath) && !string.IsNullOrEmpty(Option.UserConfig.JDKPath) && !string.IsNullOrEmpty(Option.UserConfig.ANTPath);
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002508 File Offset: 0x00000708
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

		// Token: 0x06000026 RID: 38 RVA: 0x0000253C File Offset: 0x0000073C
		public void OpenPreferenceDialog(EnumPreferenceSetting initPageType = EnumPreferenceSetting.Default)
		{
			PreferencesDialog preferencesDialog = new PreferencesDialog(initPageType);
			preferencesDialog.Run();
			preferencesDialog.Destroy();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x0000255D File Offset: 0x0000075D
		private void InstallFinishedHandler(object sender, EventArgs e)
		{
			this.RefreshEnvironmentPrompt();
		}
	}
}
