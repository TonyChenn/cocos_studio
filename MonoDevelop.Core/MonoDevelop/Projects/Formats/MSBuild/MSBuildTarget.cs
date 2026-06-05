using System;
using System.Collections.Generic;
using System.Xml;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001D2 RID: 466
	public class MSBuildTarget : MSBuildObject
	{
		// Token: 0x060011D2 RID: 4562 RVA: 0x00048B28 File Offset: 0x00046D28
		public MSBuildTarget(XmlElement elem) : base(elem)
		{
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x060011D3 RID: 4563 RVA: 0x00048B31 File Offset: 0x00046D31
		// (set) Token: 0x060011D4 RID: 4564 RVA: 0x00048B43 File Offset: 0x00046D43
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

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x060011D5 RID: 4565 RVA: 0x00048D20 File Offset: 0x00046F20
		public IEnumerable<MSBuildTask> Tasks
		{
			get
			{
				foreach (object obj in base.Element.ChildNodes)
				{
					XmlNode node = (XmlNode)obj;
					XmlElement elem = node as XmlElement;
					if (MSBuildTask.IsTask(elem))
					{
						yield return new MSBuildTask(elem);
					}
				}
				yield break;
			}
		}
	}
}
