using System;

namespace MonoDevelop.Projects
{
	/// <summary>
	/// This is the base class for project parameter classes.
	/// </summary>
	// Token: 0x02000183 RID: 387
	public class DotNetProjectParameters : ProjectParameters
	{
		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000F35 RID: 3893 RVA: 0x000391DC File Offset: 0x000373DC
		public virtual bool DefaultNamespaceIsImplicit
		{
			get
			{
				return false;
			}
		}
	}
}
