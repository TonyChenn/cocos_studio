using System;
using System.Collections.Generic;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Basic
{
	// Token: 0x0200000E RID: 14
	internal class UserConfigSerializer
	{
		// Token: 0x06000083 RID: 131 RVA: 0x0000397E File Offset: 0x00001B7E
		static UserConfigSerializer()
		{
			UserConfigSerializer.CollectIUserConfigType();
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003988 File Offset: 0x00001B88
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

		// Token: 0x06000085 RID: 133 RVA: 0x00003A2C File Offset: 0x00001C2C
		private static void OnUserConfigModelExtensionChange(object sender, ExtensionNodeEventArgs args)
		{
			TypeExtensionNode typeExtensionNode = args.ExtensionNode as TypeExtensionNode;
			if (!UserConfigSerializer.userConfigTypeList.Contains(typeExtensionNode.Type))
			{
				UserConfigSerializer.userConfigTypeList.Add(typeExtensionNode.Type);
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003A70 File Offset: 0x00001C70
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

		// Token: 0x06000087 RID: 135 RVA: 0x00003AE4 File Offset: 0x00001CE4
		public static void WriteFile(FilePath filePath, UserConfigService configData)
		{
			XmlDataSerializer serializer = UserConfigSerializer.GetSerializer(filePath);
			if (serializer != null)
			{
				serializer.Serialize(filePath, configData);
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003B14 File Offset: 0x00001D14
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

		// Token: 0x04000056 RID: 86
		private static HashSet<Type> userConfigTypeList;

		// Token: 0x04000057 RID: 87
		private static XmlDataSerializer configSerializer;
	}
}
