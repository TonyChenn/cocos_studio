using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x0200002A RID: 42
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaTextAtlasObject : LuaWidgetObject
	{
		// Token: 0x060000FB RID: 251 RVA: 0x0000761B File Offset: 0x0000581B
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00007634 File Offset: 0x00005834
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(TextAtlasObjectData) == objectData.GetType();
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00007658 File Offset: 0x00005858
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

		// Token: 0x060000FE RID: 254 RVA: 0x00007792 File Offset: 0x00005992
		public override void InitializeObject(BaseObjectData objectData)
		{
			base.InitializeObject(objectData);
		}
	}
}
