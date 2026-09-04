using System;
using System.ComponentModel;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Editor;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x0200010E RID: 270
	[EngineClassName("TextAtlas")]
	[ModelExtension(true, 4)]
	[DisplayName("Display_Component_UILableAtlas")]
	[ControlGroup("ComToolPad", 10)]
	public class TextAtlasObject : WidgetObject, IResetSize
	{
		// Token: 0x060009AE RID: 2478 RVA: 0x00026C30 File Offset: 0x00024E30
		private CSTextAtlas GetInnerWidget()
		{
			return (CSTextAtlas)this.innerNode;
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x00026C4D File Offset: 0x00024E4D
		public TextAtlasObject()
		{
			base.SetDefaultSizeType(false);
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x00026C7D File Offset: 0x00024E7D
		public TextAtlasObject(ScriptFileData fileData) : base(fileData)
		{
			base.SetDefaultSizeType(false);
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x00026CAE File Offset: 0x00024EAE
		protected override void CreateCSObject()
		{
			this.innerNode = new CSTextAtlas();
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x00026CBC File Offset: 0x00024EBC
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				this.LabelAtlasFileImage_CNB = null;
				this.StartChar = ".";
				this.LabelText = "./0123456789";
				this.CharWidth = 14;
				this.CharHeight = 18;
			}
			this.CanShowStretch = false;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x00026D14 File Offset: 0x00024F14
		protected internal override string GetNamePrefix()
		{
			return "AtlasLabel_";
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x00026D2B File Offset: 0x00024F2B
		public override void InitOperation()
		{
			this.OperationFlag = (OperationMask)65519;
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x060009B5 RID: 2485 RVA: 0x00026D3C File Offset: 0x00024F3C
		// (set) Token: 0x060009B6 RID: 2486 RVA: 0x00026D54 File Offset: 0x00024F54
		[ResourceFilter(EnumResourceType.Normal, new string[]
		{
			"png",
			"jpg"
		})]
		[UndoProperty]
		[Editor(typeof(ResourceImageEditor), typeof(ResourceImageEditor))]
		[DefaultValue(null)]
		[Category("Group_Feature")]
		[PropertyOrder(76)]
		[DisplayName("Display_TagImage")]
		public virtual ResourceFile LabelAtlasFileImage_CNB
		{
			get
			{
				return this.file;
			}
			set
			{
				if (value == null || value is ImageFile || value == ResourceFile.DefaultMarker)
				{
					this.file = value;
					ImageFile defaultFile = new ImageFile(TextAtlasObjectData.DefaultFile);
					ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.file, defaultFile, false);
					string taskName = base.GetType().Name + "LabelAtlasFileImage_CNB";
					using (CompositeTask.Run(taskName, null))
					{
						this.GetInnerWidget().SetAtlasFile(resourceFile.GetResourceData());
						this.RaisePropertyChanged<int>(() => this.CharWidth);
						this.RaisePropertyChanged<int>(() => this.CharHeight);
						this.RaisePropertyChanged<ResourceFile>(() => this.LabelAtlasFileImage_CNB);
					}
				}
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x060009B7 RID: 2487 RVA: 0x00026EAC File Offset: 0x000250AC
		// (set) Token: 0x060009B8 RID: 2488 RVA: 0x00026ECC File Offset: 0x000250CC
		[DisplayName("Display_LabelFirstChar")]
		[Category("Group_Feature")]
		[DefaultValue("")]
		[LayoutRefresh]
		[Browsable(true)]
		[UndoProperty]
		[PropertyOrder(78)]
		public virtual string StartChar
		{
			get
			{
				return this.GetInnerWidget().GetStartChar();
			}
			set
			{
				if (this.GetInnerWidget().GetStartChar() != value)
				{
					this._startChar = value;
					this.GetInnerWidget().SetStartChar(value);
					this.RaisePropertyChanged<string>(() => this.StartChar);
				}
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x060009B9 RID: 2489 RVA: 0x00026F44 File Offset: 0x00025144
		// (set) Token: 0x060009BA RID: 2490 RVA: 0x00026F64 File Offset: 0x00025164
		[DefaultValue(12)]
		[PropertyOrder(79)]
		[UndoProperty]
		[DisplayName("Display_LabelCharWidth")]
		[Category("Group_Feature")]
		[ValueRange(0, 2147483647, 1f, 10f)]
		[LayoutRefresh]
		[Browsable(true)]
		public virtual int CharWidth
		{
			get
			{
				return this.GetInnerWidget().GetCharacterWidth();
			}
			set
			{
				if (this.GetInnerWidget().GetCharacterWidth() != value && value >= 0)
				{
					this.GetInnerWidget().SetCharacterWidth(value);
					this.RaisePropertyChanged<int>(() => this.CharWidth);
				}
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x060009BB RID: 2491 RVA: 0x00026FDC File Offset: 0x000251DC
		// (set) Token: 0x060009BC RID: 2492 RVA: 0x00026FFC File Offset: 0x000251FC
		[ValueRange(0, 2147483647, 1f, 10f)]
		[PropertyOrder(80)]
		[UndoProperty]
		[DisplayName("Display_LabelCharHeight")]
		[Category("Group_Feature")]
		[DefaultValue(12)]
		[LayoutRefresh]
		[Browsable(true)]
		public virtual int CharHeight
		{
			get
			{
				return this.GetInnerWidget().GetCharacterHeight();
			}
			set
			{
				if (this.GetInnerWidget().GetCharacterHeight() != value && value >= 0)
				{
					this.GetInnerWidget().SetCharacterHeight(value);
					this.RaisePropertyChanged<int>(() => this.CharHeight);
				}
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x00027074 File Offset: 0x00025274
		// (set) Token: 0x060009BE RID: 2494 RVA: 0x00027094 File Offset: 0x00025294
		[Editor(typeof(NumberEntryEditor), typeof(NumberEntryEditor))]
		[DefaultValue("")]
		[UndoProperty]
		[Category("Group_Feature")]
		[LayoutRefresh]
		[PropertyOrder(81)]
		[DisplayName("Display_Text")]
		public virtual string LabelText
		{
			get
			{
				return this.GetInnerWidget().GetText();
			}
			set
			{
				if (this.GetInnerWidget().GetText() != value)
				{
					this._labelText = value;
					this.GetInnerWidget().SetText(value);
					this.RaisePropertyChanged<string>(() => this.LabelText);
				}
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x060009BF RID: 2495 RVA: 0x0002710C File Offset: 0x0002530C
		[Editor(typeof(LabelTooltipEditor), typeof(LabelTooltipEditor))]
		[Category("Group_Feature")]
		[DefaultValue("")]
		[PropertyOrder(77)]
		[DisplayName("")]
		public string LabelToop
		{
			get
			{
				return LanguageInfo.TextAtlasResourceExplain;
			}
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x00027124 File Offset: 0x00025324
		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			TextAtlasObject textAtlasObject = cObject as TextAtlasObject;
			if (textAtlasObject != null)
			{
				textAtlasObject.LabelAtlasFileImage_CNB = this.LabelAtlasFileImage_CNB;
				textAtlasObject.StartChar = this.StartChar;
				textAtlasObject.CharWidth = this.CharWidth;
				textAtlasObject.CharHeight = this.CharHeight;
				textAtlasObject.LabelText = this.LabelText;
			}
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x00027190 File Offset: 0x00025390
		protected override void OnMouseDoubleClick(MouseEventArgs args)
		{
			TextEditorWindow textEditorWindow = new TextEditorWindow(this, "LabelText", null, null, false);
		}

		// Token: 0x0400044B RID: 1099
		private ResourceFile file = null;

		// Token: 0x0400044C RID: 1100
		private string _startChar = "";

		// Token: 0x0400044D RID: 1101
		private string _labelText = "";
	}
}
