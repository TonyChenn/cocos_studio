using System;
using Mono.Addins;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x0200018F RID: 399
	[ExtensionNode(Description = "A project binding. The specified class must implement MonoDevelop.Projects.IProjectBinding.")]
	internal class ProjectBindingCodon : TypeExtensionNode
	{
		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000F76 RID: 3958 RVA: 0x0003A173 File Offset: 0x00038373
		public IProjectBinding ProjectBinding
		{
			get
			{
				return (IProjectBinding)base.GetInstance();
			}
		}
	}
}
