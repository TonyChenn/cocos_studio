using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataItem(Name = "AnimationInfo")]
	[DataModelExtension(typeof(AnimationInfo))]
	public class AnimationInfoData : BaseObjectData
	{
		[JsonProperty]
		[ItemProperty]
		public int StartIndex { get; set; }

		[JsonProperty]
		[ItemProperty]
		public int EndIndex { get; set; }

		[ItemProperty]
		public ColorData RenderColor { get; set; }
	}
}
