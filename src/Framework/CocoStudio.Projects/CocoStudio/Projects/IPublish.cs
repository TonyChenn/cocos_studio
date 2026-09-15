using System;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	public interface IPublish
	{
		void Publish(IProgressMonitor monitor, PublishInfo info);
	}
}
