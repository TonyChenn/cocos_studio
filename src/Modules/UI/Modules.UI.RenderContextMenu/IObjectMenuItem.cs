using System;
using CocoStudio.Model.ViewModel;

namespace Modules.UI.RenderContextMenu
{
	internal interface IObjectMenuItem
	{
		void UpdateMenuItemState();

		VisualObject TriggerObject { get; set; }
	}
}
