using System;
using Mono.Addins;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x0200021A RID: 538
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class ProjectModelDataItemAttribute : CustomExtensionAttribute, IDataItemAttribute
	{
		// Token: 0x06001435 RID: 5173 RVA: 0x00053B56 File Offset: 0x00051D56
		public ProjectModelDataItemAttribute()
		{
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x00053B5E File Offset: 0x00051D5E
		public ProjectModelDataItemAttribute([NodeAttribute("name")] string name)
		{
			this.Name = name;
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06001437 RID: 5175 RVA: 0x00053B6D File Offset: 0x00051D6D
		// (set) Token: 0x06001438 RID: 5176 RVA: 0x00053B75 File Offset: 0x00051D75
		[NodeAttribute("name")]
		public string Name { get; set; }

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001439 RID: 5177 RVA: 0x00053B7E File Offset: 0x00051D7E
		// (set) Token: 0x0600143A RID: 5178 RVA: 0x00053B86 File Offset: 0x00051D86
		public Type FallbackType { get; set; }
	}
}
