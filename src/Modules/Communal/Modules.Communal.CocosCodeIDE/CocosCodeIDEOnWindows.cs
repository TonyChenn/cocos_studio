using System;
using System.Diagnostics;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using Microsoft.Win32;
using MonoDevelop.Core;

namespace Modules.Communal.CocosCodeIDE
{
	public class CocosCodeIDEOnWindows : CocosCodeIDEService
	{
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

		private const string Cocos_DIR_HKEY = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Cocos";

		private const string CocosCodeIDE_DIR_HKEY = "CocosCodeIDEDir";

		private const string CocosCodeIDE_EXE = "Cocos Code IDE.exe";
	}
}
