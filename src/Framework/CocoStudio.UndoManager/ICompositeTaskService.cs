using System;

namespace CocoStudio.UndoManager
{
	public interface ICompositeTaskService
	{
		string CurrentCompositeTaskName { get; }

		bool IsRunningCompositeTask { get; }

		bool IsEmptyCompositeTask { get; }

		void RunAsCompositeTask(string taskName, Action aciton);

		void RunAsCompositeTask(string taskName, Action<object> aciton, object parameter);

		void BeginCompositeTask(string taskName);

		void EndCompositeTask();

		void AddRecord(UndoTask task);

		void SetCurrentDocument(IEditableDocument document);
	}
}
