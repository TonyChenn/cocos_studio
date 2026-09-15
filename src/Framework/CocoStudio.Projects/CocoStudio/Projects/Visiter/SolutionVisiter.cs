using System;
using CocoStudio.Model;
using Gdk;

namespace CocoStudio.Projects.Visiter
{
	public static class SolutionVisiter
	{
		public static SizeF GetSceneSize(this Solution solution)
		{
			string solutionSize = solution.Config.SolutionSize;
			if (!string.IsNullOrWhiteSpace(solutionSize))
			{
				return solutionSize.ConvertToSize();
			}
			return SizeF.Empty;
		}

		public static string GetSceneSizeString(this Solution solution)
		{
			return solution.Config.SolutionSize;
		}

		public static void SetSceneSize(this Solution solution, Size size)
		{
			solution.Config.SolutionSize = SolutionVisiter.ConvertToString(size);
		}

		public static void SetSceneSize(this Solution solution, string size)
		{
			solution.Config.SolutionSize = size;
		}

		private static SizeF ConvertToSize(this string sizeStr)
		{
			string[] array = sizeStr.Split(new char[]
			{
				'*'
			});
			float width = Convert.ToSingle(array[0]);
			float height = Convert.ToSingle(array[1]);
			return new SizeF(width, height);
		}

		private static string ConvertToString(Size size)
		{
			return string.Format("{0}*{1}", size.Width, size.Height);
		}

		public static string GetResolutionName(this Solution solution)
		{
			return solution.Config.ResolutionName;
		}

		public static void SetResolutionName(this Solution solution, string name)
		{
			solution.Config.ResolutionName = name;
		}
	}
}
