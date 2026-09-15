using System;
using System.IO;
using System.Text.RegularExpressions;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Projects;

namespace Modules.Communal.ProjectSetting
{
	internal class ProjectSettingHelper
	{
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
