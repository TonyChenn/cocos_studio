using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(TextFieldObject))]
	public class TextFieldObjectData : WidgetObjectData
	{
		[ItemProperty]
		[JsonProperty]
		public ResourceItemData FontResource { get; set; }

		[ItemProperty]
		[JsonProperty]
		public int FontSize { get; set; }

		[JsonProperty]
		[ItemProperty]
		public bool IsCustomSize { get; set; }

		[ItemProperty]
		[JsonProperty]
		public string LabelText { get; set; }

		[JsonProperty]
		[ItemProperty]
		public string PlaceHolderText { get; set; }

		[ItemProperty(DefaultValue = false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(false)]
		public bool MaxLengthEnable { get; set; }

		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int MaxLengthText { get; set; }

		[ItemProperty(DefaultValue = false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(false)]
		public bool PasswordEnable { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = "*")]
		[DefaultValue("*")]
		public string PasswordStyleText { get; set; }

		public TextFieldObjectData()
		{
			this.PasswordStyleText = "*";
			this.IsCustomSize = true;
		}
	}
}
