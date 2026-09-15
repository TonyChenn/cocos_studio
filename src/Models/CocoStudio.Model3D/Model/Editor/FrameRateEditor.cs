using System;
using CocoStudio.Model.ViewModel;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class FrameRateEditor : OneNumberEditor
	{
		public FrameRateEditor() : base(LanguageInfo.Display_FramesPerSecond)
		{
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
			Func<Slice3DObject, Slice3DObject, bool> func = (Slice3DObject a, Slice3DObject b) => a.framerate == b.framerate;
			if (base.IsWhipNode<Slice3DObject>(func))
			{
				this.innerEntry.SetToSubState();
				return;
			}
			this.innerEntry.Value = value;
		}
	}
}
