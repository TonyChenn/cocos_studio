using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataItem(Name = "BlendFuncFrame")]
	[DataModelExtension(typeof(BlendFuncFrame))]
	public class BlendFuncFrameData : FrameData
	{
		[ItemProperty]
		[JsonProperty]
		public int Src { get; set; }

		[ItemProperty]
		[JsonProperty]
		public int Dst { get; set; }

		public override bool FrameEquals(FrameData framedata)
		{
			BlendFuncFrameData blendFuncFrameData = framedata as BlendFuncFrameData;
			return blendFuncFrameData != null && base.FrameEquals(framedata) && this.Src == blendFuncFrameData.Src && this.Dst == blendFuncFrameData.Dst;
		}
	}
}
