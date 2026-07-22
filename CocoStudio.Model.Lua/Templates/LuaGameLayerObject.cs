using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x0200000F RID: 15
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaGameLayerObject : LuaNodeObject
	{
		// Token: 0x0600006C RID: 108 RVA: 0x000040BD File Offset: 0x000022BD
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000040D5 File Offset: 0x000022D5
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(GameLayerObjectData) == objectData.GetType() || typeof(LayerObjectData) == objectData.GetType();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004105 File Offset: 0x00002305
		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.OnCreateObject(objectData);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004110 File Offset: 0x00002310
		public override void InitializeObject(BaseObjectData objectData)
		{
			GameLayerObjectData gameLayerObjectData = objectData as GameLayerObjectData;
			base.InitializeObject(gameLayerObjectData);
			base.Write("layout = ccui.LayoutComponent:bindLayoutComponent(");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(gameLayerObjectData.Name)));
			base.Write(")\r\nlayout:setSize(cc.size(");
			base.Write(base.ToStringHelper.ToStringWithCulture(gameLayerObjectData.Size));
			base.Write("))\r\n");
		}
	}
}
