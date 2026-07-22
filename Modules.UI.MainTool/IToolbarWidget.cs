using System;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using Gtk;

namespace Modules.UI.MainTool
{
	// Token: 0x02000002 RID: 2
	internal interface IToolbarWidget
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1
		Widget GtkWidget { get; }

		// Token: 0x06000002 RID: 2
		void OnProjectChanged(ProjectsOperations.ProjectEventArgs args);

		// Token: 0x06000003 RID: 3
		void OnSolutionChanged(SolutionEventArgs args);

		// Token: 0x06000004 RID: 4
		void OnSolutionClosed(SolutionEventArgs args);
	}
}
