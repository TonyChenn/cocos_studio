using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CocoStudio.UndoManager.TaskModel
{
	// Token: 0x0200001F RID: 31
	public class CompositeTask<T> : TaskBase<T>
	{
		// Token: 0x060000E6 RID: 230 RVA: 0x00004B44 File Offset: 0x00002D44
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

		// Token: 0x060000E7 RID: 231 RVA: 0x00004C08 File Offset: 0x00002E08
		private void OnExecute(object sender, TaskEventArgs<T> e)
		{
			this.ExecuteInternal(this.taskDictionary, e.TaskMode);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004C20 File Offset: 0x00002E20
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

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00004C54 File Offset: 0x00002E54
		public override string DescriptionForUser
		{
			get
			{
				return this.descriptionForUser;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00004C6C File Offset: 0x00002E6C
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00004C83 File Offset: 0x00002E83
		public bool Parallel { get; set; }

		// Token: 0x060000EC RID: 236 RVA: 0x00004C8C File Offset: 0x00002E8C
		private static void ExecuteInSequence(Dictionary<TaskBase<T>, T> taskDictionary, TaskMode taskMode)
		{
			foreach (KeyValuePair<TaskBase<T>, T> keyValuePair in taskDictionary)
			{
				IInternalTask key = keyValuePair.Key;
				key.PerformTask(keyValuePair.Value, taskMode);
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004E28 File Offset: 0x00003028
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

		// Token: 0x0400002C RID: 44
		private readonly string descriptionForUser;

		// Token: 0x0400002D RID: 45
		private readonly Dictionary<TaskBase<T>, T> taskDictionary;
	}
}
