using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using CocoStudio.Model.ExtensionModel;
using CocoStudio.Model.ViewModel;
using Mono.Addins;

namespace CocoStudio.Model
{
	// Token: 0x02000135 RID: 309
	public class ModelManager
	{
		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000B6D RID: 2925 RVA: 0x0002D30C File Offset: 0x0002B50C
		// (set) Token: 0x06000B6E RID: 2926 RVA: 0x0002D323 File Offset: 0x0002B523
		public IEnumerable<ModelMetaData> ModelCollection { get; private set; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000B6F RID: 2927 RVA: 0x0002D32C File Offset: 0x0002B52C
		// (set) Token: 0x06000B70 RID: 2928 RVA: 0x0002D342 File Offset: 0x0002B542
		public static ModelManager Instance { get; private set; } = new ModelManager();

		// Token: 0x06000B72 RID: 2930 RVA: 0x0002D358 File Offset: 0x0002B558
		private ModelManager()
		{
			this.Initialize();
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0002D36C File Offset: 0x0002B56C
		private void Initialize()
		{
			IModelScanner[] scannerArray = this.LoadModelScanner();
			this.LoadModelType(scannerArray);
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0002D38C File Offset: 0x0002B58C
		private void LoadModelType(IModelScanner[] scannerArray)
		{
			if (scannerArray != null)
			{
				List<ModelMetaData> list = new List<ModelMetaData>(40);
				foreach (IModelScanner modelScanner in scannerArray)
				{
					IEnumerable<ModelMetaData> models = modelScanner.GetModels();
					if (models != null)
					{
						list.AddRange(models);
					}
				}
				list.TrimExcess();
				this.ModelCollection = list;
			}
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0002D400 File Offset: 0x0002B600
		private IModelScanner[] LoadModelScanner()
		{
			IModelScanner[] result;
			try
			{
				IModelScanner[] extensionObjects = AddinManager.GetExtensionObjects<IModelScanner>();
				result = extensionObjects;
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Load model scanner failed.", exception);
				result = null;
			}
			return result;
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0002D444 File Offset: 0x0002B644
		internal ModelMetaData GetMetaData(AbstractNodeObject nodeObject)
		{
			return this.GetMetaData(nodeObject.GetType(), nodeObject.ScriptData);
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x0002D468 File Offset: 0x0002B668
		internal ModelMetaData GetMetaData(Type nodeType, ScriptFileData scriptData)
		{
			foreach (ModelMetaData modelMetaData in this.ModelCollection)
			{
				if (nodeType == modelMetaData.Type)
				{
					if (scriptData == null)
					{
						if (modelMetaData.ScriptData == null)
						{
							return modelMetaData;
						}
					}
					else if (scriptData.IsEqual(modelMetaData.ScriptData))
					{
						return modelMetaData;
					}
				}
			}
			return null;
		}
	}
}
