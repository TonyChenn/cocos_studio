using System;
using System.CodeDom.Compiler;
using System.IO;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x0200000B RID: 11
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaArmatureNodeObject : LuaNodeObject
	{
		// Token: 0x06000058 RID: 88 RVA: 0x000038D3 File Offset: 0x00001AD3
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000038EC File Offset: 0x00001AEC
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(ArmatureNodeObjectData) == objectData.GetType();
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003910 File Offset: 0x00001B10
		public override void CreateObject(BaseObjectData objectData)
		{
			base.Write("local ");
			base.Write(base.ToStringHelper.ToStringWithCulture(objectData.Name));
			base.Write(" = ccs.Armature:create()\r\n");
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003940 File Offset: 0x00001B40
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
