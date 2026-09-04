using System;
using System.Collections.Generic;
using CocoStudio.Basic;

namespace CocoStudio.Model.ExtensionModel
{
	// Token: 0x02000082 RID: 130
	public abstract class BaseModelScanner : IModelScanner
	{
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x00013CBC File Offset: 0x00011EBC
		public virtual string Description
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00013CD0 File Offset: 0x00011ED0
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

		// Token: 0x06000487 RID: 1159
		protected abstract IEnumerable<ModelMetaData> OnGetModels();
	}
}
