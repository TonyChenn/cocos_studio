using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaInnerActionFrame : LuaFrame
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(InnerActionFrameData) == objectData.GetType();
		}

		public override void CreateObject(BaseObjectData objectData)
		{
			base.Write("ccs.InnerActionFrame:create()\r\n");
		}

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
