using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model3D.Lua.Templates
{
	// Token: 0x02000004 RID: 4
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaParticle3DObject : LuaNode3DObject
	{
		// Token: 0x06000009 RID: 9 RVA: 0x000027D6 File Offset: 0x000009D6
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000027F0 File Offset: 0x000009F0
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(Particle3DObjectData) == objectData.GetType();
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002814 File Offset: 0x00000A14
		protected override void OnCreateObject(BaseObjectData objectData)
		{
			Particle3DObjectData particle3DObjectData = objectData as Particle3DObjectData;
			base.PreloadPlist(particle3DObjectData.FileData);
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = cc.PUParticleSystem3D:create(\"");
			base.Write(base.ToStringHelper.ToStringWithCulture(particle3DObjectData.FileData.Path));
			base.Write("\")\r\n");
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002884 File Offset: 0x00000A84
		public override void InitializeObject(BaseObjectData objectData)
		{
			Particle3DObjectData particle3DObjectData = objectData as Particle3DObjectData;
			base.InitializeObject(particle3DObjectData);
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(particle3DObjectData.Name)));
			base.Write(":startParticleSystem()\r\n");
		}
	}
}
