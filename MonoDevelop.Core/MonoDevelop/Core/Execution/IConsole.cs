using System;
using System.IO;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x0200000E RID: 14
	public interface IConsole : IDisposable
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600005E RID: 94
		TextReader In { get; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600005F RID: 95
		TextWriter Out { get; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000060 RID: 96
		TextWriter Error { get; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000061 RID: 97
		TextWriter Log { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000062 RID: 98
		bool CloseOnDispose { get; }

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000063 RID: 99
		// (remove) Token: 0x06000064 RID: 100
		event EventHandler CancelRequested;
	}
}
