using System;
using CocoStudio.Core;
using CocoStudio.Core.View;
using CocoStudio.Projects;
using Gtk;
using MonoDevelop.Ide.Gui;
using MonoDevelop.SourceEditor;

namespace CocoStudio.SourceEditor
{
	internal class TextEditorView : SourceEditorView, IViewContentExtend, IViewContent, IBaseViewContent, IDisposable
	{
		public IDocumentWindow DocumentWindow { get; private set; }

		public CocosItem File { get; set; }

		public void Activated()
		{
		}

		public void AfterActivated()
		{
		}

		public void Deactivated()
		{
		}

		public void Closing()
		{
		}

		public void Closed()
		{
		}

		public void Reload()
		{
		}

		public Widget GetToolbar()
		{
			return null;
		}

		public override bool CanReuseView(string fileName)
		{
			return base.Document.FileName.Equals(fileName);
		}
	}
}
