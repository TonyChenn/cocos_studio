using Mono.Addins;
using Mono.TextEditor;
using Mono.TextEditor.Highlighting;

namespace MonoDevelop.SourceEditor
{
	public static class SyntaxModeService
	{
		static SyntaxModeService()
		{
			AddinManager.AddExtensionNodeHandler("/MonoDevelop/SourceEditor2/CustomModes", delegate(object sender, ExtensionNodeEventArgs args)
			{
				SyntaxModeCodon syntaxModeCodon = (SyntaxModeCodon)args.ExtensionNode;
				if (args.Change == ExtensionChange.Add)
				{
					Mono.TextEditor.Highlighting.SyntaxModeService.InstallSyntaxMode(syntaxModeCodon.MimeTypes, new SyntaxModeProvider(delegate(TextDocument d)
					{
						SyntaxMode syntaxMode = syntaxModeCodon.SyntaxMode;
						syntaxMode.Document = d;
						return syntaxMode;
					}));
				}
			});
		}

		public static void EnsureLoad()
		{
		}
	}
}
