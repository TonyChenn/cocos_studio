using System;
using System.IO;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x0200002D RID: 45
	public sealed class ExternalConsole : IConsole, IDisposable
	{
		// Token: 0x06000171 RID: 369 RVA: 0x000068F1 File Offset: 0x00004AF1
		internal ExternalConsole(bool closeOnDispose)
		{
			this.closeOnDispose = closeOnDispose;
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00006900 File Offset: 0x00004B00
		public TextReader In
		{
			get
			{
				return Console.In;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00006907 File Offset: 0x00004B07
		public TextWriter Out
		{
			get
			{
				return Console.Out;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000174 RID: 372 RVA: 0x0000690E File Offset: 0x00004B0E
		public TextWriter Error
		{
			get
			{
				return Console.Error;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00006915 File Offset: 0x00004B15
		public bool CloseOnDispose
		{
			get
			{
				return this.closeOnDispose;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000176 RID: 374 RVA: 0x0000691D File Offset: 0x00004B1D
		public TextWriter Log
		{
			get
			{
				return this.Out;
			}
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00006925 File Offset: 0x00004B25
		public void Dispose()
		{
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000178 RID: 376 RVA: 0x00006927 File Offset: 0x00004B27
		// (remove) Token: 0x06000179 RID: 377 RVA: 0x00006929 File Offset: 0x00004B29
		public event EventHandler CancelRequested
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x04000090 RID: 144
		private bool closeOnDispose;
	}
}
