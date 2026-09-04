using System;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	public class LogWriter : MarshalByRefObject, ILogWriter
	{
		private TextWriter writer;

		public LogWriter(TextWriter writer)
		{
			this.writer = writer;
		}

		[OneWay]
		public void WriteLine(string text)
		{
			writer.WriteLine(text);
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
}
