using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataItem(Name = "ColorFrame")]
	[DataModelExtension(typeof(ColorFrame))]
	public class ColorFrameData : FrameData
	{
		[ItemProperty]
		[JsonProperty]
		public int Alpha { get; set; }

		[JsonProperty]
		[ItemProperty]
		public ColorData Color { get; set; }

		public ColorFrameData()
		{
			this.Alpha = 255;
		}

		public override bool FrameEquals(FrameData framedata)
		{
			ColorFrameData colorFrameData = framedata as ColorFrameData;
			bool flag = colorFrameData != null;
			return flag && base.FrameEquals(framedata) && this.Alpha == colorFrameData.Alpha && this.Color.Equals(colorFrameData.Color);
		}
	}
}
