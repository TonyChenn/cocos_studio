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
	// Token: 0x020000F0 RID: 240
	public class NodeObject : AbstractNodeObject
	{
		// Token: 0x060007EE RID: 2030 RVA: 0x0001FAED File Offset: 0x0001DCED
		public NodeObject()
		{
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x0001FB06 File Offset: 0x0001DD06
		public NodeObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0001FB20 File Offset: 0x0001DD20
		protected override void CreateCSObject()
		{
			this.innerNode = new CSNode2D();
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0001FB30 File Offset: 0x0001DD30
		private CSNode2D GetInnerObject()
		{
			return this.innerNode as CSNode2D;
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0001FB4D File Offset: 0x0001DD4D
		protected void InitIcon(string iconFile)
		{
			this.GetInnerObject().InitIcon(iconFile);
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x0001FB5D File Offset: 0x0001DD5D
		protected void InitIcon(SizeF iconSize)
		{
			this.GetInnerObject().InitIcon(iconSize);
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x0001FB70 File Offset: 0x0001DD70
		// (set) Token: 0x060007F5 RID: 2037 RVA: 0x0001FB90 File Offset: 0x0001DD90
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

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x0001FBF4 File Offset: 0x0001DDF4
		// (set) Token: 0x060007F7 RID: 2039 RVA: 0x0001FC14 File Offset: 0x0001DE14
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

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x0001FC6C File Offset: 0x0001DE6C
		// (set) Token: 0x060007F9 RID: 2041 RVA: 0x0001FC8C File Offset: 0x0001DE8C
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

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x0001FD70 File Offset: 0x0001DF70
		// (set) Token: 0x060007FB RID: 2043 RVA: 0x0001FD90 File Offset: 0x0001DF90
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

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x0001FDF4 File Offset: 0x0001DFF4
		// (set) Token: 0x060007FD RID: 2045 RVA: 0x0001FE0C File Offset: 0x0001E00C
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

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060007FE RID: 2046 RVA: 0x0001FE5C File Offset: 0x0001E05C
		// (set) Token: 0x060007FF RID: 2047 RVA: 0x0001FE7C File Offset: 0x0001E07C
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

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x0001FECC File Offset: 0x0001E0CC
		// (set) Token: 0x06000801 RID: 2049 RVA: 0x0001FF14 File Offset: 0x0001E114
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

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x0001FFBC File Offset: 0x0001E1BC
		// (set) Token: 0x06000803 RID: 2051 RVA: 0x0001FFDC File Offset: 0x0001E1DC
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

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x00020070 File Offset: 0x0001E270
		// (set) Token: 0x06000805 RID: 2053 RVA: 0x00020090 File Offset: 0x0001E290
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

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000806 RID: 2054 RVA: 0x00020124 File Offset: 0x0001E324
		// (set) Token: 0x06000807 RID: 2055 RVA: 0x0002013C File Offset: 0x0001E33C
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

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x00020148 File Offset: 0x0001E348
		// (set) Token: 0x06000809 RID: 2057 RVA: 0x00020160 File Offset: 0x0001E360
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

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x0002016C File Offset: 0x0001E36C
		// (set) Token: 0x0600080B RID: 2059 RVA: 0x0002018C File Offset: 0x0001E38C
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

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x0600080C RID: 2060 RVA: 0x000201FC File Offset: 0x0001E3FC
		// (set) Token: 0x0600080D RID: 2061 RVA: 0x0002021C File Offset: 0x0001E41C
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

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x0600080E RID: 2062 RVA: 0x00020274 File Offset: 0x0001E474
		// (set) Token: 0x0600080F RID: 2063 RVA: 0x00020294 File Offset: 0x0001E494
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

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x000202EC File Offset: 0x0001E4EC
		// (set) Token: 0x06000811 RID: 2065 RVA: 0x0002030C File Offset: 0x0001E50C
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

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x00020364 File Offset: 0x0001E564
		// (set) Token: 0x06000813 RID: 2067 RVA: 0x00020394 File Offset: 0x0001E594
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

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x00020410 File Offset: 0x0001E610
		// (set) Token: 0x06000815 RID: 2069 RVA: 0x00020430 File Offset: 0x0001E630
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

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x000204DC File Offset: 0x0001E6DC
		// (set) Token: 0x06000817 RID: 2071 RVA: 0x000204FC File Offset: 0x0001E6FC
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

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000818 RID: 2072 RVA: 0x000205A8 File Offset: 0x0001E7A8
		// (set) Token: 0x06000819 RID: 2073 RVA: 0x000205D8 File Offset: 0x0001E7D8
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

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x00020658 File Offset: 0x0001E858
		// (set) Token: 0x0600081B RID: 2075 RVA: 0x00020678 File Offset: 0x0001E878
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

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x0600081C RID: 2076 RVA: 0x0002075C File Offset: 0x0001E95C
		// (set) Token: 0x0600081D RID: 2077 RVA: 0x0002077C File Offset: 0x0001E97C
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

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x00020860 File Offset: 0x0001EA60
		// (set) Token: 0x0600081F RID: 2079 RVA: 0x00020880 File Offset: 0x0001EA80
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

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x000208D8 File Offset: 0x0001EAD8
		// (set) Token: 0x06000821 RID: 2081 RVA: 0x000208F8 File Offset: 0x0001EAF8
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

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x00020950 File Offset: 0x0001EB50
		// (set) Token: 0x06000823 RID: 2083 RVA: 0x00020970 File Offset: 0x0001EB70
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

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x000209C8 File Offset: 0x0001EBC8
		// (set) Token: 0x06000825 RID: 2085 RVA: 0x000209E8 File Offset: 0x0001EBE8
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

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x00020A40 File Offset: 0x0001EC40
		// (set) Token: 0x06000827 RID: 2087 RVA: 0x00020A58 File Offset: 0x0001EC58
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

		// Token: 0x06000828 RID: 2088 RVA: 0x00020AA8 File Offset: 0x0001ECA8
		public virtual void ResetSize()
		{
			this.RaisePropertyChanged<SizeF>(() => this.Size);
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x00020AE6 File Offset: 0x0001ECE6
		internal override void OnResourcePropertyChanged()
		{
			this.ResetSize();
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x00020AF0 File Offset: 0x0001ECF0
		public SizeF BoxSize
		{
			get
			{
				return this.GetInnerObject().GetBoxSize();
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x00020B10 File Offset: 0x0001ED10
		public ScaleValue BoxAnchorPoint
		{
			get
			{
				return this.GetInnerObject().GetBoxAnchorPoint();
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x0600082C RID: 2092 RVA: 0x00020B30 File Offset: 0x0001ED30
		// (set) Token: 0x0600082D RID: 2093 RVA: 0x00020B4D File Offset: 0x0001ED4D
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

		// Token: 0x0600082E RID: 2094 RVA: 0x00020B60 File Offset: 0x0001ED60
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

		// Token: 0x0600082F RID: 2095 RVA: 0x00020C84 File Offset: 0x0001EE84
		protected internal virtual bool IsCanChangeSize()
		{
			return false;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x00020C98 File Offset: 0x0001EE98
		protected override void OnBindingRecorder()
		{
			ExtenderFactory.Binding(this, new BaseExtender[]
			{
				new LayoutExtender(this),
				new ResourceExtender(this)
			});
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x00020CC7 File Offset: 0x0001EEC7
		internal override void AfterAdded()
		{
			LayoutExtender.RefreshLayout(this);
		}

		// Token: 0x04000328 RID: 808
		private bool uniformScale = false;

		// Token: 0x04000329 RID: 809
		private bool _layoutState = false;
	}
}
