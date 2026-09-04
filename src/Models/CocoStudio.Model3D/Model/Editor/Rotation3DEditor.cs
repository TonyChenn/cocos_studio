using System;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000013 RID: 19
	internal class Rotation3DEditor : ThreeNumberEditor
	{
		// Token: 0x060000AB RID: 171 RVA: 0x00003920 File Offset: 0x00001B20
		protected override void OnSetControl()
		{
			Func<Node3DObject, Node3DObject, bool> funcX = (Node3DObject a, Node3DObject b) => a.Rotation3D.X == b.Rotation3D.X;
			Func<Node3DObject, Node3DObject, bool> funcY = (Node3DObject a, Node3DObject b) => a.Rotation3D.Y == b.Rotation3D.Y;
			Func<Node3DObject, Node3DObject, bool> funcZ = (Node3DObject a, Node3DObject b) => a.Rotation3D.Z == b.Rotation3D.Z;
			base.CompareNumber(funcX, funcY, funcZ);
		}
	}
}
