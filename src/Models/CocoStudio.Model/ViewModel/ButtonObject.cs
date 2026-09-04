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
	// Token: 0x02000109 RID: 265
	[EngineClassName("Button")]
	[DisplayName("Display_Component_UIButton")]
	[ModelExtension(true, 0)]
	[ControlGroup("ComToolPad", 1)]
	public class ButtonObject : WidgetObject, IScale9, IDisplayState, IResetSize, IFlipped
	{
		// Token: 0x060008E8 RID: 2280 RVA: 0x00023898 File Offset: 0x00021A98
		private CSButton GetInnerWidget()
		{
			return (CSButton)this.innerNode;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x000238B8 File Offset: 0x00021AB8
		public ButtonObject()
		{
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00023994 File Offset: 0x00021B94
		public ButtonObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x00023A6F File Offset: 0x00021C6F
		protected override void CreateCSObject()
		{
			this.innerNode = new CSButton();
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x00023A80 File Offset: 0x00021C80
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

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x060008ED RID: 2285 RVA: 0x00023B80 File Offset: 0x00021D80
		// (set) Token: 0x060008EE RID: 2286 RVA: 0x00023B98 File Offset: 0x00021D98
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

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x060008EF RID: 2287 RVA: 0x00023BFC File Offset: 0x00021DFC
		// (set) Token: 0x060008F0 RID: 2288 RVA: 0x00023C39 File Offset: 0x00021E39
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

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x00023C44 File Offset: 0x00021E44
		// (set) Token: 0x060008F2 RID: 2290 RVA: 0x00023C8C File Offset: 0x00021E8C
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

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x060008F3 RID: 2291 RVA: 0x00023D08 File Offset: 0x00021F08
		// (set) Token: 0x060008F4 RID: 2292 RVA: 0x00023D50 File Offset: 0x00021F50
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

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x00023DCC File Offset: 0x00021FCC
		// (set) Token: 0x060008F6 RID: 2294 RVA: 0x00023E14 File Offset: 0x00022014
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

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x00023E90 File Offset: 0x00022090
		// (set) Token: 0x060008F8 RID: 2296 RVA: 0x00023ED8 File Offset: 0x000220D8
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

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060008F9 RID: 2297 RVA: 0x00023FD4 File Offset: 0x000221D4
		// (set) Token: 0x060008FA RID: 2298 RVA: 0x00023FF4 File Offset: 0x000221F4
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

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060008FB RID: 2299 RVA: 0x00024070 File Offset: 0x00022270
		// (set) Token: 0x060008FC RID: 2300 RVA: 0x00024090 File Offset: 0x00022290
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

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x00024118 File Offset: 0x00022318
		// (set) Token: 0x060008FE RID: 2302 RVA: 0x00024130 File Offset: 0x00022330
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

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x000241D4 File Offset: 0x000223D4
		// (set) Token: 0x06000900 RID: 2304 RVA: 0x000241F7 File Offset: 0x000223F7
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

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x00024214 File Offset: 0x00022414
		// (set) Token: 0x06000902 RID: 2306 RVA: 0x00024234 File Offset: 0x00022434
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

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x000242A0 File Offset: 0x000224A0
		// (set) Token: 0x06000904 RID: 2308 RVA: 0x000242C0 File Offset: 0x000224C0
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

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x0002432C File Offset: 0x0002252C
		public bool IsReverse
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x00024340 File Offset: 0x00022540
		// (set) Token: 0x06000907 RID: 2311 RVA: 0x00024358 File Offset: 0x00022558
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

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000908 RID: 2312 RVA: 0x000243B8 File Offset: 0x000225B8
		// (set) Token: 0x06000909 RID: 2313 RVA: 0x000243D8 File Offset: 0x000225D8
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

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x0002443C File Offset: 0x0002263C
		// (set) Token: 0x0600090B RID: 2315 RVA: 0x0002445C File Offset: 0x0002265C
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

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x0600090C RID: 2316 RVA: 0x000244C0 File Offset: 0x000226C0
		// (set) Token: 0x0600090D RID: 2317 RVA: 0x000244E0 File Offset: 0x000226E0
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

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x00024544 File Offset: 0x00022744
		// (set) Token: 0x0600090F RID: 2319 RVA: 0x00024564 File Offset: 0x00022764
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

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x000245C8 File Offset: 0x000227C8
		public SizeF ResourceSize
		{
			get
			{
				return this.GetInnerWidget().GetWidgetAutoSize();
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000911 RID: 2321 RVA: 0x000245E8 File Offset: 0x000227E8
		// (set) Token: 0x06000912 RID: 2322 RVA: 0x00024605 File Offset: 0x00022805
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

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000913 RID: 2323 RVA: 0x00024608 File Offset: 0x00022808
		// (set) Token: 0x06000914 RID: 2324 RVA: 0x00024625 File Offset: 0x00022825
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

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x00024628 File Offset: 0x00022828
		// (set) Token: 0x06000916 RID: 2326 RVA: 0x00024645 File Offset: 0x00022845
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

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000917 RID: 2327 RVA: 0x00024648 File Offset: 0x00022848
		// (set) Token: 0x06000918 RID: 2328 RVA: 0x00024665 File Offset: 0x00022865
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

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000919 RID: 2329 RVA: 0x00024668 File Offset: 0x00022868
		// (set) Token: 0x0600091A RID: 2330 RVA: 0x00024680 File Offset: 0x00022880
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

		// Token: 0x0600091B RID: 2331 RVA: 0x000246F8 File Offset: 0x000228F8
		private void DisableShadow()
		{
			this.GetInnerWidget().DisabledEffect();
			if (this.OutlineEnabled)
			{
				this.EnableOutline();
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x0600091C RID: 2332 RVA: 0x00024728 File Offset: 0x00022928
		// (set) Token: 0x0600091D RID: 2333 RVA: 0x00024740 File Offset: 0x00022940
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

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x000247AC File Offset: 0x000229AC
		// (set) Token: 0x0600091F RID: 2335 RVA: 0x000247C4 File Offset: 0x000229C4
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

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x0002482C File Offset: 0x00022A2C
		// (set) Token: 0x06000921 RID: 2337 RVA: 0x00024844 File Offset: 0x00022A44
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

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000922 RID: 2338 RVA: 0x000248AC File Offset: 0x00022AAC
		// (set) Token: 0x06000923 RID: 2339 RVA: 0x000248C4 File Offset: 0x00022AC4
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

		// Token: 0x06000924 RID: 2340 RVA: 0x0002492A File Offset: 0x00022B2A
		private void EnableShadow()
		{
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x00024930 File Offset: 0x00022B30
		// (set) Token: 0x06000926 RID: 2342 RVA: 0x00024948 File Offset: 0x00022B48
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

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x000249C0 File Offset: 0x00022BC0
		// (set) Token: 0x06000928 RID: 2344 RVA: 0x000249D8 File Offset: 0x00022BD8
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

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x00024A44 File Offset: 0x00022C44
		// (set) Token: 0x0600092A RID: 2346 RVA: 0x00024A5C File Offset: 0x00022C5C
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

		// Token: 0x0600092B RID: 2347 RVA: 0x00024ADA File Offset: 0x00022CDA
		private void EnableOutline()
		{
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x00024AE0 File Offset: 0x00022CE0
		private void DisableOutline()
		{
			this.GetInnerWidget().DisabledEffect();
			if (this.ShadowEnabled)
			{
				this.EnableShadow();
			}
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x00024B10 File Offset: 0x00022D10
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

		// Token: 0x0600092E RID: 2350 RVA: 0x00024C78 File Offset: 0x00022E78
		protected override void OnMouseDoubleClick(MouseEventArgs args)
		{
			TextEditorWindow textEditorWindow = new TextEditorWindow(this, "ButtonText", "TextColor", "FontSize", false);
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x00024CA0 File Offset: 0x00022EA0
		protected internal override bool IsCanChangeSize()
		{
			return true;
		}

		// Token: 0x0400041D RID: 1053
		private bool isNormal = true;

		// Token: 0x0400041E RID: 1054
		private ResourceFile normalFile = null;

		// Token: 0x0400041F RID: 1055
		private ResourceFile pressedFile = null;

		// Token: 0x04000420 RID: 1056
		private ResourceFile disabledFile = null;

		// Token: 0x04000421 RID: 1057
		private ResourceFile fontFile = null;

		// Token: 0x04000422 RID: 1058
		private string text;

		// Token: 0x04000423 RID: 1059
		private Color _textColor = Color.FromArgb(255, 255, 255, 255);

		// Token: 0x04000424 RID: 1060
		private bool _scale9Enabled = false;

		// Token: 0x04000425 RID: 1061
		private int _left = 0;

		// Token: 0x04000426 RID: 1062
		private int _right = 0;

		// Token: 0x04000427 RID: 1063
		private int _top = 0;

		// Token: 0x04000428 RID: 1064
		private int _bottom = 0;

		// Token: 0x04000429 RID: 1065
		private bool shadowEnabled = false;

		// Token: 0x0400042A RID: 1066
		private Color shadowColor = Color.FromArgb(255, 110, 110, 110);

		// Token: 0x0400042B RID: 1067
		private float shadowOffsetX = 2f;

		// Token: 0x0400042C RID: 1068
		private float shadowOffsetY = -2f;

		// Token: 0x0400042D RID: 1069
		private int shadowBlurRadius = 0;

		// Token: 0x0400042E RID: 1070
		private bool outlineEnabled = false;

		// Token: 0x0400042F RID: 1071
		private Color outlineColor = Color.FromArgb(255, 255, 0, 0);

		// Token: 0x04000430 RID: 1072
		private int outlineSize = 1;
	}
}
