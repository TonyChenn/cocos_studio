using Mono.TextEditor;

namespace MonoDevelop.SourceEditor
{
	public class TabAction
	{
		private ExtensibleTextEditor editor;

		public TabAction(ExtensibleTextEditor editor)
		{
			this.editor = editor;
		}

		public void Action(TextEditorData data)
		{
			if (!editor.DoInsertTemplate())
			{
				MiscActions.InsertTab(data);
			}
		}
	}
}
