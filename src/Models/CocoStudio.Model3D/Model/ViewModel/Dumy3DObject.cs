using System;
using CocoStudio.EngineAdapterWrap;

namespace CocoStudio.Model.ViewModel
{
	public class Dumy3DObject : Node3DObject
	{
		public Dumy3DObject()
		{
		}

		public Dumy3DObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSDumyNode();
		}

		private CSDumyNode GetInnerNode()
		{
			return this.innerNode as CSDumyNode;
		}

		public void EffectResult(ControlResult result)
		{
			this.GetInnerNode().EffectResult(result);
		}

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
