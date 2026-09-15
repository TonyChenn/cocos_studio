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
	[EngineClassName("TextAtlas")]
	[ModelExtension(true, 4)]
	[DisplayName("Display_Component_UILableAtlas")]
	[ControlGroup("ComToolPad", 10)]
	public class TextAtlasObject : WidgetObject, IResetSize
	{
		private CSTextAtlas GetInnerWidget()
		{
			return (CSTextAtlas)this.innerNode;
		}

		public TextAtlasObject()
		{
			base.SetDefaultSizeType(false);
		}

		public TextAtlasObject(ScriptFileData fileData) : base(fileData)
		{
			base.SetDefaultSizeType(false);
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSTextAtlas();
		}

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

		protected internal override string GetNamePrefix()
		{
			return "AtlasLabel_";
		}

		public override void InitOperation()
		{
			this.OperationFlag = (OperationMask)65519;
		}

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

		protected override void OnMouseDoubleClick(MouseEventArgs args)
		{
			TextEditorWindow textEditorWindow = new TextEditorWindow(this, "LabelText", null, null, false);
		}

		private ResourceFile file = null;

		private string _startChar = "";

		private string _labelText = "";
	}
}
