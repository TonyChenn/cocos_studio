using System;

namespace CocoStudio.UndoManager
{
	public interface IEditableDocument
	{
		bool IsDirty { get; set; }

		string Name { get; }
	}
}
