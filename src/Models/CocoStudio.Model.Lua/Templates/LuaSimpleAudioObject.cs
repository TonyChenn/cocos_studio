using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000025 RID: 37
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaSimpleAudioObject : LuaNodeObject
	{
		// Token: 0x060000E2 RID: 226 RVA: 0x00006C53 File Offset: 0x00004E53
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00006C6C File Offset: 0x00004E6C
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(SimpleAudioObjectData) == objectData.GetType();
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00006C90 File Offset: 0x00004E90
		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = cc.Node:create()\r\n");
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00006CBC File Offset: 0x00004EBC
		public override void InitializeObject(BaseObjectData objectData)
		{
			SimpleAudioObjectData simpleAudioObjectData = objectData as SimpleAudioObjectData;
			base.Write("local audio = ccs.ComAudio:create()\r\n");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(simpleAudioObjectData.Name)));
			base.Write(":addComponent(audio)\r\n");
			if (simpleAudioObjectData.FileData != null)
			{
				base.Write("audio:preloadBackgroundMusic(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(simpleAudioObjectData.FileData.Path)));
				base.Write("\")\r\n");
			}
			if (base.CanExport<bool>(simpleAudioObjectData.Loop, false))
			{
				base.Write("audio:setLoop(");
				base.Write(base.ToStringHelper.ToStringWithCulture(simpleAudioObjectData.Loop));
				base.Write(")\r\n");
			}
		}
	}
}
