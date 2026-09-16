using System;
using System.IO;
using CocoStudio.Basic;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	public class Cocos2dxInfo
	{
		public string RootPath { get; private set; }

		public string VersionText { get; private set; }

		[Obsolete("现在编辑器已经不会再使用2.x版本的引擎创建项目，因此该属性也不再使用")]
		public int MainVersion { get; private set; }

		public bool EnableCpp { get; private set; }

		public bool EnableLua { get; private set; }

		public bool EnableJs { get; private set; }

		private Cocos2dxInfo()
		{
			this.RootPath = "";
			this.VersionText = "";
			this.MainVersion = 3;
			this.EnableCpp = false;
			this.EnableLua = false;
			this.EnableJs = false;
		}

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
            if (Cocos2dxInfo.TryCreateDefaultCodeInfo(text, version, out Cocos2dxInfo result))
            {
                return result;
            }
            return null;
		}

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

		public const string IdeOnlyCocos = "IDE cocos";

		public const string CppVersionFileName = "cocos2d.cpp";

		public const string DefaultVersionText = "cocos2d-x";

		private const string Cocos2_folder = "cocos2dx";

		private const string Cocos3_folder = "cocos";

		private const string FileListFile = "cocos2dx_files.json";

		private const string consoleFolderName = "Cocos2dxConsole";
	}
}
