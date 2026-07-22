using System;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200000D RID: 13
	internal class ClipPlaneEditor : TwoNumberEditor
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00002C3D File Offset: 0x00000E3D
		public ClipPlaneEditor() : base(string.Format(" {0} ", LanguageInfo.Display_NearPlane), string.Format(" {0} ", LanguageInfo.Display_FarPlane))
		{
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00002C64 File Offset: 0x00000E64
		protected override Widget OnCreateWidget()
		{
			Widget result = base.OnCreateWidget();
			this.xInnerEntry.MaxValue = (this.yInnerEntry.MaxValue = 9999999);
			this.xInnerEntry.MinValue = (this.yInnerEntry.MinValue = 0);
			return result;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00002CB4 File Offset: 0x00000EB4
		protected override void OnXValueChanged(EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				foreach (object obj in PropertyItem.Objects)
				{
					PointF pointF = (PointF)base.PropertyItem.Values[obj];
					pointF.X = e.Value;
					if (pointF.Y <= pointF.X)
					{
						pointF.Y = pointF.X + 1f;
						this.yInnerEntry.Value = pointF.Y;
					}
					if (pointF.X < 1f)
					{
						pointF.X = 1f;
						this.xInnerEntry.Value = pointF.X;
					}
					base.PropertyItem.Values[obj] = pointF;
				}
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00002DB0 File Offset: 0x00000FB0
		protected override void OnYValueChanged(EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				foreach (object obj in PropertyItem.Objects)
				{
					PointF pointF = (PointF)base.PropertyItem.Values[obj];
					pointF.Y = e.Value;
					if (pointF.Y <= pointF.X)
					{
						pointF.Y = pointF.X + 1f;
						this.yInnerEntry.Value = pointF.Y;
					}
					base.PropertyItem.Values[obj] = pointF;
				}
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00002EB0 File Offset: 0x000010B0
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
