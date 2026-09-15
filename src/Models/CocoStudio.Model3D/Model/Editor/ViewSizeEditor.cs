using System;
using CocoStudio.Model.ViewModel;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class ViewSizeEditor : TwoNumberEditor
	{
		public ViewSizeEditor() : base(LanguageInfo.Display_ResourceWidth, LanguageInfo.Display_ResourceHeight)
		{
		}

		protected override void OnSetControl()
		{
			PointF pointF = (PointF)base.PropertyItem.Values[0];
			if (PropertyItem.Objects.Count <= 1)
			{
				object firstObject = PropertyItem.FirstObject;
				this.xInnerEntry.Value = pointF.X;
				this.yInnerEntry.Value = pointF.Y;
				return;
			}
			Func<UserCameraObject, UserCameraObject, bool> func = (UserCameraObject a, UserCameraObject b) => a.ClipPlane.X == b.ClipPlane.X;
			Func<UserCameraObject, UserCameraObject, bool> func2 = (UserCameraObject a, UserCameraObject b) => a.ClipPlane.Y == b.ClipPlane.Y;
			if (base.IsWhipNode<UserCameraObject>(func))
			{
				this.xInnerEntry.SetToSubState();
			}
			else
			{
				this.xInnerEntry.Value = pointF.X;
			}
			if (base.IsWhipNode<UserCameraObject>(func2))
			{
				this.yInnerEntry.SetToSubState();
				return;
			}
			this.yInnerEntry.Value = pointF.Y;
		}
	}
}
