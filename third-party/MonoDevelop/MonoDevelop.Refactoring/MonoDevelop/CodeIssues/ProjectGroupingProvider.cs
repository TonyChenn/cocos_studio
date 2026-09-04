using MonoDevelop.Projects;

namespace MonoDevelop.CodeIssues
{
	[GroupingDescription("Project")]
	public class ProjectGroupingProvider : AbstractGroupingProvider<Project>
	{
		protected override Project GetGroupingKey(IssueSummary issue)
		{
			return issue.Project;
		}

		protected override string GetGroupName(IssueSummary issue)
		{
			return issue.Project.Name;
		}
	}
}
