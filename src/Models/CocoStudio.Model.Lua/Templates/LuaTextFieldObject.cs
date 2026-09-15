using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaTextFieldObject : LuaWidgetObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(TextFieldObjectData) == objectData.GetType();
		}

		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccui.TextField:create()\r\n");
		}

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
