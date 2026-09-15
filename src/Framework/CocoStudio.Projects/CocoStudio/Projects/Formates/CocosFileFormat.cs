using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects.Formates
{
	[Extension(typeof(IFileFormat))]
	public class CocosFileFormat : XmlFileFormat
	{
		static CocosFileFormat()
		{
			CocosFileFormat.CollectFileContentType();
		}

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

		private static void OnFileContentExtensionChange(object sender, ExtensionNodeEventArgs args)
		{
			TypeExtensionNode typeExtensionNode = args.ExtensionNode as TypeExtensionNode;
			if (!CocosFileFormat.fileContentList.Contains(typeExtensionNode.Type))
			{
				CocosFileFormat.fileContentList.Add(typeExtensionNode.Type);
			}
		}

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

		protected override bool OnCanWriteFile(object obj)
		{
			return obj.GetType().Equals(typeof(CocosFile)) || obj.GetType().Equals(typeof(GameFile));
		}

		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			string @namespace = typeof(CocosFile).Namespace;
			XElement xelement = XElement.Load(file);
			string localName = xelement.Name.LocalName;
			string typeName = @namespace + "." + localName;
			Type type = Type.GetType(typeName);
			return base.OnReadFile(file, type, monitor);
		}

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

		private static HashSet<Type> fileContentList;

		private static XmlDataSerializer dataSerializer;
	}
}
