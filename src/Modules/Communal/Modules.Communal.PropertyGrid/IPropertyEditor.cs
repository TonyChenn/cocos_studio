using System;
using System.ComponentModel;
using Gtk;

namespace Modules.Communal.PropertyGrid
{
	public interface IPropertyEditor : IComparable<IPropertyEditor>
	{
		string DisplayName { get; }

		string Group { get; }

		int Order { get; }

		bool Visible { get; set; }

		bool Enable { get; set; }

		Widget EditorWidget { get; }

		PropertyItem PropertyItem { get; }

		bool SupportMultiSelect { get; }

		bool CanCaching { get; }

		void Initialize(PropertyItem propItem);

		void HandlePropertyChanged(PropertyChangedEventArgs e);
	}
}
