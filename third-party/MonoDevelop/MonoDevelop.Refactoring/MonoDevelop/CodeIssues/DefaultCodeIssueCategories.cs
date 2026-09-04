using MonoDevelop.Core;

namespace MonoDevelop.CodeIssues
{
	public static class DefaultCodeIssueCategories
	{
		public static readonly string Improvements = GettextCatalog.GetString("Code Improvements");

		public static readonly string CodeQualityIssues = GettextCatalog.GetString("Code Quality Issues");

		public static readonly string ConstraintViolations = GettextCatalog.GetString("Constraint Violations");

		public static readonly string Redundancies = GettextCatalog.GetString("Redundancies");

		public static readonly string Opportunities = GettextCatalog.GetString("Language Usage Opportunities");

		public static readonly string Notifications = GettextCatalog.GetString("Code Notifications");
	}
}
