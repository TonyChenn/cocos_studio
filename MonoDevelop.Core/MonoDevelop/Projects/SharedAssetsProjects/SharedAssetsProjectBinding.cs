using System;
using System.Xml;

namespace MonoDevelop.Projects.SharedAssetsProjects
{
	// Token: 0x02000253 RID: 595
	public class SharedAssetsProjectBinding : IProjectBinding
	{
		// Token: 0x060015EC RID: 5612 RVA: 0x00059004 File Offset: 0x00057204
		public Project CreateProject(ProjectCreateInformation info, XmlElement projectOptions)
		{
			return new SharedAssetsProject(info, projectOptions);
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x0005900D File Offset: 0x0005720D
		public Project CreateSingleFileProject(string sourceFile)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x00059014 File Offset: 0x00057214
		public bool CanCreateSingleFileProject(string sourceFile)
		{
			return false;
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x060015EF RID: 5615 RVA: 0x00059017 File Offset: 0x00057217
		public string Name
		{
			get
			{
				return "SharedAssetsProject";
			}
		}
	}
}
