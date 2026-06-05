using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x02000181 RID: 385
	public class CyclicDependencyException : Exception
	{
		// Token: 0x06000F2C RID: 3884 RVA: 0x00038E58 File Offset: 0x00037058
		public CyclicDependencyException() : base(GettextCatalog.GetString("A cyclic build dependency has been detected."))
		{
		}
	}
}
