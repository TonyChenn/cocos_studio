using Mono.TextEditor;

namespace MonoDevelop.AnalysisCore
{
	public interface IAnalysisFixAction
	{
		string Label { get; }

		bool SupportsBatchFix { get; }

		DocumentRegion DocumentRegion { get; }

		string IdString { get; }

		void Fix();

		void BatchFix();
	}
}
