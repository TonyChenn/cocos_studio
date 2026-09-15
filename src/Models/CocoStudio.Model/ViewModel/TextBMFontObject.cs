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
	[EngineClassName("TextBMFont")]
	[DisplayName("Display_Component_UILableBMFont")]
	[ModelExtension(true, 5)]
	[ControlGroup("ComToolPad", 1)]
	public class TextBMFontObject : WidgetObject
	{
		private CSTextBMFont GetInnerWidget()
		{
			return (CSTextBMFont)this.innerNode;
		}

		public TextBMFontObject()
		{
			base.SetDefaultSizeType(false);
		}

		public TextBMFontObject(ScriptFileData fileData) : base(fileData)
		{
			base.SetDefaultSizeType(false);
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSTextBMFont();
		}

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

		protected internal override string GetNamePrefix()
		{
			return "BitmapFontLabel_";
		}

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

		protected override void OnMouseDoubleClick(MouseEventArgs args)
		{
			TextEditorWindow textEditorWindow = new TextEditorWindow(this, "LabelText", "CColor", null, false);
		}

		private string _labelText = "";

		private ResourceFile filePath = null;
	}
}
