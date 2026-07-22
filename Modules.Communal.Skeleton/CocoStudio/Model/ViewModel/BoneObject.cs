using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.Editor;
using CocoStudio.Model.Visiter;
using CocoStudio.UndoManager;
using CocoStudio.UndoManager.Recorder;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;
using Modules.Communal.Skeleton;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000024 RID: 36
	[DisplayName("Display_BoneObject")]
	[EngineClassName("BoneNode")]
	public class BoneObject : AbstractNodeObject, IBlendFunc
	{
		// Token: 0x06000171 RID: 369 RVA: 0x000082B3 File Offset: 0x000064B3
		private CSBoneNode GetInnerObject()
		{
			return (CSBoneNode)this.innerNode;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x000082C0 File Offset: 0x000064C0
		public BoneObject()
		{
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000082C8 File Offset: 0x000064C8
		public BoneObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000174 RID: 372 RVA: 0x000082D1 File Offset: 0x000064D1
		// (set) Token: 0x06000175 RID: 373 RVA: 0x000082DC File Offset: 0x000064DC
		[Browsable(true)]
		[UndoProperty]
		[DisplayName("Display_Name")]
		[Category("Group_Routine")]
		public override string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				bool isCreateDefaultRecorder = BaseRecorder.IsCreateDefaultRecorder;
				SkeletonObject skeletonObject = Services.Workbench.ActiveDocument.File.GetRootNode() as SkeletonObject;
				if (!isCreateDefaultRecorder || skeletonObject == null || !skeletonObject.IsNameExists(value))
				{
					if (value == null)
					{
						value = string.Empty;
					}
					base.Name = value;
					this.GetInnerObject().SetName(value);
					return;
				}
				string message = string.Format(LanguageInfo.Skeleton_BoneAlreadyExist, value);
				LogConfig.Output.Info(message, true);
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000834E File Offset: 0x0000654E
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			this._children = new BoneCollection(this);
			this.OperationFlag &= ~OperationMask.AnchorMoveFlag;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00008372 File Offset: 0x00006572
		protected override void CreateCSObject()
		{
			this.innerNode = new CSBoneNode();
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000837F File Offset: 0x0000657F
		protected internal override string GetNamePrefix()
		{
			return "Bone_";
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00008386 File Offset: 0x00006586
		// (set) Token: 0x0600017A RID: 378 RVA: 0x0000838E File Offset: 0x0000658E
		public override SizeF Size
		{
			get
			{
				return base.Size;
			}
			set
			{
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00008390 File Offset: 0x00006590
		// (set) Token: 0x0600017C RID: 380 RVA: 0x000083A0 File Offset: 0x000065A0
		[DisplayName("MainTool_Color")]
		[Category("Group_Routine")]
		[Browsable(true)]
		[PropertyOrder(13)]
		[UndoProperty]
		[FrameProperty]
		public virtual Color CColor
		{
			get
			{
				return this.GetCSVisual().GetColor();
			}
			set
			{
				this.GetCSVisual().SetColor(value);
				this.RaisePropertyChanged<Color>(() => this.CColor);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600017D RID: 381 RVA: 0x000083F3 File Offset: 0x000065F3
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00008400 File Offset: 0x00006600
		[DisplayName("Display_BoneLength")]
		[UndoProperty]
		[PropertyOrder(10)]
		[ValueRange(20, 1000000, 0.1f, 10f)]
		[Category("Group_PosAndSize")]
		[Browsable(true)]
		public float Length
		{
			get
			{
				return this.GetInnerObject().GetLength();
			}
			set
			{
				this.GetInnerObject().SetLength(value);
				this.RaisePropertyChanged<float>(() => this.Length);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00008453 File Offset: 0x00006653
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00008460 File Offset: 0x00006660
		[PropertyOrder(8)]
		[Editor(typeof(BoneObjectPositionEditor), typeof(BoneObjectPositionEditor))]
		[Browsable(true)]
		[UndoProperty]
		[FrameProperty(true)]
		[DisplayName("Display_Position")]
		[Category("Group_PosAndSize")]
		public override PointF Position
		{
			get
			{
				return this.GetInnerObject().GetPosition();
			}
			set
			{
				if (this.NeedRefreshStaticProperty())
				{
					PointF deltaValue = new PointF(value.X - this.Position.X, value.Y - this.Position.Y);
					this.RefreshStaticPropertyValue("Position", deltaValue);
				}
				this.GetInnerObject().SetPosition(value);
				this.RaisePropertyChanged<PointF>(() => this.Position);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000181 RID: 385 RVA: 0x000084F1 File Offset: 0x000066F1
		// (set) Token: 0x06000182 RID: 386 RVA: 0x00008500 File Offset: 0x00006700
		[Category("Group_Routine")]
		[PropertyOrder(9)]
		[UndoProperty]
		[FrameProperty(true)]
		[Editor(typeof(ScaleEditor), typeof(ScaleEditor))]
		[DisplayName("Display_Scale")]
		[Browsable(true)]
		public override ScaleValue Scale
		{
			get
			{
				return this.GetInnerObject().GetScale();
			}
			set
			{
				if (this.NeedRefreshStaticProperty())
				{
					ScaleValue deltaValue = new ScaleValue(value.ScaleX - this.Scale.ScaleX, value.ScaleY - this.Scale.ScaleY, 0.1, -99999999.0, 99999999.0);
					this.RefreshStaticPropertyValue("Scale", deltaValue);
				}
				this.GetInnerObject().SetScale(value);
				this.RaisePropertyChanged<ScaleValue>(() => this.Scale);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000183 RID: 387 RVA: 0x000085AC File Offset: 0x000067AC
		// (set) Token: 0x06000184 RID: 388 RVA: 0x000085BC File Offset: 0x000067BC
		[Browsable(true)]
		[PropertyOrder(10)]
		[DisplayName("Display_Rotation")]
		[Category("Group_Routine")]
		[DefaultValue(0f)]
		[Editor(typeof(RotationEditor), typeof(RotationEditor))]
		public override float Rotation
		{
			get
			{
				return this.GetInnerObject().GetRotationSkewX();
			}
			set
			{
				float num = value - this.Rotation;
				float scaleY = this.RotationSkew.ScaleY + num;
				this.RotationSkew = new ScaleValue(value, scaleY, 0.1, -99999999.0, 99999999.0);
				this.RaisePropertyChanged<float>(() => this.Rotation);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00008644 File Offset: 0x00006844
		// (set) Token: 0x06000186 RID: 390 RVA: 0x0000867C File Offset: 0x0000687C
		[DefaultValue(0f)]
		[Category("Group_Routine")]
		[Browsable(false)]
		[PropertyOrder(11)]
		[FrameProperty(true)]
		[UndoProperty]
		[Editor(typeof(SkewEditor), typeof(SkewEditor))]
		[DisplayName("Display_RotationSkew")]
		public override ScaleValue RotationSkew
		{
			get
			{
				return new ScaleValue(this.GetInnerObject().GetRotationSkewX(), this.GetInnerObject().GetRotationSkewY(), 0.1, -99999999.0, 99999999.0);
			}
			set
			{
				if (this.NeedRefreshStaticProperty())
				{
					ScaleValue deltaValue = new ScaleValue(value.ScaleX - this.RotationSkew.ScaleX, value.ScaleY - this.RotationSkew.ScaleY, 0.1, -99999999.0, 99999999.0);
					this.RefreshStaticPropertyValue("RotationSkew", deltaValue);
				}
				this.GetInnerObject().SetRotationSkewX(value.ScaleX);
				this.GetInnerObject().SetRotationSkewY(value.ScaleY);
				this.RaisePropertyChanged<ScaleValue>(() => this.RotationSkew);
				this.RaisePropertyChanged<float>(() => this.Rotation);
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00008778 File Offset: 0x00006978
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00008788 File Offset: 0x00006988
		public virtual float RotationSkewX
		{
			get
			{
				return this.GetInnerObject().GetRotationSkewX();
			}
			set
			{
				this.GetInnerObject().SetRotationSkewX(value);
				this.RaisePropertyChanged<float>(() => this.Rotation);
				this.RaisePropertyChanged<ScaleValue>(() => this.RotationSkew);
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00008815 File Offset: 0x00006A15
		// (set) Token: 0x0600018A RID: 394 RVA: 0x00008824 File Offset: 0x00006A24
		public virtual float RotationSkewY
		{
			get
			{
				return this.GetInnerObject().GetRotationSkewY();
			}
			set
			{
				this.GetInnerObject().SetRotationSkewY(value);
				this.RaisePropertyChanged<float>(() => this.Rotation);
				this.RaisePropertyChanged<ScaleValue>(() => this.RotationSkew);
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600018B RID: 395 RVA: 0x000088B1 File Offset: 0x00006AB1
		// (set) Token: 0x0600018C RID: 396 RVA: 0x000088BC File Offset: 0x00006ABC
		[UndoProperty]
		[FrameProperty]
		[Category("Group_Feature")]
		[DisplayName("Display_RenderLevel")]
		[DefaultValue(1)]
		[Browsable(true)]
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
					base.ZOrder = value;
					SkeletonObject skeletonObject = this.GetRootBone() as SkeletonObject;
					if (skeletonObject != null)
					{
						skeletonObject.SubBonesZOrderChangedHandler(this);
					}
					this.RaisePropertyChanged<int>(() => this.ZOrder);
				}
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00008929 File Offset: 0x00006B29
		// (set) Token: 0x0600018E RID: 398 RVA: 0x00008934 File Offset: 0x00006B34
		[Browsable(true)]
		[PropertyOrder(1)]
		[FrameProperty]
		[UndoProperty]
		[DisplayName("Display_Visible")]
		[Category("Group_Routine")]
		public override bool VisibleForFrame
		{
			get
			{
				return base.VisibleForFrame;
			}
			set
			{
				base.VisibleForFrame = value;
				this.RaisePropertyChanged<bool>(() => this.VisibleForFrame);
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00008982 File Offset: 0x00006B82
		// (set) Token: 0x06000190 RID: 400 RVA: 0x00008990 File Offset: 0x00006B90
		[Editor(typeof(SliderEditor), typeof(SliderEditor))]
		[FrameProperty]
		[UndoProperty]
		[ValueRange(0, 255, 1f, 10f)]
		[DisplayName("Display_Capacity")]
		[Category("Group_Routine")]
		[Browsable(true)]
		[PropertyOrder(12)]
		public override int Alpha
		{
			get
			{
				return this.GetCSVisual().GetAlpha();
			}
			set
			{
				if (this.GetCSVisual().GetAlpha() == value)
				{
					return;
				}
				this.GetCSVisual().SetAlpha(value);
				this.RaisePropertyChanged<int>(() => this.Alpha);
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000191 RID: 401 RVA: 0x000089F2 File Offset: 0x00006BF2
		// (set) Token: 0x06000192 RID: 402 RVA: 0x00008A14 File Offset: 0x00006C14
		[PropertyOrder(110)]
		[FrameProperty]
		[Category("Group_Feature")]
		[UndoProperty]
		[DisplayName("Animation_Blend_Blend")]
		[Editor(typeof(BlendFuncEditor), typeof(BlendFuncEditor))]
		[Browsable(true)]
		public virtual BlendFuncValue BlendFunc
		{
			get
			{
				if (this.blendFunc == null)
				{
					this.blendFunc = this.GetInnerObject().GetBlendFunc();
				}
				return this.blendFunc;
			}
			set
			{
				if (value != null && this.blendFunc != value)
				{
					this.blendFunc = value;
					this.GetInnerObject().SetBlendFunc(this.blendFunc);
				}
				this.RaisePropertyChanged<BlendFuncValue>(() => this.BlendFunc);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00008A7F File Offset: 0x00006C7F
		// (set) Token: 0x06000194 RID: 404 RVA: 0x00008A8C File Offset: 0x00006C8C
		[UndoProperty]
		[PropertyOrder(13)]
		[DisplayName("Display_BoneColor")]
		[Category("Group_Feature")]
		[Browsable(true)]
		public virtual Color BoneColor
		{
			get
			{
				return this.GetInnerObject().GetBoneRackColor();
			}
			set
			{
				this.GetInnerObject().SetBoneRackColor(value);
				this.RaisePropertyChanged<Color>(() => this.BoneColor);
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00008ADF File Offset: 0x00006CDF
		// (set) Token: 0x06000196 RID: 406 RVA: 0x00008AE7 File Offset: 0x00006CE7
		[Browsable(true)]
		[Category("Group_Feature")]
		[DisplayName("Display_Target")]
		[PropertyOrder(3)]
		[UndoProperty]
		[DefaultValue(-1)]
		public override int Tag
		{
			get
			{
				return base.Tag;
			}
			set
			{
				base.Tag = value;
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00008AF0 File Offset: 0x00006CF0
		protected override void SetValue(object cObject)
		{
			BoneObject boneObject = cObject as BoneObject;
			if (boneObject == null)
			{
				return;
			}
			boneObject.ScriptData = this.ScriptData;
			boneObject.name = this.Name;
			boneObject.GetCSVisual().SetName(boneObject.name);
			boneObject.CanEdit = this.CanEdit;
			boneObject.OperationFlag = this.OperationFlag;
			boneObject.Alpha = this.Alpha;
			boneObject.Visible = this.Visible;
			boneObject.ZOrder = this.ZOrder;
			boneObject.VisibleForFrame = this.VisibleForFrame;
			boneObject.Parent = base.Parent;
			boneObject.FrameEvent = this.FrameEvent;
			boneObject.CustomClassName = this.CustomClassName;
			boneObject.CallBackName = this.CallBackName;
			boneObject.CallBackType = this.CallBackType;
			boneObject.UserData = this.UserData;
			boneObject.CColor = this.CColor;
			boneObject.Length = this.Length;
			boneObject.Scale = this.Scale;
			boneObject.BoneColor = this.BoneColor;
			boneObject.BlendFunc = this.BlendFunc;
			boneObject.Rotation = this.Rotation;
			boneObject.Position = this.Position;
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00008C15 File Offset: 0x00006E15
		public SizeF BoxSize
		{
			get
			{
				return this.GetInnerObject().GetBoxSize();
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00008C22 File Offset: 0x00006E22
		[UndoProperty]
		public override NodeCollection Children
		{
			get
			{
				return this._children;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00008C2A File Offset: 0x00006E2A
		public IReadOnlyList<BoneObject> Bones
		{
			get
			{
				return this._children.Bones;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00008C37 File Offset: 0x00006E37
		public IReadOnlyList<NodeObject> Skins
		{
			get
			{
				return this._children.Skins;
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00008C44 File Offset: 0x00006E44
		public List<BoneObject> GetAllSubBones()
		{
			List<BoneObject> list = new List<BoneObject>();
			Stack<BoneObject> stack = new Stack<BoneObject>();
			using (IEnumerator<BoneObject> enumerator = this.Bones.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BoneObject item = enumerator.Current;
					stack.Push(item);
				}
				goto IL_86;
			}
			IL_41:
			BoneObject boneObject = stack.Pop();
			list.Add(boneObject);
			foreach (BoneObject item2 in boneObject.Bones)
			{
				stack.Push(item2);
			}
			IL_86:
			if (stack.Count <= 0)
			{
				return list;
			}
			goto IL_41;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00008D00 File Offset: 0x00006F00
		public BoneObject GetRootBone()
		{
			AbstractNodeObject abstractNodeObject = this;
			while (abstractNodeObject.Parent != null)
			{
				abstractNodeObject = abstractNodeObject.Parent;
			}
			return abstractNodeObject as BoneObject;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00008D28 File Offset: 0x00006F28
		private bool NeedRefreshStaticProperty()
		{
			bool needRefreshAnimate = TimelineActionManager.Instance.NeedRefreshAnimate;
			return needRefreshAnimate & (!TimelineActionManager.Instance.AutoKey && TimelineActionManager.Instance.CanAutoKey);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00008D74 File Offset: 0x00006F74
		private void RefreshStaticPropertyValue(string propertyName, object deltaValue)
		{
			Timeline timeline = null;
			foreach (Timeline timeline2 in base.Timelines)
			{
				if (timeline2.PropertyInfo.Name == propertyName)
				{
					timeline = timeline2;
					break;
				}
			}
			if (timeline != null)
			{
				Parallel.ForEach<Frame>(timeline.Frames, delegate(Frame f)
				{
					f.UpdateValue(deltaValue);
				});
			}
		}

		// Token: 0x0400007E RID: 126
		public static readonly int MINLENGHT = 20;

		// Token: 0x0400007F RID: 127
		private BlendFuncValue blendFunc;

		// Token: 0x04000080 RID: 128
		private BoneCollection _children;
	}
}
