using System;
using System.CodeDom.Compiler;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Lua.Templates;

namespace CocoStudio.Model3D.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaNode3DObject : LuaNodeObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(Node3DObjectData) == objectData.GetType();
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			Node3DObjectData node3DObjectData = objectData as Node3DObjectData;
			base.InitializeObject(node3DObjectData);
			if (base.CanExport<float>(node3DObjectData.Position3D.X, 0f) || base.CanExport<float>(node3DObjectData.Position3D.Y, 0f))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(node3DObjectData.Name)));
				base.Write(":setPosition(");
				base.Write(base.ToStringHelper.ToStringWithCulture(node3DObjectData.Position3D.X));
				base.Write(",");
				base.Write(base.ToStringHelper.ToStringWithCulture(node3DObjectData.Position3D.Y));
				base.Write(")\r\n");
			}
			if (base.CanExport<float>(node3DObjectData.Position3D.Z, 0f))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(node3DObjectData.Name)));
				base.Write(":setPositionZ(");
				base.Write(base.ToStringHelper.ToStringWithCulture(node3DObjectData.Position3D.Z));
				base.Write(")\r\n");
			}
			if (node3DObjectData.Scale3D != null && base.CanExport<float>(node3DObjectData.Scale3D.X, 1f))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(node3DObjectData.Name)));
				base.Write(":setScaleX(");
				base.Write(base.ToStringHelper.ToStringWithCulture(node3DObjectData.Scale3D.X));
				base.Write(")\r\n");
			}
			if (node3DObjectData.Scale3D != null && base.CanExport<float>(node3DObjectData.Scale3D.Y, 1f))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(node3DObjectData.Name)));
				base.Write(":setScaleY(");
				base.Write(base.ToStringHelper.ToStringWithCulture(node3DObjectData.Scale3D.Y));
				base.Write(")\r\n");
			}
			if (node3DObjectData.Scale3D != null && base.CanExport<float>(node3DObjectData.Scale3D.Z, 1f))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(node3DObjectData.Name)));
				base.Write(":setScaleZ(");
				base.Write(base.ToStringHelper.ToStringWithCulture(node3DObjectData.Scale3D.Z));
				base.Write(")\r\n");
			}
			if (node3DObjectData.Rotation3D != null && base.CanExport<Point3F>(node3DObjectData.Rotation3D, Point3F.Empty))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(node3DObjectData.Name)));
				base.Write(":setRotation3D(cc.vec3(");
				base.Write(base.ToStringHelper.ToStringWithCulture(node3DObjectData.Rotation3D.X));
				base.Write(", ");
				base.Write(base.ToStringHelper.ToStringWithCulture(node3DObjectData.Rotation3D.Y));
				base.Write(", ");
				base.Write(base.ToStringHelper.ToStringWithCulture(node3DObjectData.Rotation3D.Z));
				base.Write("))\r\n");
			}
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(node3DObjectData.Name)));
			base.Write(":setCameraMask(");
			base.Write(base.ToStringHelper.ToStringWithCulture(node3DObjectData.CameraFlagMode));
			base.Write(")\r\n");
		}
	}
}
