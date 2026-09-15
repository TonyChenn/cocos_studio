using System;
using System.CodeDom.Compiler;
using System.IO;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaArmatureNodeObject : LuaNodeObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(ArmatureNodeObjectData) == objectData.GetType();
		}

		public override void CreateObject(BaseObjectData objectData)
		{
			base.Write("local ");
			base.Write(base.ToStringHelper.ToStringWithCulture(objectData.Name));
			base.Write(" = ccs.Armature:create()\r\n");
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			ArmatureNodeObjectData armatureNodeObjectData = objectData as ArmatureNodeObjectData;
			base.InitializeObject(armatureNodeObjectData);
			if (armatureNodeObjectData.FileData == null)
			{
				return;
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(armatureNodeObjectData.FileData.Path);
			string currentAnimationName = armatureNodeObjectData.CurrentAnimationName;
			int num = armatureNodeObjectData.IsLoop ? 1 : 0;
			base.Write("ccs.ArmatureDataManager:getInstance():addArmatureFileInfo(\"");
			base.Write(base.ToStringHelper.ToStringWithCulture(armatureNodeObjectData.FileData.Path));
			base.Write("\")\r\n");
			base.Write(base.ToStringHelper.ToStringWithCulture(armatureNodeObjectData.Name));
			base.Write(":init(\"");
			base.Write(base.ToStringHelper.ToStringWithCulture(fileNameWithoutExtension));
			base.Write("\")\r\n");
			if (armatureNodeObjectData.IsAutoPlay)
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(armatureNodeObjectData.Name));
				base.Write(":getAnimation():play(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(currentAnimationName));
				base.Write("\",-1, ");
				base.Write(base.ToStringHelper.ToStringWithCulture(num));
				base.Write(")\r\n");
				return;
			}
			base.Write(base.ToStringHelper.ToStringWithCulture(armatureNodeObjectData.Name));
			base.Write(":getAnimation():play(\"");
			base.Write(base.ToStringHelper.ToStringWithCulture(currentAnimationName));
			base.Write("\")\r\n");
			base.Write(base.ToStringHelper.ToStringWithCulture(armatureNodeObjectData.Name));
			base.Write(":getAnimation():gotoAndPause(0)\r\n");
		}
	}
}
