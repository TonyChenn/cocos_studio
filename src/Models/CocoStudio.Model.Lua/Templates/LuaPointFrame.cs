using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000012 RID: 18
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaPointFrame : LuaFrame
	{
		// Token: 0x06000079 RID: 121 RVA: 0x00004344 File Offset: 0x00002544
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000435C File Offset: 0x0000255C
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(PointFrameData) == objectData.GetType();
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004373 File Offset: 0x00002573
		public override void CreateObject(BaseObjectData objectData)
		{
			base.Write("ccs.PositionFrame:create()\r\n");
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004380 File Offset: 0x00002580
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
