using System;
using CocoStudio.Model.ViewModel;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x0200001D RID: 29
	public static class ObjectCopyHelper
	{
		// Token: 0x060000D8 RID: 216 RVA: 0x000058F0 File Offset: 0x00003AF0
		public static bool GetChildGlobalIndex(AbstractNodeObject parent, AbstractNodeObject vObject, ref int index)
		{
			index++;
			bool result;
			if (parent.Children == null || parent.Children.Count == 0)
			{
				result = false;
			}
			else
			{
				foreach (AbstractNodeObject abstractNodeObject in parent.Children)
				{
					if (abstractNodeObject == vObject)
					{
						return true;
					}
					if (ObjectCopyHelper.GetChildGlobalIndex(abstractNodeObject, vObject, ref index))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}
	}
}
