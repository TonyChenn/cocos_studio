using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x0200001C RID: 28
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaImageViewObject : LuaWidgetObject
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x0000594E File Offset: 0x00003B4E
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005968 File Offset: 0x00003B68
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(ImageViewObjectData) == objectData.GetType();
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000598C File Offset: 0x00003B8C
		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccui.ImageView:create()\r\n");
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000059B8 File Offset: 0x00003BB8
		public override void InitializeObject(BaseObjectData objectData)
		{
			ImageViewObjectData imageViewObjectData = objectData as ImageViewObjectData;
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(imageViewObjectData.Name)));
			base.Write(":ignoreContentAdaptWithSize(false)\r\n");
			if (imageViewObjectData.FileData != null)
			{
				base.PreloadPlist(imageViewObjectData.FileData);
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(imageViewObjectData.Name)));
				base.Write(":loadTexture(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(imageViewObjectData.FileData.Path)));
				base.Write("\",");
				base.Write(base.ToStringHelper.ToStringWithCulture(imageViewObjectData.FileData.Type.ToLuaType()));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(imageViewObjectData.FlipX, false))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(imageViewObjectData.Name)));
				base.Write(":setFlippedX(");
				base.Write(base.ToStringHelper.ToStringWithCulture(imageViewObjectData.FlipX));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(imageViewObjectData.FlipY, false))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(imageViewObjectData.Name)));
				base.Write(":setFlippedY(");
				base.Write(base.ToStringHelper.ToStringWithCulture(imageViewObjectData.FlipY));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(imageViewObjectData.Scale9Enable, false))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(imageViewObjectData.Name)));
				base.Write(":setScale9Enabled(");
				base.Write(base.ToStringHelper.ToStringWithCulture(imageViewObjectData.Scale9Enable));
				base.Write(")\r\n");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(imageViewObjectData.Name)));
				base.Write(":setCapInsets(cc.rect(");
				base.Write(base.ToStringHelper.ToStringWithCulture(imageViewObjectData.Scale9OriginX));
				base.Write(",");
				base.Write(base.ToStringHelper.ToStringWithCulture(imageViewObjectData.Scale9OriginY));
				base.Write(",");
				base.Write(base.ToStringHelper.ToStringWithCulture(imageViewObjectData.Scale9Width));
				base.Write(",");
				base.Write(base.ToStringHelper.ToStringWithCulture(imageViewObjectData.Scale9Height));
				base.Write("))\r\n");
			}
			base.InitializeObject(imageViewObjectData);
		}
	}
}
