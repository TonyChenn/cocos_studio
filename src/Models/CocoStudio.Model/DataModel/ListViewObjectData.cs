using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataInclude(typeof(ListViewVertical))]
	[DataModelExtension(typeof(ListViewObject))]
	[DataInclude(typeof(ListViewDirectionType))]
	[DataInclude(typeof(ListViewHorizontal))]
	public class ListViewObjectData : ScrollViewObjectData
	{
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(0)]
		[ItemProperty(DefaultValue = 0)]
		public int ItemMargin { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = ListViewDirectionType.Horizontal)]
		[DefaultValue(ListViewDirectionType.Horizontal)]
		public ListViewDirectionType DirectionType { get; set; }

		[DefaultValue(ListViewHorizontal.Align_Left)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = ListViewHorizontal.Align_Left)]
		public ListViewHorizontal HorizontalType { get; set; }

		[DefaultValue(ListViewVertical.Align_Top)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = ListViewVertical.Align_Top)]
		public ListViewVertical VerticalType
		{
			get
			{
				return this._vertical;
			}
			set
			{
				if (value < ListViewVertical.Align_Top)
				{
					this._vertical = ListViewVertical.Align_Top;
				}
				else
				{
					this._vertical = value;
				}
			}
		}

		public ListViewObjectData()
		{
			this.DirectionType = ListViewDirectionType.Horizontal;
		}

		private ListViewVertical _vertical = ListViewVertical.Align_Top;
	}
}
