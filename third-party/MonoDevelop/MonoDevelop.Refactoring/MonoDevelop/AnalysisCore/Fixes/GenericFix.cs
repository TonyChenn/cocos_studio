using System;
using Mono.TextEditor;

namespace MonoDevelop.AnalysisCore.Fixes
{
	public class GenericFix : IAnalysisFix, IAnalysisFixAction
	{
		private Action fix;

		private Action batchFix;

		private string label;

		public DocumentRegion DocumentRegion { get; set; }

		public string IdString { get; set; }

		public string FixType => "Generic";

		public bool SupportsBatchFix => batchFix != null;

		public string Label => label;

		public GenericFix(string label, Action fix, Action batchFix = null)
		{
			this.batchFix = batchFix;
			this.fix = fix;
			this.label = label;
		}

		public void Fix()
		{
			fix();
		}

		public void BatchFix()
		{
			if (!SupportsBatchFix)
			{
				throw new InvalidOperationException("Batch fixing is not supported.");
			}
			batchFix();
		}
	}
}
