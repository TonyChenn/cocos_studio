using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CocoStudio.UndoManager.TaskModel
{
	// Token: 0x02000029 RID: 41
	public class CompositeUndoableTask<T> : UndoableTaskBase<T>
	{
		// Token: 0x06000144 RID: 324 RVA: 0x0000672C File Offset: 0x0000492C
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

		// Token: 0x06000145 RID: 325 RVA: 0x00006804 File Offset: 0x00004A04
		private void OnExecute(object sender, TaskEventArgs<T> e)
		{
			this.ExecuteInternal(this.taskDictionary, e.TaskMode);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000681C File Offset: 0x00004A1C
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

		// Token: 0x06000147 RID: 327 RVA: 0x00006850 File Offset: 0x00004A50
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

		// Token: 0x06000148 RID: 328 RVA: 0x000068F4 File Offset: 0x00004AF4
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

		// Token: 0x06000149 RID: 329 RVA: 0x00006984 File Offset: 0x00004B84
		private void OnUndo(object sender, TaskEventArgs<T> e)
		{
			this.UndoInternal(this.taskDictionary);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00006994 File Offset: 0x00004B94
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

		// Token: 0x0600014B RID: 331 RVA: 0x000069C4 File Offset: 0x00004BC4
		private static void UndoSequentially(Dictionary<UndoableTaskBase<T>, T> taskDictionary)
		{
			foreach (KeyValuePair<UndoableTaskBase<T>, T> keyValuePair in taskDictionary)
			{
				IUndoableTask key = keyValuePair.Key;
				key.Undo();
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00006A24 File Offset: 0x00004C24
		public override string DescriptionForUser
		{
			get
			{
				return this.descriptionForUser;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00006A3C File Offset: 0x00004C3C
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00006A53 File Offset: 0x00004C53
		public bool Parallel { get; set; }

		// Token: 0x0600014F RID: 335 RVA: 0x00006B8C File Offset: 0x00004D8C
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

		// Token: 0x06000150 RID: 336 RVA: 0x00006E5C File Offset: 0x0000505C
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

		// Token: 0x04000057 RID: 87
		private readonly string descriptionForUser;

		// Token: 0x04000058 RID: 88
		private readonly Dictionary<UndoableTaskBase<T>, T> taskDictionary;
	}
}
