using MonoDevelop.Core;
using MonoDevelop.SourceEditor.QuickTasks;

namespace MonoDevelop.AnalysisCore
{
	public static class AnalysisOptions
	{
		public static readonly PropertyWrapper<bool> EnableUnitTestEditorIntegration = new PropertyWrapper<bool>("Testing.EnableUnitTestEditorIntegration", defaultValue: false);

		public static PropertyWrapper<bool> AnalysisEnabled => QuickTaskStrip.EnableFancyFeatures;
	}
}
