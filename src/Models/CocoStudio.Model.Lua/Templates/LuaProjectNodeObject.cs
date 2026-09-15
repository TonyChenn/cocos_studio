using System;
using System.CodeDom.Compiler;
using System.IO;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaProjectNodeObject : LuaNodeObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(FileNodeObjectData) == objectData.GetType();
		}

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
