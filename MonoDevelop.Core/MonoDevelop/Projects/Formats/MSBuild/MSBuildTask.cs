using System;
using System.Xml;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001D3 RID: 467
	public class MSBuildTask : MSBuildObject
	{
		// Token: 0x060011D6 RID: 4566 RVA: 0x00048D3D File Offset: 0x00046F3D
		public static bool IsTask(XmlElement elem)
		{
			return elem != null && elem.LocalName == "Error";
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x00048D54 File Offset: 0x00046F54
		public MSBuildTask(XmlElement elem) : base(elem)
		{
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x060011D8 RID: 4568 RVA: 0x00048D5D File Offset: 0x00046F5D
		// (set) Token: 0x060011D9 RID: 4569 RVA: 0x00048D6F File Offset: 0x00046F6F
		public string Name
		{
			get
			{
				return base.EvaluatedElement.GetAttribute("Name");
			}
			set
			{
				base.Element.SetAttribute("Name", value);
			}
		}
	}
}
