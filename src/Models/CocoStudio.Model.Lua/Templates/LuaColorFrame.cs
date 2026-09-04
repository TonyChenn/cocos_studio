using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000017 RID: 23
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaColorFrame : LuaFrame
	{
		// Token: 0x06000094 RID: 148 RVA: 0x0000538C File Offset: 0x0000358C
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000053A4 File Offset: 0x000035A4
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(ColorFrameData) == objectData.GetType();
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000053BB File Offset: 0x000035BB
		public override void CreateObject(BaseObjectData objectData)
		{
			base.Write("ccs.ColorFrame:create()\r\n");
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000053C8 File Offset: 0x000035C8
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
