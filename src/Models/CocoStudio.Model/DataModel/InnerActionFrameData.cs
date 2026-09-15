using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataItem(Name = "InnerActionFrame")]
	[DataModelExtension(typeof(InnerActionFrame))]
	public class InnerActionFrameData : FrameData
	{
		[JsonProperty]
		[ItemProperty]
		public InnerActionType InnerActionType { get; set; }

		[ItemProperty]
		[JsonProperty]
		public string CurrentAniamtionName { get; set; }

		[ItemProperty]
		[JsonProperty]
		public int SingleFrameIndex { get; set; }

		public override bool FrameEquals(FrameData framedata)
		{
			InnerActionFrameData innerActionFrameData = framedata as InnerActionFrameData;
			bool flag = innerActionFrameData != null;
			return flag && base.FrameEquals(framedata) && this.InnerActionType == innerActionFrameData.InnerActionType && this.CurrentAniamtionName.Equals(innerActionFrameData.CurrentAniamtionName) && this.SingleFrameIndex == innerActionFrameData.SingleFrameIndex;
		}
	}
}
