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
	[ModelExtension(true, 50)]
	[ControlGroup("Control_Container", 2)]
	[EngineClassName("Layout")]
	[DisplayName("Display_Component_UIPanel")]
	public class PanelObject : WidgetObject, IScale9
	{
		private CSPanel GetInnerWidget()
		{
			return (CSPanel)this.innerNode;
		}

		public PanelObject()
		{
		}

		public PanelObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSPanel();
		}

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

		public ScaleValue ColorVector { get; set; }

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

		public override bool CanReceiveDragObject(ModelDragData objectData, bool showLog)
		{
			return objectData != null;
		}

		protected override bool CanContinueTest()
		{
			return base.CanContinueTest() && !this.ClipAble;
		}

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

		protected internal override bool IsCanChangeSize()
		{
			return true;
		}

		private ResourceFile file = null;

		private float _colorAngle = 0f;

		private bool _scale9Enabled = false;

		private int _left = 0;

		private int _right = 0;

		private int _top = 0;

		private int _bottom = 0;
	}
}
