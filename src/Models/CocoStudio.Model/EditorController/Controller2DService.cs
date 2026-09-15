using System;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;

namespace CocoStudio.Model.EditorController
{
	internal static class Controller2DService
	{
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
