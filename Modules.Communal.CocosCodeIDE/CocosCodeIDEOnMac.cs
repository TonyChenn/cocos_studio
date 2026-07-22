using System;
using System.Diagnostics;
using System.IO;
using MonoDevelop.Core;

namespace Modules.Communal.CocosCodeIDE
{
	// Token: 0x02000004 RID: 4
	public class CocosCodeIDEOnMac : CocosCodeIDEService
	{
		// Token: 0x06000014 RID: 20 RVA: 0x00002B00 File Offset: 0x00000D00
		protected override void OpenIDEWithCmd(FilePath projectDir)
		{
			string cocosCodeIDEExePath = this.GetCocosCodeIDEExePath();
			Process.Start(new ProcessStartInfo(cocosCodeIDEExePath)
			{
				Arguments = "-project " + projectDir,
				UseShellExecute = false
			});
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002B40 File Offset: 0x00000D40
		protected override string GetCocosCodeIDEDirectoryPath()
		{
			string text = "/Applications/Cocos Code IDE.app";
			if (Directory.Exists(text))
			{
				return text;
			}
			text = "/Applications/Cocos/Cocos Code IDE.app";
			if (Directory.Exists(text))
			{
				return text;
			}
			return null;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002B70 File Offset: 0x00000D70
		protected override string GetCocosCodeIDEExePath()
		{
			string cocosCodeIDEDirectoryPath = this.GetCocosCodeIDEDirectoryPath();
			if (!string.IsNullOrEmpty(cocosCodeIDEDirectoryPath))
			{
				return Path.Combine(cocosCodeIDEDirectoryPath, "Contents/MacOS/eclipse");
			}
			return null;
		}

		// Token: 0x0400000F RID: 15
		private const string InstallDir_Mac_Cocos = "/Applications/Cocos/Cocos Code IDE.app";

		// Token: 0x04000010 RID: 16
		private const string InstallDir_Mac_Alone = "/Applications/Cocos Code IDE.app";

		// Token: 0x04000011 RID: 17
		private const string InstallDir_Mac_RelativePath = "Contents/MacOS/eclipse";
	}
}
