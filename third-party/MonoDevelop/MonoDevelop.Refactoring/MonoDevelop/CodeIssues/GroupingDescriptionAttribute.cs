using System;

namespace MonoDevelop.CodeIssues
{
	public class GroupingDescriptionAttribute : Attribute
	{
		public string Title { get; private set; }

		public GroupingDescriptionAttribute(string title)
		{
			Title = title;
		}
	}
}
