using System;
using System.Collections.Generic;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension]
	public class GameFileData : BaseObjectData
	{
		[ItemProperty]
		[JsonProperty]
		public TimelineActionData Animation { get; set; }

		[ItemProperty]
		[JsonProperty]
		public List<AnimationInfoData> AnimationList { get; set; }

		[JsonProperty]
		[ItemProperty]
		public AbstractNodeObjectData ObjectData { get; set; }

		[JsonProperty]
		public List<string> UsedResources { get; set; }

		public GameFileData()
		{
			this.Animation = new TimelineActionData();
			this.AnimationList = new List<AnimationInfoData>();
		}
	}
}
