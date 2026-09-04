using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.Editor;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace Modules.Communal.Skeleton
{
	// Token: 0x0200000A RID: 10
	[Extension(typeof(IEditorController))]
	internal class CanShowSwitchButtonController : BaseSkeletonController
	{
		// Token: 0x06000047 RID: 71 RVA: 0x000031F8 File Offset: 0x000013F8
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
