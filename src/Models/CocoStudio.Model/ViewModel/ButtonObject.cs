using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Editor;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	[EngineClassName("Button")]
	[DisplayName("Display_Component_UIButton")]
	[ModelExtension(true, 0)]
	[ControlGroup("ComToolPad", 1)]
	public class ButtonObject : WidgetObject, IScale9, IDisplayState, IResetSize, IFlipped
	{
		private CSButton GetInnerWidget()
		{
			return (CSButton)this.innerNode;
		}

		public ButtonObject()
		{
		}

		public ButtonObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSButton();
		}

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				this.FontResource = null;
				this.ButtonText = "Button";
				this.FontSize = 14;
				this.TextColor = Color.FromArgb(255, 65, 65, 70);
				this.NormalFileData = ResourceFile.DefaultMarker;
				if (!Option.UserConfig.IsSimplifyDefaultRes)
				{
					this.PressedFileData = ResourceFile.DefaultMarker;
					this.DisabledFileData = ResourceFile.DefaultMarker;
				}
				else
				{
					this.PressedFileData = null;
					this.DisabledFileData = null;
				}
				this.Filp = new FilpValue(false, false);
				this.Scale9Enable = true;
				this.TouchEnable = true;
				this.LeftEage = (this.RightEage = (int)(this.ResourceSize.Width * 0.33f));
				this.TopEage = (this.BottomEage = (int)(this.ResourceSize.Height * 0.33f));
			}
		}

		[Editor(typeof(CheckBoxEditor), typeof(CheckBoxEditor))]
		[UndoProperty]
		[DisplayName("Display_State")]
		[Category("Group_Feature")]
		[DefaultValue(true)]
		[PropertyOrder(96)]
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

		[Editor(typeof(ResourceGroupEditor), typeof(ResourceGroupEditor))]
		[PropertyOrder(92)]
		[DisplayName("Display_ImageResources")]
		[Category("Group_Feature")]
		public virtual List<string> ResourceValue
		{
			get
			{
				return new List<string>
				{
					"NormalFileData",
					"PressedFileData",
					"DisabledFileData"
				};
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		[ResourceFilter(true, true, new string[]
		{
			"png",
			"jpg"
		})]
		[Editor(typeof(ResourceImageEditor), typeof(ResourceImageEditor))]
		[DisplayName("Display_NormalState")]
		[UndoProperty]
		[DefaultValue(null)]
		public ResourceFile NormalFileData
		{
			get
			{
				if (this.normalFile == null)
				{
					this.normalFile = (Services.ProjectOperations.FindResourceItem(this.GetInnerWidget().GetNormalFilePath()) as ResourceFile);
				}
				return this.normalFile;
			}
			set
			{
				this.normalFile = value;
				ImageFile defaultFile = new ImageFile(ButtonObjectData.Default_NormalFile);
				ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.normalFile, defaultFile, true);
				this.GetInnerWidget().SetNormalFilePath(resourceFile.GetResourceData());
				this.RaisePropertyChanged<ResourceFile>(() => this.NormalFileData);
			}
		}

		[UndoProperty]
		[Editor(typeof(ResourceImageEditor), typeof(ResourceImageEditor))]
		[ResourceFilter(false, true, new string[]
		{
			"png",
			"jpg"
		})]
		[DisplayName("Display_BtnDown")]
		[IgnoreResize]
		[DefaultValue(null)]
		public ResourceFile PressedFileData
		{
			get
			{
				if (this.pressedFile == null)
				{
					this.pressedFile = (Services.ProjectOperations.FindResourceItem(this.GetInnerWidget().GetPressedFilePath()) as ResourceFile);
				}
				return this.pressedFile;
			}
			set
			{
				this.pressedFile = value;
				ImageFile defaultFile = new ImageFile(ButtonObjectData.Default_PressedFile);
				ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.pressedFile, defaultFile, true);
				this.GetInnerWidget().SetPressedFilePath(resourceFile.GetResourceData());
				this.RaisePropertyChanged<ResourceFile>(() => this.PressedFileData);
			}
		}

		[UndoProperty]
		[DefaultValue(null)]
		[ResourceFilter(false, true, new string[]
		{
			"png",
			"jpg"
		})]
		[Editor(typeof(ResourceImageEditor), typeof(ResourceImageEditor))]
		[DisplayName("Display_Disable")]
		public ResourceFile DisabledFileData
		{
			get
			{
				if (this.disabledFile == null)
				{
					this.disabledFile = (Services.ProjectOperations.FindResourceItem(this.GetInnerWidget().GetDisabledFilePath()) as ResourceFile);
				}
				return this.disabledFile;
			}
			set
			{
				this.disabledFile = value;
				ImageFile defaultFile = new ImageFile(ButtonObjectData.Default_DisabledFile);
				ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.disabledFile, defaultFile, true);
				this.GetInnerWidget().SetDisabledFilePath(resourceFile.GetResourceData());
				this.RaisePropertyChanged<ResourceFile>(() => this.DisabledFileData);
			}
		}

		[ResourceFilter(new string[]
		{
			"ttf",
			"ttc"
		})]
		[UndoProperty]
		[Category("Group_Feature")]
		[Editor(typeof(ResourceFileEditor), typeof(ResourceFileEditor))]
		[Browsable(true)]
		[DisplayName("Display_FontFile")]
		[DefaultValue(null)]
		[PropertyOrder(100)]
		[IgnoreResize]
		public ResourceFile FontResource
		{
			get
			{
				if (this.fontFile == null)
				{
					this.fontFile = (Services.ProjectOperations.FindResourceItem(this.GetInnerWidget().GetFontName()) as ResourceFile);
				}
				return this.fontFile;
			}
			set
			{
				this.fontFile = value;
				if (this.fontFile == ResourceFile.DefaultMarker || this.fontFile == null || this.fontFile.DataError != null)
				{
					this.GetInnerWidget().SetFontName(WidgetObjectData.DefaultFont);
					this.GetInnerWidget().SetFontSize(this.FontSize);
				}
				else
				{
					this.GetInnerWidget().SetFontName(this.fontFile.GetResourceData().Path);
				}
				this.RaisePropertyChanged<ResourceFile>(() => this.FontResource);
				this.RaisePropertyChanged<bool>(() => this.ShadowEnabled);
			}
		}

		[DisplayName("Display_FontStyle")]
		[ValueRange(5, 100, 1f, 10f)]
		[UndoProperty]
		[PropertyOrder(99)]
		[Editor(typeof(PropertyColorEditor), typeof(PropertyColorEditor))]
		[DefaultValue(24)]
		[Category("Group_Feature")]
		[LayoutRefresh]
		public int FontSize
		{
			get
			{
				return this.GetInnerWidget().GetFontSize();
			}
			set
			{
				if (value < 1)
				{
					value = 1;
				}
				if (this.GetInnerWidget().GetFontSize() != value)
				{
					this.GetInnerWidget().SetFontSize(value);
					this.RaisePropertyChanged<int>(() => this.FontSize);
				}
			}
		}

		[DisplayName("Display_Text")]
		[UndoProperty]
		[PropertyOrder(97)]
		[LayoutRefresh]
		[Category("Group_Feature")]
		public string ButtonText
		{
			get
			{
				return this.GetInnerWidget().GetText();
			}
			set
			{
				if (value != this.text)
				{
					this.text = value;
					if (value == null)
					{
						value = "";
					}
					this.GetInnerWidget().SetText(value);
					this.RaisePropertyChanged<string>(() => this.ButtonText);
				}
			}
		}

		[DisplayName("Display_TextColor")]
		[Category("Group_Feature")]
		[PropertyOrder(98)]
		[Browsable(false)]
		[UndoProperty]
		public Color TextColor
		{
			get
			{
				return this._textColor;
			}
			set
			{
				if (this._textColor.B != value.B || this._textColor.G != value.G || this._textColor.R != value.R)
				{
					this._textColor = value;
					this.GetInnerWidget().SetTextColor(value);
					this.RaisePropertyChanged<Color>(() => this.TextColor);
				}
			}
		}

		[Category("Group_Routine")]
		[UndoProperty]
		[Editor(typeof(FilpEditor), typeof(FilpEditor))]
		[DisplayName("Display_Flip")]
		[DefaultValue(false)]
		[Browsable(true)]
		[PropertyOrder(15)]
		public virtual FilpValue Filp
		{
			get
			{
				return new FilpValue(this.FlipX, this.FlipY);
			}
			set
			{
				this.FlipX = value.FlipX;
				this.FlipY = value.FlipY;
			}
		}

		public virtual bool FlipY
		{
			get
			{
				return this.GetInnerWidget().GetFlipY();
			}
			set
			{
				if (this.GetInnerWidget().GetFlipY() != value)
				{
					this.GetInnerWidget().SetFlipY(value);
					this.RaisePropertyChanged<FilpValue>(() => this.Filp);
				}
			}
		}

		public virtual bool FlipX
		{
			get
			{
				return this.GetInnerWidget().GetFlipX();
			}
			set
			{
				if (this.GetInnerWidget().GetFlipX() != value)
				{
					this.GetInnerWidget().SetFlipX(value);
					this.RaisePropertyChanged<FilpValue>(() => this.Filp);
				}
			}
		}

		public bool IsReverse
		{
			get
			{
				return true;
			}
		}

		[DisplayName("Display_Sudoku")]
		[Category("Display_Sudoku")]
		[Editor(typeof(Scale9Editor), typeof(Scale9Editor))]
		[PropertyOrder(37)]
		[LayoutRefresh]
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

		[PropertyOrder(101)]
		[Browsable(false)]
		[UndoProperty]
		[Category("Group_Feature")]
		public bool ShadowEnabled
		{
			get
			{
				return this.shadowEnabled;
			}
			set
			{
				if (this.ShadowEnabled != value)
				{
					this.shadowEnabled = value;
					if (!value)
					{
						this.DisableShadow();
					}
					else
					{
						this.EnableShadow();
					}
					this.RaisePropertyChanged<bool>(() => this.ShadowEnabled);
				}
			}
		}

		private void DisableShadow()
		{
			this.GetInnerWidget().DisabledEffect();
			if (this.OutlineEnabled)
			{
				this.EnableOutline();
			}
		}

		[UndoProperty]
		public Color ShadowColor
		{
			get
			{
				return this.shadowColor;
			}
			set
			{
				if (this.ShadowColor != value)
				{
					this.shadowColor = value;
					this.EnableShadow();
					this.RaisePropertyChanged<Color>(() => this.ShadowColor);
				}
			}
		}

		[UndoProperty]
		public float ShadowOffsetX
		{
			get
			{
				return this.shadowOffsetX;
			}
			set
			{
				if (this.ShadowOffsetX != value)
				{
					this.shadowOffsetX = value;
					this.EnableShadow();
					this.RaisePropertyChanged<float>(() => this.ShadowOffsetX);
				}
			}
		}

		[UndoProperty]
		public float ShadowOffsetY
		{
			get
			{
				return this.shadowOffsetY;
			}
			set
			{
				if (this.ShadowOffsetY != value)
				{
					this.shadowOffsetY = value;
					this.EnableShadow();
					this.RaisePropertyChanged<float>(() => this.ShadowOffsetY);
				}
			}
		}

		[UndoProperty]
		public int ShadowBlurRadius
		{
			get
			{
				return this.shadowBlurRadius;
			}
			set
			{
				if (this.ShadowBlurRadius != value)
				{
					this.shadowBlurRadius = value;
					this.EnableShadow();
					this.RaisePropertyChanged<int>(() => this.ShadowBlurRadius);
				}
			}
		}

		private void EnableShadow()
		{
		}

		[UndoProperty]
		public bool OutlineEnabled
		{
			get
			{
				return this.outlineEnabled;
			}
			set
			{
				if (this.OutlineEnabled != value)
				{
					this.outlineEnabled = value;
					if (!value)
					{
						this.DisableOutline();
					}
					else
					{
						this.EnableOutline();
					}
					this.RaisePropertyChanged<bool>(() => this.OutlineEnabled);
				}
			}
		}

		[UndoProperty]
		public Color OutlineColor
		{
			get
			{
				return this.outlineColor;
			}
			set
			{
				if (this.OutlineColor != value)
				{
					this.outlineColor = value;
					this.EnableOutline();
					this.RaisePropertyChanged<Color>(() => this.OutlineColor);
				}
			}
		}

		[UndoProperty]
		public int OutlineSize
		{
			get
			{
				return this.outlineSize;
			}
			set
			{
				if (this.OutlineSize != value)
				{
					this.outlineSize = value;
					if (value == 0)
					{
						this.DisableOutline();
					}
					else
					{
						this.EnableOutline();
					}
					this.RaisePropertyChanged<int>(() => this.OutlineSize);
				}
			}
		}

		private void EnableOutline()
		{
		}

		private void DisableOutline()
		{
			this.GetInnerWidget().DisabledEffect();
			if (this.ShadowEnabled)
			{
				this.EnableShadow();
			}
		}

		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			ButtonObject buttonObject = cObject as ButtonObject;
			if (buttonObject != null)
			{
				buttonObject.NormalFileData = this.NormalFileData;
				buttonObject.PressedFileData = this.PressedFileData;
				buttonObject.DisabledFileData = this.DisabledFileData;
				buttonObject.FontSize = this.FontSize;
				buttonObject.ButtonText = this.ButtonText;
				buttonObject.TextColor = this.TextColor;
				buttonObject.FlipX = this.FlipX;
				buttonObject.FlipY = this.FlipY;
				buttonObject.Scale9Enable = this.Scale9Enable;
				buttonObject.LeftEage = this.LeftEage;
				buttonObject.RightEage = this.RightEage;
				buttonObject.TopEage = this.TopEage;
				buttonObject.BottomEage = this.BottomEage;
				buttonObject.FontResource = this.FontResource;
				buttonObject.Size = this.Size;
				buttonObject.DisplayState = this.DisplayState;
				buttonObject.ShadowEnabled = this.ShadowEnabled;
				buttonObject.ShadowOffsetX = this.ShadowOffsetX;
				buttonObject.ShadowOffsetY = this.ShadowOffsetY;
				buttonObject.ShadowBlurRadius = this.ShadowBlurRadius;
				buttonObject.ShadowColor = this.ShadowColor;
				buttonObject.OutlineEnabled = this.OutlineEnabled;
				buttonObject.OutlineSize = this.OutlineSize;
				buttonObject.OutlineColor = this.OutlineColor;
			}
		}

		protected override void OnMouseDoubleClick(MouseEventArgs args)
		{
			TextEditorWindow textEditorWindow = new TextEditorWindow(this, "ButtonText", "TextColor", "FontSize", false);
		}

		protected internal override bool IsCanChangeSize()
		{
			return true;
		}

		private bool isNormal = true;

		private ResourceFile normalFile = null;

		private ResourceFile pressedFile = null;

		private ResourceFile disabledFile = null;

		private ResourceFile fontFile = null;

		private string text;

		private Color _textColor = Color.FromArgb(255, 255, 255, 255);

		private bool _scale9Enabled = false;

		private int _left = 0;

		private int _right = 0;

		private int _top = 0;

		private int _bottom = 0;

		private bool shadowEnabled = false;

		private Color shadowColor = Color.FromArgb(255, 110, 110, 110);

		private float shadowOffsetX = 2f;

		private float shadowOffsetY = -2f;

		private int shadowBlurRadius = 0;

		private bool outlineEnabled = false;

		private Color outlineColor = Color.FromArgb(255, 255, 0, 0);

		private int outlineSize = 1;
	}
}
