using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaSpriteObject : LuaNodeObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(SpriteObjectData) == objectData.GetType();
		}

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
