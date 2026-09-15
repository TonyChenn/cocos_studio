using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Lua;

namespace CocoStudio.Model3D.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaUserCameraObject : LuaNode3DObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(UserCameraObjectData) == objectData.GetType();
		}

		protected override void OnCreateObject(BaseObjectData objectData)
		{
			UserCameraObjectData userCameraObjectData = objectData as UserCameraObjectData;
			if (userCameraObjectData.ViewSize != null)
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
				base.Write(" = cc.Camera:createPerspective(");
				base.Write(base.ToStringHelper.ToStringWithCulture(userCameraObjectData.Fov));
				base.Write(", ");
				base.Write(base.ToStringHelper.ToStringWithCulture(userCameraObjectData.ViewSize.Width / userCameraObjectData.ViewSize.Height));
				base.Write(", ");
				base.Write(base.ToStringHelper.ToStringWithCulture(userCameraObjectData.ClipPlane.X));
				base.Write(", ");
				base.Write(base.ToStringHelper.ToStringWithCulture(userCameraObjectData.ClipPlane.Y));
				base.Write(")\r\n");
			}
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			UserCameraObjectData userCameraObjectData = objectData as UserCameraObjectData;
			base.InitializeObject(userCameraObjectData);
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(userCameraObjectData.Name)));
			base.Write(":setCameraFlag(");
			base.Write(base.ToStringHelper.ToStringWithCulture((int)userCameraObjectData.UserCameraFlagMode));
			base.Write(")\r\n");
			if (userCameraObjectData.SkyBoxEnabled && userCameraObjectData.SkyBoxValid)
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(userCameraObjectData.Name + "_Brush")));
				base.Write("=cc.CameraBackgroundSkyBoxBrush:create(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(userCameraObjectData.LeftImage));
				base.Write("\", \"");
				base.Write(base.ToStringHelper.ToStringWithCulture(userCameraObjectData.RightImage));
				base.Write("\", \"");
				base.Write(base.ToStringHelper.ToStringWithCulture(userCameraObjectData.UpImage));
				base.Write("\", \"");
				base.Write(base.ToStringHelper.ToStringWithCulture(userCameraObjectData.DownImage));
				base.Write("\", \"");
				base.Write(base.ToStringHelper.ToStringWithCulture(userCameraObjectData.ForwardImage));
				base.Write("\", \"");
				base.Write(base.ToStringHelper.ToStringWithCulture(userCameraObjectData.BackImage));
				base.Write("\")\r\n");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(userCameraObjectData.Name)));
				base.Write(":setBackgroundBrush(");
				base.Write(base.ToStringHelper.ToStringWithCulture(userCameraObjectData.Name + "_Brush"));
				base.Write(")\r\n");
				return;
			}
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(userCameraObjectData.Name)));
			base.Write(":setBackgroundBrush(");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(LuaObjectSerializer.GameFileData.ObjectData.Name + "_SceneBrush")));
			base.Write(")\r\n");
		}
	}
}
