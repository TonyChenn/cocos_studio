using System;
using System.Collections.Generic;
using Mono.Addins;

namespace Modules.Communal.PropertyGrid
{
	[TypeExtensionPoint]
	public interface IEditorController
	{
		IEnumerable<string> CorrespondProperties { get; }

		bool CanHandle();

		void RefreshEditor(IReadOnlyList<object> selectedObjs, string propertyName);
	}
}
