using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Refactoring;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.TextEditor;
using MonoDevelop.CodeActions;

namespace MonoDevelop.CodeIssues
{
	public class CodeIssue
	{
		public string Description { get; private set; }

		public DomRegion Region { get; private set; }

		public IssueMarker IssueMarker { get; private set; }

		public IEnumerable<CodeAction> Actions { get; private set; }

		public string InspectorIdString { get; private set; }

		public IList<Type> ActionProvider { get; set; }

		public CodeIssue(IssueMarker issueMarker, string description, string fileName, DocumentLocation start, DocumentLocation end, string inspectorIdString, IEnumerable<CodeAction> actions = null)
			: this(issueMarker, description, new DomRegion(fileName, start, end), inspectorIdString, actions)
		{
		}

		public CodeIssue(IssueMarker issueMarker, string description, DomRegion region, string inspectorIdString, IEnumerable<CodeAction> actions = null)
		{
			IssueMarker = issueMarker;
			Description = description;
			Region = region;
			Actions = actions;
			InspectorIdString = inspectorIdString;
		}
	}
}
