using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaScaleValueFrame : LuaFrame
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(ScaleValueFrameData) == objectData.GetType() && (((IFrameDataLuaExtend)objectData).Property == "Scale" || ((IFrameDataLuaExtend)objectData).Property == "RotationSkew" || ((IFrameDataLuaExtend)objectData).Property == "AnchorPoint");
		}

		public override void CreateObject(BaseObjectData objectData)
		{
			string property;
			if ((property = ((IFrameDataLuaExtend)objectData).Property) != null)
			{
				if (property == "Scale")
				{
					base.Write("ccs.ScaleFrame:create()\r\n");
					return;
				}
				if (property == "RotationSkew")
				{
					base.Write("ccs.RotationSkewFrame:create()\r\n");
					return;
				}
				if (!(property == "AnchorPoint"))
				{
					return;
				}
				base.Write("ccs.AnchorPointFrame:create()\r\n");
			}
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			ScaleValueFrameData scaleValueFrameData = objectData as ScaleValueFrameData;
			base.InitializeObject(scaleValueFrameData);
			string property;
			if ((property = ((IFrameDataLuaExtend)scaleValueFrameData).Property) != null)
			{
				if (property == "Scale")
				{
					base.Write("localFrame:setScaleX(");
					base.Write(base.ToStringHelper.ToStringWithCulture(scaleValueFrameData.X));
					base.Write(")\r\nlocalFrame:setScaleY(");
					base.Write(base.ToStringHelper.ToStringWithCulture(scaleValueFrameData.Y));
					base.Write(")\r\n");
					return;
				}
				if (property == "RotationSkew")
				{
					base.Write("localFrame:setSkewX(");
					base.Write(base.ToStringHelper.ToStringWithCulture(scaleValueFrameData.X));
					base.Write(")\r\nlocalFrame:setSkewY(");
					base.Write(base.ToStringHelper.ToStringWithCulture(scaleValueFrameData.Y));
					base.Write(")\r\n");
					return;
				}
				if (!(property == "AnchorPoint"))
				{
					return;
				}
				base.Write("localFrame:setAnchorPoint(cc.p(");
				base.Write(base.ToStringHelper.ToStringWithCulture(scaleValueFrameData.X));
				base.Write(",");
				base.Write(base.ToStringHelper.ToStringWithCulture(scaleValueFrameData.Y));
				base.Write("))\r\n");
			}
		}
	}
}
