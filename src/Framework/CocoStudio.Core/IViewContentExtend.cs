using System;
using CocoStudio.Core.View;
using CocoStudio.Projects;
using Gtk;
using MonoDevelop.Ide.Gui;

namespace CocoStudio.Core
{
	public interface IViewContentExtend : IViewContent, IBaseViewContent, IDisposable
	{
		IDocumentWindow DocumentWindow { get; }

		CocosItem File { get; set; }

		void Activated();

		void AfterActivated();

		void Deactivated();

		void Closing();

		void Closed();

		void Reload();

		Widget GetToolbar();
	}
}
