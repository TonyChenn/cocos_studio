using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using CocoStudio.Model.ViewModel;
using Mono.Addins;

namespace CocoStudio.Model.ExtensionModel.ViewModel
{
	// Token: 0x02000083 RID: 131
	[Extension(typeof(IModelScanner))]
	internal class DefaultModelScanner : BaseModelScanner
	{
		// Token: 0x06000489 RID: 1161 RVA: 0x00013D1C File Offset: 0x00011F1C
		public DefaultModelScanner()
		{
			this.LoadDefaultModels();
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00013D30 File Offset: 0x00011F30
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

		// Token: 0x0600048B RID: 1163 RVA: 0x00013DE0 File Offset: 0x00011FE0
		protected override IEnumerable<ModelMetaData> OnGetModels()
		{
			return this.modelCollection;
		}

		// Token: 0x04000226 RID: 550
		private List<ModelMetaData> modelCollection;
	}
}
