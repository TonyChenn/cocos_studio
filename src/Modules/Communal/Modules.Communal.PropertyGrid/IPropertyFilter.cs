using System;
using System.Collections.Generic;
using Mono.Addins;

namespace Modules.Communal.PropertyGrid
{
	[TypeExtensionPoint]
	public interface IPropertyFilter
	{
		bool CanHandle();

		bool CanShow(IReadOnlyList<object> selectedObjs, string propertyName);
	}
}
