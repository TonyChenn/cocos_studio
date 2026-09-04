using System;
using System.Collections;
using System.Collections.Generic;
using CocoStudio.Model.Editor;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000006 RID: 6
	[DataInclude(typeof(PointF))]
	[DataModelExtension(typeof(BaseObject))]
	[JsonObject(MemberSerialization.OptIn)]
	[DataInclude(typeof(SizeValue))]
	[DataInclude(typeof(ScaleValue))]
	public class BaseObjectData : IExtendedDataItem, IDataModel
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000023A0 File Offset: 0x000005A0
		// (set) Token: 0x0600001E RID: 30 RVA: 0x000023B7 File Offset: 0x000005B7
		[JsonProperty]
		[ItemProperty]
		public string Name { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000023C0 File Offset: 0x000005C0
		// (set) Token: 0x06000020 RID: 32 RVA: 0x000023D7 File Offset: 0x000005D7
		[JsonProperty]
		protected internal virtual string ctype { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000023E0 File Offset: 0x000005E0
		public IDictionary ExtendedProperties
		{
			get
			{
				if (this.hashtable == null)
				{
					this.hashtable = new Hashtable();
				}
				return this.hashtable;
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002415 File Offset: 0x00000615
		public BaseObjectData()
		{
			this.ctype = base.GetType().Name;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002434 File Offset: 0x00000634
		internal PropertyAccessorHandler[] GetProperties()
		{
			return DataTypeCache.GetProperties(base.GetType());
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002454 File Offset: 0x00000654
		internal IEnumerable<PropertyAccessorHandler> GetResourceProperties()
		{
			return DataTypeCache.GetProperties<ResourceItemData>(base.GetType());
		}

		// Token: 0x04000007 RID: 7
		private Hashtable hashtable;
	}
}
