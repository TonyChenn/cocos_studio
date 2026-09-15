using System;
using CocoStudio.Core;
using CocoStudio.Model.DataModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Model
{
	[DataInclude(typeof(ResourceData))]
	[DataModelExtension]
	public class ResourceItemData : ResourceData, IDataModel, IDataConvert
	{
		private ResourceItemData()
		{
		}

		public ResourceItemData(string path) : base(path)
		{
		}

		public ResourceItemData(EnumResourceType type, string path) : base(type, path)
		{
		}

		public ResourceItemData(EnumResourceType type, string path, string plistFile) : base(type, path, plistFile)
		{
		}

		public object CreateViewModel()
		{
			ResourceItem resourceItem = Services.ProjectOperations.CurrentResourceGroup.FindResourceItem(this);
			if (base.Type == EnumResourceType.MarkedSubImage)
			{
				ImageFile imageFile = resourceItem as ImageFile;
				if (imageFile != null)
				{
					if (!imageFile.IsPacked())
					{
						base.Type = EnumResourceType.Normal;
					}
				}
			}
			return resourceItem;
		}

		public void SetData(object viewObject)
		{
			if (!(viewObject is ResourceItem))
			{
				throw new ArgumentException("Only support ResourceFile type.");
			}
			ResourceItem resourceItem = viewObject as ResourceItem;
			ResourceData resourceData = resourceItem.GetResourceData();
			this.Update(resourceData);
		}

		internal void Update(ResourceData data)
		{
			base.Type = data.Type;
			base.Path = data.Path;
			base.Plist = data.Plist;
			this.UpdateHashcode();
		}

		private void UpdateHashcode()
		{
			this.hashCode = 0;
			this.GetHashCode();
		}

		public static readonly ResourceItemData DefaultMarker = new ResourceItemData(string.Empty);
	}
}
