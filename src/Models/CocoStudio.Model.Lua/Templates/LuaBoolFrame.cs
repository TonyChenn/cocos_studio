using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaBoolFrame : LuaFrame
	{
		public override string TransformText()
		{
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(BoolFrameData) == objectData.GetType() && ((IFrameDataLuaExtend)objectData).Property == "VisibleForFrame";
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
			BoolFrameData boolFrameData = objectData as BoolFrameData;
			base.InitializeObject(boolFrameData);
			string property;
			if ((property = ((IFrameDataLuaExtend)boolFrameData).Property) != null)
			{
				if (!(property == "VisibleForFrame"))
				{
					return;
				}
				base.Write("localFrame:setVisible(");
				base.Write(base.ToStringHelper.ToStringWithCulture(boolFrameData.Value.ToString().ToLower()));
				base.Write(")\r\n");
			}
		}
	}
}
