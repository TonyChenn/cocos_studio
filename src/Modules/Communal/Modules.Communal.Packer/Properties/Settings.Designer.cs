using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Modules.Communal.Packer.Properties
{
	// Token: 0x0200001A RID: 26
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00006210 File Offset: 0x00004410
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x04000040 RID: 64
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
