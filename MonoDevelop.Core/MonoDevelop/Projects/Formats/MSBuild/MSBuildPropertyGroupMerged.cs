using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001CE RID: 462
	internal class MSBuildPropertyGroupMerged : MSBuildPropertySet
	{
		// Token: 0x060011A8 RID: 4520 RVA: 0x00047D4F File Offset: 0x00045F4F
		public void Add(MSBuildPropertyGroup g)
		{
			this.groups.Add(g);
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x060011A9 RID: 4521 RVA: 0x00047D5D File Offset: 0x00045F5D
		public int GroupCount
		{
			get
			{
				return this.groups.Count;
			}
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x00047D6C File Offset: 0x00045F6C
		public MSBuildProperty GetProperty(string name)
		{
			for (int i = this.groups.Count - 1; i >= 0; i--)
			{
				MSBuildPropertyGroup msbuildPropertyGroup = this.groups[i];
				MSBuildProperty property = msbuildPropertyGroup.GetProperty(name);
				if (property != null)
				{
					return property;
				}
			}
			return null;
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x00047DAC File Offset: 0x00045FAC
		public MSBuildProperty SetPropertyValue(string name, string value, bool preserveExistingCase, bool isXml = false)
		{
			MSBuildProperty property = this.GetProperty(name);
			if (property != null)
			{
				if (!preserveExistingCase || !string.Equals(value, property.GetValue(isXml), StringComparison.OrdinalIgnoreCase))
				{
					property.SetValue(value, isXml);
				}
				return property;
			}
			return this.groups[0].SetPropertyValue(name, value, preserveExistingCase, isXml);
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x00047DFC File Offset: 0x00045FFC
		public string GetPropertyValue(string name, bool isXml = false)
		{
			MSBuildProperty property = this.GetProperty(name);
			if (property == null)
			{
				return null;
			}
			return property.GetValue(isXml);
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x00047E20 File Offset: 0x00046020
		public bool RemoveProperty(string name)
		{
			bool result = false;
			foreach (MSBuildPropertyGroup msbuildPropertyGroup in this.groups)
			{
				if (msbuildPropertyGroup.RemoveProperty(name))
				{
					this.Prune(msbuildPropertyGroup);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x00047E84 File Offset: 0x00046084
		public void RemoveAllProperties()
		{
			foreach (MSBuildPropertyGroup msbuildPropertyGroup in this.groups)
			{
				msbuildPropertyGroup.RemoveAllProperties();
				this.Prune(msbuildPropertyGroup);
			}
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x00047EE0 File Offset: 0x000460E0
		public void UnMerge(MSBuildPropertySet baseGrp, ISet<string> propertiesToExclude)
		{
			foreach (MSBuildPropertyGroup msbuildPropertyGroup in this.groups)
			{
				msbuildPropertyGroup.UnMerge(baseGrp, propertiesToExclude);
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x060011B0 RID: 4528 RVA: 0x00048144 File Offset: 0x00046344
		public IEnumerable<MSBuildProperty> Properties
		{
			get
			{
				foreach (MSBuildPropertyGroup g in this.groups)
				{
					foreach (MSBuildProperty p in g.Properties)
					{
						yield return p;
					}
				}
				yield break;
			}
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x00048161 File Offset: 0x00046361
		private void Prune(MSBuildPropertyGroup g)
		{
			if (g != this.groups[0] && !g.Properties.Any<MSBuildProperty>())
			{
				g.Parent.RemoveGroup(g);
			}
		}

		// Token: 0x04000519 RID: 1305
		private List<MSBuildPropertyGroup> groups = new List<MSBuildPropertyGroup>();
	}
}
