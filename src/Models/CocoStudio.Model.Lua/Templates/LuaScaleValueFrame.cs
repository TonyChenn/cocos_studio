using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000013 RID: 19
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaScaleValueFrame : LuaFrame
	{
		// Token: 0x0600007E RID: 126 RVA: 0x000043FC File Offset: 0x000025FC
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004414 File Offset: 0x00002614
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(ScaleValueFrameData) == objectData.GetType() && (((IFrameDataLuaExtend)objectData).Property == "Scale" || ((IFrameDataLuaExtend)objectData).Property == "RotationSkew" || ((IFrameDataLuaExtend)objectData).Property == "AnchorPoint");
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004480 File Offset: 0x00002680
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

		// Token: 0x06000081 RID: 129 RVA: 0x000044E8 File Offset: 0x000026E8
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
