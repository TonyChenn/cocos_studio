using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(FloatFrame))]
	[DataItem(Name = "FloatFrame")]
	public class FloatFrameData : FrameData
	{
		[JsonProperty]
		[ItemProperty]
		public float Value { get; set; }

		public override bool FrameEquals(FrameData framedata)
		{
			FloatFrameData floatFrameData = framedata as FloatFrameData;
			bool flag = floatFrameData != null;
			return flag && base.FrameEquals(framedata) && this.Value == floatFrameData.Value;
		}
	}
}
