using System;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	public interface ICocosItem
	{
		void ReloadReferencedItem(IProgressMonitor monitor);

		bool HasReferencedItem(CocosItem item);
	}
}
