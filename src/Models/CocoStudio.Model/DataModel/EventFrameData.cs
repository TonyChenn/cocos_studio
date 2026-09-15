using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Model.DataModel
{
	[DataItem(Name = "EventFrame")]
	[DataModelExtension(typeof(EventFrame))]
	public class EventFrameData : StringFrameData
	{
	}
}
