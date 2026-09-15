using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Basic;

namespace CocoStudio.UndoManager.TaskModel
{
	public class TaskService : ITaskService, IInternalTaskService
	{
		public bool IsUndoing
		{
			get
			{
				return this.isUndoing;
			}
			set
			{
				this.isUndoing = value;
			}
		}

		public TaskResult PerformTask<T>(TaskBase<T> task, T argument, object ownerKey = null)
		{
			TaskResult result;
			if (!this.Enable || this.IsUndoing)
			{
				result = TaskResult.NoEnable;
			}
			else
			{
				ArgumentValidator.AssertNotNull<TaskBase<T>>(task, "task");
				if (ownerKey == null)
				{
					result = this.PerformTask<T>(task, argument);
				}
				else
				{
					CancellableTaskServiceEventArgs cancellableTaskServiceEventArgs = new CancellableTaskServiceEventArgs(task);
					this.OnExecuting(cancellableTaskServiceEventArgs);
					if (cancellableTaskServiceEventArgs.Cancel)
					{
						result = TaskResult.Cancelled;
					}
					else
					{
						this.undoableDictionary.Remove(ownerKey);
						this.redoableDictionary.Remove(ownerKey);
						TaskService.TaskCollection<IInternalTask> taskCollection;
						if (!this.repeatableDictionary.TryGetValue(ownerKey, out taskCollection))
						{
							taskCollection = new TaskService.TaskCollection<IInternalTask>();
							this.repeatableDictionary[ownerKey] = taskCollection;
						}
						taskCollection.AddLast(task);
						TaskResult taskResult = task.PerformTask(argument, TaskMode.FirstTime);
						this.TrimIfRequired(ownerKey);
						this.OnExecuted(new TaskServiceEventArgs(task));
						result = taskResult;
					}
				}
			}
			return result;
		}

		private TaskResult PerformTask<T>(TaskBase<T> task, T argument)
		{
			CancellableTaskServiceEventArgs cancellableTaskServiceEventArgs = new CancellableTaskServiceEventArgs(task);
			this.OnExecuting(cancellableTaskServiceEventArgs);
			TaskResult result;
			if (cancellableTaskServiceEventArgs.Cancel)
			{
				result = TaskResult.Cancelled;
			}
			else
			{
				this.globallyRedoableTasks.Clear();
				this.globallyUndoableTasks.Clear();
				this.globallyRepeatableTasks.AddLast(task);
				TaskResult taskResult = task.PerformTask(argument, TaskMode.FirstTime);
				this.TrimIfRequired(null);
				this.OnExecuted(new TaskServiceEventArgs(task));
				result = taskResult;
			}
			return result;
		}

		public TaskResult PerformTask<T>(UndoableTaskBase<T> task, T argument, object ownerKey = null)
		{
			TaskResult result;
			if (!this.Enable || this.IsUndoing)
			{
				result = TaskResult.NoEnable;
			}
			else
			{
				ArgumentValidator.AssertNotNull<UndoableTaskBase<T>>(task, "task");
				if (ownerKey == null)
				{
					result = this.PerformTask<T>(task, argument);
				}
				else
				{
					CancellableTaskServiceEventArgs cancellableTaskServiceEventArgs = new CancellableTaskServiceEventArgs(task)
					{
						OwnerKey = ownerKey
					};
					this.OnExecuting(cancellableTaskServiceEventArgs);
					if (cancellableTaskServiceEventArgs.Cancel)
					{
						result = TaskResult.Cancelled;
					}
					else
					{
						this.redoableDictionary.Remove(ownerKey);
						TaskService.TaskCollection<IInternalTask> taskCollection;
						if (!this.repeatableDictionary.TryGetValue(ownerKey, out taskCollection))
						{
							taskCollection = new TaskService.TaskCollection<IInternalTask>();
							this.repeatableDictionary[ownerKey] = taskCollection;
						}
						taskCollection.AddLast(task);
						TaskService.TaskCollection<IUndoableTask> taskCollection2;
						if (!this.undoableDictionary.TryGetValue(ownerKey, out taskCollection2))
						{
							taskCollection2 = new TaskService.TaskCollection<IUndoableTask>();
							this.undoableDictionary[ownerKey] = taskCollection2;
						}
						taskCollection2.AddLast(task);
						TaskResult taskResult = task.PerformTask(argument, TaskMode.FirstTime);
						this.TrimIfRequired(ownerKey);
						this.OnExecuted(new TaskServiceEventArgs(task));
						result = taskResult;
					}
				}
			}
			return result;
		}

