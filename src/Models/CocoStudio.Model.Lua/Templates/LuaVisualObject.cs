using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaVisualObject : LuaBaseObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(VisualObjectData) == objectData.GetType();
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			VisualObjectData visualObjectData = objectData as VisualObjectData;
			base.InitializeObject(visualObjectData);
			if (base.CanExport<int>(visualObjectData.ZOrder, 0))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(visualObjectData.Name)));
				base.Write(":setLocalZOrder(");
				base.Write(base.ToStringHelper.ToStringWithCulture(visualObjectData.ZOrder));
				base.Write(")\r\n");
			}
		}
	}
}
