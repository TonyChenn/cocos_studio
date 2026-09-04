using System;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Editor;
using CocoStudio.Model.Interface;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000116 RID: 278
	[EngineClassName("TextField")]
	[DisplayName("Display_Component_UIField")]
	[ModelExtension(true, 9)]
	[ControlGroup("ComToolPad", 1)]
	public class TextFieldObject : WidgetObject, ICallBackEvent, IResetSize
	{
		// Token: 0x06000A7C RID: 2684 RVA: 0x00029D00 File Offset: 0x00027F00
		private CSTextField GetInnerWidget()
		{
			return (CSTextField)this.innerNode;
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x00029D1D File Offset: 0x00027F1D
		public TextFieldObject()
		{
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00029D45 File Offset: 0x00027F45
		public TextFieldObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x00029D6E File Offset: 0x00027F6E
		protected override void CreateCSObject()
		{
			this.innerNode = new CSTextField();
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00029D7C File Offset: 0x00027F7C
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				this.IsCustomSize = true;
				this.FontResource = null;
				this.FontSize = 20;
				this.LabelText = "";
				this.PlaceHolderText = "Text Field";
				this.Password = new PasswordValue(false, "*");
				this.AstrictLength = new AstrictLengthValue(false, this.LabelText.Length);
				this.TouchEnable = true;
				this.MaxLengthText = 10;
				this.ResetSize();
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x00029E0C File Offset: 0x0002800C
		public ObjectSizeType SupportSizeType
		{
			get
			{
				return ObjectSizeType.CustomOnly;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000A82 RID: 2690 RVA: 0x00029E20 File Offset: 0x00028020
		// (set) Token: 0x06000A83 RID: 2691 RVA: 0x00029E40 File Offset: 0x00028040
		[UndoProperty]
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

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x00029E98 File Offset: 0x00028098
		// (set) Token: 0x06000A85 RID: 2693 RVA: 0x00029EB0 File Offset: 0x000280B0
		[UndoProperty]
		[Category("Group_Feature")]
		[PropertyOrder(61)]
		[IgnoreResize]
		[ResourceFilter(new string[]
		{
			"ttf",
			"ttc"
		})]
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
					this.GetInnerWidget().SetFontName(WidgetObjectData.DefaultFont);
					this.GetInnerWidget().SetFontSize(this.FontSize);
				}
				else
				{
					this.GetInnerWidget().SetFontName(this.file.GetResourceData().Path);
				}
				this.RaisePropertyChanged<ResourceFile>(() => this.FontResource);
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000A86 RID: 2694 RVA: 0x00029F70 File Offset: 0x00028170
		// (set) Token: 0x06000A87 RID: 2695 RVA: 0x00029F90 File Offset: 0x00028190
		[DisplayName("Display_ColorBlend")]
		[PropertyOrder(13)]
		[UndoProperty]
		[Category("Group_Routine")]
		[Browsable(false)]
		public override Color CColor
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

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000A88 RID: 2696 RVA: 0x00029FE8 File Offset: 0x000281E8
		// (set) Token: 0x06000A89 RID: 2697 RVA: 0x0002A008 File Offset: 0x00028208
		[UndoProperty]
		[PropertyOrder(60)]
		[LayoutRefresh]
		[IgnoreResize]
		[ValueRange(5, 100, 1f, 10f)]
		[Editor(typeof(PropertyColorEditor), typeof(PropertyColorEditor))]
		[DisplayName("Display_FontStyle")]
		[Category("Group_Feature")]
		[Browsable(true)]
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
					this.GetInnerWidget().SetLabelText(this.LabelText);
					this.RaisePropertyChanged<int>(() => this.FontSize);
				}
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000A8A RID: 2698 RVA: 0x0002A08C File Offset: 0x0002828C
		// (set) Token: 0x06000A8B RID: 2699 RVA: 0x0002A0A4 File Offset: 0x000282A4
		[UndoProperty]
		[DisplayName("Display_Text")]
		[Category("Group_Feature")]
		[PropertyOrder(57)]
		[DefaultValue("input words here")]
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

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000A8C RID: 2700 RVA: 0x0002A118 File Offset: 0x00028318
		// (set) Token: 0x06000A8D RID: 2701 RVA: 0x0002A130 File Offset: 0x00028330
		[DefaultValue("")]
		[PropertyOrder(56)]
		[UndoProperty]
		[DisplayName("Placeholder_Text")]
		[Category("Group_Feature")]
		public string PlaceHolderText
		{
			get
			{
				return this._placeText;
			}
			set
			{
				if (this._placeText != value)
				{
					this._placeText = value;
					this.GetInnerWidget().SetPlaceHolderText(value);
					this.RaisePropertyChanged<string>(() => this.PlaceHolderText);
				}
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x0002A1A4 File Offset: 0x000283A4
		// (set) Token: 0x06000A8F RID: 2703 RVA: 0x0002A1D4 File Offset: 0x000283D4
		[UndoProperty]
		[DisplayName("ContexMenu_ShowCiphertext")]
		[Browsable(true)]
		[PropertyOrder(62)]
		[Editor(typeof(PasswordEditor), typeof(PasswordEditor))]
		[Category("Group_Feature")]
		public PasswordValue Password
		{
			get
			{
				return new PasswordValue(this.GetInnerWidget().GetPassWordEnabled(), this.GetInnerWidget().GetPasswordStyleText());
			}
			set
			{
				if (this.GetInnerWidget().GetPassWordEnabled() != value.PasswordEnable)
				{
					this.GetInnerWidget().SetPassWordEnabled(value.PasswordEnable);
				}
				if (value.PasswordStyleText != null)
				{
					this.GetInnerWidget().SetPasswordStyleText(value.PasswordStyleText);
				}
				this.RaisePropertyChanged<PasswordValue>(() => this.Password);
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x0002A268 File Offset: 0x00028468
		// (set) Token: 0x06000A91 RID: 2705 RVA: 0x0002A288 File Offset: 0x00028488
		[UndoProperty]
		public bool PasswordEnable
		{
			get
			{
				return this.GetInnerWidget().GetPassWordEnabled();
			}
			set
			{
				this.GetInnerWidget().SetPassWordEnabled(value);
				this.RaisePropertyChanged<bool>(() => this.PasswordEnable);
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000A92 RID: 2706 RVA: 0x0002A2E0 File Offset: 0x000284E0
		// (set) Token: 0x06000A93 RID: 2707 RVA: 0x0002A300 File Offset: 0x00028500
		[UndoProperty]
		public string PasswordStyleText
		{
			get
			{
				return this.GetInnerWidget().GetPasswordStyleText();
			}
			set
			{
				this.GetInnerWidget().SetPasswordStyleText(value);
				this.RaisePropertyChanged<string>(() => this.PasswordStyleText);
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x0002A358 File Offset: 0x00028558
		// (set) Token: 0x06000A95 RID: 2709 RVA: 0x0002A388 File Offset: 0x00028588
		[Editor(typeof(LengthLimitEditor), typeof(LengthLimitEditor))]
		[Browsable(true)]
		[UndoProperty]
		[DisplayName("Length_limit")]
		[Category("Group_Feature")]
		[PropertyOrder(63)]
		public AstrictLengthValue AstrictLength
		{
			get
			{
				return new AstrictLengthValue(this.GetInnerWidget().GetLengthLimited(), this.GetInnerWidget().GetMaxLength());
			}
			set
			{
				if (this.GetInnerWidget().GetLengthLimited() != value.MaxLengthEnable)
				{
					this.GetInnerWidget().SetLengthLimited(value.MaxLengthEnable);
				}
				this.GetInnerWidget().SetMaxLength(value.MaxLengthText);
				this.GetInnerWidget().SetLabelText(this._labelText);
				this.RaisePropertyChanged<AstrictLengthValue>(() => this.AstrictLength);
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000A96 RID: 2710 RVA: 0x0002A420 File Offset: 0x00028620
		// (set) Token: 0x06000A97 RID: 2711 RVA: 0x0002A440 File Offset: 0x00028640
		[UndoProperty]
		public bool MaxLengthEnable
		{
			get
			{
				return this.GetInnerWidget().GetLengthLimited();
			}
			set
			{
				this.GetInnerWidget().SetLengthLimited(value);
				this.RaisePropertyChanged<bool>(() => this.MaxLengthEnable);
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000A98 RID: 2712 RVA: 0x0002A498 File Offset: 0x00028698
		// (set) Token: 0x06000A99 RID: 2713 RVA: 0x0002A4B8 File Offset: 0x000286B8
		[UndoProperty]
		public int MaxLengthText
		{
			get
			{
				return this.GetInnerWidget().GetMaxLength();
			}
			set
			{
				this.GetInnerWidget().SetMaxLength(value);
				this.GetInnerWidget().SetLabelText(this._labelText);
				this.RaisePropertyChanged<int>(() => this.MaxLengthText);
			}
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x0002A520 File Offset: 0x00028720
		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			TextFieldObject textFieldObject = cObject as TextFieldObject;
			if (textFieldObject != null)
			{
				textFieldObject.Password = this.Password;
				textFieldObject.AstrictLength = this.AstrictLength;
				textFieldObject.FontResource = this.FontResource;
				textFieldObject.FontSize = this.FontSize;
				textFieldObject.LabelText = this.LabelText;
				textFieldObject.PlaceHolderText = this.PlaceHolderText;
				textFieldObject.Size = this.Size;
			}
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0002A5A8 File Offset: 0x000287A8
		protected override void OnMouseDoubleClick(MouseEventArgs args)
		{
			TextEditorWindow textEditorWindow = new TextEditorWindow(this, "PlaceHolderText", null, "FontSize", false);
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0002A5CC File Offset: 0x000287CC
		protected internal override bool IsCanChangeSize()
		{
			return true;
		}

		// Token: 0x0400046B RID: 1131
		private ResourceFile file = null;

		// Token: 0x0400046C RID: 1132
		private string _labelText = "";

		// Token: 0x0400046D RID: 1133
		private string _placeText = "";
	}
}
