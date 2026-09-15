using System;

namespace CocoStudio.Model
{
	public interface IListViewType
	{
		ListViewDirectionType DirectionType { get; set; }

		ListViewHorizontal HorizontalType { get; set; }

		ListViewVertical VerticalType { get; set; }
	}
}
