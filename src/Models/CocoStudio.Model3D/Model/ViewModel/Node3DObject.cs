using System;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.Editor;
using CocoStudio.Model.ViewModel.HitTest;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	[ModelExtension(true, 103, EnumModelType.ThreeDimensional)]
	[DisplayName("Display_Component_Node3D")]
	[ControlGroup("Control_3DControl", 0)]
	public class Node3DObject : AbstractNodeObject
	{
		public Node3DObject()
		{
		}

		public Node3DObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSNode3D();
		}

		private CSNode3D GetInnerObject()
		{
			return this.innerNode as CSNode3D;
		}

		[DisplayName("Display_Position3D")]
		[PropertyOrder(8)]
		[UndoProperty]
		[Editor(typeof(Position3DEditor), typeof(Position3DEditor))]
		[Category("Group_Routine")]
		[Browsable(true)]
		public virtual Point3F Position3D
		{
			get
			{
				return this.GetInnerObject().GetPosition3D();
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this.GetInnerObject().SetPosition3D(value);
				this.RaisePropertyChanged<Point3F>(() => this.Position3D);
				this.RefreshBaseProperties();
			}
		}

		[Editor(typeof(Rotation3DEditor), typeof(Rotation3DEditor))]
		[PropertyOrder(10)]
		[Category("Group_Routine")]
		[Browsable(true)]
		[UndoProperty]
		[DisplayName("Display_Rotation")]
		public virtual Point3F Rotation3D
		{
			get
			{
				return this.GetInnerObject().GetRotation3D();
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this.GetInnerObject().SetRotation3D(value);
				this.RaisePropertyChanged<Point3F>(() => this.Rotation3D);
				this.RefreshBaseProperties();
			}
		}

		[Editor(typeof(Scale3DEditor), typeof(Scale3DEditor))]
		[Browsable(true)]
		[PropertyOrder(9)]
		[Category("Group_Routine")]
		[UndoProperty]
		[DisplayName("Display_Scale")]
		public virtual Point3F Scale3D
		{
			get
			{
				return this.GetInnerObject().GetScale3D();
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this.GetInnerObject().SetScale3D(value);
				this.RefreshObjectRenderFace();
				this.RaisePropertyChanged<Point3F>(() => this.Scale3D);
				this.RefreshBaseProperties();
			}
		}

		[DefaultValue("")]
		[Category("Group_Routine")]
		[UndoProperty]
		[PropertyOrder(21)]
		[Editor(typeof(MultipleComboxEditor), typeof(MultipleComboxEditor))]
		[DisplayName("Display_CameraFlag")]
		public virtual int CameraFlagMode
		{
			get
			{
				return this._cameraFlagMode;
			}
			set
			{
				bool applyChildren = (value & 128) != 0;
				value &= 127;
				this.SetCameraMask(value, applyChildren);
				this.RaisePropertyChanged<int>(() => this.CameraFlagMode);
			}
		}

		[PropertyOrder(13)]
		[Category("Group_Routine")]
		[DefaultValue(1)]
		[Browsable(false)]
		[FrameProperty]
		[UndoProperty]
		[DisplayName("Display_RenderLevel")]
		public override int ZOrder
		{
			get
			{
				return base.ZOrder;
			}
			set
			{
				base.ZOrder = value;
			}
		}

		[Category("Group_Routine")]
		[PropertyOrder(1)]
		[DisplayName("Display_Visible")]
		[UndoProperty]
		[Browsable(true)]
		[FrameProperty]
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

		[FrameProperty]
		[Browsable(true)]
		[Editor(typeof(SliderEditor), typeof(SliderEditor))]
		[PropertyOrder(12)]
		[UndoProperty]
		[ValueRange(0, 255, 1f, 10f)]
		[DisplayName("Display_Capacity")]
		[Category("Group_Routine")]
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

		[PropertyOrder(13)]
		[FrameProperty]
		[UndoProperty]
		[DisplayName("MainTool_Color")]
		[Category("Group_Routine")]
		[Browsable(true)]
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

		public virtual Color DisplayColor
		{
			get
			{
				return this.GetCSVisual().GetDisplayColor();
			}
		}

		public virtual CameraFlag MultipleComboxType
		{
			get
			{
				return (CameraFlag)this.GetInnerObject().GetNodeCameraMask();
			}
		}

		public virtual Point3F WorldPosition3D
		{
			get
			{
				return this.GetCSVisual().GetWorldPosition();
			}
		}

		public virtual void Rotate3D(Quaternion q, Node3DObject target = null)
		{
			if (target == null)
			{
				this.GetInnerObject().Rotate3D(q, CSVisualObject.TransformSpace.TS_LOCAL);
			}
			else
			{
				this.GetInnerObject().Rotate3D(q, target.GetInnerObject());
			}
			this.RaisePropertyChanged<Point3F>(() => this.Rotation3D);
		}

		private void SetCameraMask(int flag, bool applyChildren)
		{
			uint mask = (uint)(flag | 1);
			this.GetInnerObject().SetNodeCameraMask(mask, false);
			this._cameraFlagMode = flag;
			if (applyChildren)
			{
				foreach (AbstractNodeObject abstractNodeObject in this.Children)
				{
					Node3DObject node3DObject = abstractNodeObject as Node3DObject;
					if (node3DObject != null)
					{
						node3DObject.SetCameraMask(flag, applyChildren);
					}
				}
			}
		}

		private void RefreshObjectRenderFace()
		{
			this.GetInnerObject().RefreshObjectFace();
			foreach (AbstractNodeObject abstractNodeObject in this.Children)
			{
				Node3DObject node3DObject = abstractNodeObject as Node3DObject;
				if (node3DObject != null)
				{
					node3DObject.RefreshObjectRenderFace();
				}
			}
		}

		protected override RectTestResult RectTestCore(RectF rect)
		{
			bool flag = this.GetCSVisual().RectTest3D(rect);
			RectTestResult result;
			if (flag)
			{
				result = new RectTestResult(this, rect);
			}
			else
			{
				result = new RectTestResult(rect, this.CanContinueTest());
			}
			return result;
		}

		protected override HitTestResult HitTestCore(PointF point)
		{
			float num = this.GetCSVisual().HitTest3D(point);
			HitTestResult hitTestResult;
			if (num != -1f)
			{
				hitTestResult = new HitTestResult(this, point, MouseOperationType.OPERATION_POSITION, ControlPointType.POINT_NONE);
				hitTestResult.Distance = num;
			}
			else
			{
				hitTestResult = new HitTestResult(point, this.CanContinueTest());
				hitTestResult.Distance = num;
			}
			return hitTestResult;
		}

		protected virtual Point3F Unproject(CSCamera camera, PointF point)
		{
			return camera.Unproject(point);
		}

		internal override void InsertChild(int index, AbstractNodeObject nObject)
		{
			base.InsertChild(index, nObject);
			Node3DObject node3DObject = nObject as Node3DObject;
			if (node3DObject != null)
			{
				node3DObject.RefreshObjectRenderFace();
			}
		}

		public override bool CanReceiveDragObject(ModelDragData objectData, bool showLog)
		{
			return false;
		}

		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			Node3DObject node3DObject = cObject as Node3DObject;
			if (node3DObject == null)
			{
				return;
			}
			node3DObject.Alpha = this.Alpha;
			node3DObject.CColor = this.CColor;
			node3DObject.Position3D = this.Position3D;
			node3DObject.Rotation3D = this.Rotation3D;
			node3DObject.Scale3D = this.Scale3D;
			node3DObject.CameraFlagMode = this.CameraFlagMode;
		}

		protected override void OnBindingRecorder()
		{
			ExtenderFactory.Binding(this, new BaseExtender[]
			{
				new ResourceExtender(this)
			});
		}

		public virtual void SetPixelRenderMode(Color color)
		{
			this.GetInnerObject().SetPixelRenderMode(color);
		}

		public virtual void RestoreRenderMode()
		{
			this.GetInnerObject().RestoreRenderMode();
		}

		protected virtual void RefreshBaseProperties()
		{
		}

		public const ushort DefaultSkyboxFlag = 1024;

		public const ushort DefaultCameraFlag = 1;

		private int _cameraFlagMode = 30;
	}
}
