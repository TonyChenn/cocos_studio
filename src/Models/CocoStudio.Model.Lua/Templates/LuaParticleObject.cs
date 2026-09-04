using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000022 RID: 34
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaParticleObject : LuaNodeObject
	{
		// Token: 0x060000D3 RID: 211 RVA: 0x000067D3 File Offset: 0x000049D3
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000067EC File Offset: 0x000049EC
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(ParticleObjectData) == objectData.GetType();
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006810 File Offset: 0x00004A10
		protected override void OnCreateObject(BaseObjectData objectData)
		{
			ParticleObjectData particleObjectData = objectData as ParticleObjectData;
			if (particleObjectData.FileData != null)
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
				base.Write(" = cc.ParticleSystemQuad:create(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(particleObjectData.FileData.Path)));
				base.Write("\")\r\n");
				return;
			}
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = cc.ParticleSystemQuad:create()\r\n");
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000068B0 File Offset: 0x00004AB0
		public override void InitializeObject(BaseObjectData objectData)
		{
			base.InitializeObject(objectData);
		}
	}
}
