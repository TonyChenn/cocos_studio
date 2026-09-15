using System;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects.Formates
{
	[Extension(typeof(IFileFormat))]
	internal class SolutionFormat : XmlFileFormat
	{
		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			return expectedObjectType.Equals(typeof(Solution)) && FileFormat.CheckFileSuffix(file, new string[]
			{
				".ccs"
			});
		}

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

		protected override bool OnCanWriteFile(object obj)
		{
			return obj.GetType().Equals(typeof(Solution));
		}
	}
}
