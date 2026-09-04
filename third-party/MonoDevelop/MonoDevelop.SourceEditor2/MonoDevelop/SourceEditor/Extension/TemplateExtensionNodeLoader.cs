using Mono.Addins;
using Mono.TextEditor.Highlighting;

namespace MonoDevelop.SourceEditor.Extension
{
	public static class TemplateExtensionNodeLoader
	{
		private static bool initialized;

		public static void Init()
		{
			if (!initialized)
			{
				initialized = true;
				AddinManager.AddExtensionNodeHandler("/MonoDevelop/SourceEditor2/SyntaxModes", OnSyntaxModeExtensionChanged);
				AddinManager.AddExtensionNodeHandler("/MonoDevelop/SourceEditor2/Styles", OnStylesExtensionChanged);
			}
		}

		private static void OnSyntaxModeExtensionChanged(object s, ExtensionNodeEventArgs args)
		{
			TemplateCodon provider = (TemplateCodon)args.ExtensionNode;
			if (args.Change == ExtensionChange.Add)
			{
				Mono.TextEditor.Highlighting.SyntaxModeService.AddSyntaxMode(provider);
			}
			else
			{
				Mono.TextEditor.Highlighting.SyntaxModeService.RemoveSyntaxMode(provider);
			}
		}

		private static void OnStylesExtensionChanged(object s, ExtensionNodeEventArgs args)
		{
			TemplateCodon provider = (TemplateCodon)args.ExtensionNode;
			if (args.Change == ExtensionChange.Add)
			{
				Mono.TextEditor.Highlighting.SyntaxModeService.AddStyle(provider);
			}
			else
			{
				Mono.TextEditor.Highlighting.SyntaxModeService.RemoveStyle(provider);
			}
		}
	}
}
