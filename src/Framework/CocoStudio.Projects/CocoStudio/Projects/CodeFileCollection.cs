using System;

namespace CocoStudio.Projects
{
	public class CodeFileCollection : ItemCollection<CodeFile>
	{
		private CodeFileCollection()
		{
		}

		public CodeFileCollection(CocosItem parentItem)
		{
			this.parentItem = parentItem;
		}

		protected override void OnAdd(CodeFile item)
		{
			item.Parent = this.parentItem;
		}

		protected override void OnRemove(CodeFile item)
		{
			item.Parent = null;
		}

		private CocosItem parentItem;
	}
}
