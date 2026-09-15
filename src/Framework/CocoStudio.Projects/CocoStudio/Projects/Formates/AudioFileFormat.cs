using System;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	[Extension(typeof(IFileFormat))]
	public class AudioFileFormat : FileFormat
	{
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is AudioFile;
		}

		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			return FileFormat.CheckFileSuffix(file, new string[]
			{
				".mp3",
				".wav"
			}) && (expectedObjectType.Equals(typeof(AudioFile)) || expectedObjectType.Equals(typeof(ResourceItem)));
		}

		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new AudioFile(file);
		}
	}
}
