using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaPageViewObject : LuaPanelObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(PageViewObjectData) == objectData.GetType();
		}

		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccui.PageView:create()\r\n");
		}

		public override void AddChild(BaseObjectData parent, BaseObjectData child)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(parent.Name)));
			base.Write(":addPage(");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(child.Name)));
			base.Write(")\r\n");
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			PageViewObjectData objectData2 = objectData as PageViewObjectData;
			base.InitializeObject(objectData2);
		}
	}
}
