using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000018 RID: 24
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaEventFrame : LuaFrame
	{
		// Token: 0x06000099 RID: 153 RVA: 0x00005418 File Offset: 0x00003618
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00005430 File Offset: 0x00003630
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(EventFrameData) == objectData.GetType();
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00005447 File Offset: 0x00003647
		public override void CreateObject(BaseObjectData objectData)
		{
			base.Write("ccs.EventFrame:create()\r\n");
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00005454 File Offset: 0x00003654
		public override void InitializeObject(BaseObjectData objectData)
		{
			EventFrameData eventFrameData = objectData as EventFrameData;
			base.InitializeObject(eventFrameData);
			base.Write("localFrame:setEvent(\"");
			base.Write(base.ToStringHelper.ToStringWithCulture(eventFrameData.Value));
			base.Write("\")\r\n");
		}
	}
}
