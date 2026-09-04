using Mono.Addins;
using Mono.TextEditor.Highlighting;

namespace MonoDevelop.SourceEditor
{
	[ExtensionNode(Description = "A syntax mode. The specified class must be a valid syntax mode.")]
	internal class SyntaxModeCodon : TypeExtensionNode
	{
		[NodeAttribute("mimeTypes", true, "Mime types of the syntax mode.")]
		private string mimeTypes;

		public SyntaxMode SyntaxMode => CreateInstance() as SyntaxMode;

		public string MimeTypes => mimeTypes;
	}
}
