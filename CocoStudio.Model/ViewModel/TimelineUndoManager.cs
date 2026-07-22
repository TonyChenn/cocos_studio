using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.UndoManager;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000ED RID: 237
	public class TimelineUndoManager
	{
		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x0001F080 File Offset: 0x0001D280
		public static TimelineUndoManager Instance
		{
			get
			{
				if (TimelineUndoManager.timelineUndoManager == null)
				{
					TimelineUndoManager.timelineUndoManager = new TimelineUndoManager();
					TimelineUndoManager.timelineUndoManager.undoManager = Services.TaskService;
				}
				return TimelineUndoManager.timelineUndoManager;
			}
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0001F0C1 File Offset: 0x0001D2C1
		public void Clear()
		{
			this.undoObjects.Clear();
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0001F0D0 File Offset: 0x0001D2D0
		public void RegisterUndoObject(BaseObject o)
		{
			this.undoObjects.Add(o);
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0001F0E0 File Offset: 0x0001D2E0
		public void UnRegisterUndoObject(BaseObject o)
		{
			this.undoObjects.Remove(o);
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0001F0F0 File Offset: 0x0001D2F0
		public void BeginCompositeTask()
		{
			if (this.undoManager.IsRunningCompositeTask)
			{
				this.undoManager.EndCompositeTask();
			}
			this.undoManager.BeginCompositeTask("TimelineTask");
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0001F130 File Offset: 0x0001D330
		public void EndCompositeTask()
		{
			if (this.undoManager.IsRunningCompositeTask)
			{
				this.undoManager.EndCompositeTask();
			}
		}

		// Token: 0x0400031A RID: 794
		private HashSet<BaseObject> undoObjects = new HashSet<BaseObject>();

		// Token: 0x0400031B RID: 795
		private IUndoManager undoManager;

		// Token: 0x0400031C RID: 796
		private static TimelineUndoManager timelineUndoManager;
	}
}
