using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaStringFrame : LuaFrame
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return false;
		}

		public override void CreateObject(BaseObjectData objectData)
		{
			string property;
			if ((property = ((IFrameDataLuaExtend)objectData).Property) != null)
			{
				if (!(property == "VisibleForFrame"))
				{
					return;
				}
				base.Write("ccs.VisibleFrame:create()\r\n");
			}
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			StringFrameData stringFrameData = objectData as StringFrameData;
			base.InitializeObject(stringFrameData);
			string property;
			if ((property = ((IFrameDataLuaExtend)objectData).Property) != null)
			{
				if (!(property == "VisibleForFrame"))
				{
					return;
				}
				base.Write("localFrame:setVisible(");
				base.Write(base.ToStringHelper.ToStringWithCulture(stringFrameData.Value.ToString().ToLower()));
				base.Write(")\r\n");
			}
		}
	}
}
