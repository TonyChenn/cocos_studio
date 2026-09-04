using System;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x02000027 RID: 39
	[Extension(typeof(IFileFormat))]
	public class AudioFileFormat : FileFormat
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x0000451C File Offset: 0x0000271C
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is AudioFile;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004528 File Offset: 0x00002728
		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			return FileFormat.CheckFileSuffix(file, new string[]
			{
				".mp3",
				".wav"
			}) && (expectedObjectType.Equals(typeof(AudioFile)) || expectedObjectType.Equals(typeof(ResourceItem)));
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004580 File Offset: 0x00002780
		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new AudioFile(file);
		}
	}
}
