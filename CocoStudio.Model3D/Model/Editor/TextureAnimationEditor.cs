using System;
using CocoStudio.Model.ViewModel;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000017 RID: 23
	internal class TextureAnimationEditor : TwoNumberEditor
	{
		// Token: 0x060000C5 RID: 197 RVA: 0x00003FF5 File Offset: 0x000021F5
		public TextureAnimationEditor() : base(LanguageInfo.Display_RowNumber, LanguageInfo.Display_ColumnNumber)
		{
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000403C File Offset: 0x0000223C
		protected override void OnSetControl()
		{
			Func<Slice3DObject, Slice3DObject, bool> funcX = (Slice3DObject a, Slice3DObject b) => a.TextureAnimation.X == b.TextureAnimation.X;
			Func<Slice3DObject, Slice3DObject, bool> funcY = (Slice3DObject a, Slice3DObject b) => a.TextureAnimation.Y == b.TextureAnimation.Y;
			base.CompareNumber(funcX, funcY);
		}
	}
}
