using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaSkeletonNodeObject : LuaBoneNodeObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(SkeletonNodeObjectData) == objectData.GetType();
		}

		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccs.SkeletonNode:create()\r\n");
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			SkeletonNodeObjectData objectData2 = objectData as SkeletonNodeObjectData;
			base.InitializeObject(objectData2);
		}
	}
}
