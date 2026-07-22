using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000015 RID: 21
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaButtonObject : LuaWidgetObject
	{
		// Token: 0x0600008A RID: 138 RVA: 0x0000484C File Offset: 0x00002A4C
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004864 File Offset: 0x00002A64
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(ButtonObjectData) == objectData.GetType();
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004888 File Offset: 0x00002A88
		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccui.Button:create()\r\n");
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000048B4 File Offset: 0x00002AB4
		public override void InitializeObject(BaseObjectData objectData)
		{
			ButtonObjectData buttonObjectData = objectData as ButtonObjectData;
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(buttonObjectData.Name)));
			base.Write(":ignoreContentAdaptWithSize(false)\r\n");
			if (buttonObjectData.NormalFileData != null)
			{
				base.PreloadPlist(buttonObjectData.NormalFileData);
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(buttonObjectData.Name)));
				base.Write(":loadTextureNormal(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(buttonObjectData.NormalFileData.Path)));
				base.Write("\",");
				base.Write(base.ToStringHelper.ToStringWithCulture(buttonObjectData.NormalFileData.Type.ToLuaType()));
				base.Write(")\r\n");
			}
			if (buttonObjectData.PressedFileData != null)
			{
				base.PreloadPlist(buttonObjectData.PressedFileData);
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(buttonObjectData.Name)));
				base.Write(":loadTexturePressed(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(buttonObjectData.PressedFileData.Path)));
				base.Write("\",");
				base.Write(base.ToStringHelper.ToStringWithCulture(buttonObjectData.PressedFileData.Type.ToLuaType()));
				base.Write(")\r\n");
			}
			if (buttonObjectData.DisabledFileData != null)
			{
				base.PreloadPlist(buttonObjectData.DisabledFileData);
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(buttonObjectData.Name)));
				base.Write(":loadTextureDisabled(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(buttonObjectData.DisabledFileData.Path)));
				base.Write("\",");
				base.Write(base.ToStringHelper.ToStringWithCulture(buttonObjectData.DisabledFileData.Type.ToLuaType()));
				base.Write(")\r\n");
			}
			if (buttonObjectData.FontResource != null)
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(buttonObjectData.Name)));
				base.Write(":setTitleFontName(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(buttonObjectData.FontResource.Path)));
				base.Write("\")\r\n");
			}
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(buttonObjectData.Name)));
			base.Write(":setTitleFontSize(");
			base.Write(base.ToStringHelper.ToStringWithCulture(buttonObjectData.FontSize));
			base.Write(")\r\n");
			if (buttonObjectData.ButtonText != null && base.CanExport<string>(buttonObjectData.ButtonText, ""))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(buttonObjectData.Name)));
				base.Write(":setTitleText(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(buttonObjectData.ButtonText));
				base.Write("\")\r\n");
			}
			if (base.CanExport<ColorData>(buttonObjectData.TextColor, ColorData.White))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(buttonObjectData.Name)));
				base.Write(":setTitleColor(");
				base.Write(base.ToStringHelper.ToStringWithCulture(buttonObjectData.TextColor));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(buttonObjectData.FlipX, false))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(buttonObjectData.Name)));
				base.Write(":setFlippedX(");
				base.Write(base.ToStringHelper.ToStringWithCulture(buttonObjectData.FlipX));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(buttonObjectData.FlipY, false))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(buttonObjectData.Name)));
				base.Write(":setFlippedY(");
				base.Write(base.ToStringHelper.ToStringWithCulture(buttonObjectData.FlipY));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(buttonObjectData.Scale9Enable, false))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(buttonObjectData.Name)));
				base.Write(":setScale9Enabled(");
				base.Write(base.ToStringHelper.ToStringWithCulture(buttonObjectData.Scale9Enable));
				base.Write(")\r\n");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(buttonObjectData.Name)));
				base.Write(":setCapInsets(cc.rect(");
				base.Write(base.ToStringHelper.ToStringWithCulture(buttonObjectData.Scale9OriginX));
				base.Write(",");
				base.Write(base.ToStringHelper.ToStringWithCulture(buttonObjectData.Scale9OriginY));
				base.Write(",");
				base.Write(base.ToStringHelper.ToStringWithCulture(buttonObjectData.Scale9Width));
				base.Write(",");
				base.Write(base.ToStringHelper.ToStringWithCulture(buttonObjectData.Scale9Height));
				base.Write("))\r\n");
			}
			if (base.CanExport<bool>(buttonObjectData.DisplayState, true))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(buttonObjectData.Name)));
				base.Write(":setBright(");
				base.Write(base.ToStringHelper.ToStringWithCulture(buttonObjectData.DisplayState));
				base.Write(")\r\n");
			}
			base.InitializeObject(buttonObjectData);
		}
	}
}
