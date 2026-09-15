using System;
using CocoStudio.Model.Event;

namespace Modules.Communal.Render.Model
{
	public interface ICommandService
	{
		void DeleteObject();

		void CopyObject();

		void PasteObject(PasteObjectsChangeEventArgs args);

		void CutObject();

		void RotateObject(float rotation);
	}
}
