using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Content;

namespace MonoDevelop.DesignerSupport.Projects
{
	public class PropertyPadTextEditorExtension : TextEditorExtension, IPropertyPadProvider
	{
		public object GetActiveComponent()
		{
			if (base.Document.HasProject)
			{
				string text = base.Document.FileName;
				return base.Document.Project.Files.GetFile(text);
			}
			return null;
		}

		public object GetProvider()
		{
			return null;
		}

		public void OnEndEditing(object obj)
		{
		}

		public void OnChanged(object obj)
		{
			if (base.Document.HasProject)
			{
				IdeApp.ProjectOperations.Save(base.Document.Project);
			}
		}
	}
}
