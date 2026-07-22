using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.ViewModel;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace Modules.Communal.Skeleton
{
	// Token: 0x02000009 RID: 9
	[Extension(typeof(IEditorController))]
	internal class BlendController : BaseSkeletonController
	{
		// Token: 0x06000045 RID: 69 RVA: 0x00003180 File Offset: 0x00001380
		public override void RefreshEditor(IReadOnlyList<object> selectedObjs, string propertyName)
		{
			IPropertyGrid service = Services.GetService<IPropertyGrid>();
			IPropertyEditor editor = service.GetEditor("BlendFunc");
			if (editor != null)
			{
				foreach (object obj in selectedObjs)
				{
					if (obj is SpriteObject || obj is ParticleObject)
					{
						editor.Enable = false;
						break;
					}
				}
			}
		}
	}
}
