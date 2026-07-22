using System;
using System.IO;
using System.Text.RegularExpressions;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Projects;

namespace Modules.Communal.ProjectSetting
{
	// Token: 0x02000004 RID: 4
	internal class ProjectSettingHelper
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002050 File Offset: 0x00000250
		public static string GetUnifiedPath(string newPath, string basePath)
		{
			if (!newPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				newPath += Path.DirectorySeparatorChar;
			}
			if (!basePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				basePath += Path.DirectorySeparatorChar;
			}
			Uri uri = new Uri(newPath);
			Uri uri2 = new Uri(basePath);
			Uri uri3 = uri2.MakeRelativeUri(uri);
			string result;
			if (uri3.ToString().StartsWith(".."))
			{
				result = Uri.UnescapeDataString(uri.AbsolutePath);
			}
			else
			{
				result = Uri.UnescapeDataString(uri3.ToString());
			}
			return result;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020F8 File Offset: 0x000002F8
		public static bool CheckPathValidity(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return true;
			}
			string input = path.Replace("\\", "").Replace("/", "").Replace(":", "");
			if (!Regex.IsMatch(input, "^[A-Za-z0-9,._-]+$"))
			{
				return false;
			}
			string dir;
			if (Path.IsPathRooted(path))
			{
				dir = path;
			}
			else
			{
				Solution currentSelectedSolution = Services.ProjectOperations.CurrentSelectedSolution;
				dir = Path.Combine(currentSelectedSolution.BaseDirectory, path);
			}
			return Option.CheckIsWritableDir(dir);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000217C File Offset: 0x0000037C
		public static string ConvertToAbsolutePath(string path)
		{
			Solution currentSelectedSolution = Services.ProjectOperations.CurrentSelectedSolution;
			if (currentSelectedSolution == null)
			{
				return string.Empty;
			}
			if (string.IsNullOrEmpty(path))
			{
				return currentSelectedSolution.BaseDirectory;
			}
			if (Path.IsPathRooted(path))
			{
				return path;
			}
			return Path.Combine(currentSelectedSolution.BaseDirectory, path);
		}
	}
}
