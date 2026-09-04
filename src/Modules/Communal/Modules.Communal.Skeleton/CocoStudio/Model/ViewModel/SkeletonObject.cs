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
	// Token: 0x02000028 RID: 40
	[EngineClassName("SkeletonNode")]
	[DisplayName("Property_SkeletonFile")]
	public class SkeletonObject : BoneObject
	{
		// Token: 0x060001B6 RID: 438 RVA: 0x0000929D File Offset: 0x0000749D
		private CSSkeletonNode GetInnerObject()
		{
			return (CSSkeletonNode)this.innerNode;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000092AA File Offset: 0x000074AA
		public SkeletonObject()
		{
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x000092B2 File Offset: 0x000074B2
		public SkeletonObject(ResourceFile resourceFile) : this()
		{
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x000092BA File Offset: 0x000074BA
		public SkeletonObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x060001BA RID: 442 RVA: 0x000092C3 File Offset: 0x000074C3
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			OperationMask operationFlag = this.OperationFlag;
			this.OperationFlag = OperationMask.NoneFlag;
			this.GetInnerObject().SetDebugDrawEnable(!HideBoneTool.ToolHideAllBones);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000092ED File Offset: 0x000074ED
		protected override void CreateCSObject()
		{
			this.innerNode = new CSSkeletonNode();
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001BC RID: 444 RVA: 0x000092FA File Offset: 0x000074FA
		// (set) Token: 0x060001BD RID: 445 RVA: 0x00009304 File Offset: 0x00007504
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

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00009360 File Offset: 0x00007560
		// (set) Token: 0x060001BF RID: 447 RVA: 0x00009368 File Offset: 0x00007568
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

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00009371 File Offset: 0x00007571
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x00009379 File Offset: 0x00007579
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

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00009382 File Offset: 0x00007582
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x00009390 File Offset: 0x00007590
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

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x000093E3 File Offset: 0x000075E3
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x000093EB File Offset: 0x000075EB
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

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x000093F4 File Offset: 0x000075F4
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x000093FC File Offset: 0x000075FC
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

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x00009405 File Offset: 0x00007605
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x0000940D File Offset: 0x0000760D
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

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001CA RID: 458 RVA: 0x00009416 File Offset: 0x00007616
		// (set) Token: 0x060001CB RID: 459 RVA: 0x0000941E File Offset: 0x0000761E
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

		// Token: 0x060001CC RID: 460 RVA: 0x00009428 File Offset: 0x00007628
		public void SetHideAllBones(bool ishide)
		{
			bool flag = !ishide;
			this.GetInnerObject().SetDebugDrawEnable(flag);
			this.RecursiveChildrenHitTest(this, flag);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000944E File Offset: 0x0000764E
		public void SetHideAllSkins(bool ishide)
		{
			this.RecursiveHideSkin(this, !ishide);
			Services.Workbench.ActiveDocument.IsDirty = true;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000946C File Offset: 0x0000766C
		private void RecursiveChildrenHitTest(BoneObject bone, bool canHitTest)
		{
			foreach (BoneObject boneObject in bone.Bones)
			{
				boneObject.IsHitTestVisible = canHitTest;
				this.RecursiveChildrenHitTest(boneObject, canHitTest);
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000094C4 File Offset: 0x000076C4
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

		// Token: 0x060001D0 RID: 464 RVA: 0x00009568 File Offset: 0x00007768
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

		// Token: 0x060001D1 RID: 465 RVA: 0x000095D4 File Offset: 0x000077D4
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

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x00009611 File Offset: 0x00007811
		internal int SubBonesMaxZOrder
		{
			get
			{
				return this._subBonesMaxZOrder;
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00009619 File Offset: 0x00007819
		internal void ResetScaleWidth()
		{
			this.GetInnerObject().ResetAllSubBoneScaledWidth();
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00009628 File Offset: 0x00007828
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

		// Token: 0x04000085 RID: 133
		private int _subBonesMaxZOrder;

		// Token: 0x04000086 RID: 134
		public EventHandler<ZOrderChangeEventArgs> SubBonesZOrderChangeEvent;
	}
}
