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
	[DataInclude(typeof(PointF))]
	[DataModelExtension(typeof(BaseObject))]
	[JsonObject(MemberSerialization.OptIn)]
	[DataInclude(typeof(SizeValue))]
	[DataInclude(typeof(ScaleValue))]
	public class BaseObjectData : IExtendedDataItem, IDataModel
	{
		[JsonProperty]
		[ItemProperty]
		public string Name { get; set; }

		[JsonProperty]
		protected internal virtual string ctype { get; set; }

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

		public BaseObjectData()
		{
			this.ctype = base.GetType().Name;
		}

		internal PropertyAccessorHandler[] GetProperties()
		{
			return DataTypeCache.GetProperties(base.GetType());
		}

		internal IEnumerable<PropertyAccessorHandler> GetResourceProperties()
		{
			return DataTypeCache.GetProperties<ResourceItemData>(base.GetType());
		}

		private Hashtable hashtable;
	}
}
