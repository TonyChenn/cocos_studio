using System;

namespace CocoStudio.UserStatistics
{
	public class EditorInfo
	{
		public string Type { get; private set; }

		public Version Version { get; private set; }

		public EditorInfo(string type, Version version)
		{
			this.Type = type;
			this.Version = version;
		}
	}
}
