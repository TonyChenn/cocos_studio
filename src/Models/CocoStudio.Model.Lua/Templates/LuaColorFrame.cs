using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaColorFrame : LuaFrame
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(ColorFrameData) == objectData.GetType();
		}

		public override void CreateObject(BaseObjectData objectData)
		{
			base.Write("ccs.ColorFrame:create()\r\n");
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			ColorFrameData colorFrameData = objectData as ColorFrameData;
			base.InitializeObject(colorFrameData);
			base.Write("localFrame:setColor(");
			base.Write(base.ToStringHelper.ToStringWithCulture(colorFrameData.Color));
			base.Write(")\r\n");
		}
	}
}
