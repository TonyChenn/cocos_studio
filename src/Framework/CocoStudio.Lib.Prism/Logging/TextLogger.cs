using System;
using System.Globalization;
using System.IO;
using CocoStudio.Lib.Prism.Properties;

namespace CocoStudio.Lib.Prism.Logging
{
	public class TextLogger : ILoggerFacade, IDisposable
	{
		public TextLogger() : this(Console.Out)
		{
		}

		public TextLogger(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.writer = writer;
		}

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

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private readonly TextWriter writer;
	}
}
