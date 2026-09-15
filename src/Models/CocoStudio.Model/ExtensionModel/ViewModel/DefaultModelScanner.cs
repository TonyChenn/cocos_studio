using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using CocoStudio.Model.ViewModel;
using Mono.Addins;

namespace CocoStudio.Model.ExtensionModel.ViewModel
{
	[Extension(typeof(IModelScanner))]
	internal class DefaultModelScanner : BaseModelScanner
	{
		public DefaultModelScanner()
		{
			this.LoadDefaultModels();
		}

		private void LoadDefaultModels()
		{
			try
			{
				this.modelCollection = new List<ModelMetaData>();
				ExtensionNodeList<ModelExtensionNode> extensionNodes = AddinManager.GetExtensionNodes<ModelExtensionNode>(typeof(AbstractNodeObject));
				foreach (ModelExtensionNode extensionNode in extensionNodes)
				{
					ModelMetaData item = new ModelMetaData(extensionNode);
					this.modelCollection.Add(item);
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("LoadDefaultModels failed.", exception);
			}
		}

		protected override IEnumerable<ModelMetaData> OnGetModels()
		{
			return this.modelCollection;
		}

		private List<ModelMetaData> modelCollection;
	}
}
