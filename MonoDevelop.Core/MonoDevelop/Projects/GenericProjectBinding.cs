using System;
using System.Xml;

namespace MonoDevelop.Projects
{
	// Token: 0x02000156 RID: 342
	public class GenericProjectBinding : IProjectBinding
	{
		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x0002E81A File Offset: 0x0002CA1A
		public virtual string Name
		{
			get
			{
				return "GenericProject";
			}
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x0002E821 File Offset: 0x0002CA21
		public Project CreateProject(ProjectCreateInformation info, XmlElement projectOptions)
		{
			return new GenericProject(info, projectOptions);
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0002E82A File Offset: 0x0002CA2A
		public Project CreateSingleFileProject(string file)
		{
			return null;
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x0002E82D File Offset: 0x0002CA2D
		public bool CanCreateSingleFileProject(string file)
		{
			return false;
		}
	}
}
