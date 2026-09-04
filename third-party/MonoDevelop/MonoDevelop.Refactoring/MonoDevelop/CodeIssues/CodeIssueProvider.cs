using System;
using System.Collections.Generic;

namespace MonoDevelop.CodeIssues
{
	public abstract class CodeIssueProvider : BaseCodeIssueProvider
	{
		private string mimeType;

		public override string MimeType => mimeType;

		public string Category { get; set; }

		public abstract bool HasSubIssues { get; }

		public virtual IEnumerable<BaseCodeIssueProvider> SubIssues
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		public void SetMimeType(string mimeType)
		{
			this.mimeType = mimeType;
			UpdateSeverity();
		}

		public IEnumerable<BaseCodeIssueProvider> GetEffectiveProviderSet()
		{
			if (HasSubIssues)
			{
				return SubIssues;
			}
			return new CodeIssueProvider[1] { this };
		}
	}
}
