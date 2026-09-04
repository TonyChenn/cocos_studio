using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000029 RID: 41
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaStringFrame : LuaFrame
	{
		// Token: 0x060000F6 RID: 246 RVA: 0x0000754F File Offset: 0x0000574F
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00007567 File Offset: 0x00005767
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return false;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000756C File Offset: 0x0000576C
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

		// Token: 0x060000F9 RID: 249 RVA: 0x000075A4 File Offset: 0x000057A4
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
