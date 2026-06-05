using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Basic;

namespace CocoStudio.UndoManager.TaskModel.UndoableTask
{
	// Token: 0x0200002A RID: 42
	public class SequentiallyCompositeUndoableTask<T> : UndoableTaskBase<T>
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00006FF0 File Offset: 0x000051F0
		public IEnumerable<UndoableTaskBase<T>> TaskList
		{
			get
			{
				return this.taskList;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00007008 File Offset: 0x00005208
		public override string DescriptionForUser
		{
			get
			{
				return this.descriptionForUser;
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00007020 File Offset: 0x00005220
		public SequentiallyCompositeUndoableTask(List<UndoableTaskBase<T>> taskList, string descriptionForUser)
		{
			ArgumentValidator.AssertNotNull<List<UndoableTaskBase<T>>>(taskList, "tasks");
			this.descriptionForUser = descriptionForUser;
			this.taskList = taskList.ToList<UndoableTaskBase<T>>();
			base.Execute += this.OnExecute;
			base.Undo += this.OnUndo;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000707B File Offset: 0x0000527B
		private void OnExecute(object sender, TaskEventArgs<T> e)
		{
			this.ExecuteInternal(this.taskList, e.TaskMode);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00007094 File Offset: 0x00005294
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

		// Token: 0x0600015A RID: 346 RVA: 0x0000711C File Offset: 0x0000531C
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

		// Token: 0x0600015B RID: 347 RVA: 0x000071B8 File Offset: 0x000053B8
		private void OnUndo(object sender, TaskEventArgs<T> e)
		{
			for (int i = this.taskList.Count - 1; i >= 0; i--)
			{
				((IUndoableTask)this.taskList[i]).Undo();
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000071FC File Offset: 0x000053FC
		~SequentiallyCompositeUndoableTask()
		{
			this.Dispose();
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00007230 File Offset: 0x00005430
		public override void Dispose()
		{
			for (int i = this.taskList.Count - 1; i >= 0; i--)
			{
				this.taskList[i].Dispose();
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}

		// Token: 0x0400005E RID: 94
		private List<UndoableTaskBase<T>> taskList;

		// Token: 0x0400005F RID: 95
		private string descriptionForUser;
	}
}
