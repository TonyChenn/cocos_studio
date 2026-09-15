using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Basic;

namespace CocoStudio.UndoManager.TaskModel.UndoableTask
{
	public class SequentiallyCompositeUndoableTask<T> : UndoableTaskBase<T>
	{
		public IEnumerable<UndoableTaskBase<T>> TaskList
		{
			get
			{
				return this.taskList;
			}
		}

		public override string DescriptionForUser
		{
			get
			{
				return this.descriptionForUser;
			}
		}

		public SequentiallyCompositeUndoableTask(List<UndoableTaskBase<T>> taskList, string descriptionForUser)
		{
			ArgumentValidator.AssertNotNull<List<UndoableTaskBase<T>>>(taskList, "tasks");
			this.descriptionForUser = descriptionForUser;
			this.taskList = taskList.ToList<UndoableTaskBase<T>>();
			base.Execute += this.OnExecute;
			base.Undo += this.OnUndo;
		}

		private void OnExecute(object sender, TaskEventArgs<T> e)
		{
			this.ExecuteInternal(this.taskList, e.TaskMode);
		}

		protected internal virtual void ExecuteInternal(List<UndoableTaskBase<T>> taskList, TaskMode taskMode)
		{
			List<UndoableTaskBase<T>> list = new List<UndoableTaskBase<T>>();
			foreach (UndoableTaskBase<T> undoableTaskBase in taskList)
			{
				try
				{
					undoableTaskBase.PerformTask(null, taskMode);
					list.Add(undoableTaskBase);
				}
				catch (Exception)
				{
					SequentiallyCompositeUndoableTask<T>.SafelyUndoTasks(list.Cast<IUndoableTask>());
					throw;
				}
			}
		}

		private static void SafelyUndoTasks(IEnumerable<IUndoableTask> undoableTasks)
		{
			try
			{
				foreach (IUndoableTask undoableTask in undoableTasks)
				{
					try
					{
						undoableTask.Undo();
					}
					catch (Exception ex)
					{
						LogConfig.Logger.Error("SafelyUndoTasks failed", ex);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		private void OnUndo(object sender, TaskEventArgs<T> e)
		{
			for (int i = this.taskList.Count - 1; i >= 0; i--)
			{
				((IUndoableTask)this.taskList[i]).Undo();
			}
		}

		~SequentiallyCompositeUndoableTask()
		{
			this.Dispose();
		}

		public override void Dispose()
		{
			for (int i = this.taskList.Count - 1; i >= 0; i--)
			{
				this.taskList[i].Dispose();
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}

		private List<UndoableTaskBase<T>> taskList;

		private string descriptionForUser;
	}
}
