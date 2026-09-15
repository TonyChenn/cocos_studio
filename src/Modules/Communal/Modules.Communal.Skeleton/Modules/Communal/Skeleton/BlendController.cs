using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.ViewModel;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace Modules.Communal.Skeleton
{
	[Extension(typeof(IEditorController))]
	internal class BlendController : BaseSkeletonController
	{
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
