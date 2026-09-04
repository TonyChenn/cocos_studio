using MonoDevelop.SourceEditor;

namespace CocoStudio.SourceEditor
{
	internal class SourceEditorService
	{
		private static bool isInitialized;

		public static void Initialize()
		{
			if (!isInitialized)
			{
				isInitialized = true;
				AutoSave.DisableAutoSave();
			}
		}
	}
}
