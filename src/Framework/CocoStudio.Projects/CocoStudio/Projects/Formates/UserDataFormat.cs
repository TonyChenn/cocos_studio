using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x0200001F RID: 31
	[Extension(typeof(IFileFormat))]
	public class UserDataFormat : XmlFileFormat
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x00003C84 File Offset: 0x00001E84
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

		// Token: 0x060000AA RID: 170 RVA: 0x00003D28 File Offset: 0x00001F28
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

		// Token: 0x060000AB RID: 171 RVA: 0x00003DBC File Offset: 0x00001FBC
		private static void OnUserDataModelExtensionChange(object sender, ExtensionNodeEventArgs args)
		{
			TypeExtensionNode typeExtensionNode = args.ExtensionNode as TypeExtensionNode;
			if (!UserDataFormat.userDataContentList.Contains(typeExtensionNode.Type))
			{
				UserDataFormat.userDataContentList.Add(typeExtensionNode.Type);
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003DF8 File Offset: 0x00001FF8
		internal void InitCurrentSolution(Solution sln)
		{
			this.currentSolution = sln;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003E04 File Offset: 0x00002004
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

		// Token: 0x060000AE RID: 174 RVA: 0x00003E68 File Offset: 0x00002068
		protected override bool OnCanWriteFile(object obj)
		{
			return UserDataFormat.supportedType.Contains(obj.GetType());
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003E80 File Offset: 0x00002080
		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			string @namespace = typeof(UserData).Namespace;
			XElement xelement = XElement.Load(file);
			string localName = xelement.Name.LocalName;
			string typeName = @namespace + "." + localName;
			Type type = Type.GetType(typeName);
			return base.OnReadFile(file, type, monitor);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003ED4 File Offset: 0x000020D4
		protected override void OnWriteFile(FilePath file, object obj, IProgressMonitor monitor)
		{
			base.OnWriteFile(file, obj, monitor);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003EE0 File Offset: 0x000020E0
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

		// Token: 0x0400002E RID: 46
		private static HashSet<Type> userDataContentList;

		// Token: 0x0400002F RID: 47
		private static XmlDataSerializer dataSerializer;

		// Token: 0x04000030 RID: 48
		private static HashSet<string> supportedExtensions;

		// Token: 0x04000031 RID: 49
		private static HashSet<string> supportedRootNode;

		// Token: 0x04000032 RID: 50
		private static HashSet<Type> supportedType;

		// Token: 0x04000033 RID: 51
		private Solution currentSolution;
	}
}
