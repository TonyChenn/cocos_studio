using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x0200001D RID: 29
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaInnerActionFrame : LuaFrame
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x00005C87 File Offset: 0x00003E87
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005C9F File Offset: 0x00003E9F
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(InnerActionFrameData) == objectData.GetType();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005CB6 File Offset: 0x00003EB6
		public override void CreateObject(BaseObjectData objectData)
		{
			base.Write("ccs.InnerActionFrame:create()\r\n");
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005CC4 File Offset: 0x00003EC4
		public override void InitializeObject(BaseObjectData objectData)
		{
			InnerActionFrameData innerActionFrameData = objectData as InnerActionFrameData;
			base.InitializeObject(innerActionFrameData);
			string objectToConvert = innerActionFrameData.CurrentAniamtionName ?? string.Empty;
			base.Write("localFrame:setInnerActionType(");
			base.Write(base.ToStringHelper.ToStringWithCulture((int)innerActionFrameData.InnerActionType));
			base.Write(")\r\nif ");
			base.Write(base.ToStringHelper.ToStringWithCulture((int)innerActionFrameData.InnerActionType));
			base.Write(" == 2 then\r\n    localFrame:setSingleFrameIndex(");
			base.Write(base.ToStringHelper.ToStringWithCulture(innerActionFrameData.SingleFrameIndex));
			base.Write(")\r\nelse\r\n    localFrame:setEnterWithName(true)\r\n    localFrame:setAnimationName(\"");
			base.Write(base.ToStringHelper.ToStringWithCulture(objectToConvert));
			base.Write("\")\r\nend\r\n");
		}
	}
}
