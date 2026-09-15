using System;
using CocoStudio.Model.ViewModel;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model.Editor
{
	internal class AnimationSpeedEditor : TwoNumberEditor
	{
		public AnimationSpeedEditor() : base(LanguageInfo.Display_USpeed, LanguageInfo.Display_VSpeed)
		{
		}

		protected override void OnSetControl()
		{
			Func<Slice3DObject, Slice3DObject, bool> funcX = (Slice3DObject a, Slice3DObject b) => a.AnimationSpeed.X == b.AnimationSpeed.X;
			Func<Slice3DObject, Slice3DObject, bool> funcY = (Slice3DObject a, Slice3DObject b) => a.AnimationSpeed.Y == b.AnimationSpeed.Y;
			base.CompareNumber(funcX, funcY);
		}
	}
}
