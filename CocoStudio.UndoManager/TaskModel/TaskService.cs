using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Basic;

namespace CocoStudio.UndoManager.TaskModel
{
	// Token: 0x02000024 RID: 36
	public class TaskService : ITaskService, IInternalTaskService
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00005084 File Offset: 0x00003284
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x0000509C File Offset: 0x0000329C
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

		// Token: 0x060000F7 RID: 247 RVA: 0x000050A8 File Offset: 0x000032A8
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

		// Token: 0x060000F8 RID: 248 RVA: 0x00005198 File Offset: 0x00003398
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

		// Token: 0x060000F9 RID: 249 RVA: 0x00005214 File Offset: 0x00003414
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

		// Token: 0x060000FA RID: 250 RVA: 0x00005340 File Offset: 0x00003540
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

		// Token: 0x060000FB RID: 251 RVA: 0x000053C0 File Offset: 0x000035C0
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

		// Token: 0x060000FC RID: 252 RVA: 0x00005414 File Offset: 0x00003614
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

		// Token: 0x060000FD RID: 253 RVA: 0x0000555C File Offset: 0x0000375C
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

		// Token: 0x060000FE RID: 254 RVA: 0x0000560C File Offset: 0x0000380C
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

		// Token: 0x060000FF RID: 255 RVA: 0x0000566C File Offset: 0x0000386C
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

		// Token: 0x06000100 RID: 256 RVA: 0x000056A8 File Offset: 0x000038A8
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

