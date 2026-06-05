using System;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x02000038 RID: 56
	[Extension(typeof(IFileFormat))]
	internal class SolutionFormat : XmlFileFormat
	{
		// Token: 0x0600014B RID: 331 RVA: 0x00005F44 File Offset: 0x00004144
		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			return expectedObjectType.Equals(typeof(Solution)) && FileFormat.CheckFileSuffix(file, new string[]
			{
				".ccs"
			});
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00005F80 File Offset: 0x00004180
		protected override XmlDataSerializer CreateSerializer(FilePath file)
		{
			XmlDataSerializer xmlDataSerializer = base.CreateSerializer(file);
			DataContext dataContext = xmlDataSerializer.SerializationContext.Serializer.DataContext;
			foreach (Type type in FileFormat.ResourceTypeManager.ResourceTypeList)
			{
				dataContext.IncludeType(type);
			}
			string baseFile = file.ParentDirectory.Combine(new string[]
			{
				"CocosStudio".ToLower()
			}).Combine(new string[]
			{
				"TempAppendFileName.Folder"
			});
			xmlDataSerializer.SerializationContext.BaseFile = baseFile;
			return xmlDataSerializer;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00006048 File Offset: 0x00004248
		protected override bool OnCanWriteFile(object obj)
		{
			return obj.GetType().Equals(typeof(Solution));
		}
	}
}
