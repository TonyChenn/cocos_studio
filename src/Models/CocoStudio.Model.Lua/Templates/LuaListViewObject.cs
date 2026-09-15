using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaListViewObject : LuaPanelObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(ListViewObjectData) == objectData.GetType();
		}

		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccui.ListView:create()\r\n");
		}

		public override void AddChild(BaseObjectData parent, BaseObjectData child)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(parent.Name)));
			base.Write(":pushBackCustomItem(");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(child.Name)));
			base.Write(")\r\n");
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			ListViewObjectData listViewObjectData = objectData as ListViewObjectData;
			if (base.CanExport<int>(listViewObjectData.ItemMargin, 0))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(listViewObjectData.Name)));
				base.Write(":setItemsMargin(");
				base.Write(base.ToStringHelper.ToStringWithCulture(listViewObjectData.ItemMargin));
				base.Write(")\r\n");
			}
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(listViewObjectData.Name)));
			base.Write(":setDirection(");
			base.Write(base.ToStringHelper.ToStringWithCulture((int)listViewObjectData.DirectionType));
			base.Write(")\r\n");
			if (listViewObjectData.DirectionType == ListViewDirectionType.Horizontal)
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(listViewObjectData.Name)));
				base.Write(":setGravity(");
				base.Write(base.ToStringHelper.ToStringWithCulture((int)listViewObjectData.VerticalType));
				base.Write(")\r\n");
			}
			else if (listViewObjectData.DirectionType == ListViewDirectionType.Vertical)
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(listViewObjectData.Name)));
				base.Write(":setGravity(");
				base.Write(base.ToStringHelper.ToStringWithCulture((int)listViewObjectData.HorizontalType));
				base.Write(")\r\n");
			}
			base.InitializeObject(listViewObjectData);
		}
	}
}
