using MonoDevelop.Ide.Gui;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	public interface ITextToolboxNode
	{
		string GetDragPreview(Document document);

		bool IsCompatibleWith(Document document);

		void InsertAtCaret(Document document);
	}
}
