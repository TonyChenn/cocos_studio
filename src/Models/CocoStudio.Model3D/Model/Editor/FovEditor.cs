using System;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class FovEditor : OneNumberEditor
	{
		public FovEditor() : base("")
		{
		}

		protected override Widget OnCreateWidget()
		{
			Widget result = base.OnCreateWidget();
			this.innerEntry.MaxValue = 179;
			this.innerEntry.MinValue = 1;
			return result;
		}

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