		private TaskResult PerformTask<T>(UndoableTaskBase<T> task, T argument)
		{
			CancellableTaskServiceEventArgs cancellableTaskServiceEventArgs = new CancellableTaskServiceEventArgs(task);
			this.OnExecuting(cancellableTaskServiceEventArgs);
			TaskResult result;
			if (cancellableTaskServiceEventArgs.Cancel)
			{
				result = TaskResult.Cancelled;
			}
			else
			{
				this.globallyRedoableTasks.Clear();
				this.globallyRepeatableTasks.AddLast(task);
				this.globallyUndoableTasks.AddLast(task);
				TaskResult taskResult = task.PerformTask(argument, TaskMode.FirstTime);
				this.TrimIfRequired(null);
				this.OnExecuted(new TaskServiceEventArgs(task));
				result = taskResult;
			}
			return result;
		}

		public bool CanUndo(object ownerKey = null)
		{
			bool result;
			if (ownerKey == null)
			{
				result = (this.globallyUndoableTasks.Count > 0);
			}
			else
			{
				TaskService.TaskCollection<IUndoableTask> taskCollection;
				result = (this.undoableDictionary.TryGetValue(ownerKey, out taskCollection) && taskCollection.Count > 0);
			}
			return result;
		}

		public TaskResult Undo(object ownerKey = null)
		{
			TaskResult result;
			if (ownerKey == null)
			{
				result = this.Undo();
			}
			else
			{
				TaskService.TaskCollection<IUndoableTask> taskCollection;
				if (!this.undoableDictionary.TryGetValue(ownerKey, out taskCollection))
				{
					throw new InvalidOperationException("No undoable tasks for the specified owner key.");
				}
				IUndoableTask undoableTask = taskCollection.Pop();
				TaskService.TaskCollection<IInternalTask> taskCollection2;
				if (!this.repeatableDictionary.TryGetValue(ownerKey, out taskCollection2))
				{
					throw new InvalidOperationException("No repeatable tasks for the specified owner key.");
				}
				taskCollection2.RemoveLast();
				CancellableTaskServiceEventArgs cancellableTaskServiceEventArgs = new CancellableTaskServiceEventArgs(undoableTask)
				{
					OwnerKey = ownerKey
				};
				this.OnUndoing(cancellableTaskServiceEventArgs);
				if (cancellableTaskServiceEventArgs.Cancel)
				{
					taskCollection.AddLast(undoableTask);
					result = TaskResult.Cancelled;
				}
				else
				{
					TaskService.TaskCollection<IUndoableTask> taskCollection3;
					if (!this.redoableDictionary.TryGetValue(ownerKey, out taskCollection3))
					{
						taskCollection3 = new TaskService.TaskCollection<IUndoableTask>();
						this.redoableDictionary[ownerKey] = taskCollection3;
					}
					taskCollection3.AddLast(undoableTask);
					try
					{
						TaskResult taskResult = undoableTask.Undo();
						this.TrimIfRequired(ownerKey);
						result = taskResult;
					}
					catch (Exception exception)
					{
						LogConfig.Logger.Error("Undo excute failed.", exception);
						result = TaskResult.Failed;
					}
					finally
					{
						this.OnUndone(new TaskServiceEventArgs(undoableTask));
					}
				}
			}
			return result;
		}

