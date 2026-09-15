using System;
using System.Collections.Generic;
using CocoStudio.Basic;

namespace CocoStudio.Model.ExtensionModel
{
	public abstract class BaseModelScanner : IModelScanner
	{
		public virtual string Description
		{
			get
			{
				return null;
			}
		}

		public IEnumerable<ModelMetaData> GetModels()
		{
			IEnumerable<ModelMetaData> result;
			try
			{
				result = this.OnGetModels();
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("GetModels failed.", exception);
				result = null;
			}
			return result;
		}

		protected abstract IEnumerable<ModelMetaData> OnGetModels();
	}
}
