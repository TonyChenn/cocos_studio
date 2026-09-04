using System;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	// Token: 0x0200005D RID: 93
	public interface IPublish
	{
		// Token: 0x060002A3 RID: 675
		void Publish(IProgressMonitor monitor, PublishInfo info);
	}
}
