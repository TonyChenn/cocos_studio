using System;
using CocoStudio.Model.Event;

namespace Modules.Communal.Skeleton
{
	internal interface ISkeletonTool
	{
		void OnSelectObjectsChangeEvent(SelectedVisualObjectsChangeEventArgs args);

		void OnCanvasZoomedChangedEvent();

		void OnRefreshControlDraw();
	}
}
