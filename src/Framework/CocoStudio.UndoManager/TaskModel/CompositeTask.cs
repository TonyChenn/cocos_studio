using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CocoStudio.UndoManager.TaskModel
{
	public class CompositeTask<T> : TaskBase<T>
	{
		public CompositeTask(IDictionary<TaskBase<T>, T> tasks, string descriptionForUser)
		{
			ArgumentValidator.AssertNotNull<string>(descriptionForUser, "descriptionForUser");
			ArgumentValidator.AssertNotNull<IDictionary<TaskBase<T>, T>>(tasks, "tasks");
			this.descriptionForUser = descriptionForUser;
			this.taskDictionary = new Dictionary<TaskBase<T>, T>(tasks);
			base.Execute += this.OnExecute;
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

		protected internal virtual void ExecuteInternal(Dictionary<TaskBase<T>, T> taskDictionary, TaskMode taskMode)
		{
			if (this.Parallel)
			{
				CompositeTask<T>.ExecuteInParallel(taskDictionary, taskMode);
			}
			else
			{
				CompositeTask<T>.ExecuteInSequence(taskDictionary, taskMode);
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

		private static void ExecuteInSequence(Dictionary<TaskBase<T>, T> taskDictionary, TaskMode taskMode)
		{
			foreach (KeyValuePair<TaskBase<T>, T> keyValuePair in taskDictionary)
			{
				IInternalTask key = keyValuePair.Key;
				key.PerformTask(keyValuePair.Value, taskMode);
			}
		}

		private static void ExecuteInParallel(Dictionary<TaskBase<T>, T> taskDictionary, TaskMode taskMode)
		{
			List<TaskBase<T>> performedTasks = new List<TaskBase<T>>();
			object performedTasksLock = new object();
			List<Exception> exceptions = new List<Exception>();
			object exceptionsLock = new object();
			Dictionary<KeyValuePair<TaskBase<T>, T>, AutoResetEvent> dictionary = taskDictionary.ToDictionary((KeyValuePair<TaskBase<T>, T> x) => x, (KeyValuePair<TaskBase<T>, T> x) => new AutoResetEvent(false));
			foreach (KeyValuePair<TaskBase<T>, T> key in taskDictionary)
			{
				AutoResetEvent autoResetEvent = dictionary[key];
				IInternalTask task = key.Key;
				TaskBase<T> undoableTask = key.Key;
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
				throw new CompositeException("Unable to undo tasks", exceptions);
			}
		}

		private readonly string descriptionForUser;

		private readonly Dictionary<TaskBase<T>, T> taskDictionary;
	}
}
