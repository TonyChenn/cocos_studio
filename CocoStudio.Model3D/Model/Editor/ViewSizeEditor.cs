using System;
using CocoStudio.Model.ViewModel;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000018 RID: 24
	internal class ViewSizeEditor : TwoNumberEditor
	{
		// Token: 0x060000C9 RID: 201 RVA: 0x0000408D File Offset: 0x0000228D
		public ViewSizeEditor() : base(LanguageInfo.Display_ResourceWidth, LanguageInfo.Display_ResourceHeight)
		{
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000040D4 File Offset: 0x000022D4
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
