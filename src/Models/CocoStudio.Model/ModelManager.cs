using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using CocoStudio.Model.ExtensionModel;
using CocoStudio.Model.ViewModel;
using Mono.Addins;

namespace CocoStudio.Model
{
	public class ModelManager
	{
		public IEnumerable<ModelMetaData> ModelCollection { get; private set; }

		public static ModelManager Instance { get; private set; } = new ModelManager();

		private ModelManager()
		{
			this.Initialize();
		}

		private void Initialize()
		{
			IModelScanner[] scannerArray = this.LoadModelScanner();
			this.LoadModelType(scannerArray);
		}

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

		internal ModelMetaData GetMetaData(AbstractNodeObject nodeObject)
		{
			return this.GetMetaData(nodeObject.GetType(), nodeObject.ScriptData);
		}

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
