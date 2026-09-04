using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Lua.Templates;

namespace CocoStudio.Model3D.Lua.Templates
{
	// Token: 0x02000007 RID: 7
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaGameNode3DObject : LuaGameNodeObject
	{
		// Token: 0x06000018 RID: 24 RVA: 0x00002D72 File Offset: 0x00000F72
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002D8A File Offset: 0x00000F8A
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(GameNode3DObjectData) == objectData.GetType();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002DA4 File Offset: 0x00000FA4
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
