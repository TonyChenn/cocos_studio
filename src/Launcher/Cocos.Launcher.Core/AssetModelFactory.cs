using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Addins;

namespace Cocos.Launcher.Core
{
	internal class AssetModelFactory
	{
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

		private AssetModelFactory()
		{
			this.Initialize();
		}

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

		private List<BaseAssetModel> AllTypeList = new List<BaseAssetModel>();

		private static AssetModelFactory instance;
	}
}
