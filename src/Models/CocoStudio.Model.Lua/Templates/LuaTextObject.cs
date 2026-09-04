using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x0200002D RID: 45
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaTextObject : LuaWidgetObject
	{
		// Token: 0x0600010A RID: 266 RVA: 0x00007C59 File Offset: 0x00005E59
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00007C74 File Offset: 0x00005E74
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(TextObjectData) == objectData.GetType();
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00007C98 File Offset: 0x00005E98
		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccui.Text:create()\r\n");
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00007CC4 File Offset: 0x00005EC4
		public override void InitializeObject(BaseObjectData objectData)
		{
			TextObjectData textObjectData = objectData as TextObjectData;
			if (!textObjectData.IsCustomSize)
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textObjectData.Name)));
				base.Write(":ignoreContentAdaptWithSize(true)\r\n");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textObjectData.Name)));
				base.Write(":setTextAreaSize(cc.size(0, 0))\r\n");
			}
			else
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textObjectData.Name)));
				base.Write(":ignoreContentAdaptWithSize(false)\r\n");
			}
			if (textObjectData.FontResource != null)
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textObjectData.Name)));
				base.Write(":setFontName(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(textObjectData.FontResource.Path)));
				base.Write("\")\r\n");
			}
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textObjectData.Name)));
			base.Write(":setFontSize(");
			base.Write(base.ToStringHelper.ToStringWithCulture(textObjectData.FontSize));
			base.Write(")\r\n");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textObjectData.Name)));
			base.Write(":setString(");
			base.Write(base.ToStringHelper.ToStringWithCulture(LuaWidgetObject.GetLabelTextString(textObjectData.LabelText)));
			base.Write(")\r\n");
			if (base.CanExport<TextHorizontalType>(textObjectData.HorizontalAlignmentType, TextHorizontalType.HT_Left))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textObjectData.Name)));
				base.Write(":setTextHorizontalAlignment(");
				base.Write(base.ToStringHelper.ToStringWithCulture((int)textObjectData.HorizontalAlignmentType));
				base.Write(")\r\n");
			}
			if (base.CanExport<TextVerticalType>(textObjectData.VerticalAlignmentType, TextVerticalType.VT_Top))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textObjectData.Name)));
				base.Write(":setTextVerticalAlignment(");
				base.Write(base.ToStringHelper.ToStringWithCulture((int)textObjectData.VerticalAlignmentType));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(textObjectData.TouchScaleChangeAble, false))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textObjectData.Name)));
				base.Write(":setTouchScaleChangeEnabled(");
				base.Write(base.ToStringHelper.ToStringWithCulture(textObjectData.TouchScaleChangeAble));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(textObjectData.FlipX, false))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textObjectData.Name)));
				base.Write(":setFlippedX(");
				base.Write(base.ToStringHelper.ToStringWithCulture(textObjectData.FlipX));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(textObjectData.FlipY, false))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textObjectData.Name)));
				base.Write(":setFlippedY(");
				base.Write(base.ToStringHelper.ToStringWithCulture(textObjectData.FlipY));
				base.Write(")\r\n");
			}
			if (textObjectData.ShadowEnabled)
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textObjectData.Name)));
				base.Write(":enableShadow(cc.c4b(");
				base.Write(base.ToStringHelper.ToStringWithCulture(textObjectData.ShadowColor.R));
				base.Write(", ");
				base.Write(base.ToStringHelper.ToStringWithCulture(textObjectData.ShadowColor.G));
				base.Write(", ");
				base.Write(base.ToStringHelper.ToStringWithCulture(textObjectData.ShadowColor.B));
				base.Write(", ");
				base.Write(base.ToStringHelper.ToStringWithCulture(textObjectData.ShadowColor.A));
				base.Write("), cc.size(");
				base.Write(base.ToStringHelper.ToStringWithCulture(textObjectData.ShadowOffsetX));
				base.Write(", ");
				base.Write(base.ToStringHelper.ToStringWithCulture(textObjectData.ShadowOffsetY));
				base.Write("), ");
				base.Write(base.ToStringHelper.ToStringWithCulture(textObjectData.ShadowBlurRadius));
				base.Write(")\r\n");
			}
			if (textObjectData.OutlineEnabled)
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textObjectData.Name)));
				base.Write(":enableOutline(cc.c4b(");
				base.Write(base.ToStringHelper.ToStringWithCulture(textObjectData.OutlineColor.R));
				base.Write(", ");
				base.Write(base.ToStringHelper.ToStringWithCulture(textObjectData.OutlineColor.G));
				base.Write(", ");
				base.Write(base.ToStringHelper.ToStringWithCulture(textObjectData.OutlineColor.B));
				base.Write(", ");
				base.Write(base.ToStringHelper.ToStringWithCulture(textObjectData.OutlineColor.A));
				base.Write("), ");
				base.Write(base.ToStringHelper.ToStringWithCulture(textObjectData.OutlineSize));
				base.Write(")\r\n");
			}
			base.InitializeObject(textObjectData);
		}
	}
}
