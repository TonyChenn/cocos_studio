using System;
using System.Diagnostics;
using Mono.TextEditor;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.SourceEditor
{
	public class MarkerOperationsHandler : CommandHandler
	{
		protected override void Run(object data)
		{
			UrlMarker urlMarker = data as UrlMarker;
			if (data == null)
			{
				return;
			}
			try
			{
				if (urlMarker.UrlType == UrlType.Email)
				{
					Process.Start("mailto:" + urlMarker.Url);
				}
				else
				{
					Process.Start(urlMarker.Url);
				}
			}
			catch (Exception)
			{
				MessageService.ShowError(GettextCatalog.GetString("Could not open the url {0}", urlMarker.Url));
			}
		}

		protected override void Update(CommandArrayInfo ainfo)
		{
			Document activeDocument = IdeApp.Workbench.ActiveDocument;
			if (activeDocument == null)
			{
				return;
			}
			SourceEditorView content = IdeApp.Workbench.ActiveDocument.GetContent<SourceEditorView>();
			if (content == null)
			{
				return;
			}
			DocumentLocation location = content.TextEditor.Caret.Location;
			if (location.IsEmpty)
			{
				return;
			}
			DocumentLine line = content.Document.GetLine(location.Line);
			if (line == null || line.Markers == null)
			{
				return;
			}
			foreach (TextLineMarker marker in line.Markers)
			{
				if (marker is UrlMarker urlMarker && urlMarker.StartColumn <= location.Column && location.Column < urlMarker.EndColumn)
				{
					ainfo.Add((urlMarker.UrlType == UrlType.Email) ? GettextCatalog.GetString("_Write an e-mail to...") : GettextCatalog.GetString("_Open URL..."), urlMarker);
					ainfo.AddSeparator();
				}
			}
		}
	}
}
