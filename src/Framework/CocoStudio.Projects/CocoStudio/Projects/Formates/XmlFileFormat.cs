using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects.Formates
{
	public abstract class XmlFileFormat : FileFormat
	{
		protected override void OnWriteFile(FilePath file, object obj, IProgressMonitor monitor)
		{
			XmlDataSerializer xmlDataSerializer = this.CreateSerializer(file);
			xmlDataSerializer.Serialize(file, obj);
		}

		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			XmlDataSerializer xmlDataSerializer = this.CreateSerializer(file);
			return xmlDataSerializer.Deserialize(file, expectedType);
		}

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