		private TaskResult Undo()
		{
			if (this.globallyRepeatableTasks.Count < 1)
			{
				throw new InvalidOperationException("No task to undo.");
			}
			IUndoableTask undoableTask = this.globallyUndoableTasks.Pop();
			IInternalTask value = this.globallyRepeatableTasks.Pop();
			CancellableTaskServiceEventArgs cancellableTaskServiceEventArgs = new CancellableTaskServiceEventArgs(undoableTask);
			this.OnUndoing(cancellableTaskServiceEventArgs);
			TaskResult result;
			if (cancellableTaskServiceEventArgs.Cancel)
			{
				this.globallyUndoableTasks.AddLast(undoableTask);
				this.globallyRepeatableTasks.AddLast(value);
				result = TaskResult.Cancelled;
			}
			else
			{
				this.globallyRedoableTasks.AddLast(undoableTask);
				TaskResult taskResult = undoableTask.Undo();
				this.OnUndone(new TaskServiceEventArgs(undoableTask));
				result = taskResult;
			}
			return result;
		}

		public TaskResult Undo(int undoCount, object ownerKey = null)
		{
			ArgumentValidator.AssertGreaterThan(undoCount, 0, "undoCount");
			TaskResult result;
			if (ownerKey == null)
			{
				result = this.Undo(undoCount);
			}
			else
			{
				for (int i = 0; i < undoCount; i++)
				{
					TaskResult taskResult = this.Undo(ownerKey);
					if (taskResult != TaskResult.Completed)
					{
						return taskResult;
					}
				}
				result = TaskResult.Completed;
			}
			return result;
		}

		private TaskResult Undo(int undoCount)
		{
			for (int i = 0; i < undoCount; i++)
			{
				TaskResult taskResult = this.Undo();
				if (taskResult != TaskResult.Completed)
				{
					return taskResult;
				}
			}
			return TaskResult.Completed;
		}

		public bool CanRedo(object ownerKey = null)
		{
			bool result;
			if (ownerKey == null)
			{
				result = this.CanRedo();
			}
			else
			{
				TaskService.TaskCollection<IUndoableTask> taskCollection;
				result = (this.redoableDictionary.TryGetValue(ownerKey, out taskCollection) && taskCollection.Count > 0);
			}
			return result;
		}

		private bool CanRedo()
		{
			return this.globallyRedoableTasks.Count > 0;
		}

		public TaskResult Redo(object ownerKey = null)
		{
			TaskResult result;
			if (ownerKey == null)
			{
				result = this.Redo();
			}
			else
			{
				TaskService.TaskCollection<IUndoableTask> taskCollection;
				if (!this.redoableDictionary.TryGetValue(ownerKey, out taskCollection))
				{
					throw new InvalidOperationException("No tasks to be redone for the specified owner key.");
				}
				IUndoableTask undoableTask = taskCollection.Pop();
				CancellableTaskServiceEventArgs cancellableTaskServiceEventArgs = new CancellableTaskServiceEventArgs(undoableTask);
				this.OnRedoing(cancellableTaskServiceEventArgs);
				if (cancellableTaskServiceEventArgs.Cancel)
				{
					taskCollection.AddLast(undoableTask);
					result = TaskResult.Cancelled;
				}
				else
				{
					IInternalTask internalTask = (IInternalTask)undoableTask;
					TaskService.TaskCollection<IInternalTask> taskCollection2;
					if (!this.repeatableDictionary.TryGetValue(ownerKey, out taskCollection2))
					{
						taskCollection2 = new TaskService.TaskCollection<IInternalTask>();
					}
					taskCollection2.AddLast(internalTask);
					TaskService.TaskCollection<IUndoableTask> taskCollection3;
					if (!this.undoableDictionary.TryGetValue(ownerKey, out taskCollection3))
					{
						taskCollection3 = new TaskService.TaskCollection<IUndoableTask>();
					}
					taskCollection3.AddLast(undoableTask);
					TaskResult taskResult = internalTask.PerformTask(internalTask.Argument, TaskMode.Redo);
					this.TrimIfRequired(ownerKey);
					this.OnRedone(new TaskServiceEventArgs(undoableTask));
					result = taskResult;
				}
			}
			return result;
		}

