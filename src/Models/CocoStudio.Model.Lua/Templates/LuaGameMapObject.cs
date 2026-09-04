using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000019 RID: 25
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaGameMapObject : LuaNodeObject
	{
		// Token: 0x0600009E RID: 158 RVA: 0x000054A4 File Offset: 0x000036A4
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000054BC File Offset: 0x000036BC
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(GameMapObjectData) == objectData.GetType();
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000054E0 File Offset: 0x000036E0
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

		// Token: 0x060000A1 RID: 161 RVA: 0x00005580 File Offset: 0x00003780
		public override void InitializeObject(BaseObjectData objectData)
		{
			base.InitializeObject(objectData);
		}
	}
}
