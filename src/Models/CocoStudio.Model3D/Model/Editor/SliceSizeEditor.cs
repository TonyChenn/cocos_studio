using System;
using CocoStudio.Model.ViewModel;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model.Editor
{
	internal class SliceSizeEditor : TwoNumberEditor
	{
		public SliceSizeEditor() : base(LanguageInfo.Display_ResourceWidth, LanguageInfo.Display_ResourceHeight)
		{
		}

		protected override void OnSetControl()
		{
			Func<Slice3DObject, Slice3DObject, bool> funcX = (Slice3DObject a, Slice3DObject b) => a.SliceSize.Width == b.SliceSize.Width;
			Func<Slice3DObject, Slice3DObject, bool> funcY = (Slice3DObject a, Slice3DObject b) => a.SliceSize.Height == b.SliceSize.Height;
			base.CompareNumber(funcX, funcY);
		}
	}
}
