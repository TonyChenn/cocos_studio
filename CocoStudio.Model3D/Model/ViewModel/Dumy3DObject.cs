using System;
using CocoStudio.EngineAdapterWrap;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000021 RID: 33
	public class Dumy3DObject : Node3DObject
	{
		// Token: 0x06000122 RID: 290 RVA: 0x000056A9 File Offset: 0x000038A9
		public Dumy3DObject()
		{
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000056B1 File Offset: 0x000038B1
		public Dumy3DObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000056BA File Offset: 0x000038BA
		protected override void CreateCSObject()
		{
			this.innerNode = new CSDumyNode();
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000056C7 File Offset: 0x000038C7
		private CSDumyNode GetInnerNode()
		{
			return this.innerNode as CSDumyNode;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000056D4 File Offset: 0x000038D4
		public void EffectResult(ControlResult result)
		{
			this.GetInnerNode().EffectResult(result);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000056E2 File Offset: 0x000038E2
		public void EffectToTarget(Node3DObject node, ControlResult result, bool center, bool global)
		{
			if (node == null)
			{
				return;
			}
			this.GetInnerNode().EffectToTarget(node.GetCSVisual(), result, center, global);
			node.Position3D = node.Position3D;
			node.Rotation3D = node.Rotation3D;
			node.Scale3D = node.Scale3D;
		}
	}
}
