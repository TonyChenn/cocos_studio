using System;
using System.Reflection;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x0200012B RID: 299
	public abstract class BaseExtender : IDisposable
	{
		// Token: 0x06000B1A RID: 2842 RVA: 0x0002BBD8 File Offset: 0x00029DD8
		~BaseExtender()
		{
			this.Dispose();
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x0002BC0C File Offset: 0x00029E0C
		internal virtual void OnObjectPropertyChanged(PropertyInfo propertyInfo)
		{
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0002BC0F File Offset: 0x00029E0F
		public virtual void Dispose()
		{
		}
	}
}
