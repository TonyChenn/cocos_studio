using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.NRefactory.Refactoring;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.CodeActions;
using MonoDevelop.Projects;

namespace MonoDevelop.CodeIssues
{
	public class IssueSummary : IIssueTreeNode
	{
		private static readonly ICollection<IIssueTreeNode> emptyCollection = new IIssueTreeNode[0];

		private bool visible = true;

		private IList<ActionSummary> actions;

		string IIssueTreeNode.Text
		{
			get
			{
				string arg = ((Region.BeginLine != Region.EndLine) ? $"{Region.BeginLine}-{Region.EndLine}" : Region.BeginLine.ToString());
				string fileName = Path.GetFileName(File.Name);
				return $"{IssueDescription} [{fileName}:{arg}]";
			}
		}

		ICollection<IIssueTreeNode> IIssueTreeNode.Children => emptyCollection;

		bool IIssueTreeNode.HasVisibleChildren => false;

		bool IIssueTreeNode.Visible
		{
			get
			{
				return visible;
			}
			set
			{
				if (visible != value)
				{
					visible = value;
					OnVisibleChanged(new IssueGroupEventArgs(this));
				}
			}
		}

		ICollection<IIssueTreeNode> IIssueTreeNode.AllChildren => emptyCollection;

		public string IssueDescription { get; set; }

		public DomRegion Region { get; set; }

		public string ProviderCategory { get; set; }

		public string ProviderTitle { get; set; }

		public string ProviderDescription { get; set; }

		public Severity Severity { get; set; }

		public IssueMarker IssueMarker { get; set; }

		public ProjectFile File { get; set; }

		public Project Project { get; set; }

		public string InspectorIdString { get; set; }

		public IList<ActionSummary> Actions
		{
			get
			{
				if (actions == null)
				{
					Actions = new List<ActionSummary>();
				}
				return actions;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				actions = value;
			}
		}

		private event EventHandler<IssueGroupEventArgs> visibleChanged;

		event EventHandler<IssueGroupEventArgs> IIssueTreeNode.VisibleChanged
		{
			add
			{
				visibleChanged += value;
			}
			remove
			{
				visibleChanged -= value;
			}
		}

		event EventHandler<IssueGroupEventArgs> IIssueTreeNode.ChildrenInvalidated
		{
			add
			{
			}
			remove
			{
			}
		}

		event EventHandler<IssueTreeNodeEventArgs> IIssueTreeNode.ChildAdded
		{
			add
			{
			}
			remove
			{
			}
		}

		event EventHandler<IssueGroupEventArgs> IIssueTreeNode.TextChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		public static IssueSummary FromCodeIssue(ProjectFile file, BaseCodeIssueProvider provider, CodeIssue codeIssue)
		{
			CodeIssueProvider codeIssueProvider = (provider as CodeIssueProvider) ?? provider.Parent;
			if (codeIssueProvider == null)
			{
				throw new ArgumentException("must be a CodeIssueProvider or a BaseCodeIssueProvider with Parent != null", "provider");
			}
			IssueSummary issueSummary = new IssueSummary
			{
				IssueDescription = codeIssue.Description,
				Region = codeIssue.Region,
				ProviderTitle = codeIssueProvider.Title,
				ProviderDescription = codeIssueProvider.Description,
				ProviderCategory = codeIssueProvider.Category,
				Severity = codeIssueProvider.GetSeverity(),
				IssueMarker = codeIssue.IssueMarker,
				File = file,
				Project = file.Project,
				InspectorIdString = codeIssue.InspectorIdString
			};
			issueSummary.Actions = codeIssue.Actions.Select((CodeAction a) => new ActionSummary
			{
				Batchable = a.SupportsBatchRunning,
				SiblingKey = a.SiblingKey,
				Title = a.Title,
				Region = a.DocumentRegion,
				IssueSummary = issueSummary
			}).ToList();
			return issueSummary;
		}

		protected virtual void OnVisibleChanged(IssueGroupEventArgs eventArgs)
		{
			visibleChanged?.Invoke(this, eventArgs);
		}

		public override string ToString()
		{
			return string.Format("[IssueSummary: ProviderTitle={2}, Region={0}, ProviderCategory={1}]", Region, ProviderCategory, ProviderTitle);
		}
	}
}
