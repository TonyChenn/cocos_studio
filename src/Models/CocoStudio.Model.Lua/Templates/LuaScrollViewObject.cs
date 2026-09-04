using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000024 RID: 36
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaScrollViewObject : LuaPanelObject
	{
		// Token: 0x060000DD RID: 221 RVA: 0x00006A89 File Offset: 0x00004C89
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00006AA4 File Offset: 0x00004CA4
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(ScrollViewObjectData) == objectData.GetType();
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00006AC8 File Offset: 0x00004CC8
		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccui.ScrollView:create()\r\n");
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00006AF4 File Offset: 0x00004CF4
		public override void InitializeObject(BaseObjectData objectData)
		{
			ScrollViewObjectData scrollViewObjectData = objectData as ScrollViewObjectData;
			if (base.CanExport<bool>(scrollViewObjectData.IsBounceEnabled, false))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(scrollViewObjectData.Name)));
				base.Write(":setBounceEnabled(");
				base.Write(base.ToStringHelper.ToStringWithCulture(scrollViewObjectData.IsBounceEnabled));
				base.Write(")\r\n");
			}
			if (base.CanExport<ScrollViewDirectionType>(scrollViewObjectData.ScrollDirectionType, ScrollViewDirectionType.Vertical))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(scrollViewObjectData.Name)));
				base.Write(":setDirection(");
				base.Write(base.ToStringHelper.ToStringWithCulture((int)scrollViewObjectData.ScrollDirectionType));
				base.Write(")\r\n");
			}
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(scrollViewObjectData.Name)));
			base.Write(":setInnerContainerSize(cc.size(");
			base.Write(base.ToStringHelper.ToStringWithCulture(scrollViewObjectData.InnerNodeSize.Width));
			base.Write(",");
			base.Write(base.ToStringHelper.ToStringWithCulture(scrollViewObjectData.InnerNodeSize.Height));
			base.Write("))\r\n");
			base.InitializeObject(scrollViewObjectData);
		}
	}
}
