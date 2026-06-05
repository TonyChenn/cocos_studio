using System;
using System.IO;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x0200000F RID: 15
	internal class ProcessHostConsole : IConsole, IDisposable
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000065 RID: 101 RVA: 0x00003DC9 File Offset: 0x00001FC9
		// (remove) Token: 0x06000066 RID: 102 RVA: 0x00003DCB File Offset: 0x00001FCB
		event EventHandler IConsole.CancelRequested
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00003DCD File Offset: 0x00001FCD
		public TextReader In
		{
			get
			{
				return Console.In;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00003DD4 File Offset: 0x00001FD4
		public TextWriter Out
		{
			get
			{
				return Console.Out;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00003DDB File Offset: 0x00001FDB
		public TextWriter Error
		{
			get
			{
				return Console.Error;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00003DE2 File Offset: 0x00001FE2
		public TextWriter Log
		{
			get
			{
				return this.Out;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00003DEA File Offset: 0x00001FEA
		public bool CloseOnDispose
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003DED File Offset: 0x00001FED
		public void Dispose()
		{
		}
	}
}
