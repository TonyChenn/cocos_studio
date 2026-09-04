using System;
using CocoStudio.Model.Event;

namespace Modules.Communal.Skeleton
{
	// Token: 0x02000005 RID: 5
	internal interface ISkeletonTool
	{
		// Token: 0x06000025 RID: 37
		void OnSelectObjectsChangeEvent(SelectedVisualObjectsChangeEventArgs args);

		// Token: 0x06000026 RID: 38
		void OnCanvasZoomedChangedEvent();

		// Token: 0x06000027 RID: 39
		void OnRefreshControlDraw();
	}
}
