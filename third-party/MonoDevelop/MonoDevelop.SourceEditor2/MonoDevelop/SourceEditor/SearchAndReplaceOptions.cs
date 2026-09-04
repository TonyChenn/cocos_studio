using System;

namespace MonoDevelop.SourceEditor
{
	internal class SearchAndReplaceOptions
	{
		private static string searchPattern;

		private static string replacePattern;

		public static string SearchPattern
		{
			get
			{
				return searchPattern;
			}
			set
			{
				if (!(searchPattern == value))
				{
					searchPattern = value;
					OnSearchPatternChanged(EventArgs.Empty);
				}
			}
		}

		public static string ReplacePattern
		{
			get
			{
				return replacePattern;
			}
			set
			{
				if (!(replacePattern == value))
				{
					replacePattern = value;
					OnReplacePatternChanged(EventArgs.Empty);
				}
			}
		}

		public static event EventHandler SearchPatternChanged;

		public static event EventHandler ReplacePatternChanged;

		private static void OnSearchPatternChanged(EventArgs e)
		{
			SearchPatternChanged?.Invoke(null, e);
		}

		private static void OnReplacePatternChanged(EventArgs e)
		{
			ReplacePatternChanged?.Invoke(null, e);
		}
	}
}
