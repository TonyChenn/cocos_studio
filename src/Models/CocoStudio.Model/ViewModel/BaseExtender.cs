using System;
using System.Reflection;

namespace CocoStudio.Model.ViewModel
{
	public abstract class BaseExtender : IDisposable
	{
		~BaseExtender()
		{
			this.Dispose();
		}

		internal virtual void OnObjectPropertyChanged(PropertyInfo propertyInfo)
		{
		}

		public virtual void Dispose()
		{
		}
	}
}
