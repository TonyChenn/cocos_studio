using System;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using Gtk;

namespace Modules.UI.MainTool
{
	// Token: 0x02000003 RID: 3
	internal abstract class BaseToolbarWidget : IToolbarWidget
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5
		public abstract Widget GtkWidget { get; }

		// Token: 0x06000006 RID: 6 RVA: 0x00002050 File Offset: 0x00000250
		public virtual void OnProjectChanged(ProjectsOperations.ProjectEventArgs args)
		{
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002053 File Offset: 0x00000253
		public virtual void OnSolutionChanged(SolutionEventArgs args)
		{
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002056 File Offset: 0x00000256
		public virtual void OnSolutionClosed(SolutionEventArgs args)
		{
		}
	}
}
