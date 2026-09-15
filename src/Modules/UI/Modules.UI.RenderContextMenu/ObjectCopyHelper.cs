using System;
using CocoStudio.Model.ViewModel;

namespace Modules.UI.RenderContextMenu
{
	public static class ObjectCopyHelper
	{
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
