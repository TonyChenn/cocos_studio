using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model3D.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaParticle3DObject : LuaNode3DObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(Particle3DObjectData) == objectData.GetType();
		}

		protected override void OnCreateObject(BaseObjectData objectData)
		{
			Particle3DObjectData particle3DObjectData = objectData as Particle3DObjectData;
			base.PreloadPlist(particle3DObjectData.FileData);
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = cc.PUParticleSystem3D:create(\"");
			base.Write(base.ToStringHelper.ToStringWithCulture(particle3DObjectData.FileData.Path));
			base.Write("\")\r\n");
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			Particle3DObjectData particle3DObjectData = objectData as Particle3DObjectData;
			base.InitializeObject(particle3DObjectData);
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(particle3DObjectData.Name)));
			base.Write(":startParticleSystem()\r\n");
		}
	}
}
