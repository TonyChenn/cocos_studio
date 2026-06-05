using System;
using System.Xml;

namespace MonoDevelop.Projects
{
	// Token: 0x0200023F RID: 575
	public class PortableDotNetProjectBinding : IProjectBinding
	{
		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x0600153D RID: 5437 RVA: 0x00056D96 File Offset: 0x00054F96
		public string Name
		{
			get
			{
				return "PortableDotNet";
			}
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x00056DA0 File Offset: 0x00054FA0
		public Project CreateProject(ProjectCreateInformation info, XmlElement projectOptions)
		{
			string attribute = projectOptions.GetAttribute("language");
			return new PortableDotNetProject(attribute, info, projectOptions);
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x00056DC1 File Offset: 0x00054FC1
		public Project CreateSingleFileProject(string sourceFile)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x00056DC8 File Offset: 0x00054FC8
		public bool CanCreateSingleFileProject(string sourceFile)
		{
			return false;
		}
	}
}
