using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.Editor;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace Modules.Communal.Skeleton
{
	[Extension(typeof(IEditorController))]
	internal class CanShowSwitchButtonController : BaseSkeletonController
	{
		public override void RefreshEditor(IReadOnlyList<object> selectedObjs, string propertyName)
		{
			IPropertyGrid service = Services.GetService<IPropertyGrid>();
			UISizeEditor uisizeEditor = service.GetEditor("Size") as UISizeEditor;
			if (uisizeEditor != null)
			{
				uisizeEditor.CanShowSwitchButton = false;
			}
			PositionEditor positionEditor = service.GetEditor("Position") as PositionEditor;
			if (positionEditor != null)
			{
				positionEditor.CanShowSwitchButton = false;
			}
		}
	}
}
