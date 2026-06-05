using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x0200001E RID: 30
	public abstract class XmlFileFormat : FileFormat
	{
		// Token: 0x060000A5 RID: 165 RVA: 0x00003C0C File Offset: 0x00001E0C
		protected override void OnWriteFile(FilePath file, object obj, IProgressMonitor monitor)
		{
			XmlDataSerializer xmlDataSerializer = this.CreateSerializer(file);
			xmlDataSerializer.Serialize(file, obj);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00003C30 File Offset: 0x00001E30
		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			XmlDataSerializer xmlDataSerializer = this.CreateSerializer(file);
			return xmlDataSerializer.Deserialize(file, expectedType);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00003C54 File Offset: 0x00001E54
		protected virtual XmlDataSerializer CreateSerializer(FilePath file)
		{
			DataContext ctx = new DataContext();
			return new XmlDataSerializer(ctx)
			{
				SerializationContext = 
				{
					DirectorySeparatorChar = '/'
				}
			};
		}
	}
}
