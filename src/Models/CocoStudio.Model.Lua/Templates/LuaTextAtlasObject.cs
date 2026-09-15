using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaTextAtlasObject : LuaWidgetObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(TextAtlasObjectData) == objectData.GetType();
		}

		protected override void OnCreateObject(BaseObjectData objectData)
		{
			TextAtlasObjectData textAtlasObjectData = objectData as TextAtlasObjectData;
			if (textAtlasObjectData.LabelAtlasFileImage_CNB != null)
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
				base.Write(" = ccui.TextAtlas:create(");
				base.Write(base.ToStringHelper.ToStringWithCulture(LuaWidgetObject.GetLabelTextString(textAtlasObjectData.LabelText)));
				base.Write(",\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(textAtlasObjectData.LabelAtlasFileImage_CNB.Path)));
				base.Write("\",\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t");
				base.Write(base.ToStringHelper.ToStringWithCulture(textAtlasObjectData.CharWidth));
				base.Write(",\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t");
				base.Write(base.ToStringHelper.ToStringWithCulture(textAtlasObjectData.CharHeight));
				base.Write(",\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(textAtlasObjectData.StartChar));
				base.Write("\")\r\n");
				return;
			}
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccui.TextAtlas:create()\r\n");
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			base.InitializeObject(objectData);
		}
	}
}
