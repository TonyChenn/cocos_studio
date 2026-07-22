using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000026 RID: 38
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaSkeletonNodeObject : LuaBoneNodeObject
	{
		// Token: 0x060000E7 RID: 231 RVA: 0x00006D92 File Offset: 0x00004F92
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00006DAC File Offset: 0x00004FAC
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(SkeletonNodeObjectData) == objectData.GetType();
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00006DD0 File Offset: 0x00004FD0
		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccs.SkeletonNode:create()\r\n");
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00006DFC File Offset: 0x00004FFC
		public override void InitializeObject(BaseObjectData objectData)
		{
			SkeletonNodeObjectData objectData2 = objectData as SkeletonNodeObjectData;
			base.InitializeObject(objectData2);
		}
	}
}
