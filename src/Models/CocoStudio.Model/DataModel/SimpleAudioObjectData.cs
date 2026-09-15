using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(SimpleAudioObject))]
	public class SimpleAudioObjectData : NodeObjectData
	{
		[ItemProperty(DefaultValue = 0f)]
		[DefaultValue(0f)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public float Volume { get; set; }

		[ItemProperty(DefaultValue = false)]
		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public bool Loop { get; set; }

		[ItemProperty]
		[JsonProperty]
		public ResourceItemData FileData { get; set; }
	}
}
