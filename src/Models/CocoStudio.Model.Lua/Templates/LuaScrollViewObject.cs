using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaScrollViewObject : LuaPanelObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(ScrollViewObjectData) == objectData.GetType();
		}

		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccui.ScrollView:create()\r\n");
		}

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
