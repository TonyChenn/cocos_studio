using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CocoStudio.UndoManager.TaskModel
{
	public class CompositeUndoableTask<T> : UndoableTaskBase<T>
	{
		public CompositeUndoableTask(IDictionary<UndoableTaskBase<T>, T> tasks, string descriptionForUser)
		{
			ArgumentValidator.AssertNotNull<string>(descriptionForUser, "descriptionForUser");
			ArgumentValidator.AssertNotNull<IDictionary<UndoableTaskBase<T>, T>>(tasks, "tasks");
			this.descriptionForUser = descriptionForUser;
			this.taskDictionary = new Dictionary<UndoableTaskBase<T>, T>(tasks);
			base.Execute += this.OnExecute;
			base.Undo += this.OnUndo;
			bool repeatable = this.taskDictionary.Keys.Count > 0;
			foreach (IInternalTask internalTask in tasks.Keys)
			{
				if (!internalTask.Repeatable)
				{
					repeatable = false;
				}
			}
			base.Repeatable = repeatable;
		}

		private void OnExecute(object sender, TaskEventArgs<T> e)
		{
			this.ExecuteInternal(this.taskDictionary, e.TaskMode);
		}

		protected internal virtual void ExecuteInternal(Dictionary<UndoableTaskBase<T>, T> taskDictionary, TaskMode taskMode)
		{
			if (this.Parallel)
			{
				CompositeUndoableTask<T>.ExecuteInParallel(taskDictionary, taskMode);
			}
			else
			{
				CompositeUndoableTask<T>.ExecuteSequentially(taskDictionary, taskMode);
			}
		}

		private static void ExecuteSequentially(Dictionary<UndoableTaskBase<T>, T> taskDictionary, TaskMode taskMode)
		{
			List<UndoableTaskBase<T>> list = new List<UndoableTaskBase<T>>();
			foreach (KeyValuePair<UndoableTaskBase<T>, T> keyValuePair in taskDictionary)
			{
				IInternalTask key = keyValuePair.Key;
				try
				{
					key.PerformTask(keyValuePair.Value, taskMode);
					list.Add(keyValuePair.Key);
				}
				catch (Exception)
				{
					CompositeUndoableTask<T>.SafelyUndoTasks(list.Cast<IUndoableTask>());
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
					catch (Exception value)
					{
						Console.WriteLine(value);
					}
				}
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
		}

		private void OnUndo(object sender, TaskEventArgs<T> e)
		{
			this.UndoInternal(this.taskDictionary);
		}

		protected internal virtual void UndoInternal(Dictionary<UndoableTaskBase<T>, T> taskDictionary)
		{
			if (this.Parallel)
			{
				CompositeUndoableTask<T>.UndoInParallel(taskDictionary);
			}
			else
			{
				CompositeUndoableTask<T>.UndoSequentially(taskDictionary);
			}
		}

		private static void UndoSequentially(Dictionary<UndoableTaskBase<T>, T> taskDictionary)
		{
			foreach (KeyValuePair<UndoableTaskBase<T>, T> keyValuePair in taskDictionary)
			{
				IUndoableTask key = keyValuePair.Key;
				key.Undo();
			}
		}

		public override string DescriptionForUser
		{
			get
			{
				return this.descriptionForUser;
			}
		}

		public bool Parallel { get; set; }

		private static void ExecuteInParallel(Dictionary<UndoableTaskBase<T>, T> taskDictionary, TaskMode taskMode)
		{
			List<UndoableTaskBase<T>> performedTasks = new List<UndoableTaskBase<T>>();
			object performedTasksLock = new object();
			List<Exception> exceptions = new List<Exception>();
			object exceptionsLock = new object();
			Dictionary<KeyValuePair<UndoableTaskBase<T>, T>, AutoResetEvent> dictionary = taskDictionary.ToDictionary((KeyValuePair<UndoableTaskBase<T>, T> x) => x, (KeyValuePair<UndoableTaskBase<T>, T> x) => new AutoResetEvent(false));
			foreach (KeyValuePair<UndoableTaskBase<T>, T> key in taskDictionary)
			{
				AutoResetEvent autoResetEvent = dictionary[key];
				IInternalTask task = key.Key;
				UndoableTaskBase<T> undoableTask = key.Key;
				T arg = key.Value;
				ThreadPool.QueueUserWorkItem(delegate(object param0)
				{
					try
					{
						task.PerformTask(arg, taskMode);
						lock (performedTasksLock)
						{
							performedTasks.Add(undoableTask);
						}
					}
					catch (Exception item)
					{
						lock (exceptionsLock)
						{
							exceptions.Add(item);
						}
					}
					autoResetEvent.Set();
				});
			}
			foreach (AutoResetEvent autoResetEvent2 in dictionary.Values)
			{
				autoResetEvent2.WaitOne();
			}
			if (exceptions.Count > 0)
			{
				CompositeUndoableTask<T>.SafelyUndoTasks(performedTasks.Cast<IUndoableTask>());
				throw new CompositeException("Unable to undo tasks", exceptions);
			}
		}

		private static void UndoInParallel(Dictionary<UndoableTaskBase<T>, T> taskDictionary)
		{
			List<UndoableTaskBase<T>> performedTasks = new List<UndoableTaskBase<T>>();
			object performedTasksLock = new object();
			List<Exception> exceptions = new List<Exception>();
			object exceptionsLock = new object();
			Dictionary<KeyValuePair<UndoableTaskBase<T>, T>, AutoResetEvent> dictionary = taskDictionary.ToDictionary((KeyValuePair<UndoableTaskBase<T>, T> x) => x, (KeyValuePair<UndoableTaskBase<T>, T> x) => new AutoResetEvent(false));
			foreach (KeyValuePair<UndoableTaskBase<T>, T> key in taskDictionary)
			{
				AutoResetEvent autoResetEvent = dictionary[key];
				UndoableTaskBase<T> undoableTask = key.Key;
				ThreadPool.QueueUserWorkItem(delegate(object param0)
				{
					try
					{
						((IUndoableTask)undoableTask).Undo();
						lock (performedTasksLock)
						{
							performedTasks.Add(undoableTask);
						}
					}
					catch (Exception item)
					{
						lock (exceptionsLock)
						{
							exceptions.Add(item);
						}
					}
					autoResetEvent.Set();
				});
			}
			foreach (AutoResetEvent autoResetEvent2 in dictionary.Values)
			{
				autoResetEvent2.WaitOne();
			}
			if (exceptions.Count > 0)
			{
				CompositeUndoableTask<T>.SafelyUndoTasks(performedTasks.Cast<IUndoableTask>());
				throw new CompositeException("Unable to undo tasks", exceptions);
			}
		}

		private readonly string descriptionForUser;

		private readonly Dictionary<UndoableTaskBase<T>, T> taskDictionary;
	}
}
