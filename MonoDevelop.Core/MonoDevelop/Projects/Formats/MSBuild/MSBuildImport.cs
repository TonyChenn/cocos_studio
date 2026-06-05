using System;
using System.Xml;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001CB RID: 459
	public class MSBuildImport : MSBuildObject
	{
		// Token: 0x06001195 RID: 4501 RVA: 0x00047C1C File Offset: 0x00045E1C
		public MSBuildImport(XmlElement elem) : base(elem)
		{
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06001196 RID: 4502 RVA: 0x00047C25 File Offset: 0x00045E25
		// (set) Token: 0x06001197 RID: 4503 RVA: 0x00047C37 File Offset: 0x00045E37
		public string Project
		{
			get
			{
				return base.EvaluatedElement.GetAttribute("Project");
			}
			set
			{
				base.Element.SetAttribute("Project", value);
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06001198 RID: 4504 RVA: 0x00047C4A File Offset: 0x00045E4A
		// (set) Token: 0x06001199 RID: 4505 RVA: 0x00047C5C File Offset: 0x00045E5C
		public new string Condition
		{
			get
			{
				return base.EvaluatedElement.GetAttribute("Condition");
			}
			set
			{
				base.Element.SetAttribute("Condition", value);
			}
		}
	}
}
