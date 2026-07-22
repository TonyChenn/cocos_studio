using System;
using System.Collections.Generic;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x0200000E RID: 14
	public abstract class BaseEditorController : IEditorController
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002C80 File Offset: 0x00000E80
		public IEnumerable<string> CorrespondProperties
		{
			get
			{
				return this._correspondProperties;
			}
		}

		// Token: 0x06000060 RID: 96
		public abstract void RefreshEditor(IReadOnlyList<object> selectedObjs, string propertyName);

		// Token: 0x06000061 RID: 97
		public abstract bool CanHandle();

		// Token: 0x06000062 RID: 98 RVA: 0x00002C98 File Offset: 0x00000E98
		protected void AddCorrespondProperty(string propertyName)
		{
			this._correspondProperties.Add(propertyName);
		}

		// Token: 0x04000014 RID: 20
		private List<string> _correspondProperties = new List<string>();
	}
}
