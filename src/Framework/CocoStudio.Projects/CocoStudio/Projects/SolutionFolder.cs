using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x02000085 RID: 133
	[DataInclude(typeof(ResourceGroup))]
	public class SolutionFolder : SolutionItem, IFoldeItem
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x0000DBD5 File Offset: 0x0000BDD5
		// (set) Token: 0x0600042E RID: 1070 RVA: 0x0000DBF4 File Offset: 0x0000BDF4
		[ExpandedCollection]
		[ItemProperty("Group")]
		public SolutionItemCollection Items
		{
			get
			{
				if (this.items == null)
				{
					this.items = new SolutionItemCollection(this);
				}
				return this.items;
			}
			private set
			{
				this.items = value;
				foreach (SolutionEntityItem solutionEntityItem in this.items)
				{
					solutionEntityItem.ParentFolder = this;
				}
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x0000DC48 File Offset: 0x0000BE48
		// (set) Token: 0x06000430 RID: 1072 RVA: 0x0000DC50 File Offset: 0x0000BE50
		public override string Name { get; set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x0000DC59 File Offset: 0x0000BE59
		public FilePath BaseDirectory
		{
			get
			{
				return base.ParentSolution.BaseDirectory;
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000DC70 File Offset: 0x0000BE70
		internal CocosItem GetProjectContainingFile(FilePath fileName)
		{
			foreach (SolutionEntityItem solutionEntityItem in this.Items)
			{
			}
			return null;
		}

		// Token: 0x0400011F RID: 287
		private SolutionItemCollection items;
	}
}
