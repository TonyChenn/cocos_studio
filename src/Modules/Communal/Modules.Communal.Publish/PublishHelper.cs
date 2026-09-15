using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gtk;
using Microsoft.Win32;
using Modules.Communal.CocosAdapter;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace Modules.Communal.Publish
{
	internal class PublishHelper
	{
		static PublishHelper()
		{
			for (int i = 15; i >= 10; i--)
			{
				string item = "Software\\Microsoft\\VisualStudio\\" + i + ".0_Config";
				PublishHelper.VsRegistKeys.Add(item);
			}
		}

		internal static bool HasSolution()
		{
			return Services.ProjectOperations.CurrentSelectedSolution != null;
		}

		internal static bool CanPublishToCocosCodeIDE()
		{
			if (Services.ProjectOperations.CurrentSelectedSolution == null)
			{
				return false;
			}
			CocosProperties cocosProperties = Cocos2dxServices.CocosProperties;
			return cocosProperties.SolutionCodeType != EnumSolutionCodeType.Complete || cocosProperties.ProgramLanguage != EnumProgramLanguage.cpp;
		}

		internal static bool CanPublishToVS()
		{
			return Services.ProjectOperations.CurrentSelectedSolution != null && Platform.IsWindows;
		}

		internal static bool CanPublishToXcode()
		{
			return Services.ProjectOperations.CurrentSelectedSolution != null && Platform.IsMac;
		}

		internal static void OpenProjectWithVisualStudio()
		{
			Solution currentSolution = Services.ProjectsService.CurrentSolution;
			if (currentSolution == null)
			{
				return;
			}
			string winProjectDir = PublishHelper.GetWinProjectDir(currentSolution);
			if (string.IsNullOrEmpty(winProjectDir))
			{
				MessageBox.Show(LanguageInfo.MessageBox184_GetPrjPathError, MessageBoxImage.Other, null, null);
				return;
			}
			if (!File.Exists(winProjectDir))
			{
				MessageBox.Show(string.Format(LanguageInfo.MessageBox217_CodeProjectNotFound, winProjectDir), MessageBoxImage.Other, null, null);
				return;
			}
			bool flag = false;
			try
			{
				RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(".sln");
				if (registryKey != null)
				{
					string text = registryKey.GetValue("") as string;
					text.Equals("VisualStudio.Launcher.sln");
					flag = true;
				}
			}
			catch
			{
				flag = false;
			}
			try
			{
				if (flag)
				{
					MessageBoxResult messageBoxResult = MessageBox.Show(LanguageInfo.MessageBox196_AskOpenWithVS, MessageBoxButton.YesNo, MessageBoxImage.Other, null, EnumMainButton.Yes, null);
					if (messageBoxResult == MessageBoxResult.Yes)
					{
						Process.Start(winProjectDir);
					}
				}
				else
				{
					string text2 = null;
					foreach (string name in PublishHelper.VsRegistKeys)
					{
						using (RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey(name))
						{
							if (registryKey2 != null)
							{
								text2 = (registryKey2.GetValue("InstallDir") as string);
								break;
							}
						}
					}
					if (string.IsNullOrEmpty(text2))
					{
						MessageBoxResult messageBoxResult2 = MessageBox.Show(LanguageInfo.MessageBox186_VSNotInstalled, MessageBoxButton.YesNo, MessageBoxImage.Other, null, EnumMainButton.Yes, null);
						if (messageBoxResult2 == MessageBoxResult.Yes)
						{
							try
							{
								using (Process.Start("http://www.visualstudio.com/"))
								{
								}
							}
							catch (Exception exception)
							{
								LogConfig.Output.Error(LanguageInfo.MessageBox182_FailedToOpenWeb, exception);
							}
						}
					}
					else
					{
						text2 = Path.Combine(text2, "devenv.exe");
						MessageBoxResult messageBoxResult3 = MessageBox.Show(LanguageInfo.MessageBox196_AskOpenWithVS, MessageBoxButton.YesNo, MessageBoxImage.Other, null, EnumMainButton.Yes, null);
						if (messageBoxResult3 == MessageBoxResult.Yes)
						{
							new Process
							{
								StartInfo = new ProcessStartInfo
								{
									Arguments = winProjectDir,
									FileName = text2
								}
							}.Start();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(LanguageInfo.MessageBox187_OpenWithVSError, MessageBoxImage.Other, null, null);
				LogConfig.Logger.Error(LanguageInfo.MessageBox187_OpenWithVSError + "\r\n" + ex.ToString());
			}
		}

		internal static void OpenProjectWithXcode()
		{
			Solution currentSolution = Services.ProjectsService.CurrentSolution;
			if (currentSolution == null)
			{
				return;
			}
			string macProjectDir = PublishHelper.GetMacProjectDir(currentSolution);
			if (string.IsNullOrEmpty(macProjectDir))
			{
				MessageBox.Show(LanguageInfo.MessageBox184_GetPrjPathError, MessageBoxImage.Other, null, null);
				return;
			}
			if (!Directory.Exists(macProjectDir))
			{
				MessageBox.Show(string.Format(LanguageInfo.MessageBox217_CodeProjectNotFound, macProjectDir), MessageBoxImage.Other, null, null);
				return;
			}
			if (!Directory.Exists("/Applications/Xcode.app"))
			{
				MessageBoxResult messageBoxResult = MessageBox.Show(LanguageInfo.MessageBox183_XcodeNotInstalled, MessageBoxButton.YesNo, MessageBoxImage.Other, null, EnumMainButton.Yes, null);
				if (messageBoxResult == MessageBoxResult.Yes)
				{
					try
					{
						using (Process.Start("https://developer.apple.com/cn/xcode/"))
						{
						}
					}
					catch (Exception exception)
					{
						LogConfig.Output.Error(LanguageInfo.MessageBox182_FailedToOpenWeb, exception);
					}
				}
				return;
			}
			try
			{
				Process.Start(macProjectDir);
			}
			catch
			{
				LogConfig.Output.Error(LanguageInfo.MessageBox185_OpenWithXcodeError);
			}
		}

		private static string GetWinProjectDir(Solution sln)
		{
			string directoryName = Path.GetDirectoryName(sln.ItemDirectory);
			string name = sln.Name;
			switch (Cocos2dxServices.CocosProperties.ProgramLanguage)
			{
			case EnumProgramLanguage.cpp:
				return Path.Combine(directoryName, "proj.win32", name + ".sln");
			case EnumProgramLanguage.lua:
			case EnumProgramLanguage.js:
				return Path.Combine(new string[]
				{
					directoryName,
					"frameworks",
					"runtime-src",
					"proj.win32",
					name + ".sln"
				});
			default:
				return null;
			}
		}

		private static string GetMacProjectDir(Solution sln)
		{
			string directoryName = Path.GetDirectoryName(sln.ItemDirectory);
			string name = sln.Name;
			switch (Cocos2dxServices.CocosProperties.ProgramLanguage)
			{
			case EnumProgramLanguage.cpp:
				return Path.Combine(directoryName, "proj.ios_mac", name + ".xcodeproj");
			case EnumProgramLanguage.lua:
			case EnumProgramLanguage.js:
				return Path.Combine(new string[]
				{
					directoryName,
					"frameworks",
					"runtime-src",
					"proj.ios_mac",
					name + ".xcodeproj"
				});
			default:
				return null;
			}
		}

		private const string Uri_Xcode = "https://developer.apple.com/cn/xcode/";

		private const string InstallDir_Xcode = "/Applications/Xcode.app";

		private const string Uri_VisualStudio = "http://www.visualstudio.com/";

		private static List<string> VsRegistKeys = new List<string>();
	}
}
