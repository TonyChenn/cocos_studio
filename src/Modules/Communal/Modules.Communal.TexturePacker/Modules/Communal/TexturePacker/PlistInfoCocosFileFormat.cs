using System;
using CocoStudio.Core;
using CocoStudio.Projects;
using CocoStudio.Projects.Formates;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace Modules.Communal.TexturePacker
{
	// Token: 0x0200000E RID: 14
	[Extension(typeof(IFileFormat))]
	internal class PlistInfoCocosFileFormat : CocosFileFormat
	{
		// Token: 0x0600009E RID: 158 RVA: 0x00004ADB File Offset: 0x00002CDB
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is PlistInfoCocosFile;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004AE8 File Offset: 0x00002CE8
		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			return expectedObjectType.Equals(typeof(CocosFile)) && FileFormat.CheckFileSuffix(file, new string[]
			{
				".csi"
			});
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004B24 File Offset: 0x00002D24
		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			if (!base.CanReadFile(file, expectedType))
			{
				monitor.ReportError("Unsupport file format.", null);
			}
			XmlDataSerializer xmlDataSerializer = this.CreateSerializer(file);
			return xmlDataSerializer.Deserialize(file, typeof(PlistInfoCocosFile));
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004B68 File Offset: 0x00002D68
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
