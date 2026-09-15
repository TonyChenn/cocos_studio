using System;
using CocoStudio.Core;
using CocoStudio.Projects;
using CocoStudio.Projects.Formates;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace Modules.Communal.TexturePacker
{
	[Extension(typeof(IFileFormat))]
	internal class PlistInfoCocosFileFormat : CocosFileFormat
	{
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is PlistInfoCocosFile;
		}

		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			return expectedObjectType.Equals(typeof(CocosFile)) && FileFormat.CheckFileSuffix(file, new string[]
			{
				".csi"
			});
		}

		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			if (!base.CanReadFile(file, expectedType))
			{
				monitor.ReportError("Unsupport file format.", null);
			}
			XmlDataSerializer xmlDataSerializer = this.CreateSerializer(file);
			return xmlDataSerializer.Deserialize(file, typeof(PlistInfoCocosFile));
		}

		protected override XmlDataSerializer CreateSerializer(FilePath file)
		{
			XmlDataSerializer xmlDataSerializer = base.CreateSerializer(file);
			if (Services.ProjectOperations.CurrentSelectedSolution != null)
			{
				xmlDataSerializer.SerializationContext.BaseFile = Services.ProjectOperations.CurrentSelectedSolution.ItemDirectory;
			}
			return xmlDataSerializer;
		}
	}
}
