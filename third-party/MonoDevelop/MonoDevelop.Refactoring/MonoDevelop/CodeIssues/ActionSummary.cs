using Mono.TextEditor;

namespace MonoDevelop.CodeIssues
{
	public class ActionSummary
	{
		public string Title { get; set; }

		public object SiblingKey { get; set; }

		public bool Batchable { get; set; }

		public DocumentRegion Region { get; set; }

		public IssueSummary IssueSummary { get; set; }

		public override string ToString()
		{
			return $"[ActionSummary: Title={Title}, Batchable={Batchable}, Region={Region}]";
		}
	}
}
