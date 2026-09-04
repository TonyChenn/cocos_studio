using ICSharpCode.NRefactory.Refactoring;
using Mono.Addins;

namespace MonoDevelop.CodeIssues
{
	public class CodeIssueAddinNode : TypeExtensionNode
	{
		[NodeAttribute("mimeType", Required = true, Description = "The mime type of this action.")]
		private string mimeType;

		[NodeAttribute("severity", Required = true, Localizable = false, Description = "The severity of this action.")]
		private Severity severity;

		private CodeIssueProvider inspector;

		public string MimeType => mimeType;

		public Severity Severity => severity;

		public CodeIssueProvider Inspector
		{
			get
			{
				if (inspector == null)
				{
					inspector = (CodeIssueProvider)CreateInstance();
					inspector.DefaultSeverity = severity;
					inspector.SetMimeType(MimeType);
				}
				return inspector;
			}
		}
	}
}
