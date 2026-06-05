using System;
using System.IO;

namespace MonoDevelop.Core.ProgressMonitoring
{
	// Token: 0x02000020 RID: 32
	public class ConsoleProgressMonitor : NullProgressMonitor
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00005ECC File Offset: 0x000040CC
		private string TimeStamp
		{
			get
			{
				if (!this.EnableTimeStamp)
				{
					return string.Empty;
				}
				return string.Format("[{0}] ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.f"));
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005F04 File Offset: 0x00004104
		public ConsoleProgressMonitor() : this(Console.Out, true)
		{
			try
			{
				this.columns = Console.WindowWidth;
			}
			catch (IOException)
			{
				this.columns = 0;
			}
			this.wrap = (this.columns > 0);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005F54 File Offset: 0x00004154
		public ConsoleProgressMonitor(TextWriter writer, bool leaveOpen)
		{
			this.writer = writer;
			this.leaveOpen = leaveOpen;
			this.logger = new LogTextWriter();
			this.logger.TextWritten += this.WriteLog;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005FAC File Offset: 0x000041AC
		public ConsoleProgressMonitor(TextWriter writer) : this(writer, false)
		{
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005FB6 File Offset: 0x000041B6
		public override void Dispose()
		{
			this.logger.TextWritten -= this.WriteLog;
			this.logger.Dispose();
			if (!this.leaveOpen)
			{
				this.writer.Dispose();
			}
			base.Dispose();
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00005FF3 File Offset: 0x000041F3
		// (set) Token: 0x06000122 RID: 290 RVA: 0x00005FFB File Offset: 0x000041FB
		public bool EnableTimeStamp { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00006004 File Offset: 0x00004204
		// (set) Token: 0x06000124 RID: 292 RVA: 0x0000600C File Offset: 0x0000420C
		public bool WrapText
		{
			get
			{
				return this.wrap;
			}
			set
			{
				this.wrap = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00006015 File Offset: 0x00004215
		// (set) Token: 0x06000126 RID: 294 RVA: 0x0000601D File Offset: 0x0000421D
		public bool IgnoreLogMessages
		{
			get
			{
				return this.ignoreLogMessages;
			}
			set
			{
				this.ignoreLogMessages = value;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00006026 File Offset: 0x00004226
		// (set) Token: 0x06000128 RID: 296 RVA: 0x0000602E File Offset: 0x0000422E
		public int WrapColumns
		{
			get
			{
				return this.columns;
			}
			set
			{
				this.columns = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00006037 File Offset: 0x00004237
		// (set) Token: 0x0600012A RID: 298 RVA: 0x0000603F File Offset: 0x0000423F
		public bool IndentTasks
		{
			get
			{
				return this.indent;
			}
			set
			{
				this.indent = value;
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00006048 File Offset: 0x00004248
		public override void BeginTask(string name, int totalWork)
		{
			if (!this.ignoreLogMessages)
			{
				this.WriteText(name);
				this.Indent();
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000605F File Offset: 0x0000425F
		public override void BeginStepTask(string name, int totalWork, int stepSize)
		{
			this.BeginTask(name, totalWork);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00006069 File Offset: 0x00004269
		public override void EndTask()
		{
			if (!this.ignoreLogMessages)
			{
				this.Unindent();
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006079 File Offset: 0x00004279
		private void WriteLog(string text)
		{
			if (!this.ignoreLogMessages)
			{
				this.WriteText(text);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600012F RID: 303 RVA: 0x0000608A File Offset: 0x0000428A
		public override TextWriter Log
		{
			get
			{
				return this.logger;
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006092 File Offset: 0x00004292
		public override void ReportSuccess(string message)
		{
			this.WriteText(message + "\n");
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000060A5 File Offset: 0x000042A5
		public override void ReportWarning(string message)
		{
			this.WriteText("WARNING: " + message + "\n");
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000060C0 File Offset: 0x000042C0
		public override void ReportError(string message, Exception ex)
		{
			if (message == null && ex != null)
			{
				message = ex.Message;
			}
			else if (message != null && ex != null)
			{
				if (!message.EndsWith("."))
				{
					message += ".";
				}
				message = message + " " + ex.Message;
			}
			this.WriteText("ERROR: " + message + "\n");
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006126 File Offset: 0x00004326
		private void WriteText(string text)
		{
			if (this.indent)
			{
				this.WriteText(text, this.ilevel);
				return;
			}
			this.WriteText(text, 0);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00006148 File Offset: 0x00004348
		private void WriteText(string text, int leftMargin)
		{
			if (text == null)
			{
				return;
			}
			string timeStamp = this.TimeStamp;
			int num = this.wrap ? (this.columns - timeStamp.Length) : int.MaxValue;
			int i = 0;
			while (i < text.Length)
			{
				if (this.col == -1)
				{
					this.writer.Write(timeStamp + new string(' ', leftMargin));
					this.col = leftMargin;
				}
				int num2 = -1;
				int num3 = i;
				bool flag = false;
				while (this.col < num && i < text.Length)
				{
					char c = text[i];
					if (c == '\r')
					{
						i++;
					}
					else
					{
						if (c == '\n')
						{
							flag = true;
							break;
						}
						if (char.IsWhiteSpace(c))
						{
							num2 = i;
						}
						this.col++;
						i++;
					}
				}
				if (num2 == -1 || this.col < num)
				{
					num2 = i;
				}
				else if (this.col >= num)
				{
					i = num2 + 1;
				}
				this.writer.Write(text.Substring(num3, num2 - num3));
				if (flag || this.col >= num)
				{
					this.col = -1;
					this.writer.WriteLine();
					if (flag)
					{
						i++;
					}
				}
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000626E File Offset: 0x0000446E
		private void Indent()
		{
			this.ilevel += this.isize;
			if (this.col != -1)
			{
				this.writer.WriteLine();
				this.col = -1;
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000629E File Offset: 0x0000449E
		private void Unindent()
		{
			this.ilevel -= this.isize;
			if (this.ilevel < 0)
			{
				this.ilevel = 0;
			}
			if (this.col != -1)
			{
				this.writer.WriteLine();
				this.col = -1;
			}
		}

		// Token: 0x04000076 RID: 118
		private int columns;

		// Token: 0x04000077 RID: 119
		private bool leaveOpen;

		// Token: 0x04000078 RID: 120
		private bool indent = true;

		// Token: 0x04000079 RID: 121
		private bool wrap;

		// Token: 0x0400007A RID: 122
		private int ilevel;

		// Token: 0x0400007B RID: 123
		private int isize = 3;

		// Token: 0x0400007C RID: 124
		private int col = -1;

		// Token: 0x0400007D RID: 125
		private LogTextWriter logger;

		// Token: 0x0400007E RID: 126
		private bool ignoreLogMessages;

		// Token: 0x0400007F RID: 127
		private TextWriter writer;
	}
}
