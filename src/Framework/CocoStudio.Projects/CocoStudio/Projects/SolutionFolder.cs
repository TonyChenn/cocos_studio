using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	[DataInclude(typeof(ResourceGroup))]
	public class SolutionFolder : SolutionItem, IFoldeItem
	{
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

		public override string Name { get; set; }

		public FilePath BaseDirectory
		{
			get
			{
				return base.ParentSolution.BaseDirectory;
			}
		}

		internal CocosItem GetProjectContainingFile(FilePath fileName)
		{
			foreach (SolutionEntityItem solutionEntityItem in this.Items)
			{
			}
			return null;
		}

		private SolutionItemCollection items;
	}
}
