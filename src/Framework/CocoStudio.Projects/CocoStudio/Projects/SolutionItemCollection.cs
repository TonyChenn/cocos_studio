using System;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	[DataItem("ResourceGroup")]
	public class SolutionItemCollection : ItemCollection<SolutionEntityItem>
	{
		public SolutionItemCollection()
		{
		}

		public SolutionItemCollection(SolutionFolder parentSolutionFolder)
		{
			this.parentSolutionFolder = parentSolutionFolder;
		}

		protected override void OnAdd(SolutionEntityItem item)
		{
			if (this.parentSolutionFolder != null)
			{
				item.ParentFolder = this.parentSolutionFolder;
				item.ParentSolution = this.parentSolutionFolder.ParentSolution;
			}
		}

		protected override void OnRemove(SolutionEntityItem item)
		{
			item.ParentFolder = null;
			item.ParentSolution = null;
		}

		private SolutionFolder parentSolutionFolder;
	}
}
