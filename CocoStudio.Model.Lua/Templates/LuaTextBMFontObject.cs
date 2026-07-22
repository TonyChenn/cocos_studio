using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x0200002B RID: 43
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaTextBMFontObject : LuaWidgetObject
	{
		// Token: 0x06000100 RID: 256 RVA: 0x000077A3 File Offset: 0x000059A3
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000077BC File Offset: 0x000059BC
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(TextBMFontObjectData) == objectData.GetType();
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000077E0 File Offset: 0x000059E0
		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccui.TextBMFont:create()\r\n");
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000780C File Offset: 0x00005A0C
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
