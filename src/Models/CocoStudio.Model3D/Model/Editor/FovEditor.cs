using System;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200000F RID: 15
	internal class FovEditor : OneNumberEditor
	{
		// Token: 0x06000090 RID: 144 RVA: 0x00003048 File Offset: 0x00001248
		public FovEditor() : base("")
		{
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003058 File Offset: 0x00001258
		protected override Widget OnCreateWidget()
		{
			Widget result = base.OnCreateWidget();
			this.innerEntry.MaxValue = 179;
			this.innerEntry.MinValue = 1;
			return result;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000309C File Offset: 0x0000129C
		protected override void OnSetControl()
		{
			float value = (float)base.PropertyItem.Values[0];
			if (PropertyItem.Objects.Count <= 1)
			{
				object firstObject = PropertyItem.FirstObject;
				this.innerEntry.Value = value;
				return;
			}
			Func<UserCameraObject, UserCameraObject, bool> func = (UserCameraObject a, UserCameraObject b) => a.Fov == b.Fov;
			if (base.IsWhipNode<UserCameraObject>(func))
			{
				this.innerEntry.SetToSubState();
				return;
			}
			this.innerEntry.Value = value;
		}
	}
}
