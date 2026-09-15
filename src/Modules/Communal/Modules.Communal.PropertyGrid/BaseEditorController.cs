using System;
using System.Collections.Generic;

namespace Modules.Communal.PropertyGrid
{
	public abstract class BaseEditorController : IEditorController
	{
		public IEnumerable<string> CorrespondProperties
		{
			get
			{
				return this._correspondProperties;
			}
		}

		public abstract void RefreshEditor(IReadOnlyList<object> selectedObjs, string propertyName);

		public abstract bool CanHandle();

		protected void AddCorrespondProperty(string propertyName)
		{
			this._correspondProperties.Add(propertyName);
		}

		private List<string> _correspondProperties = new List<string>();
	}
}
