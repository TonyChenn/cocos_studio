using System;
using System.IO;
using Mono.TextEditor.Highlighting;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects;
using MonoDevelop.SourceEditor.Extension;

namespace MonoDevelop.SourceEditor
{
	public class SourceEditorDisplayBinding : IViewDisplayBinding, IDisplayBinding
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
				SyntaxModeService.EnsureLoad();
				LoadCustomStylesAndModes();
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
				catch (Exception ex)
				{
					flag = false;
					LoggingService.LogError("Can't create syntax mode directory", ex);
				}
			}
			if (flag)
			{
				Mono.TextEditor.Highlighting.SyntaxModeService.LoadStylesAndModes(SyntaxModePath);
			}
		}

		public bool CanHandle(FilePath fileName, string mimeType, Project ownerProject)
		{
			if (fileName != null)
			{
				return DesktopService.GetFileIsText(fileName, mimeType);
			}
			if (!string.IsNullOrEmpty(mimeType))
			{
				return DesktopService.GetMimeTypeIsText(mimeType);
			}
			return false;
		}

		public IViewContent CreateContent(FilePath fileName, string mimeType, Project ownerProject)
		{
			return new SourceEditorView();
		}

		public bool CanHandleFile(string fileName)
		{
			return DesktopService.GetFileIsText(fileName);
		}
	}
}
