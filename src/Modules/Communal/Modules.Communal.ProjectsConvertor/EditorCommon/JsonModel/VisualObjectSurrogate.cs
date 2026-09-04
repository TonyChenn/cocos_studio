using System;
using System.Runtime.Serialization;
using Mono.Addins;

namespace EditorCommon.JsonModel
{
	// Token: 0x02000009 RID: 9
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class VisualObjectSurrogate : BaseEntitySurrogate
	{
		// Token: 0x0600005F RID: 95 RVA: 0x000041E0 File Offset: 0x000023E0
		protected VisualObjectSurrogate()
		{
		}
	}
}
