using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.DataModel
{
	internal static class DataTypeCache
	{
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

		private static Dictionary<Type, PropertyAccessorHandler[]> typeCollection = new Dictionary<Type, PropertyAccessorHandler[]>();

		private class DataModelProperty
		{
			public PropertyInfo PropertyInfo { get; private set; }

			public int Order { get; private set; }

			public DataModelProperty(PropertyInfo propertyInfo)
			{
				this.PropertyInfo = propertyInfo;
				this.SearchOrder();
			}

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
