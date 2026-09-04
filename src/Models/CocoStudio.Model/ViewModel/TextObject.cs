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
	// Token: 0x0200010D RID: 269
	[ControlGroup("ComToolPad", 1)]
	[EngineClassName("Text")]
	[DisplayName("Display_Component_UILable")]
	[ModelExtension(true, 8)]
	public class TextObject : WidgetObject, ISizeType, IFlipped, IResetSize, ILabelEffect
	{
		// Token: 0x06000979 RID: 2425 RVA: 0x00025CF8 File Offset: 0x00023EF8
		private CSText GetInnerWidget()
		{
			return (CSText)this.innerNode;
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00025D18 File Offset: 0x00023F18
		public TextObject()
		{
			base.SetDefaultSizeType(false);
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x00025DA8 File Offset: 0x00023FA8
		public TextObject(ScriptFileData fileData) : base(fileData)
		{
			base.SetDefaultSizeType(false);
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x00025E38 File Offset: 0x00024038
		protected override void CreateCSObject()
		{
			this.innerNode = new CSText();
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x00025E48 File Offset: 0x00024048
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

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x0600097E RID: 2430 RVA: 0x00025E90 File Offset: 0x00024090
		public ObjectSizeType SupportSizeType
		{
			get
			{
				return ObjectSizeType.AutoAndCustom;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x00025EA4 File Offset: 0x000240A4
		// (set) Token: 0x06000980 RID: 2432 RVA: 0x00025EC4 File Offset: 0x000240C4
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

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x00025F1C File Offset: 0x0002411C
		// (set) Token: 0x06000982 RID: 2434 RVA: 0x00025F3C File Offset: 0x0002413C
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

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x00025F94 File Offset: 0x00024194
		// (set) Token: 0x06000984 RID: 2436 RVA: 0x00025FB7 File Offset: 0x000241B7
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

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x00025FD4 File Offset: 0x000241D4
		// (set) Token: 0x06000986 RID: 2438 RVA: 0x00025FF4 File Offset: 0x000241F4
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

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000987 RID: 2439 RVA: 0x00026060 File Offset: 0x00024260
		// (set) Token: 0x06000988 RID: 2440 RVA: 0x00026080 File Offset: 0x00024280
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

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x000260EC File Offset: 0x000242EC
		public bool IsReverse
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x0600098A RID: 2442 RVA: 0x00026100 File Offset: 0x00024300
		// (set) Token: 0x0600098B RID: 2443 RVA: 0x00026118 File Offset: 0x00024318
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

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x0600098C RID: 2444 RVA: 0x0002621C File Offset: 0x0002441C
		// (set) Token: 0x0600098D RID: 2445 RVA: 0x00026258 File Offset: 0x00024458
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

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x000262D0 File Offset: 0x000244D0
		// (set) Token: 0x0600098F RID: 2447 RVA: 0x000262F0 File Offset: 0x000244F0
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

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000990 RID: 2448 RVA: 0x00026360 File Offset: 0x00024560
		// (set) Token: 0x06000991 RID: 2449 RVA: 0x00026378 File Offset: 0x00024578
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

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000992 RID: 2450 RVA: 0x000263EC File Offset: 0x000245EC
		// (set) Token: 0x06000993 RID: 2451 RVA: 0x0002640C File Offset: 0x0002460C
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

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x00026464 File Offset: 0x00024664
		// (set) Token: 0x06000995 RID: 2453 RVA: 0x00026484 File Offset: 0x00024684
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

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x000264DC File Offset: 0x000246DC
		// (set) Token: 0x06000997 RID: 2455 RVA: 0x000264F4 File Offset: 0x000246F4
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

		// Token: 0x06000998 RID: 2456 RVA: 0x0002656C File Offset: 0x0002476C
		private void DisableShadow()
		{
			this.GetInnerWidget().DisableShadow();
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000999 RID: 2457 RVA: 0x0002657C File Offset: 0x0002477C
		// (set) Token: 0x0600099A RID: 2458 RVA: 0x00026594 File Offset: 0x00024794
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

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x00026600 File Offset: 0x00024800
		// (set) Token: 0x0600099C RID: 2460 RVA: 0x00026618 File Offset: 0x00024818
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

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x00026680 File Offset: 0x00024880
		// (set) Token: 0x0600099E RID: 2462 RVA: 0x00026698 File Offset: 0x00024898
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

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x0600099F RID: 2463 RVA: 0x00026700 File Offset: 0x00024900
		// (set) Token: 0x060009A0 RID: 2464 RVA: 0x00026718 File Offset: 0x00024918
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

		// Token: 0x060009A1 RID: 2465 RVA: 0x00026780 File Offset: 0x00024980
		private void EnableShadow()
		{
			if (this.ShadowEnabled)
			{
				Color4B color4B = new Color4B(this.ShadowColor.R, this.ShadowColor.G, this.ShadowColor.B, this.ShadowColor.A);
				SizeF offset = new SizeF(this.ShadowOffsetX, this.ShadowOffsetY);
				this.GetInnerWidget().EnableShadow(color4B, offset, this.ShadowBlurRadius);
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x060009A2 RID: 2466 RVA: 0x00026804 File Offset: 0x00024A04
		// (set) Token: 0x060009A3 RID: 2467 RVA: 0x0002681C File Offset: 0x00024A1C
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

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x060009A4 RID: 2468 RVA: 0x00026894 File Offset: 0x00024A94
		// (set) Token: 0x060009A5 RID: 2469 RVA: 0x000268AC File Offset: 0x00024AAC
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

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x00026918 File Offset: 0x00024B18
		// (set) Token: 0x060009A7 RID: 2471 RVA: 0x00026930 File Offset: 0x00024B30
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

		// Token: 0x060009A8 RID: 2472 RVA: 0x000269B0 File Offset: 0x00024BB0
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

		// Token: 0x060009A9 RID: 2473 RVA: 0x00026A6C File Offset: 0x00024C6C
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

		// Token: 0x060009AA RID: 2474 RVA: 0x00026AB4 File Offset: 0x00024CB4
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

		// Token: 0x060009AB RID: 2475 RVA: 0x00026BCC File Offset: 0x00024DCC
		protected override void OnMouseDoubleClick(MouseEventArgs args)
		{
			TextEditorWindow textEditorWindow = new TextEditorWindow(this, "LabelText", "CColor", "FontSize", true);
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x00026BF4 File Offset: 0x00024DF4
		internal override void OnResourcePropertyChanged()
		{
			if (!this.IsCustomSize)
			{
				this.ResetSize();
			}
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x00026C18 File Offset: 0x00024E18
		protected internal override bool IsCanChangeSize()
		{
			return this.IsCustomSize;
		}

		// Token: 0x04000441 RID: 1089
		private ResourceFile file = null;

		// Token: 0x04000442 RID: 1090
		private string _labelText = "";

		// Token: 0x04000443 RID: 1091
		private bool shadowEnabled = false;

		// Token: 0x04000444 RID: 1092
		private Color shadowColor = Color.FromArgb(255, 110, 110, 110);

		// Token: 0x04000445 RID: 1093
		private float shadowOffsetX = 2f;

		// Token: 0x04000446 RID: 1094
		private float shadowOffsetY = -2f;

		// Token: 0x04000447 RID: 1095
		private int shadowBlurRadius = 0;

		// Token: 0x04000448 RID: 1096
		private bool outlineEnabled = false;

		// Token: 0x04000449 RID: 1097
		private Color outlineColor = Color.FromArgb(255, 255, 0, 0);

		// Token: 0x0400044A RID: 1098
		private int outlineSize = 1;
	}
}
