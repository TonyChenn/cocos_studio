using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Lua.Templates;

namespace CocoStudio.Model3D.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaGameNode3DObject : LuaGameNodeObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(GameNode3DObjectData) == objectData.GetType();
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			base.InitializeObject(objectData);
			GameNode3DObjectData gameNode3DObjectData = objectData as GameNode3DObjectData;
			if (gameNode3DObjectData == null)
			{
				return;
			}
			if (gameNode3DObjectData.SkyBoxEnabled && gameNode3DObjectData.SkyBoxValid)
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(gameNode3DObjectData.Name + "_SceneBrush")));
				base.Write("=cc.CameraBackgroundSkyBoxBrush:create(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(gameNode3DObjectData.LeftImage));
				base.Write("\", \"");
				base.Write(base.ToStringHelper.ToStringWithCulture(gameNode3DObjectData.RightImage));
				base.Write("\", \"");
				base.Write(base.ToStringHelper.ToStringWithCulture(gameNode3DObjectData.UpImage));
				base.Write("\", \"");
				base.Write(base.ToStringHelper.ToStringWithCulture(gameNode3DObjectData.DownImage));
				base.Write("\", \"");
				base.Write(base.ToStringHelper.ToStringWithCulture(gameNode3DObjectData.ForwardImage));
				base.Write("\", \"");
				base.Write(base.ToStringHelper.ToStringWithCulture(gameNode3DObjectData.BackImage));
				base.Write("\")\r\n");
			}
		}
	}
}
