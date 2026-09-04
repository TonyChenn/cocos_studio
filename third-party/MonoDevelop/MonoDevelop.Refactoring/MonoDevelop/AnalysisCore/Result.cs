using System;
using ICSharpCode.NRefactory.Refactoring;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.AnalysisCore.Extensions;

namespace MonoDevelop.AnalysisCore
{
	public class Result
	{
		public virtual bool HasOptionsDialog => false;

		public virtual string OptionsTitle => "";

		public string Message { get; private set; }

		public Severity Level { get; private set; }

		public IssueMarker InspectionMark { get; private set; }

		public DomRegion Region { get; private set; }

		public bool Underline { get; private set; }

		internal AnalysisRuleAddinNode Source { get; set; }

		public Result(DomRegion region, string message, bool underLine = true)
		{
			Region = region;
			Message = message;
			Underline = underLine;
		}

		public Result(DomRegion region, string message, Severity level, IssueMarker inspectionMark, bool underline = true)
		{
			Region = region;
			Message = message;
			Level = level;
			InspectionMark = inspectionMark;
			Underline = underline;
		}

		public void SetSeverity(Severity level, IssueMarker inspectionMark)
		{
			Level = level;
			InspectionMark = inspectionMark;
		}

		public virtual void ShowResultOptionsDialog()
		{
			throw new InvalidOperationException();
		}
	}
}
