using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Addins;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000016 RID: 22
	internal class AssetModelFactory
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00004F4E File Offset: 0x0000314E
		public static AssetModelFactory Instance
		{
			get
			{
				if (AssetModelFactory.instance == null)
				{
					AssetModelFactory.instance = new AssetModelFactory();
				}
				return AssetModelFactory.instance;
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004F66 File Offset: 0x00003166
		private AssetModelFactory()
		{
			this.Initialize();
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004FC0 File Offset: 0x000031C0
		private void Initialize()
		{
			BaseAssetModel[] extensionObjects = AddinManager.GetExtensionObjects<BaseAssetModel>();
			List<BaseAssetModel> collection = extensionObjects.OrderBy(delegate(BaseAssetModel a)
			{
				Type type = a.GetType();
				object[] customAttributes = type.GetCustomAttributes(typeof(AssetOrderAttribute), false);
				AssetOrderAttribute assetOrderAttribute = customAttributes.FirstOrDefault<object>() as AssetOrderAttribute;
				int result = 0;
				if (assetOrderAttribute != null)
				{
					result = assetOrderAttribute.Order;
				}
				return result;
			}).ToList<BaseAssetModel>();
			this.AllTypeList.AddRange(collection);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005008 File Offset: 0x00003208
		public BaseAssetModel GetAssetModel(Plugin info)
		{
			BaseAssetModel result = null;
			try
			{
				foreach (BaseAssetModel baseAssetModel in this.AllTypeList)
				{
					if (baseAssetModel.CanHandle(info))
					{
						result = (baseAssetModel.Clone(info) as BaseAssetModel);
						return result;
					}
					result = null;
				}
			}
			catch (Exception)
			{
			}
			return result;
		}

		// Token: 0x0400004D RID: 77
		private List<BaseAssetModel> AllTypeList = new List<BaseAssetModel>();

		// Token: 0x0400004E RID: 78
		private static AssetModelFactory instance;
	}
}
