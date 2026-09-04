using System;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.View;
using CocoStudio.Projects;
using Mono.Addins;
using Mono.TextEditor.Highlighting;
using MonoDevelop.Core;
using MonoDevelop.SourceEditor;
using MonoDevelop.SourceEditor.Extension;

namespace CocoStudio.SourceEditor
{
	[Extension(Path = "CocoStudio/Ide/DisplayBuilder")]
	public class SourceEditorDisplayBinding : IViewDisplayBuilder, IDisplayBuilder
	{
		private static bool IsInitialized;

		public static FilePath SyntaxModePath => UserProfile.Current.UserDataRoot.Combine("HighlightingSchemes");

		public string Name => GettextCatalog.GetString("Source Code Editor");

		public bool CanUseAsDefault => true;

		static SourceEditorDisplayBinding()
		{
			InitSourceEditor();
		}

		public static void InitSourceEditor()
		{
			if (!IsInitialized)
			{
				IsInitialized = true;
				TemplateExtensionNodeLoader.Init();
				DefaultSourceEditorOptions.Init();
				MonoDevelop.SourceEditor.SyntaxModeService.EnsureLoad();
				LoadCustomStylesAndModes();
				SourceEditorService.Initialize();
			}
		}

		internal static void LoadCustomStylesAndModes()
		{
			bool flag = true;
			if (!Directory.Exists(SyntaxModePath))
			{
				try
				{
					Directory.CreateDirectory(SyntaxModePath);
				}
				catch (Exception exception)
				{
					flag = false;
					LogConfig.Logger.Error("Can't create syntax mode directory", exception);
				}
			}
			if (flag)
			{
				Mono.TextEditor.Highlighting.SyntaxModeService.LoadStylesAndModes(SyntaxModePath);
			}
		}

		public IViewContentExtend CreateContent(FilePath fileName, string mimeType, CocosItem ownerProject)
		{
			return new TextEditorView();
		}

		public bool CanHandle(FilePath fileName, string mimeType, CocosItem ownerProject)
		{
			if (fileName.Extension.Equals(".lua", StringComparison.InvariantCultureIgnoreCase))
			{
				return true;
			}
			return false;
		}
	}
}
