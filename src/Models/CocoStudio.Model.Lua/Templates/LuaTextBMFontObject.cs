using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaTextBMFontObject : LuaWidgetObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(TextBMFontObjectData) == objectData.GetType();
		}

		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccui.TextBMFont:create()\r\n");
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			TextBMFontObjectData textBMFontObjectData = objectData as TextBMFontObjectData;
			if (textBMFontObjectData.LabelBMFontFile_CNB != null)
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textBMFontObjectData.Name)));
				base.Write(":setFntFile(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(textBMFontObjectData.LabelBMFontFile_CNB.Path)));
				base.Write("\")\r\n");
			}
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(textBMFontObjectData.Name)));
			base.Write(":setString(");
			base.Write(base.ToStringHelper.ToStringWithCulture(LuaWidgetObject.GetLabelTextString(textBMFontObjectData.LabelText)));
			base.Write(")\r\n");
			base.InitializeObject(textBMFontObjectData);
		}
	}
}
