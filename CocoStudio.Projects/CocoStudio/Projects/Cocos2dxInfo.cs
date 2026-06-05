using System;
using System.IO;
using CocoStudio.Basic;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	// Token: 0x0200003B RID: 59
	public class Cocos2dxInfo
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000165 RID: 357 RVA: 0x000060F2 File Offset: 0x000042F2
		// (set) Token: 0x06000166 RID: 358 RVA: 0x000060FA File Offset: 0x000042FA
		public string RootPath { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00006103 File Offset: 0x00004303
		// (set) Token: 0x06000168 RID: 360 RVA: 0x0000610B File Offset: 0x0000430B
		public string VersionText { get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00006114 File Offset: 0x00004314
		// (set) Token: 0x0600016A RID: 362 RVA: 0x0000611C File Offset: 0x0000431C
		[Obsolete("现在编辑器已经不会再使用2.x版本的引擎创建项目，因此该属性也不再使用")]
		public int MainVersion { get; private set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00006125 File Offset: 0x00004325
		// (set) Token: 0x0600016C RID: 364 RVA: 0x0000612D File Offset: 0x0000432D
		public bool EnableCpp { get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00006136 File Offset: 0x00004336
		// (set) Token: 0x0600016E RID: 366 RVA: 0x0000613E File Offset: 0x0000433E
		public bool EnableLua { get; private set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00006147 File Offset: 0x00004347
		// (set) Token: 0x06000170 RID: 368 RVA: 0x0000614F File Offset: 0x0000434F
		public bool EnableJs { get; private set; }

		// Token: 0x06000171 RID: 369 RVA: 0x00006158 File Offset: 0x00004358
		private Cocos2dxInfo()
		{
			this.RootPath = "";
			this.VersionText = "";
			this.MainVersion = 3;
			this.EnableCpp = false;
			this.EnableLua = false;
			this.EnableJs = false;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00006194 File Offset: 0x00004394
		public static Cocos2dxInfo GetSimplifiedConsole()
		{
			Cocos2dxInfo cocos2dxInfo = new Cocos2dxInfo();
			if (Platform.IsWindows)
			{
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
				cocos2dxInfo.RootPath = Path.Combine(folderPath, "Cocos", "Cocos2dxConsole");
			}
			else
			{
				cocos2dxInfo.RootPath = Path.Combine(Option.AssemblyDir, "Cocos2dxConsole");
			}
			if (!Directory.Exists(cocos2dxInfo.RootPath))
			{
				return null;
			}
			cocos2dxInfo.VersionText = "IDE cocos";
			cocos2dxInfo.EnableLua = true;
			cocos2dxInfo.EnableCpp = false;
			cocos2dxInfo.EnableJs = true;
			return cocos2dxInfo;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00006214 File Offset: 0x00004414
		public static Cocos2dxInfo GetFramework(string version)
		{
			if ("IDE cocos".Equals(version))
			{
				return Cocos2dxInfo.GetSimplifiedConsole();
			}
			string text = Cocos2dxInfo.TryGetFrameworkPath(version);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			Cocos2dxInfo result;
			if (Cocos2dxInfo.TryCreateDefaultCodeInfo(text, version, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00006254 File Offset: 0x00004454
		private static string TryGetFrameworkPath(string frameworkName)
		{
			string frameworkBaseDirectory = FrameworkHelper.FrameworkBaseDirectory;
			if (string.IsNullOrEmpty(frameworkBaseDirectory))
			{
				return string.Empty;
			}
			string text = Path.Combine(frameworkBaseDirectory, frameworkName);
			if (Directory.Exists(text))
			{
				return text;
			}
			return string.Empty;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000628C File Offset: 0x0000448C
		private static bool TryCreateCodeInfoV2(string path, out Cocos2dxInfo codeInfo)
		{
			string text = Path.Combine(path, "cocos2dx", "cocos2d.cpp");
			if (File.Exists(text))
			{
				codeInfo = new Cocos2dxInfo
				{
					RootPath = path,
					MainVersion = 2,
					EnableCpp = true,
					EnableLua = true,
					EnableJs = true,
					VersionText = Cocos2dxInfo.GetVersionText(text)
				};
				return true;
			}
			codeInfo = null;
			return false;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000062F0 File Offset: 0x000044F0
		private static bool TryCreateCodeInfoV3(string path, out Cocos2dxInfo codeInfo)
		{
			string text = Path.Combine(path, "cocos", "cocos2d.cpp");
			if (File.Exists(text))
			{
				codeInfo = new Cocos2dxInfo
				{
					RootPath = path,
					MainVersion = 3,
					EnableCpp = true,
					EnableLua = true,
					EnableJs = false,
					VersionText = Cocos2dxInfo.GetVersionText(text)
				};
				return true;
			}
			codeInfo = null;
			return false;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00006354 File Offset: 0x00004554
		private static bool TryCreateCodeInfoV3_JS(string path, out Cocos2dxInfo codeInfo)
		{
			string text = Path.Combine(new string[]
			{
				path,
				"frameworks",
				"js-bindings",
				"cocos2d-x",
				"cocos",
				"cocos2d.cpp"
			});
			if (File.Exists(text))
			{
				codeInfo = new Cocos2dxInfo
				{
					RootPath = path,
					MainVersion = 3,
					EnableCpp = false,
					EnableLua = false,
					EnableJs = true,
					VersionText = Cocos2dxInfo.GetVersionText(text)
				};
				return true;
			}
			codeInfo = null;
			return false;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000063E4 File Offset: 0x000045E4
		private static bool TryCreateCodeInfoWithoutVersion(string path, out Cocos2dxInfo codeInfo)
		{
			string path2 = Path.Combine(path, "templates", "cocos2dx_files.json");
			if (File.Exists(path2))
			{
				codeInfo = new Cocos2dxInfo
				{
					RootPath = path,
					MainVersion = 3,
					EnableCpp = true,
					EnableLua = true,
					EnableJs = false,
					VersionText = "Cocos2d-x 3.0"
				};
				return true;
			}
			codeInfo = null;
			return false;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00006448 File Offset: 0x00004648
		private static bool TryCreateDefaultCodeInfo(string path, string version, out Cocos2dxInfo codeInfo)
		{
			bool result;
			try
			{
				string path2 = Path.Combine(path, "version");
				if (File.Exists(path2))
				{
					Cocos2dxInfo cocos2dxInfo = new Cocos2dxInfo();
					cocos2dxInfo.RootPath = path;
					cocos2dxInfo.MainVersion = 3;
					cocos2dxInfo.EnableCpp = true;
					cocos2dxInfo.EnableLua = true;
					cocos2dxInfo.EnableJs = false;
					if (string.IsNullOrWhiteSpace(version))
					{
						StreamReader streamReader = new StreamReader(path2);
						cocos2dxInfo.VersionText = streamReader.ReadLine();
					}
					else
					{
						cocos2dxInfo.VersionText = version;
					}
					codeInfo = cocos2dxInfo;
					result = true;
				}
				else
				{
					codeInfo = null;
					result = false;
				}
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error("获取默认引擎信息失败：\r\n" + ex.ToString());
				codeInfo = null;
				result = false;
			}
			return result;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000064FC File Offset: 0x000046FC
		internal static string GetVersionText(string cppFilePath)
		{
			string result;
			try
			{
				StreamReader streamReader = new StreamReader(cppFilePath);
				string value = "cocos2dVersion";
				bool flag = false;
				for (string text = streamReader.ReadLine(); text != null; text = streamReader.ReadLine())
				{
					if (text.Contains(value))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					result = null;
				}
				else
				{
					string text = streamReader.ReadLine();
					text = streamReader.ReadLine();
					string[] array = text.Split(new char[]
					{
						'"'
					});
					if (string.IsNullOrEmpty(array[1]))
					{
						result = "cocos2d-x";
					}
					else
					{
						result = array[1];
					}
				}
			}
			catch
			{
				result = "cocos2d-x";
			}
			return result;
		}

		// Token: 0x04000057 RID: 87
		public const string IdeOnlyCocos = "IDE cocos";

		// Token: 0x04000058 RID: 88
		public const string CppVersionFileName = "cocos2d.cpp";

		// Token: 0x04000059 RID: 89
		public const string DefaultVersionText = "cocos2d-x";

		// Token: 0x0400005A RID: 90
		private const string Cocos2_folder = "cocos2dx";

		// Token: 0x0400005B RID: 91
		private const string Cocos3_folder = "cocos";

		// Token: 0x0400005C RID: 92
		private const string FileListFile = "cocos2dx_files.json";

		// Token: 0x0400005D RID: 93
		private const string consoleFolderName = "Cocos2dxConsole";
	}
}
