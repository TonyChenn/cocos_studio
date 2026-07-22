using System;
using System.CodeDom.Compiler;
using System.IO;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000023 RID: 35
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaProjectNodeObject : LuaNodeObject
	{
		// Token: 0x060000D8 RID: 216 RVA: 0x000068C1 File Offset: 0x00004AC1
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000068DC File Offset: 0x00004ADC
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(FileNodeObjectData) == objectData.GetType();
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00006900 File Offset: 0x00004B00
		public override void CreateObject(BaseObjectData objectData)
		{
			FileNodeObjectData fileNodeObjectData = objectData as FileNodeObjectData;
			if (fileNodeObjectData.FileData != null)
			{
				string objectToConvert = Path.ChangeExtension(base.LuaPathFormat(fileNodeObjectData.FileData.Path), ".lua").Replace(".lua", null).Replace('/', '.');
				base.Write("innerCSD = require(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(objectToConvert));
				base.Write("\")\r\ninnerProject = innerCSD.create(callBackProvider)\r\n");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
				base.Write(" = innerProject.root\r\n");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(objectData.Name)));
				base.Write(".animation = innerProject.animation\r\n");
				return;
			}
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = cc.Node:create()\r\n");
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00006A0C File Offset: 0x00004C0C
		public override void InitializeObject(BaseObjectData objectData)
		{
			FileNodeObjectData fileNodeObjectData = objectData as FileNodeObjectData;
			base.InitializeObject(objectData);
			base.Write("innerProject.animation:setTimeSpeed(");
			base.Write(base.ToStringHelper.ToStringWithCulture(fileNodeObjectData.InnerActionSpeed));
			base.Write(")\r\n");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(objectData.Name)));
			base.Write(":runAction(innerProject.animation)\r\n");
		}
	}
}
