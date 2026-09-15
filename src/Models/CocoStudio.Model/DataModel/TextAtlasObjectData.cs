using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(TextAtlasObject))]
	public class TextAtlasObjectData : WidgetObjectData
	{
		[ItemProperty]
		[JsonProperty]
		public int CharWidth { get; set; }

		[JsonProperty]
		[ItemProperty]
		public int CharHeight { get; set; }

		[JsonProperty]
		[ItemProperty]
		public string LabelText { get; set; }

		[JsonProperty]
		[ItemProperty]
		public string StartChar { get; set; }

		[ItemProperty]
		[JsonProperty]
		public ResourceItemData LabelAtlasFileImage_CNB
		{
			get
			{
				return this.labelAtlasFileImage_CNB;
			}
			set
			{
				this.labelAtlasFileImage_CNB = value;
				if (this.labelAtlasFileImage_CNB == null)
				{
					this.labelAtlasFileImage_CNB = TextAtlasObjectData.DefaultFile;
				}
			}
		}

		protected override void OnDataInitialize(VisualObject vObject)
		{
			TextAtlasObject textAtlasObject = vObject as TextAtlasObject;
			if (textAtlasObject != null)
			{
				if ((this.LabelAtlasFileImage_CNB != null && textAtlasObject.LabelAtlasFileImage_CNB.GetResourceData().Type != this.LabelAtlasFileImage_CNB.Type) || textAtlasObject.LabelAtlasFileImage_CNB.GetResourceData().Type == EnumResourceType.Default)
				{
					textAtlasObject.LabelAtlasFileImage_CNB = null;
				}
			}
		}

		internal static readonly ResourceItemData DefaultFile = new ResourceItemData(EnumResourceType.Default, "Default/TextAtlas.png");

		private ResourceItemData labelAtlasFileImage_CNB;
	}
}
