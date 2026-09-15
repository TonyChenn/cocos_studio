using System;
using MonoDevelop.Ide.Gui;

namespace CocoStudio.Core.View
{
	public interface IDocumentWindow : IWorkbenchWindow
	{
		IViewContentExtend ContentExtend { get; }
	}
}
