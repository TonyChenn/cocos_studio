using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaSimpleAudioObject : LuaNodeObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(SimpleAudioObjectData) == objectData.GetType();
		}

		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = cc.Node:create()\r\n");
		}

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
