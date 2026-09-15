using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaFrame : LuaBaseObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(FrameData) == objectData.GetType();
		}

		public override void CreateObject(BaseObjectData objectData)
		{
			throw new InvalidOperationException();
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			FrameData frameData = objectData as FrameData;
			base.Write("localFrame:setFrameIndex(");
			base.Write(base.ToStringHelper.ToStringWithCulture(frameData.FrameIndex));
			base.Write(")\r\nlocalFrame:setTween(");
			base.Write(base.ToStringHelper.ToStringWithCulture(frameData.Tween));
			base.Write(")\r\n");
		}
	}
}
