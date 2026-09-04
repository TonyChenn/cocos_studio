using MonoDevelop.Core.Instrumentation;

namespace MonoDevelop.SourceEditor
{
	internal class Counters
	{
		public static Counter EditorsInMemory = InstrumentationService.CreateCounter("Editors in Memory", "Text Editor");

		public static Counter SourceViewsInMemory = InstrumentationService.CreateCounter("Source Views in Memory", "Text Editor");

		public static Counter LoadedEditors = InstrumentationService.CreateCounter("Loaded Editors", "Text Editor");

		public static Counter AutoSavedFiles = InstrumentationService.CreateCounter("Autosaved Files", "Text Editor");
	}
}
