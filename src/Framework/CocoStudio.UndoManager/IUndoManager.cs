using System;

namespace CocoStudio.UndoManager
{
	public interface IUndoManager : ITaskService
	{
		string CurrentCompositeTaskName { get; }

		bool IsRunningCompositeTask { get; }

		bool IsEmptyCompositeTask { get; }

		void BeginCompositeTask(string taskName);

		void EndCompositeTask();

		void SetCurrentDocument(IEditableDocument document);
	}
}
