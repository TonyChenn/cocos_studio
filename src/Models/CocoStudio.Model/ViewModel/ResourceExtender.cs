using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CocoStudio.Core;
using CocoStudio.Projects;

namespace CocoStudio.Model.ViewModel
{
	public class ResourceExtender : BaseExtender
	{
		private Dictionary<string, ResourceFile> ResourceCollection
		{
			get
			{
				if (this.resourceCollection == null)
				{
					this.resourceCollection = new Dictionary<string, ResourceFile>();
				}
				return this.resourceCollection;
			}
		}

		private HashSet<string> IgnoreResourcePropertySet
		{
			get
			{
				if (this.ignoreResourcePropertySet == null)
				{
					this.ignoreResourcePropertySet = new HashSet<string>();
				}
				return this.ignoreResourcePropertySet;
			}
		}

		protected ResourceExtender(BaseObject bindingObject)
		{
			this.objectInstance = bindingObject;
		}

		public ResourceExtender(AbstractNodeObject bindingObject)
		{
			this.objectInstance = bindingObject;
			bindingObject.ParentChanged += this.OnObjectParentChanged;
		}

		internal override void OnObjectPropertyChanged(PropertyInfo propertyInfo)
		{
			if (this.ResourceCollection != null && this.ResourceCollection.ContainsKey(propertyInfo.Name))
			{
				bool isUndoing = Services.TaskService.IsUndoing;
				if (this.objectInstance.IsRaisePropertyChanged && !this.isSettingValue)
				{
					this.RefreshRegistedResourceChangedEvent(propertyInfo, isUndoing);
				}
				if (!this.isContentChanged && !isUndoing)
				{
					if (this.ignoreResourcePropertySet == null || !this.ignoreResourcePropertySet.Contains(propertyInfo.Name))
					{
						this.objectInstance.OnResourcePropertyChanged();
					}
				}
			}
		}

		private void OnObjectParentChanged(object sender, EventArgs e)
		{
			if (e == null)
			{
				this.ClearResources();
			}
			else
			{
				this.CollectResources();
			}
		}

