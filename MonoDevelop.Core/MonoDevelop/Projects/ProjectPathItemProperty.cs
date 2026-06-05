using System;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x02000101 RID: 257
	public class ProjectPathItemProperty : ItemPropertyAttribute
	{
		// Token: 0x06000959 RID: 2393 RVA: 0x00025649 File Offset: 0x00023849
		public ProjectPathItemProperty()
		{
			base.SerializationDataType = typeof(PathDataType);
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x00025661 File Offset: 0x00023861
		public ProjectPathItemProperty(string name) : base(name)
		{
			base.SerializationDataType = typeof(PathDataType);
		}
	}
}
