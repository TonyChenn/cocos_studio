using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000008 RID: 8
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaAnimationInfoData : LuaBaseObject
	{
		// Token: 0x06000041 RID: 65 RVA: 0x000028E2 File Offset: 0x00000AE2
		public override string TransformText()
		{
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000028EF File Offset: 0x00000AEF
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(AnimationInfoData) == objectData.GetType();
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002908 File Offset: 0x00000B08
		public override void CreateObject(BaseObjectData objectData)
		{
			AnimationInfoData animationInfoData = objectData as AnimationInfoData;
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = {name=\"");
			base.Write(base.ToStringHelper.ToStringWithCulture(animationInfoData.Name));
			base.Write("\", startIndex=");
			base.Write(base.ToStringHelper.ToStringWithCulture(animationInfoData.StartIndex));
			base.Write(", endIndex=");
			base.Write(base.ToStringHelper.ToStringWithCulture(animationInfoData.EndIndex));
			base.Write("}\r\n");
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000029B4 File Offset: 0x00000BB4
		public override void InitializeObject(BaseObjectData objectData)
		{
		}
	}
}
