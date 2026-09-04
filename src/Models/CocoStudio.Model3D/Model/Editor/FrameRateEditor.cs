using System;
using CocoStudio.Model.ViewModel;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000010 RID: 16
	internal class FrameRateEditor : OneNumberEditor
	{
		// Token: 0x06000094 RID: 148 RVA: 0x0000311F File Offset: 0x0000131F
		public FrameRateEditor() : base(LanguageInfo.Display_FramesPerSecond)
		{
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000313C File Offset: 0x0000133C
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
