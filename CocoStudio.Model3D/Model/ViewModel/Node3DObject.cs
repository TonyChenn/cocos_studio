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
	// Token: 0x02000020 RID: 32
	[ModelExtension(true, 103, EnumModelType.ThreeDimensional)]
	[DisplayName("Display_Component_Node3D")]
	[ControlGroup("Control_3DControl", 0)]
	public class Node3DObject : AbstractNodeObject
	{
		// Token: 0x060000FE RID: 254 RVA: 0x000050F9 File Offset: 0x000032F9
		public Node3DObject()
		{
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005109 File Offset: 0x00003309
		public Node3DObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000511A File Offset: 0x0000331A
		protected override void CreateCSObject()
		{
			this.innerNode = new CSNode3D();
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005127 File Offset: 0x00003327
		private CSNode3D GetInnerObject()
		{
			return this.innerNode as CSNode3D;
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00005134 File Offset: 0x00003334
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00005144 File Offset: 0x00003344
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

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000104 RID: 260 RVA: 0x000051A1 File Offset: 0x000033A1
		// (set) Token: 0x06000105 RID: 261 RVA: 0x000051B0 File Offset: 0x000033B0
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

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000106 RID: 262 RVA: 0x0000520D File Offset: 0x0000340D
		// (set) Token: 0x06000107 RID: 263 RVA: 0x0000521C File Offset: 0x0000341C
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

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000108 RID: 264 RVA: 0x0000527F File Offset: 0x0000347F
		// (set) Token: 0x06000109 RID: 265 RVA: 0x00005288 File Offset: 0x00003488
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

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600010A RID: 266 RVA: 0x000052EB File Offset: 0x000034EB
		// (set) Token: 0x0600010B RID: 267 RVA: 0x000052F3 File Offset: 0x000034F3
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

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600010C RID: 268 RVA: 0x000052FC File Offset: 0x000034FC
		// (set) Token: 0x0600010D RID: 269 RVA: 0x00005304 File Offset: 0x00003504
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

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600010E RID: 270 RVA: 0x0000530D File Offset: 0x0000350D
		// (set) Token: 0x0600010F RID: 271 RVA: 0x0000531C File Offset: 0x0000351C
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000110 RID: 272 RVA: 0x0000537E File Offset: 0x0000357E
		// (set) Token: 0x06000111 RID: 273 RVA: 0x0000538C File Offset: 0x0000358C
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

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000112 RID: 274 RVA: 0x000053DF File Offset: 0x000035DF
		public virtual Color DisplayColor
		{
			get
			{
				return this.GetCSVisual().GetDisplayColor();
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000113 RID: 275 RVA: 0x000053EC File Offset: 0x000035EC
		public virtual CameraFlag MultipleComboxType
		{
			get
			{
				return (CameraFlag)this.GetInnerObject().GetNodeCameraMask();
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000114 RID: 276 RVA: 0x000053F9 File Offset: 0x000035F9
		public virtual Point3F WorldPosition3D
		{
			get
			{
				return this.GetCSVisual().GetWorldPosition();
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005408 File Offset: 0x00003608
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

		// Token: 0x06000116 RID: 278 RVA: 0x00005474 File Offset: 0x00003674
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

		// Token: 0x06000117 RID: 279 RVA: 0x000054E8 File Offset: 0x000036E8
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

		// Token: 0x06000118 RID: 280 RVA: 0x0000554C File Offset: 0x0000374C
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

		// Token: 0x06000119 RID: 281 RVA: 0x00005584 File Offset: 0x00003784
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

		// Token: 0x0600011A RID: 282 RVA: 0x000055CE File Offset: 0x000037CE
		protected virtual Point3F Unproject(CSCamera camera, PointF point)
		{
			return camera.Unproject(point);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000055D8 File Offset: 0x000037D8
		internal override void InsertChild(int index, AbstractNodeObject nObject)
		{
			base.InsertChild(index, nObject);
			Node3DObject node3DObject = nObject as Node3DObject;
			if (node3DObject != null)
			{
				node3DObject.RefreshObjectRenderFace();
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000055FD File Offset: 0x000037FD
		public override bool CanReceiveDragObject(ModelDragData objectData, bool showLog)
		{
			return false;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005600 File Offset: 0x00003800
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

		// Token: 0x0600011E RID: 286 RVA: 0x00005668 File Offset: 0x00003868
		protected override void OnBindingRecorder()
		{
			ExtenderFactory.Binding(this, new BaseExtender[]
			{
				new ResourceExtender(this)
			});
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000568C File Offset: 0x0000388C
		public virtual void SetPixelRenderMode(Color color)
		{
			this.GetInnerObject().SetPixelRenderMode(color);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000569A File Offset: 0x0000389A
		public virtual void RestoreRenderMode()
		{
			this.GetInnerObject().RestoreRenderMode();
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000056A7 File Offset: 0x000038A7
		protected virtual void RefreshBaseProperties()
		{
		}

		// Token: 0x0400007A RID: 122
		public const ushort DefaultSkyboxFlag = 1024;

		// Token: 0x0400007B RID: 123
		public const ushort DefaultCameraFlag = 1;

		// Token: 0x0400007C RID: 124
		private int _cameraFlagMode = 30;
	}
}