		private TaskResult Redo()
		{
			if (this.globallyRedoableTasks.Count < 1)
			{
				throw new InvalidOperationException("No task to redo.");
			}
			IUndoableTask undoableTask = this.globallyRedoableTasks.Pop();
			CancellableTaskServiceEventArgs cancellableTaskServiceEventArgs = new CancellableTaskServiceEventArgs(undoableTask);
			this.OnRedoing(cancellableTaskServiceEventArgs);
			TaskResult result;
			if (cancellableTaskServiceEventArgs.Cancel)
			{
				this.globallyRedoableTasks.AddLast(undoableTask);
				result = TaskResult.Cancelled;
			}
			else
			{
				IInternalTask internalTask = (IInternalTask)undoableTask;
				this.globallyRepeatableTasks.AddLast(internalTask);
				this.globallyUndoableTasks.AddLast(undoableTask);
				TaskResult taskResult = internalTask.PerformTask(internalTask.Argument, TaskMode.Redo);
				this.TrimIfRequired(null);
				this.OnRedone(new TaskServiceEventArgs(undoableTask));
				result = taskResult;
			}
			return result;
		}

		public TaskResult Repeat(object ownerKey = null)
		{
			TaskResult result;
			if (ownerKey == null)
			{
				result = this.Repeat();
			}
			else
			{
				TaskService.TaskCollection<IInternalTask> taskCollection;
				if (!this.repeatableDictionary.TryGetValue(ownerKey, out taskCollection))
				{
					throw new InvalidOperationException("No tasks to be redone for the specified owner key.");
				}
				IInternalTask internalTask = taskCollection.Peek();
				if (!internalTask.Repeatable)
				{
					result = TaskResult.NoTask;
				}
				else
				{
					CancellableTaskServiceEventArgs cancellableTaskServiceEventArgs = new CancellableTaskServiceEventArgs(internalTask)
					{
						OwnerKey = ownerKey
					};
					this.OnExecuting(cancellableTaskServiceEventArgs);
					if (cancellableTaskServiceEventArgs.Cancel)
					{
						result = TaskResult.Cancelled;
					}
					else
					{
						taskCollection.AddLast(internalTask);
						TaskService.TaskCollection<IUndoableTask> taskCollection2;
						if (!this.undoableDictionary.TryGetValue(ownerKey, out taskCollection2))
						{
							taskCollection2 = new TaskService.TaskCollection<IUndoableTask>();
							this.undoableDictionary[ownerKey] = taskCollection2;
						}
						IUndoableTask undoableTask = internalTask as IUndoableTask;
						if (undoableTask != null)
						{
							taskCollection2.AddLast(undoableTask);
						}
						else
						{
							this.undoableDictionary[ownerKey] = null;
							this.redoableDictionary[ownerKey] = null;
						}
						TaskResult taskResult = internalTask.PerformTask(internalTask.Argument, TaskMode.Repeat);
						this.TrimIfRequired(ownerKey);
						this.OnExecuted(new TaskServiceEventArgs(internalTask));
						result = taskResult;
					}
				}
			}
			return result;
		}

		private void TrimIfRequired(object ownerKey = null)
		{
			long num = this.taskCountMax;
			TaskService.TaskCollection<IUndoableTask> taskCollection;
			TaskService.TaskCollection<IInternalTask> taskCollection2;
			TaskService.TaskCollection<IUndoableTask> taskCollection3;
			if (ownerKey != null)
			{
				int num2;
				if (this.taskCountMaximums.TryGetValue(ownerKey, out num2))
				{
					num = (long)num2;
				}
				if (num == 9223372036854775807L)
				{
					return;
				}
				this.undoableDictionary.TryGetValue(ownerKey, out taskCollection);
				this.repeatableDictionary.TryGetValue(ownerKey, out taskCollection2);
				this.redoableDictionary.TryGetValue(ownerKey, out taskCollection3);
			}
			else
			{
				if (this.taskCountMax == 9223372036854775807L)
				{
					return;
				}
				taskCollection = this.globallyUndoableTasks;
				taskCollection2 = this.globallyRepeatableTasks;
				taskCollection3 = this.globallyRedoableTasks;
			}
			int num3 = (taskCollection != null) ? taskCollection.Count : 0;
			int num4 = (taskCollection2 != null) ? taskCollection2.Count : 0;
			int num5 = (taskCollection3 != null) ? taskCollection3.Count : 0;
			long num6 = (long)num3 - num;
			long num7 = (long)num4 - num;
			long num8 = (long)num5 - num;
			for (long num9 = 0L; num9 < num6; num9 += 1L)
			{
				taskCollection.RemoveFirst();
			}
			for (long num9 = 0L; num9 < num7; num9 += 1L)
			{
				taskCollection2.RemoveFirst();
			}
			for (long num9 = 0L; num9 < num8; num9 += 1L)
			{
				taskCollection3.RemoveFirst();
			}
		}

