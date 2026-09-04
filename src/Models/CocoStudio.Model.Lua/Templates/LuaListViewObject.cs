using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x0200001F RID: 31
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaListViewObject : LuaPanelObject
	{
		// Token: 0x060000C2 RID: 194 RVA: 0x00006287 File Offset: 0x00004487
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000062A0 File Offset: 0x000044A0
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(ListViewObjectData) == objectData.GetType();
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000062C4 File Offset: 0x000044C4
		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccui.ListView:create()\r\n");
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000062F0 File Offset: 0x000044F0
		public override void AddChild(BaseObjectData parent, BaseObjectData child)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(parent.Name)));
			base.Write(":pushBackCustomItem(");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(child.Name)));
			base.Write(")\r\n");
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00006350 File Offset: 0x00004550
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
