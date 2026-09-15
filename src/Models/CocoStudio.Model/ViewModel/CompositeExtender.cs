using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace CocoStudio.Model.ViewModel
{
	public class CompositeExtender : IDisposable
	{
		public CompositeExtender(BaseObject baseObject)
		{
			this.objectInstance = baseObject;
			this.objectInstance.PropertyChanged += this.OnObjectPropertyChanged;
			this.recorderList = new List<BaseExtender>();
		}

		~CompositeExtender()
		{
			this.Dispose();
		}

		public void Add(BaseExtender monitor)
		{
			this.recorderList.Add(monitor);
		}

		private void OnObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			PropertyInfo property = sender.GetType().GetProperty(e.PropertyName);
			foreach (BaseExtender baseExtender in this.recorderList)
			{
				baseExtender.OnObjectPropertyChanged(property);
			}
		}

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

		private BaseObject objectInstance;

		private List<BaseExtender> recorderList;
	}
}
