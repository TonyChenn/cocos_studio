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
	[EngineClassName("TextField")]
	[DisplayName("Display_Component_UIField")]
	[ModelExtension(true, 9)]
	[ControlGroup("ComToolPad", 1)]
	public class TextFieldObject : WidgetObject, ICallBackEvent, IResetSize
	{
		private CSTextField GetInnerWidget()
		{
			return (CSTextField)this.innerNode;
		}

		public TextFieldObject()
		{
		}

		public TextFieldObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSTextField();
		}

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

		public ObjectSizeType SupportSizeType
		{
			get
			{
				return ObjectSizeType.CustomOnly;
			}
		}

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

		protected override void OnMouseDoubleClick(MouseEventArgs args)
		{
			TextEditorWindow textEditorWindow = new TextEditorWindow(this, "PlaceHolderText", null, "FontSize", false);
		}

		protected internal override bool IsCanChangeSize()
		{
			return true;
		}

		private ResourceFile file = null;

		private string _labelText = "";

		private string _placeText = "";
	}
}
