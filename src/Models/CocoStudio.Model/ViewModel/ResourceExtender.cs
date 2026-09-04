using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CocoStudio.Core;
using CocoStudio.Projects;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x0200012D RID: 301
	public class ResourceExtender : BaseExtender
	{
		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000B29 RID: 2857 RVA: 0x0002C020 File Offset: 0x0002A220
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

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000B2A RID: 2858 RVA: 0x0002C058 File Offset: 0x0002A258
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

		// Token: 0x06000B2B RID: 2859 RVA: 0x0002C08D File Offset: 0x0002A28D
		protected ResourceExtender(BaseObject bindingObject)
		{
			this.objectInstance = bindingObject;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0002C09F File Offset: 0x0002A29F
		public ResourceExtender(AbstractNodeObject bindingObject)
		{
			this.objectInstance = bindingObject;
			bindingObject.ParentChanged += this.OnObjectParentChanged;
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0002C0C4 File Offset: 0x0002A2C4
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

		// Token: 0x06000B2E RID: 2862 RVA: 0x0002C16C File Offset: 0x0002A36C
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

		// Token: 0x06000B2F RID: 2863 RVA: 0x0002C198 File Offset: 0x0002A398
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

		// Token: 0x06000B30 RID: 2864 RVA: 0x0002C254 File Offset: 0x0002A454
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

		// Token: 0x06000B31 RID: 2865 RVA: 0x0002C2E4 File Offset: 0x0002A4E4
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

		// Token: 0x06000B32 RID: 2866 RVA: 0x0002C334 File Offset: 0x0002A534
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

		// Token: 0x06000B33 RID: 2867 RVA: 0x0002C3A8 File Offset: 0x0002A5A8
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

		// Token: 0x06000B34 RID: 2868 RVA: 0x0002C440 File Offset: 0x0002A640
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

		// Token: 0x06000B35 RID: 2869 RVA: 0x0002C4D0 File Offset: 0x0002A6D0
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

		// Token: 0x06000B36 RID: 2870 RVA: 0x0002C548 File Offset: 0x0002A748
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

		// Token: 0x06000B37 RID: 2871 RVA: 0x0002C5B0 File Offset: 0x0002A7B0
		private void UnRegisterResourceChanged(string propertyName, ResourceFile resourceFile)
		{
			if (resourceFile != null && !resourceFile.IsDefault)
			{
				resourceFile.Deleted -= this.OnResourceFileDeleted;
				resourceFile.ContentChanged -= this.OnResourceContentChanged;
			}
			this.ResourceCollection[propertyName] = null;
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0002C608 File Offset: 0x0002A808
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

		// Token: 0x06000B39 RID: 2873 RVA: 0x0002C6C8 File Offset: 0x0002A8C8
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

		// Token: 0x040004A9 RID: 1193
		protected BaseObject objectInstance;

		// Token: 0x040004AA RID: 1194
		protected bool isSettingValue;

		// Token: 0x040004AB RID: 1195
		protected bool isContentChanged;

		// Token: 0x040004AC RID: 1196
		private Dictionary<string, ResourceFile> resourceCollection;

		// Token: 0x040004AD RID: 1197
		private HashSet<string> ignoreResourcePropertySet;
	}
}
