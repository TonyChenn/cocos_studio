using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace EditorCommon.JsonModel.JsonManager
{
	// Token: 0x02000031 RID: 49
	public class JsonModelManager
	{
		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000366 RID: 870 RVA: 0x00008B7A File Offset: 0x00006D7A
		// (set) Token: 0x06000367 RID: 871 RVA: 0x00008B81 File Offset: 0x00006D81
		public static List<Type> ModelTypeList { get; private set; } = JsonModelManager.CreateModelTypeList();

		// Token: 0x06000368 RID: 872 RVA: 0x00008B8C File Offset: 0x00006D8C
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
