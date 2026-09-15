using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Modules.Communal.PropertyGrid;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(WidgetObject))]
	[DataInclude(typeof(UISizeType))]
	public class WidgetObjectData : NodeObjectData
	{
		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		public bool TouchEnable { get; set; }

		[ItemProperty(DefaultValue = false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[PropertyOrder(2147483647)]
		[DefaultValue(false)]
		public bool StretchWidthEnable { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		[DefaultValue(false)]
		[PropertyOrder(2147483647)]
		public bool StretchHeightEnable { get; set; }

		internal static readonly string DefaultFont = "";
	}
}
