using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x02000034 RID: 52
	[Extension(typeof(IFileFormat))]
	public class CocosFileFormat : XmlFileFormat
	{
		// Token: 0x06000130 RID: 304 RVA: 0x00005A56 File Offset: 0x00003C56
		static CocosFileFormat()
		{
			CocosFileFormat.CollectFileContentType();
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00005A60 File Offset: 0x00003C60
		private static void CollectFileContentType()
		{
			CocosFileFormat.fileContentList = new HashSet<Type>();
			ExtensionNodeList extensionNodes = AddinManager.GetExtensionNodes(typeof(ICocosFileContent));
			foreach (object obj in extensionNodes)
			{
				TypeExtensionNode typeExtensionNode = (TypeExtensionNode)obj;
				CocosFileFormat.fileContentList.Add(typeExtensionNode.Type);
			}
			AddinManager.AddExtensionNodeHandler(typeof(ICocosFileContent), new ExtensionNodeEventHandler(CocosFileFormat.OnFileContentExtensionChange));
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00005AF4 File Offset: 0x00003CF4
		private static void OnFileContentExtensionChange(object sender, ExtensionNodeEventArgs args)
		{
			TypeExtensionNode typeExtensionNode = args.ExtensionNode as TypeExtensionNode;
			if (!CocosFileFormat.fileContentList.Contains(typeExtensionNode.Type))
			{
				CocosFileFormat.fileContentList.Add(typeExtensionNode.Type);
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00005B30 File Offset: 0x00003D30
		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			if (!expectedObjectType.Equals(typeof(CocosFile)))
			{
				return false;
			}
			if (!FileFormat.CheckFileSuffix(file, new string[]
			{
				".csd"
			}))
			{
				return false;
			}
			XElement xelement = XElement.Load(file);
			string localName = xelement.Name.LocalName;
			return !(localName != typeof(CocosFile).Name) || !(localName != typeof(GameFile).Name);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00005BB4 File Offset: 0x00003DB4
		protected override bool OnCanWriteFile(object obj)
		{
			return obj.GetType().Equals(typeof(CocosFile)) || obj.GetType().Equals(typeof(GameFile));
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005BE8 File Offset: 0x00003DE8
		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			string @namespace = typeof(CocosFile).Namespace;
			XElement xelement = XElement.Load(file);
			string localName = xelement.Name.LocalName;
			string typeName = @namespace + "." + localName;
			Type type = Type.GetType(typeName);
			return base.OnReadFile(file, type, monitor);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00005C3C File Offset: 0x00003E3C
		protected override XmlDataSerializer CreateSerializer(FilePath file)
		{
			if (CocosFileFormat.dataSerializer != null)
			{
				return CocosFileFormat.dataSerializer;
			}
			XmlDataSerializer xmlDataSerializer = base.CreateSerializer(file);
			foreach (Type type in CocosFileFormat.fileContentList)
			{
				xmlDataSerializer.SerializationContext.Serializer.DataContext.IncludeType(type);
			}
			foreach (Type type2 in ProjectsService.Instance.DataModelManager.GetDataModelCollection())
			{
				xmlDataSerializer.SerializationContext.Serializer.DataContext.IncludeType(type2);
			}
			CocosFileFormat.dataSerializer = xmlDataSerializer;
			CocosFileFormat.fileContentList = null;
			return CocosFileFormat.dataSerializer;
		}

		// Token: 0x0400004F RID: 79
		private static HashSet<Type> fileContentList;

		// Token: 0x04000050 RID: 80
		private static XmlDataSerializer dataSerializer;
	}
}
