using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaGameMapObject : LuaNodeObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(GameMapObjectData) == objectData.GetType();
		}

		protected override void OnCreateObject(BaseObjectData objectData)
		{
			GameMapObjectData gameMapObjectData = objectData as GameMapObjectData;
			if (gameMapObjectData.FileData != null)
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
				base.Write(" = cc.TMXTiledMap:create(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(gameMapObjectData.FileData.Path)));
				base.Write("\")\r\n");
				return;
			}
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = cc.TMXTiledMap:create(\"\")\r\n");
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			base.InitializeObject(objectData);
		}
	}
}
