using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(ScaleValueFrame))]
	[DataItem(Name = "ScaleFrame")]
	public class ScaleValueFrameData : FrameData
	{
		[JsonProperty]
		[ItemProperty]
		public float X { get; set; }

		[JsonProperty]
		[ItemProperty]
		public float Y { get; set; }

		public override bool FrameEquals(FrameData framedata)
		{
			ScaleValueFrameData scaleValueFrameData = framedata as ScaleValueFrameData;
			bool flag = scaleValueFrameData != null;
			return flag && base.FrameEquals(framedata) && FrameData.IsFloatEqual(this.X, scaleValueFrameData.X, 0.0001f) && FrameData.IsFloatEqual(this.Y, scaleValueFrameData.Y, 0.0001f);
		}
	}
}
