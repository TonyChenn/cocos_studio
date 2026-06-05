using System;

namespace MonoDevelop.Projects.Extensions
{
	/// <summary>
	/// This interface can be implemented by a ISolutionItemHandler class to provide
	/// custom resource id generation rules
	/// </summary>
	// Token: 0x020001A4 RID: 420
	public interface IResourceHandler
	{
		// Token: 0x06000FF3 RID: 4083
		string GetDefaultResourceId(ProjectFile file);
	}
}
