using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaEventFrame : LuaFrame
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(EventFrameData) == objectData.GetType();
		}

		public override void CreateObject(BaseObjectData objectData)
		{
			base.Write("ccs.EventFrame:create()\r\n");
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			EventFrameData eventFrameData = objectData as EventFrameData;
			base.InitializeObject(eventFrameData);
			base.Write("localFrame:setEvent(\"");
			base.Write(base.ToStringHelper.ToStringWithCulture(eventFrameData.Value));
			base.Write("\")\r\n");
		}
	}
}