		protected void CollectResources()
		{
			PropertyInfo[] properties = this.objectInstance.GetType().GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (this.CheckIsResource(propertyInfo) && !this.ResourceCollection.ContainsKey(propertyInfo.Name))
				{
					this.RefreshRegistedResourceChangedEvent(propertyInfo, true);
					object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(IgnoreResizeAttribute), false);
					if (customAttributes != null && customAttributes.Length > 0)
					{
						this.IgnoreResourcePropertySet.Add(propertyInfo.Name);
					}
				}
			}
			this.IgnoreResourcePropertySet.TrimExcess();
		}

		protected void ClearResources()
		{
			if (this.ResourceCollection != null)
			{
				List<KeyValuePair<string, ResourceFile>> list = this.ResourceCollection.ToList<KeyValuePair<string, ResourceFile>>();
				foreach (KeyValuePair<string, ResourceFile> keyValuePair in list)
				{
					this.UnRegisterResourceChanged(keyValuePair.Key, keyValuePair.Value);
				}
				this.ResourceCollection.Clear();
			}
		}

		private bool CheckIsResource(PropertyInfo propertyInfo)
		{
			bool result;
			if (typeof(ResourceFile).IsAssignableFrom(propertyInfo.PropertyType))
			{
				IEnumerable<Attribute> customAttributes = propertyInfo.GetCustomAttributes(typeof(ResourceIgnoreAttribute));
				result = (customAttributes.Count<Attribute>() == 0);
			}
			else
			{
				result = false;
			}
			return result;
		}

		private void SetResource(PropertyInfo info, ResourceFile file)
		{
			this.isSettingValue = true;
			if (this.objectInstance.Recorder != null)
			{
				this.objectInstance.Recorder.IsAutoRecord = false;
				info.SetValue(this.objectInstance, file, null);
				this.objectInstance.Recorder.IsAutoRecord = true;
			}
			else
			{
				info.SetValue(this.objectInstance, file, null);
			}
			this.isSettingValue = false;
		}

		private List<PropertyInfo> FindPropertyInfo(ResourceFile file)
		{
			List<PropertyInfo> list = new List<PropertyInfo>();
			foreach (KeyValuePair<string, ResourceFile> keyValuePair in this.ResourceCollection)
			{
				if (keyValuePair.Value == file)
				{
					PropertyInfo property = this.objectInstance.GetType().GetProperty(keyValuePair.Key);
					list.Add(property);
				}
			}
			return list;
		}

		private void OnResourceFileDeleted(object sender, EventArgs e)
		{
			this.isContentChanged = true;
			ResourceFile file = sender as ResourceFile;
			List<PropertyInfo> list = this.FindPropertyInfo(file);
			foreach (PropertyInfo propertyInfo in list)
			{
				this.UnRegisterResourceChanged(propertyInfo.Name, sender as ResourceFile);
				this.SetResource(propertyInfo, ResourceFile.DefaultMarker);
			}
			this.isContentChanged = false;
		}

		private void OnResourceContentChanged(object sender, EventArgs e)
		{
			this.isContentChanged = true;
			ResourceFile file = sender as ResourceFile;
			List<PropertyInfo> list = this.FindPropertyInfo(file);
			foreach (PropertyInfo info in list)
			{
				this.SetResource(info, file);
			}
			this.isContentChanged = false;
		}

		private void RegisterResourceChanged(string propertyName, ResourceFile resourceFile)
		{
			if (resourceFile != null && !resourceFile.IsDefault)
			{
				if (!this.ResourceCollection.ContainsValue(resourceFile))
				{
					resourceFile.Deleted += this.OnResourceFileDeleted;
					resourceFile.ContentChanged += this.OnResourceContentChanged;
				}
			}
			this.ResourceCollection[propertyName] = resourceFile;
		}

		private void UnRegisterResourceChanged(string propertyName, ResourceFile resourceFile)
		{
			if (resourceFile != null && !resourceFile.IsDefault)
			{
				resourceFile.Deleted -= this.OnResourceFileDeleted;
				resourceFile.ContentChanged -= this.OnResourceContentChanged;
			}
			this.ResourceCollection[propertyName] = null;
		}

		private void RefreshRegistedResourceChangedEvent(PropertyInfo propertyInfo, bool isCheckResource = false)
		{
			ResourceFile resourceFile;
			bool flag = this.ResourceCollection.TryGetValue(propertyInfo.Name, out resourceFile);
			if (flag)
			{
				List<PropertyInfo> list = this.FindPropertyInfo(resourceFile);
				if (list.Count > 1)
				{
					this.ResourceCollection[propertyInfo.Name] = null;
				}
				else
				{
					this.UnRegisterResourceChanged(propertyInfo.Name, resourceFile);
				}
			}
			resourceFile = (propertyInfo.GetValue(this.objectInstance, null) as ResourceFile);
			this.RegisterResourceChanged(propertyInfo.Name, resourceFile);
			if (isCheckResource)
			{
				if (resourceFile != null && !resourceFile.IsDefault && resourceFile.Parent == null)
				{
					this.SetResource(propertyInfo, ResourceFile.DefaultMarker);
				}
			}
		}

		public override void Dispose()
		{
			AbstractNodeObject abstractNodeObject = this.objectInstance as AbstractNodeObject;
			if (abstractNodeObject != null)
			{
				abstractNodeObject.ParentChanged -= this.OnObjectParentChanged;
			}
			this.ClearResources();
			GC.SuppressFinalize(this);
		}

		protected BaseObject objectInstance;

		protected bool isSettingValue;

		protected bool isContentChanged;

		private Dictionary<string, ResourceFile> resourceCollection;

		private HashSet<string> ignoreResourcePropertySet;
	}
}
