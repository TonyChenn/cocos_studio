using System;
using System.IO;

namespace MonoDevelop.Core.Execution
{
	/// <summary>
	/// This is an implementation of the IConsole interface which allows reading
	/// the output generated from a process, and writing its input.
	/// </summary>
	// Token: 0x020000E8 RID: 232
	public class LocalConsole : IConsole, IDisposable
	{
		// Token: 0x0600081C RID: 2076 RVA: 0x00020FB5 File Offset: 0x0001F1B5
		public LocalConsole()
		{
			this.cout = new InternalWriter();
			this.cerror = new InternalWriter();
			this.clog = new InternalWriter();
			this.cin = new InternalWriter();
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00020FE9 File Offset: 0x0001F1E9
		public void Dispose()
		{
			this.cout.Dispose();
			this.cerror.Dispose();
			this.clog.Dispose();
			this.cin.Dispose();
		}

		/// <summary>
		/// Flushes and closes the readers and writers
		/// </summary>
		// Token: 0x0600081E RID: 2078 RVA: 0x00021017 File Offset: 0x0001F217
		public void SetDone()
		{
			this.cout.SetDone();
			this.cerror.SetDone();
			this.clog.SetDone();
			this.cin.SetDone();
		}

		/// <summary>
		/// This writer can be used to provide the input of the console.
		/// </summary>
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x00021045 File Offset: 0x0001F245
		public TextWriter In
		{
			get
			{
				return this.cin;
			}
		}

		/// <summary>
		/// Output of the process.
		/// </summary>
		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x0002104D File Offset: 0x0001F24D
		public TextReader Out
		{
			get
			{
				return this.cout.DataReader;
			}
		}

		/// <summary>
		/// Error log of the process
		/// </summary>
		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x0002105A File Offset: 0x0001F25A
		public TextReader Error
		{
			get
			{
				return this.cerror.DataReader;
			}
		}

		/// <summary>
		/// Log of the process
		/// </summary>
		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x00021067 File Offset: 0x0001F267
		public TextReader Log
		{
			get
			{
				return this.clog.DataReader;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x00021074 File Offset: 0x0001F274
		TextReader IConsole.In
		{
			get
			{
				return this.cin.DataReader;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x00021081 File Offset: 0x0001F281
		TextWriter IConsole.Out
		{
			get
			{
				return this.cout;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x00021089 File Offset: 0x0001F289
		TextWriter IConsole.Error
		{
			get
			{
				return this.cerror;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x00021091 File Offset: 0x0001F291
		TextWriter IConsole.Log
		{
			get
			{
				return this.clog;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000827 RID: 2087 RVA: 0x00021099 File Offset: 0x0001F299
		bool IConsole.CloseOnDispose
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06000828 RID: 2088 RVA: 0x0002109C File Offset: 0x0001F29C
		// (remove) Token: 0x06000829 RID: 2089 RVA: 0x0002109E File Offset: 0x0001F29E
		public event EventHandler CancelRequested
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x04000299 RID: 665
		private InternalWriter cin;

		// Token: 0x0400029A RID: 666
		private InternalWriter cout;

		// Token: 0x0400029B RID: 667
		private InternalWriter cerror;

		// Token: 0x0400029C RID: 668
		private InternalWriter clog;
	}
}
