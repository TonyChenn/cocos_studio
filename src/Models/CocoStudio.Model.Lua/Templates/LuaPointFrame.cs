using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaPointFrame : LuaFrame
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(PointFrameData) == objectData.GetType();
		}

		public override void CreateObject(BaseObjectData objectData)
		{
			base.Write("ccs.PositionFrame:create()\r\n");
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			PointFrameData pointFrameData = objectData as PointFrameData;
			base.InitializeObject(pointFrameData);
			base.Write("localFrame:setX(");
			base.Write(base.ToStringHelper.ToStringWithCulture(pointFrameData.X));
			base.Write(")\r\nlocalFrame:setY(");
			base.Write(base.ToStringHelper.ToStringWithCulture(pointFrameData.Y));
			base.Write(")\r\n");
		}
	}
}
