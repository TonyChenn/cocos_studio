using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataItem(Name = "IntFrame")]
	[DataModelExtension(typeof(IntFrame))]
	public class IntFrameData : FrameData
	{
		[ItemProperty]
		[JsonProperty]
		public int Value { get; set; }

		public override bool FrameEquals(FrameData framedata)
		{
			IntFrameData intFrameData = framedata as IntFrameData;
			bool flag = intFrameData != null;
			return flag && base.FrameEquals(framedata) && this.Value == intFrameData.Value;
		}
	}
}
