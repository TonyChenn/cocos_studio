using System;
using System.Diagnostics;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using Microsoft.Win32;
using MonoDevelop.Core;

namespace Modules.Communal.CocosCodeIDE
{
	// Token: 0x02000005 RID: 5
	public class CocosCodeIDEOnWindows : CocosCodeIDEService
	{
		// Token: 0x06000018 RID: 24 RVA: 0x00002BA4 File Offset: 0x00000DA4
		protected override void OpenIDEWithCmd(FilePath projectDir)
		{
			string cocosCodeIDEExePath = this.GetCocosCodeIDEExePath();
			string cocosCodeIDEDirectoryPath = this.GetCocosCodeIDEDirectoryPath();
			Process.Start(new ProcessStartInfo(cocosCodeIDEExePath)
			{
				WorkingDirectory = cocosCodeIDEDirectoryPath,
				Arguments = "\"Cocos Code IDE\" -project " + projectDir,
				UseShellExecute = false
			});
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002BF4 File Offset: 0x00000DF4
		protected override string GetCocosCodeIDEDirectoryPath()
		{
			try
			{
				string text = Services.RecentFileService.CocosCodeIDEDir;
				if (string.IsNullOrEmpty(text))
				{
					string text2 = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Cocos", "CocosCodeIDEDir", null).ToString();
					if (string.IsNullOrEmpty(text2))
					{
						return null;
					}
					text = Path.Combine(text2, "Cocos Code IDE.exe");
					if (File.Exists(text))
					{
						return Path.GetDirectoryName(text);
					}
				}
				else if (File.Exists(text))
				{
					return Path.GetDirectoryName(text);
				}
			}
			catch (Exception message)
			{
				LogConfig.Logger.Error(message);
			}
			return null;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002C88 File Offset: 0x00000E88
		protected override string GetCocosCodeIDEExePath()
		{
			string cocosCodeIDEDir = Services.RecentFileService.CocosCodeIDEDir;
			if (File.Exists(cocosCodeIDEDir))
			{
				return cocosCodeIDEDir;
			}
			string cocosCodeIDEDirectoryPath = this.GetCocosCodeIDEDirectoryPath();
			if (!string.IsNullOrEmpty(cocosCodeIDEDirectoryPath))
			{
				return Path.Combine(cocosCodeIDEDirectoryPath, "Cocos Code IDE.exe");
			}
			return null;
		}

		// Token: 0x04000012 RID: 18
		private const string Cocos_DIR_HKEY = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Cocos";

		// Token: 0x04000013 RID: 19
		private const string CocosCodeIDE_DIR_HKEY = "CocosCodeIDEDir";

		// Token: 0x04000014 RID: 20
		private const string CocosCodeIDE_EXE = "Cocos Code IDE.exe";
	}
}
