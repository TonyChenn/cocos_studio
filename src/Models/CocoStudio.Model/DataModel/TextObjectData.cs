using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataInclude(typeof(TextHorizontalType))]
	[DataModelExtension(typeof(TextObject))]
	[DataInclude(typeof(TextVerticalType))]
	public class TextObjectData : WidgetObjectData
	{
		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		public bool IsCustomSize { get; set; }

		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		public bool FlipX { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		[DefaultValue(false)]
		public bool FlipY { get; set; }

		[JsonProperty]
		[ItemProperty]
		public int FontSize { get; set; }

		[ItemProperty]
		[JsonProperty]
		public string LabelText { get; set; }

		[JsonProperty]
		[ItemProperty]
		public ResourceItemData FontResource { get; set; }

		[ItemProperty(DefaultValue = false)]
		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public bool TouchScaleChangeAble { get; set; }

		[DefaultValue(TextHorizontalType.HT_Left)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = TextHorizontalType.HT_Left)]
		public TextHorizontalType HorizontalAlignmentType { get; set; }

		[ItemProperty(DefaultValue = TextVerticalType.VT_Top)]
		[DefaultValue(TextVerticalType.VT_Top)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public TextVerticalType VerticalAlignmentType { get; set; }

		[ItemProperty(DefaultValue = 1)]
		[DefaultValue(1)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int OutlineSize { get; set; }

		[ItemProperty]
		[JsonProperty]
		public ColorData OutlineColor { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(false)]
		[ItemProperty(DefaultValue = false)]
		public bool OutlineEnabled { get; set; }

		[ItemProperty]
		[JsonProperty]
		public ColorData ShadowColor { get; set; }

		[DefaultValue(2)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 2)]
		public float ShadowOffsetX { get; set; }

		[ItemProperty(DefaultValue = -2)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(-2)]
		public float ShadowOffsetY { get; set; }

		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int ShadowBlurRadius { get; set; }

		[DefaultValue(false)]
		[ItemProperty(DefaultValue = false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public bool ShadowEnabled { get; set; }

		public TextObjectData()
		{
			this.IsCustomSize = false;
			this.ShadowOffsetX = 2f;
			this.ShadowOffsetY = -2f;
			this.OutlineSize = 1;
		}

		protected override void OnDataInitialize(VisualObject vObject)
		{
			TextObject textObject = vObject as TextObject;
			if (textObject != null)
			{
				if ((this.IsCustomSize && this.FontResource != null && textObject.FontResource.GetResourceData().Type == this.FontResource.Type) || !this.IsCustomSize)
				{
					textObject.Size = base.Size;
				}
			}
		}
	}
}