		private TaskResult Repeat()
		{
			IInternalTask internalTask = this.globallyRepeatableTasks.Peek();
			TaskResult result;
			if (!internalTask.Repeatable)
			{
				result = TaskResult.NoTask;
			}
			else
			{
				CancellableTaskServiceEventArgs cancellableTaskServiceEventArgs = new CancellableTaskServiceEventArgs(internalTask);
				this.OnExecuting(cancellableTaskServiceEventArgs);
				if (cancellableTaskServiceEventArgs.Cancel)
				{
					result = TaskResult.Cancelled;
				}
				else
				{
					this.globallyRedoableTasks.Clear();
					this.globallyRepeatableTasks.AddLast(internalTask);
					IUndoableTask undoableTask = internalTask as IUndoableTask;
					if (undoableTask != null)
					{
						this.globallyUndoableTasks.AddLast(undoableTask);
					}
					else
					{
						this.globallyUndoableTasks.Clear();
						this.globallyRedoableTasks.Clear();
					}
					TaskResult taskResult = internalTask.PerformTask(internalTask.Argument, TaskMode.Repeat);
					this.OnExecuted(new TaskServiceEventArgs(internalTask));
					result = taskResult;
				}
			}
			return result;
		}

		public bool CanRepeat(object ownerKey = null)
		{
			TaskService.TaskCollection<IInternalTask> taskCollection;
			if (ownerKey == null)
			{
				taskCollection = this.globallyRepeatableTasks;
			}
			else if (!this.repeatableDictionary.TryGetValue(ownerKey, out taskCollection))
			{
				return false;
			}
			return taskCollection.Count > 0 && taskCollection.Peek().Repeatable;
		}

		public IEnumerable<ITask> GetUndoableTasks(object ownerKey = null)
		{
			IEnumerable<ITask> result;
			TaskService.TaskCollection<IUndoableTask> source;
			if (ownerKey == null)
			{
				result = new List<ITask>(this.globallyUndoableTasks.Cast<ITask>());
			}
			else if (!this.undoableDictionary.TryGetValue(ownerKey, out source))
			{
				result = new List<ITask>();
			}
			else
			{
				result = new List<ITask>(source.Cast<ITask>());
			}
			return result;
		}

		public IEnumerable<ITask> GetRedoableTasks(object ownerKey = null)
		{
			IEnumerable<ITask> result;
			TaskService.TaskCollection<IUndoableTask> source;
			if (ownerKey == null)
			{
				result = new List<ITask>(this.globallyRedoableTasks.Cast<ITask>());
			}
			else if (!this.redoableDictionary.TryGetValue(ownerKey, out source))
			{
				result = new List<ITask>();
			}
			else
			{
				result = new List<ITask>(source.Cast<ITask>());
			}
			return result;
		}

		public IEnumerable<ITask> GetRepeatableTasks(object ownerKey = null)
		{
			IEnumerable<ITask> result;
			TaskService.TaskCollection<IInternalTask> source;
			if (ownerKey == null)
			{
				List<ITask> list = (from task in this.globallyRepeatableTasks
				where task.Repeatable
				select task).Cast<ITask>().ToList<ITask>();
				result = list;
			}
			else if (!this.repeatableDictionary.TryGetValue(ownerKey, out source))
			{
				result = new List<ITask>();
			}
			else
			{
				List<ITask> list = (from task in source
				where task.Repeatable
				select task).Cast<ITask>().ToList<ITask>();
				result = list;
			}
			return result;
		}

