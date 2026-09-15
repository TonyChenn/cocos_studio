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
	[DisplayName("Display_BoneObject")]
	[EngineClassName("BoneNode")]
	public class BoneObject : AbstractNodeObject, IBlendFunc
	{
		private CSBoneNode GetInnerObject()
		{
			return (CSBoneNode)this.innerNode;
		}

		public BoneObject()
		{
		}

		public BoneObject(ScriptFileData fileData) : base(fileData)
		{
		}

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

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			this._children = new BoneCollection(this);
			this.OperationFlag &= ~OperationMask.AnchorMoveFlag;
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSBoneNode();
		}

		protected internal override string GetNamePrefix()
		{
			return "Bone_";
		}

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

		public SizeF BoxSize
		{
			get
			{
				return this.GetInnerObject().GetBoxSize();
			}
		}

		[UndoProperty]
		public override NodeCollection Children
		{
			get
			{
				return this._children;
			}
		}

		public IReadOnlyList<BoneObject> Bones
		{
			get
			{
				return this._children.Bones;
			}
		}

		public IReadOnlyList<NodeObject> Skins
		{
			get
			{
				return this._children.Skins;
			}
		}

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

		public BoneObject GetRootBone()
		{
			AbstractNodeObject abstractNodeObject = this;
			while (abstractNodeObject.Parent != null)
			{
				abstractNodeObject = abstractNodeObject.Parent;
			}
			return abstractNodeObject as BoneObject;
		}

		private bool NeedRefreshStaticProperty()
		{
			bool needRefreshAnimate = TimelineActionManager.Instance.NeedRefreshAnimate;
			return needRefreshAnimate & (!TimelineActionManager.Instance.AutoKey && TimelineActionManager.Instance.CanAutoKey);
		}

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

		public static readonly int MINLENGHT = 20;

		private BlendFuncValue blendFunc;

		private BoneCollection _children;
	}
}
