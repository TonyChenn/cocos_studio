using System;
using CocoStudio.Core;
using CocoStudio.Model.DataModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Model
{
	// Token: 0x020000C8 RID: 200
	[DataInclude(typeof(ResourceData))]
	[DataModelExtension]
	public class ResourceItemData : ResourceData, IDataModel, IDataConvert
	{
		// Token: 0x0600064D RID: 1613 RVA: 0x00019B93 File Offset: 0x00017D93
		private ResourceItemData()
		{
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00019B9E File Offset: 0x00017D9E
		public ResourceItemData(string path) : base(path)
		{
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00019BAA File Offset: 0x00017DAA
		public ResourceItemData(EnumResourceType type, string path) : base(type, path)
		{
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00019BB7 File Offset: 0x00017DB7
		public ResourceItemData(EnumResourceType type, string path, string plistFile) : base(type, path, plistFile)
		{
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00019BC8 File Offset: 0x00017DC8
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

		// Token: 0x06000652 RID: 1618 RVA: 0x00019C24 File Offset: 0x00017E24
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

		// Token: 0x06000653 RID: 1619 RVA: 0x00019C60 File Offset: 0x00017E60
		internal void Update(ResourceData data)
		{
			base.Type = data.Type;
			base.Path = data.Path;
			base.Plist = data.Plist;
			this.UpdateHashcode();
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00019C91 File Offset: 0x00017E91
		private void UpdateHashcode()
		{
			this.hashCode = 0;
			this.GetHashCode();
		}

		// Token: 0x040002C0 RID: 704
		public static readonly ResourceItemData DefaultMarker = new ResourceItemData(string.Empty);
	}
}
