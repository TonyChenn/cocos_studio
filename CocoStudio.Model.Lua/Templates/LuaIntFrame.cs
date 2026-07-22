using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000011 RID: 17
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaIntFrame : LuaFrame
	{
		// Token: 0x06000074 RID: 116 RVA: 0x000041D8 File Offset: 0x000023D8
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000041F0 File Offset: 0x000023F0
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(IntFrameData) == objectData.GetType() && (((IFrameDataLuaExtend)objectData).Property == "Alpha" || ((IFrameDataLuaExtend)objectData).Property == "ZOrder");
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004244 File Offset: 0x00002444
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

		// Token: 0x06000077 RID: 119 RVA: 0x00004294 File Offset: 0x00002494
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
