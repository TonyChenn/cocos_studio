using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200003F RID: 63
	internal static class DataTypeCache
	{
		// Token: 0x06000270 RID: 624 RVA: 0x00007618 File Offset: 0x00005818
		public static PropertyAccessorHandler[] GetProperties(Type type)
		{
			PropertyAccessorHandler[] result;
			PropertyAccessorHandler[] array;
			if (type == null)
			{
				result = null;
			}
			else if (DataTypeCache.typeCollection.TryGetValue(type, out array))
			{
				result = array;
			}
			else
			{
				array = DataTypeCache.CreateProperties(type);
				DataTypeCache.typeCollection.Add(type, array);
				result = array;
			}
			return result;
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000766C File Offset: 0x0000586C
		public static IEnumerable<PropertyAccessorHandler> GetProperties(Type type, string propertyName)
		{
			PropertyAccessorHandler[] properties = DataTypeCache.GetProperties(type);
			IEnumerable<PropertyAccessorHandler> result;
			if (properties == null)
			{
				result = properties;
			}
			else
			{
				result = from a in properties.AsParallel<PropertyAccessorHandler>()
				where a.PropertyName == propertyName
				select a;
			}
			return result;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x000076B8 File Offset: 0x000058B8
		public static IEnumerable<PropertyAccessorHandler> GetProperties<T>(Type type)
		{
			PropertyAccessorHandler[] properties = DataTypeCache.GetProperties(type);
			IEnumerable<PropertyAccessorHandler> result;
			if (properties == null)
			{
				result = properties;
			}
			else
			{
				result = from a in properties.AsParallel<PropertyAccessorHandler>()
				where a.PropertyType.Equals(typeof(T))
				select a;
			}
			return result;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x000076F8 File Offset: 0x000058F8
		private static PropertyAccessorHandler[] CreateProperties(Type type)
		{
			PropertyInfo[] modelProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			DataTypeCache.DataModelProperty[] list = new DataTypeCache.DataModelProperty[modelProperties.Length];
			Parallel.For(0, modelProperties.Length, delegate(int i)
			{
				list[i] = new DataTypeCache.DataModelProperty(modelProperties[i]);
			});
			DataTypeCache.DataModelProperty[] sortList = (from a in list
			orderby a.Order
			select a).ToArray<DataTypeCache.DataModelProperty>();
			PropertyAccessorHandler[] properties = new PropertyAccessorHandler[modelProperties.Length];
			Parallel.For(0, properties.Length, delegate(int index)
			{
				properties[index] = new PropertyAccessorHandler(sortList[index].PropertyInfo);
			});
			return properties;
		}

		// Token: 0x04000105 RID: 261
		private static Dictionary<Type, PropertyAccessorHandler[]> typeCollection = new Dictionary<Type, PropertyAccessorHandler[]>();

		// Token: 0x02000040 RID: 64
		private class DataModelProperty
		{
			// Token: 0x170000E6 RID: 230
			// (get) Token: 0x06000276 RID: 630 RVA: 0x000077F8 File Offset: 0x000059F8
			// (set) Token: 0x06000277 RID: 631 RVA: 0x0000780F File Offset: 0x00005A0F
			public PropertyInfo PropertyInfo { get; private set; }

			// Token: 0x170000E7 RID: 231
			// (get) Token: 0x06000278 RID: 632 RVA: 0x00007818 File Offset: 0x00005A18
			// (set) Token: 0x06000279 RID: 633 RVA: 0x0000782F File Offset: 0x00005A2F
			public int Order { get; private set; }

			// Token: 0x0600027A RID: 634 RVA: 0x00007838 File Offset: 0x00005A38
			public DataModelProperty(PropertyInfo propertyInfo)
			{
				this.PropertyInfo = propertyInfo;
				this.SearchOrder();
			}

			// Token: 0x0600027B RID: 635 RVA: 0x00007854 File Offset: 0x00005A54
			private void SearchOrder()
			{
				object[] customAttributes = this.PropertyInfo.GetCustomAttributes(typeof(PropertyOrderAttribute), false);
				if (customAttributes.Length > 0)
				{
					this.Order = ((PropertyOrderAttribute)customAttributes[0]).Order;
				}
			}
		}
	}
}
