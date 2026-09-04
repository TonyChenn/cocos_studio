using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x0200002E RID: 46
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaTextureFrame : LuaFrame
	{
		// Token: 0x0600010F RID: 271 RVA: 0x000082A2 File Offset: 0x000064A2
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000082BA File Offset: 0x000064BA
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(TextureFrameData) == objectData.GetType();
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000082D1 File Offset: 0x000064D1
		public override void CreateObject(BaseObjectData objectData)
		{
			base.Write("ccs.TextureFrame:create()\r\n");
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000082E0 File Offset: 0x000064E0
		public override void InitializeObject(BaseObjectData objectData)
		{
			TextureFrameData textureFrameData = objectData as TextureFrameData;
			base.InitializeObject(textureFrameData);
			if (textureFrameData.TextureFile == null)
			{
				base.Write("localFrame:setTextureName(nil)\r\n");
				return;
			}
			base.PreloadPlist(textureFrameData.TextureFile);
			base.Write("localFrame:setTextureName(\"");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(textureFrameData.TextureFile.Path)));
			base.Write("\")\r\n");
		}
	}
}
