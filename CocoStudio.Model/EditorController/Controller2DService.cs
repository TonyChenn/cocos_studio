using System;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;

namespace CocoStudio.Model.EditorController
{
	// Token: 0x0200004B RID: 75
	internal static class Controller2DService
	{
		// Token: 0x06000298 RID: 664 RVA: 0x000083A4 File Offset: 0x000065A4
		public static bool CheckSizeCanUse(object obj)
		{
			IStretchSize stretchSize = obj as IStretchSize;
			bool result;
			if (stretchSize == null || !stretchSize.CanShowStretch)
			{
				result = false;
			}
			else
			{
				ISizeType sizeType = obj as ISizeType;
				if (sizeType != null && !sizeType.IsCustomSize)
				{
					result = false;
				}
				else
				{
					FileNodeObject fileNodeObject = obj as FileNodeObject;
					if (fileNodeObject != null)
					{
						if (fileNodeObject.Project == null)
						{
							return false;
						}
						string fileType = fileNodeObject.Project.GetFileType();
						if (fileType != NodeType.Layer.ToString())
						{
							return false;
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00008454 File Offset: 0x00006654
		public static bool CheckOperationModeIsValid(object obj, OperationMask? requestMode)
		{
			bool result;
			if (requestMode == null)
			{
				result = true;
			}
			else
			{
				AbstractNodeObject abstractNodeObject = obj as AbstractNodeObject;
				result = (abstractNodeObject != null && abstractNodeObject.OperationFlag.HasFlag((Enum)requestMode));
			}
			return result;
		}
	}
}
