using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x0200000E RID: 14
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaBoolFrame : LuaFrame
	{
		// Token: 0x06000067 RID: 103 RVA: 0x00003FD1 File Offset: 0x000021D1
		public override string TransformText()
		{
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003FDE File Offset: 0x000021DE
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(BoolFrameData) == objectData.GetType() && ((IFrameDataLuaExtend)objectData).Property == "VisibleForFrame";
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004010 File Offset: 0x00002210
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

		// Token: 0x0600006A RID: 106 RVA: 0x00004048 File Offset: 0x00002248
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
