using System;
using System.ComponentModel;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Editor;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x0200010F RID: 271
	[EngineClassName("TextBMFont")]
	[DisplayName("Display_Component_UILableBMFont")]
	[ModelExtension(true, 5)]
	[ControlGroup("ComToolPad", 1)]
	public class TextBMFontObject : WidgetObject
	{
		// Token: 0x060009C2 RID: 2498 RVA: 0x000271B0 File Offset: 0x000253B0
		private CSTextBMFont GetInnerWidget()
		{
			return (CSTextBMFont)this.innerNode;
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x000271CD File Offset: 0x000253CD
		public TextBMFontObject()
		{
			base.SetDefaultSizeType(false);
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x000271F2 File Offset: 0x000253F2
		public TextBMFontObject(ScriptFileData fileData) : base(fileData)
		{
			base.SetDefaultSizeType(false);
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x00027218 File Offset: 0x00025418
		protected override void CreateCSObject()
		{
			this.innerNode = new CSTextBMFont();
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x00027228 File Offset: 0x00025428
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				this.LabelBMFontFile_CNB = null;
				this.LabelText = "Fnt Text Label";
			}
			this.CanShowStretch = false;
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x00027264 File Offset: 0x00025464
		protected internal override string GetNamePrefix()
		{
			return "BitmapFontLabel_";
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x0002727C File Offset: 0x0002547C
		// (set) Token: 0x060009C9 RID: 2505 RVA: 0x00027294 File Offset: 0x00025494
		[DefaultValue("")]
		[Category("Group_Feature")]
		[UndoProperty]
		[PropertyOrder(81)]
		[LayoutRefresh]
		[DisplayName("Display_Text")]
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
					this.GetInnerWidget().SetText(value);
					this.RaisePropertyChanged<string>(() => this.LabelText);
				}
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x00027308 File Offset: 0x00025508
		// (set) Token: 0x060009CB RID: 2507 RVA: 0x00027320 File Offset: 0x00025520
		[DisplayName("Display_FNTFile")]
		[Category("Group_Feature")]
		[UndoProperty]
		[ResourceFilter(new string[]
		{
			"fnt"
		})]
		[DefaultValue(null)]
		[PropertyOrder(76)]
		[Editor(typeof(ResourceImageEditor), typeof(ResourceImageEditor))]
		public ResourceFile LabelBMFontFile_CNB
		{
			get
			{
				return this.filePath;
			}
			set
			{
				this.filePath = value;
				FntFile defaultFile = new FntFile(TextBMFontObjectData.DefaultFntFont);
				if (value == null || value.DataError != null || value.IsDefault)
				{
					this.filePath = defaultFile;
				}
				ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.filePath, defaultFile, false);
				this.GetInnerWidget().SetFntFile(resourceFile.GetResourceData());
				this.RaisePropertyChanged<ResourceFile>(() => this.LabelBMFontFile_CNB);
			}
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x000273C0 File Offset: 0x000255C0
		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			TextBMFontObject textBMFontObject = cObject as TextBMFontObject;
			if (textBMFontObject != null)
			{
				textBMFontObject.LabelBMFontFile_CNB = this.LabelBMFontFile_CNB;
				textBMFontObject.LabelText = this.LabelText;
			}
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x00027404 File Offset: 0x00025604
		protected override void OnMouseDoubleClick(MouseEventArgs args)
		{
			TextEditorWindow textEditorWindow = new TextEditorWindow(this, "LabelText", "CColor", null, false);
		}

		// Token: 0x0400044E RID: 1102
		private string _labelText = "";

		// Token: 0x0400044F RID: 1103
		private ResourceFile filePath = null;
	}
}
