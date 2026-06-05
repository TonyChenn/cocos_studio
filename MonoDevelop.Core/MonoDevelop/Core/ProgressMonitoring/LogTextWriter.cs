using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MonoDevelop.Core.ProgressMonitoring
{
	// Token: 0x02000025 RID: 37
	public class LogTextWriter : TextWriter
	{
		// Token: 0x06000144 RID: 324 RVA: 0x000062DE File Offset: 0x000044DE
		public void ChainWriter(TextWriter writer)
		{
			if (this.chainedWriters == null)
			{
				this.chainedWriters = new List<TextWriter>();
			}
			this.chainedWriters.Add(writer);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000062FF File Offset: 0x000044FF
		public void UnchainWriter(TextWriter writer)
		{
			if (this.chainedWriters != null)
			{
				this.chainedWriters.Remove(writer);
				if (this.chainedWriters.Count == 0)
				{
					this.chainedWriters = null;
				}
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000146 RID: 326 RVA: 0x0000632A File Offset: 0x0000452A
		public override Encoding Encoding
		{
			get
			{
				return Encoding.Default;
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00006331 File Offset: 0x00004531
		public override void Close()
		{
			if (this.Closed != null)
			{
				this.Closed(this, null);
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00006348 File Offset: 0x00004548
		public override void Write(char[] buffer, int index, int count)
		{
			if (this.TextWritten != null)
			{
				this.TextWritten(new string(buffer, index, count));
			}
			if (this.chainedWriters != null)
			{
				foreach (TextWriter textWriter in this.chainedWriters)
				{
					textWriter.Write(buffer, index, count);
				}
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000063C0 File Offset: 0x000045C0
		public override void Write(char value)
		{
			if (this.TextWritten != null)
			{
				this.TextWritten(value.ToString());
			}
			if (this.chainedWriters != null)
			{
				foreach (TextWriter textWriter in this.chainedWriters)
				{
					textWriter.Write(value);
				}
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00006438 File Offset: 0x00004638
		public override void Write(string value)
		{
			if (this.TextWritten != null)
			{
				this.TextWritten(value);
			}
			if (this.chainedWriters != null)
			{
				foreach (TextWriter textWriter in this.chainedWriters)
				{
					textWriter.Write(value);
				}
			}
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600014B RID: 331 RVA: 0x000064A8 File Offset: 0x000046A8
		// (remove) Token: 0x0600014C RID: 332 RVA: 0x000064E0 File Offset: 0x000046E0
		public event LogTextEventHandler TextWritten;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x0600014D RID: 333 RVA: 0x00006518 File Offset: 0x00004718
		// (remove) Token: 0x0600014E RID: 334 RVA: 0x00006550 File Offset: 0x00004750
		public event EventHandler Closed;

		// Token: 0x04000081 RID: 129
		private List<TextWriter> chainedWriters;
	}
}
