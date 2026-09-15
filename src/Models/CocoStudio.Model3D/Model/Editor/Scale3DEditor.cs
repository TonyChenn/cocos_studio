using System;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model.Editor
{
	internal class Scale3DEditor : ThreeNumberEditor
	{
		protected override void OnSetControl()
		{
			Func<Node3DObject, Node3DObject, bool> funcX = (Node3DObject a, Node3DObject b) => a.Scale3D.X == b.Scale3D.X;
			Func<Node3DObject, Node3DObject, bool> funcY = (Node3DObject a, Node3DObject b) => a.Scale3D.Y == b.Scale3D.Y;
			Func<Node3DObject, Node3DObject, bool> funcZ = (Node3DObject a, Node3DObject b) => a.Scale3D.Z == b.Scale3D.Z;
			base.CompareNumber(funcX, funcY, funcZ);
		}
	}
}
