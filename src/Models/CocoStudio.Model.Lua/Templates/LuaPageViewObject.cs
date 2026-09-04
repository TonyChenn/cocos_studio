using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000021 RID: 33
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaPageViewObject : LuaPanelObject
	{
		// Token: 0x060000CD RID: 205 RVA: 0x000066E7 File Offset: 0x000048E7
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00006700 File Offset: 0x00004900
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(PageViewObjectData) == objectData.GetType();
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00006724 File Offset: 0x00004924
		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccui.PageView:create()\r\n");
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00006750 File Offset: 0x00004950
		public override void AddChild(BaseObjectData parent, BaseObjectData child)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(parent.Name)));
			base.Write(":addPage(");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(child.Name)));
			base.Write(")\r\n");
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000067B0 File Offset: 0x000049B0
		public override void InitializeObject(BaseObjectData objectData)
		{
			PageViewObjectData objectData2 = objectData as PageViewObjectData;
			base.InitializeObject(objectData2);
		}
	}
}
