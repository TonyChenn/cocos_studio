using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x0200002C RID: 44
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaTextFieldObject : LuaWidgetObject
	{
		// Token: 0x06000105 RID: 261 RVA: 0x000078E1 File Offset: 0x00005AE1
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000078FC File Offset: 0x00005AFC
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(TextFieldObjectData) == objectData.GetType();
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00007920 File Offset: 0x00005B20
		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccui.TextField:create()\r\n");
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000794C File Offset: 0x00005B4C
		public override void InitializeObject(BaseObjectData objectData)
		{
			TextFieldObjectData textFieldObjectData = objectData as TextFieldObjectData;
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textFieldObjectData.Name)));
			base.Write(":ignoreContentAdaptWithSize(false)\r\n");
			if (textFieldObjectData.FontResource != null)
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textFieldObjectData.Name)));
				base.Write(":setFontName(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(textFieldObjectData.FontResource.Path)));
				base.Write("\")\r\n");
			}
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textFieldObjectData.Name)));
			base.Write(":setFontSize(");
			base.Write(base.ToStringHelper.ToStringWithCulture(textFieldObjectData.FontSize));
			base.Write(")\r\n");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textFieldObjectData.Name)));
			base.Write(":setPlaceHolder(\"");
			base.Write(base.ToStringHelper.ToStringWithCulture(textFieldObjectData.PlaceHolderText));
			base.Write("\")\r\n");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textFieldObjectData.Name)));
			base.Write(":setString(");
			base.Write(base.ToStringHelper.ToStringWithCulture(LuaWidgetObject.GetLabelTextString(textFieldObjectData.LabelText)));
			base.Write(")\r\n");
			if (base.CanExport<bool>(textFieldObjectData.MaxLengthEnable, false))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textFieldObjectData.Name)));
				base.Write(":setMaxLengthEnabled(");
				base.Write(base.ToStringHelper.ToStringWithCulture(textFieldObjectData.MaxLengthEnable));
				base.Write(")\r\n");
			}
			if (base.CanExport<int>(textFieldObjectData.MaxLengthText, 0))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textFieldObjectData.Name)));
				base.Write(":setMaxLength(");
				base.Write(base.ToStringHelper.ToStringWithCulture(textFieldObjectData.MaxLengthText));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(textFieldObjectData.PasswordEnable, false))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textFieldObjectData.Name)));
				base.Write(":setPasswordEnabled(");
				base.Write(base.ToStringHelper.ToStringWithCulture(textFieldObjectData.PasswordEnable));
				base.Write(")\r\n");
			}
			if (base.CanExport<string>(textFieldObjectData.PasswordStyleText, "*"))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textFieldObjectData.Name)));
				base.Write(":setPasswordStyleText(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(textFieldObjectData.PasswordStyleText));
				base.Write("\")\r\n");
			}
			base.InitializeObject(textFieldObjectData);
		}
	}
}
