using System;
using CocoStudio.Model.ViewModel;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200000C RID: 12
	internal class AnimationSpeedEditor : TwoNumberEditor
	{
		// Token: 0x06000080 RID: 128 RVA: 0x00002BA5 File Offset: 0x00000DA5
		public AnimationSpeedEditor() : base(LanguageInfo.Display_USpeed, LanguageInfo.Display_VSpeed)
		{
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00002BEC File Offset: 0x00000DEC
		protected override void OnSetControl()
		{
			Func<Slice3DObject, Slice3DObject, bool> funcX = (Slice3DObject a, Slice3DObject b) => a.AnimationSpeed.X == b.AnimationSpeed.X;
			Func<Slice3DObject, Slice3DObject, bool> funcY = (Slice3DObject a, Slice3DObject b) => a.AnimationSpeed.Y == b.AnimationSpeed.Y;
			base.CompareNumber(funcX, funcY);
		}
	}
}
