using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using CocoStudio.UndoManager.Recorder;

namespace Modules.Communal.Skeleton
{
	// Token: 0x02000023 RID: 35
	public class BoneCollection : NodeCollection
	{
		// Token: 0x0600016C RID: 364 RVA: 0x0000816F File Offset: 0x0000636F
		public BoneCollection(BoneObject parentObject) : base(parentObject)
		{
			this.parentBone = parentObject;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00008198 File Offset: 0x00006398
		protected override void InsertItem(int index, AbstractNodeObject item)
		{
			if (item == null)
			{
				throw new ArgumentException("Child should not be null.");
			}
			BoneObject boneObject = item as BoneObject;
			base.InsertItem(index, item);
			if (boneObject != null)
			{
				this.bones.Add(boneObject);
				boneObject.IsHitTestVisible = !HideBoneTool.ToolHideAllBones;
			}
			else
			{
				NodeObject nodeObject = item as NodeObject;
				this.skins.Add(nodeObject);
				if (BaseRecorder.IsCreateDefaultRecorder)
				{
					bool flag = !HideSkinTool.ToolHideAllSkins;
					if (nodeObject.Visible != flag)
					{
						if (nodeObject.Recorder != null)
						{
							bool isAutoRecord = nodeObject.Recorder.IsAutoRecord;
							nodeObject.Recorder.IsAutoRecord = false;
							nodeObject.Visible = flag;
							nodeObject.Recorder.IsAutoRecord = isAutoRecord;
						}
						else
						{
							nodeObject.Visible = flag;
						}
					}
				}
			}
			item.ApplyCachedWorldMatrix();
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00008250 File Offset: 0x00006450
		protected override void RemoveItem(int index)
		{
			AbstractNodeObject abstractNodeObject = base.Items[index];
			abstractNodeObject.CacheWorldMatrix();
			base.RemoveItem(index);
			BoneObject boneObject = abstractNodeObject as BoneObject;
			if (boneObject != null)
			{
				this.bones.Remove(boneObject);
				return;
			}
			NodeObject item = abstractNodeObject as NodeObject;
			this.skins.Remove(item);
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600016F RID: 367 RVA: 0x000082A3 File Offset: 0x000064A3
		internal IReadOnlyList<BoneObject> Bones
		{
			get
			{
				return this.bones;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000170 RID: 368 RVA: 0x000082AB File Offset: 0x000064AB
		internal IReadOnlyList<NodeObject> Skins
		{
			get
			{
				return this.skins;
			}
		}

		// Token: 0x0400007B RID: 123
		private BoneObject parentBone;

		// Token: 0x0400007C RID: 124
		private List<BoneObject> bones = new List<BoneObject>();

		// Token: 0x0400007D RID: 125
		private List<NodeObject> skins = new List<NodeObject>(1);
	}
}
