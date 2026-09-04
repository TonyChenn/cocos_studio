using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x0200000D RID: 13
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaFrame : LuaBaseObject
	{
		// Token: 0x06000062 RID: 98 RVA: 0x00003F26 File Offset: 0x00002126
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003F3E File Offset: 0x0000213E
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(FrameData) == objectData.GetType();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003F55 File Offset: 0x00002155
		public override void CreateObject(BaseObjectData objectData)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003F5C File Offset: 0x0000215C
		public override void InitializeObject(BaseObjectData objectData)
		{
			FrameData frameData = objectData as FrameData;
			base.Write("localFrame:setFrameIndex(");
			base.Write(base.ToStringHelper.ToStringWithCulture(frameData.FrameIndex));
			base.Write(")\r\nlocalFrame:setTween(");
			base.Write(base.ToStringHelper.ToStringWithCulture(frameData.Tween));
			base.Write(")\r\n");
		}
	}
}
