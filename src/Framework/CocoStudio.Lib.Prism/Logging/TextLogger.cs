using System;
using System.Globalization;
using System.IO;
using CocoStudio.Lib.Prism.Properties;

namespace CocoStudio.Lib.Prism.Logging
{
	// Token: 0x02000006 RID: 6
	public class TextLogger : ILoggerFacade, IDisposable
	{
		// Token: 0x06000004 RID: 4 RVA: 0x0000205B File Offset: 0x0000025B
		public TextLogger() : this(Console.Out)
		{
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000206C File Offset: 0x0000026C
		public TextLogger(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.writer = writer;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020A0 File Offset: 0x000002A0
		public void Log(string message, Category category, Priority priority)
		{
			string value = string.Format(CultureInfo.InvariantCulture, Resources.DefaultTextLoggerPattern, new object[]
			{
				DateTime.Now,
				category.ToString().ToUpper(CultureInfo.InvariantCulture),
				message,
				priority.ToString()
			});
			this.writer.WriteLine(value);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000210C File Offset: 0x0000030C
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.writer != null)
				{
					this.writer.Dispose();
				}
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000213F File Offset: 0x0000033F
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0400000B RID: 11
		private readonly TextWriter writer;
	}
}
