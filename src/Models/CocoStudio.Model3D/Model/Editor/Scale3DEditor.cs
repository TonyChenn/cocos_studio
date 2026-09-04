using System;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000014 RID: 20
	internal class Scale3DEditor : ThreeNumberEditor
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x000039E8 File Offset: 0x00001BE8
		protected override void OnSetControl()
		{
			Func<Node3DObject, Node3DObject, bool> funcX = (Node3DObject a, Node3DObject b) => a.Scale3D.X == b.Scale3D.X;
			Func<Node3DObject, Node3DObject, bool> funcY = (Node3DObject a, Node3DObject b) => a.Scale3D.Y == b.Scale3D.Y;
			Func<Node3DObject, Node3DObject, bool> funcZ = (Node3DObject a, Node3DObject b) => a.Scale3D.Z == b.Scale3D.Z;
			base.CompareNumber(funcX, funcY, funcZ);
		}
	}
}
