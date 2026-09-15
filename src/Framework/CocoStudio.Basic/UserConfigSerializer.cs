using System;
using System.Collections.Generic;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Basic
{
	internal class UserConfigSerializer
	{
		static UserConfigSerializer()
		{
			UserConfigSerializer.CollectIUserConfigType();
		}

		private static void CollectIUserConfigType()
		{
			UserConfigSerializer.userConfigTypeList = new HashSet<Type>();
			ExtensionNodeList extensionNodes = AddinManager.GetExtensionNodes(typeof(IUserConfig));
			foreach (object obj in extensionNodes)
			{
				TypeExtensionNode typeExtensionNode = (TypeExtensionNode)obj;
				UserConfigSerializer.userConfigTypeList.Add(typeExtensionNode.Type);
			}
			AddinManager.AddExtensionNodeHandler(typeof(IUserConfig), new ExtensionNodeEventHandler(UserConfigSerializer.OnUserConfigModelExtensionChange));
		}

		private static void OnUserConfigModelExtensionChange(object sender, ExtensionNodeEventArgs args)
		{
			TypeExtensionNode typeExtensionNode = args.ExtensionNode as TypeExtensionNode;
			if (!UserConfigSerializer.userConfigTypeList.Contains(typeExtensionNode.Type))
			{
				UserConfigSerializer.userConfigTypeList.Add(typeExtensionNode.Type);
			}
		}

		public static UserConfigService ReadFile(FilePath filePath)
		{
			XmlDataSerializer serializer = UserConfigSerializer.GetSerializer(filePath);
			UserConfigService result;
			if (serializer == null)
			{
				result = null;
			}
			else
			{
				object obj = serializer.Deserialize(filePath, typeof(UserConfigService));
				UserConfigService userConfigService = obj as UserConfigService;
				if (userConfigService == null && obj != null)
				{
					LogConfig.Logger.Error(string.Format("读取UserConfig时出错，无法将{0}转换成对应的对象", obj));
				}
				result = userConfigService;
			}
			return result;
		}

		public static void WriteFile(FilePath filePath, UserConfigService configData)
		{
			XmlDataSerializer serializer = UserConfigSerializer.GetSerializer(filePath);
			if (serializer != null)
			{
				serializer.Serialize(filePath, configData);
			}
		}

		private static XmlDataSerializer GetSerializer(FilePath filePath)
		{
			XmlDataSerializer result;
			if (UserConfigSerializer.configSerializer != null)
			{
				result = UserConfigSerializer.configSerializer;
			}
			else
			{
				DataContext ctx = new DataContext();
				XmlDataSerializer xmlDataSerializer = new XmlDataSerializer(ctx);
				xmlDataSerializer.SerializationContext.DirectorySeparatorChar = '/';
				foreach (Type type in UserConfigSerializer.userConfigTypeList)
				{
					xmlDataSerializer.SerializationContext.Serializer.DataContext.IncludeType(type);
				}
				UserConfigSerializer.configSerializer = xmlDataSerializer;
				UserConfigSerializer.userConfigTypeList = null;
				result = UserConfigSerializer.configSerializer;
			}
			return result;
		}

		private static HashSet<Type> userConfigTypeList;

		private static XmlDataSerializer configSerializer;
	}
}
