using Mono.TextEditor;
using MonoDevelop.Ide.FindInFiles;

namespace MonoDevelop.SourceEditor.QuickTasks
{
	public struct Usage
	{
		public DocumentLocation Location;

		public ReferenceUsageType UsageType;

		public Usage(DocumentLocation location, ReferenceUsageType usageType)
		{
			Location = location;
			UsageType = usageType;
		}
	}
}
