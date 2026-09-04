using System;
using System.Collections.Generic;
using System.Threading;
using ICSharpCode.NRefactory.Refactoring;
using Mono.TextEditor;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.CodeIssues
{
	public abstract class BaseCodeIssueProvider
	{
		protected Severity severity;

		protected bool isEnabled;

		public virtual CodeIssueProvider Parent => null;

		public abstract string MimeType { get; }

		public string Title { get; set; }

		public string Description { get; set; }

		public virtual string IdString => "refactoring.codeissues." + MimeType + "." + GetType().FullName;

		public Severity DefaultSeverity { get; set; }

		public bool IsEnabledByDefault { get; set; }

		public virtual bool CanDisableOnce => false;

		public virtual bool CanDisableAndRestore => false;

		public virtual bool CanDisableWithPragma => false;

		public virtual bool CanSuppressWithAttribute => false;

		public Severity GetSeverity()
		{
			return severity;
		}

		public void SetSeverity(Severity severity)
		{
			if (this.severity != severity)
			{
				this.severity = severity;
				PropertyService.Set(IdString, severity);
			}
		}

		public bool GetIsEnabled()
		{
			return isEnabled;
		}

		public void SetIsEnabled(bool isEnabled)
		{
			if (this.isEnabled != isEnabled)
			{
				this.isEnabled = isEnabled;
				PropertyService.Set(IdString + ".isEnabled", isEnabled);
			}
		}

		protected void UpdateSeverity()
		{
			severity = PropertyService.Get(IdString, DefaultSeverity);
			isEnabled = PropertyService.Get(IdString + ".isEnabled", IsEnabledByDefault);
		}

		public abstract IEnumerable<CodeIssue> GetIssues(object refactoringContext, CancellationToken cancellationToken);

		public virtual void DisableOnce(Document document, DocumentRegion loc)
		{
			throw new NotSupportedException();
		}

		public virtual void DisableAndRestore(Document document, DocumentRegion loc)
		{
			throw new NotSupportedException();
		}

		public virtual void DisableWithPragma(Document document, DocumentRegion loc)
		{
			throw new NotSupportedException();
		}

		public virtual void SuppressWithAttribute(Document document, DocumentRegion loc)
		{
			throw new NotSupportedException();
		}
	}
}
