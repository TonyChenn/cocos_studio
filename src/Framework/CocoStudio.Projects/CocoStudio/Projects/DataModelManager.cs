using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using Mono.Addins;

namespace CocoStudio.Projects
{
	// Token: 0x02000025 RID: 37
	public class DataModelManager
	{
		// Token: 0x060000C9 RID: 201 RVA: 0x00004170 File Offset: 0x00002370
		public DataModelManager()
		{
			this.Initialize();
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004180 File Offset: 0x00002380
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

		// Token: 0x060000CB RID: 203 RVA: 0x00004238 File Offset: 0x00002438
		private void OnDataModelExtensionChange(object sender, ExtensionNodeEventArgs args)
		{
			this.RegisteDataModel(args.ExtensionNode as DataModelExtensionNode);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000424C File Offset: 0x0000244C
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

		// Token: 0x060000CD RID: 205 RVA: 0x000042F9 File Offset: 0x000024F9
		public IEnumerable<Type> GetDataModelCollection()
		{
			return this.dataModelList;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004304 File Offset: 0x00002504
		public Type GetDataModelType(Type viewModelType)
		{
			Type result;
			this.viewToDataCollection.TryGetValue(viewModelType, out result);
			return result;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004324 File Offset: 0x00002524
		public Type GetViewModelType(Type dataModelType)
		{
			Type result;
			this.dataToViewCollection.TryGetValue(dataModelType, out result);
			return result;
		}

		// Token: 0x04000038 RID: 56
		private HashSet<Type> dataModelList;

		// Token: 0x04000039 RID: 57
		private Dictionary<Type, Type> viewToDataCollection;

		// Token: 0x0400003A RID: 58
		private Dictionary<Type, Type> dataToViewCollection;
	}
}
