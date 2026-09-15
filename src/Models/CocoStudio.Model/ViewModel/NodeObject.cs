using System;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.Editor;
using CocoStudio.UndoManager;
using CocoStudio.UndoManager.Recorder;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	public class NodeObject : AbstractNodeObject
	{
		public NodeObject()
		{
		}

		public NodeObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSNode2D();
		}

		private CSNode2D GetInnerObject()
		{
			return this.innerNode as CSNode2D;
		}

		protected void InitIcon(string iconFile)
		{
			this.GetInnerObject().InitIcon(iconFile);
		}

		protected void InitIcon(SizeF iconSize)
		{
			this.GetInnerObject().InitIcon(iconSize);
		}

		[PropertyOrder(16)]
		[UndoProperty]
		[LayoutRefresh]
		[Browsable(true)]
		[RequestOperationMode(OperationMask.SizeFlag)]
		[Editor(typeof(UISizeEditor), typeof(UISizeEditor))]
		[DisplayName("grid_sudoku")]
		[Category("Group_PosAndSize")]
		public override SizeF Size
		{
			get
			{
				return this.GetInnerObject().GetSize();
			}
			set
			{
				if (value != null)
				{
					this.GetInnerObject().SetSize(value);
					this.RaisePropertyChanged<SizeF>(() => this.Size);
				}
			}
		}

		[RequestOperationMode(OperationMask.MoveFlag)]
		[Browsable(true)]
		[Editor(typeof(PositionEditor), typeof(PositionEditor))]
		[PropertyOrder(8)]
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
				this.GetInnerObject().SetPosition(value);
				this.RaisePropertyChanged<PointF>(() => this.Position);
			}
		}

		[RequestOperationMode(OperationMask.AnchorMoveFlag)]
		[FrameProperty]
		[PropertyOrder(7)]
		[UndoProperty]
		[DisplayName("Display_AnchorPoint")]
		[Browsable(true)]
		[Editor(typeof(AnchorPointEditor), typeof(AnchorPointEditor))]
		[Category("Group_PosAndSize")]
		public override ScaleValue AnchorPoint
		{
			get
			{
				return this.GetInnerObject().GetAnchorPoint();
			}
			set
			{
				if (value != null)
				{
					this.GetInnerObject().SetAnchorPoint(value);
					string taskName = base.GetType().Name + "AnchorPoint";
					using (CompositeTask.Run(taskName, null))
					{
						DefaultRecorder defaultRecorder = base.Recorder as DefaultRecorder;
						if (defaultRecorder != null)
						{
							defaultRecorder.UpdateCachedValue("PrePosition", this.PrePosition);
							defaultRecorder.UpdateCachedValue("Position", this.Position);
						}
						this.RaisePropertyChanged<ScaleValue>(() => this.AnchorPoint);
					}
				}
			}
		}

		[Browsable(true)]
		[Category("Group_Routine")]
		[DisplayName("Display_Scale")]
		[RequestOperationMode(OperationMask.MoveFlag)]
		[PropertyOrder(9)]
		[FrameProperty(true)]
		[Editor(typeof(ScaleEditor), typeof(ScaleEditor))]
		[UndoProperty]
		public override ScaleValue Scale
		{
			get
			{
				return this.GetInnerObject().GetScale();
			}
			set
			{
				if (value != null)
				{
					this.GetInnerObject().SetScale(value);
					this.RaisePropertyChanged<ScaleValue>(() => this.Scale);
				}
			}
		}

		[UndoProperty]
		public override bool UniformScale
		{
			get
			{
				return this.uniformScale;
			}
			set
			{
				this.uniformScale = value;
				this.RaisePropertyChanged<bool>(() => this.UniformScale);
			}
		}

		[Browsable(true)]
		[Category("Group_Routine")]
		[RequestOperationMode(OperationMask.MoveFlag)]
		[Editor(typeof(RotationEditor), typeof(RotationEditor))]
		[PropertyOrder(10)]
		[DefaultValue(0f)]
		[DisplayName("Display_Rotation")]
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
			}
		}

		[DefaultValue(0f)]
		[Category("Group_Routine")]
		[Browsable(true)]
		[PropertyOrder(11)]
		[DisplayName("Display_RotationSkew")]
		[FrameProperty(true)]
		[UndoProperty]
		[RequestOperationMode(OperationMask.MoveFlag)]
		[Editor(typeof(SkewEditor), typeof(SkewEditor))]
		public override ScaleValue RotationSkew
		{
			get
			{
				return new ScaleValue(this.GetInnerObject().GetRotationSkewX(), this.GetInnerObject().GetRotationSkewY(), 0.1, -99999999.0, 99999999.0);
			}
			set
			{
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
		[PropertyOrder(13)]
		[DisplayName("Display_RenderLevel")]
		[FrameProperty]
		[Category("Group_Routine")]
		[DefaultValue(1)]
		[Browsable(false)]
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

		[FrameProperty]
		[DisplayName("Display_Visible")]
		[Category("Group_Routine")]
		[PropertyOrder(1)]
		[Browsable(true)]
		[RequestOperationMode(OperationMask.VisibleFlag)]
		[UndoProperty]
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

		[Category("Group_Routine")]
		[Browsable(true)]
		[Editor(typeof(SliderEditor), typeof(SliderEditor))]
		[PropertyOrder(12)]
		[FrameProperty]
		[UndoProperty]
		[ValueRange(0, 255, 1f, 10f)]
		[DisplayName("Display_Capacity")]
		public override int Alpha
		{
			get
			{
				return this.GetCSVisual().GetAlpha();
			}
			set
			{
				if (this.GetCSVisual().GetAlpha() != value)
				{
					this.GetCSVisual().SetAlpha(value);
					this.RaisePropertyChanged<int>(() => this.Alpha);
				}
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

		[UndoProperty]
		public virtual bool PercentWidthEnable
		{
			get
			{
				return this.GetInnerObject().GetPercentWidthEnable();
			}
			set
			{
				this.GetInnerObject().SetPercentWidthEnable(value);
				this.RaisePropertyChanged<bool>(() => this.PercentWidthEnable);
			}
		}

		[UndoProperty]
		public virtual bool PercentHeightEnable
		{
			get
			{
				return this.GetInnerObject().GetPercentHeightEnable();
			}
			set
			{
				this.GetInnerObject().SetPercentHeightEnable(value);
				this.RaisePropertyChanged<bool>(() => this.PercentHeightEnable);
			}
		}

		[LayoutRefresh]
		[UndoProperty]
		public virtual SizeF PreSize
		{
			get
			{
				return new SizeF(this.GetInnerObject().GetPercentWidth(), this.GetInnerObject().GetPercentHeight());
			}
			set
			{
				if (value != null)
				{
					this.GetInnerObject().SetPercentWidth(value.Width);
					this.GetInnerObject().SetPercentHeight(value.Height);
					this.RaisePropertyChanged<SizeF>(() => this.PreSize);
				}
			}
		}

		[UndoProperty]
		public virtual bool PositionPercentXEnabled
		{
			get
			{
				return this.GetInnerObject().IsUsingPositionPercentX();
			}
			set
			{
				this.GetInnerObject().SetPositionPercentXEnabled(value);
				string taskName = base.GetType().Name + "PositionPercentXEnabled";
				using (CompositeTask.Run(taskName, null))
				{
					if (value)
					{
						this.HorizontalEdge = HorizontalBerthEdge.None;
					}
					this.RaisePropertyChanged<bool>(() => this.PositionPercentXEnabled);
				}
			}
		}

		[UndoProperty]
		public virtual bool PositionPercentYEnabled
		{
			get
			{
				return this.GetInnerObject().IsUsingPositionPercentY();
			}
			set
			{
				this.GetInnerObject().SetPositionPercentYEnabled(value);
				string taskName = base.GetType().Name + "PositionPercentYEnabled";
				using (CompositeTask.Run(taskName, null))
				{
					if (value)
					{
						this.VerticalEdge = VerticalBerthEdge.None;
					}
					this.RaisePropertyChanged<bool>(() => this.PositionPercentYEnabled);
				}
			}
		}

		[LayoutRefresh]
		[UndoProperty]
		public virtual PointF PrePosition
		{
			get
			{
				return new PointF(this.GetInnerObject().GetPositionPercentX(), this.GetInnerObject().GetPositionPercentY());
			}
			set
			{
				if (!(value == null))
				{
					this.GetInnerObject().SetPositionPercentX(value.X);
					this.GetInnerObject().SetPositionPercentY(value.Y);
					this.RaisePropertyChanged<PointF>(() => this.PrePosition);
				}
			}
		}

		[Category("Group_PosAndSize")]
		[PropertyOrder(17)]
		[LayoutRefresh]
		[RequestOperationMode(OperationMask.LayoutFlag)]
		[DisplayName("")]
		[Browsable(true)]
		[UndoProperty]
		[Editor(typeof(FixZoomEditor), typeof(FixZoomEditor))]
		public virtual HorizontalBerthEdge HorizontalEdge
		{
			get
			{
				return (HorizontalBerthEdge)this.GetInnerObject().GetHorizontalEdge();
			}
			set
			{
				this.GetInnerObject().SetHorizontalEdge((int)value);
				string taskName = base.GetType().Name + "HorizontalEdge";
				using (CompositeTask.Run(taskName, null))
				{
					if (value != HorizontalBerthEdge.None)
					{
						this.PositionPercentXEnabled = false;
					}
					this.RaisePropertyChanged<PointF>(() => this.Position);
					this.RaisePropertyChanged<HorizontalBerthEdge>(() => this.HorizontalEdge);
				}
			}
		}

		[UndoProperty]
		[LayoutRefresh]
		public virtual VerticalBerthEdge VerticalEdge
		{
			get
			{
				return (VerticalBerthEdge)this.GetInnerObject().GetVerticalEdge();
			}
			set
			{
				this.GetInnerObject().SetVerticalEdge((int)value);
				string taskName = base.GetType().Name + "VerticalEdge";
				using (CompositeTask.Run(taskName, null))
				{
					if (value != VerticalBerthEdge.None)
					{
						this.PositionPercentYEnabled = false;
					}
					this.RaisePropertyChanged<PointF>(() => this.Position);
					this.RaisePropertyChanged<VerticalBerthEdge>(() => this.VerticalEdge);
				}
			}
		}

		[UndoProperty]
		[LayoutRefresh]
		public virtual float LeftMargin
		{
			get
			{
				return this.GetInnerObject().GetLeftMargin();
			}
			set
			{
				this.GetInnerObject().SetLeftMargin(value);
				this.RaisePropertyChanged<float>(() => this.LeftMargin);
			}
		}

		[LayoutRefresh]
		[UndoProperty]
		public virtual float RightMargin
		{
			get
			{
				return this.GetInnerObject().GetRightMargin();
			}
			set
			{
				this.GetInnerObject().SetRightMargin(value);
				this.RaisePropertyChanged<float>(() => this.RightMargin);
			}
		}

		[LayoutRefresh]
		[UndoProperty]
		public virtual float TopMargin
		{
			get
			{
				return this.GetInnerObject().GetTopMargin();
			}
			set
			{
				this.GetInnerObject().SetTopMargin(value);
				this.RaisePropertyChanged<float>(() => this.TopMargin);
			}
		}

		[LayoutRefresh]
		[UndoProperty]
		public virtual float BottomMargin
		{
			get
			{
				return this.GetInnerObject().GetBottomMargin();
			}
			set
			{
				this.GetInnerObject().SetBottomMargin(value);
				this.RaisePropertyChanged<float>(() => this.BottomMargin);
			}
		}

		public bool LayoutState
		{
			get
			{
				return this._layoutState;
			}
			set
			{
				this._layoutState = value;
				this.RaisePropertyChanged<bool>(() => this.LayoutState);
			}
		}

		public virtual void ResetSize()
		{
			this.RaisePropertyChanged<SizeF>(() => this.Size);
		}

		internal override void OnResourcePropertyChanged()
		{
			this.ResetSize();
		}

		public SizeF BoxSize
		{
			get
			{
				return this.GetInnerObject().GetBoxSize();
			}
		}

		public ScaleValue BoxAnchorPoint
		{
			get
			{
				return this.GetInnerObject().GetBoxAnchorPoint();
			}
		}

		public bool IconVisible
		{
			get
			{
				return this.GetInnerObject().GetIconVisible();
			}
			set
			{
				this.GetInnerObject().SetIconVisible(value);
			}
		}

		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			NodeObject nodeObject = cObject as NodeObject;
			if (nodeObject != null)
			{
				nodeObject.Alpha = this.Alpha;
				nodeObject.CColor = this.CColor;
				nodeObject.Scale = this.Scale;
				nodeObject.AnchorPoint = this.AnchorPoint;
				nodeObject.Position = this.Position;
				nodeObject.RotationSkew = this.RotationSkew;
				nodeObject.PositionPercentXEnabled = this.PositionPercentXEnabled;
				nodeObject.PositionPercentYEnabled = this.PositionPercentYEnabled;
				nodeObject.PrePosition = this.PrePosition;
				nodeObject.PercentWidthEnable = this.PercentWidthEnable;
				nodeObject.PercentHeightEnable = this.PercentHeightEnable;
				nodeObject.PreSize = this.PreSize;
				nodeObject.HorizontalEdge = this.HorizontalEdge;
				nodeObject.VerticalEdge = this.VerticalEdge;
				nodeObject.LeftMargin = this.LeftMargin;
				nodeObject.RightMargin = this.RightMargin;
				nodeObject.TopMargin = this.TopMargin;
				nodeObject.BottomMargin = this.BottomMargin;
				nodeObject.IconVisible = this.IconVisible;
			}
		}

		protected internal virtual bool IsCanChangeSize()
		{
			return false;
		}

		protected override void OnBindingRecorder()
		{
			ExtenderFactory.Binding(this, new BaseExtender[]
			{
				new LayoutExtender(this),
				new ResourceExtender(this)
			});
		}

		internal override void AfterAdded()
		{
			LayoutExtender.RefreshLayout(this);
		}

		private bool uniformScale = false;

		private bool _layoutState = false;
	}
}
