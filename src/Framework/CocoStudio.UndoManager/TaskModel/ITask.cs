using System;

namespace CocoStudio.UndoManager.TaskModel
{
	public interface ITask : IDisposable
	{
		string DescriptionForUser { get; }

		bool Undoable { get; }

		bool Repeatable { get; }
	}
}
