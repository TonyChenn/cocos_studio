using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000028 RID: 40
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaSpriteObject : LuaNodeObject
	{
		// Token: 0x060000F1 RID: 241 RVA: 0x00007310 File Offset: 0x00005510
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00007328 File Offset: 0x00005528
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(SpriteObjectData) == objectData.GetType();
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000734C File Offset: 0x0000554C
		protected override void OnCreateObject(BaseObjectData objectData)
		{
			SpriteObjectData spriteObjectData = objectData as SpriteObjectData;
			if (spriteObjectData.FileData == null)
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
				base.Write(" = cc.Sprite:create()\r\n");
			}
			else
			{
				base.PreloadPlist(spriteObjectData.FileData);
			}
			if (spriteObjectData.FileData.Type == EnumResourceType.PlistSubImage || spriteObjectData.FileData.Type == EnumResourceType.MarkedSubImage)
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
				base.Write(" = cc.Sprite:createWithSpriteFrameName(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(spriteObjectData.FileData.Path)));
				base.Write("\")\r\n");
				return;
			}
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = cc.Sprite:create(\"");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(spriteObjectData.FileData.Path)));
			base.Write("\")\r\n");
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000746C File Offset: 0x0000566C
		public override void InitializeObject(BaseObjectData objectData)
		{
			SpriteObjectData spriteObjectData = objectData as SpriteObjectData;
			base.InitializeObject(spriteObjectData);
			if (spriteObjectData == null)
			{
				return;
			}
			if (base.CanExport<bool>(spriteObjectData.FlipX, false))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(spriteObjectData.Name)));
				base.Write(":setFlippedX(");
				base.Write(base.ToStringHelper.ToStringWithCulture(spriteObjectData.FlipX));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(spriteObjectData.FlipY, false))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(spriteObjectData.Name)));
				base.Write(":setFlippedY(");
				base.Write(base.ToStringHelper.ToStringWithCulture(spriteObjectData.FlipY));
				base.Write(")\r\n");
			}
		}
	}
}
