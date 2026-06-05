using System;
using System.Collections.Generic;
using System.Xml;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001D1 RID: 465
	public class MSBuildItemGroup : MSBuildObject
	{
		// Token: 0x060011CE RID: 4558 RVA: 0x000488AC File Offset: 0x00046AAC
		internal MSBuildItemGroup(MSBuildProject parent, XmlElement elem) : base(elem)
		{
			this.parent = parent;
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x000488BC File Offset: 0x00046ABC
		public MSBuildItem AddNewItem(string name, string include)
		{
			XmlElement elem = base.AddChildElement(name);
			MSBuildItem item = this.parent.GetItem(elem);
			item.Include = include;
			return item;
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x060011D0 RID: 4560 RVA: 0x00048AB8 File Offset: 0x00046CB8
		public IEnumerable<MSBuildItem> Items
		{
			get
			{
				foreach (object obj in base.Element.ChildNodes)
				{
					XmlNode node = (XmlNode)obj;
					XmlElement elem = node as XmlElement;
					if (elem != null)
					{
						yield return this.parent.GetItem(elem);
					}
				}
				yield break;
			}
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x00048AD8 File Offset: 0x00046CD8
		internal override void Evaluate(MSBuildEvaluationContext context)
		{
			foreach (MSBuildItem msbuildItem in this.Items)
			{
				msbuildItem.Evaluate(context);
			}
		}

		// Token: 0x0400051E RID: 1310
		private MSBuildProject parent;
	}
}