		public void SetMaximumUndoCount(int count, object ownerKey = null)
		{
			ArgumentValidator.AssertGreaterThan(count, 0, "count");
			if (ownerKey == null)
			{
				this.taskCountMax = (long)count;
			}
			else
			{
				this.taskCountMaximums[ownerKey] = count;
			}
		}

		public void Clear(object ownerKey = null)
		{
			if (ownerKey == null)
			{
				this.globallyRepeatableTasks.Clear();
				this.globallyUndoableTasks.Clear();
				this.globallyRedoableTasks.Clear();
				this.OnCleared(EventArgs.Empty);
			}
			else
			{
				TaskService.TaskCollection<IInternalTask> taskCollection;
				if (this.repeatableDictionary.TryGetValue(ownerKey, out taskCollection))
				{
					taskCollection.Clear();
				}
				TaskService.TaskCollection<IUndoableTask> taskCollection2;
				if (this.undoableDictionary.TryGetValue(ownerKey, out taskCollection2))
				{
					taskCollection2.Clear();
				}
				TaskService.TaskCollection<IUndoableTask> taskCollection3;
				if (this.redoableDictionary.TryGetValue(ownerKey, out taskCollection3))
				{
					taskCollection3.Clear();
				}
				if (this.taskCountMaximums.ContainsKey(ownerKey))
				{
					this.taskCountMaximums.Remove(ownerKey);
				}
				this.OnCleared(EventArgs.Empty);
			}
		}

		public bool Enable
		{
			get
			{
				return this.enable;
			}
			set
			{
				this.enable = value;
			}
		}

		private event EventHandler<CancellableTaskServiceEventArgs> executing;

		public event EventHandler<CancellableTaskServiceEventArgs> Executing
		{
			add
			{
				this.executing += value;
			}
			remove
			{
				this.executing -= value;
			}
		}

		private void OnExecuting(CancellableTaskServiceEventArgs e)
		{
			if (this.executing != null)
			{
				this.executing(this, e);
			}
		}

		private event EventHandler<TaskServiceEventArgs> executed;

		public event EventHandler<TaskServiceEventArgs> Executed
		{
			add
			{
				this.executed += value;
			}
			remove
			{
				this.executed -= value;
			}
		}

		private void OnExecuted(TaskServiceEventArgs e)
		{
			if (this.executed != null)
			{
				this.executed(this, e);
			}
		}

		private event EventHandler<CancellableTaskServiceEventArgs> undoing;

		public event EventHandler<CancellableTaskServiceEventArgs> Undoing
		{
			add
			{
				this.undoing += value;
			}
			remove
			{
				this.undoing -= value;
			}
		}

		protected virtual void OnUndoing(CancellableTaskServiceEventArgs e)
		{
			this.IsUndoing = true;
			if (this.undoing != null)
			{
				this.undoing(this, e);
			}
		}

		private event EventHandler<TaskServiceEventArgs> undone;

		public event EventHandler<TaskServiceEventArgs> Undone
		{
			add
			{
				this.undone += value;
			}
			remove
			{
				this.undone -= value;
			}
		}

		protected virtual void OnUndone(TaskServiceEventArgs e)
		{
			this.IsUndoing = false;
			if (this.undone != null)
			{
				this.undone(this, e);
			}
		}

		private event EventHandler<CancellableTaskServiceEventArgs> redoing;

		public event EventHandler<CancellableTaskServiceEventArgs> Redoing
		{
			add
			{
				this.redoing += value;
			}
			remove
			{
				this.redoing -= value;
			}
		}

		protected virtual void OnRedoing(CancellableTaskServiceEventArgs e)
		{
			this.IsUndoing = true;
			if (this.redoing != null)
			{
				this.redoing(this, e);
			}
		}

