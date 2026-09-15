using System;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model.Editor
{
	internal class Rotation3DEditor : ThreeNumberEditor
	{
		protected override void OnSetControl()
		{
			Func<Node3DObject, Node3DObject, bool> funcX = (Node3DObject a, Node3DObject b) => a.Rotation3D.X == b.Rotation3D.X;
			Func<Node3DObject, Node3DObject, bool> funcY = (Node3DObject a, Node3DObject b) => a.Rotation3D.Y == b.Rotation3D.Y;
			Func<Node3DObject, Node3DObject, bool> funcZ = (Node3DObject a, Node3DObject b) => a.Rotation3D.Z == b.Rotation3D.Z;
			base.CompareNumber(funcX, funcY, funcZ);
		}
	}
}