		// Token: 0x06000101 RID: 257 RVA: 0x000056F4 File Offset: 0x000038F4
		private bool CanRedo()
		{
			return this.globallyRedoableTasks.Count > 0;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005714 File Offset: 0x00003914
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

		// Token: 0x06000103 RID: 259 RVA: 0x00005810 File Offset: 0x00003A10
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

		// Token: 0x06000104 RID: 260 RVA: 0x000058CC File Offset: 0x00003ACC
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

		// Token: 0x06000105 RID: 261 RVA: 0x00005A04 File Offset: 0x00003C04
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

		// Token: 0x06000106 RID: 262 RVA: 0x00005B6C File Offset: 0x00003D6C
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

		// Token: 0x06000107 RID: 263 RVA: 0x00005C38 File Offset: 0x00003E38
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

		// Token: 0x06000108 RID: 264 RVA: 0x00005C94 File Offset: 0x00003E94
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

		// Token: 0x06000109 RID: 265 RVA: 0x00005CEC File Offset: 0x00003EEC
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

		// Token: 0x0600010A RID: 266 RVA: 0x00005D74 File Offset: 0x00003F74
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

		// Token: 0x0600010B RID: 267 RVA: 0x00005E18 File Offset: 0x00004018
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

		// Token: 0x0600010C RID: 268 RVA: 0x00005E5C File Offset: 0x0000405C
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

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00005F34 File Offset: 0x00004134
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00005F4C File Offset: 0x0000414C
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

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600010F RID: 271 RVA: 0x00005F58 File Offset: 0x00004158
		// (remove) Token: 0x06000110 RID: 272 RVA: 0x00005F94 File Offset: 0x00004194
		private event EventHandler<CancellableTaskServiceEventArgs> executing;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000111 RID: 273 RVA: 0x00005FD0 File Offset: 0x000041D0
		// (remove) Token: 0x06000112 RID: 274 RVA: 0x00005FDB File Offset: 0x000041DB
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

		// Token: 0x06000113 RID: 275 RVA: 0x00005FE8 File Offset: 0x000041E8
		private void OnExecuting(CancellableTaskServiceEventArgs e)
		{
			if (this.executing != null)
			{
				this.executing(this, e);
			}
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000114 RID: 276 RVA: 0x00006014 File Offset: 0x00004214
		// (remove) Token: 0x06000115 RID: 277 RVA: 0x00006050 File Offset: 0x00004250
		private event EventHandler<TaskServiceEventArgs> executed;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000116 RID: 278 RVA: 0x0000608C File Offset: 0x0000428C
		// (remove) Token: 0x06000117 RID: 279 RVA: 0x00006097 File Offset: 0x00004297
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

		// Token: 0x06000118 RID: 280 RVA: 0x000060A4 File Offset: 0x000042A4
		private void OnExecuted(TaskServiceEventArgs e)
		{
			if (this.executed != null)
			{
				this.executed(this, e);
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000119 RID: 281 RVA: 0x000060D0 File Offset: 0x000042D0
		// (remove) Token: 0x0600011A RID: 282 RVA: 0x0000610C File Offset: 0x0000430C
		private event EventHandler<CancellableTaskServiceEventArgs> undoing;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x0600011B RID: 283 RVA: 0x00006148 File Offset: 0x00004348
		// (remove) Token: 0x0600011C RID: 284 RVA: 0x00006153 File Offset: 0x00004353
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

		// Token: 0x0600011D RID: 285 RVA: 0x00006160 File Offset: 0x00004360
		protected virtual void OnUndoing(CancellableTaskServiceEventArgs e)
		{
			this.IsUndoing = true;
			if (this.undoing != null)
			{
				this.undoing(this, e);
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x0600011E RID: 286 RVA: 0x00006194 File Offset: 0x00004394
		// (remove) Token: 0x0600011F RID: 287 RVA: 0x000061D0 File Offset: 0x000043D0
		private event EventHandler<TaskServiceEventArgs> undone;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000120 RID: 288 RVA: 0x0000620C File Offset: 0x0000440C
		// (remove) Token: 0x06000121 RID: 289 RVA: 0x00006217 File Offset: 0x00004417
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

		// Token: 0x06000122 RID: 290 RVA: 0x00006224 File Offset: 0x00004424
		protected virtual void OnUndone(TaskServiceEventArgs e)
		{
			this.IsUndoing = false;
			if (this.undone != null)
			{
				this.undone(this, e);
			}
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000123 RID: 291 RVA: 0x00006258 File Offset: 0x00004458
		// (remove) Token: 0x06000124 RID: 292 RVA: 0x00006294 File Offset: 0x00004494
		private event EventHandler<CancellableTaskServiceEventArgs> redoing;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000125 RID: 293 RVA: 0x000062D0 File Offset: 0x000044D0
		// (remove) Token: 0x06000126 RID: 294 RVA: 0x000062DB File Offset: 0x000044DB
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

		// Token: 0x06000127 RID: 295 RVA: 0x000062E8 File Offset: 0x000044E8
		protected virtual void OnRedoing(CancellableTaskServiceEventArgs e)
		{
			this.IsUndoing = true;
			if (this.redoing != null)
			{
				this.redoing(this, e);
			}
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000128 RID: 296 RVA: 0x0000631C File Offset: 0x0000451C
		// (remove) Token: 0x06000129 RID: 297 RVA: 0x00006358 File Offset: 0x00004558
		private event EventHandler<TaskServiceEventArgs> redone;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x0600012A RID: 298 RVA: 0x00006394 File Offset: 0x00004594
		// (remove) Token: 0x0600012B RID: 299 RVA: 0x0000639F File Offset: 0x0000459F
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

		// Token: 0x0600012C RID: 300 RVA: 0x000063AC File Offset: 0x000045AC
		protected virtual void OnRedone(TaskServiceEventArgs e)
		{
			this.IsUndoing = false;
			if (this.redone != null)
			{
				this.redone(this, e);
			}
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x0600012D RID: 301 RVA: 0x000063E0 File Offset: 0x000045E0
		// (remove) Token: 0x0600012E RID: 302 RVA: 0x0000641C File Offset: 0x0000461C
		private event EventHandler<EventArgs> cleared;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x0600012F RID: 303 RVA: 0x00006458 File Offset: 0x00004658
		// (remove) Token: 0x06000130 RID: 304 RVA: 0x00006463 File Offset: 0x00004663
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

		// Token: 0x06000131 RID: 305 RVA: 0x00006470 File Offset: 0x00004670
		private void OnCleared(EventArgs e)
		{
			if (this.cleared != null)
			{
				this.cleared(this, e);
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000649B File Offset: 0x0000469B
		void IInternalTaskService.NotifyTaskRepeatableChanged(IInternalTask task)
		{
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000064A0 File Offset: 0x000046A0
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

		// Token: 0x0400003D RID: 61
		private readonly Dictionary<object, TaskService.TaskCollection<IInternalTask>> repeatableDictionary = new Dictionary<object, TaskService.TaskCollection<IInternalTask>>();

		// Token: 0x0400003E RID: 62
		private readonly Dictionary<object, TaskService.TaskCollection<IUndoableTask>> redoableDictionary = new Dictionary<object, TaskService.TaskCollection<IUndoableTask>>();

		// Token: 0x0400003F RID: 63
		private readonly Dictionary<object, TaskService.TaskCollection<IUndoableTask>> undoableDictionary = new Dictionary<object, TaskService.TaskCollection<IUndoableTask>>();

		// Token: 0x04000040 RID: 64
		private readonly TaskService.TaskCollection<IInternalTask> globallyRepeatableTasks = new TaskService.TaskCollection<IInternalTask>();

		// Token: 0x04000041 RID: 65
		private readonly TaskService.TaskCollection<IUndoableTask> globallyRedoableTasks = new TaskService.TaskCollection<IUndoableTask>();

		// Token: 0x04000042 RID: 66
		private readonly TaskService.TaskCollection<IUndoableTask> globallyUndoableTasks = new TaskService.TaskCollection<IUndoableTask>();

		// Token: 0x04000043 RID: 67
		private bool isUndoing;

		// Token: 0x04000044 RID: 68
		private Dictionary<object, int> taskCountMaximums = new Dictionary<object, int>();

		// Token: 0x04000045 RID: 69
		private long taskCountMax = long.MaxValue;

		// Token: 0x04000046 RID: 70
		private bool enable;

		// Token: 0x02000025 RID: 37
		internal enum TaskType
		{
			// Token: 0x04000051 RID: 81
			Undoable,
			// Token: 0x04000052 RID: 82
			Redoable,
			// Token: 0x04000053 RID: 83
			Repeatable
		}

		// Token: 0x02000026 RID: 38
		private class TaskCollection<T> : LinkedList<T>
		{
			// Token: 0x06000137 RID: 311 RVA: 0x00006614 File Offset: 0x00004814
			public T Pop()
			{
				T value = base.Last.Value;
				base.RemoveLast();
				return value;
			}

			// Token: 0x06000138 RID: 312 RVA: 0x0000663C File Offset: 0x0000483C
			public T Peek()
			{
				LinkedListNode<T> last = base.Last;
				return (last != null) ? last.Value : default(T);
			}
		}
	}
}
