using System;
using System.Collections.Generic;
using System.ComponentModel;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Editor;
using CocoStudio.Model.Interface;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000115 RID: 277
	[DisplayName("Display_Component_UISlider")]
	[ModelExtension(true, 7)]
	[ControlGroup("ComToolPad", 1)]
	[EngineClassName("Slider")]
	public class SliderObject : WidgetObject, IDisplayState, ICallBackEvent, IResetSize
	{
		// Token: 0x06000A50 RID: 2640 RVA: 0x000292B4 File Offset: 0x000274B4
		private CSSlider GetInnerWidget()
		{
			return (CSSlider)this.innerNode;
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x000292D4 File Offset: 0x000274D4
		public SliderObject()
		{
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x00029338 File Offset: 0x00027538
		public SliderObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x0002939C File Offset: 0x0002759C
		protected override void CreateCSObject()
		{
			this.innerNode = new CSSlider();
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x000293AC File Offset: 0x000275AC
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				this.BackGroundData = ResourceFile.DefaultMarker;
				this.BallNormalData = ResourceFile.DefaultMarker;
				this.ProgressBarData = ResourceFile.DefaultMarker;
				if (!Option.UserConfig.IsSimplifyDefaultRes)
				{
					this.BallPressedData = ResourceFile.DefaultMarker;
					this.BallDisabledData = ResourceFile.DefaultMarker;
				}
				else
				{
					this.BallPressedData = null;
					this.BallDisabledData = null;
				}
				this.PercentInfo = 50;
				this.TouchEnable = true;
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x0002943C File Offset: 0x0002763C
		// (set) Token: 0x06000A56 RID: 2646 RVA: 0x00029454 File Offset: 0x00027654
		[Editor(typeof(CheckBoxEditor), typeof(CheckBoxEditor))]
		[Category("Group_Feature")]
		[DefaultValue(true)]
		[PropertyOrder(71)]
		[DisplayName("Display_State")]
		[UndoProperty]
		public virtual bool DisplayState
		{
			get
			{
				return this.isNormal;
			}
			set
			{
				this.isNormal = value;
				this.GetInnerWidget().ChangeState(this.isNormal);
				this.RaisePropertyChanged<bool>(() => this.DisplayState);
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000A57 RID: 2647 RVA: 0x000294B8 File Offset: 0x000276B8
		// (set) Token: 0x06000A58 RID: 2648 RVA: 0x000294E9 File Offset: 0x000276E9
		[Category("Group_Feature")]
		[PropertyOrder(66)]
		[DisplayName("Display_ImageResources")]
		[Editor(typeof(ResourceGroupEditor), typeof(ResourceGroupEditor))]
		[DefaultValue(null)]
		public virtual List<string> ResourceValue
		{
			get
			{
				return new List<string>
				{
					"BackGroundData",
					"ProgressBarData"
				};
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000A59 RID: 2649 RVA: 0x000294F4 File Offset: 0x000276F4
		// (set) Token: 0x06000A5A RID: 2650 RVA: 0x0002950C File Offset: 0x0002770C
		[Editor(typeof(ResourceImageEditor), typeof(ResourceImageEditor))]
		[DisplayName("ContexMenu_BackgroundStyle")]
		[UndoProperty]
		[ResourceFilter(true, true, new string[]
		{
			"png",
			"jpg"
		})]
		[DefaultValue(null)]
		public virtual ResourceFile BackGroundData
		{
			get
			{
				return this.backGroundFile;
			}
			set
			{
				this.backGroundFile = value;
				ImageFile defaultFile = new ImageFile(SliderObjectData.DefaultBackgroundFile);
				ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.backGroundFile, defaultFile, true);
				this.GetInnerWidget().SetGroundBarTexture(resourceFile.GetResourceData());
				this.RaisePropertyChanged<ResourceFile>(() => this.BackGroundData);
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x00029588 File Offset: 0x00027788
		// (set) Token: 0x06000A5C RID: 2652 RVA: 0x000295A0 File Offset: 0x000277A0
		[ResourceFilter(true, true, new string[]
		{
			"png",
			"jpg"
		})]
		[Editor(typeof(ResourceImageEditor), typeof(ResourceImageEditor))]
		[DisplayName("ContexMenu_InnerSliderStyle")]
		[UndoProperty]
		[DefaultValue(null)]
		public virtual ResourceFile ProgressBarData
		{
			get
			{
				return this.progressBarFile;
			}
			set
			{
				this.progressBarFile = value;
				ImageFile defaultFile = new ImageFile(SliderObjectData.DefaultProgressBarFile);
				ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.progressBarFile, defaultFile, true);
				this.GetInnerWidget().SetProgressBarTexture(resourceFile.GetResourceData());
				this.RaisePropertyChanged<ResourceFile>(() => this.ProgressBarData);
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000A5D RID: 2653 RVA: 0x0002961C File Offset: 0x0002781C
		// (set) Token: 0x06000A5E RID: 2654 RVA: 0x00029659 File Offset: 0x00027859
		[Editor(typeof(ResourceGroupEditor), typeof(ResourceGroupEditor))]
		[PropertyOrder(70)]
		[Category("Group_Feature")]
		[DisplayName("Display_NodeResource")]
		public virtual List<string> ResourceNodeValue
		{
			get
			{
				return new List<string>
				{
					"BallNormalData",
					"BallPressedData",
					"BallDisabledData"
				};
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x00029664 File Offset: 0x00027864
		// (set) Token: 0x06000A60 RID: 2656 RVA: 0x0002967C File Offset: 0x0002787C
		[DisplayName("ContexMenu_NormalStyle")]
		[ResourceFilter(true, true, new string[]
		{
			"png",
			"jpg"
		})]
		[UndoProperty]
		[DefaultValue(null)]
		[Editor(typeof(ResourceImageEditor), typeof(ResourceImageEditor))]
		public virtual ResourceFile BallNormalData
		{
			get
			{
				return this.ballNormalFile;
			}
			set
			{
				this.ballNormalFile = value;
				ImageFile defaultFile = new ImageFile(SliderObjectData.DefaultBallNormalFile);
				ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.ballNormalFile, defaultFile, true);
				this.GetInnerWidget().SetBallNormalTexture(resourceFile.GetResourceData());
				this.RaisePropertyChanged<ResourceFile>(() => this.BallNormalData);
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x000296F8 File Offset: 0x000278F8
		// (set) Token: 0x06000A62 RID: 2658 RVA: 0x00029740 File Offset: 0x00027940
		[ResourceFilter(false, true, new string[]
		{
			"png",
			"jpg"
		})]
		[DisplayName("ContexMenu_PressedStyle")]
		[DefaultValue(null)]
		[UndoProperty]
		[Editor(typeof(ResourceImageEditor), typeof(ResourceImageEditor))]
		public virtual ResourceFile BallPressedData
		{
			get
			{
				if (this.ballPressedFile == null)
				{
					this.ballPressedFile = (Services.ProjectOperations.FindResourceItem(this.GetInnerWidget().GetBallPressedTexture()) as ResourceFile);
				}
				return this.ballPressedFile;
			}
			set
			{
				this.ballPressedFile = value;
				ImageFile defaultFile = new ImageFile(SliderObjectData.DefaultBallPressedFile);
				ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.ballPressedFile, defaultFile, true);
				this.GetInnerWidget().SetBallPressedTexture(resourceFile.GetResourceData());
				this.RaisePropertyChanged<ResourceFile>(() => this.BallPressedData);
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x000297BC File Offset: 0x000279BC
		// (set) Token: 0x06000A64 RID: 2660 RVA: 0x000297D4 File Offset: 0x000279D4
		[DefaultValue(null)]
		[UndoProperty]
		[Editor(typeof(ResourceImageEditor), typeof(ResourceImageEditor))]
		[DisplayName("ContexMenu_DisabledStyle")]
		[ResourceFilter(false, true, new string[]
		{
			"png",
			"jpg"
		})]
		public virtual ResourceFile BallDisabledData
		{
			get
			{
				return this.ballDisableFile;
			}
			set
			{
				this.ballDisableFile = value;
				ImageFile defaultFile = new ImageFile(SliderObjectData.DefaultBallDisabledFile);
				ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.ballDisableFile, defaultFile, true);
				this.GetInnerWidget().SetBallDisabledTexture(resourceFile.GetResourceData());
				this.RaisePropertyChanged<ResourceFile>(() => this.BallDisabledData);
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x00029850 File Offset: 0x00027A50
		// (set) Token: 0x06000A66 RID: 2662 RVA: 0x00029870 File Offset: 0x00027A70
		[DisplayName("Display_SliderProgress")]
		[DefaultValue(50)]
		[ValueRange(0, 100, 1f, 10f)]
		[PropertyOrder(72)]
		[UndoProperty]
		[Editor(typeof(SliderEditor), typeof(SliderEditor))]
		[Category("Group_Feature")]
		public virtual int PercentInfo
		{
			get
			{
				return this.GetInnerWidget().GetPercent();
			}
			set
			{
				int num = value;
				if (num < 0)
				{
					num = 0;
				}
				else if (num > 100)
				{
					num = 100;
				}
				this.GetInnerWidget().SetPercent(num);
				this.RaisePropertyChanged<int>(() => this.PercentInfo);
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x000298EC File Offset: 0x00027AEC
		// (set) Token: 0x06000A68 RID: 2664 RVA: 0x00029904 File Offset: 0x00027B04
		[UndoProperty]
		[PropertyOrder(37)]
		[DisplayName("Display_Sudoku")]
		[Browsable(false)]
		[Category("Display_Sudoku")]
		[LayoutRefresh]
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

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x00029964 File Offset: 0x00027B64
		// (set) Token: 0x06000A6A RID: 2666 RVA: 0x00029984 File Offset: 0x00027B84
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

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x000299E8 File Offset: 0x00027BE8
		// (set) Token: 0x06000A6C RID: 2668 RVA: 0x00029A08 File Offset: 0x00027C08
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

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x00029A6C File Offset: 0x00027C6C
		// (set) Token: 0x06000A6E RID: 2670 RVA: 0x00029A8C File Offset: 0x00027C8C
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

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x00029AF0 File Offset: 0x00027CF0
		// (set) Token: 0x06000A70 RID: 2672 RVA: 0x00029B10 File Offset: 0x00027D10
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

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x00029B74 File Offset: 0x00027D74
		public SizeF ResourceSize
		{
			get
			{
				return this.GetInnerWidget().GetWidgetAutoSize();
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x00029B94 File Offset: 0x00027D94
		// (set) Token: 0x06000A73 RID: 2675 RVA: 0x00029BB1 File Offset: 0x00027DB1
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

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000A74 RID: 2676 RVA: 0x00029BB4 File Offset: 0x00027DB4
		// (set) Token: 0x06000A75 RID: 2677 RVA: 0x00029BD1 File Offset: 0x00027DD1
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

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000A76 RID: 2678 RVA: 0x00029BD4 File Offset: 0x00027DD4
		// (set) Token: 0x06000A77 RID: 2679 RVA: 0x00029BF1 File Offset: 0x00027DF1
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

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000A78 RID: 2680 RVA: 0x00029BF4 File Offset: 0x00027DF4
		// (set) Token: 0x06000A79 RID: 2681 RVA: 0x00029C11 File Offset: 0x00027E11
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

		// Token: 0x06000A7A RID: 2682 RVA: 0x00029C14 File Offset: 0x00027E14
		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			SliderObject sliderObject = cObject as SliderObject;
			if (sliderObject != null)
			{
				sliderObject.BackGroundData = this.BackGroundData;
				sliderObject.ProgressBarData = this.ProgressBarData;
				sliderObject.BallNormalData = this.BallNormalData;
				sliderObject.BallPressedData = this.BallPressedData;
				sliderObject.BallDisabledData = this.BallDisabledData;
				sliderObject.Scale9Enable = this.Scale9Enable;
				sliderObject.LeftEage = this.LeftEage;
				sliderObject.RightEage = this.RightEage;
				sliderObject.TopEage = this.TopEage;
				sliderObject.BottomEage = this.BottomEage;
				sliderObject.PercentInfo = this.PercentInfo;
				sliderObject.Size = this.Size;
				sliderObject.DisplayState = this.DisplayState;
			}
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x00029CEC File Offset: 0x00027EEC
		protected internal override bool IsCanChangeSize()
		{
			return true;
		}

		// Token: 0x04000460 RID: 1120
		private bool isNormal = true;

		// Token: 0x04000461 RID: 1121
		private ResourceFile backGroundFile = null;

		// Token: 0x04000462 RID: 1122
		private ResourceFile progressBarFile = null;

		// Token: 0x04000463 RID: 1123
		private ResourceFile ballNormalFile = null;

		// Token: 0x04000464 RID: 1124
		private ResourceFile ballPressedFile = null;

		// Token: 0x04000465 RID: 1125
		private ResourceFile ballDisableFile = null;

		// Token: 0x04000466 RID: 1126
		private bool _scale9Enabled = false;

		// Token: 0x04000467 RID: 1127
		private int _left = 0;

		// Token: 0x04000468 RID: 1128
		private int _right = 0;

		// Token: 0x04000469 RID: 1129
		private int _top = 0;

		// Token: 0x0400046A RID: 1130
		private int _bottom = 0;
	}
}
