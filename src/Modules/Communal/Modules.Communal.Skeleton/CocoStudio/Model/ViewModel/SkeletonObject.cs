using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;
using Modules.Communal.Skeleton;

namespace CocoStudio.Model.ViewModel
{
	[EngineClassName("SkeletonNode")]
	[DisplayName("Property_SkeletonFile")]
	public class SkeletonObject : BoneObject
	{
		private CSSkeletonNode GetInnerObject()
		{
			return (CSSkeletonNode)this.innerNode;
		}

		public SkeletonObject()
		{
		}

		public SkeletonObject(ResourceFile resourceFile) : this()
		{
		}

		public SkeletonObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			OperationMask operationFlag = this.OperationFlag;
			this.OperationFlag = OperationMask.NoneFlag;
			this.GetInnerObject().SetDebugDrawEnable(!HideBoneTool.ToolHideAllBones);
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSSkeletonNode();
		}

		[Browsable(false)]
		[PropertyOrder(13)]
		public override int ZOrder
		{
			get
			{
				return base.ZOrder;
			}
			set
			{
				if (this.ZOrder != value)
				{
					this.GetCSVisual().SetZOrder(value);
					this.RaisePropertyChanged<int>(() => this.ZOrder);
				}
			}
		}

		[UndoProperty]
		[Browsable(false)]
		public override Color CColor
		{
			get
			{
				return base.CColor;
			}
			set
			{
				base.CColor = value;
			}
		}

		[Browsable(false)]
		[UndoProperty]
		public override PointF Position
		{
			get
			{
				return base.Position;
			}
			set
			{
				base.Position = value;
			}
		}

		[Browsable(false)]
		[UndoProperty]
		public override ScaleValue Scale
		{
			get
			{
				return this.GetInnerObject().GetScale();
			}
			set
			{
				this.GetInnerObject().SetScale(value);
				this.RaisePropertyChanged<ScaleValue>(() => this.Scale);
			}
		}

		[DefaultValue(0f)]
		[Browsable(false)]
		public override float Rotation
		{
			get
			{
				return base.Rotation;
			}
			set
			{
				base.Rotation = value;
			}
		}

		[UndoProperty]
		[Browsable(false)]
		public override bool VisibleForFrame
		{
			get
			{
				return base.VisibleForFrame;
			}
			set
			{
				base.VisibleForFrame = value;
			}
		}

		[UndoProperty]
		[ValueRange(0, 255, 1f, 10f)]
		[Browsable(false)]
		public override int Alpha
		{
			get
			{
				return base.Alpha;
			}
			set
			{
				base.Alpha = value;
			}
		}

		[UndoProperty]
		[Browsable(false)]
		public override BlendFuncValue BlendFunc
		{
			get
			{
				return base.BlendFunc;
			}
			set
			{
				base.BlendFunc = value;
			}
		}

		public void SetHideAllBones(bool ishide)
		{
			bool flag = !ishide;
			this.GetInnerObject().SetDebugDrawEnable(flag);
			this.RecursiveChildrenHitTest(this, flag);
		}

		public void SetHideAllSkins(bool ishide)
		{
			this.RecursiveHideSkin(this, !ishide);
			Services.Workbench.ActiveDocument.IsDirty = true;
		}

		private void RecursiveChildrenHitTest(BoneObject bone, bool canHitTest)
		{
			foreach (BoneObject boneObject in bone.Bones)
			{
				boneObject.IsHitTestVisible = canHitTest;
				this.RecursiveChildrenHitTest(boneObject, canHitTest);
			}
		}

		private void RecursiveHideSkin(BoneObject bone, bool visible)
		{
			foreach (NodeObject nodeObject in bone.Skins)
			{
				nodeObject.Recorder.IsAutoRecord = false;
				nodeObject.Visible = visible;
				nodeObject.Recorder.IsAutoRecord = true;
			}
			foreach (BoneObject bone2 in bone.Bones)
			{
				this.RecursiveHideSkin(bone2, visible);
			}
		}

		internal void ReCaculateMaxZorder()
		{
			this._subBonesMaxZOrder = 0;
			List<BoneObject> allSubBones = base.GetAllSubBones();
			foreach (BoneObject boneObject in allSubBones)
			{
				int zorder = boneObject.ZOrder;
				if (zorder > this._subBonesMaxZOrder)
				{
					this._subBonesMaxZOrder = zorder;
				}
			}
		}

		internal void SubBonesZOrderChangedHandler(BoneObject bone)
		{
			int zorder = bone.ZOrder;
			if (this._subBonesMaxZOrder < zorder)
			{
				this._subBonesMaxZOrder = zorder;
			}
			if (this.SubBonesZOrderChangeEvent != null)
			{
				this.SubBonesZOrderChangeEvent(bone, new ZOrderChangeEventArgs());
			}
		}

		internal int SubBonesMaxZOrder
		{
			get
			{
				return this._subBonesMaxZOrder;
			}
		}

		internal void ResetScaleWidth()
		{
			this.GetInnerObject().ResetAllSubBoneScaledWidth();
		}

		public bool IsNameExists(string name)
		{
			if (this.Name == name)
			{
				return true;
			}
			List<BoneObject> allSubBones = base.GetAllSubBones();
			foreach (BoneObject boneObject in allSubBones)
			{
				if (boneObject.Name == name)
				{
					return true;
				}
			}
			return false;
		}

		private int _subBonesMaxZOrder;

		public EventHandler<ZOrderChangeEventArgs> SubBonesZOrderChangeEvent;
	}
}
