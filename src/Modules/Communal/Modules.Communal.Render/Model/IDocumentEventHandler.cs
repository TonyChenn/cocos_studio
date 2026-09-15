using System;
using CocoStudio.Projects;

namespace Modules.Communal.Render.Model
{
	public interface IDocumentEventHandler
	{
		void OnDocumentChanged(CocosItem cocosItem);

		void OnDocumentSaved(CocosItem cocosItem);

		void OnDocumentBeforeSave(CocosItem cocosItem);

		void OnDocumentClosed(CocosItem cocosItem);
	}
}
