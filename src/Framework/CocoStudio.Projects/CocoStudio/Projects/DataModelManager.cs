using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using Mono.Addins;

namespace CocoStudio.Projects
{
	public class DataModelManager
	{
		public DataModelManager()
		{
			this.Initialize();
		}

		private void Initialize()
		{
			try
			{
				this.dataModelList = new HashSet<Type>();
				this.viewToDataCollection = new Dictionary<Type, Type>();
				this.dataToViewCollection = new Dictionary<Type, Type>();
				ExtensionNodeList<DataModelExtensionNode> extensionNodes = AddinManager.GetExtensionNodes<DataModelExtensionNode>(typeof(IDataModel));
				foreach (DataModelExtensionNode extensionNode in extensionNodes)
				{
					this.RegisteDataModel(extensionNode);
				}
				AddinManager.AddExtensionNodeHandler(typeof(IDataModel), new ExtensionNodeEventHandler(this.OnDataModelExtensionChange));
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Debug("Load data mode type failed", exception);
			}
		}

		private void OnDataModelExtensionChange(object sender, ExtensionNodeEventArgs args)
		{
			this.RegisteDataModel(args.ExtensionNode as DataModelExtensionNode);
		}

		private void RegisteDataModel(DataModelExtensionNode extensionNode)
		{
			if (extensionNode == null)
			{
				return;
			}
			if (this.dataModelList.Contains(extensionNode.Type))
			{
				return;
			}
			this.dataModelList.Add(extensionNode.Type);
			if (extensionNode.Data.ModelType != null)
			{
				if (!this.viewToDataCollection.ContainsKey(extensionNode.Data.ModelType))
				{
					this.viewToDataCollection.Add(extensionNode.Data.ModelType, extensionNode.Type);
				}
				if (!this.dataToViewCollection.ContainsKey(extensionNode.Type))
				{
					this.dataToViewCollection.Add(extensionNode.Type, extensionNode.Data.ModelType);
				}
			}
		}

		public IEnumerable<Type> GetDataModelCollection()
		{
			return this.dataModelList;
		}

		public Type GetDataModelType(Type viewModelType)
		{
			Type result;
			this.viewToDataCollection.TryGetValue(viewModelType, out result);
			return result;
		}

		public Type GetViewModelType(Type dataModelType)
		{
			Type result;
			this.dataToViewCollection.TryGetValue(dataModelType, out result);
			return result;
		}

		private HashSet<Type> dataModelList;

		private Dictionary<Type, Type> viewToDataCollection;

		private Dictionary<Type, Type> dataToViewCollection;
	}
}
