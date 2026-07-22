using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000035 RID: 53
	[DataItem(Name = "EventFrame")]
	[DataModelExtension(typeof(EventFrame))]
	public class EventFrameData : StringFrameData
	{
	}
}