		private event EventHandler<TaskServiceEventArgs> redone;

		public event EventHandler<TaskServiceEventArgs> Redone
		{
			add
			{
				this.redone += value;
			}
			remove
			{
				this.redone -= value;
			}
		}

		protected virtual void OnRedone(TaskServiceEventArgs e)
		{
			this.IsUndoing = false;
			if (this.redone != null)
			{
				this.redone(this, e);
			}
		}

		private event EventHandler<EventArgs> cleared;

		public event EventHandler<EventArgs> Cleared
		{
			add
			{
				this.cleared += value;
			}
			remove
			{
				this.cleared -= value;
			}
		}

		private void OnCleared(EventArgs e)
		{
			if (this.cleared != null)
			{
				this.cleared(this, e);
			}
		}

		void IInternalTaskService.NotifyTaskRepeatableChanged(IInternalTask task)
		{
		}

		internal int GetTaskCount(TaskService.TaskType taskType, object ownerKey = null)
		{
			int result;
			if (taskType == TaskService.TaskType.Undoable)
			{
				TaskService.TaskCollection<IUndoableTask> taskCollection;
				if (ownerKey == null)
				{
					result = this.globallyUndoableTasks.Count;
				}
				else if (!this.undoableDictionary.TryGetValue(ownerKey, out taskCollection))
				{
					result = 0;
				}
				else
				{
					result = taskCollection.Count;
				}
			}
			else if (taskType == TaskService.TaskType.Repeatable)
			{
				TaskService.TaskCollection<IInternalTask> taskCollection2;
				if (ownerKey == null)
				{
					result = this.globallyRepeatableTasks.Count;
				}
				else if (!this.repeatableDictionary.TryGetValue(ownerKey, out taskCollection2))
				{
					result = 0;
				}
				else
				{
					result = taskCollection2.Count;
				}
			}
			else
			{
				if (taskType != TaskService.TaskType.Redoable)
				{
					throw new InvalidOperationException("Unknown task type: " + taskType);
				}
				TaskService.TaskCollection<IUndoableTask> taskCollection;
				if (ownerKey == null)
				{
					result = this.globallyRedoableTasks.Count;
				}
				else if (!this.redoableDictionary.TryGetValue(ownerKey, out taskCollection))
				{
					result = 0;
				}
				else
				{
					result = taskCollection.Count;
				}
			}
			return result;
		}

		private readonly Dictionary<object, TaskService.TaskCollection<IInternalTask>> repeatableDictionary = new Dictionary<object, TaskService.TaskCollection<IInternalTask>>();

		private readonly Dictionary<object, TaskService.TaskCollection<IUndoableTask>> redoableDictionary = new Dictionary<object, TaskService.TaskCollection<IUndoableTask>>();

		private readonly Dictionary<object, TaskService.TaskCollection<IUndoableTask>> undoableDictionary = new Dictionary<object, TaskService.TaskCollection<IUndoableTask>>();

		private readonly TaskService.TaskCollection<IInternalTask> globallyRepeatableTasks = new TaskService.TaskCollection<IInternalTask>();

		private readonly TaskService.TaskCollection<IUndoableTask> globallyRedoableTasks = new TaskService.TaskCollection<IUndoableTask>();

		private readonly TaskService.TaskCollection<IUndoableTask> globallyUndoableTasks = new TaskService.TaskCollection<IUndoableTask>();

		private bool isUndoing;

		private Dictionary<object, int> taskCountMaximums = new Dictionary<object, int>();

		private long taskCountMax = long.MaxValue;

		private bool enable;

		internal enum TaskType
		{
			Undoable,
			Redoable,
			Repeatable
		}

		private class TaskCollection<T> : LinkedList<T>
		{
			public T Pop()
			{
				T value = base.Last.Value;
				base.RemoveLast();
				return value;
			}

			public T Peek()
			{
				LinkedListNode<T> last = base.Last;
				return (last != null) ? last.Value : default(T);
			}
		}
	}
}
