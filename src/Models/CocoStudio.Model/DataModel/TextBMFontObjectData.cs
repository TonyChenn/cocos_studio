using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(TextBMFontObject))]
	public class TextBMFontObjectData : WidgetObjectData
	{
		[ItemProperty]
		[JsonProperty]
		public string LabelText { get; set; }

		[JsonProperty]
		[ItemProperty]
		public ResourceItemData LabelBMFontFile_CNB
		{
			get
			{
				return this.labelBMFontFile_CNB;
			}
			set
			{
				this.labelBMFontFile_CNB = value;
				if (this.labelBMFontFile_CNB == null)
				{
					this.labelBMFontFile_CNB = TextBMFontObjectData.DefaultFntFont;
				}
			}
		}

		protected override void OnDataInitialize(VisualObject vObject)
		{
			TextBMFontObject textBMFontObject = vObject as TextBMFontObject;
			if (textBMFontObject != null)
			{
				if (this.labelBMFontFile_CNB != null && textBMFontObject.LabelBMFontFile_CNB.GetResourceData().Type != this.labelBMFontFile_CNB.Type)
				{
					textBMFontObject.LabelBMFontFile_CNB = null;
				}
			}
		}

		internal static readonly ResourceItemData DefaultFntFont = new ResourceItemData(EnumResourceType.Default, "Default/defaultBMFont.fnt");

		private ResourceItemData labelBMFontFile_CNB;
	}
}
