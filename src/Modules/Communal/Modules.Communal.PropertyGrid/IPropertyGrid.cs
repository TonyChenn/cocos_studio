using System;
using System.Collections.Generic;
using CocoStudio.Core;

namespace Modules.Communal.PropertyGrid
{
	public interface IPropertyGrid : IService
	{
		IReadOnlyList<object> SelectedObjects { get; set; }

		bool IsShowTitle { get; set; }

		List<IPropertyEditor> GetEditors();

		IPropertyEditor GetEditor(string propertyName);

		void ForceRefresh();
	}
}
