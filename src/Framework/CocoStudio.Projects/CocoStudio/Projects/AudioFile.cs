using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	[DataItem(Name = "Audio")]
	public class AudioFile : ResourceFile
	{
		public AudioFile(FilePath fileName) : base(fileName)
		{
		}

		public AudioFile()
		{
		}
	}
}
