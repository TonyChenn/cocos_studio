using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace EditorCommon.JsonModel.JsonManager
{
	public class JsonModelManager
	{
		public static List<Type> ModelTypeList { get; private set; } = JsonModelManager.CreateModelTypeList();

		private static List<Type> CreateModelTypeList()
		{
			List<Type> result;
			try
			{
				ExtensionNodeList<TypeExtensionNode> extensionNodes = AddinManager.GetExtensionNodes<TypeExtensionNode>(typeof(IJsonModel));
				List<Type> list = new List<Type>();
				foreach (TypeExtensionNode typeExtensionNode in extensionNodes)
				{
					list.Add(typeExtensionNode.Type);
				}
				result = list;
			}
			catch (Exception exception)
			{
				LogConfig.Output.Error(LanguageInfo.InitJsonFail, exception);
				result = null;
			}
			return result;
		}
	}
}
