using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000010 RID: 16
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaGameNodeObject : LuaNodeObject
	{
		// Token: 0x06000071 RID: 113 RVA: 0x00004188 File Offset: 0x00002388
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000041A0 File Offset: 0x000023A0
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(GameNodeObjectData) == objectData.GetType() || typeof(SingleNodeObjectData) == objectData.GetType();
		}
	}
}
