using System;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.Editor;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000110 RID: 272
	[ModelExtension(true, 50)]
	[ControlGroup("Control_Container", 2)]
	[EngineClassName("Layout")]
	[DisplayName("Display_Component_UIPanel")]
	public class PanelObject : WidgetObject, IScale9
	{
		// Token: 0x060009CE RID: 2510 RVA: 0x00027428 File Offset: 0x00025628
		private CSPanel GetInnerWidget()
		{
			return (CSPanel)this.innerNode;
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x00027445 File Offset: 0x00025645
		public PanelObject()
		{
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x00027488 File Offset: 0x00025688
		public PanelObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x000274D4 File Offset: 0x000256D4
		protected override void CreateCSObject()
		{
			this.innerNode = new CSPanel();
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x000274E4 File Offset: 0x000256E4
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				this.BackColorAlpha = 102;
				this.SingleColor = Color.FromArgb(255, 150, 200, 255);
				this.FirstColor = Color.FromArgb(255, 150, 200, 255);
				this.EndColor = Color.FromArgb(255, 255, 255, 255);
				this.ComboBoxType = PanelColorFillType.Color_solid;
				this.ColorAngle = 90f;
				this.Size = new SizeF(200f, 200f);
				this.TouchEnable = true;
				this.ClipAble = false;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x060009D3 RID: 2515 RVA: 0x000275A8 File Offset: 0x000257A8
		// (set) Token: 0x060009D4 RID: 2516 RVA: 0x000275C8 File Offset: 0x000257C8
		[UndoProperty]
		[DisplayName("Display_Clip")]
		[Category("Group_Feature")]
		[PropertyOrder(38)]
		public virtual bool ClipAble
		{
			get
			{
				return this.GetInnerWidget().GetClipAble();
			}
			set
			{
				this.GetInnerWidget().SetClipAble(value);
				this.RaisePropertyChanged<bool>(() => this.ClipAble);
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x060009D5 RID: 2517 RVA: 0x00027620 File Offset: 0x00025820
		// (set) Token: 0x060009D6 RID: 2518 RVA: 0x00027680 File Offset: 0x00025880
		[IgnoreResize]
		[DisplayName("Display_BackgroundImage")]
		[Editor(typeof(ResourceImageEditor), typeof(ResourceImageEditor))]
		[DefaultValue(null)]
		[PropertyOrder(54)]
		[UndoProperty]
		[Category("Group_Feature")]
		[ResourceFilter(new string[]
		{
			"png",
			"jpg"
		})]
		public ResourceFile FileData
		{
			get
			{
				if (this.file == null)
				{
					this.file = (Services.ProjectOperations.FindResourceItem(this.GetInnerWidget().GetFilePath()) as ResourceFile);
				}
				ResourceFile result;
				if (this.file == ResourceFile.DefaultMarker)
				{
					result = null;
				}
				else
				{
					result = this.file;
				}
				return result;
			}
			set
			{
				this.file = value;
				if (this.file == ResourceFile.DefaultMarker || this.file == null || this.file.DataError != null)
				{
					this.GetInnerWidget().SetFilePath(new ResourceData(""));
				}
				else
				{
					this.GetInnerWidget().SetFilePath(this.file.GetResourceData());
				}
				this.RaisePropertyChanged<ResourceFile>(() => this.FileData);
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x060009D7 RID: 2519 RVA: 0x00027730 File Offset: 0x00025930
		// (set) Token: 0x060009D8 RID: 2520 RVA: 0x00027748 File Offset: 0x00025948
		[UndoProperty]
		[PropertyOrder(46)]
		[Category("Group_Feature")]
		[DisplayName("Fill_color")]
		public PanelColorFillType ComboBoxType
		{
			get
			{
				return (PanelColorFillType)this.ComboBoxIndex;
			}
			set
			{
				this.GetInnerWidget().SetGroundColorType((int)value);
				this.RaisePropertyChanged<PanelColorFillType>(() => this.ComboBoxType);
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x060009D9 RID: 2521 RVA: 0x000277A0 File Offset: 0x000259A0
		// (set) Token: 0x060009DA RID: 2522 RVA: 0x000277C0 File Offset: 0x000259C0
		public int ComboBoxIndex
		{
			get
			{
				return this.GetInnerWidget().GetGroundColorType();
			}
			set
			{
				this.GetInnerWidget().SetGroundColorType(value);
				this.RaisePropertyChanged<PanelColorFillType>(() => this.ComboBoxType);
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x060009DB RID: 2523 RVA: 0x00027818 File Offset: 0x00025A18
		// (set) Token: 0x060009DC RID: 2524 RVA: 0x00027838 File Offset: 0x00025A38
		[PropertyOrder(50)]
		[UndoProperty]
		[DisplayName("End_color_set")]
		[Category("Group_Feature")]
		public Color EndColor
		{
			get
			{
				return this.GetInnerWidget().GetGroundLineEndColor();
			}
			set
			{
				this.GetInnerWidget().SetGroundLineEndColor(value);
				this.RaisePropertyChanged<Color>(() => this.EndColor);
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x060009DD RID: 2525 RVA: 0x00027890 File Offset: 0x00025A90
		// (set) Token: 0x060009DE RID: 2526 RVA: 0x000278B0 File Offset: 0x00025AB0
		[UndoProperty]
		[PropertyOrder(48)]
		[DisplayName("Start_color_set")]
		[Category("Group_Feature")]
		public Color FirstColor
		{
			get
			{
				return this.GetInnerWidget().GetGroundLineStartColor();
			}
			set
			{
				this.GetInnerWidget().SetGroundLineStartColor(value);
				this.RaisePropertyChanged<Color>(() => this.FirstColor);
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x060009DF RID: 2527 RVA: 0x00027908 File Offset: 0x00025B08
		// (set) Token: 0x060009E0 RID: 2528 RVA: 0x00027928 File Offset: 0x00025B28
		[PropertyOrder(47)]
		[UndoProperty]
		[DisplayName("Single_color_set")]
		[Category("Group_Feature")]
		public Color SingleColor
		{
			get
			{
				return this.GetInnerWidget().GetGroundSingleColor();
			}
			set
			{
				this.GetInnerWidget().SetGroundSingleColor(value);
				this.RaisePropertyChanged<Color>(() => this.SingleColor);
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x00027980 File Offset: 0x00025B80
		// (set) Token: 0x060009E2 RID: 2530 RVA: 0x00027998 File Offset: 0x00025B98
		[DisplayName("Changed_Direction")]
		[UndoProperty]
		[Editor(typeof(SliderEditor), typeof(SliderEditor))]
		[ConstructParams(new object[]
		{
			false,
			false,
			true,
			"Property_Angle"
		})]
		[ValueRange(0, 360, 1f, 10f)]
		[Category("Group_Feature")]
		[PropertyOrder(53)]
		public float ColorAngle
		{
			get
			{
				return this._colorAngle;
			}
			set
			{
				if (this._colorAngle != value)
				{
					this._colorAngle = value;
					float num = (float)(3.141592653589793 * (double)(90f - this._colorAngle) / 180.0);
					float scaleX = (float)Math.Sin((double)num);
					float scaleY = (float)Math.Cos((double)num);
					this.ColorVector = new ScaleValue(scaleX, scaleY, 0.1, -99999999.0, 99999999.0);
					this.GetInnerWidget().SetGroundColorVector(this.ColorVector);
					this.RaisePropertyChanged<float>(() => this.ColorAngle);
				}
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x00027A70 File Offset: 0x00025C70
		// (set) Token: 0x060009E4 RID: 2532 RVA: 0x00027A87 File Offset: 0x00025C87
		public ScaleValue ColorVector { get; set; }

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x00027A90 File Offset: 0x00025C90
		// (set) Token: 0x060009E6 RID: 2534 RVA: 0x00027AB0 File Offset: 0x00025CB0
		[DisplayName("Background_color_transparency")]
		[ValueRange(0, 255, 1f, 10f)]
		[Editor(typeof(SliderEditor), typeof(SliderEditor))]
		[Category("Group_Feature")]
		[PropertyOrder(52)]
		[UndoProperty]
		public virtual int BackColorAlpha
		{
			get
			{
				return this.GetInnerWidget().GetGroundAlpha();
			}
			set
			{
				if (this.GetInnerWidget().GetGroundAlpha() != value)
				{
					this.GetInnerWidget().SetGroundAlpha(value);
					this.RaisePropertyChanged<int>(() => this.BackColorAlpha);
				}
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x00027B1C File Offset: 0x00025D1C
		// (set) Token: 0x060009E8 RID: 2536 RVA: 0x00027B34 File Offset: 0x00025D34
		[DisplayName("Display_Sudoku")]
		[PropertyOrder(37)]
		[Category("Display_Sudoku")]
		[Editor(typeof(Scale9Editor), typeof(Scale9Editor))]
		[Browsable(true)]
		[UndoProperty]
		public virtual bool Scale9Enable
		{
			get
			{
				return this._scale9Enabled;
			}
			set
			{
				this._scale9Enabled = value;
				this.GetInnerWidget().SetScale9Enabled(value);
				this.RaisePropertyChanged<bool>(() => this.Scale9Enable);
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x00027B94 File Offset: 0x00025D94
		// (set) Token: 0x060009EA RID: 2538 RVA: 0x00027BB4 File Offset: 0x00025DB4
		[UndoProperty]
		public virtual int LeftEage
		{
			get
			{
				return this.GetInnerWidget().GetScale9Left();
			}
			set
			{
				this._left = value;
				this.GetInnerWidget().SetScale9Left(this._left);
				this.RaisePropertyChanged<int>(() => this.LeftEage);
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x060009EB RID: 2539 RVA: 0x00027C18 File Offset: 0x00025E18
		// (set) Token: 0x060009EC RID: 2540 RVA: 0x00027C38 File Offset: 0x00025E38
		[UndoProperty]
		public virtual int RightEage
		{
			get
			{
				return this.GetInnerWidget().GetScale9Right();
			}
			set
			{
				this._right = value;
				this.GetInnerWidget().SetScale9Right(this._right);
				this.RaisePropertyChanged<int>(() => this.RightEage);
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x060009ED RID: 2541 RVA: 0x00027C9C File Offset: 0x00025E9C
		// (set) Token: 0x060009EE RID: 2542 RVA: 0x00027CBC File Offset: 0x00025EBC
		[UndoProperty]
		public virtual int TopEage
		{
			get
			{
				return this.GetInnerWidget().GetScale9Top();
			}
			set
			{
				this._top = value;
				this.GetInnerWidget().SetScale9Top(this._top);
				this.RaisePropertyChanged<int>(() => this.TopEage);
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x060009EF RID: 2543 RVA: 0x00027D20 File Offset: 0x00025F20
		// (set) Token: 0x060009F0 RID: 2544 RVA: 0x00027D40 File Offset: 0x00025F40
		[UndoProperty]
		public virtual int BottomEage
		{
			get
			{
				return this.GetInnerWidget().GetScale9Bottom();
			}
			set
			{
				this._bottom = value;
				this.GetInnerWidget().SetScale9Bottom(this._bottom);
				this.RaisePropertyChanged<int>(() => this.BottomEage);
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x060009F1 RID: 2545 RVA: 0x00027DA4 File Offset: 0x00025FA4
		public SizeF ResourceSize
		{
			get
			{
				SizeF result;
				if (this.FileData == null || this.FileData.PreviewImageInfo == null)
				{
					result = SizeF.Empty;
				}
				else
				{
					result = this.FileData.PreviewImageInfo.Size;
				}
				return result;
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x060009F2 RID: 2546 RVA: 0x00027DF0 File Offset: 0x00025FF0
		// (set) Token: 0x060009F3 RID: 2547 RVA: 0x00027E0D File Offset: 0x0002600D
		public virtual int Scale9OriginX
		{
			get
			{
				return this.GetInnerWidget().GetScale9OriginX();
			}
			set
			{
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x060009F4 RID: 2548 RVA: 0x00027E10 File Offset: 0x00026010
		// (set) Token: 0x060009F5 RID: 2549 RVA: 0x00027E2D File Offset: 0x0002602D
		public virtual int Scale9OriginY
		{
			get
			{
				return this.GetInnerWidget().GetScale9OriginY();
			}
			set
			{
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x060009F6 RID: 2550 RVA: 0x00027E30 File Offset: 0x00026030
		// (set) Token: 0x060009F7 RID: 2551 RVA: 0x00027E4D File Offset: 0x0002604D
		public virtual int Scale9Width
		{
			get
			{
				return this.GetInnerWidget().GetScale9Width();
			}
			set
			{
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x060009F8 RID: 2552 RVA: 0x00027E50 File Offset: 0x00026050
		// (set) Token: 0x060009F9 RID: 2553 RVA: 0x00027E6D File Offset: 0x0002606D
		public virtual int Scale9Height
		{
			get
			{
				return this.GetInnerWidget().GetScale9Height();
			}
			set
			{
			}
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x00027E70 File Offset: 0x00026070
		public override bool CanReceiveDragObject(ModelDragData objectData, bool showLog)
		{
			return objectData != null;
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x00027E94 File Offset: 0x00026094
		protected override bool CanContinueTest()
		{
			return base.CanContinueTest() && !this.ClipAble;
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00027EBC File Offset: 0x000260BC
		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			PanelObject panelObject = cObject as PanelObject;
			if (panelObject != null)
			{
				panelObject.FileData = this.FileData;
				panelObject.Alpha = this.Alpha;
				panelObject.CColor = this.CColor;
				panelObject.BackColorAlpha = this.BackColorAlpha;
				panelObject.ClipAble = this.ClipAble;
				panelObject.SingleColor = this.SingleColor;
				panelObject.FirstColor = this.FirstColor;
				panelObject.EndColor = this.EndColor;
				panelObject.ColorAngle = this.ColorAngle;
				panelObject.ComboBoxIndex = this.ComboBoxIndex;
				panelObject.Scale9Enable = this.Scale9Enable;
				panelObject.LeftEage = this.LeftEage;
				panelObject.RightEage = this.RightEage;
				panelObject.TopEage = this.TopEage;
				panelObject.BottomEage = this.BottomEage;
				panelObject.Size = this.Size;
			}
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x00027FBC File Offset: 0x000261BC
		protected internal override bool IsCanChangeSize()
		{
			return true;
		}

		// Token: 0x04000450 RID: 1104
		private ResourceFile file = null;

		// Token: 0x04000451 RID: 1105
		private float _colorAngle = 0f;

		// Token: 0x04000452 RID: 1106
		private bool _scale9Enabled = false;

		// Token: 0x04000453 RID: 1107
		private int _left = 0;

		// Token: 0x04000454 RID: 1108
		private int _right = 0;

		// Token: 0x04000455 RID: 1109
		private int _top = 0;

		// Token: 0x04000456 RID: 1110
		private int _bottom = 0;
	}
}
