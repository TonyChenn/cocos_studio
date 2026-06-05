using System;

namespace MonoDevelop.Projects.Extensions
{
	/// <summary>
	/// This interface can be implemented by a ISolutionItemHandler class to provide
	/// custom rules for encoding and decoding paths
	/// </summary>
	// Token: 0x020001A6 RID: 422
	public interface IPathHandler
	{
		// Token: 0x06000FF7 RID: 4087
		string EncodePath(string path, string oldPath);

		// Token: 0x06000FF8 RID: 4088
		string DecodePath(string path);
	}
}
