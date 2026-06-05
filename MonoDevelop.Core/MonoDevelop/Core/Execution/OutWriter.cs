using System;
using System.IO;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x02000016 RID: 22
	internal class OutWriter
	{
		// Token: 0x060000AA RID: 170 RVA: 0x00004D60 File Offset: 0x00002F60
		public OutWriter(TextWriter writer)
		{
			this.writer = writer;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004D6F File Offset: 0x00002F6F
		public void WriteOut(object sender, string s)
		{
			this.writer.Write(s);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004D7D File Offset: 0x00002F7D
		public static ProcessEventHandler GetWriteHandler(TextWriter tw)
		{
			if (tw == null)
			{
				return null;
			}
			return new ProcessEventHandler(new OutWriter(tw).WriteOut);
		}

		// Token: 0x04000052 RID: 82
		private TextWriter writer;
	}
}
