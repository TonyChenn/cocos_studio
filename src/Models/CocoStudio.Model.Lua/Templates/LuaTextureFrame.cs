using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaTextureFrame : LuaFrame
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(TextureFrameData) == objectData.GetType();
		}

		public override void CreateObject(BaseObjectData objectData)
		{
			base.Write("ccs.TextureFrame:create()\r\n");
		}

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
