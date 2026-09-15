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
	[DisplayName("Display_Component_UISlider")]
	[ModelExtension(true, 7)]
	[ControlGroup("ComToolPad", 1)]
	[EngineClassName("Slider")]
	public class SliderObject : WidgetObject, IDisplayState, ICallBackEvent, IResetSize
	{
		private CSSlider GetInnerWidget()
		{
			return (CSSlider)this.innerNode;
		}

		public SliderObject()
		{
		}

		public SliderObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSSlider();
		}

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

		public SizeF ResourceSize
		{
			get
			{
				return this.GetInnerWidget().GetWidgetAutoSize();
			}
		}

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

		protected internal override bool IsCanChangeSize()
		{
			return true;
		}

		private bool isNormal = true;

		private ResourceFile backGroundFile = null;

		private ResourceFile progressBarFile = null;

		private ResourceFile ballNormalFile = null;

		private ResourceFile ballPressedFile = null;

		private ResourceFile ballDisableFile = null;

		private bool _scale9Enabled = false;

		private int _left = 0;

		private int _right = 0;

		private int _top = 0;

		private int _bottom = 0;
	}
}
