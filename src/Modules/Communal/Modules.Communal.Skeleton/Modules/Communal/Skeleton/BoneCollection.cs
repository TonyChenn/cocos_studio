using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using CocoStudio.UndoManager.Recorder;

namespace Modules.Communal.Skeleton
{
	public class BoneCollection : NodeCollection
	{
		public BoneCollection(BoneObject parentObject) : base(parentObject)
		{
			this.parentBone = parentObject;
		}

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

		internal IReadOnlyList<BoneObject> Bones
		{
			get
			{
				return this.bones;
			}
		}

		internal IReadOnlyList<NodeObject> Skins
		{
			get
			{
				return this.skins;
			}
		}

		private BoneObject parentBone;

		private List<BoneObject> bones = new List<BoneObject>();

		private List<NodeObject> skins = new List<NodeObject>(1);
	}
}
