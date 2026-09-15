using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects.Formates
{
	[Extension(typeof(IFileFormat))]
	public class UserDataFormat : XmlFileFormat
	{
		static UserDataFormat()
		{
			UserDataFormat.CollectDataModelType();
			UserDataFormat.supportedExtensions = new HashSet<string>();
			UserDataFormat.supportedExtensions.Add(".udf".ToLower());
			UserDataFormat.supportedExtensions.Add(".cfg".ToLower());
			UserDataFormat.supportedRootNode = new HashSet<string>();
			UserDataFormat.supportedRootNode.Add("UserData");
			UserDataFormat.supportedRootNode.Add("SolutionConfig");
			UserDataFormat.supportedType = new HashSet<Type>();
			UserDataFormat.supportedType.Add(typeof(UserData));
			UserDataFormat.supportedType.Add(typeof(SolutionConfig));
		}

		private static void CollectDataModelType()
		{
			UserDataFormat.userDataContentList = new HashSet<Type>();
			ExtensionNodeList extensionNodes = AddinManager.GetExtensionNodes(typeof(IUserData));
			foreach (object obj in extensionNodes)
			{
				TypeExtensionNode typeExtensionNode = (TypeExtensionNode)obj;
				UserDataFormat.userDataContentList.Add(typeExtensionNode.Type);
			}
			AddinManager.AddExtensionNodeHandler(typeof(IUserData), new ExtensionNodeEventHandler(UserDataFormat.OnUserDataModelExtensionChange));
		}

		private static void OnUserDataModelExtensionChange(object sender, ExtensionNodeEventArgs args)
		{
			TypeExtensionNode typeExtensionNode = args.ExtensionNode as TypeExtensionNode;
			if (!UserDataFormat.userDataContentList.Contains(typeExtensionNode.Type))
			{
				UserDataFormat.userDataContentList.Add(typeExtensionNode.Type);
			}
		}

		internal void InitCurrentSolution(Solution sln)
		{
			this.currentSolution = sln;
		}

		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			string item = Path.GetExtension(file).ToLower();
			if (!UserDataFormat.supportedExtensions.Contains(item))
			{
				return false;
			}
			if (!File.Exists(file))
			{
				return false;
			}
			XElement xelement = XElement.Load(file);
			string localName = xelement.Name.LocalName;
			return UserDataFormat.supportedRootNode.Contains(localName);
		}

		protected override bool OnCanWriteFile(object obj)
		{
			return UserDataFormat.supportedType.Contains(obj.GetType());
		}

		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			string @namespace = typeof(UserData).Namespace;
			XElement xelement = XElement.Load(file);
			string localName = xelement.Name.LocalName;
			string typeName = @namespace + "." + localName;
			Type type = Type.GetType(typeName);
			return base.OnReadFile(file, type, monitor);
		}

		protected override void OnWriteFile(FilePath file, object obj, IProgressMonitor monitor)
		{
			base.OnWriteFile(file, obj, monitor);
		}

		protected override XmlDataSerializer CreateSerializer(FilePath file)
		{
			Solution solution = ProjectsService.Instance.CurrentSolution;
			if (solution == null)
			{
				solution = this.currentSolution;
			}
			if (solution == null)
			{
				return null;
			}
			if (UserDataFormat.dataSerializer != null)
			{
				UserDataFormat.dataSerializer.SerializationContext.BaseFile = solution.ItemDirectory;
				return UserDataFormat.dataSerializer;
			}
			XmlDataSerializer xmlDataSerializer = base.CreateSerializer(file);
			xmlDataSerializer.SerializationContext.BaseFile = solution.ItemDirectory;
			foreach (Type type in UserDataFormat.userDataContentList)
			{
				xmlDataSerializer.SerializationContext.Serializer.DataContext.IncludeType(type);
			}
			UserDataFormat.dataSerializer = xmlDataSerializer;
			UserDataFormat.userDataContentList = null;
			return UserDataFormat.dataSerializer;
		}

		private static HashSet<Type> userDataContentList;

		private static XmlDataSerializer dataSerializer;

		private static HashSet<string> supportedExtensions;

		private static HashSet<string> supportedRootNode;

		private static HashSet<Type> supportedType;

		private Solution currentSolution;
	}
}
