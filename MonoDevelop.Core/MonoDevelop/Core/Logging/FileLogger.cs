using System;
using System.IO;

namespace MonoDevelop.Core.Logging
{
	// Token: 0x02000056 RID: 86
	public class FileLogger : ILogger, IDisposable
	{
		// Token: 0x060002C9 RID: 713 RVA: 0x0000AF38 File Offset: 0x00009138
		public FileLogger(string filename) : this(filename, false)
		{
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000AF42 File Offset: 0x00009142
		public FileLogger(string filename, bool append)
		{
			this.writer = new StreamWriter(filename, append);
			this.name = filename;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000AF68 File Offset: 0x00009168
		public void Log(LogLevel level, string message)
		{
			string @string;
			switch (level)
			{
			case LogLevel.Fatal:
				@string = GettextCatalog.GetString("FATAL ERROR");
				goto IL_71;
			case LogLevel.Error:
				@string = GettextCatalog.GetString("ERROR");
				goto IL_71;
			case (LogLevel)3:
				break;
			case LogLevel.Warn:
				@string = GettextCatalog.GetString("WARNING");
				goto IL_71;
			default:
				if (level == LogLevel.Info)
				{
					@string = GettextCatalog.GetString("INFO");
					goto IL_71;
				}
				if (level == LogLevel.Debug)
				{
					@string = GettextCatalog.GetString("DEBUG");
					goto IL_71;
				}
				break;
			}
			@string = GettextCatalog.GetString("LOG");
			IL_71:
			this.writer.WriteLine("{0}[{1}]: {2}", @string, DateTime.Now.ToString("u"), message);
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060002CC RID: 716 RVA: 0x0000B00A File Offset: 0x0000920A
		// (set) Token: 0x060002CD RID: 717 RVA: 0x0000B012 File Offset: 0x00009212
		public EnabledLoggingLevel EnabledLevel
		{
			get
			{
				return this.enabledLevel;
			}
			set
			{
				this.enabledLevel = value;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002CE RID: 718 RVA: 0x0000B01B File Offset: 0x0000921B
		// (set) Token: 0x060002CF RID: 719 RVA: 0x0000B023 File Offset: 0x00009223
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000B02C File Offset: 0x0000922C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000B03B File Offset: 0x0000923B
		protected void Dispose(bool disposing)
		{
			if (disposing && this.writer != null)
			{
				this.writer.Dispose();
				this.writer = null;
			}
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000B05C File Offset: 0x0000925C
		~FileLogger()
		{
			this.Dispose(false);
		}

		// Token: 0x0400010C RID: 268
		private TextWriter writer;

		// Token: 0x0400010D RID: 269
		private string name;

		// Token: 0x0400010E RID: 270
		private EnabledLoggingLevel enabledLevel = EnabledLoggingLevel.UpToInfo;
	}
}
