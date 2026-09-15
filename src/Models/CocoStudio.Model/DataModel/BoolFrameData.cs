using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataItem(Name = "BoolFrame")]
	[DataModelExtension(typeof(BoolFrame))]
	public class BoolFrameData : FrameData
	{
		[JsonProperty]
		[ItemProperty]
		public bool Value { get; set; }

		public override bool FrameEquals(FrameData framedata)
		{
			BoolFrameData boolFrameData = framedata as BoolFrameData;
			bool flag = boolFrameData != null;
			return flag && base.FrameEquals(framedata) && this.Value == boolFrameData.Value;
		}
	}
}
