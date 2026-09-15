using System;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.Basic;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Editor;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	[ControlGroup("ComToolPad", 1)]
	[EngineClassName("Text")]
	[DisplayName("Display_Component_UILable")]
	[ModelExtension(true, 8)]
	public class TextObject : WidgetObject, ISizeType, IFlipped, IResetSize, ILabelEffect
	{
		private CSText GetInnerWidget()
		{
			return (CSText)this.innerNode;
		}

		public TextObject()
		{
			base.SetDefaultSizeType(false);
		}

		public TextObject(ScriptFileData fileData) : base(fileData)
		{
			base.SetDefaultSizeType(false);
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSText();
		}

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				this.FontResource = null;
				this.LabelText = "Text Label";
				this.FontSize = 20;
				this.Filp = new FilpValue(false, false);
			}
		}

		public ObjectSizeType SupportSizeType
		{
			get
			{
				return ObjectSizeType.AutoAndCustom;
			}
		}

		[Browsable(true)]
		[LayoutRefresh]
		[PropertyOrder(-1)]
		[UndoProperty]
		[DisplayName("Property_CustomSize")]
		[Category("Group_Feature")]
		public bool IsCustomSize
		{
			get
			{
				return this.GetInnerWidget().GetCustomSizeEnabled();
			}
			set
			{
				this.GetInnerWidget().SetCustomSizeEnabled(value);
				this.RaisePropertyChanged<bool>(() => this.IsCustomSize);
			}
		}

		[DisplayName("Display_Animation")]
		[PropertyOrder(55)]
		[DefaultValue(255)]
		[UndoProperty]
		[Category("Group_Feature")]
		public virtual bool TouchScaleChangeAble
		{
			get
			{
				return this.GetInnerWidget().GetTouchScaleChangeEanbleState();
			}
			set
			{
				this.GetInnerWidget().SetTouchScaleChangeEanbleState(value);
				this.RaisePropertyChanged<bool>(() => this.TouchScaleChangeAble);
			}
		}

		[DefaultValue(false)]
		[Browsable(false)]
		[PropertyOrder(15)]
		[Editor(typeof(FilpEditor), typeof(FilpEditor))]
		[DisplayName("Display_Flip")]
		[Category("Group_Feature")]
		[UndoProperty]
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

		[ResourceFilter(new string[]
		{
			"ttf",
			"ttc"
		})]
		[Category("Group_Feature")]
		[PropertyOrder(61)]
		[UndoProperty]
		[Editor(typeof(ResourceFileEditor), typeof(ResourceFileEditor))]
		[DefaultValue(null)]
		[DisplayName("Display_FontFile")]
		public ResourceFile FontResource
		{
			get
			{
				return this.file;
			}
			set
			{
				this.file = value;
				if (this.file == ResourceFile.DefaultMarker || this.file == null || this.file.DataError != null)
				{
					this.DisableOutline();
					this.GetInnerWidget().SetFontName(WidgetObjectData.DefaultFont);
					this.GetInnerWidget().SetFontSize(this.FontSize);
				}
				else
				{
					this.GetInnerWidget().SetFontName(this.file.GetResourceData().Path);
				}
				this.RaisePropertyChanged<ResourceFile>(() => this.FontResource);
				this.RaisePropertyChanged<bool>(() => this.ShadowEnabled);
			}
		}

		[PropertyOrder(13)]
		[Browsable(false)]
		[UndoProperty]
		[DisplayName("Display_ColorBlend")]
		[Category("Group_Routine")]
		public override Color CColor
		{
			get
			{
				Color4B textColor = this.GetInnerWidget().GetTextColor();
				return Color.FromArgb((int)textColor.a, (int)textColor.r, (int)textColor.g, (int)textColor.b);
			}
			set
			{
				Color4B textColor = new Color4B(value.R, value.G, value.B, value.A);
				this.GetInnerWidget().SetTextColor(textColor);
				this.RaisePropertyChanged<Color>(() => this.CColor);
			}
		}

		[PropertyOrder(60)]
		[Browsable(true)]
		[Category("Group_Feature")]
		[LayoutRefresh]
		[UndoProperty]
		[DefaultValue(24)]
		[ValueRange(5, 100, 1f, 10f)]
		[Editor(typeof(PropertyColorEditor), typeof(PropertyColorEditor))]
		[DisplayName("Display_FontStyle")]
		public int FontSize
		{
			get
			{
				return this.GetInnerWidget().GetFontSize();
			}
			set
			{
				if (this.FontSize != value && value >= 1)
				{
					this.GetInnerWidget().SetFontSize(value);
					this.RaisePropertyChanged<int>(() => this.FontSize);
				}
			}
		}

		[LayoutRefresh]
		[PropertyOrder(57)]
		[UndoProperty]
		[Editor(typeof(EntryTextViewEditor), typeof(EntryTextViewEditor))]
		[DefaultValue("")]
		[DisplayName("Display_Text")]
		[Category("Group_Feature")]
		public string LabelText
		{
			get
			{
				return this._labelText;
			}
			set
			{
				if (this._labelText != value)
				{
					this._labelText = value;
					this.GetInnerWidget().SetLabelText(value);
					this.RaisePropertyChanged<string>(() => this.LabelText);
				}
			}
		}

		[UndoProperty]
		[Category("Group_Feature")]
		[PropertyOrder(58)]
		[DefaultValue("")]
		[DisplayName("Display_HAlign")]
		public TextHorizontalType HorizontalAlignmentType
		{
			get
			{
				return (TextHorizontalType)this.GetInnerWidget().GetHorizontalAlignmentType();
			}
			set
			{
				this.GetInnerWidget().SetHorizontalAlignmentType((int)value);
				this.RaisePropertyChanged<TextHorizontalType>(() => this.HorizontalAlignmentType);
			}
		}

		[PropertyOrder(59)]
		[Category("Group_Feature")]
		[UndoProperty]
		[DefaultValue("")]
		[DisplayName("Display_VAlign")]
		public TextVerticalType VerticalAlignmentType
		{
			get
			{
				return (TextVerticalType)this.GetInnerWidget().GetVerticalAlignmentType();
			}
			set
			{
				this.GetInnerWidget().SetVerticalAlignmentType((int)value);
				this.RaisePropertyChanged<TextVerticalType>(() => this.VerticalAlignmentType);
			}
		}

		[Category("Group_Feature")]
		[PropertyOrder(101)]
		[UndoProperty]
		[DisplayName("Property_ShadowEnabled")]
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
			this.GetInnerWidget().DisableShadow();
		}

		[UndoProperty]
		[Category("Group_Feature")]
		[DisplayName("Property_ShadowColor")]
		[PropertyOrder(102)]
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

		[Editor(typeof(ShadowOffsetEditor), typeof(ShadowOffsetEditor))]
		[UndoProperty]
		[PropertyOrder(103)]
		[Category("Group_Feature")]
		[DisplayName("Property_ShadowOffset")]
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
			if (this.ShadowEnabled)
			{
				Color4B color4B = new Color4B(this.ShadowColor.R, this.ShadowColor.G, this.ShadowColor.B, this.ShadowColor.A);
				SizeF offset = new SizeF(this.ShadowOffsetX, this.ShadowOffsetY);
				this.GetInnerWidget().EnableShadow(color4B, offset, this.ShadowBlurRadius);
			}
		}

		[DisplayName("Property_OutlineEnabled")]
		[UndoProperty]
		[PropertyOrder(104)]
		[Editor(typeof(OutlineEnableEditor), typeof(OutlineEnableEditor))]
		[Category("Group_Feature")]
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
		[DisplayName("Property_OutlineColor")]
		[PropertyOrder(105)]
		[Category("Group_Feature")]
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

		[Category("Group_Feature")]
		[DisplayName("Property_OutlineSize")]
		[PropertyOrder(106)]
		[UndoProperty]
		[ValueRange(0, 100, 1f, 10f)]
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
			if (this.OutlineEnabled)
			{
				Color4B color4B = new Color4B(this.OutlineColor.R, this.OutlineColor.G, this.OutlineColor.B, this.OutlineColor.A);
				int i = 0;
				while (i < 3)
				{
					try
					{
						this.GetInnerWidget().EnableOutline(color4B, this.OutlineSize);
						break;
					}
					catch (Exception ex)
					{
						this.DisableOutline();
						this.GetInnerWidget().EnableOutline(color4B, this.OutlineSize);
						i++;
					}
				}
			}
		}

		private void DisableOutline()
		{
			try
			{
				this.GetInnerWidget().DisableOutline();
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("禁用描边时出错", exception);
			}
		}

		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			TextObject textObject = cObject as TextObject;
			if (textObject != null)
			{
				textObject.FontResource = this.FontResource;
				textObject.FontSize = this.FontSize;
				textObject.LabelText = this.LabelText;
				textObject.HorizontalAlignmentType = this.HorizontalAlignmentType;
				textObject.VerticalAlignmentType = this.VerticalAlignmentType;
				textObject.TouchScaleChangeAble = this.TouchScaleChangeAble;
				textObject.FlipX = this.FlipX;
				textObject.FlipY = this.FlipY;
				textObject.IsCustomSize = this.IsCustomSize;
				textObject.Size = this.Size;
				textObject.ShadowEnabled = this.ShadowEnabled;
				textObject.ShadowOffsetX = this.ShadowOffsetX;
				textObject.ShadowOffsetY = this.ShadowOffsetY;
				textObject.ShadowBlurRadius = this.ShadowBlurRadius;
				textObject.ShadowColor = this.ShadowColor;
				textObject.OutlineEnabled = this.OutlineEnabled;
				textObject.OutlineSize = this.OutlineSize;
				textObject.OutlineColor = this.OutlineColor;
			}
		}

		protected override void OnMouseDoubleClick(MouseEventArgs args)
		{
			TextEditorWindow textEditorWindow = new TextEditorWindow(this, "LabelText", "CColor", "FontSize", true);
		}

		internal override void OnResourcePropertyChanged()
		{
			if (!this.IsCustomSize)
			{
				this.ResetSize();
			}
		}

		protected internal override bool IsCanChangeSize()
		{
			return this.IsCustomSize;
		}

		private ResourceFile file = null;

		private string _labelText = "";

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
