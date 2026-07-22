using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000009 RID: 9
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaVisualObject : LuaBaseObject
	{
		// Token: 0x06000046 RID: 70 RVA: 0x000029BE File Offset: 0x00000BBE
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000029D6 File Offset: 0x00000BD6
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(VisualObjectData) == objectData.GetType();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000029F0 File Offset: 0x00000BF0
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
