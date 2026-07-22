using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x0200007A RID: 122
	public abstract class NotificationObject : INotifyPropertyChanged
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000460 RID: 1120 RVA: 0x00018604 File Offset: 0x00016804
		// (remove) Token: 0x06000461 RID: 1121 RVA: 0x00018640 File Offset: 0x00016840
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06000462 RID: 1122 RVA: 0x0001867C File Offset: 0x0001687C
		protected void RaisePropertyChanged(PropertyInfo propertyInfo)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyInfo.Name));
			}
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x000186B0 File Offset: 0x000168B0
		protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
		{
			PropertyInfo propertyInfo = PropertySupport.ExtractPropertyInfo<T>(propertyExpression);
			this.RaisePropertyChanged(propertyInfo);
		}
	}
}
