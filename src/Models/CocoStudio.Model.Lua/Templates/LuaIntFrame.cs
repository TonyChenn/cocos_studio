using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaIntFrame : LuaFrame
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(IntFrameData) == objectData.GetType() && (((IFrameDataLuaExtend)objectData).Property == "Alpha" || ((IFrameDataLuaExtend)objectData).Property == "ZOrder");
		}

		public override void CreateObject(BaseObjectData objectData)
		{
			string property;
			if ((property = ((IFrameDataLuaExtend)objectData).Property) != null)
			{
				if (property == "Alpha")
				{
					base.Write("ccs.AlphaFrame:create()\r\n");
					return;
				}
				if (!(property == "ZOrder"))
				{
					return;
				}
				base.Write("ccs.ZOrderFrame:create()\r\n");
			}
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			IntFrameData intFrameData = objectData as IntFrameData;
			base.InitializeObject(intFrameData);
			string property;
			if ((property = ((IFrameDataLuaExtend)intFrameData).Property) != null)
			{
				if (property == "Alpha")
				{
					base.Write("localFrame:setAlpha(");
					base.Write(base.ToStringHelper.ToStringWithCulture(intFrameData.Value));
					base.Write(")\r\n");
					return;
				}
				if (!(property == "ZOrder"))
				{
					return;
				}
				base.Write("localFrame:setZOrder(");
				base.Write(base.ToStringHelper.ToStringWithCulture(intFrameData.Value));
				base.Write(")\r\n");
			}
		}
	}
}
