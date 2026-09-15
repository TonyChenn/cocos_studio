using System;
using CocoStudio.Model.ViewModel;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model.Editor
{
	internal class TextureAnimationEditor : TwoNumberEditor
	{
		public TextureAnimationEditor() : base(LanguageInfo.Display_RowNumber, LanguageInfo.Display_ColumnNumber)
		{
		}

		protected override void OnSetControl()
		{
			Func<Slice3DObject, Slice3DObject, bool> funcX = (Slice3DObject a, Slice3DObject b) => a.TextureAnimation.X == b.TextureAnimation.X;
			Func<Slice3DObject, Slice3DObject, bool> funcY = (Slice3DObject a, Slice3DObject b) => a.TextureAnimation.Y == b.TextureAnimation.Y;
			base.CompareNumber(funcX, funcY);
		}
	}
}
