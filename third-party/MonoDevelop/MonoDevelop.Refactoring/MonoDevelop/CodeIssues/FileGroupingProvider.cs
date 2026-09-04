using MonoDevelop.Projects;

namespace MonoDevelop.CodeIssues
{
	[GroupingDescription("File")]
	public class FileGroupingProvider : AbstractGroupingProvider<ProjectFile>
	{
		protected override ProjectFile GetGroupingKey(IssueSummary issue)
		{
			return issue.File;
		}

		protected override string GetGroupName(IssueSummary issue)
		{
			return issue.File.FilePath.ToRelative(issue.Project.BaseDirectory);
		}
	}
}
