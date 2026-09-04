using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x0200012A RID: 298
	public class CompositeExtender : IDisposable
	{
		// Token: 0x06000B14 RID: 2836 RVA: 0x0002BA50 File Offset: 0x00029C50
		public CompositeExtender(BaseObject baseObject)
		{
			this.objectInstance = baseObject;
			this.objectInstance.PropertyChanged += this.OnObjectPropertyChanged;
			this.recorderList = new List<BaseExtender>();
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x0002BA88 File Offset: 0x00029C88
		~CompositeExtender()
		{
			this.Dispose();
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x0002BABC File Offset: 0x00029CBC
		public void Add(BaseExtender monitor)
		{
			this.recorderList.Add(monitor);
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x0002BACC File Offset: 0x00029CCC
		private void OnObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			PropertyInfo property = sender.GetType().GetProperty(e.PropertyName);
			foreach (BaseExtender baseExtender in this.recorderList)
			{
				baseExtender.OnObjectPropertyChanged(property);
			}
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x0002BB3C File Offset: 0x00029D3C
		public void Dispose()
		{
			foreach (BaseExtender baseExtender in this.recorderList)
			{
				baseExtender.Dispose();
			}
			if (this.objectInstance != null)
			{
				this.objectInstance.PropertyChanged -= this.OnObjectPropertyChanged;
				this.objectInstance = null;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x040004A3 RID: 1187
		private BaseObject objectInstance;

		// Token: 0x040004A4 RID: 1188
		private List<BaseExtender> recorderList;
	}
}
