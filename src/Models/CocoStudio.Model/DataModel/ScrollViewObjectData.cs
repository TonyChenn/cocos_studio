using System;
using System.ComponentModel;
using CocoStudio.Model.Editor;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(ScrollViewObject))]
	[DataInclude(typeof(ScrollViewDirectionType))]
	public class ScrollViewObjectData : PanelObjectData
	{
		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		public bool IsBounceEnabled { get; set; }

		[ItemProperty]
		[JsonProperty]
		public SizeValue InnerNodeSize { get; set; }

		[ItemProperty]
		[JsonProperty]
		public ScrollViewDirectionType ScrollDirectionType { get; set; }

		protected override void OnDataInitialize(VisualObject vObject)
		{
			ScrollViewObject scrollViewObject = vObject as ScrollViewObject;
			if (scrollViewObject != null)
			{
				scrollViewObject.InnerNodeSize = this.InnerNodeSize;
			}
		}
	}
}
