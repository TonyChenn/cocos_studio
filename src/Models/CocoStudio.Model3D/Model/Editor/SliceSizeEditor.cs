using System;
using CocoStudio.Model.ViewModel;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000016 RID: 22
	internal class SliceSizeEditor : TwoNumberEditor
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x00003F5E File Offset: 0x0000215E
		public SliceSizeEditor() : base(LanguageInfo.Display_ResourceWidth, LanguageInfo.Display_ResourceHeight)
		{
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00003FA4 File Offset: 0x000021A4
		protected override void OnSetControl()
		{
			Func<Slice3DObject, Slice3DObject, bool> funcX = (Slice3DObject a, Slice3DObject b) => a.SliceSize.Width == b.SliceSize.Width;
			Func<Slice3DObject, Slice3DObject, bool> funcY = (Slice3DObject a, Slice3DObject b) => a.SliceSize.Height == b.SliceSize.Height;
			base.CompareNumber(funcX, funcY);
		}
	}
}
