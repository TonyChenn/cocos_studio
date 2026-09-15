using System;
using System.Diagnostics;
using System.IO;
using MonoDevelop.Core;

namespace Modules.Communal.CocosCodeIDE
{
	public class CocosCodeIDEOnMac : CocosCodeIDEService
	{
		protected override void OpenIDEWithCmd(FilePath projectDir)
		{
			string cocosCodeIDEExePath = this.GetCocosCodeIDEExePath();
			Process.Start(new ProcessStartInfo(cocosCodeIDEExePath)
			{
				Arguments = "-project " + projectDir,
				UseShellExecute = false
			});
		}

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

		protected override string GetCocosCodeIDEExePath()
		{
			string cocosCodeIDEDirectoryPath = this.GetCocosCodeIDEDirectoryPath();
			if (!string.IsNullOrEmpty(cocosCodeIDEDirectoryPath))
			{
				return Path.Combine(cocosCodeIDEDirectoryPath, "Contents/MacOS/eclipse");
			}
			return null;
		}

		private const string InstallDir_Mac_Cocos = "/Applications/Cocos/Cocos Code IDE.app";

		private const string InstallDir_Mac_Alone = "/Applications/Cocos Code IDE.app";

		private const string InstallDir_Mac_RelativePath = "Contents/MacOS/eclipse";
	}
}
