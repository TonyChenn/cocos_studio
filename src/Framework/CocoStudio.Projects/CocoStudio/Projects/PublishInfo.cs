using System;

namespace CocoStudio.Projects
{
	public class PublishInfo
	{
		public virtual string PublishDirectory { get; set; }

		public string SourceFilePath { get; set; }

		public string DestinationFilePath { get; set; }

		public PublishType PublishType { get; set; }

		public PublishInfo()
		{
		}

		public PublishInfo(string publishDirectory, PublishType publishType = PublishType.Reference)
		{
			this.PublishDirectory = publishDirectory;
			this.PublishType = publishType;
		}
	}
}
